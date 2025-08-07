
-- nfe 4.0
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('indEscala'))
begin
	alter table mercadoria alter column indEscala varchar(1)
end
else
begin
	alter table mercadoria add indEscala varchar(1)
end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('cBenef'))
begin
	alter table mercadoria alter column cBenef varchar(10)
end
else
begin
	alter table mercadoria add cBenef varchar(10)
end 


go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('cnpj_Fabricante'))
begin
	alter table mercadoria alter column cnpj_Fabricante varchar(18)
end
else
begin
	alter table mercadoria add cnpj_Fabricante varchar(18)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('indEscala'))
begin
	alter table nf_item alter column indEscala varchar(1)
end
else
begin
	alter table nf_item add indEscala varchar(1)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('cnpj_Fabricante'))
begin
	alter table nf_item alter column cnpj_Fabricante varchar(18)
end
else
begin
	alter table nf_item add cnpj_Fabricante varchar(18)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('cBenef'))
begin
	alter table nf_item alter column cBenef varchar(10)
end
else
begin
	alter table nf_item add cBenef varchar(10)
end 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('vBCFCP'))
begin
	alter table nf_item alter column vBCFCP numeric(13,2)
	alter table nf_item alter column pFCP numeric(13,4)
	alter table nf_item alter column vFCP numeric(13,4)
	alter table nf_item alter column vBCFCPST numeric(13,4)
	alter table nf_item alter column pFCPST numeric(13,4)
	alter table nf_item alter column vFCPST numeric(13,4)
end
else
begin
	alter table nf_item add vBCFCP numeric(13,2)
	alter table nf_item add pFCP numeric(13,4)
	alter table nf_item add vFCP numeric(13,4)
	alter table nf_item add vBCFCPST numeric(13,4)
	alter table nf_item add pFCPST numeric(13,4)
	alter table nf_item add vFCPST numeric(13,4)
end 

go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('vFCP'))
begin
	
	alter table nf alter column vFCP numeric(13,4)
	alter table nf alter column vFCPST numeric(13,4)
end
else
begin
	alter table nf add vFCP numeric(13,4)
	alter table nf add vFCPST numeric(13,4)
end 

go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('tPag'))
begin
	alter table nf alter column tPag varchar(2)
end
else
begin
	alter table nf add tPag varchar(2)
end 
go 



update mercadoria set indEscala= 'S' 

go 


 GO 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('vIPIDevol'))
begin
	alter table nf_item alter column vIPIDevol numeric(18,2)
end
else
begin
	alter table nf_item add  vIPIDevol numeric(18,2)
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('pDevol'))
begin
	alter table nf_item alter column pDevol numeric(18,2)
end
else
begin
	alter table nf_item add  pDevol numeric(18,2)
end 
go 

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('vIPIDevol'))
begin
	alter table nf alter column vIPIDevol numeric(18,2)
end
else
begin
	alter table nf add  vIPIDevol numeric(18,2)
end 
go 


/****** Object:  Table [dbo].[uf_pobreza]    Script Date: 06/13/2018 12:44:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uf_pobreza]') AND type in (N'U'))
DROP TABLE [dbo].[uf_pobreza]
GO
CREATE TABLE [dbo].[uf_pobreza](
	[uf] [varchar](2) NULL,
	[porc] [numeric](12, 2) NULL
) ON [PRIMARY]

 
 go 
 
 
if not exists (Select * from uf_pobreza )
begin 
	insert into uf_pobreza values 
	('RJ',2.0),
	('AL',1.0),
	('PI',1.0)
end 
 go 


 
 
 go 

insert into Versoes_Atualizadas select 'Versão:1.204.680', getdate();
GO
