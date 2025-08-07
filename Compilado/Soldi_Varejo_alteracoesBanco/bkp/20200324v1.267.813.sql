IF OBJECT_ID (N'Embaladoras', N'U') IS NULL 
begin
Create table Embaladoras(
	id int,
	Filial varchar(40),
	Descricao varchar(50),
	End_FTP varchar(200),
	usuario varchar(50),
	senha varchar(40)
)
end
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_ecommercer'))
begin
	alter table mercadoria alter column id_ecommercer varchar(20)
end
else
begin
	alter table mercadoria add id_ecommercer varchar(20)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Excluir_proxima_integracao'))
begin
	alter table mercadoria alter column Excluir_proxima_integracao tinyint
end
else
begin
	alter table mercadoria add Excluir_proxima_integracao tinyint
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('bandeja'))
begin
	alter table mercadoria alter column bandeja int
end
else
begin
	alter table mercadoria add bandeja int
end 
go 


Create table bandeja (
id int,
descricao varchar(30)
)

insert into bandeja values
(0,''),
(1,'B1'),
(2,'B2 FUNDA'),
(3,'B3 FUNDA'),
(4,'B3 RASA'),
(5,'B2 BANDEJA'),
(6,'M06 VERDE')


GO 
insert into Versoes_Atualizadas select 'Versão:1.267.813', getdate();

