IF not EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente_pet') 
            AND  UPPER(COLUMN_NAME) = UPPER('id'))
begin
	alter table cliente_pet add id int  identity
end

go 
update cliente_pet set codigo_pet = Codigo_Cliente +  convert(varchar,Id) where codigo_pet is null

go 

update Cliente_Pet_Vacina 
set codigo_pet = (Select top 1 codigo_pet 
				  from Cliente_Pet 
				  where ltrim(rtrim(cliente_Pet_Vacina.Codigo_Cliente)) = ltrim(rtrim(Cliente_pet.Codigo_Cliente))
				       and ltrim(rtrim(Cliente_Pet_Vacina.Nome_Pet)) = ltrim(rtrim(Cliente_pet.Nome_Pet)))
where codigo_pet is null

go 
insert into Versoes_Atualizadas select 'Versão:1.259.801', getdate();