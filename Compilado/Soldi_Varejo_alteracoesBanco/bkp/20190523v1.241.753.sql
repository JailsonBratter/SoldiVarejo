IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('item') 
            AND  UPPER(COLUMN_NAME) = UPPER('Qtde'))
begin
	alter table item alter column qtde numeric(18,4)
end
else
begin
	alter table item  add qtde numeric(18,4)
end 
go 

UPDATE ITEM SET qtde = fator_conversao
go 
update item set fator_conversao =1 



go
	insert into Versoes_Atualizadas select 'Versão:1.241.753', getdate();
GO
