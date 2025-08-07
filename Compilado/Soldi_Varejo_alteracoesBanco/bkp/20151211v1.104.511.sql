/****** Object:  Index [ix_Rel_Fin_PorOperador]    Script Date: 12/11/2015 14:49:30 ******/
CREATE NONCLUSTERED INDEX [ix_Rel_Fin_PorOperador] ON [dbo].[Saida_estoque] 
(
	[PLU] ASC,
	[Filial] ASC,
	[data_cancelamento] ASC,
	[Data_movimento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/****** Object:  Index [ix_Lista_rel_fin_PorOperador]    Script Date: 12/11/2015 14:56:10 ******/
CREATE NONCLUSTERED INDEX [ix_Lista_rel_fin_PorOperador] ON [dbo].[Lista_finalizadora] 
(
	[Documento] ASC,
	[pdv] ASC,
	[Emissao] ASC,
	[filial] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].SP_REL_VENDAS_POR_ALIQUOTA') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_VENDAS_POR_ALIQUOTA
end
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperador]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
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

if(LEN(@fornecedor)=0 and LEN(@Caixa)=0)
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
	inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
	--inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
	--	and isnull(Operadores.Nome ,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
else
begin 
	if(LEN(@Caixa)>0)
	begin
		select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
	inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
	--inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		and (Select top 1 Operadores.Nome from Operadores inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador where a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial   ) = @Caixa
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
	from saida_estoque a with(index(ix_Rel_fin_porOperador))  inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
		 inner join Fornecedor_Mercadoria fm on a.PLU = fm.plu 	
		 INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON  a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
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
end

  



go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].SP_REL_VENDAS_POR_ALIQUOTA') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_VENDAS_POR_ALIQUOTA
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE SP_REL_VENDAS_POR_ALIQUOTA
				@Filial      As Varchar(20),
                @DataDe      As Varchar(8),
                @DataAte     As Varchar(8)
AS
begin
-- exec SP_REL_VENDAS_POR_ALIQUOTA 'MATRIZ','20151101','20151130'
Declare crTributacao cursor
for select Saida_ICMS from Tributacao group by Saida_ICMS order by Saida_ICMS
declare @SqlString Nvarchar(max)
SET @SqlString = ''
DECLARE @TRIB VARCHAR(10)
open crTributacao
FETCH NEXT FROM crTributacao INTO @TRIB;
WHILE @@FETCH_STATUS = 0
   BEGIN
	
		SET  @SqlString = @SqlString + ',[' + CASE WHEN @TRIB = '0.00' THEN 'INSENTO' ELSE @TRIB+'%' END + '] =ISNULL((select SUM(ISNULL(vlr,0)-isnull(desconto,0)) from saida_estoque with(index(ix_Rel_Venda_Aliquota)) where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data_movimento =se.Data_movimento and Aliquota_ICMS ='+ @TRIB +' AND data_cancelamento IS NULL),0)'
	  --SET @SqlString = @SqlString + CONVERT(VARCHAR(10),@TRIB)
		 
      FETCH  NEXT FROM crTributacao INTO @TRIB;
      
   END;

CLOSE crTributacao;
DEALLOCATE crTributacao;
SET @SqlString = 'Select Data =convert(varchar,Data_movimento,103),SUM(ISNULL(vlr,0)-isnull(desconto,0)) AS TOTAL '+@SqlString +'  from saida_estoque se with(index(ix_Rel_Venda_Aliquota)) ' +
                    ' where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and Data_movimento between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) + ' AND  data_cancelamento IS NULL '+
                    ' group by Data_movimento  order by convert(varchar,data_movimento,102) ';


EXECUTE (@SqlString);

end


go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].SP_REL_VENDAS_POR_FINALIZADORA') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_VENDAS_POR_FINALIZADORA
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE SP_REL_VENDAS_POR_FINALIZADORA
				@Filial      As Varchar(20),
                @DataDe      As Varchar(8),
                @DataAte     As Varchar(8)
