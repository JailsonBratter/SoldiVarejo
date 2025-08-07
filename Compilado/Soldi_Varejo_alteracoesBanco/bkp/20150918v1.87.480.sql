--CRIA NOVOS CAMPOS =====================================================================================================================================

IF ( COL_LENGTH( 'nf_item', 'base_pis' ) IS NULL )
BEGIN
			alter table nf_item add 
				base_pis numeric(12,2),  
				base_cofins numeric(12,2),
				aliquota_pis numeric(12,2),
				aliquota_cofins numeric(12,2),
				cst_icms varchar(5),
				base_icms numeric(12,2),
				icmsv numeric(12,2)
END

go

IF ( COL_LENGTH( 'nf', 'base_pis' ) IS NULL )
BEGIN
			alter table nf add 
				base_pis numeric(12,2),  
				base_cofins numeric(12,2),
				pisv numeric(12,2),
				cofinsv numeric(12,2),
				total_produto numeric(12,2)
				
END

--PROCEDURE ===============================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LX_SEQUENCIAL]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [LX_SEQUENCIAL]
end
GO

create PROCEDURE [dbo].[LX_SEQUENCIAL] @TABELA_COLUNA VARCHAR(37), @EMPRESA INT = NULL, @SEQUENCIA VARCHAR(20) OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE	@SEQUENCIA_ANT VARCHAR(20), 
		@TAMANHO INT, 
		@PERMITE_POR_EMPRESA SMALLINT,
		@EXECUTA SMALLINT,
		@errno   int,
		@errmsg  varchar(255)

	SET

	SELECT @EXECUTA=1
	WHILE @EXECUTA=1
	BEGIN
		SELECT 	@SEQUENCIA_ANT	= ISNULL(SEQUENCIA,0), 
			@TAMANHO	= ISNULL(TAMANHO,DATALENGTH(RTRIM(SEQUENCIA))), 
			@PERMITE_POR_EMPRESA =0
		FROM SEQUENCIAIS 
		WHERE TABELA_COLUNA=@TABELA_COLUNA
		IF @@ROWCOUNT=0
		BEGIN
			SELECT 	@ERRNO=50001,
				@ERRMSG='O sequencial #'+RTRIM(@TABELA_COLUNA) +' #nao existe na tabela de sequenciais !!!'
			GOTO error
		END

		BEGIN
			SELECT @SEQUENCIA=RIGHT(REPLICATE('0',@TAMANHO)+CONVERT(VARCHAR(20),CONVERT(INT,@SEQUENCIA_ANT)+1),@TAMANHO)

			UPDATE 	SEQUENCIAIS 
			SET 	SEQUENCIA = @SEQUENCIA
			WHERE 	TABELA_COLUNA=@TABELA_COLUNA AND 
				SEQUENCIA=@SEQUENCIA_ANT
			
		END
		IF @@ROWCOUNT=1 OR @@ERROR <> 0
			SELECT @EXECUTA=0
	END


RETURN
error:
    raiserror @errno @errmsg
RETURN
END






--PROCEDURE =============================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]
end
GO

create PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE](
--declare
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@GRUPO as Varchar(40), 
@SUBGRUPO as Varchar(40),
@DEPARTAMENTO AS VARCHAR(40),
@FAMILIA    AS VARCHAR(40)
)
As


DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

--set @DataInicio ='20150302'
--set @DataFim ='20150330'
--Set @GRUPO ='PADARIA'
--set @SUBGRUPO ='PAES'
--set @DEPARTAMENTO='PAES'
--set @FAMILIA =''
--drop table #OUTROS

Create table #OUTROS
(
	PLU  VARCHAR(17) COLLATE SQL_Latin1_General_CP1_CI_AS,
	Ref_Fornecedor varchar(8) COLLATE SQL_Latin1_General_CP1_CI_AS,
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
	OUTRAS_MOVIMENTACOES DECIMAL(18,2),
	NF_Saida DECIMAL(18,2),
	NF_Saida_Preco	 DECIMAL(18,2),
	Cupom DECIMAL(18,2),  
	Cupom_Preco DECIMAL(18,2)
);

