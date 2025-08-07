
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Controle_Filial_PDV') 
            AND  UPPER(COLUMN_NAME) = UPPER('link_server'))
begin
	alter table controle_filial_pdv alter column link_server varchar(500)
	alter table controle_filial_pdv alter column ativa_link_server tinyint
	alter table controle_filial_pdv alter column data_ult_atualizacao datetime
	
end
else
begin
	alter table Controle_filial_pdv add link_server varchar(500)
	alter table Controle_filial_pdv add ativa_link_server tinyint
	alter table Controle_filial_pdv add data_ult_atualizacao datetime
end 
go 





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.193.647', getdate();
GO