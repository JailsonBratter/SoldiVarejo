


/****** Object:  StoredProcedure [dbo].[sp_Rel_Mapa_Resumo_SAT]    Script Date: 06/06/2018 12:27:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Mapa_Resumo_SAT]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Mapa_Resumo_SAT]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Mapa_Resumo_SAT]    Script Date: 06/06/2018 12:27:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE Procedure [dbo].[sp_Rel_Mapa_Resumo_SAT]

	@Filial		varchar(20),
	@DataDe		varchar(8),
	@DataAte	varchar(8),
	@tipo	    varchar(30)
	
As

Begin
		-- 

		--set @filial ='MATRIZ'
		--SET @DataDe = '20170720'
		--set @DataAte =@DataDe
		
		--exec     sp_Rel_Mapa_Resumo_SAT    @Filial='MATRIZ', @datade = '20170711',  @dataate = '20170711',  @tipo = 'ANALITICO' 
		
		declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
		select @ini_periodo = inicio_periodo
			 , @fim_periodo =fim_periodo
			 , @dias_periodo = dias_periodo 
				
		from filial where filial = @filial
				

		IF OBJECT_ID(N'tempdb..#tbFiscal', N'U') IS NOT NULL  
			DROP TABLE #tbFiscal; 


			
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
			inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where saida_estoque.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
		Order by 
			saida_estoque.Filial, Data_movimento, Caixa_saida
			
			
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
				0
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte
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
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte
			group by data_movimento
			
			
			
			) as a 
			order by ordem
			
			alter table #tbFiscalFinal drop column ordem
			alter table #tbFiscalFinal drop column [Data Movimento]
			
			Select * from #tbFiscalFinal
			union all 
			Select 
				'|-SUB-|TOTAL:',
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
				[25%]				= sum([25%])
			From 
				#tbFiscalFinal 
			where filial ='|-SUB-|'
			
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
			From 
				#tbFiscal2

			)as a 
			order by filial desc, convert(varchar,[Data movimento],102) 


		END



End




GO




GO


UPDATE MERCADORIA SET CEST =  REPLACE(LTRIM(RTRIM(CEST)),' ','')

GO
UPDATE MERCADORIA SET CEST =0 WHERE CEST IS NULL OR CEST =''

GO 
ALTER TABLE MERCADORIA ALTER COLUMN CEST INT NOT NULL 

GO 

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('MERCADORIA') 
            AND  UPPER(COLUMN_NAME) = UPPER('CEST')
            AND COLUMN_DEFAULT IS NULL)
BEGIN 
	ALTER TABLE MERCADORIA ADD DEFAULT 0 FOR CEST
END

 
GO 


/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 06/06/2018 10:58:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Movimento_Venda]
GO


/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 06/06/2018 10:58:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--PROCEDURES =======================================================================================
CREATE Procedure [dbo].[sp_Movimento_Venda]

                @Filial				As Varchar(20),
                @DataDe				As Varchar(8),
                @DataAte			As Varchar(8),
                @finalizadora		As varchar(30),
                @plu				As varchar(17),
                @cupom				As varchar(20),
                @pdv				as varchar(10),                
                @horaInicio			as varchar(5),				
				@horafim			as varchar(5),
				@cancelados			as varchar(5),
				@comanda			as varchar(20),
				@ext_sat			as varchar(6)

AS

begin 

IF(@plu='' AND @cupom='' and @ext_sat ='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = 'SFT_'+SUBSTRING(S.ID_CHAVE,35,6),
                             CONVERT(varchar , s.Hora_venda,108) as HORA,
                             
							[COMANDA/PEDIDOS] =  '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                        
                             
                             FINALIZADORA = lista.id_finalizadora,
							
						     VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) 
										FROM Lista_finalizadora list1
				                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 
								  			 --INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01)) ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim
									WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
											 AND (list1.Emissao = lista.Emissao)
											 and list1.pdv =lista.pdv
											 and list1.documento = lista.documento
											 AND LIST1.id_finalizadora = LISTA.id_finalizadora
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
								and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)FROM
                             Lista_finalizadora lista
                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
							INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	and s.Data_movimento = lista.Emissao
                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )
								  and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim 
                                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
								   and (LEN(@cancelados)=0 OR
										
								   (@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
																						   
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')																   
                                   
						GROUP BY lista.Emissao, lista.pdv, lista.Documento ,lista.id_finalizadora, CONVERT(varchar , s.Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6) 
						

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = 'SFT_'+SUBSTRING(S.ID_CHAVE,35,6),
                             CONVERT(varchar , Hora_venda,108) AS HORA,

                             [COMANDA/PEDIDOS] = '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                                   VLR =(SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                                   
                         ),        

                             FINALIZADORA = id_finalizadora,
                             
                     
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento
                                    )

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
                             INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and s.Data_movimento = lista.Emissao

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 
						and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
                         and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
						 and (LEN(@cancelados)=0 OR
								(@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
									
                       and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora,CONVERT(varchar , Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6)

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='' and @ext_sat = '')

BEGIN

      SELECT CUPOM = S.Documento,
			 [Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),
             DATA = CONVERT(VARCHAR,s.Data_movimento,103),
             HORA = convert(varchar,Hora_venda),
	         PDV=convert(varchar,s.caixa_saida) ,
			 'PLU'+S.PLU as PLU,
			 DESCRICAO =M.Descricao,
             QTDE=replace(convert(varchar,S.Qtde),'.',','),
             VLR=replace(convert(varchar,S.vlr),'.',','),
			 [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),
			 [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),
			 TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

      FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
					--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
                    INNER JOIN Mercadoria M ON S.PLU = M.PLU      
	  where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )
                        and (len(@plu)=0 or s.PLU = @plu )
                        And s.Data_Cancelamento is null
                        and s.Data_movimento BETWEEN @DataDe  AND  @DataAte
						and s.Data_movimento between @DataDe aND @DataAte
                         --and (LEN(@pdv)=0 or l.pdv = @pdv)
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by s.Data_movimento , Hora_venda

      END

ELSE

      BEGIN
      
            SELECT	CUPOM = S.Documento, --1
					[Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),--2
					DATA = CONVERT(VARCHAR,s.Data_movimento,103), --3
					PDV=convert(varchar,s.caixa_saida) ,--4
					'PLU'+S.PLU AS PLU, --5
					DESCRICAO = M.Descricao, --6
					QTDE=replace(convert(varchar,S.Qtde),'.',','),--7
					VLR=replace(convert(varchar,S.vlr),'.',','),--8
					[-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
					[+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
					TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
						--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
						INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
						and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
                        and s.PLU like (case when @plu <>'' then @plu else '%' end )
                        and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                        And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                       Group by s.Documento,s.data_movimento,s.caixa_saida,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda,SUBSTRING(S.ID_CHAVE,35,6)
		union all                      
			SELECT '',--1
				   '',--2
                   '|-CANCELADO-|',--3
				   pdv=convert(varchar,s.caixa_saida) ,--4
                   '|-'+S.PLU+'-|',--5
                   '|-'+M.Descricao+'-|', --6
                   Qtde=replace(convert(varchar,S.Qtde),'.',','),--7
                   Vlr=replace(convert(varchar,S.vlr),'.',','),--8
                   [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
                   [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
                   Total='0,000' --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))
				--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
				INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
					and s.PLU like (case when @plu <>'' then @plu else '%' end )
                    and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                	And s.Data_Cancelamento is NOT null
					and s.Data_movimento between @DataDe aND @DataAte 
                    and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                    and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
		union all

            select  '',--1
					'',--2
					'',--3
					'',--4
					'',--5
					'|-'+ id_finalizadora+'-|'  ,--6
					'',--7
					'',--8
					'',--9
					'',--10
					 '|-'+replace(convert(varchar,(SUM(Lista_Finalizadora.Total))),'.',',')+'-|' --11
			from Lista_finalizadora
			where  Documento like (case when @cupom <>'TODOS' then @cupom  else '%' end  )
		
                   and Emissao BETWEEN @DataDe  AND  @DataAte
                   And Isnull(Cancelado,0) = 0
                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
			GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END


end






GO





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END


GO 

go 


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END




go 


insert into Versoes_Atualizadas select 'Versão:1.202.678', getdate();
GO
