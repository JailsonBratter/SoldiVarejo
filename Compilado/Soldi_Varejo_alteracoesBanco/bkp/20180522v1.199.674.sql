
/****** Object:  StoredProcedure [dbo].[sp_estoque_sugestao_compra]    Script Date: 05/22/2018 11:35:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


ALTER  procedure [dbo].[sp_estoque_sugestao_compra](
            @FILIAL VARCHAR(30),
            @plu varchar(20),
            @ean varchar(20),
            @descricao varchar(50),
            @ncm varchar(20),
            @campo1 varchar(20),
            @onde varchar(5),
            @campo2 varchar(20),
            @refForn varchar(30),
            @grupo varchar(40),
            @subgrupo varchar(50),
            @departamento varchar(50),
            @familia varchar(50),
            @tipoData Varchar(30),
            @dtDe Varchar(15),
            @dtAte Varchar(15),
            @ordem varchar(30),
            @prazo int,
            @conEmbalagem int,
            @ultimo_fornec varchar(40)
            
           
           
)
as
-- execute sp_estoque_sugestao_compra 'MATRIZ','','','COCA','','','=','0','','','','','','','','',null,5,0,''
           

begin
        Declare @strSqlItens  As nVarchar(1024)
        -- SELECT* FROM MERCADORIA sp_help fornecedor_mercadoria
        -- SELECT * FROM Acumulado_Geral
        --    SELECT CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)
         
                SET @strSqlItens= 'Select  a.plu as PLU,ean =(Select top 1 ean from ean where plu = a.plu),REF =ISNULL(a.Ref_Fornecedor,'+ CHAR(39) ++ CHAR(39) +'), a.DESCRICAO  ,EMB =A.EMBALAGEM , SALDO=ISNULL(l.Saldo_atual,0), PRC_CUSTO= l.Preco_Custo,l.margem,PRC_VENDA=l.Preco , COB_CAD = ISNULL(A.COBERTURA,0) '
        SET @strSqlItens= @strSqlItens+' INTO ##ITENS from mercadoria a  inner join mercadoria_loja l on l.plu=a.plu    inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao '
        SET @strSqlItens= @strSqlItens+' left join W_BR_CADASTRO_DEPARTAMENTO d on (a.codigo_departamento= d.codigo_Departamento and a.filial=l.filial) left join Familia f on a.Codigo_familia = f.Codigo_familia '
        if(LEN(@ultimo_fornec)>0)
        begin
			        SET @strSqlItens= @strSqlItens+' inner join  fornecedor_mercadoria fmerc on (a.plu= fmerc.plu and a.filial=fmerc.filial)'
        end
        
        
        SET @strSqlItens= @strSqlItens+' WHERE L.FILIAL = '+ CHAR(39) + @FILIAL+ CHAR(39) + ' and a.inativo<>1 '
		if(LEN(@plu)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu= '+ CHAR(39) + @plu+ CHAR(39)
        end
        if(LEN(@ean)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu = (Select plu from ean where plu = '+ CHAR(39) + @ean+ CHAR(39)+' ) '
        end
        if(LEN(@descricao)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.descricao like '+ CHAR(39)+'%' + @descricao+'%' + CHAR(39)
        end
        if(LEN(@campo1)>0 and LEN(@campo2)>0 and len(@onde)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and ' + @campo1+ ' ' + @onde+ ' ' + @campo2;
        end
        if(LEN(@ncm)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and cf= '+ CHAR(39) + @ncm+ CHAR(39)
        end
        if(LEN(@grupo)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_grupo = '+ CHAR(39) + @grupo+ CHAR(39)
        end
        if(LEN(@subgrupo)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_subgrupo = '+ CHAR(39) + @subgrupo+ CHAR(39)
        end
       
        if(LEN(@departamento)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_departamento = '+ CHAR(39) + @departamento+ CHAR(39)
        end
        if(LEN(@familia)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and f.descricao_familia = '+ CHAR(39) + @familia+ CHAR(39)
        end
        if(LEN(@tipoData)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and '+@tipoData+' between '+ CHAR(39) + @dtDe+ CHAR(39)+ ' and ' + CHAR(39) + @dtAte+ CHAR(39)
        end
        if(LEN(@refForn)>0)
        begin
                SET @strSqlItens= @strSqlItens+' and  a.Ref_Fornecedor LIKE  '+ CHAR(39) + @dtDe+'%' +CHAR(39)
        end
        if(LEN(@ultimo_fornec)>0)
        begin
			        SET @strSqlItens= @strSqlItens+' and fmerc.fornecedor='+ CHAR(39) +@ultimo_fornec+ CHAR(39) 
        end
        
       
       --PRINT(@strSqlItens) 
        
         EXECUTE (@strSqlItens)
       
         SELECT ##ITENS.*,MES5=AG.VQMES05,MES4=AG.VQMES04,MES3=AG.VQMES03,MES2=AG.VQMES02,ULT_30D=ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S (INDEX=IX_SAIDA_ESTOQUE_01) WHERE S.FILIAL = @FILIAL AND S.PLU = ##ITENS.PLU  AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102)BETWEEN CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)and CONVERT(VARCHAR, GETDATE(),102) AND S.DATA_CANCELAMENTO IS NULL ),0),VDA_MED =CONVERT(NUMERIC(12,2),0.00), COB_DIAS=CONVERT(NUMERIC(12,2),0.00),SUG_UNID=CONVERT(NUMERIC(12,2),0.00),QTDE_COMPRA=CONVERT(NUMERIC(12,2),0.00)
         INTO ##QFinal FROM ##ITENS INNER JOIN ACUMULADO_GERAL AG (INDEX=PK_Acumulado_Geral) ON ##ITENS.PLU=AG.PLU AND AG.FILIAL=@FILIAL
        
         update ##QFinal set vda_med = CONVERT(NUMERIC(12,2),(mes5+mes4+mes3+mes2+ult_30d)/5,2) 
		 where (mes5+mes4+mes3+mes2+ult_30d)>0;
		 
		 update ##QFinal set COB_DIAS =  case when saldo <0 then CONVERT(NUMERIC(12,2),0.00) else  CONVERT(NUMERIC(12,2),( CONVERT(NUMERIC(12,2),SALDO) /(VDA_MED/30)))end 
				,sug_unid =  round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) 
				,qtde_compra = round(round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) /case when @conEmbalagem =1 then emb else 1 end  ,0)*
								case when @conEmbalagem =1 then emb else 1 end
		 where  Saldo <>0 and  VDA_MED <>0 and emb >0;
        
        update ##QFinal  SET QTDE_COMPRA =0 WHERE QTDE_COMPRA <0
        
         SET @strSqlItens= ' Select top 1000 PLU,EAN,REF,DESCRICAO,EMB,SALDO,PRC_CUSTO,MARGEM,PRC_VENDA,COB_CAD,MES5=CONVERT(INT,MES5),MES4=CONVERT(INT,MES4),MES3=CONVERT(INT,MES3),MES2=CONVERT(INT,MES2),ULT_30D= CONVERT(INT,ULT_30D),VDA_MED=CONVERT(INT,VDA_MED),COB_DIAS=CONVERT(INT,COB_DIAS),SUG_UNID= CONVERT(INT,SUG_UNID),QTDE_COMPRA=CONVERT(INT,QTDE_COMPRA) from ##QFinal  '
         
         DECLARE @vSug varchar(5) 
         SELECT @vSug= VALOR_ATUAL FROM PARAMETROS WHERE PARAMETRO ='SUGESTAO_ZERADA';
         
         IF(@vSug <> 'TRUE')
         BEGIN 
			SET @strSqlItens= @strSqlItens+' where sug_unid >0  '
         END 
         
         
         if(LEN(@ordem)>0)
			begin
				SET @strSqlItens= @strSqlItens+' order by '+@ordem
			end
          EXECUTE (@strSqlItens)
        
        
         drop table ##ITENS
         drop table ##Qfinal
      
	      
end


go 

/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperador]    Script Date: 05/23/2018 09:08:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_Fin_PorOperador]
GO

--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(30),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40),
    @plu	      as Varchar(20),
    @descricao    as Varchar(50),
    @ordem		  as Varchar(30),
    @ordemS	      as Varchar(30) 
            )
as


	select distinct	
			a.plu as PLU,
			b.descricao AS DESCRICAO,
			c.Descricao_grupo as GRUPO,
			C.descricao_subgrupo AS SUBGRUPO,
			C.descricao_departamento DEPARTAMENTO,
			F.Descricao_Familia AS FAMILIA ,
			sum(Qtde) as Qtde ,
			((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
			
		into #vendasTot			
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) 
			inner join mercadoria b on a.plu =b.plu  
			inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento 
			left join familia f on b.codigo_familia=f.codigo_familia 
			left join Fornecedor_Mercadoria fm on a.PLU = fm.plu and (len(@fornecedor)<>0  and  fm.fornecedor = @fornecedor) 
	where a.filial=@FILIAL 
		and  data_cancelamento is null 
		and (Data_movimento between @Datade and @Dataate)
		and (len(@Grupo)=0 or  c.Descricao_grupo = @Grupo)
		and (len(@subGrupo)=0 or  c.descricao_subgrupo = @subGrupo)
		and (len(@Departamento)=0 or c.descricao_departamento = @Departamento)
		and (len(@familia)=0 or isnull(f.Descricao_Familia,'') = @Familia)
		and (LEN(@plu)=0 or a.PLU=@plu)
		and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
		and (len(@fornecedor)=0 or fm.fornecedor = @fornecedor)
		and (@caixa='TODOS' or ( Select TOP 1 Operadores.Nome 
					  from Operadores 
						inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador 
					  where  l.filial =a.Filial  
						and l.Emissao = a.Data_movimento 
						and a.Caixa_Saida = l.pdv 
						and a.Documento=L.Documento) = @caixa)
	group by a.plu,b.descricao
			,c.Descricao_grupo
			,c.Descricao_subgrupo
			,c.Descricao_departamento
			,f.Descricao_familia
			
	order by b.descricao



Select ORDEM,
	   PLU,
	   DESCRICAO,
	   SUBGRUPO,
	   DEPARTAMENTO,
	   FAMILIA ,
	   Qtde ,
	   Valor    
	   INTO #tbFinalVenda
from 
(
	Select	Ordem = GRUPO+'0' -- CABECALHO
			,GRUPO + '|-TITULO-||CONCAT|' AS PLU
			,'' AS DESCRICAO
			,'' AS SUBGRUPO
			,'' AS DEPARTAMENTO
			,'' AS FAMILIA 
			,0  AS Qtde 
			,0  AS Valor     
	FROM  #vendasTot
	group by grupo
	UNION ALL 
	Select	Ordem = GRUPO+'1' -- ITENS
			,'PLU'+PLU
			,DESCRICAO
			,SUBGRUPO
			,DEPARTAMENTO
			,FAMILIA 
			,Qtde 
			,Valor     
	FROM  #vendasTot
	UNION ALL
	
	Select	Ordem = GRUPO+'9' -- TOTAL
			,'|-SUB-|' --PLU
			,'' --DESCRICAO
			,'' --SUBGRUPO
			,'' --DEPARTAMENTO
			,'' --FAMILIA 
			,SUM(QTDE)  --Qtde 
			,SUM(Valor)  --Valor     
	FROM  #vendasTot
	group by grupo
	union all
	Select	Ordem = 'ZZZZZ9' -- TOTAL GERAL
			,'|-SUB-|TOTAL ' --PLU
			,'' --DESCRICAO
			,'' --SUBGRUPO
			,'' --DEPARTAMENTO
			,'' --FAMILIA 
			,SUM(QTDE)  --Qtde 
			,SUM(VALOR)  --Valor     
	FROM  #vendasTot
) as a




IF( @ORDEM='DESCRICAO' AND @ORDEMS='DECRESCENTE' )  
BEGIN	
	SELECT PLU,
	   DESCRICAO,
	   SUBGRUPO,
	   DEPARTAMENTO,
	   FAMILIA ,
	   Qtde ,
	   Valor    
	FROM #tbFinalVenda  ORDER BY ORDEM, DESCRICAO DESC 
END
		
ELSE IF(@ORDEM='DESCRICAO' AND @ORDEMS='CRESCENTE'  ) 
BEGIN
	SELECT PLU,
	   DESCRICAO,
	   SUBGRUPO,
	   DEPARTAMENTO,
	   FAMILIA ,
	   Qtde ,
	   Valor     
	 FROM #tbFinalVenda  ORDER BY ORDEM, DESCRICAO
END
ELSE IF(@ORDEM='VALOR' AND @ORDEMS='CRESCENTE'  ) 
BEGIN
	SELECT PLU,
	   DESCRICAO,
	   SUBGRUPO,
	   DEPARTAMENTO,
	   FAMILIA ,
	   Qtde ,
	   Valor     
	FROM #tbFinalVenda  ORDER BY ORDEM, VALOR
END
ELSE IF(@ORDEM='VALOR' AND @ORDEMS='DECRESCENTE'  ) 
BEGIN
	SELECT PLU,
	   DESCRICAO,
	   SUBGRUPO,
	   DEPARTAMENTO,
	   FAMILIA ,
	   Qtde ,
	   Valor     
	FROM #tbFinalVenda  ORDER BY ORDEM, VALOR DESC
END


					
GO






IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.199.674', getdate();
GO