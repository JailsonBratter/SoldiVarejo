

go
if exists(select 1 from syscolumns where id =OBJECT_ID('Saida_estoque') and name='FlagNI')
	alter table Saida_estoque alter column FlagNI varchar(1)
else	
	alter table Saida_estoque add FlagNI varchar(1)

 
go
if exists(select 1 from syscolumns where id =OBJECT_ID('Saida_estoque') and name='ComandaPedido')
	alter table Saida_estoque alter column ComandaPedido varchar(10)
else	
	alter table Saida_estoque add ComandaPedido varchar(10)

 
go
if exists(select 1 from syscolumns where id =OBJECT_ID('Saida_estoque') and name='ComandaPedidoCupom')
	alter table Saida_estoque alter column ComandaPedidoCupom varchar(255)
else	
	alter table Saida_estoque add ComandaPedidoCupom varchar(255)



go
if exists(select 1 from syscolumns where id =OBJECT_ID('CARTAO') and name='id_Bandeira')
	alter table CARTAO alter column id_Bandeira varchar(3)
else	
	ALTER TABLE CARTAO ADD  id_Bandeira VARCHAR(3)
go
if exists(select 1 from syscolumns where id =OBJECT_ID('CARTAO') and name='id_Rede')
	alter table CARTAO alter column id_Rede varchar(3)
else	
	ALTER TABLE CARTAO ADD  id_Rede VARCHAR(3)
go
if exists(select 1 from syscolumns where id =OBJECT_ID('conta_a_receber') and name='id_Bandeira')
	alter table conta_a_receber alter column id_Bandeira varchar(3)
else
	ALTER TABLE Conta_a_receber ADD id_Bandeira VARCHAR(3)
	

go
if exists(select 1 from syscolumns where id =OBJECT_ID('conta_a_receber') and name='rede_cartao')
	alter table conta_a_receber alter column rede_cartao varchar(3)
else
	alter table conta_a_receber add rede_cartao varchar(3)
go
if exists(select 1 from syscolumns where id =OBJECT_ID('Lista_finalizadora') and name='id_Bandeira')
	alter table Lista_finalizadora alter column id_Bandeira varchar(3)
else
	ALTER TABLE Lista_finalizadora ADD id_Bandeira VARCHAR(3)
go

if exists(select 1 from syscolumns where id =OBJECT_ID('Lista_finalizadora') and name='rede_cartao')
	alter table Lista_finalizadora alter column rede_cartao varchar(3)
else
	ALTER TABLE Lista_finalizadora ADD rede_cartao VARCHAR(3)

go

go
if exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_saida') and name='pis_cst_saida')
	alter table pis_cst_saida alter column pis_cst_saida varchar(2)
else	
	alter table pis_cst_saida add pis_cst_saida varchar(2)
	
go

if exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_saida') and name='FILIAL')
	alter table pis_cst_saida alter column FILIAL varchar(20)
else	
	alter table pis_cst_saida add FILIAL varchar(20)
go

if (exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_saida') and name='CST'))
begin
	set quoted_identifier off
	exec ("update PIS_CST_Saida set pis_cst_saida=CST , filial='MATRIZ'" )
end
go
if exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_entrada') and name='pis_cst_entrada')
	alter table pis_cst_entrada alter column pis_cst_entrada varchar(2)
else	
	alter table pis_cst_entrada add pis_cst_entrada varchar(2)
	
go

if exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_entrada') and name='FILIAL')
	alter table pis_cst_entrada alter column FILIAL varchar(20)
else	
	alter table pis_cst_entrada add FILIAL varchar(20)
go

if exists(select 1 from syscolumns where id =OBJECT_ID('pis_cst_entrada') and name='CST')
begin
	set quoted_identifier off
	exec ("update PIS_CST_entrada set pis_cst_entrada =CST , filial='MATRIZ'")
end

go



if not exists(select 1 from PARAMETROS where PARAMETRO='BLOQ_CLIENTE_FINANCEIRO')
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
           ('BLOQ_CLIENTE_FINANCEIRO'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'BLOQUEIA CLIENTES COM STATUS DIFERENTE DE OK'
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


