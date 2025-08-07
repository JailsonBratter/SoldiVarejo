
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Mapa_Resumo_SAT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_Mapa_Resumo_SAT]
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

		IF OBJECT_ID(N'tempdb..#tbFiscal', N'U') IS NOT NULL  
			DROP TABLE #tbFiscal; 


			
		Select 
			saida_estoque.Filial,
			Data_movimento, 
			Caixa_saida,
			coo  cupom, 
			vlr , 
			case when data_cancelamento is not null then vlr else 0 end 'Canc',
			cst = isnull((Select top 1 indice_st from tributacao where tributacao.nro_ecf= saida_estoque.nro_ecf and tributacao.saida_icms = saida_estoque.aliquota_icms ),'90'),
			aliq = aliquota_icms 
		Into #tbFiscal
		From 
			saida_estoque
				
		Where Data_movimento between @DataDe and @DataAte
			And	saida_estoque.Filial = @Filial	
		Order by 
			saida_estoque.Filial, Data_movimento, Caixa_saida
			
			
		if(@tipo='ANALITICO')
		BEGIN 
		
			select * into #tbFiscalFinal from (	
			Select 
				ordem =convert(varchar,Data_movimento,102)+'01', 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				Caixa				= 'SFT_'+CONVERT(VARCHAR,Caixa_saida),
				[Cupom Inicial]		= 'SFT_'+CONVERT(VARCHAR,min(cupom)), 
				[Cupom Final]		= 'SFT_'+CONVERT(VARCHAR,max(cupom)),  
				[Venda Bruta]		=  sum(vlr), 
				[Total Canc]		= sum(Canc), 
				[Venda Contabil]	= sum(vlr) - sum(canc), 
				[Total Subs]		= sum(case when cst ='60' AND canc=0 then vlr else 0 end) , 
				[Total Isento]		= sum(case when cst ='40' AND canc=0 then vlr else 0 end), 
				[Total Nao Trib]	= sum(case when cst ='41' AND canc=0 then vlr else 0 end),
				[Total Outros]		= sum(case when cst ='90' AND canc=0 then vlr else 0 end),
				[3,2%]				= sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end),	
				[7%]				= sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end),
				[12%]				= sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end),
				[18%]				= sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end),
				[25%]				= sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
			From 
				#tbFiscal 
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
			group by data_movimento
			union all 
			Select 
				ordem =convert(varchar,Data_movimento,102)+'99' ,
				'|-SUB-|',
				[Data Movimento]	= '', 
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		=  sum(vlr), 
				[Total Canc]		= sum(Canc), 
				[Venda Contabil]	= sum(vlr) - sum(canc), 
				[Total Subs]		= sum(case when cst ='60' AND canc=0 then vlr else 0 end) , 
				[Total Isento]		= sum(case when cst ='40' AND canc=0 then vlr else 0 end), 
				[Total Nao Trib]	= sum(case when cst ='41' AND canc=0 then vlr else 0 end),
				[Total Outros]		= sum(case when cst ='90' AND canc=0 then vlr else 0 end),
				[3,2%]				= sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end),	
				[7%]				= sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end),
				[12%]				= sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end),
				[18%]				= sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end),
				[25%]				= sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
			From 
				#tbFiscal 
			group by data_movimento
			union all 
			Select 
				ordem				='ZZZZZ99' ,
				'|-SUB-|TOTAL:',
				[Data Movimento]	= '', 
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		= sum(vlr), 
				[Total Canc]		= sum(Canc), 
				[Venda Contabil]	= sum(vlr) - sum(canc), 
				[Total Subs]		= sum(case when cst ='60' AND canc=0 then vlr else 0 end) , 
				[Total Isento]		= sum(case when cst ='40' AND canc=0 then vlr else 0 end), 
				[Total Nao Trib]	= sum(case when cst ='41' AND canc=0 then vlr else 0 end),
				[Total Outros]		= sum(case when cst ='90' AND canc=0 then vlr else 0 end),
				[3,2%]				= sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end),	
				[7%]				= sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end),
				[12%]				= sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end),
				[18%]				= sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end),
				[25%]				= sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
			From 
				#tbFiscal 
			
			
			) as a 
			order by ordem
			
			alter table #tbFiscalFinal drop column ordem
			alter table #tbFiscalFinal drop column [Data Movimento]
			Select * from #tbFiscalFinal
		END
		ELSE
		BEGIN
			Select * from ( 
			Select 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				[Venda Bruta]		= sum(vlr), 
				[Total Canc]		= sum(Canc), 
				[Venda Contabil]	= sum(vlr) - sum(canc), 
				[Total Subs]		= sum(case when cst ='60' AND canc=0 then vlr else 0 end) , 
				[Total Isento]		= sum(case when cst ='40' AND canc=0 then vlr else 0 end), 
				[Total Nao Trib]	= sum(case when cst ='41' AND canc=0 then vlr else 0 end),
				[Total Outros]		= sum(case when cst ='90' AND canc=0 then vlr else 0 end),
				[3,2%]				= sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end),	
				[7%]				= sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end),
				[12%]				= sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end),
				[18%]				= sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end),
				[25%]				= sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
				
			From 
				#tbFiscal 
			group by Filial, data_movimento
		
			union all 
			Select 
				'|-SUB-|TOTAL:',
				[Data Movimento]	= '', 
				[Venda Bruta]		= sum(vlr), 
				[Total Canc]		= sum(Canc), 
				[Venda Contabil]	= sum(vlr) - sum(canc), 
				[Total Subs]		= sum(case when cst ='60' AND canc=0 then vlr else 0 end) , 
				[Total Isento]		= sum(case when cst ='40' AND canc=0 then vlr else 0 end), 
				[Total Nao Trib]	= sum(case when cst ='41' AND canc=0 then vlr else 0 end),
				[Total Outros]		= sum(case when cst ='90' AND canc=0 then vlr else 0 end),
				[3,2%]				= sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end),	
				[7%]				= sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end),
				[12%]				= sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end),
				[18%]				= sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end),
				[25%]				= sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
			From 
				#tbFiscal 

			)as a 
			order by filial desc, convert(varchar,[Data movimento],102) 


		END



End





GO





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.201.677', getdate();
GO
