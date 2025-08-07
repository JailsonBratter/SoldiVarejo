IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('configuravel'))
begin
	alter table mercadoria alter column configuravel tinyint
end
else
begin
	alter table mercadoria add configuravel tinyint
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NFe_XML') 
            AND  UPPER(COLUMN_NAME) = UPPER('Total_ICMSTot_vFCP'))
begin
	alter table NFe_XML alter column Total_ICMSTot_vFCP decimal(12, 2)
end
else
begin
	alter table NFe_XML add Total_ICMSTot_vFCP decimal(12, 2)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NFe_XML') 
            AND  UPPER(COLUMN_NAME) = UPPER('Total_ICMSTot_vFCPST'))
begin
	alter table NFe_XML alter column Total_ICMSTot_vFCPST decimal(12, 2)
end
else
begin
	alter table NFe_XML add Total_ICMSTot_vFCPST decimal(12, 2)
end 
go 
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NFe_XML') 
            AND  UPPER(COLUMN_NAME) = UPPER('Total_ICMSTot_vFCPSTRet'))
begin
	alter table NFe_XML alter column Total_ICMSTot_vFCPSTRet decimal(12, 2)
end
else
begin
	alter table NFe_XML add Total_ICMSTot_vFCPSTRet decimal(12, 2)
end 
go 
GO
SET QUOTED_IDENTIFIER ON

GO

 

ALTER PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](

       @FILIAL         AS VARCHAR(17),

       @Datade               As DATETIME,

       @Dataate        As DATETIME,

       @Caixa          As varchar(30),

       @Grupo        As Varchar(60),

       @subGrupo   as Varchar(60),

       @Departamento as Varchar(60),

       @Familia   as Varchar(60),

       @fornecedor   as Varchar(40),

       @plu           as Varchar(20),

       @descricao    as Varchar(50),

       @ordem            as Varchar(30),

       @ordemS        as Varchar(30),

       @finalizadora as Varchar(30)

)

 

as

