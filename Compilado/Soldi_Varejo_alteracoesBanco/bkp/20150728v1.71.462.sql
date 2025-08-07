Create procedure sp_consulta_movimento_venda
(@FILIAL as varchar(20),
@DataDe as Datetime,
@DataAte as Datetime,
@pdv as varchar(5),
@finalizadora    As varchar(30)
)
as 
BEGIN

            
                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR = convert(decimal(18,2),SUM(Lista_Finalizadora.Total )),

                             FINALIZADORA = id_finalizadora

                        FROM

                             Lista_finalizadora

                             INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE Lista_Finalizadora.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (Emissao BETWEEN @DataDe  AND  @DataAte )
                                   and (len(@pdv)=0 or pdv=@pdv)
                                   and (LEN(@finalizadora)=0 or Lista_finalizadora.id_finalizadora=@finalizadora)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

          end