--select * from pedido_itens

IF @PedEstoque = 'TRUE'
INSERT INTO  #OUTROS select PLU =itemPd.PLU  ,Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
		   ,[NF Saida] =Sum(itemPd.Qtde*itemPd.Embalagem)
		   ,[NF Saida Preco]	= Sum(itemPd.total)
		  ,[Cupom] = 0  
		  ,[Cupom Preco]=0
	 from pedido_itens itemPd inner join pedido pd on pd.Filial=@FILIAL and  pd.Pedido = itempd.Pedido and  pd.Tipo = itemPd.tipo
		inner join Natureza_operacao Eop on pd.cfop = Eop.Codigo_operacao
		INNER JOIN Mercadoria M ON M.PLU=itemPd.PLU
		INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
	where (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
		AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
		AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
		AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
		AND  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
		and pd.Filial = @FILIAL
	group by itemPd.PLU , m.Ref_Fornecedor
ELSE
begin
INSERT INTO  #OUTROS 
	select PLU =se.PLU  , Ref_Fornecedor = Isnull(M.Ref_fornecedor,''), 
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
		   ,[NF Saida] =0
		   ,[NF Saida Preco]	= 0
		  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
		  ,[Cupom Preco]=Sum(se.vlr)	
	from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
	where (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
		AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
		AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
		AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
		and  se.Data_movimento between @DataInicio AND @DataFim
		and se.Filial = @FILIAL
	group by se.PLU , m.Ref_Fornecedor
END;



Select PLU =todos.PLU ,Ref_fornecedor= isnull(m.Ref_Fornecedor,'')
		,DESCRICAO = M.DESCRICAO
		,[Entrada]=Sum(todos.NF_ENTRADA)
		,[Entrada_Valor]=Sum(todos.NF_ENTRADA_PRECO)
		,[Outras_Mov]=SUM(OUTRAS_MOVIMENTACOES)
		,[Saida_Outros] = SUM(todos.NF_Saida)
		,[Saida_Valor] = Sum(todos.NF_Saida_Preco)
		,[Cupom] = SUM(todos.Cupom)
		,[Cupom_Valor] = Sum(todos.Cupom_Preco)
	 from (

select PLU =isnull(A.PLU,''), Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
	   [NF_ENTRADA] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE Eitem.PLU = A.PLU and (Enf.Data between @DataInicio AND @DataFim ) 
							    AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF_ENTRADA_PRECO]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							where  Eitem.PLU = A.PLU  and (Enf.Data between @DataInicio AND @DataFim ) 
									 AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1   ),0) 				
	   ,[OUTRAS_MOVIMENTACOES]=0
	   ,[NF_Saida] =isnull((Select SUM (isnull(sitem.Qtde,0)*ISNULL(sitem.Embalagem,0)) 
								from NF_Item Sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							where  Sitem.PLU = A.PLU   and (Snf.Data between @DataInicio AND @DataFim ) 
							AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 ),0) 
	   ,[NF_Saida_Preco]	= isnull((Select Sum(isnull(sitem.Total,0)) 
							from NF_Item sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							where  Sitem.PLU = A.PLU and (Snf.Data between @DataInicio AND @DataFim  )
							 AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and Sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1   ),0) 				
	  ,[Cupom] = 0.00-- isnull((Select SUM(isnull(se.Qtde,0)) from Saida_estoque se with( index(IX_Saida_Estoque_01)) where se.plu =@plu and se.Data_movimento = b.data),0) 
	  ,[Cupom_Preco]=0.00	
		
