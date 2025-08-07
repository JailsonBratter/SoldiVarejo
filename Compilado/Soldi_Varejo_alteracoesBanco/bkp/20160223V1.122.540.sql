if not exists(select 1 from PARAMETROS where PARAMETRO='PEDIDO_IMPRIMIR_40')
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
           ('PEDIDO_IMPRIMIR_40'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'HABILITA IMPRESSAO DO PEDIDO VENDA COM 40 CARACTERS'
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


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Pedido_Venda_Analitico]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Pedido_Venda_Analitico]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Analitico]
            @Filial					As Varchar(20),
            @DataDe                 As Varchar(8),
            @DataAte				As Varchar(8),
			@PLU					As Varchar(17) = '',
			@REF_Fornecedor			As Varchar(20) = '',
            @Descricao				As Varchar(60) = '',
			@Cliente				As Varchar(40) = '',
			@Simples    			As VarChar(3) = '',
			@Vendedor				as Varchar(30) = ''			
AS
                
	Declare @String               As nVarchar(3000)
	Declare @String2			  As nVarchar(1024)
	Declare @Where                As nVarchar(1024)

	SET @Where = ' WHERE PEDIDO.FILIAL='+ CHAR(39) +@Filial+ CHAR(39) +' AND Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
	SET @Where = @Where + 'AND Pedido.Data_Cadastro BETWEEN '  + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)	
	--//Filtro PLU
	IF @PLU <> ''
		SET @Where = @Where + ' AND Pedido_Itens.PLU = ' + CHAR(39) + @PLU + CHAR(39)
	--** Filtro Descricao	
	IF @Descricao <> ''
		SET @Where = @Where + ' AND Mercadoria.Descricao LIKE' + CHAR(39) + '%' + @Descricao + '%' + CHAR(39)
	--** Filtro Ref_Fornecedor
	IF @REF_Fornecedor <> ''
		SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE' + CHAR(39) + @REF_Fornecedor + '%' + CHAR(39)
	--** Filtro Cliente
	IF @Cliente <> ''
		BEGIN
			SET @Where = @Where + ' AND (Cliente.Nome_Cliente LIKE ' + CHAR(39) + '%' + @Cliente + '%' + CHAR(39)
			SET @Where = @Where + '	OR Replace(Replace(Replace(Cliente.CNPJ, ' + CHAR(39) + '.' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '/' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '-' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ') LIKE ' + CHAR(39) + '%' + @Cliente + '%'+ CHAR(39) + ')'
		END
	--//Filtro Vendedor
	IF @Vendedor <> 'TODOS'
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)		
	-- ** Filtro Pedido Simples
	IF @Simples <> 'TODOS' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' Data = CONVERT(VARCHAR, Pedido.Data_cadastro, 103) , Pedido.Pedido,'
		SET @String = @String + ' Cliente.Nome_Cliente, ISNULL(Pedido.Funcionario, ' + CHAR(39) + CHAR(39) + ') AS Vendedor, MERCADORIA.PLU, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String = @String + ' Qtde= CONVERT(NUMERIC(12,2), ISNULL(SUM(PEDIDO_ITENS.QTDE * Pedido_Itens.Embalagem), 0)), '
		SET @String = @String + ' Vlr = CONVERT(DECIMAL(12,2), ISNULL(SUM(PEDIDO_ITENS.TOTAL),0))'
		SET @String = @String + ' FROM Pedido '
		SET @String2 = ' INNER JOIN Pedido_ITENS ON PEDIDO_ITENS.Pedido = PEDIDO.Pedido and pedido.tipo=pedido_itens.tipo'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria ON Mercadoria.PLU = Pedido_itens.PLU '
		SET @String2 = @String2 + ' INNER JOIN Mercadoria_Loja ON MERCADORIA_LOJA.FILIAL=PEDIDO.FILIAL AND  Pedido_itens.PLU = MERCADORIA_LOJA.PLU '
		SET @String2 = @String2 + ' INNER JOIN Cliente ON cliente.Codigo_Cliente = pedido.Cliente_Fornec '
		SET @String2 = @String2 + @Where 
		SET @String2 = @String2 + ' GROUP BY MERCADORIA.PLU, '
		SET @String2 = @String2 + ' Pedido.Data_cadastro, Pedido.Pedido, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Venda_pedido_cliente]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Venda_pedido_cliente] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@vendedor   as varchar(30)
		
    	as
			-- exec sp_Rel_Venda_pedido_cliente 'MATRIZ','20141201','20141220','',''
	
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			[Total Venda]=ROUND(Total,2), 
			ISNULL(Pedido.funcionario, '') As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			(len(@cliente) =0 or  (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/',''))) 
		and 
			(@vendedor ='TODOS' OR Pedido.funcionario =@vendedor)
		
				 





GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_estoque]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_produtos_estoque]
end
GO
--PROCEDURES =======================================================================================
CREATE    procedure [dbo].[sp_rel_produtos_estoque]
@filial varchar(20) ,
@plu varchar(20)
,@ean varchar(20)
,@ref varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@saldo varchar(14)
,@LINHA VARCHAR(30)
as
Declare @String	As Varchar(MAX)
Declare @Fantasia as varchar(50)

