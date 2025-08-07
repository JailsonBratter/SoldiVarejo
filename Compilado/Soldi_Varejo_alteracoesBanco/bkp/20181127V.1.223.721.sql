


IF OBJECT_ID('sp_rel_conta_a_pagar_Saldo_geral', 'P') IS NOT NULL
begin 
	drop procedure [sp_rel_conta_a_pagar_Saldo_geral]
end 

go
-- exec [sp_rel_conta_a_pagar_Saldo_geral] '' 
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_rel_conta_a_pagar_Saldo_geral](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10),
	@tipoPagamento varchar(50)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
--exec sp_Rel_conta_a_pagar_Saldo_geral @Filial='MATRIZ', @datade = '20181201',  @dataate = '20181226',  @Status = 'PREVISTO',  @valor = '',  @fornecedor = '',  @centrocusto = '',  @Cheque = '',  @CONFERIDO = 'TODOS',  @tipoPagamento = 'TODOS' 

	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where +  ' vencimento between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
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
	
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			[Valor Pagar] = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[Saldo Geral]='+char(39)+char(39)+'

			
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc and conta_a_pagar.filial=Conta_corrente.filial  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
			left outer join Fornecedor on Conta_a_pagar.Fornecedor = Fornecedor.Fornecedor
		'+@where+'  Order By convert(varchar ,vencimento'	+',102) '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	print(@string)
	Exec(@string)
End

go 


IF OBJECT_ID('sp_rel_fin_Posicao_Cliente', 'P') IS NOT NULL
begin 
	drop procedure [sp_rel_fin_Posicao_Cliente]
end 

go

CREATE   procedure [dbo].[sp_rel_fin_Posicao_Cliente](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@valor VARCHAR(11) ,
@Codigo_cliente varchar(50),
@status varchar(10),
@centrocusto varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples	as varchar(5)	
)
--sp_rel_FIN_POSICAO_CLIENTE 'MATRIZ', '20150801', '20150804', 'EMISSAO', '', 'ANTO', '', '', '', ''
As

Declare @String as nvarchar(max)
Declare @Where as nvarchar(max)

Begin
--Monta Clausula Where da Procura
set @Where = ' Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
 set @Where = @Where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo

		if len(rtrim(ltrim(@Codigo_cliente))) > 0
		Begin
		   set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
		End

		if len(rtrim(ltrim(@valor))) > 1
		Begin
				set @Where = @Where + ' And valor ='+REPLACE(@valor,',','.')
		End

if @status<>'PREVISTO'
begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
								  			 WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
											 WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
											 WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4' 
										END)
end

if LEN(@centrocusto)>0
begin
set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39)
end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF @AVista <>'TODOS' 
begin
	SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AVISTA' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PRAZO' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) '

IF @simples <>'TODOS'
BEGIN
	
	SET @Where = @Where + ' And ISNULL(PD.pedido_simples,0) ='+ case   when  @simples  =  'SIM'  THEN  '1'    
																	   when  @simples  =  'NAO'  THEN  '0'  
																   END 
END 

--Monta Select
set @string = 'select
Simples = Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ',
Documento = rtrim(ltrim(documento)),
Cliente = rtrim(ltrim(cliente.Codigo_Cliente)),
Nome_Cliente = rtrim(ltrim(cliente.nome_Cliente)),
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') '+
' from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento'+

@Where+

' Order By cliente.Codigo_Cliente, cliente.nome_Cliente, PD.pedido_simples ';  

 Execute (@string)


-- Print @Where;




End


go 
IF OBJECT_ID('sp_Rel_ProdutosAlterados', 'P') IS NOT NULL
begin 
	drop procedure [sp_Rel_ProdutosAlterados]
end 

go

--PROCEDURES =======================================================================================
CREATE Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20),
                @Descricao              As Varchar(60) ,
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40)
				,@custoMargem			as varchar(40)
				,@LINHA VARCHAR(30)
				,@saldo varchar(14)


AS

