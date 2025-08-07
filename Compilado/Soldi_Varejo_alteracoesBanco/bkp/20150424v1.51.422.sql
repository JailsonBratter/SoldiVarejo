
GO
-- SP_MOVIMENTO_VENDA 'MATRIZ', '20141202', '20141202', 'DINHEIRO','','246565','3'
ALTER Procedure [dbo].[sp_Movimento_Venda]
                @Filial          As Varchar(20),
                @DataDe          As Varchar(8),
                @DataAte         As Varchar(8),
                @finalizadora    As varchar(30),
                @plu			 As varchar(17),
                @cupom			 As varchar(20),
                @pdv			 as varchar(2)
AS

IF(@plu='' AND @cupom='')
	BEGIN
		IF(@finalizadora ='')
			BEGIN
				SELECT 
					DATA = CONVERT(VARCHAR,EMISSAO,103), 
					PDV, 
					CUPOM = DOCUMENTO, 
					VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
					FINALIZADORA = id_finalizadora 
				FROM 
					Lista_finalizadora 
					INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
				WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
						 and pdv like (case when @pdv <> '' then @pdv else '%' end)
				GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
		 
			END
		ELSE
			BEGIN
				SELECT 
					DATA = CONVERT(VARCHAR,EMISSAO,103), 
					PDV, 
					CUPOM = DOCUMENTO, 
					VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
					FINALIZADORA = id_finalizadora 
				FROM 
					Lista_finalizadora 
					INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
				WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
				AND finalizadora.Finalizadora  = @finalizadora  
				 and pdv like (case when @pdv <> '' then @pdv else '%' end)
				GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
					
			END
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
		FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento
					INNER JOIN Mercadoria M ON S.PLU = M.PLU 	
				where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
				 and s.PLU like (case when @plu <>'' then @plu else '%' end ) 
				 and l.Emissao BETWEEN @DataDe  AND  @DataAte 
				 and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
				 Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo
				 union all 
				
				select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)))) 
					from Lista_finalizadora 
					where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and Emissao BETWEEN @DataDe  AND  @DataAte 
					and pdv like (case when @pdv <> '' then @pdv else '%' end)
				 GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
				
	END
