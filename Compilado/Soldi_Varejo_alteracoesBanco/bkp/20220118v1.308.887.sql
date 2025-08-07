

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Preco_Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_arredondamento'))
begin
	alter table Preco_mercadoria alter column tipo_arredondamento tinyint 
end
else
begin
	alter table Preco_mercadoria add tipo_arredondamento tinyint default 1
end 
go 

update preco_mercadoria set tipo_arredondamento = 1 

insert into Versoes_Atualizadas select 'Versão:1.308.887', getdate();