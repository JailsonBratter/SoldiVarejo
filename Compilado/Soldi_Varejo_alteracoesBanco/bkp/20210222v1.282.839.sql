
create table CFOPEntra_Saida(
	cfop_entrada varchar(4),
	cfop_saida varchar(4),
	Descricao varchar(250)
)

insert into Versoes_Atualizadas select 'Versão:1.282.839', getdate();
