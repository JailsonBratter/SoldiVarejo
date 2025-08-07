GO
DECLARE @LINHA AS INTEGER
SELECT @LINHA = COUNT(*) FROM PARAMETROS WHERE PARAMETRO = 'LINHAS_IMP_POS_PEDIDO'
IF @LINHA <= 0
	BEGIN
		INSERT INTO PARAMETROS (PARAMETRO, PENULT_ATUALIZACAO, VALOR_DEFAULT, ULT_ATUALIZACAO, VALOR_ATUAL, DESC_PARAMETRO, TIPO_DADO, GLOBAL, POR_USUARIO_OK, PERMITE_POR_EMPRESA)
		VALUES
		('LINHAS_IMP_POS_PEDIDO', GETDATE(), 0, GETDATE(), 0, 'NUMERO DE LINHAS A SEREM IMPRESSAS APOS IMPRESSAO DO PEDIDO DE VENDA', 'N', 1, 0, 0)
	END
	
GO

/****** Object:  Index [IX_Lista_Finalizadora_ResumoPorDia]    Script Date: 12/29/2015 11:15:33 ******/
CREATE NONCLUSTERED INDEX [IX_Lista_Finalizadora_ResumoPorDia] ON [dbo].[Lista_finalizadora] 
(
      [filial] ASC,
      [Emissao] ASC,
      [Cancelado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  Index [IX_Saida_Estoque_ResumoPorDia]    Script Date: 12/28/2015 14:26:09 ******/
CREATE NONCLUSTERED INDEX [IX_Saida_Estoque_ResumoPorDia] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[data_cancelamento] ASC,
	[Hora_venda] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperadorCancelamento]    Script Date: 12/28/2015 10:30:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- sp_Rel_Fin_PorOperadorCancelamento 'matriz', '2015-12-28', '2015-12-28', 'TODOS', 'TODOS'

--PROCEDURES =======================================================================================
ALTER  PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As DATETIME,
      @Dataate			As DATETIME,
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as
	declare @strQuery as nvarchar(3000)
	declare @strWhere As nVarchar(1024)
    set @strWhere = ''
    
if len(@Operador) > 0 AND @Operador <>'TODOS'

      begin
            set @strWhere = ' AND b.Nome LIKE ' + CHAR(39) + @Operador + '%' + CHAR(39)     
      end
      
if (LEN(@Pdv) > 0 AND @Pdv <>'TODOS')

		begin
			set @strWhere = @strWhere + ' AND a.PDV = ' + @PDV 
		end
 
set @strQuery = 'select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data ' 
set @strQuery = @strQuery + ', Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and isnull(c.cancelado,0)=0), 0) as  Vendas'
set @strQuery = @strQuery + ', isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   isnull(c.cancelado,0)=1),0)as  Cancelados'
set @strQuery = @strQuery + ' from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador '
set @strQuery = @strQuery + ' where a.filial= ' + char(39) + @FILIAL + char(39) + ' and a.emissao between ' + char(39) + REPLACE(CONVERT(VARCHAR, @Datade, 102), '.', '-') + char(39) + ' and ' + + char(39) + REPLACE(CONVERT(VARCHAR, @Dataate, 102), '.', '-') + char(39) 
set @strQuery = @strQuery + @strWhere
set @strQuery = @strQuery + ' group by a.operador,a.Pdv, b.nome,a.emissao '
set @strQuery = @strQuery + ' order by a.Pdv, b.nome'

--print @strQuery 
execute(@strQuery)

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperador]    Script Date: 12/28/2015 11:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
ALTER PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(30),
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

if(LEN(@fornecedor)=0 and @Caixa='TODOS')
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
	if(@Caixa<>'TODOS')
		begin
				select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
			from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
			inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
			--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
			--inner join Operadores on l.operador = Operadores.ID_Operador	
			where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade 
			    and Data_movimento <=@Dataate 
				and (Select TOP 1 Operadores.Nome from Operadores inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador where  l.filial =a.Filial  and l.Emissao = a.Data_movimento and a.Caixa_Saida = l.pdv and a.Documento=L.Documento) = @Caixa 
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
		and (@Caixa='TODOS' OR isnull(Operadores.Nome ,'') like @Caixa)
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and fm.fornecedor = @fornecedor
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
end

