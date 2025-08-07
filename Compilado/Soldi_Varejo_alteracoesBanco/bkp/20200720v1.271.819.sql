IF OBJECT_ID (N'Cardapio', N'U') IS NULL 
begin
create table Cardapio(
	Filial varchar(20),
	Data_cadastro datetime,
	Data_ultima_alteracao datetime,
	Usuario_cadastro varchar(40),
	Usuario_alteracao varchar(40),
	url_padrao_cardapio nvarchar(max),
	token varchar(500)
);
end
go 



IF OBJECT_ID (N'Cardapio_categorias', N'U') IS NULL 
begin
create table Cardapio_categorias(
	Filial varchar(20),
	id int,
	Categoria varchar(150),
	Pizza int,
	CategoriaMeia int,
	CategoriaTerco int,
	Status varchar(140)
);
end
go 

IF OBJECT_ID (N'Cardapio_produtos', N'U') IS NULL 
begin
create table Cardapio_produtos(
	Filial varchar(20),
	Plu varchar(20),
	idCategoria int,
	Ativo tinyint,
	precoPorObs int
);

end
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria_OBS') 
            AND  UPPER(COLUMN_NAME) = UPPER('Tipo'))
begin
	alter table Mercadoria_OBS alter column Tipo varchar(20)
end
else
begin
	alter table Mercadoria_OBS add Tipo varchar(20)
	
end 
go 




/****** Object:  Index [ix_tesouraria]    Script Date: 24/08/2020 12:22:55 ******/
CREATE NONCLUSTERED INDEX [ix_tesouraria] ON [dbo].[Saida_estoque]
(
	[Filial] ASC,
	[data_cancelamento] ASC,
	[id_movimento] ASC,
	[Caixa_Saida] ASC,
	[Codigo_Funcionario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Rel_Fin_PorOperador]
END
GO
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL         AS VARCHAR(17),
	@Datade               As DATETIME,
	@Dataate        As DATETIME,
	@Caixa          As varchar(30),
	@Grupo        As Varchar(60),
	@subGrupo   as Varchar(60),
	@Departamento as Varchar(60),
	@Familia   as Varchar(60),
	@fornecedor   as Varchar(40),
	@plu           as Varchar(20),
	@descricao    as Varchar(50),
	@ordem            as Varchar(30),
	@ordemS        as Varchar(30),
	@finalizadora as Varchar(30)
)

