create table promocaoJornal (
	filial varchar(20),
	codigo integer,
	descricao varchar(60),
	inicio  datetime,
	fim  datetime
)
	
create table promocaoJornalItens(
	filial varchar(20),
	ordem integer,
	codigo_jornal integer,
	plu varchar(20),
	descricao varchar(60),
	preco numeric(14,2),
	preco_promocao numeric(14,2)
)




insert into Versoes_Atualizadas select 'Versão:1.284.845', getdate();