IF OBJECT_ID('[sp_Rel_Resumo_Vendas_hora_media]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Resumo_Vendas_hora_media]
end 

go

create Procedure [dbo].[sp_Rel_Resumo_Vendas_hora_media]
            @Filial           As Varchar(20),
            @DataDe           As Varchar(8),
			@DataAte          As Varchar(8),
			@ini_periodo	  as varchar(5),
			@fim_periodo	  as varchar(5),
            @plu			  as Varchar (20),
            @descricao		  as varchar(50),
            @grupo			  as Varchar(50),
            @subGrupo		  as varchar(50),
            @departamento	  as varchar(50),
            @relatorio		  as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas_hora_media 'MATRIZ','20180601','20180630','00:00','23:59','','','','','','TODOS'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	hora varchar(5),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)

	declare @dias int 

	Select @dias = DATEDIFF(day,@DataDe,@DataAte)+1
	print(convert(varchar,@dias))

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT      saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ,
			hora_venda
		  INTO
				#Lixo
      FROM
            Saida_Estoque with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
      WHERE
	          saida_estoque.Filial = @Filial
			  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
			  and   hora_venda between @ini_periodo+':00' and @fim_periodo+':59'
			  AND   Data_Movimento  between @DataDe and @DataAte
			  AND   Data_Cancelamento IS NULL
			  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
			  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
			  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
      
      GROUP BY
            Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

PRINT('#TEMPVENDAS')
insert into #tmpVendas
 SELECT
	  substring(Hora_venda,1,2),
      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) ,
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2)  ) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) /  (SELECT COUNT(*) FROM #Lixo WHERE  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> ''),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> ''),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2) AND #lixo.CPF_CNPJ <> '') > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> '') /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(saida_estoque.Hora_venda,1,2)  )) * 100)

                  ELSE 0 END
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

    

		  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
		  AND (Data_Movimento between @DataDe and @DataAte)
		  AND (hora_venda between @ini_periodo+':00' and @fim_periodo+':59')
		  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
		  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
		  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	   
      GROUP BY    Saida_Estoque.Data_movimento , substring(Hora_venda,1,2)
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
		substring(N.emissao_hora,1,2)  as horas,
		SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) AS Venda,
      
		(
		  	Select COUNT(*) 
		from nf 
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
				AND  (NF.Emissao = n.Emissao )
				and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
				AND   NF.TIPO_NF = 1 
				AND ISNULL(NF.nf_Canc,0)=0	
				and nf.status='AUTORIZADO'
				AND (NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											

											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nii.TOTAL-(isnull(nii.Total,0)*isnull(nii.desconto,0)/100)),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
							 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							 and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
							 AND (NF.Emissao= n.Emissao )
							 and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
							 AND NF.TIPO_NF = 1
							 and nf.status='AUTORIZADO'
							 AND (LEN(@PLU)=0 OR nii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf 
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								and  nf.status='AUTORIZADO'
								and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
								
								AND (
						NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as nii  inner join mercadoria as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao between @DataDe and @DataAte )
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 and  (n.emissao_hora between @ini_periodo+':00' and @fim_periodo+':59')
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY substring(N.emissao_hora,1,2),n.emissao
END


--if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
--BEGIN
--insert into #tmpVendas
--select 
--	CONVERT(VARCHAR,p.Data_cadastro,103) AS DATA ,
--	  Dia_Semana = CASE
--            WHEN DATEPART(dw, p.Data_cadastro) = 1 THEN 'DOMINGO'
--            WHEN DATEPART(dw, p.Data_cadastro) = 2 THEN 'SEGUNDA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 3 THEN 'TERÇA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 4 THEN 'QUARTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 5 THEN 'QUINTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 6 THEN 'SEXTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 7 THEN 'SABADO'

--      END,
--      SUM(pit.TOTAL) AS Venda,
      
--      (
--		  	Select COUNT(*) 
--		from pedido  with (index(ix_pedido_fluxo_caixa))
--		where pedido.pedido_simples = 1  
--				AND  (pedido.Data_cadastro= p.Data_cadastro )
--				and isnull(pedido.Status,0)<>3
--				AND   pedido.Tipo = 1 
--				and pedido.FILIAL=@filial 
--				AND (pedido.pedido IN (SELECT DISTINCT pedido 
--											FROM pedido_Itens as	nii										
--											inner join mercadoria  as mi on mi.PLU = nii.PLU
--											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
--											WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
--													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
--										)
--							)
--      ) 
--      as Clientes ,
--      (
--      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM pedido WHERE pedido.Data_cadastro = p.Data_cadastro) > 0 THEN

--                  (
--                  Select isnull(SUM(pii.total),0) 
--					from pedido with (index(ix_pedido_fluxo_caixa))
--					inner join pedido_itens as pii on pedido.pedido=pii.Pedido and pedido.Filial=pii.Filial and pedido.Tipo = pii.Tipo
--					inner join mercadoria as mi on mi.PLU = pii.PLU
--					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
--					where pedido.FILIAL=@filial 
--							 AND (pedido.Data_cadastro= p.Data_cadastro)
--							 AND pedido.TIPO = 1
--							 and pedido.pedido_simples = 1
--							 AND (LEN(@PLU)=0 OR pii.PLU = @plu)
--							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
--							) 
--							 /   
--					(	
--					Select COUNT(*) 
--					from pedido with (index(ix_pedido_fluxo_caixa))
--					where pedido.FILIAL=@filial 
--								AND  (pedido.Data_cadastro= p.Data_cadastro )
--								AND   pedido.TIPO = 1	
--								and pedido.pedido_simples = 1
								
--								AND (
--						pedido.pedido IN (SELECT DISTINCT pedido 
--											FROM pedido_Itens as pii  inner join mercadoria as mi on mi.PLU = pii.PLU
--											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
--											 WHERE  (LEN(@PLU)=0 OR pii.PLU = @plu)
--													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
--													  )
--									)
--					)

--                  ELSE 0 END)
                  
                  
--      ) as [Venda_MD],
--      0 as [NFP],
--      0 AS [Vlr NFP],
--      0 as [Perc NP]