as
begin 
	   -- sp_Rel_Fin_PorOperador 'MATRIZ', '2020-07-01', '2020-08-05', 'TODOS', '', '', '', '', '', '', '', 'VALOR', 'DECRESCENTE', 'IFOOD'

		   select distinct    

						a.plu as PLU,

						b.descricao AS DESCRICAO,

						c.Descricao_grupo as GRUPO,

						C.descricao_subgrupo AS SUBGRUPO,

						C.descricao_departamento DEPARTAMENTO,

						F.Descricao_Familia AS FAMILIA ,

						sum(Qtde) as Qtde ,

						((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor

                   

				 into #vendasTot                  

		   from saida_estoque   a with(index(ix_Rel_fin_porOperador))

						inner join mercadoria b on a.plu =b.plu 

						inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento

						left join familia f on b.codigo_familia=f.codigo_familia

						left join Fornecedor_Mercadoria fm on a.PLU = fm.plu and (len(@fornecedor)<>0  and  fm.fornecedor = @fornecedor)

		   where a.filial=@FILIAL

				 and  data_cancelamento is null

				 and (Data_movimento between @Datade and @Dataate)

				 and (len(@Grupo)=0 or  c.Descricao_grupo = @Grupo)

				 and (len(@subGrupo)=0 or  c.descricao_subgrupo = @subGrupo)

				 and (len(@Departamento)=0 or c.descricao_departamento = @Departamento)

				 and (len(@familia)=0 or isnull(f.Descricao_Familia,'') = @Familia)

				 and (LEN(@plu)=0 or a.PLU=@plu)

				 and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')

				 and (len(@fornecedor)=0 or fm.fornecedor = @fornecedor)

				 and (@caixa='TODOS' or ( Select TOP 1 Operadores.Nome

										from Operadores

											inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador

										where  l.filial =a.Filial 

											and l.Emissao = a.Data_movimento

											and a.Caixa_Saida = l.pdv

											and a.Documento=L.Documento) = @caixa)

				 and (@finalizadora ='TODOS' or ( Select TOP 1 Finalizadora.Finalizadora 

										from Finalizadora

											inner join Lista_finalizadora l on l.finalizadora  = Finalizadora.Nro_Finalizadora 

										where  l.filial =a.Filial 

											and l.Emissao = a.Data_movimento

											and a.Caixa_Saida = l.pdv

											and a.Documento=L.Documento) = @finalizadora )

		   group by a.plu,b.descricao

						,c.Descricao_grupo

						,c.Descricao_subgrupo

						,c.Descricao_departamento

						,f.Descricao_familia

                   

		   order by b.descricao

 

 

 

	Select ORDEM,

			  PLU,

			  DESCRICAO,

			  SUBGRUPO,

			  DEPARTAMENTO,

			  FAMILIA ,

			  Qtde ,

			  Valor   

			  INTO #tbFinalVenda

	from

	(

		   Select Ordem = GRUPO+'0' -- CABECALHO

						,GRUPO + '|-TITULO-||CONCAT|' AS PLU

						,'' AS DESCRICAO

						,'' AS SUBGRUPO

						,'' AS DEPARTAMENTO

						,'' AS FAMILIA

						,0  AS Qtde

						,0  AS Valor    

		   FROM  #vendasTot

		   group by grupo

		   UNION ALL

		   Select Ordem = GRUPO+'1' -- ITENS

						,'PLU'+PLU

						,DESCRICAO

						,SUBGRUPO

						,DEPARTAMENTO

						,FAMILIA

						,Qtde

						,Valor    

		   FROM  #vendasTot

		   UNION ALL

      

		   Select Ordem = GRUPO+'9' -- TOTAL

						,'|-SUB-|' --PLU

						,'' --DESCRICAO

						,'' --SUBGRUPO

						,'' --DEPARTAMENTO

						,'' --FAMILIA

						,SUM(QTDE)  --Qtde

						,SUM(Valor)  --Valor    

		   FROM  #vendasTot

		   group by grupo

		   union all

		   Select Ordem = 'ZZZZZ9' -- TOTAL GERAL

						,'|-SUB-|TOTAL ' --PLU

						,'' --DESCRICAO

						,'' --SUBGRUPO

						,'' --DEPARTAMENTO

						,'' --FAMILIA

						,SUM(QTDE)  --Qtde

						,SUM(VALOR)  --Valor    

		   FROM  #vendasTot

	) as a

 

 

 

 

	IF( @ORDEM='DESCRICAO' AND @ORDEMS='DECRESCENTE' ) 

	BEGIN 

		   SELECT PLU,

			  DESCRICAO,

			  SUBGRUPO,

			  DEPARTAMENTO,

			  FAMILIA ,

			  Qtde ,

			  Valor   

		   FROM #tbFinalVenda  ORDER BY ORDEM, DESCRICAO DESC

	END

            

	ELSE IF(@ORDEM='DESCRICAO' AND @ORDEMS='CRESCENTE'  )

	BEGIN

		   SELECT PLU,

			  DESCRICAO,

			  SUBGRUPO,

			  DEPARTAMENTO,

			  FAMILIA ,

			  Qtde ,

			  Valor    

			FROM #tbFinalVenda  ORDER BY ORDEM, DESCRICAO

	END

	ELSE IF(@ORDEM='VALOR' AND @ORDEMS='CRESCENTE'  )

	BEGIN

		   SELECT PLU,

			  DESCRICAO,

			  SUBGRUPO,

			  DEPARTAMENTO,

			  FAMILIA ,

			  Qtde ,

			  Valor    

		   FROM #tbFinalVenda  ORDER BY ORDEM, VALOR

	END

	ELSE IF(@ORDEM='VALOR' AND @ORDEMS='DECRESCENTE'  )

	BEGIN

		   SELECT PLU,

			  DESCRICAO,

			  SUBGRUPO,

			  DEPARTAMENTO,

			  FAMILIA ,

			  Qtde ,

			  Valor    

		   FROM #tbFinalVenda  ORDER BY ORDEM, VALOR DESC

	END

end 

 

                                 

GO
 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_ICMSIPI_1010]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_EFD_ICMSIPI_1010]
