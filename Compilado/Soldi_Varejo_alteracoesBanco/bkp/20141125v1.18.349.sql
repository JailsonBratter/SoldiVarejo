
IF ( COL_LENGTH( 'Inventario', 'tipoMovimentacao' ) IS NOT NULL )
BEGIN

alter table inventario alter column tipoMovimentacao varchar(20)

END
ELSE
BEGIN

alter table inventario add tipoMovimentacao varchar(20)
END


go

alter table inventario_itens add custo decimal 

GO 

insert into Tipo_movimentacao values ('INVENTARIO',2,0,0)
