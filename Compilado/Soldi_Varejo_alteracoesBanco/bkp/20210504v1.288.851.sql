IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('codigo_centro_custo'))
begin
	alter table nf_item alter column codigo_centro_custo varchar(20);
end
else
begin
	alter table nf_item add codigo_centro_custo varchar(20)
end 

go 

IF OBJECT_ID (N'NF_CentroCusto', N'U') IS NULL
begin
	Create table NF_CentroCusto (
		filial varchar(20), 
		codigo varchar(20), 
		cliente_fornecedor varchar(20), 
		tipo_NF tinyint, 
		serie int,
		data datetime, 
		codigo_centro_custo varchar(20) ,
		valor decimal(18,2)
	);	

end 

go 

insert into Versoes_Atualizadas select 'Versão:1.288.851', getdate();