--'Depois tem que tirar esse parametro direto 
Set @Fantasia = (select Max(Fantasia)  from filial)

begin

    SET @String = ' Select distinct a.PLU,'
    --begin 
    --'Depois tem que tirar esse parametro direto 
    --if(@Fantasia='CHAMANA')
		SET @String = @String + 'Ref = a.Ref_Fornecedor,'    
	--end	

    SET @String = @String + 'a.Descricao,'    
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    -- SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
   -- begin 
    
    --'Depois tem que tirar esse parametro direto 
   -- if(@Fantasia <> 'CHAMANA')
	--	SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
	-- End		
    SET @String = @String + 'isnull(l.Preco_Custo,0) AS [PRECO CUSTO],'
    SET @String = @String + 'isnull(l.Preco,0) AS [PRECO VENDA],'    
    SET @String = @String + 'isnull(l.saldo_atual,0) AS [SALDO ATUAL],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco_Custo,0)))[VALOR ESTOQUE CUSTO],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco,0)))[VALOR ESTOQUE VENDA]'    
    SET @String = @String + ' from '
    SET @String = @String + ' Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b '
    SET @String = @String + ' on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial) '
    SET @String = @String + ' inner join mercadoria_loja l on a.plu=l.PLU '
    SET @String = @String + ' left join Familia c on  a.Codigo_familia =c.Codigo_familia '
    SET @String = @String + ' left join EAN on a.plu = ean.plu '
    SET @String = @String + ' left join LINHA on a.Cod_Linha = LINHA.codigo_linha'
    
    SET @String = @String + ' where a.Inativo <>1 '
    begin 
    if(len(@plu)>0)
		SET @String = @String + ' AND a.PLU= ' + CHAR(39) + @plu + CHAR(39)
	end	
	begin
		if(LEN(@descricao)>0) 
			SET @String = @String + ' and a.Descricao like '+ CHAR(39) +'%'+@descricao+'%'+ CHAR(39) 
	end
	begin 
		if(LEN(@grupo)>0)
		SET @String = @String + ' and (b.Descricao_grupo = '+ CHAR(39) +@grupo+ CHAR(39) +')'
	end	
	begin
		if(LEN(@subGrupo)>0)
		SET @String = @String + ' and (b.descricao_subgrupo = '+CHAR(39)+@subGrupo+CHAR(39)+')'
	end
	begin 
		if(LEN(@departamento)>0)
			SET @String = @String + ' and (b.descricao_departamento = '+CHAR(39)+ @departamento+CHAR(39)+')'
	end
	begin
		if(LEN(@familia)>0)
			SET @String = @String + ' and (a.Descricao_familia = '+CHAR(39)+@familia+CHAR(39)+')' 
	end

		SET @String = @String + ' and l.Filial = '+char(39)+@filial+CHAR(39)
	begin
		if(LEN(@ean)>0)
		SET @String = @String + ' and (ean.EAN ='+char(39)+@ean+char(39)+')'
	end
	begin 
		if(LEN(@ean)>0)
		 SET @String = @String + ' 	and (a.Ref_fornecedor = '+char(39)+@ref+CHAR(39)+')'
	end
	begin
	if(@saldo='Igual a Zero')
		 SET @String = @String + ' 	and (l.saldo_atual=0)'
	end
	begin
	if(@saldo='Menor que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual<0)'
	end
	begin
	if(@saldo='Maior que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual>0)'
	end
	if(LEN(@LINHA)>0 )
	BEGIN
		SET @String = @String + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
	END
	
	--PRINT @STRING	
	 EXECUTE(@String)	
  end




go

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_NotasFiscais]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_NotasFiscais]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial           varchar(20),                -- Loja

      @DataDe           varchar(8),                 -- Data Inicial

      @DataAte          varchar(8),                 -- Data Fim

      @Tipo             varchar(20),                -- Tipo = 1 - Saidas, Tipo = 2 - Entrada

      @Nota             varchar(20),                -- Nmero Nota Fiscal

      @Fornecedor       varchar(20),                -- Nome Fornecedor
      
      @NF_CANC          varchar(20),		    -- Normal = NAO, Cancelada = SIM
      @plu			   varchar(20),
      @ean			   varchar(20),
      @ref             varchar(20),
      @descricao		varchar(40),
      @NF_Devolucao    varchar(20),
      @Excluir_Natureza varchar(20),
      @Incluir_Natureza varchar(20)	