Declare @String               As Varchar(max)
Declare @Where               As Varchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , '
		SET @String = @String + 'mercadoria.PESO , '

		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin

		SET @String = @String + 'CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO,'
		end		
		if(@custoMargem='CUSTO-MARGEM-VENDA' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
			set @String = @String + ' mercadoria_loja.PRECO_CUSTO,MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
			SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		end
	
	
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu '
		SET @String = @String + ' left join LINHA on mercadoria.Cod_Linha = LINHA.codigo_linha '
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'
		Set @Where = ' WHERE  (Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)+')'
        
         IF (LEN(@DESCRICAO) > 0)
		BEGIN
            SET @Where = @Where + ' AND ( mercadoria.DESCRICAO LIKE '+ CHAR(39) +'%'+ @DESCRICAO + '%'+CHAR(39)+')'

        END      
        
        
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
		
			Set @Where = @Where +' And (convert(date,Mercadoria.Data_Alteracao) BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)+')'
		end
	       
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)+')'

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND (e.ean = '+ CHAR(39) + @ean + CHAR(39)+')'

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)+')'

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)+')'

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)+')'

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)+')'

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end


		Begin
		IF(@saldo='Igual a Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual=0)'
		End
		
		Begin
		IF(@saldo='Menor que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual<0)'
		End
		
		Begin
		IF(@saldo='Maior que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual>0)'
		End
		
		IF(LEN(@LINHA)>0 )
		Begin
			SET @Where = @Where + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
		End
	 

		--print(@String + @Where)
		EXECUTE(@String + @Where)

END

 



go 


if not exists(select 1 from PARAMETROS where PARAMETRO='N_DETALHE_UM_PRODUTO')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('N_DETALHE_UM_PRODUTO'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'NÃO MOSTRAR DETALHE QUANDO PESQUISA RETORNAR UM PRODUTO'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end

GO 
if not exists(select 1 from PARAMETROS where PARAMETRO='CONTA_CC_REC')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('CONTA_CC_RECEBER'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,''
           ,'CONTA_CC_REC'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO
if not exists(select 1 from PARAMETROS where PARAMETRO='CENTRO_CUSTO_REC')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('CENTRO_CUSTO_REC'
           ,GETDATE()
           ,'001001001'
           ,GETDATE()
           ,'001001001'
           ,'CENTRO DE CUSTO CONTAS A RECEBER'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO



if not exists(select 1 from PARAMETROS where PARAMETRO='PERMITE_DUPLO_ESP')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('PERMITE_DUPLO_ESP'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'PERMITE ESPACOS DUPLOS'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO

if not exists(select 1 from PARAMETROS where PARAMETRO='OCULTA_PG_PEDIDO')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('OCULTA_PG_PEDIDO'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'OCULTA VALOR DO PAGAMENTO NA IMPRESSAO DO PEDIDO'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO





GO

IF OBJECT_ID('sp_EFD_ICMSIPI_C800', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800]
end 

go

--[sp_EFD_ICMSIPI_C800] 'MATRIZ','20180601', '20180630'

CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
	
As
BEGIN
	

		select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '00',
			NUM_CFE = S.coo,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/',''),
			VL_CFE = (SUM(S.vlr)-SUM(CONVERT(NUMERIC(18,2),S.Desconto)))+SUM(S.Acrescimo),
			VL_PIS = SUM(S.TotalPis),
			VL_COFINS = SUM(S.TotalCofins),
			CNPJ_CPF = REPLACE(REPLACE(REPLACE(c.CNPJ,'.',''),'-',''),'/',''),
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =SUM(S.Desconto),
			VL_MERC = SUM(S.VLR),
			VL_OUT_DA =SUM(s.acrescimo),
			VL_ICMS = SUM(S.TotalICMS),
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is null  
		  and (CONVERT(DATE,S.Data_Movimento) Between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				,c.CNPJ
				,S.numero_serie
				,id_Chave
	UNION ALL 
			select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '02',
			NUM_CFE = S.coo,
			DT_DOC = NULL,
			VL_CFE = NULL,
			VL_PIS = NULL,
			VL_COFINS = NULL,
			CNPJ_CPF = NULL,
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =NULL,
			VL_MERC = NULL,
			VL_OUT_DA =NULL,
			VL_ICMS = NULL,
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is NOT null  
		  and (CONVERT(DATE,S.Data_Movimento) Between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,CONVERT(DATE,S.Data_Movimento)
				,c.CNPJ
				,S.numero_serie
				,id_Chave

			
END


go 




IF OBJECT_ID('sp_EFD_ICMSIPI_C850', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C850]
end 

go

--[sp_EFD_ICMSIPI_C800] 'MATRIZ','20180601', '20180630'
--[sp_EFD_ICMSIPI_C850] 'MATRIZ','35180610564952000191590005069470014994802070'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C850]
	@Filial		AS Varchar(20),
	@CHV_CFE as varchar(44)
As
BEGIN
	

	BEGIN
		SELECT
		       	REG = 'C850',
		       	CST_ICMS = '0' + RTRIM(LTRIM(CASE WHEN b.NRO_ECF = 4 THEN '60' 
							  WHEN b.NRO_ECF = 5 THEN '40' 
							  WHEN b.NRO_ECF = 6 THEN '41'
							ELSE '00' END )),
		       	CFOP = CASE WHEN b.NRO_ECF = 4 THEN '5403' ELSE '5102' END,
			--** Aliuota ICMS	
			ALIQ_ICMS =  CASE WHEN b.NRO_ECF = 4 THEN 0 ELSE b.Aliquota_ICMS END,
			VL_OPR = b.VLR,
			VL_BC_ICMS = CASE WHEN b.NRO_ECF = 4 OR b.NRO_ECF = 5 THEN 0 ELSE b.VLR END
		INTO
			#EFD_ICMSIPI_C850
		FROM
		
		       Mercadoria a INNER JOIN Saida_Estoque b ON a.PLU = b.PLU
		
		       INNER JOIN Tributacao t ON t.Codigo_Tributacao = a.Codigo_Tributacao AND t.Filial = b.Filial
		WHERE
		       b.Filial = @Filial
		
		       AND b.Data_Cancelamento IS NULL
			   AND REPLACE(b.id_Chave,'Cfe','') = @CHV_CFE
		      
	END	
	--** 
	BEGIN
		SELECT 
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS,
			VL_OPR = SUM(VL_OPR),
			VL_BC_ICMS = SUM(ISNULL(VL_BC_ICMS,0)),
			VL_ICMS = SUM(ISNULL(VL_OPR * (ALIQ_ICMS/100),0)),
			COD_OBS = ' '
		
		FROM
			#EFD_ICMSIPI_C850
		GROUP BY
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS
	END
			
END


go 


IF OBJECT_ID('sp_EFD_ICMSIPI_C800_GDT', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800_GDT]
end 

go

--[sp_EFD_ICMSIPI_C800_GDT] 'MATRIZ','20180601', '20180630'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800_GDT]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8) 
As
BEGIN
		select 
			REG = 'C800',
			NR_SAT= S.numero_serie,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')			
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and (CONVERT(DATE,S.Data_Movimento) between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		 AND S.numero_serie IS NOT NULL
		group by S.numero_serie
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				
			
END

GO 



IF OBJECT_ID('sp_EFD_ICMSIPI_C800_G', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800_G]
end 

go

--[sp_EFD_ICMSIPI_C800_G] 'MATRIZ','20180601', '20180630'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800_G]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8) 
As
BEGIN
		select 
			REG = 'C800',
			NR_SAT= S.numero_serie
						
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and (CONVERT(DATE,S.Data_Movimento) between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		 AND S.numero_serie IS NOT NULL
		group by S.numero_serie
		        
				
			
END

GO

GO



IF not  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sped_blocos]') AND type in (N'U'))
begin
	create table sped_blocos
	(
			filial varchar(40),
			id int,
			tipoArquivo varchar(40),
			bloco varchar(10),
			str_procedure varchar(50),
			id_bloco_pai int,
			ordem int ,
			gerarArquivo tinyint,
			campoArquivo varchar(40),
			blocoTotaliza varchar(40),
			bloco_grupo tinyint 
			
	)
end
else
begin 
			IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
					WHERE UPPER(TABLE_NAME) = UPPER('sped_blocos') 
					AND  UPPER(COLUMN_NAME) = UPPER('bloco_grupo'))
		begin
			alter table sped_blocos alter column bloco_grupo tinyint
		end
		else
		begin
			alter table sped_blocos add bloco_grupo tinyint
		end 


end

go


go 

IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sped_blocos_parametros]') AND type in (N'U'))
begin
create table sped_blocos_parametros
	(
		filial varchar(40),
		id_parametro int,
		id_bloco int,
		nome_parametro varchar(50),
		tipo_dados varchar(40),
		tamanho int,
		campo_pai varchar(40),
		herda_pai tinyint	
	)

end

go



-- exec sp_EFD_PisCofins_C405 '','','',0,0
-- select * from sped_blocos
-- select * from sped_blocos_parametros 
--


declare @id int;
declare @id_pai int; 
declare @ordem int ;
declare @filial varchar(40);
declare @tipoArquivo varchar(20);
set @filial ='MATRIZ';
set @tipoArquivo ='ICMSIPI'


delete SP from sped_blocos_parametros AS  sp INNER JOIN SPED_BLOCOS as sb ON  sb.id = sp.id_bloco
where sb.tipoArquivo =@tipoArquivo;
 
delete from sped_blocos where tipoArquivo=@tipoArquivo;

-- Registro 0000====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0000','sp_EFD_PisCofins_0000',0,@ordem,1,'0990');

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data_Ini','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data_Fim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Tipo','Numero',1,'',0);			

-- Registro 0001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0001','sp_EFD_PisCofins_0001',@id_pai,@ordem,0,'0990');

-- Registro 0005====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0005','sp_EFD_PisCofins_0005',@id_pai,@ordem,0,'0990');



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);

-- Registro 0100====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0100','sp_EFD_PisCofins_0100',@id_pai,@ordem,0,'0990');


-- Registro 0150====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0150','sp_EFD_PisCofins_0150',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_ini','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_Fim','Data',8,'',0);


-- Registro 0190====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0190','sp_EFD_PisCofins_0190',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataFim','Data',8,'',0);

-- Registro 0200====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem =  isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0200','sp_EFD_PisCofins_0200',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataFim','Data',8,'',0);



