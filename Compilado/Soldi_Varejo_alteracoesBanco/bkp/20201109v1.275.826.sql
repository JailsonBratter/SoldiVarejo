IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('url_img'))
begin
	alter table mercadoria alter column url_img varchar(500)
end
else
begin
	alter table mercadoria add url_img varchar(500)
end 
go 


go 
insert into Versoes_Atualizadas select 'Versão:1.275.826', getdate();