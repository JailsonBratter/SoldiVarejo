
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperador]
end
GO
--PROCEDURES =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(8),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40) 
            )
as
	
if len(@Caixa)= 0
	begin
		set @Caixa = '%'
	end
if LEN(@Grupo)=0
	begin
		set @Grupo ='%'
	end
if LEN(@subGrupo)=0
	begin
		set @subGrupo = '%'
	end
if LEN(@Departamento)=0
	begin 
		set @Departamento = '%'
	end
if LEN(@Familia)=0
	begin
		set @Familia = '%'
	end

if(LEN(@fornecedor)=0)
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	INNER JOIN Lista_finalizadora L ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv
	inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		
		and isnull(Operadores.Nome ,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
else
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
		 inner join Fornecedor_Mercadoria fm on a.PLU = fm.plu 	
		 INNER JOIN Lista_finalizadora L ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv
		inner join Operadores on l.operador = Operadores.ID_Operador	
				
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		and isnull(Operadores.Nome ,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and fm.fornecedor = @fornecedor
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end


  
  go
  
  
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Movimento_Venda]
end
GO
--PROCEDURES =======================================================================================
CREATE   Procedure [dbo].[sp_Movimento_Venda]

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
							INNER JOIN  Saida_estoque S  ON S.Documento=list1.Documento and s.Caixa_Saida = list1.pdv	
                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
								and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
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
							INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	
                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )
								  and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim 
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

                             CUPOM = lista.DOCUMENTO,

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
                             INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 
						and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
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
									
                        GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora

                            

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

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

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

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

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

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total='0,000' 

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


