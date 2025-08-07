


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('Cargo1'))
begin
	alter table Fornecedor alter column cargo1 varchar(20)
	alter table Fornecedor alter column telefone1_2 varchar(15)
	alter table Fornecedor alter column telefone1_3 varchar(15)
	alter table Fornecedor alter column email1 varchar(50)

	alter table Fornecedor alter column cargo2 varchar(20)
	alter table Fornecedor alter column telefone2_2 varchar(15)
	alter table Fornecedor alter column telefone2_3 varchar(15)
	alter table Fornecedor alter column email2 varchar(50)

	alter table Fornecedor alter column cargo3 varchar(20)
	alter table Fornecedor alter column telefone3_2 varchar(15)
	alter table Fornecedor alter column telefone3_3 varchar(15)
	alter table Fornecedor alter column email3 varchar(50)

end
else
begin
		
	alter table fornecedor add cargo1 varchar(20)
	alter table fornecedor add telefone1_2 varchar(15)
	alter table fornecedor add telefone1_3 varchar(15)
	alter table fornecedor add email1 varchar(50)

	alter table fornecedor add cargo2 varchar(20)
	alter table fornecedor add telefone2_2 varchar(15)
	alter table fornecedor add telefone2_3 varchar(15)
	alter table fornecedor add email2 varchar(50)

	alter table fornecedor add cargo3 varchar(20)
	alter table fornecedor add telefone3_2 varchar(15)
	alter table fornecedor add telefone3_3 varchar(15)
	alter table fornecedor add email3 varchar(50)

end 
go 


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.176.623', getdate();
GO

