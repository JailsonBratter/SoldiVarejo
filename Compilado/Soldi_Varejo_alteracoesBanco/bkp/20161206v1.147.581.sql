
/****** Object:  StoredProcedure [dbo].[sp_rel_Tesouraria]    Script Date: 12/07/2016 16:12:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Tesouraria]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_Tesouraria]
GO



/****** Object:  StoredProcedure [dbo].[sp_rel_Tesouraria]    Script Date: 12/07/2016 16:12:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--sp_rel_Tesouraria 'MATRIZ',20160205,20160205,'TODOS','FECHADO'

CREATE Procedure [dbo].[sp_rel_Tesouraria]
		  @Filial       As Varchar(17),
		  @DataDe             As Varchar(8),
		  @DataAte      As Varchar(8),
		  @Operador     As Varchar(20),
		  @Status_Pdv     As varchar(20)
    
As    

Declare @String as nvarchar(max)
Declare @Where as nvarchar(max)
Declare @And as nvarchar(max)
Declare @WhereReceber as nvarchar(max);
      
SET @Where= '';
set @WhereReceber = '';
SET @And = '';

            Set @Where = ' And l.Filial = '+ char(39) + @filial + char(39) + ' and '
            Set @Where = @Where + ' Emissao between ' + char(39) + @DataDe + char(39) + ' and ' + char(39) + @DataAte + char(39)
            
            Set @WhereReceber = ' And l.Filial = '+ char(39) + @filial + char(39) + ' and '
            Set @WhereReceber = @WhereReceber + ' Emissao between ' + char(39) + @DataDe + char(39) + ' and ' + char(39) + @DataAte + char(39)

IF @Operador <> 'TODOS'
  Begin
            Set @Where = @Where + ' And op.ID_Operador = '+ char(39) + @Operador + char(39)
            Set @WhereReceber = @WhereReceber + ' And Operador = '+ char(39) + @Operador + char(39)
  End
IF @Status_Pdv <> 'TODOS'
  Begin
            Set @And = ' And sp.Status = '+ char(39) + @Status_Pdv + char(39)                 
  End         
      

Set @String = 'Select 
      NroFinalizadora = Isnull(l.Finalizadora,0), 
      Finalizadora = Isnull(l.id_Finalizadora,' + CHAR(39) + CHAR(39) + '), 
      [Valores Caixas] = Sum(ISNULL(l.Valor,0)-ISNULL(l.Troco,0)), 
      [Valores Tesouraria] = (Select Isnull(Sum(c.Valor),0) From Conta_a_Receber c 
            Where c.id_Finalizadora = l.id_Finalizadora And c.Finalizadora = l.Finalizadora      '     
            +@WhereReceber+')
		From 
			  Lista_Finalizadora l Inner Join Operadores as op on op.ID_Operador=l.operador 
			  Left Join Status_Pdv as sp on sp.Id_Operador = op.ID_Operador And sp.pdv= l.pdv And sp.Data_Abertura = l.Emissao  
		Where 
			  Isnull(l.Cancelado,0) = 0
		And
			  Not l.id_Finalizadora = ' + CHAR(39) + CHAR(39)  +' '+@Where +' '+@And +'
		Group by 
			  l.Finalizadora, l.id_Finalizadora, l.Filial
		Union
		Select 
			  NroFinalizadora = Isnull(l.Finalizadora,0), 
			  Finalizadora = Case When l.id_Finalizadora = ' + CHAR(39) + CHAR(39) + ' Then Upper(Convert(Varchar,Substring(Obs,1,16))) Else l.id_Finalizadora End, 
			  [Valores Caixas] = 0 , [Valores Tesouraria] = Sum(l.Valor)
		From 
			  Conta_a_Receber l
					Inner Join Operadores as op on op.ID_Operador=l.operador 
					Left Join Status_Pdv as sp on sp.Id_Operador = op.ID_Operador And sp.pdv = l.pdv And sp.Data_Abertura = l.Emissao  
		Where
			  id_Finalizadora = ' + CHAR(39) + CHAR(39) +' '+@Where +' '+@And +'
		And
			  Not Exists(Select * From Lista_Finalizadora lista 
									 Where l.Finalizadora = lista.Finalizadora
									 And l.id_Finalizadora = lista.id_Finalizadora
									 And l.Emissao = lista.Emissao      
									 And l.pdv = lista.pdv)
		Group by 
			  l.Finalizadora, l.id_Finalizadora, Convert(Varchar,Substring(l.obs,1,16)), l.Filial
		Order by 
			  1, 2 ';

		--print (@string)
		exec (@string);






GO

go 

alter table caderneta add data_canc datetime, vlr_cancelado numeric(9, 2)

go




alter table usuarios_web_telas  alter column nome_tela varchar(50)



GO

/****** Object:  Table [dbo].[Ticket]    Script Date: 11/30/2016 10:29:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ticket](
               [Numero] [bigint] NOT NULL,
               [Incluido] [datetime] NOT NULL,
               [Validado] [datetime] NULL
) ON [PRIMARY]

GO

/****** Object:  Index [IX_Ticket]    Script Date: 11/30/2016 10:30:54 ******/
CREATE NONCLUSTERED INDEX [IX_Ticket] ON [dbo].[Ticket] 
(
               [Numero] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  StoredProcedure [dbo].[sp_rel_fechamento_fornecedor]    Script Date: 11/30/2016 11:21:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fechamento_fornecedor]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_rel_fechamento_fornecedor]

