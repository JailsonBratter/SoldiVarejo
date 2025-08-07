
--sp_rel_fin_ResumoPorOperador 'MATRIZ', '20120901', 102,8
ALTER PROCEDURE [dbo].[sp_Rel_Fin_ResumoPorOperador](
	@FILIAL 	AS VARCHAR(17),
	@Data		As Varchar(8),
	@Operador	As Varchar(40),
	@Pdv		As Varchar(4),
	@horade		As varchar(5),
	@horaate	As varchar(5))
as
	
if len(@horade)= 0
	begin
		set @horade = '0'
	end
if len(@horaate) = 0
	begin
		set @horaate = '24'
	end
	
Declare @pIndex int 
Declare @idOp int 

if LEN (@Operador)>0
begin
	set @pIndex= CHARINDEX('-', @Operador)	
    set @idOp = SUBSTRING(@Operador,0,@pIndex)

end


Select 
	isnull(finalizadora.Finalizadora,'Outra') as Finalizadora, sum(ISNULL(total,0)) as valor

From 	Lista_Finalizadora
Left Outer Join	finalizadora on 
	lista_finalizadora.finalizadora = finalizadora.nro_finalizadora
Where
	lista_finalizadora.filial = @filial and 
	Emissao = @data and 
	(operador=@idOp or LEN(@operador)=0 )and
	(pdv = @Pdv or LEN(@pdv)=0)and
	isnull(cancelado,0) = 0 And
	exists(Select * from saida_Estoque where
			data_movimento = @data and 
			convert(decimal(18,2),substring(replace(hora_venda,':','.'),1,5)) 
					between isnull(convert(decimal(18,2), replace(@horade,':','.')),0) and isnull(convert(decimal(18,2), replace(@horaate,':','.')),24)
			and lista_finalizadora.cupom = saida_Estoque.documento)
group by 
	finalizadora.Finalizadora




go



ALTER TRIGGER [dbo].[itg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR INSERT
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @plu_item varchar(17),
 @fator_conversao decimal(9,3),
 @tipo_p varchar(20)

select @plu = plu, @filial = filial, @qtde = qtde from INSERTED

select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
if @tipo_p  != 'PRODUCAO'
BEGIN
 update mercadoria set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu  
update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu and filial = @filial 
end


go





ALTER TRIGGER [dbo].[utg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR UPDATE
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @odt_canc datetime,
 @ndt_canc datetime,
 @tipo_p varchar(20)


declare x_update cursor for select DELETED.plu, DELETED.filial, DELETED.qtde, DELETED.data_cancelamento odt_canc, INSERTED.data_cancelamento ndt_canc 
   from DELETED
   inner join INSERTED
      on (DELETED.filial = INSERTED.filial and DELETED.data_movimento = INSERTED.data_movimento and DELETED.documento = INSERTED.documento and DELETED.origem = INSERTED.origem and DELETED.plu = INSERTED.plu and DELETED.caixa_saida = INSERTED.caixa_saida and DELETED.codigo_funcionario = INSERTED.codigo_funcionario and DELETED.ean = INSERTED.ean and DELETED.pedido = INSERTED.pedido)

open x_update

FETCH NEXT FROM x_update
 INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc

WHILE @@FETCH_STATUS = 0
BEGIN
 select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
 if @tipo_p  != 'PRODUCAO'
 BEGIN
  if update(data_cancelamento)
  begin
   if @odt_canc is null and @ndt_canc is not null 
   begin
    update mercadoria set saldo_atual = isnull(saldo_atual,0) + @qtde where plu =  @plu 
    update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + @qtde where plu =  @plu and filial = @filial
   end 
  end
 end
 FETCH NEXT FROM x_update
  INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc 
END

close x_update

deallocate x_update


go



ALTER          Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60)
AS
                
                Declare @String               As nVarchar(2000)
		Declare @Where               As nVarchar(1024)
BEGIN
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And data_cancelamento is null'
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
		
		
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                               if ISNULL(@nLinhas, 0) > 0 
                                               SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,' 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Total Venda] = Convert(Numeric(12,2), Sum(VLR-desconto)), '
                set @String = @String + ' [Lucro Bruto] = Convert(Numeric(12,2),SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))),'
		Set @String = @String + ' [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2), Sum(VLR-desconto)) / (select sum(convert(numeric(12,2), (vlr-desconto))) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--** Inserindo Clausula Subquery
		Set @String = @String + @Where + ' )* 100)) + '+ char(39)+ '%'+ char(39)
                SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
                SET @String = @string + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao '
 

-- PRINT @STRING
                EXECUTE(@String)
END
