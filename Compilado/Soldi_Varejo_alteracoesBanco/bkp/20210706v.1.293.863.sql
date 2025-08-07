go 
IF OBJECT_ID (N'solicitacao_producao_pedidos', N'U') IS NULL
begin
	Create table solicitacao_producao_pedidos
	(
		filial  varchar(40),
		codigo varchar(20),
		pedido varchar(8),
		Filial_produzir varchar(40),
		tipo_producao tinyint
	)
end
go 

insert into Versoes_Atualizadas select 'Versão:1.293.863', getdate();