AS
begin
-- exec SP_REL_VENDAS_POR_FINALIZADORA 'MATRIZ','20151101','20151130'
Declare crFinalizadora cursor
for  SELECT Finalizadora FROM Finalizadora GROUP BY Finalizadora, Nro_Finalizadora order by Nro_Finalizadora
declare @SqlString Nvarchar(max)
SET @SqlString = ''
DECLARE @FINALIZA VARCHAR(30)
open crFinalizadora
FETCH NEXT FROM crFinalizadora INTO @FINALIZA;
WHILE @@FETCH_STATUS = 0
   BEGIN
	
		SET  @SqlString = @SqlString + ',[' + @FINALIZA +'] =ISNULL((select SUM(ISNULL(total,0))from Lista_finalizadora where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and emissao =lf.emissao and id_finalizadora ='+CHAR(39) + @FINALIZA+ CHAR(39) + ' AND Cancelado is null),0)'
	  --SET @SqlString = @SqlString + CONVERT(VARCHAR(10),@TRIB)
		 
      FETCH  NEXT FROM crFinalizadora INTO @FINALIZA;
      
   END;

CLOSE crFinalizadora;
DEALLOCATE crFinalizadora;
SET @SqlString = 'Select Data =convert(varchar,Emissao,103),SUM(ISNULL(total,0)) AS TOTAL '+@SqlString +'  from Lista_finalizadora lf  ' +
                    ' where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and Emissao between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) +
                    ' group by Emissao  order by convert(varchar,Emissao,102) ';


EXECUTE (@SqlString);

end



go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperadorCancelamento]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperadorCancelamento]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As DATETIME,
      @Dataate			As DATETIME,
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as

if len(@Operador) =0

      begin

            set @Operador ='%'     

      end
      
if LEN(@Pdv) = 0

		begin
			set @Pdv = '%'
			end
 

select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data  

      , Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and isnull(c.cancelado,0)=0), 0) as  Vendas

      , isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   isnull(c.cancelado,0)=1),0)as  Cancelados
      

      from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador

 

      where a.filial=@FILIAL and b.nome like @Operador and a.emissao between @Datade and @Dataate And a.pdv like @Pdv

group by a.operador,a.Pdv, b.nome,a.emissao

order by a.Pdv, b.nome

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

                             VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) 
										FROM Lista_finalizadora list1
				                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 
								  			 --INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01)) ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim
									WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
											 AND (list1.Emissao = lista.Emissao)
											 and list1.pdv =lista.pdv
											 and list1.documento = lista.documento
											 AND LIST1.id_finalizadora = LISTA.id_finalizadora
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
								and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),

                             FINALIZADORA = lista.id_finalizadora,
							
							[COMANDA/PEDIDOS] =  (SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)
                             

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
							INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	
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
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

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
										OR (@cancelados ='SIM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='NAO' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

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

                        'PLU'+S.PLU as PLU,

                        M.Descricao,

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

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

                        'PLU'+S.PLU AS PLU,

                        M.Descricao,

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                       Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
						union all

                        
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

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is NOT null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                        
                        union all

                        select '','','','', id_finalizadora  ,'','','','', replace(convert(varchar,(SUM(Lista_Finalizadora.Total))),'.',',')

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END




 

 
GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Vendedor]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Vendedor]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].sp_Rel_Vendedor(
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Vendedor	  as varchar(40)
	)
as
begin 
-- exec sp_rel_Vendedor 'MATRIZ', '20151102', '20151102','TODOS'
IF(@Vendedor='TODOS')
BEGIN
	SELECT  Funcionario.Codigo,
			Funcionario.Nome,
			Funcionario.Funcao,
			CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],    
			sum(se.vlr - se.Desconto) as Total,
			(sum(se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100 as [Vlr Comissao]
	FROM Saida_estoque se inner join Funcionario on se.vendedor = Funcionario.codigo
	where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
		-- and (@Vendedor='TODOS' or Funcionario.Nome = @Vendedor)
	GROUP BY Funcionario.codigo, Funcionario.Nome,Funcionario.Funcao,Funcionario.Comissao
END
ELSE
BEGIN
	SELECT  
			CONVERT(VARCHAR,SE.Data_movimento,103)AS DATA,
			
			CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],
			SUM(se.vlr - se.Desconto) as TOTAL,
			SUM(((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100) as [Vlr Comissao]
	FROM Saida_estoque se inner join Funcionario on se.vendedor = Funcionario.codigo
	
	where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
		and ( Funcionario.Nome = @Vendedor)
	GROUP BY SE.Data_movimento,Funcionario.Comissao
END
end