-- Registro 0400====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem =  isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0400','sp_EFD_PisCofins_0400',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DATAINI','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DATAFIM','Data',8,'',0);




-- Registro C001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C001','sp_EFD_PisCofins_C001',0,@ordem,1,'','C990');



-- Registro C100====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C001' and tipoArquivo=@tipoArquivo


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C100','sp_EFD_PisCofins_C100',@id_pai,0,0,'','C990');

				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
-- Registro C170====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C100' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C170','sp_EFD_PisCofins_C170',@id_pai,@ordem,0,NULL,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Fornecedor','Texto',20,'COD_PART',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'NroNota','Texto',10,'NUM_DOC',0);

-- Registro C190====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C100' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C190','sp_EFD_ICMSIPI_C190',@id_pai,@ordem,0,NULL,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Fornecedor','Texto',20,'COD_PART',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'NroNota','Texto',10,'NUM_DOC',0);

				
				
-- Registro C400====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C400','sp_EFD_PisCofins_C400','',4,1,'ECF_CX','C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
-- Registro C405====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C400' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C405','sp_EFD_PisCofins_C405',@id_pai,@ordem,1,'DT_DOC','C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'FabEqu','Texto',20,'ECF_FAB',0);

-- Registro C410====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C410','sp_EFD_PisCofins_C410',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOIni',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOFim',0);