GO

--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_fechamento_fornecedor](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10),
	@tipoPagamento varchar(50)
) as

Declare @String as nvarchar(max)
Declare @Where as nvarchar(max)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And conta_a_pagar.fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if(@Conferido<>'TODOS')
	begin
		IF(@Conferido='SIM')
		BEGIN
			set @where = @where + ' And conferido =1 '	
		END
		ELSE
		BEGIN
			set @where = @where + ' And ISNULL(conferido,0) =0 '	
		END
		 
	end 
	
	set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
											 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
											 WHEN @STATUS='CANCELADO' THEN ' status =3'
											 WHEN @STATUS='LANCADO' THEN ' status =4'
										ELSE 'status <> 3' END
																) 
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	
	if(@tipoPagamento <> 'TODOS')
	BEGIN
		set @Where = @Where + ' and Conta_a_pagar.TIPO_PAGAMENTO= '+ char(39)+ @tipoPagamento+ char(39) 
	END
	

	set @string = ' Select Codigo_Centro_Custo,
					Fornecedor,
					[Valor Pagar] = sum(Isnull(Valor,0)) - sum(Isnull(Desconto,0)) + (sum(Isnull(Acrescimo,0))) 
					from Conta_a_pagar
					'+@Where+'
					group by Codigo_Centro_Custo,Fornecedor
					order by Codigo_Centro_Custo';
			
	Exec(@string)
end
GO






/****** Object:  StoredProcedure [dbo].[sp_Rel_Curva_ABC]    Script Date: 11/30/2016 11:58:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Curva_ABC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Curva_ABC]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Curva_ABC]    Script Date: 11/30/2016 11:58:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40)
AS
                
                Declare @String               As nVarchar(2000)
		Declare @Where               As nVarchar(1024)
BEGIN
	
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And  Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And  data_cancelamento is null and (isnull(mercadoria.curva_a,0)=1 and isnull(mercadoria.curva_b,0)=1 and isnull(mercadoria.curva_c,0) = 1) '
		--** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver, 
                --** o sistema faz um LIKE no campo descrição recebido no parametro.
                BEGIN
                               IF LEN(ISNULL(@DESCRICAO,'')) > 0 
                                               SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)
                END       
                --** Checa se o parametro @Grupo contem alguma informação. Se tiver, 
                
                BEGIN
                               IF LEN(ISNULL(@GRUPO,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_GRUPO='+ CHAR(39) + @GRUPO + CHAR(39) 
                               IF LEN(ISNULL(@subGrupo,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_SUBGRUPO='+ CHAR(39) + @subGrupo + CHAR(39) 
							   IF LEN(ISNULL(@Departamento,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_DEPARTAMENTO='+ CHAR(39) + @Departamento + CHAR(39) 
							   IF LEN(ISNULL(@Familia,'')) > 0 
                                               SET @Where = @Where + ' AND familia.DESCRICAO_familia='+ CHAR(39) + @Familia + CHAR(39) 
                
                END       
		
		Begin
			if LEN(ISNULL(@Ordem,'')) = 0 
			Set @Ordem ='Qtde'
		end
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                               if ISNULL(@nLinhas, 0) > 0 
                                               SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' ROW_NUMBER() over(order by Convert(Numeric(10,3), Sum(Qtde)) desc) as RANK, Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,'
                SET @String = @String + ' [Custo] = MERCADORIA.PRECO_CUSTO, '
                SET @String = @String + ' [Margem] = MERCADORIA.MARGEM, '
                SET @String = @String + ' [Preco] = MERCADORIA.PRECO, '
                                 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                --SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Valor] = Convert(Numeric(12,2), Sum(VLR-desconto)), '
                set @String = @String + ' [Rentabilidade] = Convert(Numeric(12,2),SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo)))'
  --              Set @String = @String + ', [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2),'
		--if(@Ordem='VALOR')
		--BEGIN
		--		 Set @String = @String + ' Sum(VLR-desconto)) / SELECT SUM(VLR)FROM ((select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' convert(numeric(12,2), (vlr-desconto)) AS VLR from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia) AS T'
		--END
		--ELSE if(@Ordem='QTDE')
		--BEGIN
		--		Set @String = @String + ' Sum(Qtde)) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' Sum(ISNULL(saida_estoque.Qtde,0)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		
		--ELSE if(@Ordem='RENTABILIDADE')
		--BEGIN
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		----** Inserindo Clausula Subquery
		--Set @String = @String + @Where + ' )  * 100)) + '+ char(39)+ '%'+ char(39)
        SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque  WITH (INDEX(Ix_Fluxo_Caixa)) ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
	
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao, MERCADORIA.PRECO_CUSTO,MERCADORIA.MARGEM,MERCADORIA.PRECO  Order by ['+@Ordem+'] Desc'
 
 -- PRINT @STRING
                EXECUTE(@String)
END


insert into Versoes_Atualizadas select 'Versão:1.147.581', getdate();