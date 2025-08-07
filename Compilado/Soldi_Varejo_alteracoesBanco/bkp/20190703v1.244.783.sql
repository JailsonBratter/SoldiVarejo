IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor_mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('importado_nf'))
begin
	alter table fornecedor_mercadoria alter column importado_nf tinyint
end
else
begin
	alter table fornecedor_mercadoria add importado_nf tinyint
end 
go 


go 

	insert into Versoes_Atualizadas select 'Versão:1.244.783', getdate();
GO
