if not EXISTS( Select * from SEQUENCIAIS where TABELA_COLUNA='precificacao.codigo')
begin
INSERT INTO [SEQUENCIAIS]
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
           ('precificacao.codigo'
           ,'Sequencia Precificacao produtos'
           ,'0'
           ,10
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

go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('precificacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('todas_filiais'))
begin
	alter table precificacao alter column todas_filiais tinyint
end
else
begin
	alter table precificacao add todas_filiais tinyint
end 

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('precificacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('vlr_porc'))
begin
	alter table precificacao alter column vlr_porc numeric(12,2)
	
end
else
begin
	alter table precificacao add vlr_porc numeric(12,2)
end 

insert into Versoes_Atualizadas select 'Versão:1.145.579', getdate();




