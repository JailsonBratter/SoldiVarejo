if OBJECT_ID(N'Programacao_Precificacao',N'U') is null
BEGIN
	Create table Programacao_Precificacao (
		id int
		,filial varchar(20)
		,descricao varchar(40)
		,data_cadastro datetime
		,data_inicio datetime
		,usuario_cadastro varchar(30)
		,excluido int
		,usuario_excluiu varchar(30)
		,data_excluiu datetime

	)
end

if OBJECT_ID(N'Programacao_precificacao_itens',N'U') is null
BEGIN
	create table Programacao_precificacao_itens(
	
		id_precificacao int
		,plu varchar(20)
		,preco numeric(18,2)
	)
END
go
insert into Versoes_Atualizadas select 'Versão:1.303.881', getdate();