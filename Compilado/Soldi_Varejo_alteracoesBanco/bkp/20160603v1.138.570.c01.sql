


IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('Preco_Venda'))
begin
	alter table Natureza_operacao add Preco_Venda tinyint ;
	
	update Natureza_operacao set Preco_Venda = 1 where Saida =1;
	
	update Natureza_operacao set Preco_Venda = 0 where Saida =0;
	
	update Natureza_operacao set Preco_Venda = 0 where Codigo_operacao ='5927';
	
end 
go

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_corrente') 
            AND  UPPER(COLUMN_NAME) = UPPER('filial'))
begin
	alter table conta_corrente add filial varchar(20);
	
	update Conta_Corrente set filial ='MATRIZ';	
	
end 
go
IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_corrente') 
            AND  UPPER(COLUMN_NAME) = UPPER('conta_caixa'))
begin
	alter table conta_corrente add conta_caixa tinyint	
end 