--from  Pedido as p with (index(ix_pedido_fluxo_caixa))
--inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
--inner join mercadoria as m on m.PLU = pit.PLU
--INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
--WHERE p.pedido_simples = 1   
--	AND  (p.Data_cadastro BETWEEN @DataDe AND @DataAte)
--	 and isnull(p.Status,0)<>3
--	 AND   p.TIPO = 1	
--	 and p.FILIAL=@filial 
--	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
--	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
--	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
--	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
--GROUP BY p.Data_cadastro
--END

print 'media'
 Select Hora,
		case when Sum(Venda)>0 then  (Sum(Venda)/@dias) else 0 end as Venda
		,case when SUM(Clientes) >0 then (SUM(Clientes)/@dias) else 0 end as Clientes 
		,case when (Sum(Venda)/@dias)>0  and (SUM(Clientes)/@dias)>0 then (Sum(Venda)/@dias)/(SUM(Clientes)/@dias) else 0 end as [Venda_MD]
		,case when SUM(nfp)>0 then (SUM(nfp)/@dias) else 0 end as NFP
		,case when SUM(VLR_NFP)>0 then  (SUM(VLR_NFP)/@dias) else 0 end as VLR_NFP
		, '|-NI-|'+convert(varchar,case when SUM(VLR_NFP)>0 then  ((SUM(VLR_NFP)/@dias)/(Sum(Venda)/@dias))*100 else 0 end) AS PERC_NFP
	from 
	#tmpVendas
GROUP BY hora
ORDER BY hora ;


go 


