
update PARAMETROS set TIPO_DADO ='L' 
where VALOR_ATUAL  IN('TRUE','FALSE') 

go 

update PARAMETROS set TIPO_DADO ='C' 
	where TIPO_DADO ='' or TIPO_DADO is null


go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('cfop'))
begin
	alter table natureza_operacao alter column cfop varchar(4)
end
else
begin
	alter table natureza_operacao add cfop varchar(4)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuarioAlteracao'))
begin
	alter table Mercadoria alter column usuarioAlteracao varchar(20)
end
else
begin
	alter table Mercadoria add usuarioAlteracao varchar(20)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuario'))
begin
	alter table Cliente alter column usuario varchar(20)
end
else
begin
	alter table Cliente add usuario varchar(20)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuarioAlteracao'))
begin
	alter table Cliente alter column usuarioAlteracao varchar(20)
end
else
begin
	alter table Cliente add usuarioAlteracao varchar(20)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuario'))
begin
	alter table Fornecedor alter column usuario varchar(20)
end
else
begin
	alter table Fornecedor add usuario varchar(20)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuarioAlteracao'))
begin
	alter table Fornecedor alter column usuarioAlteracao varchar(20)
end
else
begin
	alter table Fornecedor add usuarioAlteracao varchar(20)
end 
go 

insert into Versoes_Atualizadas select 'Versão:1.269.816', getdate();