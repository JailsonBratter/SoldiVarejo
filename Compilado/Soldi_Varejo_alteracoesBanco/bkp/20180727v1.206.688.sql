

Create table web_telas
(
	cod varchar(12),
	item varchar(100),
	ordem int
)


Create table usuarios_web_telas_permissoes
(
	COD VARCHAR(12),
	USUARIO INT,
	ITEM VARCHAR(100),
	ACESSO TINYINT
)

go 
delete from web_telas 
Declare @tela varchar(10)


	 
set @tela ='C001'
insert into web_telas 
values(@tela,'Cadastro',10),
	  (@tela,'Adicionais',20),
	  (@tela,'Bancarias',30),
	  (@tela,'Entregas',40),
	  (@tela,'Pagamentos',50),
	  (@tela,'Pet',60);
	  
set @tela ='C002'

insert into web_telas 
values(@tela,'Cadastro',10),
	  (@tela,'Contato',20),
	  (@tela,'Mercadoria',30),
	  (@tela,'Adicionais',40),
	  (@tela,'Observacao',50),
	  (@tela,'Departamentos',60);

	  

set @tela ='C003'
insert into web_telas 
values(@tela,'Cadastro',10),
	  (@tela,'Preco',20),
	  (@tela,'Itens',30),
	  (@tela,'Estoque',40),
	  (@tela,'Tributacao',50),
	  (@tela,'Observacao',60),
	  (@tela,'Vendas ECF',70),
	  (@tela,'Informacao Nutricional',80),
	  (@tela,'E-commerce',90) ;
	  
	  
set @tela ='R001'
insert into web_telas 
values(@tela,'01-Produtos',10),
	  (@tela,'02-Produtos_Listagem_Precos',20),
	  (@tela,'03-Clientes',30),
	  (@tela,'04-Fornecedores',40),
	  (@tela,'05-Fornecedores_Produtos',50),
	  (@tela,'06-Familias',60),
	  (@tela,'07-Finalizadora',70),
	  (@tela,'08-CFOP',80),
	  (@tela,'09-PDV',90),
	  (@tela,'10-Aliquota_Estado',100),
	  (@tela,'11-Linhas',110)   ;
	  
	  
set @tela ='R002'
insert into web_telas 
values(@tela,'01-Analise_de_Vendas_por_Dia',10),
	  (@tela,'02-Totais_de_Vendas',20),
	  (@tela,'03-Curva_ABC_Vendas',30),
	  (@tela,'04-Resumo_de_Vendas_Por_Operador',40),
	  (@tela,'05-Resumo_de_Vendas_Por_PDV',50),
	  (@tela,'06-Resumo_de_Vendas_Por_Produto',60),
	  (@tela,'07-Vendas_Cupom_Sintetico',70),
	  (@tela,'08-Vendas_Cupom-Item_Agregado',80),
	  (@tela,'09-Resumo_Diario_das_Finalizadora_Por_Operador',90),
	  (@tela,'10-Resumo_de_Operadores_Por_Hora',100),
	  (@tela,'11-Resumo_de_Vendas_Por_Grupo',110),     
	  (@tela,'12-Vendas_Pedidos_Simplificado',120),   
	  (@tela,'13-Vendas_Notas_Fiscais',130),   
	  (@tela,'14-Vendas_Pedidos_Analitico',140),   
	  (@tela,'15-Vendas_Pedidos_Cliente',150),   
	  (@tela,'16-Vendas_Por_Aliquota',160),   
	  (@tela,'17-Vendas_Por_Finalizadora',170),   
	  (@tela,'18-Vendas_Por_Vendedor',180),   
	  (@tela,'19-Analise_de_Vendas_por_Mes',190),   
	  (@tela,'20-Vendas_Por_Cliente',120),   
	  (@tela,'21-Analise_de_Vendas_por_Hora',121);
	
	  
	  
set @tela ='R003'
insert into web_telas 
values(@tela,'01-Demonstrativo_Financeiro',10),
	  (@tela,'02-Fluxo_Caixa',20),
	  (@tela,'03-Fluxo_Caixa_Simplificado',30),
	  (@tela,'04-Contas_a_Pagar',40),
	  (@tela,'05-Contas_a_Pagar_Simplificado',50),
	  (@tela,'06-Contas_a_Receber',60),
	  (@tela,'07-Contas_a_Receber_Simplificado',70),
	  (@tela,'08-Extrato_de_Cartoes',80),
	  (@tela,'09-Caderneta',90),
	  (@tela,'10-Consulta_Posicao_de_Cliente',100),
	  (@tela,'11-Recebimento_Simplificado',110),     
	  (@tela,'12-Ficha_Cliente',120),   
	  (@tela,'13-Extrato_Contas_a_Pagar',130),   
	  (@tela,'14-Fechamento_Fornecedor',140);
	 

	  
set @tela ='R004'
insert into web_telas 
values(@tela,'01-NotaFiscal',10),
	  (@tela,'02-MapaResumo',20),
	  (@tela,'03-Produtos_imposto_cadastrado',30);
	  


set @tela ='R005'
insert into web_telas 
values(@tela,'01-Produtos_Estoque',10),
	  (@tela,'02-Outras_Movimentacoes',20),
	  (@tela,'03-Historico_Entrada_Saida_Analitico',30),
	  (@tela,'04-Historico_Entrada_Saida_Consolidado',40);
	  


	  

set @tela ='R007'
insert into web_telas 
values(@tela,'01-Comanda_Historico',10)
	  
go
	  
	
insert into usuarios_web_telas_permissoes
Select distinct tela.cod,usuario.id,tela.item,1 from web_telas as tela left join usuarios_web as usuario on ''= ''
order by usuario.id


go 	

	  
	  
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('cst_pis_cofins'))
begin
	alter table natureza_operacao alter column cst_pis_cofins varchar(2)
end
else
begin
	alter table natureza_operacao add cst_pis_cofins varchar(2)
end 
go 



 
 
 go 
 
 
 
 

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('usuarios_web') 
            AND  UPPER(COLUMN_NAME) = UPPER('senha_cry'))
begin
	alter table usuarios_web add senha_cry varbinary(128);
    
end
go 


IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE UPPER(TABLE_NAME) = UPPER('usuarios_web') 
		AND  UPPER(COLUMN_NAME) = UPPER('SENHA') 
		AND UPPER(DATA_TYPE)=UPPER('varbinary')
)
begin 

	update usuarios_web set senha_cry = pwdEncrypt(senha);
	alter table usuarios_web drop column senha;
	alter table usuarios_web add senha varbinary(128);
	update usuarios_web set senha = senha_cry ;
	
END

go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('usuarios_web') 
            AND  UPPER(COLUMN_NAME) = UPPER('senha_cry'))
begin
	alter table usuarios_web drop column senha_cry
    
end


insert into Versoes_Atualizadas select 'Versão:1.206.688', getdate();
GO