begin

          -- sp_Rel_Fin_PorOperador 'MATRIZ', '2021-09-01', '2021-09-30', 'TODOS', '', '', '', '', '', '', '', 'VALOR', 'DECRESCENTE', ''

 

                select distinct   

 

                                        a.plu as PLU,

 

                                        ean = isnull((select TOP 1 ISNULL(EAN.EAN, '') AS EAN

                                                                                        FROM EAN WHERE EAN.PLU = b.PLU order by isnull(ean.ean, '') desc), ''),

                                        b.descricao AS DESCRICAO,

 

                                        c.Descricao_grupo as GRUPO,

 

                                        C.descricao_subgrupo AS SUBGRUPO,

 

                                        C.descricao_departamento DEPARTAMENTO,

 

                                        F.Descricao_Familia AS FAMILIA ,

 

                                        sum(Qtde) as Qtde ,

 

                                        (Sum(isnull(a.vlr,0)-ISNULL(a.desconto,0)+isnull(a.Acrescimo,0))) as Valor

 

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

 

                           and (@finalizadora ='TODOS' or ( Select TOP 1 Finalizadora.Finalizadora

 

                                                                   from Finalizadora

 

                                                                          inner join Lista_finalizadora l on l.finalizadora  = Finalizadora.Nro_Finalizadora

 

                                                                   where  l.filial =a.Filial

 

                                                                          and l.Emissao = a.Data_movimento

 

                                                                          and a.Caixa_Saida = l.pdv

 

                                                                          and a.Documento=L.Documento) = @finalizadora )

 

                group by a.plu,b.descricao ,b.plu

 

                                        ,c.Descricao_grupo

 

                                        ,c.Descricao_subgrupo

 

                                        ,c.Descricao_departamento

 

                                        ,f.Descricao_familia

 

                  

 

                order by b.descricao

 

                    INSERT INTO #vendasTot                 

                select distinct   

 

                                        i.plu as PLU,

 

                                        ean = isnull((select TOP 1 ISNULL(EAN.EAN, '') AS EAN

                                               FROM EAN WHERE EAN.PLU = b.PLU order by isnull(ean.ean, '') desc), ''),

 

                                        b.descricao AS DESCRICAO,

 

                                        c.Descricao_grupo as GRUPO,

 

                                        C.descricao_subgrupo AS SUBGRUPO,

 

                                        C.descricao_departamento DEPARTAMENTO,

 

                                        F.Descricao_Familia AS FAMILIA ,

 

                                        sum(i.Qtde * i.Embalagem) as Qtde ,

                                       

                                        SUM(((i.Qtde * i.Embalagem) * i.Unitario) + 0 - i.desconto + i.IPIV) as Valor

 

                from  nf

                INNER JOIN NF_Item i WITH (index=ix_nf_item_01) ON nf.FILIAL = i.Filial AND NF.Cliente_Fornecedor = i.Cliente_Fornecedor AND NF.Codigo = i.Codigo

                    inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao

 

                                        inner join mercadoria b on i.plu =b.plu

 

                                        inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento

 

                                        left join familia f on b.codigo_familia=f.codigo_familia

 

                                        left join Fornecedor_Mercadoria fm on i.PLU = fm.plu and (len(@fornecedor)<>0  and  fm.fornecedor = @fornecedor)

 

                where nf.filial=@FILIAL

 

                           and (nf.Emissao between @Datade and @Dataate)

                          

                            AND nf.Tipo_NF = 1

                          

                            AND ISNULL(NF.nf_Canc, 0) < 1

                          

                            and nf.status='AUTORIZADO'

                          

                            AND i.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')

                            

                            and np.Saida = 1  and isnull(np.NF_devolucao,0) =0

 

                           and (len(@Grupo)=0 or  c.Descricao_grupo = @Grupo)

 

                           and (len(@subGrupo)=0 or  c.descricao_subgrupo = @subGrupo)

 

                           and (len(@Departamento)=0 or c.descricao_departamento = @Departamento)

 

                           and (len(@familia)=0 or isnull(f.Descricao_Familia,'') = @Familia)

 

                           and (LEN(@plu)=0 or i.PLU=@plu)

 

                           and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')

 

                           and (len(@fornecedor)=0 or fm.fornecedor = @fornecedor)

                          

                            group by i.plu,b.descricao, b.plu

                                        ,c.Descricao_grupo

 

                                        ,c.Descricao_subgrupo

 

                                        ,c.Descricao_departamento

 

                                        ,f.Descricao_familia

 

                  

 

                order by b.descricao

 

 

      

       select

 

                      PLU,

 

                      EAN,

 

                      DESCRICAO,

                     

                      GRUPO,

 

                      SUBGRUPO,

 

                      DEPARTAMENTO,

 

                      FAMILIA ,

 

                      SUM(Qtde) AS Qtde ,

 

                      SUM(Valor) AS Valor

       INTO #VENDA01

       FROM #vendasTot

       GROUP BY PLU, EAN, DESCRICAO, GRUPO, SUBGRUPO, DEPARTAMENTO, FAMILIA  

 

 

 

       Select ORDEM,

 

                      PLU,

 

                      EAN,

 

                      DESCRICAO,

 

                      SUBGRUPO,

 

                      DEPARTAMENTO,

 

                      FAMILIA ,

 

                      Qtde ,

 

                      Valor  

 

                      INTO #tbFinalVenda

 

       from

 

       (

 

                Select Ordem = GRUPO+'0' -- CABECALHO

 

                                        ,GRUPO + '|-TITULO-||CONCAT|' AS PLU

 

                                        ,'' AS EAN

 

                                        ,'' AS DESCRICAO

 

                                        ,'' AS SUBGRUPO

 

                                        ,'' AS DEPARTAMENTO

 

                                        ,'' AS FAMILIA

 

                                        ,0  AS Qtde

 

                                        ,0  AS Valor   

 

                FROM  #VENDA01

 

                group by grupo

 

                UNION ALL

 

                Select Ordem = GRUPO+'1' -- ITENS

 

                                        ,'PLU'+PLU

 

                                        ,'PLU'+EAN

 

                                        ,DESCRICAO

 

                                        ,SUBGRUPO

 

                                        ,DEPARTAMENTO

 

                                        ,FAMILIA

 

                                        ,Qtde

 

                                        ,Valor   

 

                FROM  #VENDA01

 

                UNION ALL

 

     

 

                Select Ordem = GRUPO+'9' -- TOTAL

 

                                        ,'|-SUB-|' --PLU

 

                                        ,'' --EAN

 

                                        ,'' --DESCRICAO

 

                                        ,'' --SUBGRUPO

 

                                        ,'' --DEPARTAMENTO

 

                                        ,'' --FAMILIA

 

                                        ,SUM(QTDE)  --Qtde

 

                                        ,SUM(Valor)  --Valor   

 

                FROM  #VENDA01

 

                group by grupo

 

                union all

 

                Select Ordem = 'ZZZZZ9' -- TOTAL GERAL

 

                                        ,'|-SUB-|TOTAL ' --PLU

 

                                        ,'' --EAN

 

                                        ,'' --DESCRICAO

 

                                        ,'' --SUBGRUPO

 

                                        ,'' --DEPARTAMENTO

 

                                        ,'' --FAMILIA

 

                                        ,SUM(QTDE)  --Qtde

 

                                        ,SUM(VALOR)  --Valor   

 

                FROM  #VENDA01

 

       ) as a

 


 


 


 


 

       IF( @ORDEM='DESCRICAO' AND @ORDEMS='DECRESCENTE' )

 

       BEGIN

 

                SELECT PLU,

                  

                      EAN,

 

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

 

                   EAN,

 

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

 

                   EAN,

 

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

 

                   EAN,

 

                      DESCRICAO,

 

                      SUBGRUPO,

 

                      DEPARTAMENTO,

 

                      FAMILIA ,

 

                      Qtde ,

 

                      Valor   

 

                FROM #tbFinalVenda  ORDER BY ORDEM, VALOR DESC

 

       END

 

end

go

 insert into Versoes_Atualizadas select 'Versão:1.305.883', getdate();