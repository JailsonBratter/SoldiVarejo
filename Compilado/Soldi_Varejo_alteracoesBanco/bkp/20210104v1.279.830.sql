IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('vCredicmssn'))
begin
	alter table nf alter column vCredicmssn numeric(10,2)
end
else
begin
	alter table nf add vCredicmssn numeric(10,2)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('desconto_valor'))
begin
	alter table nf_item alter column desconto_valor numeric(10,4)
end
else
begin
	alter table nf_item add desconto_valor numeric(10,4)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('despesas_base'))
begin
	alter table fornecedor alter column despesas_base tinyint
	alter table fornecedor alter column ipi_base tinyint
end
else
begin
	alter table fornecedor add despesas_base tinyint
	alter table fornecedor add ipi_base tinyint
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('CancelamentoAutorizador'))
begin
	alter table saida_estoque alter column CancelamentoAutorizador varchar(40)
end
else
begin
	alter table saida_estoque add CancelamentoAutorizador varchar(40)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('MotivoCancelamento'))
begin
	alter table saida_estoque alter column MotivoCancelamento varchar(30)
end
else
begin
	alter table saida_estoque add MotivoCancelamento varchar(30)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Entregador'))
begin
	alter table Pedido alter column Entregador varchar(20)
end
else
begin
	alter table Pedido add Entregador varchar(20)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Operador'))
begin
	alter table Pedido alter column Operador int
end
else
begin
	alter table Pedido add Operador int
end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Data_Movimento'))
begin
	alter table Pedido alter column Data_Movimento datetime
end
else
begin
	alter table Pedido add Data_Movimento datetime
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Origem'))
begin
	alter table Pedido alter column Origem varchar(3)
end
else
begin
	alter table Pedido add Origem varchar(3)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Id_Movimento'))
begin
	alter table Pedido alter column Id_Movimento varchar(3)
end
else
begin
	alter table Pedido add Id_Movimento varchar(3)
end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('CodVendedor'))
begin
	alter table Pedido alter column CodVendedor Int
end
else
begin
	alter table Pedido add CodVendedor Int
end 
go


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Documento'))
begin
	alter table Pedido alter column Documento varchar(20)
end
else
begin
	alter table Pedido add Documento varchar(20)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Caixa_Saida'))
begin
	alter table Pedido alter column Caixa_Saida int
end
else
begin
	alter table Pedido add Caixa_Saida int
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('ComandaPedido'))
begin
	alter table Pedido alter column ComandaPedido int
end
else
begin
	alter table Pedido add ComandaPedido int
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Lugar_Entrega'))
begin
	alter table pedido alter column Lugar_Entrega Varchar(20)
end
else
begin
	alter table pedido add Lugar_Entrega Varchar(20)
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Taxas'))
begin
	alter table pedido alter column Taxas  Decimal(12,2)
end
else
begin
	alter table pedido add Taxas  Decimal(12,2)
end 
go
/****** Object:  Index [PK_pedido_itens_obs]    Script Date: 01/08/2021 15:48:25 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[pedido_itens_obs]') AND name = N'PK_pedido_itens_obs')
ALTER TABLE [dbo].[pedido_itens_obs] DROP CONSTRAINT [PK_pedido_itens_obs]
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens_obs') 
            AND  UPPER(COLUMN_NAME) = UPPER('obs'))
begin
	alter table pedido_itens_obs alter column obs varchar(40) NOT NULL
end
else
begin
	alter table pedido_itens_obs add obs varchar(40) DEFAULT '' NOT NULL
end 
GO

/****** Object:  Index [PK_pedido_itens_obs]    Script Date: 01/08/2021 16:00:29 ******/
Alter TABLE [dbo].[pedido_itens_obs] ADD CONSTRAINT [PK_pedido_itens_obs] PRIMARY KEY CLUSTERED 
(
	[filial] ASC,
	[pedido] ASC,
	[plu] ASC,
	[id] ASC,
	[obs] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('ID_Fracao'))
