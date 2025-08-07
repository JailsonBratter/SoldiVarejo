alter table filial add chave_senha varchar(255)
go
update filial set chave_senha ='ISdSj88KlL7dlL65oj280Ooj7FrIlL659J282a4XC5tr9J9JlLC5l4IS75'
go

create table aliquota_imp_estado(
uf  varchar (2),
ncm varchar(12),
icms_interestadual decimal(5,2),
iva_ajustado decimal(5,2),
icms_estado decimal(5,2),
CST VARCHAR(5))

go 



alter table mercadoria add Origem int

go


alter table nf_item add aliquota_iva decimal(5,2),indice_st varchar(5)
go 
alter table nf_item_log add aliquota_iva decimal(5,2),indice_st varchar(5)

go

alter table familia add preco numeric(5,2), qtd_etiquetas  int, imprime_etiqueta  tinyint 
go


go
alter table fornecedor alter column Cidade varchar (60)
go
alter table cliente alter column Cidade varchar (60)
go


alter table nf add precoTodasFiliais tinyint
go