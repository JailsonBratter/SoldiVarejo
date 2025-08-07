IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('contrato_fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_reajuste'))
begin
	alter table Contrato_fornecedor alter column tipo_reajuste varchar(3)
	alter table Contrato_fornecedor alter column dia_mes_reajuste int
	
end
else
begin
	alter table contrato_fornecedor add tipo_reajuste varchar(3), dia_mes_reajuste int
end 
go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.167.608', getdate();
GO