IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('COMANDA_ITEM') 
            AND  UPPER(COLUMN_NAME) = UPPER('motivo'))
begin
	alter table comanda_item alter column motivo varchar(60)
end
else
begin
		
	alter table comanda_item add motivo varchar(40)

end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tributacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('cfop'))
begin
	alter table tributacao alter column cfop decimal(4,0)
end
else
begin
		
	alter table tributacao add cfop decimal(4,0)

end 
go 






update Tributacao set  CFOP =102 where Indice_ST in ('00','40','41','20','50','51','101','102','103','300','400');


update Tributacao set  CFOP =403 where Indice_ST in ('60','10','70','202','201','203');

go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('usuarios_web_telas') 
            AND  UPPER(COLUMN_NAME) = UPPER('nome_tela'))
begin
	alter table usuarios_web_telas  alter column nome_tela varchar(50);
end
else
begin
   alter table usuarios_web_telas  add nome_tela varchar(50);
end 
go 





IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_estoque_solicitacao_compra]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_estoque_solicitacao_compra]
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

-- execute sp_estoque_solicitacao_compra 'MATRIZ','100,1009' ,'','','','','','','','','','','','','','',null,0,0,''
           

begin
        Declare @strSqlItens  As nVarchar(MAX)
    
         
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
       
         SELECT ##ITENS.*,MES5=AG.VQMES05,MES4=AG.VQMES04,MES3=AG.VQMES03,MES2=AG.VQMES02,ULT_30D=ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S with (INDEX=IX_SAIDA_ESTOQUE_01) WHERE S.FILIAL = @FILIAL AND S.PLU = ##ITENS.PLU  AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102)BETWEEN CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)and CONVERT(VARCHAR, GETDATE(),102) AND S.DATA_CANCELAMENTO IS NULL ),0),VDA_MED =CONVERT(NUMERIC(12,2),0.00), COB_DIAS=CONVERT(NUMERIC(12,2),0.00),SUG_UNID=CONVERT(NUMERIC(12,2),0.00),QTDE_COMPRA=CONVERT(NUMERIC(12,2),0.00)
         INTO ##QFinal FROM ##ITENS LEFT JOIN ACUMULADO_GERAL AG  with (INDEX=PK_Acumulado_Geral) ON ##ITENS.PLU=AG.PLU AND AG.FILIAL=@FILIAL
        
         update ##QFinal set vda_med = CONVERT(NUMERIC(12,2),(mes5+mes4+mes3+mes2+ult_30d)/5,2) 
		 where (mes5+mes4+mes3+mes2+ult_30d)>0;
		 
		 update ##QFinal set COB_DIAS =  case when saldo <0 then CONVERT(NUMERIC(12,2),0.00) else  CONVERT(NUMERIC(12,2),( CONVERT(NUMERIC(12,2),SALDO) /(VDA_MED/30)))end 
				,sug_unid =  round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) 
				,qtde_compra = round(round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) /case when @conEmbalagem =1 then emb else 1 end  ,0)*
								case when @conEmbalagem =1 then emb else 1 end
		 where  Saldo <>0 and  VDA_MED <>0 and emb >0;
        
        
        
        SET @strSqlItens= ' Select PLU,EAN,REF,DESCRICAO, '
		SET @strSqlItens= @strSqlItens+ '	CTR =ISNULL(( '
		SET @strSqlItens= @strSqlItens+	'	Select '+CHAR(39)+'C'+CHAR(39)
		SET @strSqlItens= @strSqlItens+ '		from Contrato_fornecedor_item as cfi '
        SET @strSqlItens= @strSqlItens+ '            	INNER JOIN Contrato_fornecedor AS  CF ON CFI.id_contrato =CF.id_contrato '
        SET @strSqlItens= @strSqlItens+ '              inner join  Contrato_Fornecedor_Filial cff  on cf.id_contrato = cff.id_contrato   '
        SET @strSqlItens= @strSqlItens+ '             where cff.Filial ='+CHAR(39)+@FILIAL+CHAR(39)+' AND CF.data_validade >= GETDATE() AND CFI.plu=##QFinal.plu '			
		SET @strSqlItens= @strSqlItens+ '	),'+CHAR(39)+CHAR(39)+') '
        SET @strSqlItens= @strSqlItens+ ' ,UN,EMB,SALDO, '
        SET @strSqlItens= @strSqlItens+ 'PRC_CUSTO=('
        SET @strSqlItens= @strSqlItens+ 'isnull(('
        SET @strSqlItens= @strSqlItens+	'	Select top 1 CFI.vlr '
		SET @strSqlItens= @strSqlItens+ '		from Contrato_fornecedor_item as cfi '
        SET @strSqlItens= @strSqlItens+ '            	INNER JOIN Contrato_fornecedor AS  CF ON CFI.id_contrato =CF.id_contrato '
        SET @strSqlItens= @strSqlItens+ '              inner join  Contrato_Fornecedor_Filial cff  on cf.id_contrato = cff.id_contrato   '
        SET @strSqlItens= @strSqlItens+ '             where cff.Filial ='+CHAR(39)+@FILIAL+CHAR(39)+' AND CF.data_validade >= GETDATE() AND CFI.plu=##QFinal.plu '			
		
		SET @strSqlItens= @strSqlItens+ '	),PRC_CUSTO)'
        SET @strSqlItens= @strSqlItens+ '),'
        SET @strSqlItens= @strSqlItens+ 'PRC_VENDA,COB_CAD,MES5=CONVERT(INT,MES5),MES4=CONVERT(INT,MES4),MES3=CONVERT(INT,MES3),MES2=CONVERT(INT,MES2),ULT_30D= CONVERT(INT,ULT_30D),VDA_MED=CONVERT(INT,VDA_MED),COB_DIAS=CONVERT(INT,COB_DIAS),SUG_UNID= CONVERT(INT,SUG_UNID),QTDE_COMPRA=CONVERT(INT,QTDE_COMPRA) from ##QFinal  '
         --SET @strSqlItens= @strSqlItens+' where sug_unid >0  '
         if(LEN(@ordem)>0)
			begin
				SET @strSqlItens= @strSqlItens+' order by '+@ordem
			end
          EXECUTE (@strSqlItens)
        
        
         drop table ##ITENS
         drop table ##Qfinal
      
	      
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.171.612', getdate();
GO