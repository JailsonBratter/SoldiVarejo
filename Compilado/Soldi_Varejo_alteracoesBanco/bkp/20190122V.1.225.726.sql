
delete ean from ean left join mercadoria on ean.plu = mercadoria.plu
where mercadoria.plu is null


delete f from Fornecedor_Mercadoria as f left join mercadoria as m on f.plu=m.plu
where m.plu  is null

GO

IF OBJECT_ID('[sp_rel_Resumo_Vendas_hora]', 'P') IS NOT NULL
begin 
      drop procedure [sp_rel_Resumo_Vendas_hora]
end 

go
CREATE PROCEDURE  [dbo].[sp_rel_Resumo_Vendas_hora]
            @Filial		  As Varchar(20),
            @DataDe		  As Varchar(8),
            @DataAte	  As Varchar(8),
			@ini_periodo varchar(5),
			@fim_periodo varchar(5),
            @plu		  As Varchar (20),
            @descricao	  As varchar(50),
            @grupo	      As Varchar(50),
            @subGrupo	  As varchar(50),
            @departamento as varchar(50),
            @relatorio	  as varchar(40),
			@tipo		  as varchar(40)

 

AS
-- EXEC [sp_rel_Resumo_Vendas_hora] 'MATRIZ','20180111','20180112','11:00','11:30','','','','','','TODOS','ANALITICO'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 
-- @tipo = 'ANALITICO', 'CONSOLIDADO'	
IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
begin
	DROP TABLE #tmpVendas;  
end
CREATE TABLE #tmpVendas
(
	Data DATETIME,
	plu varchar(50),
	ean varchar(15),
	Ref varchar(50),
	descricao varchar(100),
	qtd decimal(18,2),
	total Decimal(18,2)
)

begin
--  @dias_periodo int
--select @ini_periodo = inicio_periodo
--	 , @fim_periodo =fim_periodo
--	 , @dias_periodo = dias_periodo 
		
--from filial where filial = @filial
		



if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN

  

insert into #tmpVendas
 SELECT
		S.DATA_MOVIMENTO,
		S.PLU,
		
		EAN = (SELECT TOP 1 EAN FROM EAN WHERE EAN.PLU = M.PLU),
		M.Ref_fornecedor,
		M.Descricao,
		S.Qtde,
		((ISNULL(S.vlr,0) +ISNULL(S.Acrescimo,0))-ISNULL(S.Desconto,0))
      FROM
            Saida_Estoque AS S with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = S.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            S.Filial = @Filial
	  AND (LEN(@PLU)=0 OR S.PLU = @plu)
	  AND (S.Data_Movimento BETWEEN @DataDe AND @DataAte)
	  AND (S.Hora_venda BETWEEN @ini_periodo AND @fim_periodo)
	  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	  AND S.data_cancelamento IS NULL
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
		N.Emissao,
		ni.PLU,	
		EAN = (SELECT TOP 1 EAN FROM EAN WHERE EAN.PLU = M.PLU),
		M.Ref_fornecedor,
		M.Descricao,
		qtde =(ni.Qtde*ni.Embalagem),
		ni.Total

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao BETWEEN @DataDe  AND @DataAte )
	AND (N.emissao_hora BETWEEN @ini_periodo AND @fim_periodo)
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
END


if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
BEGIN
insert into #tmpVendas
select 
		P.Data_cadastro,
	pit.PLU,	
		EAN = (SELECT TOP 1 EAN FROM EAN WHERE EAN.PLU = M.PLU),
		M.Ref_fornecedor,
		M.Descricao,
		qtde =(pit.Qtde*pit.Embalagem),
		pit.Total


from  Pedido as p with (index(ix_pedido_fluxo_caixa))
inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
inner join mercadoria as m on m.PLU = pit.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
WHERE p.pedido_simples = 1   
	AND  (p.Data_cadastro BETWEEN @DataDe   AND @DataAte )
	AND (P.hora_cadastro BETWEEN @ini_periodo AND @fim_periodo)
	 and isnull(p.Status,0)<>3
	 AND   p.TIPO = 1	
	 and p.FILIAL=@filial 
	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)

END



IF(@tipo='CONSOLIDADO')
BEGIN 
	SELECT PLU,
		   EAN,
		   REF,
		   DESCRICAO,
		   QTDE ,
		   TOTAL 
		   FROM (
	Select 
		ORDEM = 'AAAAAAA',
		PLU= 'SFT_'+PLU,
		EAN= 'SFT_'+EAN,
		REF= 'SFT_'+REF,
		DESCRICAO,
		QTDE = SUM(QTD),
		TOTAL = SUM(TOTAL) 
	from #tmpVendas
	GROUP BY PLU,EAN,REF,descricao
	UNION ALL 
	Select 
		ORDEM ='ZZZZZZ99999',
		PLU='TOTAL |-SUB-|',
		EAN='',
		REF='',
		DESCRICAO='',
		QTDE = SUM(QTD),
		TOTAL = SUM(TOTAL) 
	from #tmpVendas
	) AS A 
	ORDER BY ORDEM ,TOTAL DESC
END 
ELSE
BEGIN 
	SELECT PLU,
		   EAN,
		   REF,
		   DESCRICAO,
		   QTDE ,
		   TOTAL 
		   FROM (
	Select 
		ORDEM =CONVERT(VARCHAR,DATA,102),
		PLU= 'SFT_'+PLU,
		EAN= 'SFT_'+EAN,
		REF= 'SFT_'+REF,
		DESCRICAO,
		QTDE = SUM(QTD),
		TOTAL = SUM(TOTAL) 
	from #tmpVendas
	GROUP BY PLU,EAN,REF,descricao,DATA
	UNION ALL 
	Select 
		ORDEM =CONVERT(VARCHAR,DATA,102),
		PLU=CONVERT(VARCHAR,DATA,103)+ '|-TITULO-||CONCAT|',
		EAN='',
		REF='',
		DESCRICAO='',
		QTDE = 0,
		TOTAL = 999999999999
	from #tmpVendas
	GROUP BY DATA
	UNION ALL 
	Select 
		ORDEM =CONVERT(VARCHAR,DATA,102)+'ZZZZ',
		PLU= '',
		EAN='',
		REF='',
		DESCRICAO='TOTAL DIA '+CONVERT(VARCHAR,DATA,103)+'|-SUB-|',
		QTDE = SUM(QTD),
		TOTAL = SUM(TOTAL)
	from #tmpVendas
	GROUP BY DATA
	UNION ALL 
	Select 
		ORDEM ='ZZZZZZ99999',
		PLU='TOTAL|-SUB-|',
		EAN='',
		REF='',
		DESCRICAO='',
		QTDE = SUM(QTD),
		TOTAL = SUM(TOTAL) 
	from #tmpVendas
	) AS A 
	ORDER BY ORDEM ,TOTAL DESC
END 
end



go

insert into Versoes_Atualizadas select 'v.1.225.725', getdate();
GO