IF OBJECT_ID('[sp_Rel_Mapa_Resumo]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Mapa_Resumo]
end 

go

create Procedure [dbo].[sp_Rel_Mapa_Resumo]

	@Filial		varchar(20),
	@DataDe		varchar(11),
	@DataAte	varchar(11),
	@pdv		varchar(10)
As
--exec sp_rel_mapa_resumo 'MATRIZ','2018-04-01', '2018-04-02' , '8' 
Begin
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tbFiscal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
Begin
	Drop table tbFiscal
End
	
Select 
	Filial,
	Convert(varchar,Data,103) 'DataMovimento', 
	Caixa,
	GTInicial, 
	GTFinal, 
	isnull(Num_Seq_Primeiro_coo,0) 'CupomInicial', 
	Cupom 'CupomFinal' ,  
	Sum(GTFINAL-GTINICIAL) 'VendaBruta', 
	TotCancelado 'TotalCanc', 
	Sum(isnull(GTFINAL,0)-isnull(GTINICIAL,0)-Isnull(TotCancelado,0)-Isnull(TotDesconto,0)) 'VendaContabil', 
	TotSubstituicao 'TotalSubs', 
	TotIsenta 'TotalIsento', 
	TotNaoTrib,
	(Select Reg6 From Fiscal F Where F.GTInicial is null And F.Data = Fiscal.Data And F.Cupom = Fiscal.Cupom And F.Caixa = Fiscal.Caixa And F.Filial = Fiscal.Filial) REG6
Into tbFiscal
From 
	Fiscal 
Where 
	(Data between @DataDe And  @DataAte)
And
	Filial = @Filial	
And 
	Not GTInicial Is Null
 and (@pdv ='TODOS' or convert(varchar,caixa) = @pdv)
Group by 
	Filial, Data, Caixa, GTInicial, GTFinal, Num_Seq_Primeiro_coo, Cupom, TotCancelado, TotSubstituicao, TotIsenta, TotNaoTrib 
Order by 
	Filial, Data, Caixa
	
	
Select 
	Filial,
	DataMovimento, 
	Caixa ,
	GTInicial  , 
	GTFinal , 
	CupomInicial, 
	CupomFinal,  
	VendaBruta, 
	TotalCanc, 
	VendaContabil, 
	TotalSubs, 
	TotalIsento, 
	TotNaoTrib,
	cast(Substring(Reg6,12,6) as decimal)/100 T18,
	cast(Substring(Reg6,28,7) as decimal)/100 T07,
	cast(Substring(Reg6,45,7) as decimal)/100 T12,
	cast(Substring(Reg6,62,7) as decimal)/100 T25,
	cast(Substring(Reg6,96,7) as decimal)/100 T11	
Into #tbMapaResumo
From 
	tbFiscal 
--Where 
	--DataMovimento >= @DataDe
--And
	--DataMovimento <= @DataAte
--And
	--Filial = @Filial	
	
declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
		select @ini_periodo = inicio_periodo
			 , @fim_periodo =fim_periodo
			 , @dias_periodo = dias_periodo 
				
		from filial where filial = @filial

		
Select 
			saida_estoque.Filial,
			Data_movimento, 
			Caixa_saida,
			convert(int,coo)  cupom, 
			Vlr = Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) , 
			case when data_cancelamento is not null then Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) else 0 end 'Canc',
			cst = isnull((Select top 1 indice_st from tributacao where tributacao.nro_ecf= saida_estoque.nro_ecf and tributacao.saida_icms = saida_estoque.aliquota_icms ),'90'),
			aliq = aliquota_icms,
			hora_venda
		Into #tbFiscal
		From 
			saida_estoque
			left join Fiscal on Saida_estoque.Data_movimento = fiscal.Data
							 and Saida_estoque.Caixa_Saida = fiscal.Caixa
			inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where saida_estoque.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
			and (@PDV ='TODOS' OR CONVERT(VARCHAR,Caixa_saida) =@pdv)
			and fiscal.Caixa is null
		Order by 
			saida_estoque.Filial, Data_movimento, Caixa_saida

if ((Select COUNT(*) from #tbFiscal )>0) 
begin 
	insert into #tbMapaResumo
				Select 
				FILIAL,
				Convert(varchar,Data_movimento,103) ,
				Caixa_Saida,
				GT_INICIAL =0,
				GT_FINAL =0,
				[Cupom Inicial]		= CONVERT(VARCHAR,min(cupom)), 
				[Cupom Final]		= CONVERT(VARCHAR,max(cupom)),  
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
		
				
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[11%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=11 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
												(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or  (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
			
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte
			
			group by Filial, data_movimento, caixa_saida
end 

Select 
	Filial,               
	[Data Movimento] =DataMovimento,                  
	Caixa = 'SFT_'+CONVERT(VARCHAR,CAIXA),       
	[GT Inicial]  = GTInicial,                               
	[GT Final] =GTFinal, 
	[Cupom Inicial]='SFT_'+CONVERT(VARCHAR,CupomInicial),           
	[Cupom Final]='SFT_'+CONVERT(VARCHAR,CupomFinal), 
	[Venda Bruta] =VendaBruta,                              
	[Total Canc]=TotalCanc,                               
	[Venda Contabil]=VendaContabil,                           
	TotalSubs,                               
	TotalIsento,                             
	TotNaoTrib,                              
	Convert(Decimal(12,2),T18) T18,                                     
	Convert(Decimal(12,2),T07) T07,                                     
	Convert(Decimal(12,2),T12) T12,                                     
	Convert(Decimal(12,2),T25) T25,
	Convert(Decimal(12,2),T11) T11	
From 
	#tbMapaResumo
order by DataMovimento

Drop Table tbFiscal

End







go
insert into Versoes_Atualizadas select 'Versão:1.231.736', getdate();
GO
