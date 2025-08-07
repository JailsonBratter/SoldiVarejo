alter table mercadoria add 
	porcao numeric (12,2),
	porcao_medida varchar(2),
	porcao_numero numeric(12,2),
	porcao_div varchar(3),
	porcao_detalhe varchar(30),
	vlr_energ_nao tinyint,
	vlr_energ_qtde numeric(12,2),
	vlr_energ_qtde_igual numeric(12,2),
	vlr_energ_diario numeric(12,2),
	carboidratos_nao tinyint,
	carboidratos_qtde numeric(12,2),
	carboidratos_vlr_diario numeric(12,2),
	proteinas_nao tinyint,
	proteinas_qtde numeric(12,2),
	proteinas_vlr_diario numeric(12,2),
	gorduras_totais_nao tinyint,
	gorduras_totais_qtde numeric(12,2),
	gorduras_totais_vlr_diario numeric(12,2),
	gorduras_satu_nao tinyint,
	gorduras_satu_qtde numeric(12,2),
	gorduras_satu_vlr_diario numeric(12,2),
	gorduras_trans_nao tinyint,
	gorduras_trans_qtde numeric(12,2),
	fibra_alimen_nao tinyint,
	fibra_alimen_qtde  numeric(12,2),
	fibra_alimen_vlr_diario numeric(12,2),
	sodio_nao tinyint,
	sodio_qtde numeric(12,2),
	sodio_vlr_diario numeric(12,2)
	
	
	go
	
	
alter table cliente add indIEDest int
go
alter table fornecedor add indIEDest int
go

update cliente set indIEDest =9 where isnull(Pessoa_Juridica,0) = 0;
update cliente set indIEDest = 9 where isnull(Pessoa_Juridica,0) = 1 and IE = '' ;
update cliente set indIEDest = 1 where isnull(Pessoa_Juridica,0) = 1 and IE <> '' ;
	
go 
	
	insert into Versoes_Atualizadas select 'Versão:1.142.575', getdate();