begin
	alter table pedido_itens alter column ID_Fracao Tinyint
end
else
begin
	alter table pedido_itens add ID_Fracao Tinyint
end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('Valida_Cliente'))
begin
	alter table Finalizadora alter column Valida_Cliente  Tinyint
end
else
begin
	alter table Finalizadora add Valida_Cliente  Tinyint
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('Checa_Limite'))
begin
	alter table Finalizadora alter column Checa_Limite  Tinyint
end
else
begin
	alter table Finalizadora add Checa_Limite  Tinyint
end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('Taxa_Entrega'))
begin
	alter table Cliente alter column Taxa_Entrega decimal(12,2)
END
ELSE
begin
	alter table Cliente add Taxa_Entrega decimal(12,2)
end 

GO
/****** Object:  StoredProcedure [dbo].[sp_cons_cliente_codigo_Delivery]    Script Date: 12/15/2020 13:52:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sp_cons_cliente_codigo_Delivery]
 
                @CodigoCliente VarChar(15)
AS
-- sp_cons_cliente_codigo_Delivery '11996217587'
Begin
      DECLARE @NF varchar(5000)
	IF LEN(@CodigoCliente) > 8
		Begin
			IF (SELECT COUNT(*) FROM Cliente WHERE REPLACE(REPLACE(REPLACE(Cliente.CNPJ, '.', ''),'-',''),'/','') = @CodigoCliente) > 0
				BEGIN
					SET @NF = 'SELECT TOP 1 '
					SET @NF = @nf +' Codigo_Cliente = RTRIM(LTRIM(Cliente.Codigo_Cliente)), '
					SET @NF = @nf +' Nome_CLIENTE = RTRIM(LTRIM(Cliente.Nome_Cliente)), '
					SET @NF = @nf +' CNPJ = Cliente.CNPJ, '
					SET @NF = @nf +' Fone = (Select Top 1 RTRIM(LTRIM(Replace(Replace(Replace(replace(ID_Meio_Comunicacao, '+CHAR(39)+ '-' + CHAR(39) + ',' + CHAR(39)+CHAR(39)+  '),' + '' +CHAR(39)+ '(' +CHAR(39) + ',' + CHAR(39)+ CHAR(39)+  '),' + CHAR(39)+ ')' + CHAR(39) + ',' +' '+CHAR(39)+ CHAR(39) + '), ' + CHAR(39) + ' ' + CHAR(39) + ', ' + CHAR(39)+CHAR(39) + '))) From Cliente_Contato Where Codigo_Cliente = RTRIM(LTRIM(Cliente.Codigo_Cliente))), '
 					SET @NF = @nf +' Endereco = RTRIM(LTRIM(Cliente.Endereco)), '
 					SET @NF = @nf +' Endereco_Nro = RTRIM(LTRIM(Cliente.Endereco_Nro)), '
					SET @NF = @NF + ' Complemento_End,'
 					SET @NF = @nf +' Bairro = RTRIM(LTRIM(Cliente.Bairro)), '
 					SET @NF = @nf +' Cidade = RTRIM(LTRIM(Cliente.Cidade)), '
 					SET @NF = @nf +' UF = RTRIM(LTRIM(Cliente.UF)), '
 					SET @NF = @nf +' CEP = RTRIM(LTRIM(Cliente.CEP)), '
 					SET @NF = @nf +' Limite_Credito = Cliente.Limite_Credito, '
 					SET @NF = @nf +' Utilizado = Cliente.Utilizado, Codigo_Tabela = 0 '
					SET @NF = @NF + ', Taxa_Entrega'
					SET @NF = @nf +' FROM CLIENTE '
					SET @NF = @nf +' Where '
					SET @NF = @nf +' Replace(Replace(replace(Cliente.CNPJ, '+CHAR(39)+ '-' + CHAR(39) + ',' + CHAR(39)+CHAR(39)+  '),' + '' +CHAR(39)+ '.' +CHAR(39) + ',' + CHAR(39)+ CHAR(39)+  '),' + CHAR(39)+ '/' + CHAR(39) + ',' +' '+CHAR(39)+ CHAR(39) + ') like '+CHAR(39)+'%'+ @CodigoCliente +'%' +CHAR(39) 
				END
			ELSE
				BEGIN
					SET @NF = 'SELECT TOP 1 '
					SET @NF = @nf +' Codigo_Cliente = RTRIM(LTRIM(Cliente.Codigo_Cliente)), '
					SET @NF = @nf +' Nome_CLIENTE = RTRIM(LTRIM(Cliente.Nome_Cliente)), '
					SET @NF = @nf +' CNPJ = Cliente.CNPJ, '
					SET @NF = @nf +' Fone = RTRIM(LTRIM(Replace(Replace(Replace(replace(ID_Meio_Comunicacao, '+CHAR(39)+ '-' + CHAR(39) + ',' + CHAR(39)+CHAR(39)+  '),' + '' +CHAR(39)+ '(' +CHAR(39) + ',' + CHAR(39)+ CHAR(39)+  '),' + CHAR(39)+ ')' + CHAR(39) + ',' +' '+CHAR(39)+ CHAR(39) + '), ' + CHAR(39) + ' ' + CHAR(39) + ', ' + CHAR(39)+CHAR(39) + '))) ,'
 					SET @NF = @nf +' Endereco = RTRIM(LTRIM(Cliente.Endereco)), '
 					SET @NF = @nf +' Endereco_Nro = RTRIM(LTRIM(Cliente.Endereco_Nro)), '
					SET @NF = @NF + ' Complemento_End,'
 					SET @NF = @nf +' Bairro = RTRIM(LTRIM(Cliente.Bairro)), '
 					SET @NF = @nf +' Cidade = RTRIM(LTRIM(Cliente.Cidade)), '
 					SET @NF = @nf +' UF = RTRIM(LTRIM(Cliente.UF)), '
 					SET @NF = @nf +' CEP = RTRIM(LTRIM(Cliente.CEP)), '
 					SET @NF = @nf +' Limite_Credito = Cliente.Limite_Credito, '
 					SET @NF = @nf +' Utilizado = Cliente.Utilizado, Codigo_Tabela = 0 '
					SET @NF = @NF + ', Taxa_Entrega'
					SET @NF = @nf +' FROM CLIENTE Inner Join Cliente_contato cc on Cliente.Codigo_Cliente = cc.Codigo_Cliente '
					SET @NF = @nf +' LEFT OUTER JOIN Tabela_Preco ON Cliente.Codigo_Tabela = Tabela_Preco.Codigo_Tabela '
					SET @NF = @nf +' WHERE Cliente.Codigo_Cliente <> ' + CHAR(39) + CHAR(39)
					SET @NF = @nf +' And RTRIM(LTRIM(Replace(Replace(Replace(replace(cc.ID_Meio_Comunicacao, '+CHAR(39)+ '-' + CHAR(39) + ',' + CHAR(39)+CHAR(39)+  '),' + '' +CHAR(39)+ '(' +CHAR(39) + ',' + CHAR(39)+ CHAR(39)+  '),' + CHAR(39)+ ')' + CHAR(39) + ',' +' '+CHAR(39)+ CHAR(39) + '), ' + CHAR(39) + ' ' + CHAR(39) + ', ' + CHAR(39)+CHAR(39) + '))) like '+CHAR(39)+'%'+ @CodigoCliente +'%' +CHAR(39) 
				END
		END
	ELSE
		Begin	
			SET @NF = 'SELECT TOP 1 '
			SET @NF = @nf +' Codigo_Cliente = RTRIM(LTRIM(Cliente.Codigo_Cliente)), '
			SET @NF = @nf +' Nome_CLIENTE = RTRIM(LTRIM(Cliente.Nome_Cliente)), '
			SET @NF = @nf +' CNPJ = Cliente.CNPJ, '
			SET @NF = @nf +' Fone = (Select Top 1 RTRIM(LTRIM(Replace(Replace(Replace(replace(ID_Meio_Comunicacao, '+CHAR(39)+ '-' + CHAR(39) + ',' + CHAR(39)+CHAR(39)+  '),' + '' +CHAR(39)+ '(' +CHAR(39) + ',' + CHAR(39)+ CHAR(39)+  '),' + CHAR(39)+ ')' + CHAR(39) + ',' +' '+CHAR(39)+ CHAR(39) + '), ' + CHAR(39) + ' ' + CHAR(39) + ', ' + CHAR(39)+CHAR(39) + '))) From Cliente_Contato Where Codigo_Cliente like '+CHAR(39)+'%'+ @CodigoCliente +'%' +CHAR(39) +'), '
 			SET @NF = @nf +' Endereco = RTRIM(LTRIM(Cliente.Endereco)), '
 			SET @NF = @nf +' Endereco_Nro = RTRIM(LTRIM(Cliente.Endereco_Nro)), '
			SET @NF = @NF + ' Complemento_End,'
 			SET @NF = @nf +' Bairro = RTRIM(LTRIM(Cliente.Bairro)), '
 			SET @NF = @nf +' Cidade = RTRIM(LTRIM(Cliente.Cidade)), '
 			SET @NF = @nf +' UF = RTRIM(LTRIM(Cliente.UF)), '
 			SET @NF = @nf +' CEP = RTRIM(LTRIM(Cliente.CEP)), '
 			SET @NF = @nf +' Limite_Credito = Cliente.Limite_Credito, '
			SET @NF = @NF + ' Taxa_Entrega, '
 			SET @NF = @nf +' Utilizado = Cliente.Utilizado, Codigo_Tabela = 0 '
			SET @NF = @nf +' FROM CLIENTE LEFT OUTER JOIN Tabela_Preco ON Cliente.Codigo_Tabela = Tabela_Preco.Codigo_Tabela '
			SET @NF = @nf +' Where RTRIM(LTRIM(Codigo_Cliente)) = ' + CHAR(39) + @CodigoCliente + CHAR(39) + ' '			
		End
  execute(@NF)
  --  print @nf
End  
/****** Object:  Index [PK_pedido_itens_obs]    Script Date: 12/15/2020 10:29:24 ******/
ALTER TABLE [dbo].[pedido_itens_obs] ADD  CONSTRAINT [PK_pedido_itens_obs] PRIMARY KEY CLUSTERED 
(
	[filial] ASC,
	[pedido] ASC,
	[plu] ASC,
	[id] ASC,
	[obs] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

/****** Object:  StoredProcedure [dbo].[sp_ins_Pedido]    Script Date: 12/15/2020 10:39:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  alter PROCEDURE [dbo].[sp_ins_Pedido]  
                @FILIAL VARCHAR(20),  
                @PEDIDO CHAR(8),  
                @STATUS INT,  
                @TIPO INT,  
                @CLIENTE_FORNEC VARCHAR(20),  
                @DATA_CADASTRO DATETIME,  
                @DATA_ENTREGA DATETIME,  
                @HORA VARCHAR(5),  
                @DESCONTO DECIMAL(12,2),  
                @TOTAL DECIMAL(12,2),  
                @USUARIO VARCHAR(20),  
                @OBS TEXT,  
                @CFOP NUMERIC(4),  
                @ORCAMENTO CHAR(8),  
                @FUNCIONARIO VARCHAR(20),  
                @ID TINYINT,  
                @COTACAO INT,  
                @HORA_FIM VARCHAR(5),  
                @IMPRESSO TINYINT,  
                @TABELA_DESCONTO VARCHAR(20),  
                @PEDIDO_SIMPLES TINYINT,  
                @CENTRO_CUSTO VARCHAR(9),  
                @CODVENDEDOR INT,  
                @ENTREGADOR VARCHAR(20),
                @OPERADOR INT,
                @ORIGEM VARCHAR(3) = '',
                @ID_MOVIMENTO INT = 0,
				@ENTREGA INT = 0,
				@LUGARENTREGA VARCHAR(20) = '',
				@TXENTREGA DECIMAL(12,2) = 0
AS 
                BEGIN   
                               SET NOCOUNT  ON     
                               INSERT INTO PEDIDO  
                               ( 
                                               FILIAL,   
                                               PEDIDO,   
                                               STATUS,   
                                               TIPO,   
                                               CLIENTE_FORNEC,   
                                               DATA_CADASTRO,   
                                               DATA_ENTREGA,   
                                               HORA,   
                                               DESCONTO,   
                                               TOTAL,   
                                               USUARIO,   
                                               OBS,   
                                               CFOP,   
                                               ORCAMENTO,   
                                               FUNCIONARIO,   
                                               ID,   
                                               COTACAO,   
                                               HORA_FIM,   
                                               IMPRESSO,   
                                               TABELA_DESCONTO,   
                                               PEDIDO_SIMPLES,   
                                               CENTRO_CUSTO,   
                                               CODVENDEDOR,   
                                               ENTREGADOR,
                                               OPERADOR,
                                               ORIGEM,
                                               ID_MOVIMENTO,
											   ENTREGA,
											   LUGAR_ENTREGA,
											   TAXAS
                               ) VALUES   ( 
                                               @FILIAL,   
                                               @PEDIDO,   
                                               @STATUS,   
                                               @TIPO,   
                                               @CLIENTE_FORNEC,   
                                               @DATA_CADASTRO,   
                                               @DATA_ENTREGA,   
                                               @HORA,   
                                               @DESCONTO,   
                                               @TOTAL,   
                                               @USUARIO,   
                                               @OBS,   
                                               @CFOP,   
                                               @ORCAMENTO,   
                                               @FUNCIONARIO,   
                                               @ID,   
                                               @COTACAO,   
                                               @HORA_FIM,   
                                               @IMPRESSO,   
                                               @TABELA_DESCONTO,   
                                               0,   
                                               @CENTRO_CUSTO,   
                                               @CODVENDEDOR,   
                                               @ENTREGADOR,
                                               @OPERADOR,
                                               @ORIGEM,
                                               @ID_MOVIMENTO,
											   @ENTREGA,
											   @LUGARENTREGA,
											   @TXENTREGA
                               )  
                END

GO
/****** Object:  StoredProcedure [dbo].[sp_Ins_Pedido_Itens]    Script Date: 12/16/2020 09:01:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



alter Procedure [dbo].[sp_Ins_Pedido_Itens]  
	@FILIAL VARCHAR(20),  
	@PEDIDO CHAR(8),  
	@PLU VARCHAR(17),  
	@QTDE DECIMAL(12,3),  
	@EMBALAGEM INT,  
	@UNITARIO DECIMAL(12,2),  
	@TOTAL DECIMAL(12,2),  
	@ID TINYINT,  
	@EAN VARCHAR(15),  
	@TIPO INT,  
	@DESCONTO NUMERIC(12,2),
	@IDFRACAO INT = 0  
AS 
	BEGIN   
		SET NOCOUNT  ON     
		INSERT INTO PEDIDO_ITENS  ( 
			FILIAL,   
			PEDIDO,   
			PLU,   
			QTDE,   
			EMBALAGEM,   
			UNITARIO,   
			TOTAL,   
			ID,   
			EAN,   
			TIPO,   
			DESCONTO,
			ID_FRACAO
		) VALUES   ( 
			@FILIAL,   
			@PEDIDO,   
			@PLU,   
			@QTDE,   
			@EMBALAGEM,   
			@UNITARIO,   
			@TOTAL,   
			@ID,   
			@EAN,   
			@TIPO,   
			@DESCONTO,
			@IDFRACAO
		)  
	END






	
insert into Versoes_Atualizadas select 'Versão:1.279.830', getdate();