from NF_Item a inner join nf b on a.codigo = b.Codigo 
				inner join Natureza_operacao op on b.Codigo_operacao = op.Codigo_operacao 
				INNER JOIN Mercadoria M ON M.PLU =A.PLU
							INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
							WHERE (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
								AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
								AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
								AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
								AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 
								and a.Filial=@FILIAL
					GROUP BY A.PLU , m.Ref_Fornecedor


UNION ALL
Select
	PLU   , Ref_Fornecedor ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	[OUTRAS_MOVIMENTACOES]=0,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by PLU ,Ref_Fornecedor 
 UNION ALL
Select ITEM.PLU, Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
		NF_ENTRADA = 0,
		NF_ENTRADA_PRECO = 0 ,
		OUTRAS_MOVIMENTACOES =  SUM(case when tm.Saida =0 then ISNULL(item.Contada,0) 
													 when tm.saida=1 then (ISNULL(item.contada,0)*-1) 
													 when tm.saida=2 then (ISNULL(item.Contada,0)-ISNULL(item.Saldo_atual,0))
													end 
													   )  ,
		NF_Saida= 0,
		NF_Saida_Preco= 0	 ,
		Cupom = 0,  
		Cupom_Preco= 0 								   
	from Inventario inv 
			inner join Tipo_movimentacao tm on inv.tipoMovimentacao = tm.Movimentacao
			inner join Inventario_itens item on inv.Codigo_inventario = item.Codigo_inventario and inv.Filial = item.Filial
			INNER JOIN Mercadoria M ON M.PLU =ITEM.PLU
							INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
							
	WHERE (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
								AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
								AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
								AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
								and inv.Data between @DataInicio AND @DataFim   and status='ENCERRADO' AND INV.Filial = @FILIAL
	group by Item.PLU , m.Ref_Fornecedor




) as todos
INNER JOIN Mercadoria M ON M.PLU =todos.PLU
group by todos.PLU, m.Ref_Fornecedor, 
M.Descricao
order by CONVERT(DECIMAL,todos.PLU)  

GO

--Procedure ============================================================================================================================


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_estoque]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_produtos_estoque]
end
GO

create  procedure [dbo].[sp_rel_produtos_estoque]
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
as
Declare @String	As Varchar(MAX)
Declare @Fantasia as varchar(50)

--'Depois tem que tirar esse parametro direto 
Set @Fantasia = (select Fantasia  from filial)

begin

    SET @String = ' Select a.PLU,'
    begin 
    --'Depois tem que tirar esse parametro direto 
    if(@Fantasia='CHAMANA')
		SET @String = @String + 'Ref_Forn = a.Ref_Fornecedor,'    
	end	

    SET @String = @String + 'a.Descricao,'    
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
    begin 
    
    --'Depois tem que tirar esse parametro direto 
    if(@Fantasia <> 'CHAMANA')
		SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
	End		
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
	--PRINT @STRING	
	 EXECUTE(@String)	
  end


GO

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_pedido_simplificado]    Script Date: 09/17/2015 22:44:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--procedure=====================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_simplificado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Venda_pedido_simplificado] 
end
GO

create   PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@simples	as varchar(5),		
		@cliente   as varchar(40),
		@Vendedor	as Varchar(30) = ''
		
    	as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20141201','20141220','',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),2),UNITARIO = ROUND( UNITARIO ,2),'','','',''
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario
	UNION ALL
	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),0),0,'','','','' 
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte

	--GROUP BY P.PLU,M.Descricao,P.unitario	
	
end
else		
begin
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			--VlrVenda=ROUND(Total,2), 
			
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
			FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
			where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			
			[Total Venda]=ROUND(Total,2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
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
			Pedido.funcionario like (case when @Vendedor  <> '' then @Vendedor else '%' end)
				 
 end

go

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Pedido_Venda_Analitico]    Script Date: 09/17/2015 22:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--procedure================================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Pedido_Venda_Analitico]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Pedido_Venda_Analitico] 
end
GO

create PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Analitico]
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

	SET @Where = ' WHERE Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
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
	IF @Vendedor <> ''
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)		
	-- ** Filtro Pedido Simples
	IF @Simples <> '' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' Data = CONVERT(VARCHAR, Pedido.Data_cadastro, 102) , Pedido.Pedido,'
		SET @String = @String + ' Cliente.Nome_Cliente, ISNULL(Pedido.Funcionario, ' + CHAR(39) + CHAR(39) + ') AS Vendedor, MERCADORIA.PLU, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String = @String + ' Qtde= CONVERT(NUMERIC(12,2), ISNULL(SUM(PEDIDO_ITENS.QTDE * Pedido_Itens.Embalagem), 0)), '
		SET @String = @String + ' Vlr = CONVERT(DECIMAL(12,2), ISNULL(SUM(PEDIDO_ITENS.TOTAL),0))'
		SET @String = @String + ' FROM Pedido '
		SET @String2 = ' INNER JOIN Pedido_ITENS ON PEDIDO_ITENS.Pedido = PEDIDO.Pedido'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria ON Mercadoria.PLU = Pedido_itens.PLU'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria_Loja ON Pedido_itens.PLU = MERCADORIA_LOJA.PLU '
		SET @String2 = @String2 + ' INNER JOIN Cliente ON cliente.Codigo_Cliente = pedido.Cliente_Fornec '
		SET @String2 = @String2 + @Where 
		SET @String2 = @String2 + ' GROUP BY MERCADORIA.PLU, '
		SET @String2 = @String2 + ' Pedido.Data_cadastro, Pedido.Pedido, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)

GO

--procedure======================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Movimento_Venda] 
end
GO

create Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),

                @plu               As varchar(17),

                @cupom             As varchar(20),

                @pdv               as varchar(2),
                
                @horaInicio      as varchar(5),
				
				@horafim	     as varchar(5),
				@cancelados		as varchar(5)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,

                             VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),

                             FINALIZADORA = lista.id_finalizadora,
							
							[COMANDA/PEDIDOS] =  (SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)
                             

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                                   and pdv like (case when @pdv <> '' then @pdv else '%' end)
								   and (
										@cancelados='TODOS' 
										OR (@cancelados ='SIM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='NAO' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))=0) ) 
																						   
						GROUP BY lista.Emissao, lista.pdv, lista.Documento ,lista.id_finalizadora

           

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR =(SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                         ),
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento
                                    ),

                             FINALIZADORA = id_finalizadora,
                             
                             [COMANDA/PEDIDOS] = (SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 

                         and pdv like (case when @pdv <> '' then @pdv else '%' end)
						 and (
										@cancelados='TODOS' 
										OR (@cancelados ='SIM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='NAO' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))=0) ) 
									
                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='')

BEGIN

      SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),
                        Hora = convert(varchar,Hora_venda),

                        pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )

                        and (len(@plu)=0 or s.PLU = @plu )

                        And s.Data_Cancelamento is null

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

						 and s.Data_movimento between @DataDe aND @DataAte
                         and (LEN(@pdv)=0 or l.pdv = @pdv)
                         and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 

                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by l.Emissao , Hora_venda

      END

ELSE

      BEGIN

           

            SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),
					    pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                       
						union all

--                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
						SELECT '',

                        '|-CANCELADO-|',
					     pdv=convert(varchar,L.pdv) ,

                        '|-'+S.PLU+'-|',

                        '|-'+M.Descricao+'-|',

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total='0.000' 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is NOT null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                        
                        union all

                        select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total)))

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END


GO

--PROCEDURE==================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_FluxoCaixa] 
end
GO

create            PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End

	
	Create table Totais
(
	Descricao  varchar(90),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte),0)
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND FILIAL = @FILIAL AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', CONVERt(VARCHAR,Sum(isnull(convert(decimal(18,2),Total,0),0))) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte
	UNION ALL
	SELECT 'VENDAS CUPOM',CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0))
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP',  CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and Filial = @filial and len(isnull(cpf_cnpj,''))>10)))
	UNION ALL
	SELECT '% DE VENDAS COM NFP', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100))
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS', CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0)))
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL AND Filial = @FILIAL)),0))	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'NF DEVOLUCAO'

	UNION ALL
	
	SELECT 'RESULTADO TOTAL',
		CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO')))
	FROM  TOTAIS
	Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial)))
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial)))
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
    SELECT 'GERENCIAL ',CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE with( index(IX_Saida_Estoque)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0))
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,CONVERt(VARCHAR,SUM(TOTAL))
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',CONVERt(VARCHAR,SUM(TOTAL))
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  

