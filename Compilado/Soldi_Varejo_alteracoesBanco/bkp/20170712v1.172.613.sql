IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tesouraria') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_fechamento'))
begin
	alter table tesouraria alter column id_fechamento bigint
end
else
begin
		
	alter table tesouraria add id_fechamento bigint

end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tesouraria_detalhes') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_fechamento'))
begin
	alter table tesouraria_detalhes alter column id_fechamento bigint
end
else
begin
		
	alter table tesouraria_detalhes add id_fechamento bigint

end 
GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Status_Pdv') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_fechamento'))
begin
	alter table Status_Pdv alter column id_fechamento bigint
end
else
begin
		
	alter table Status_Pdv add id_fechamento bigint
end 
GO

update Status_Pdv set id_fechamento = id where Status = 'FECHADO' 


UPDATE Tesouraria SET TESOURARIA.ID_FECHAMENTO = Status_Pdv.ID_FECHAMENTO
FROM TESOURARIA INNER JOIN Status_Pdv  ON Tesouraria.DATA_ABERTURA = STATUS_PDV.DATA_ABERTURA
						AND TESOURARIA.ID_OPERADOR = STATUS_PDV.ID_OPERADOR
						AND TESOURARIA.PDV = Status_Pdv.Pdv
						AND Tesouraria.FILIAL =  STATUS_PDV.Filial
	
WHERE Status_Pdv.Status ='FECHADO' 


GO 

UPDATE tesouraria_detalhes  SET tesouraria_detalhes.ID_FECHAMENTO = Status_Pdv.ID_FECHAMENTO
FROM tesouraria_detalhes INNER JOIN Status_Pdv  ON tesouraria_detalhes.emissao = STATUS_PDV.DATA_ABERTURA
						AND tesouraria_detalhes.operador = STATUS_PDV.ID_OPERADOR
						AND tesouraria_detalhes.PDV = Status_Pdv.Pdv
						AND tesouraria_detalhes.FILIAL =  STATUS_PDV.Filial
	
WHERE Status_Pdv.Status ='FECHADO' 



if not exists (Select * from SEQUENCIAIS where TABELA_COLUNA ='TESOURARIA.ID_FECHAMENTO')
begin
INSERT INTO [dbo].[SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('TESOURARIA.ID_FECHAMENTO'
           ,'SEQUENCIA DE FECHAMENTO TESOURARIA'
           ,(SELECT CONVERT(VARCHAR,MAX(ID_FECHAMENTO))FROM Status_Pdv)
           ,6
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0)
end

GO




IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tesouraria_detalhes]') AND name = N'IX_tesouraria_detalhes')
	DROP INDEX [IX_tesouraria_detalhes] ON [dbo].[tesouraria_detalhes] WITH ( ONLINE = OFF )
GO

CREATE CLUSTERED INDEX [IX_tesouraria_detalhes] ON [dbo].[tesouraria_detalhes]
(
	[emissao] ASC,
	[cupom] ASC,
	[pdv] ASC,
	[filial] ASC,
	[operador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.172.613', getdate();
GO