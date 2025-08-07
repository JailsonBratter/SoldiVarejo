


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Movimento_Venda
end
GO

CREATE Procedure [dbo].[sp_Movimento_Venda]

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

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),

                             FINALIZADORA = lista.id_finalizadora

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

                             FINALIZADORA = id_finalizadora

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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE
end
GO


GO
CREATE PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE](
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
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
	NF_Saida DECIMAL(18,2),
	NF_Saida_Preco	 DECIMAL(18,2),
	Cupom DECIMAL(18,2),  
	Cupom_Preco DECIMAL(18,2)
);

--select * from pedido_itens

IF @PedEstoque = 'TRUE'
INSERT INTO  #OUTROS select PLU =itemPd.PLU  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
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
	group by itemPd.PLU
ELSE
begin
INSERT INTO  #OUTROS 
	select PLU =se.PLU  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
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
	group by se.PLU
END;



Select PLU =todos.PLU
		,DESCRICAO = M.DESCRICAO
		,[Entrada]=Sum(todos.NF_ENTRADA)
		,[Entrada_Valor]=Sum(todos.NF_ENTRADA_PRECO)
		,[Saida_Outros] = SUM(todos.NF_Saida)
		,[Saida_Valor] = Sum(todos.NF_Saida_Preco)
		,[Cupom] = SUM(todos.Cupom)
		,[Cupom_Valor] = Sum(todos.Cupom_Preco)
	 from (

select PLU =isnull(A.PLU,''),
	   [NF_ENTRADA] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE Eitem.PLU = A.PLU and (Enf.Data between @DataInicio AND @DataFim ) 
							    AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF_ENTRADA_PRECO]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							where  Eitem.PLU = A.PLU  and (Enf.Data between @DataInicio AND @DataFim ) 
									 AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1   ),0) 				
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
								AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
								and a.Filial=@FILIAL
					GROUP BY A.PLU


UNION ALL
Select
	PLU   ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by PLU
 



) as todos
INNER JOIN Mercadoria M ON M.PLU =todos.PLU
group by todos.PLU,M.Descricao
order by CONVERT(DECIMAL,todos.PLU)  

