As

      DECLARE @NF varchar(5000)
      --Verifica se sera aplicado filtro por Nota

      IF LTRIM(RTRIM(@Nota)) <> ''
		begin
            SET @NF = 'Select isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor, Plu, Ref=CODIGO_REFERENCIA,nf_item.Descricao,nf_item.Qtde,nf_item.Embalagem,nf_item.Unitario,nf_item.Total '
            SET @NF = @nf +' from NF_Item INNER JOIN NF ON NF.CODIGO=NF_ITEM.CODIGO and nf.Tipo_NF = nf_item.Tipo_NF and nf.Cliente_Fornecedor= nf_item.Cliente_Fornecedor INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '
			SET @NF = @nf+'	where nf.codigo = '+@nota
			--SET @NF = @NF + 'AND Not Exists(Select * From nf_inutilizadas i Where Convert(varchar,i.N_Inicio) >= NF.Codigo And Convert(varchar,i.N_Fim) <= NF.Codigo) '
			
			IF LTRIM(RTRIM(@Tipo)) = '1-Saida'
				SET @NF = @NF + 'AND NF.Tipo_NF = 1 '
			IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'
				SET @NF = @NF + 'AND NF.Tipo_NF = 2 '
			IF LTRIM(RTRIM(@Fornecedor)) <> ''
                SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

		end
	else
	begin
     

      SET @NF = 'SELECT NF.Filial,Tipo= case when nf.tipo_nf=1 then '+CHAR(39)+'SAIDA'+CHAR(39)+' else '+CHAR(39)+'ENTRADA'+CHAR(39)+' END, NF.Codigo ' + 'Nota' + ', isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emisso'

      SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ',Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) ' + 'VlrProd' + ', NF.Total ' + 'VlrNota'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '

      SET @NF = @NF + 'AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor AND NF.Filial = NF_Item.Filial INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '
      
      SET @NF = @NF + 'AND Not Exists(Select * From nf_inutilizadas i Where Convert(varchar,i.N_Inicio) >= NF.Codigo And Convert(varchar,i.N_Fim) <= NF.Codigo) '
      
      --Exibe todas as notas fiscais (canceladas)
      IF LTRIM(RTRIM(@NF_CANC)) = 'SIM'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 1 '
            

      --Exibe todas as notas fiscais (exceto canceladas)     
      IF LTRIM(RTRIM(@NF_CANC)) = 'NAO'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 0 '
      

      --Verifica se sera aplicado filtro por Filial

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 


 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 
	  begin
		if(LEN(@plu)>0)
		 SET @NF = @NF + ' and nf_item.plu='+CHAR(39)+@plu+CHAR(39)	
	  end
		
	
		if(@NF_Devolucao='SIM')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 1 '
		end
		if(@NF_Devolucao='NAO')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 0 '
		end
		
		if(LEN(@Excluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO NOT IN('+@Excluir_Natureza+') '
		end
		
		if(LEN(@Incluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO IN('+@Incluir_Natureza+') '
		end
		


      SET @NF = @NF + ' Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Total '

      SET @NF = @NF + ' Order By NF.Filial, Convert(Varchar,NF.Data,103), NF.Codigo '
	end
     
	
  execute(@NF)
--print @nf


GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Ficha_Cliente_Pedido]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_Ficha_Cliente_Pedido]
end
GO
--PROCEDURES =======================================================================================
CREATE   procedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@Codigo_cliente varchar(50),
@status varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples	as varchar(5)	
)
--sp_rel_fin_Ficha_Cliente_Pedido 'MATRIZ', '20160101', '20160220', 'EMISSAO', '', '', '', '', ''
As

Declare @String as nvarchar(4000)
Declare @Where as nvarchar(4000)

SET @AVista = CASE
WHEN @AVista = 'AVISTA' THEN 'AV'
WHEN @AVista = 'PRAZO' THEN 'PZ'
ELSE ''
END
Begin
--Monta Clausula Where da Procura
set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo
if len(rtrim(ltrim(@Codigo_cliente))) > 0
Begin
set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
End

if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE  ' and Conta_a_receber.status =1' end --'status like '+CHAR(39)+'%'+CHAR(39) END

)

end
if LEN(@status)=0
begin
set @Where = @Where + ' and Conta_a_receber.status <>3'