GO
/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 12/28/2015 11:33:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
ALTER   Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),

                @plu               As varchar(17),

                @cupom             As varchar(20),

                @pdv               as varchar(10),
                
                @horaInicio      as varchar(5),
				
				@horafim	     as varchar(5),
				@cancelados		as varchar(5)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='TODOS')

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
                                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
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
                         and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
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
                         --and (LEN(@pdv)=0 or l.pdv = @pdv)
                         and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
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
                        and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
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
                        and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        
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
/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_ResumoPorOperador]    Script Date: 12/28/2015 11:42:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- sp_Rel_Fin_ResumoPorOperador 'matriz','20151228', 'TODOS', 'TODOS', '00:00', '23:59'
--PROCEDURES =======================================================================================
ALTER PROCEDURE [dbo].[sp_Rel_Fin_ResumoPorOperador](
	@FILIAL 	AS VARCHAR(17),
	@Data		As Varchar(8),
	@Operador	As Varchar(40),
	@Pdv		As Varchar(10),
	@horade		As varchar(5),
	@horaate	As varchar(5))
as
	
if len(@horade)= 0
	begin
		set @horade = '0'
	end
if len(@horaate) = 0
	begin
		set @horaate = '24'
	end
	
Declare @pIndex int 
Declare @idOp int 

if @Operador <> 'TODOS'
begin
	set @pIndex= CHARINDEX('-', @Operador)	
    set @idOp = SUBSTRING(@Operador,0,@pIndex)

end


Select 
	isnull(finalizadora.Finalizadora,'Outra') as Finalizadora, sum(ISNULL(total,0)) as valor

From 	Lista_Finalizadora
Left Outer Join	finalizadora on 
	lista_finalizadora.finalizadora = finalizadora.nro_finalizadora
Where
	lista_finalizadora.filial = @filial and 
	Emissao = @data  
    --and (@Operador='TODOS' OR isnull(Operador ,'') = @idOp)
	--and (pdv = @Pdv or @pdv='TODOS')and
	and operador like (case when @idOp > 0 then CONVERT(VARCHAR, @idOp) else '%' end)
	and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
	and isnull(cancelado,0) = 0 And
	exists(Select * from saida_Estoque where
			data_movimento = @data and 
			convert(decimal(18,2),substring(replace(hora_venda,':','.'),1,5)) 
					between isnull(convert(decimal(18,2), replace(@horade,':','.')),0) and isnull(convert(decimal(18,2), replace(@horaate,':','.')),24)
			and lista_finalizadora.cupom = saida_Estoque.documento)
group by 
	finalizadora.Finalizadora