END
GO
CREATE PROC [dbo].[sp_EFD_ICMSIPI_1010]
AS
	SELECT '|1010|N|N|N|N|N|N|N|N|N|N|N|N|N|'
go 



IF OBJECT_ID (N'lerComandaImporta', N'U') IS NULL 
begin
create table lerComandaImporta (
	Comanda Int null
	
)
end
go 

if not exists(select 1 from Tipo_movimentacao where Movimentacao='DESPERDICIO')
begin	
insert into Tipo_movimentacao
select 'DESPERDICIO', 1, 0, NULL
 end
GO


 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Mapa_Resumo_SAT]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Rel_Mapa_Resumo_SAT]
END
GO

-- sp_Rel_Mapa_Resumo_SAT 'MATRIZ','20200601','20200630','CONSOLIDADO','TODOS'

create Procedure [dbo].[sp_Rel_Mapa_Resumo_SAT]

	@Filial		varchar(20),
	@DataDe		varchar(8),
	@DataAte	varchar(8),
	@tipo	    varchar(30),
	@pdv		varchar(10)
	
As

Begin
		-- 

		--set @filial ='MATRIZ'
		--SET @DataDe = '20170720'
		--set @DataAte =@DataDe
		
		--exec     sp_Rel_Mapa_Resumo_SAT    @Filial='MATRIZ', @datade = '20181201',  @dataate = '20181231',  @tipo = 'ANALITICO',@pdv='TODOS' 
		
		declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int, @CRT int,
				@PIS NUMERIC(12,2), @COFINS NUMERIC(12,2)
		select @ini_periodo = inicio_periodo
			 , @fim_periodo =fim_periodo
			 , @dias_periodo = dias_periodo 
			 , @CRT= CRT
			 ,@PIS = PIS
			 ,@COFINS = cofins
				
		from filial where filial = @filial
				

		IF OBJECT_ID(N'tempdb..#tbFiscal', N'U') IS NOT NULL  
			DROP TABLE #tbFiscal; 

