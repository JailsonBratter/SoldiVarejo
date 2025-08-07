IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('centro_custo') 
            AND  UPPER(COLUMN_NAME) = UPPER('conta_contabil_credito'))
begin
	alter table centro_custo alter column conta_contabil_credito varchar(20)
	alter table centro_custo alter column conta_contabil_debito varchar(20)
end
else
begin
	alter table centro_custo add conta_contabil_credito varchar(20)
	alter table centro_custo add conta_contabil_debito varchar(20)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('conta_contabil_credito'))
begin
	alter table cliente alter column conta_contabil_credito varchar(20)
	alter table cliente alter column conta_contabil_debito varchar(20)
end
else
begin
	alter table cliente add conta_contabil_credito varchar(20)
	alter table cliente add conta_contabil_debito varchar(20)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('conta_contabil_credito'))
begin
	alter table fornecedor alter column conta_contabil_credito varchar(20)
	alter table fornecedor alter column conta_contabil_debito varchar(20)
end
else
begin
	alter table fornecedor add conta_contabil_credito varchar(20)
	alter table fornecedor add conta_contabil_debito varchar(20)
end 
go 




go 
insert into Versoes_Atualizadas select 'Versão:1.273.823', getdate();