

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contrato_fornecedor]') AND type in (N'U'))
	DROP TABLE [dbo].[Contrato_fornecedor]
GO


CREATE TABLE Contrato_fornecedor(
	id_contrato varchar(10),
	fornecedor varchar(20),
	data_Cadastro datetime,
	data_validade datetime,
	prazo varchar(50)

)



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contrato_fornecedor_item]') AND type in (N'U'))
	DROP TABLE [dbo].[Contrato_fornecedor_item]
GO


CREATE TABLE Contrato_fornecedor_item
(
  id_contrato varchar(20),
  plu varchar(20),
  vlr decimal
  
)



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contrato_Fornecedor_Filial]') AND type in (N'U'))
	DROP TABLE [dbo].[Contrato_Fornecedor_Filial]
GO
CREATE TABLE  Contrato_Fornecedor_Filial
(
	id_contrato varchar(20),
	Filial varchar(20)
)



if not exists (Select * from SEQUENCIAIS where TABELA_COLUNA ='CONTRATO_FORNECEDOR.ID_CONTRATO')
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
           ('CONTRATO_FORNECEDOR.ID_CONTRATO'
           ,'SEQUENCIA DE NUMEROS DE CONTRATOS'
           ,'0000'
           ,7
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.162.601', getdate();
GO