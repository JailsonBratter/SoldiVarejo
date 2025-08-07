
update mercadoria set porcao_medida='0' , porcao_div='0',porcao_detalhe='00' where porcao_medida is null


go

IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[precificacao]') AND type in (N'U'))
begin 
Create table precificacao 
(
	filial varchar(20),
	codigo varchar(10),
	descricao varchar(40),
	data_cadastro datetime,
	usuario varchar(20),
	status varchar(1)
)
end

go

IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[precificacao]') AND type in (N'U'))
begin 

create table precificacao_itens
(
	filial varchar(20),
	codigo varchar(10),
	plu varchar(17),
	custo decimal(12,2),
	margem decimal(8,4),
	preco_anterior decimal(12,2),
	preco_novo decimal(12,2)
)
end

go 

insert into Versoes_Atualizadas select 'Versão:1.144.578', getdate();