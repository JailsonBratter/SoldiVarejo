update pedido_itens set ean =isnull( (select top 1 ean from ean where plu = pedido_itens.plu ),'') where ltrim(rtrim(ean)) = ''

go
insert into Versoes_Atualizadas select 'Versão:1.228.729', getdate();
GO
