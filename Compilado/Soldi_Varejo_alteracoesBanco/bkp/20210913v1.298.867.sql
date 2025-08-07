IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('solicitacao_producao_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('preco_venda'))
begin
	alter table solicitacao_producao_itens alter column preco_venda numeric(18,3)
end
else
begin
	alter table solicitacao_producao_itens add preco_venda numeric(18,3)
end 
go 

GO
insert into Versoes_Atualizadas select 'Versão:1.298.867', getdate();