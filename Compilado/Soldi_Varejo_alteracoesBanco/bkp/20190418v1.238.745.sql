
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('und_producao'))
begin
	alter table mercadoria alter column und_producao varchar(2)
end
else
begin
	alter table mercadoria add  und_producao varchar(2)
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('informacoes_extras'))
begin
	alter table mercadoria alter column informacoes_extras nvarchar(max)
end
else
begin
	alter table mercadoria add  informacoes_extras nvarchar(max)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('plu_receita'))
begin
	alter table mercadoria alter column plu_receita varchar(20)
end
else
begin
	alter table mercadoria add  plu_receita varchar(20)
end 
GO 

UPDATE MERCADORIA SET plu_receita = PLU WHERE plu_receita IS NULL

GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('prog_seg'))
begin
	alter table mercadoria alter column prog_seg tinyint
	alter table mercadoria alter column prog_ter tinyint
	alter table mercadoria alter column prog_qua tinyint
	alter table mercadoria alter column prog_qui tinyint
	alter table mercadoria alter column prog_sex tinyint
	alter table mercadoria alter column prog_sab tinyint
	alter table mercadoria alter column prog_dom tinyint
end
else
begin
	alter table mercadoria add prog_seg tinyint
	alter table mercadoria add prog_ter tinyint
	alter table mercadoria add prog_qua tinyint
	alter table mercadoria add prog_qui tinyint
	alter table mercadoria add prog_sex tinyint
	alter table mercadoria add prog_sab tinyint
	alter table mercadoria add prog_dom tinyint
end 
go 

go 

GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cotacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('Descricao'))
begin
	alter table Cotacao alter column Descricao varchar(100)
	
end
else
begin
	alter table Cotacao add  Descricao varchar(100)
end 
go 
CREATE TABLE [dbo].[solicitacao_producao](
	[filial] [varchar](20) NULL,
	[codigo] [varchar](20) NULL,
	[descricao] [varchar](80) NULL,
	[data_cadastro] [datetime] NULL,
	[usuario_cadastro] [varchar](40) NULL,
	[status] [varchar](40) NULL
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[solicitacao_producao_itens](
	[filial] [varchar](20) NULL,
	[codigo] [varchar](20) NULL,
	[plu] [varchar](17) NULL,
	[ean] [varchar](20) NULL,
	[ref_fornecedor] [varchar](8) NULL,
	[descricao] [varchar](80) NULL,
	[und] [varchar](3) NULL,
	[qtde] [numeric](12, 2) NULL,
	[ordem] [int] NULL
) ON [PRIMARY]
GO




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('peso_receita_unitario'))
begin
	alter table mercadoria alter column peso_receita_unitario numeric(18,2)
end
else
begin
	alter table mercadoria add  peso_receita_unitario numeric(18,2)
end 
go 



if not exists(select 1 from SEQUENCIAIS where TABELA_COLUNA='SOLICITACAO_PRODUCAO.CODIGO')
begin	
	
	INSERT INTO [dbo].[SEQUENCIAIS]
		 VALUES
			   (
			'SOLICITACAO_PRODUCAO.CODIGO'		--<TABELA_COLUNA, varchar(37),>
			,'SEQUENCIA SOLICITACAO DE PRODUCAO'--,<DESCRICAO, varchar(40),>
			,'0'								--,<SEQUENCIA, varchar(20),>
			,10									--,<TAMANHO, int,>
			,''									--,<OBS1, char(255),>
			,''									--,<OBS2, char(255),>
			,''									--,<OBS3, char(255),>
			,''									--,<OBS4, char(255),>
			,''									--,<OBS5, char(255),>
			,''									--,<OBS6, char(255),>
			,''									--,<OBS7, char(255),>
			,''									--,<OBS8, char(60),>
			,GETDATE()							--,<DATA_PARA_TRANSFERENCIA, datetime,>
			,0									--,<PERMITE_POR_EMPRESA, bit,>
			  );
end
GO







IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('filial_produzido'))
begin
	alter table mercadoria alter column filial_produzido varchar(20)
end
else
begin
	alter table mercadoria add filial_produzido varchar(20)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_producao'))
begin
	alter table mercadoria alter column tipo_producao varchar(20)
end
else
begin
	alter table mercadoria add tipo_producao varchar(20)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('filial') 
            AND  UPPER(COLUMN_NAME) = UPPER('produtora'))
begin
	alter table filial alter column produtora tinyint
end
else
begin
	alter table filial add produtora tinyint
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('solicitacao_producao') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_producao'))
begin
	alter table solicitacao_producao alter column tipo_producao tinyint
end
else
begin
	alter table solicitacao_producao add tipo_producao tinyint
end 
go 




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('solicitacao_producao_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('obs'))
begin
	alter table solicitacao_producao_itens alter column obs nvarchar(500)
end
else
begin
	alter table solicitacao_producao_itens add obs nvarchar(500)
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('solicitacao_producao_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_producao'))
begin
	alter table solicitacao_producao_itens alter column tipo_producao tinyint
end
else
begin
	alter table solicitacao_producao_itens add tipo_producao tinyint
end 
go 


if not exists(select 1 from SEQUENCIAIS where TABELA_COLUNA='SOLICITACAO_ENCOMENDA.CODIGO')
begin	
	
	INSERT INTO [dbo].[SEQUENCIAIS]
		 VALUES
			   (
			'SOLICITACAO_ENCOMENDA.CODIGO'		--<TABELA_COLUNA, varchar(37),>
			,'SEQUENCIA SOLICITACAO DE ENCOMENDA'--,<DESCRICAO, varchar(40),>
			,'0'								--,<SEQUENCIA, varchar(20),>
			,10									--,<TAMANHO, int,>
			,''									--,<OBS1, char(255),>
			,''									--,<OBS2, char(255),>
			,''									--,<OBS3, char(255),>
			,''									--,<OBS4, char(255),>
			,''									--,<OBS5, char(255),>
			,''									--,<OBS6, char(255),>
			,''									--,<OBS7, char(255),>
			,''									--,<OBS8, char(60),>
			,GETDATE()							--,<DATA_PARA_TRANSFERENCIA, datetime,>
			,0									--,<PERMITE_POR_EMPRESA, bit,>
			  );
end
GO




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('filial') 
            AND  UPPER(COLUMN_NAME) = UPPER('Chave_LIUV'))
begin
	alter table filial alter column Chave_LIUV varchar(50)
end
else
begin
	alter table filial add Chave_LIUV varchar(50)
end 
go 




go
	insert into Versoes_Atualizadas select 'Versão:1.238.745', getdate();
GO