USE [bratter]
GO
/****** Object:  StoredProcedure [dbo].[sp_REL_Fin_ResumoPorDia]    Script Date: 12/28/2015 13:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Sp_Rel_Fin_ResumoPorDia 'MATRIZ', '20120919', '20120920', '', ''
ALTER      Procedure [dbo].[sp_REL_Fin_ResumoPorDia](
	@Filial		As varchar(20),
	@DataDe		As varchar(8),
	@DataAte	As Varchar(8),
	@HoraDe		As Varchar(5),
	@HoraAte	As Varchar(5))
As
if len(ISNULL(@horade,''))= 0 And len(ISNULL(@horaate,'')) = 0
	begin
		set @horade = '01:00'
		set @horaate = '99:99'
	end
	
Select distinct filial, documento, data_movimento, caixa_saida Into #Saida 
		from saida_estoque with(index(IX_Saida_Estoque_ResumoPorDia)) where
		Saida_estoque.Filial = @Filial And
		Data_movimento between @DataDe and @DataAte AND data_cancelamento is null  And
		convert(decimal(18,2),substring(replace(hora_venda,':','.'),1,5)) 
			between convert(decimal(18,2), replace(@HoraDe,':','.')) and convert(decimal(18,2), replace(@HoraAte,':','.'))
				
Select 
	Nome = Case When Grouping(Nome) = 1 Then ''--'Total Em: ' + Convert(varchar,emissao,103)+ ' Das '+ @HoraDe +'h Até ' + @HoraAte + 'h - R$'+ (Convert(Varchar,sum(total))) 
			Else Isnull(Nome,'Outras') End,
  
	Data =  Case 	When Grouping(nome) = 1 then '          Total Em: ' + Convert(varchar,emissao,103)+ ' Das '+ @HoraDe +'h Até ' + @HoraAte + 'h'
			else 'R$ ' + Convert(varchar,sum(ISNULL(total,0))) end,

	Total = Case 	When grouping(Nome)=1 and Grouping(emissao) = 0 Then 'R$ '+ (Convert(Varchar,sum(total))) 
			When Grouping(Nome) = 1 Then 'Total Geral No Periodo: R$ ' + convert(varchar,Sum(isnull(total,0)))
			Else '' End

From Lista_Finalizadora l with(index(IX_Lista_Finalizadora_ResumoPorDia))
	Left Join Operadores O on l.operador  = O.Id_operador
	inner JOIN #saida s ON l.Filial = s.Filial And l.Emissao = s.Data_movimento AND S.CAIXA_SAIDA = L.PDV And l.Cupom = s.Documento 
Where
	l.filial = @filial and 
	Emissao between  @datade And @dataate And
	isnull(Cancelado,0) = 0 --And
--	Exists(Select * from #Saida s where l.Filial = s.Filial And l.Cupom = s.Documento
--		And l.Emissao = s.Data_movimento 
		 --And Data_movimento between @DataDe and @DataAte And convert(decimal(18,2),substring(replace(hora_venda,':','.'),1,5)) 
			--between convert(decimal(18,2), replace(@horade,':','.')) and convert(decimal(18,2), replace(@horaate,':','.'))
--			)
group by Emissao, Nome

With Rollup




GO
/****** Object:  StoredProcedure [dbo].[sp_rel_Resumo_Vendas_Pdv]    Script Date: 12/28/2015 14:24:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
ALTER  Procedure [dbo].[sp_rel_Resumo_Vendas_Pdv] 
		@Filial		As Varchar(20),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@PDV		AS VARCHAR(10)

AS
	Declare @String	As nVarchar(1024)
begin
	set @String= '	SELECT '
	
	SET @String = @String + '	convert(varchar,Data_Movimento,103) as Data,'
	Set @String = @String + 'Dia_da_Semana = case 
								when datepart(dw, Data_Movimento) = 2 then '+ CHAR(39) +'Segunda'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 3 then '+ CHAR(39) +'Terça'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 4 then '+ CHAR(39) +'Quarta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 5 then '+ CHAR(39) +'Quinta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 6 then '+ CHAR(39) +'Sexta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 7 then '+ CHAR(39) +'Sabado'+ CHAR(39) +
								'Else '+ CHAR(39) +'Domingo'+ CHAR(39) +' end,'
	SET @String = @String + '	Caixa_Saida as PDV, '
	SET @String = @String + '	Valor = SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))'
		
	SET @String = @String + ' FROM '
	SET @String = @String + '	Saida_Estoque '
	SET @String = @String + ' WHERE '
	SET @String = @String + '	Filial = '+ CHAR(39) +@Filial+ CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Movimento BETWEEN '+ CHAR(39) + @DataDe + CHAR(39) +' AND '+ CHAR(39) +@DataAte + CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Cancelamento IS NULL ' 
	begin 
		IF (LEN(ISNULL(@PDV,'')) > 0 and @PDV <> 'TODOS')
			SET @String = @String + ' and CAIXA_SAIDA IN (' +@PDV+ ')'
		
	end	
				
	SET @String = @String + 'GROUP BY Filial,Data_Movimento, Caixa_Saida'
	EXECUTE(@String)
--	print @string
end


