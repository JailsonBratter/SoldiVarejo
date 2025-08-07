

/****** Object:  Table [dbo].[fornecedor_departamento]    Script Date: 05/02/2017 16:01:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fornecedor_departamento]') AND type in (N'U'))
DROP TABLE [dbo].[fornecedor_departamento]
GO



/****** Object:  Table [dbo].[fornecedor_departamento]    Script Date: 05/02/2017 16:01:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[fornecedor_departamento](
	[fornecedor] [varchar](20) NULL,
	[codigo_departamento] [varchar](9) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO





/****** Object:  Table [dbo].[solicitacao_compra]    Script Date: 05/03/2017 14:26:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[solicitacao_compra]') AND type in (N'U'))
DROP TABLE [dbo].[solicitacao_compra]
GO



/****** Object:  Table [dbo].[solicitacao_compra]    Script Date: 05/03/2017 14:26:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[solicitacao_compra](
	[filial] [varchar](20) NULL,
	[codigo] [varchar](20) NULL,
	[descricao] [varchar](80) NULL,
	[data_cadastro] [datetime] NULL,
	[usuario_cadastro] [varchar](40) NULL,
	[status] varchar(40) null,
	[tipo_Solicitacao] varchar(40) null
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[solicitacao_compra_itens]') AND type in (N'U'))
DROP TABLE [dbo].[solicitacao_compra_itens]
GO


/****** Object:  Table [dbo].[solicitacao_compra_itens]    Script Date: 05/03/2017 14:28:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[solicitacao_compra_itens](
	[filial] [varchar](20) NULL,
	[codigo] [varchar](20) NULL,
	[plu] [varchar](17) NULL,
	[ean] [varchar](20) NULL,
	[ref_fornecedor] [varchar](8) NULL,
	[descricao] [varchar](80) NULL,
	[ctr] [varchar](20) null,
	[und] [varchar](3) null, 
	[embalagem] [numeric](3, 0) NULL,
	[saldo] [numeric](12, 3) NULL,
	[preco_custo] [numeric](12, 2) NULL,
	[preco_venda] [numeric](12, 2) NULL,
	[mes_5] [numeric](12, 2) NULL,
	[mes_4] [numeric](12, 2) NULL,
	[mes_3] [numeric](12, 2) NULL,
	[mes_2] [numeric](12, 2) NULL,
	[ult_30] [numeric](12, 2) NULL,
	[vda_med] [numeric](12, 2) NULL,
	[cob_cad] [numeric](12, 2) NULL,
	[cob_dias] [numeric](12, 2) NULL,
	[sugestao] [numeric](12, 2) NULL,
	[qtde_comprar] [numeric](12, 2) NULL,
	[ordem] int null  ,
	[aceita_sug] tinyint
	
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[sp_estoque_sugestao_compra]    Script Date: 05/05/2017 11:11:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_estoque_solicitacao_compra]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_estoque_solicitacao_compra]
GO



/****** Object:  StoredProcedure [dbo].[sp_estoque_sugestao_compra]    Script Date: 05/05/2017 11:11:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE  procedure [dbo].[sp_estoque_solicitacao_compra](
            @FILIAL VARCHAR(30),
            @plu varchar(max),
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
-- execute sp_estoque_solicitacao_compra 'MATRIZ','10012,10014' ,'','','','','','','','','','','','','','',null,0,0,''
           

begin
        Declare @strSqlItens  As nVarchar(1024)
    
         
                SET @strSqlItens= 'Select  a.plu as PLU,b.EAN,REF =ISNULL(a.Ref_Fornecedor,'+ CHAR(39) ++ CHAR(39) +'), a.DESCRICAO,UN=A.UND  ,EMB =A.EMBALAGEM , SALDO=ISNULL(l.Saldo_atual,0), PRC_CUSTO= l.Preco_Custo,PRC_VENDA=l.Preco , COB_CAD = ISNULL(A.COBERTURA,0) '
        SET @strSqlItens= @strSqlItens+' INTO ##ITENS from mercadoria a  LEFT join ean b on a.plu=b.plu  inner join mercadoria_loja l on l.plu=a.plu    inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao '
        SET @strSqlItens= @strSqlItens+' left join W_BR_CADASTRO_DEPARTAMENTO d on (a.codigo_departamento= d.codigo_Departamento and a.filial=l.filial) left join Familia f on a.Codigo_familia = f.Codigo_familia '
        if(LEN(@ultimo_fornec)>0)
        begin
			        SET @strSqlItens= @strSqlItens+' inner join  fornecedor_mercadoria fmerc on (a.plu= fmerc.plu and a.filial=fmerc.filial)'
        end
        
        
        SET @strSqlItens= @strSqlItens+' WHERE L.FILIAL = '+ CHAR(39) + @FILIAL+ CHAR(39) + ' and a.inativo<>1 '
		if(LEN(@plu)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu in ( ' + @plu +')'
        end
        if(LEN(@ean)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and b.ean= '+ CHAR(39) + @ean+ CHAR(39)
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
        
        
        
         SET @strSqlItens= ' Select PLU,EAN,REF,DESCRICAO,CTR ='+CHAR(39)+CHAR(39)+',UN,EMB,SALDO,PRC_CUSTO,PRC_VENDA,COB_CAD,MES5=CONVERT(INT,MES5),MES4=CONVERT(INT,MES4),MES3=CONVERT(INT,MES3),MES2=CONVERT(INT,MES2),ULT_30D= CONVERT(INT,ULT_30D),VDA_MED=CONVERT(INT,VDA_MED),COB_DIAS=CONVERT(INT,COB_DIAS),SUG_UNID= CONVERT(INT,SUG_UNID),QTDE_COMPRA=CONVERT(INT,QTDE_COMPRA) from ##QFinal  '
         --SET @strSqlItens= @strSqlItens+' where sug_unid >0  '
         if(LEN(@ordem)>0)
			begin
				SET @strSqlItens= @strSqlItens+' order by '+@ordem
			end
          EXECUTE (@strSqlItens)
        
        
         drop table ##ITENS
         drop table ##Qfinal
      
	      
end
GO

IF NOT EXISTS(SELECT * FROM SEQUENCIAIS WHERE TABELA_COLUNA ='SOLICITACAO_COMPRA.CODIGO')
BEGIN

INSERT INTO [dbo].[SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('SOLICITACAO_COMPRA.CODIGO'
           ,'SOLICITACAO_COMPRA.CODIGO'
           ,'0000'
           ,4
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0)
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.159.597', getdate();
GO