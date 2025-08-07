IF NOT EXISTS (Select * from sequenciais where tabela_coluna ='NFPEDIDO.CODIGO')
BEGIN 
	INSERT [dbo].[SEQUENCIAIS] 
				([TABELA_COLUNA], [DESCRICAO], [SEQUENCIA], [TAMANHO], [OBS1], [OBS2], [OBS3], [OBS4], [OBS5], [OBS6], [OBS7], [OBS8], [DATA_PARA_TRANSFERENCIA], [PERMITE_POR_EMPRESA]) 
	VALUES ('NFPEDIDO.CODIGO', 'NUMERO SEQUENCIAL DE EMISSAO PEDIDOS', '000', 5,'', '', '', '', '', '', '', '', NULL, 0)

END 


GO 





/****** Object:  StoredProcedure [dbo].[SP_REL_COMANDA_VENDA]    Script Date: 03/16/2018 13:36:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_VENDA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_COMANDA_VENDA]
GO


/****** Object:  StoredProcedure [dbo].[SP_REL_COMANDA_VENDA]    Script Date: 03/16/2018 13:36:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_VENDA] (
		@filial varchar(30),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		@usuario varchar(20),
		@plu varchar(20),
		@descricao varchar(50)
		)
		 AS


Declare @totalVenda Numeric(18,3)
	 
--set @dataInicio ='20170101';
--set @datafim = '20180122'

-- set @horaInicio='00:00:59' 
--set @horafim='23:59' 
		
-- drop table #tempCom
if(@usuario = 'TODOS')
BEGIN 
	select  
		   i.Usuario,
		   Qtde = sum(i.Qtde),
		   Total = Sum(i.Total)
		   into #tempCom
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
			
	where ISNULL(i.cupom,0) <>0
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and i.data_cancelamento IS NULL 
		  and (len(@plu)=0 or i.plu=@plu)
	group by i.usuario   
	order by    Sum(i.Total) desc
	  
	 
	 Select @totalVenda = sum(total) from #tempCom
	 
	 
	 Select Usuario, Qtde, Total, [%] = convert(numeric(12,2), (total/@totalVenda)*100) from #tempCom
	 
END	  
ELSE
BEGIN 
	select  
		Data=convert(varchar,i.data,103),
		Hora = convert(varchar,i.hora_evento,108),
		PLU = 'PLU'+i.Plu,
	    m.Descricao,
	    I.Qtde,
	    i.Unitario,
	    Total = I.Total
	into #tempCom1
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
		  inner join mercadoria m on i.plu=m.PLU
	where ISNULL(i.cupom,0) <>0
		  AND I.USUARIO = @USUARIO	
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and (len(@plu)=0 or i.plu=@plu)
		  and i.data_cancelamento IS NULL 
	order by i.total desc

	Select @totalVenda = sum(total) from #tempCom1
	 
	Select Plu,Descricao,Qtde=Sum(Qtde),Total=Sum(Total),[%] = convert(numeric(12,2), (sum(total)/@totalVenda)*100)  
	from #tempCom1
	group by plu,descricao
	order by sum(total) desc

END	  
	  
GO





if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_KEY_A1')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('KCW_KEY_A1'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'17617f40-fade-405d-ac73-33963edf3060'
           ,'RAKUTEN CHAVE A1'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
 else 
 begin

	update parametros set valor_atual ='17617f40-fade-405d-ac73-33963edf3060'
	where parametro ='KCW_KEY_A1'
 end 
 

 
GO


if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_KEY_A2')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('KCW_KEY_A2'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'1a723091-5e79-49d8-af12-c8505d0c5e24'
           ,'RAKUTEN CHAVE A2'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
else
begin
	update parametros set valor_atual ='1a723091-5e79-49d8-af12-c8505d0c5e24'
	where parametro ='KCW_KEY_A2'
end
GO



if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_URL_SERVER')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('KCW_URL_SERVER'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'http://update.breeds.com.br/ikcwebservice/'
           ,'RAKUTEN URL SERVER'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
else
begin 
	update parametros set valor_atual ='http://update.breeds.com.br/ikcwebservice/'
	where parametro ='KCW_URL_SERVER'
end 
GO





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.192.645', getdate();
GO