select * into #saida_Fiscal_Sat
From 
			saida_estoque with(index(ix_sp_rel_Mapa_Resumo_SAT))
			--INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where saida_estoque.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
			and (@PDV ='TODOS' OR CONVERT(VARCHAR,Caixa_saida) =@pdv)


			print('inicio-'+convert(varchar,getdate()))
		Select 
			s.Filial,
			Data_movimento, 
			Caixa_saida,
			convert(int,coo)  cupom, 
			Vlr = Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) , 
			case when data_cancelamento is not null then Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) else 0 end 'Canc',
			cst = isnull((Select top 1 indice_st from tributacao where tributacao.nro_ecf= s.nro_ecf and tributacao.saida_icms = s.aliquota_icms ),'90'),
			aliq = aliquota_icms,
			hora_venda
			,pis=CASE WHEN @CRT = 3 then case when M.CST_Saida = '01' THEN M.pis_perc_saida  else 0.0 end ELSE @PIS END 
			,pisv = CONVERT(NUMERIC(18,2),'0.00')
			,cofins=CASE WHEN @CRT = 3 then case when M.CST_Saida = '01' THEN M.cofins_perc_saida else 0.0 end ELSE @COFINS END 
			,cofinsv =CONVERT(NUMERIC(18,2),'0.00')
		Into #tbFiscal
		From 
			#saida_Fiscal_Sat s --with(index(ix_sp_rel_Mapa_Resumo_SAT))
			inner join mercadoria  as m  on m.PLU = s.PLU
			--INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where s.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
			and (@PDV ='TODOS' OR CONVERT(VARCHAR,Caixa_saida) =@pdv)
		Order by 
			s.Filial, Data_movimento, Caixa_saida
		
		
		update #tbFiscal set pisv = (vlr * ISNULL(pis,0))/100, cofinsv = (vlr * ISNULL(cofins,0))/100
		WHERE Canc =0 


		print('#tbFiscal-'+convert(varchar,getdate()))	
		if(@tipo='ANALITICO')
		BEGIN 
		
			select * into #tbFiscalFinal from (	
			Select 
				ordem =convert(varchar,Data_movimento,102)+'01'+CONVERT(VARCHAR,Caixa_saida), 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				Caixa				= 'SFT_'+CONVERT(VARCHAR,Caixa_saida),
				[Cupom Inicial]		= 'SFT_'+CONVERT(VARCHAR,min(cupom)), 
				[Cupom Final]		= 'SFT_'+CONVERT(VARCHAR,max(cupom)),  
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
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
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
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
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
											))
				,[PIS] = sum(isnull(pisv,0))
				,[COFINS] = SUM(ISNULL(cofinsv,0))
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte
			
			group by Filial, data_movimento, caixa_saida
			union  all 
				Select 
				ordem =convert(varchar,Data_movimento,102)+'00' ,
				'|-TITULO-||CONCAT|'+convert(varchar,Data_movimento,103),
				'', 
				'',
				'', 
				'',  
				0, 
				0, 
				0, 
				0, 
				0, 
				0,
				0,
				0,	
				0,
				0,
				0,
				0,
				0,
				0
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte AND @PDV='TODOS'
			group by data_movimento
			union all 
			Select 
				ordem =convert(varchar,Data_movimento,102)+'99' ,
				'|-SUB-|',
				[Data Movimento]	= '', 
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),	
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
				,[PIS] = sum(isnull(pisv,0))
				,[COFINS] = SUM(ISNULL(cofinsv,0))
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte AND @PDV='TODOS'
			group by data_movimento
			
			
			
			) as a 
			order by ordem
			
			alter table #tbFiscalFinal drop column ordem
			
			
			
			
			SELECT * INTO #tbFiscalFinal1  FROM (
			Select * from #tbFiscalFinal
			union all 
			Select 
				'|-SUB-|TOTAL:',
				Data				= '',
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		= sum([Venda Bruta]), 
				[Total Canc]		= sum([Total Canc]) , 
				[Venda Contabil]	= sum([Venda Contabil]), 
				[Total Subs]		= sum([Total Subs]), 
				[Total Isento]		= sum([Total Isento]), 
				[Total Nao Trib]	= sum([Total Nao Trib]),
				[Total Outros]		= sum([Total Outros]),
				[3,2%]				= sum([3,2%]),	
				[7%]				= sum([7%]),
				[12%]				= sum([12%]),
				[18%]				= sum([18%]),
				[25%]				= sum([25%]),
				[PIS]				=SUM([PIS]),
				[COFINS]			= SUM(COFINS)
			From 
				#tbFiscalFinal 
			where filial ='|-SUB-|' OR @PDV <>'TODOS'
			) AS F
			
			
			
			if(@pdv='TODOS')
			BEGIN
				alter table #tbFiscalFinal1 drop column [Data Movimento]
			END 
			
			SELECT * FROM #tbFiscalFinal1
			
			
		END
		ELSE
		BEGIN
			
			Select 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),	
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
				,[PIS] = sum(isnull(pisv,0))
				,[COFINS] = SUM(ISNULL(cofinsv,0))
				
			into #tbFiscal2
			From 
				#tbFiscal
			where Data_movimento between @DataDe and @DataAte
			group by Filial, data_movimento
			
			select * from (
			Select * from #tbFiscal2
			union all
			Select 
				'|-SUB-|TOTAL:',
				[Data Movimento]	= '', 
				[Venda Bruta]		= sum([Venda Bruta]), 
				[Total Canc]		= sum([Total Canc]), 
				[Venda Contabil]	= sum([Venda Contabil]), 
				[Total Subs]		= sum([Total Subs]) , 
				[Total Isento]		= sum([Total Isento]), 
				[Total Nao Trib]	= sum([Total Nao Trib]),
				[Total Outros]		= sum([Total Outros]),
				[3,2%]				= sum([3,2%]),	
				[7%]				= sum([7%]),
				[12%]				= sum([12%]),
				[18%]				= sum([18%]),
				[25%]				= sum([25%])
				,[PIS]				= sum(isnull(pis,0))
				,[COFINS]			= SUM(ISNULL(cofins,0))
			From 
				#tbFiscal2

			)as a 
			order by filial desc, convert(varchar,[Data movimento],102) 


		END



End





go



insert into Versoes_Atualizadas select 'Versão:1.271.819', getdate();