-- Registro C420====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C420','sp_EFD_PisCofins_C420',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COORDZ','Numero',10,'COOFim',0);


-- Registro C460====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C460','sp_EFD_PisCofins_C460',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOini',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOfim',0);


-- Registro C470====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C460' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C470','sp_EFD_PisCofins_C470',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',1);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Cupom','Numero',10,'NUM_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Tipo','Numero',1,'',0);
				
				

-- Registro C490====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C490','sp_EFD_ICMSIPI_C490',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOini',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOfim',0);


-- Registro C800G====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800G','sp_EFD_ICMSIPI_C800_G','',@ordem,1,'NR_SAT','',1);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);


				
-- Registro C800G DT====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800G' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai and tipoArquivo=@tipoArquivo;
insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800GDT','sp_EFD_ICMSIPI_C800_GDT',@id_pai,@ordem,1,'DT_DOC','',1);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);


-- Registro C800====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800GDT' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai and tipoArquivo=@tipoArquivo;
insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800','sp_EFD_ICMSIPI_C800',@id_pai,@ordem,0,'','C990',0);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data','Data',8,'DT_DOC',1);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'NR_SAT','texto',50,'NR_SAT',1);


-- Registro C850====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C850','sp_EFD_ICMSIPI_C850',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'CHV_CFE','TEXTO',44,'CHV_CFE',1);





insert into Versoes_Atualizadas select 'v.1.223.721', getdate();
GO