end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF LEN(@AVista ) >0
begin
SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) And CONVERT(VARCHAR, PD.pedido_simples) LIKE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'SIM' + CHAR(39) + ' THEN  '+ CHAR(39) +  '1' + CHAR(39) + '  ELSE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'NAO' + CHAR(39) + ' THEN  '+ CHAR(39) +  '0' + CHAR(39) + ' ELSE ' + CHAR(39) +  '%' + CHAR(39) + 'END END '



--Monta Select
set @string = 'select
Simples_Cliente = Ltrim(Rtrim(Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ' + ' + CHAR(39) + SPACE(1) + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente)))) ' + ', 
Documento = rtrim(ltrim(documento)),
Codigo = Convert(int,Ltrim(Rtrim(cliente.Codigo_Cliente))),
Cliente = Substring(Ltrim(Rtrim(cliente.nome_Cliente)),1,1),
Nome = ' + CHAR(39) + 'VALOR TOTAL DO CLIENTE ' + CHAR(39) + ',
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
Prazo = Convert(Varchar,DATEDIFF(DAY,GETDATE(), vencimento )) ,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' into #t from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.nome_Cliente, cliente.Codigo_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

--Set @String = @string + 'insert into #t select Simples_Cliente, ' + CHAR(39) + CHAR(39) + ',Codigo,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome order by 3 '
Set @String = @string + 'insert into #t select Simples_Cliente + ' + CHAR(39) + ' - Sub Total' + CHAR(39) + ', ' + CHAR(39) +  CHAR(39) + ',Codigo,cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select Nome, ' + CHAR(39) + CHAR(39) + ',Codigo,Cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select ' + CHAR(39) + 'TOTAL GERAL - FICHA DE CLIENTE' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' +  CHAR(39) + '999999' + CHAR(39) + ',' + CHAR(39) + 'Z' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + 'SUM(VlrReceber),Sum(VlrAberto),' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t WHERE NOME = '+ CHAR(39) + 'VALOR TOTAL DO CLIENTE' + CHAR(39) 
Set @String = @string + 'Select Simples_Cliente , Documento, VlrReceber, VlrAberto, Emissao, Vencimento, Convert(Varchar,Prazo) Prazo, TipoRecebimento, Convert(int,Codigo) Codigo, Cliente From #t Where Simples_Cliente <> ' + CHAR(39) + CHAR(39) + ' Order by 10,9,1,2,3'
--Union Select ' + CHAR(39) + 'TOTAL CLIENTE' + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente)))) ' + ', ' + CHAR(39) + CHAR(39) + ',sum(VlrReceber), SUM(VlrAberto),'+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ', Codigo from #t group by Codigo Order by Simples_Cliente '
-- set @string = @string + 'Documento = rtrim(ltrim(documento)), '
--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
--set @string = @string + @tipo + '= '+ @tipo + ', '
--set @string = @string + 'Total = valor - isnull(desconto,0), '
--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
--set @string = @string + 'From Conta_a_pagar '
--set @string = @string + @where
--set @string = @string + ' Order By convert('+ @Tipo +',102) '
--Print @string
Exec(@string)
End

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_cliente] 
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_cliente]  
  @Filial                 As Varchar(20),
  @codigo as varchar(30),
  @nome as varchar(50),
  @cnpj as varchar(20),
  @tabela as varchar(10),
  @tipoRel as varchar(40)
  
  as
  
  Begin
  
  --[sp_rel_cliente] 'MATRIZ','','','','TODOS','SIM'


		if(@tipoRel = 'NAO')
		BEGIN
		 select '|-CODIGO:-|'+a.codigo_cliente+ '|| |-NOME-|: '+a.nome_cliente +' || |-CNPJ-|: '+ isnull(a.cnpj,'_____________________')+ '  |-IE-|: '+ISNULL(a.Ie,'_____________________')+  '|| ||' AS 'DADOS GERAIS',
			'|-ENDERECO-|:'+isnull(a.endereco,'_______________________________') +','+isnull(a.endereco_nro,'___')+' |-BAIRRO-|:'+isnull(a.Bairro,'____________')+' |-CIDADE-|:'+ isnull(a.cidade,'_________')+' |-UF-|:'+isnull(a.Uf,'__') + '||' +
			'|-DATA CADASTRO-|:'+ convert(varchar,a.data_cadastro,103)+ ' || |-TELEFONE-| :' + ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%') ),'_____________') +
			' |-E-MAIL-|:'+ ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'),'_________________________')  +  '|| ||'
			as 'DETALHES' --into #ConsultaCliente
			from cliente as  a
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)	
		END
		ELSE
		BEGIN

		  select 
				a.codigo_cliente As CODIGO, 
				Ltrim(Rtrim(a.nome_cliente)) as NOME,
				TELEFONE= 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%')), ''),
				EMAIL = 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'), '')
			from cliente as  a 
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)
			Order by 2				  
		
			
		   END
END

go