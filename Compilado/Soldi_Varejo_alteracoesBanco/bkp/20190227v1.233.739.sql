
IF OBJECT_ID('[sp_estoque_sugestao_compra]', 'P') IS NOT NULL
begin 
      drop procedure [sp_estoque_sugestao_compra]
end 

go

create    procedure [dbo].[sp_estoque_sugestao_compra](
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
			        SET @strSqlItens= @strSqlItens+' and a.plu in ( Select fmerc.plu from fornecedor_mercadoria fmerc where fmerc.fornecedor='+ CHAR(39) +@ultimo_fornec+ CHAR(39) +' )'
        end
        
       
       --PRINT(@strSqlItens) 
        
         EXECUTE (@strSqlItens)
       
         SELECT ##ITENS.*,MES5=AG.VQMES05,MES4=AG.VQMES04,MES3=AG.VQMES03,MES2=AG.VQMES02,ULT_30D=ISNULL(
			(SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S with (INDEX=IX_SAIDA_ESTOQUE_01) WHERE S.FILIAL = @FILIAL AND S.PLU = ##ITENS.PLU  AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102)BETWEEN CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)and CONVERT(VARCHAR, GETDATE(),102) AND S.DATA_CANCELAMENTO IS NULL ),0),VDA_MED =CONVERT(NUMERIC(12,2),0.00), COB_DIAS=CONVERT(NUMERIC(12,2),0.00),SUG_UNID=CONVERT(NUMERIC(12,2),0.00),QTDE_COMPRA=CONVERT(NUMERIC(12,2),0.00)
         INTO ##QFinal 
		 FROM ##ITENS 
			INNER JOIN ACUMULADO_GERAL AG  with (  INDEX=PK_Acumulado_Geral) 
				ON ##ITENS.PLU=AG.PLU AND AG.FILIAL=@FILIAL
        
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



IF OBJECT_ID('[sp_Rel_Resumo_Vendas]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Resumo_Vendas]
end 

go

create     Procedure [dbo].[sp_Rel_Resumo_Vendas]
            @Filial		  As Varchar(20),
            @DataDe		  As Varchar(8),
            @DataAte	  As Varchar(8),
            @plu		  As Varchar (20),
            @descricao	  As varchar(50),
            @grupo	      As Varchar(50),
            @subGrupo	  As varchar(50),
            @departamento as varchar(50),
            @relatorio	  as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas 'MATRIZ','20161101','20161130','','','','','','PEDIDO SIMPLES'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	Data varchar(30),
	Dia_Semana varchar(30),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)

declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
select @ini_periodo = inicio_periodo
	 , @fim_periodo =fim_periodo
	 , @dias_periodo = dias_periodo 
		
from filial where filial = @filial
		



if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT
            saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ,
			hora_venda
		  INTO
				#Lixo
      FROM
            Saida_Estoque with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  

            
      WHERE
            saida_estoque.Filial = @Filial
      AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
      and   hora_venda between '00:00:00' and '23:59:59'
      AND   Data_Movimento BETWEEN @DataDe AND dateadd(day,@dias_periodo,@DataAte)
      AND   Data_Cancelamento IS NULL
      AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
      
      GROUP BY
           Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

  

insert into #tmpVendas
 SELECT

      Data = Convert(Varchar,Data_Movimento,103),
      Dia_Semana = CASE
            WHEN DATEPART(dw, Data_Movimento) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, Data_Movimento) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, Data_Movimento) = 3 THEN 'TERÇA'
            WHEN DATEPART(dw, Data_Movimento) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, Data_Movimento) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, Data_Movimento) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, Data_Movimento) = 7 THEN 'SABADO'

      END,

      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ),
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) /  (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE ( #lixo.CPF_CNPJ <> '') and ((#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  )),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) 
							FROM #Lixo 
							WHERE (isnull(#lixo.CPF_CNPJ,'') <> '')   
							and (
									(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)
								or  (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo))
							),0),

     

      Perc_NFP =    CASE WHEN (
							SELECT Convert(Decimal(18,2),SUM(VLR)) 
							FROM #Lixo 
							WHERE (isnull(#lixo.CPF_CNPJ,'') <> '')  and 
							(
							(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo )  
							or 
							(#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)
							)  
						) > 0 
				THEN

                  CONVERT(DECIMAL(8,2), 
				  (
					(
						SELECT Convert(Decimal(18,2),SUM(VLR)) 
						FROM #Lixo 
						WHERE (#lixo.CPF_CNPJ <> '') and 
						(
							(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo )  
							or 
							(#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) 
						) 
					) /
                    (	
						SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) 
						FROM #Lixo 
						WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  
								or 
							  (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) 
					)) * 100)

                  ELSE 0 END
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

    

	  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
	  AND (Data_Movimento BETWEEN @DataDe AND @DataAte)
	  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	   
      GROUP BY

            Data_Movimento
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
	CONVERT(VARCHAR,N.Emissao,103) AS DATA ,
	  Dia_Semana = CASE
            WHEN DATEPART(dw, N.Emissao) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, N.Emissao) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, N.Emissao) = 3 THEN 'TERÇA'
            WHEN DATEPART(dw, N.Emissao) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, N.Emissao) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, N.Emissao) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, N.Emissao) = 7 THEN 'SABADO'

      END,
      SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) AS Venda,
      
      (
		  	Select COUNT(*) 
		from nf 
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
				AND  (NF.Emissao= n.Emissao )
				AND   NF.TIPO_NF = 1 
				AND ISNULL(NF.nf_Canc,0)=0	
				and nf.status='AUTORIZADO'
				AND (NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											

											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nii.TOTAL-(isnull(nii.Total,0)*isnull(nii.desconto,0)/100)),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
							 AND (NF.Emissao= n.Emissao )
							 AND NF.TIPO_NF = 1
							 and nf.status='AUTORIZADO'
							 AND (LEN(@PLU)=0 OR nii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf 
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
								and nf.status='AUTORIZADO'
								AND (
						NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as nii  inner join mercadoria as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao BETWEEN @DataDe AND @DataAte)
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY N.Emissao 
END


if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
BEGIN
insert into #tmpVendas
select 
	CONVERT(VARCHAR,p.Data_cadastro,103) AS DATA ,
	  Dia_Semana = CASE
            WHEN DATEPART(dw, p.Data_cadastro) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, p.Data_cadastro) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, p.Data_cadastro) = 3 THEN 'TERÇA'
            WHEN DATEPART(dw, p.Data_cadastro) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 7 THEN 'SABADO'

      END,
      SUM(pit.TOTAL) AS Venda,
      
      (
		  	Select COUNT(*) 
		from pedido  with (index(ix_pedido_fluxo_caixa))
		where pedido.pedido_simples = 1  
				AND  (pedido.Data_cadastro= p.Data_cadastro )
				and isnull(pedido.Status,0)<>3
				AND   pedido.Tipo = 1 
				and pedido.FILIAL=@filial 
				AND (pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM pedido WHERE pedido.Data_cadastro = p.Data_cadastro) > 0 THEN

                  (
                  Select isnull(SUM(pii.total),0) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					inner join pedido_itens as pii on pedido.pedido=pii.Pedido and pedido.Filial=pii.Filial and pedido.Tipo = pii.Tipo
					inner join mercadoria as mi on mi.PLU = pii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					where pedido.FILIAL=@filial 
							 AND (pedido.Data_cadastro= p.Data_cadastro)
							 AND pedido.TIPO = 1
							 and pedido.pedido_simples = 1
							 AND (LEN(@PLU)=0 OR pii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					where pedido.FILIAL=@filial 
								AND  (pedido.Data_cadastro= p.Data_cadastro )
								AND   pedido.TIPO = 1	
								and pedido.pedido_simples = 1
								
								AND (
						pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as pii  inner join mercadoria as mi on mi.PLU = pii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR pii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  Pedido as p with (index(ix_pedido_fluxo_caixa))
inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
inner join mercadoria as m on m.PLU = pit.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
WHERE p.pedido_simples = 1   
	AND  (p.Data_cadastro BETWEEN @DataDe AND @DataAte)
	 and isnull(p.Status,0)<>3
	 AND   p.TIPO = 1	
	 and p.FILIAL=@filial 
	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY p.Data_cadastro
END






Select 
			Data,
			Dia_Semana,
			Venda,
			 Clientes ,
			 [Venda_MD],
			 NFP,
			 VLR_NFP,
			 PERC_NFP
from (
 Select 
			ORDEM='0000',
			 Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas
GROUP BY Data,Dia_Semana
UNION ALL
 Select 
			ORDEM='ZZZZ',
			'|-SUB-|TOTAL',
			'',
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas

) as a 

ORDER BY ordem ,Data ;

--EXEC @SQL;

GO 



if not exists(select 1 from SEQUENCIAIS where TABELA_COLUNA='GRUPO_DE_PAGAMENTOS.COD_GRUPO')
begin	
	
	INSERT INTO [dbo].[SEQUENCIAIS]
		 VALUES
			   (
			'GRUPO_DE_PAGAMENTOS.COD_GRUPO'		--<TABELA_COLUNA, varchar(37),>
			,'GRUPOS DE PARCELAS CONTAS A PAGAR'--,<DESCRICAO, varchar(40),>
			,'0'								--,<SEQUENCIA, varchar(20),>
			,10									--,<TAMANHO, int,>
			,''									--,<OBS1, char(255),>
			,''									--,<OBS2, char(255),>
			,''									--,<OBS3, char(255),>
			,''									--,<OBS4, char(255),>
			,''									--,<OBS5, char(255),>
			,''									--,<OBS6, char(255),>
			,''									--,<OBS7, char(255),>
			,''									--,<OBS8, char(60),>
			,GETDATE()							--,<DATA_PARA_TRANSFERENCIA, datetime,>
			,0									--,<PERMITE_POR_EMPRESA, bit,>
			  );
end
GO
if object_id('Grupo_de_pagamentos') is null
BEGIN
	CREATE TABLE Grupo_de_pagamentos(
		filial varchar(20),
		cod_grupo varchar(10),
		documento varchar(50),
		fornecedor varchar(50)
	)
END 
ELSE
BEGIN 
		PRINT('TABELA - Grupo_de_pagamentos - JÁ FOI CRIADA');
END

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_pagar') 
            AND  UPPER(COLUMN_NAME) = UPPER('grupo_pagamento'))
begin
	alter table conta_a_pagar alter column grupo_pagamento varchar(10)
end
else
begin
	alter table conta_a_pagar add grupo_pagamento varchar(8)
end 
go 



go
	insert into Versoes_Atualizadas select 'Versão:1.233.739', getdate();
GO




