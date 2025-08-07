if not exists(select 1 from PARAMETROS where PARAMETRO='INCLUI_CATEGORIA')
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
           ('INCLUI_CATEGORIA'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'MOSTRAS CATEGORIAS DOS DEPARTAMENTOS'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO


IF OBJECT_ID (N'Categorias', N'U') IS NULL 
begin
create table Categorias (
	codigo_departamento varchar(20),
	codigo varchar(20),
	descricao varchar(50)
)


end
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Categoria'))
begin
	alter table Mercadoria alter column Categoria varchar(20)
	alter table Mercadoria alter column Seguimento varchar(20)
	alter table Mercadoria alter column SubSeguimento varchar(20)
	alter table Mercadoria alter column GrupoCategoria varchar(20)
	alter table Mercadoria alter column SubGrupoCategoria varchar(20)
end
else
begin
	alter table Mercadoria add Categoria varchar(20)
	alter table Mercadoria add Seguimento varchar(20)
	alter table Mercadoria add SubSeguimento varchar(20)
	alter table Mercadoria add GrupoCategoria varchar(20)
	alter table Mercadoria add SubGrupoCategoria varchar(20)
end 
go 

GO 
insert into Versoes_Atualizadas select 'Versão:1.268.814', getdate();
