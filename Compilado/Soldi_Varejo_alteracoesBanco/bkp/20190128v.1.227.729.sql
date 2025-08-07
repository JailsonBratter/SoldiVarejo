CREATE NONCLUSTERED INDEX [ix_sp_rel_Mapa_Resumo_SAT] ON [dbo].[Saida_estoque]
(
	[PLU] ASC,
	[Filial] ASC,
	[Hora_venda] ASC,
	[Data_movimento] ASC,
	[coo] ASC,
	[Caixa_saida] ASC

	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF OBJECT_ID('[sp_Rel_Mapa_Resumo_SAT]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Mapa_Resumo_SAT]
end 

go
CREATE Procedure [dbo].[sp_Rel_Mapa_Resumo_SAT]

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


			print('inicio-'+convert(varchar,getdate()))
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
			,pis=CASE WHEN @CRT = 3 then case when M.CST_Saida = '01' THEN M.pis_perc_saida  else 0.0 end ELSE @PIS END 
			,pisv = CONVERT(NUMERIC(18,2),'0.00')
			,cofins=CASE WHEN @CRT = 3 then case when M.CST_Saida = '01' THEN M.cofins_perc_saida else 0.0 end ELSE @COFINS END 
			,cofinsv =CONVERT(NUMERIC(18,2),'0.00')
		Into #tbFiscal
		From 
			saida_estoque with(index(ix_sp_rel_Mapa_Resumo_SAT))
			inner join mercadoria  as m  on m.PLU = Saida_Estoque.PLU
			--INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where saida_estoque.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
			and (@PDV ='TODOS' OR CONVERT(VARCHAR,Caixa_saida) =@pdv)
		Order by 
			saida_estoque.Filial, Data_movimento, Caixa_saida
		
		
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


go
insert into Versoes_Atualizadas select 'Versão:1.227.729', getdate();
GO