GO


--PROCEDURE =====================================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Posicao_Cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_Posicao_Cliente]
end
GO

create  procedure [dbo].[sp_rel_fin_Posicao_Cliente](
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

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)

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
if len(rtrim(ltrim(@valor))) > 1
Begin
set @where = @where + ' And valor ='+REPLACE(@valor,',','.')
End
if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END

)
end

if LEN(@centrocusto)>0
begin
set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39)
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
Simples = Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ',
Documento = rtrim(ltrim(documento)),
Cliente = rtrim(ltrim(cliente.Codigo_Cliente)),
Nome_Cliente = rtrim(ltrim(cliente.nome_Cliente)),
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.Codigo_Cliente, cliente.nome_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

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
-- Print @string
Exec(@string)
End


GO

--PROCEDURE ==============================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Receb_SIMPLIFICADO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_Receb_SIMPLIFICADO]
end
GO

create procedure [dbo].[sp_rel_fin_Receb_SIMPLIFICADO](
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
if len(rtrim(ltrim(@Codigo_cliente))) > 1
Begin
set @where = @where + ' And cliente.Codigo_Cliente like ' + char(39) +'%' + @Codigo_cliente + '%' +char(39)
End
if len(rtrim(ltrim(@valor))) > 1
Begin
set @where = @where + ' And valor ='+REPLACE(@valor,',','.')
End
if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
)
end

if LEN(@centrocusto)>0
begin
set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39)
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
Simples = Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ',
Recebimento = Case when rtrim(ltrim(Conta_a_receber.tipo_Recebimento)) like ' + CHAR(39) + '%Carteira%' + CHAR(39) + ' Then ' + CHAR(39) + 'CARTEIRA' + CHAR(39) + ' when rtrim(ltrim(Conta_a_receber.tipo_Recebimento)) like ' + CHAR(39) + '%Cheque%' + CHAR(39) + ' Then ' + CHAR(39) + 'CHEQUE' + CHAR(39) + ' when rtrim(ltrim(Conta_a_receber.tipo_Recebimento)) like ' + CHAR(39) + '%Cartao%' + CHAR(39) + ' Then ' + CHAR(39) + 'CARTAO' + CHAR(39) + ' when rtrim(ltrim(Conta_a_receber.tipo_Recebimento)) like ' + CHAR(39) + '%Boleto%' + CHAR(39) + ' Then ' + CHAR(39) + 'BOLETO' + CHAR(39) + ' Else rtrim(ltrim(Conta_a_receber.tipo_Recebimento)) END , 
VlrReceber = Sum(Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0)),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Sum(Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0)-Isnull(Conta_a_receber.Valor_Pago,0)) else 0 end

from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Group by conta_a_Receber.status,dbo.Tipo_Pagamento.A_Vista,
Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ',
Case when rtrim(ltrim(tipo_Recebimento)) like ' + CHAR(39) + '%Carteira%' + CHAR(39) + ' Then ' + CHAR(39) + 'CARTEIRA' + CHAR(39) + ' when rtrim(ltrim(tipo_Recebimento)) like ' + CHAR(39) + '%Cheque%' + CHAR(39) + ' Then ' + CHAR(39) + 'CHEQUE' + CHAR(39) + ' when rtrim(ltrim(tipo_Recebimento)) like ' + CHAR(39) + '%Cartao%' + CHAR(39) + ' Then ' + CHAR(39) + 'CARTAO' + CHAR(39) + ' when rtrim(ltrim(tipo_Recebimento)) like ' + CHAR(39) + '%Boleto%' + CHAR(39) + ' Then ' + CHAR(39) + 'BOLETO' + CHAR(39) + ' Else rtrim(ltrim(tipo_Recebimento)) END 
Order by 1, 2 '

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






       