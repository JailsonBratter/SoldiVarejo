

IF OBJECT_ID('[sp_Rel_Resumo_Vendas_hora_media]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Resumo_Vendas_hora_media]
end 

go

Create  Procedure [dbo].[sp_Rel_Resumo_Vendas_hora_media]
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
	hora varchar(20),
	Venda Decimal(18,2),
	Qtde Decimal(18,2),
	Clientes int
	
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
            Qtde =SUM(ISNULL(Saida_estoque.Qtde,0) ),
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
	  substring(Hora_venda,1,2)+':00 - '+substring(Hora_venda,1,2)+':59',
      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
      Qtde = (SELECT Convert(Decimal(18,2),SUM(ISNULL(Qtde,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
	  Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) 
    
	 
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
		substring(N.emissao_hora,1,2)+':00 - '+substring(N.emissao_hora,1,2)+':59'  as horas,
		Venda = SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) ,
		Qtde = SUM(isnull(ni.Qtde,0) * isnull(ni.Embalagem,0)) ,
		Clientes = (
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


print 'media'
 Select Hora
		,Sum(Venda) as [Venda TT]
		,case when Sum(Venda)>0 then  (Sum(Venda)/@dias) else 0 end as [Venda MD]
		,Sum(Qtde) as [Qtde TT]
		,case when Sum(Qtde)>0 then  (Sum(Qtde)/@dias) else 0 end as [Qtde MD]
		,Sum(clientes) as [Clientes TT]
		,case when SUM(Clientes) >0 then (SUM(Clientes)/@dias) else 0 end as [Clientes MD] 
		
from 
	#tmpVendas
GROUP BY hora
ORDER BY hora ;


go
	insert into Versoes_Atualizadas select 'Versão:1.234.740', getdate();
GO
