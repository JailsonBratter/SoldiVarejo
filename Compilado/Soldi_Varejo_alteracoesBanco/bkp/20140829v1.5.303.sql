alter table pedido_itens 
DROP CONSTRAINT PK_Pedido_itens

alter table pedido_itens alter column pedido char(8)
go
update pedido_itens set pedido = convert(decimal,pedido)


go 

alter table mercadoria_loja add data_alteracao datetime

go
