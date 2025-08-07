
alter table centro_custo add Agrupamento varchar(4)

go
Alter table inventario_itens add venda numeric(12,2)
go




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Movimento_Venda]
end
GO
--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Movimento_Venda]

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
							INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01)) ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim
                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
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

                        S.PLU,

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

                        S.PLU,

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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_conta_a_pagar]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And FORNECEDOR.fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if(LEN(@Conferido)>1)
	begin
		set @where = @where + ' And conferido =1 '	
	end 
	
	if LEN(@status)>0
	begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
																 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
																 WHEN @STATUS='CANCELADO' THEN ' status =3'
																 WHEN @STATUS='LANCADO' THEN ' status =4'
																ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
																) 
	end
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Dupl = case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			
			[Tipo pag]=tipo_pagamento,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_pagar.codigo_centro_custo,
			Banco = Conta_a_pagar.id_cc					
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
			left outer join Fornecedor on Conta_a_pagar.Fornecedor = Fornecedor.Fornecedor
		'+@where+'  Order By convert(varchar ,'+@tipo	+',102) '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
	--	set @string = @string + 'Documento = rtrim(ltrim(documento)), '
		--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
		--set @string = @string + @tipo + '= '+ @tipo + ', '
		--set @string = @string + 'Total = valor - isnull(desconto,0), '
		--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
			--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
			--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
			--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
		--set @string = @string + 'From Conta_a_pagar '
		--set @string = @string + @where
		--set @string = @string + ' Order By '+ @Tipo + ', Fornecedor, Documento'
	--Print @string
	Exec(@string)
End

go 




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Posicao_Cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_Posicao_Cliente]
end
GO
--PROCEDURES =======================================================================================
CREATE    procedure [dbo].[sp_rel_fin_Posicao_Cliente](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@valor VARCHAR(11) ,
@Codigo_cliente varchar(50),
@status varchar(10),
@centrocusto varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples	as varchar(5)	
)
--sp_rel_FIN_POSICAO_CLIENTE 'MATRIZ', '20151101', '20151126', 'EMISSAO', '', '1213', '', '', '', '',''
As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)

SET @AVista = CASE
WHEN @AVista = 'AVISTA' THEN 'AV'
WHEN @AVista = 'PRAZO' THEN 'PZ'
ELSE ''
END
Begin
--Monta Clausula Where da Procura
set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo
if len(rtrim(ltrim(@Codigo_cliente))) > 0
Begin
set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
End
if len(rtrim(ltrim(@valor))) > 1
Begin
set @where = @where + ' And valor ='+REPLACE(@valor,',','.')
End
if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE  ' and Conta_a_receber.status =1' end --'status like '+CHAR(39)+'%'+CHAR(39) END

)
end

if LEN(@status)=0
begin
set @Where = @Where + ' and Conta_a_receber.status <>3'

end

if LEN(@centrocusto)>0
begin
set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39)
end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF LEN(@AVista ) >0
begin
SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) And CONVERT(VARCHAR, PD.pedido_simples) LIKE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'SIM' + CHAR(39) + ' THEN  '+ CHAR(39) +  '1' + CHAR(39) + '  ELSE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'NAO' + CHAR(39) + ' THEN  '+ CHAR(39) +  '0' + CHAR(39) + ' ELSE ' + CHAR(39) +  '%' + CHAR(39) + 'END END '



--Monta Select
set @string = 'select
Simples = Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ',
Documento = rtrim(ltrim(documento)),
Cliente = rtrim(ltrim(cliente.Codigo_Cliente)),
Nome_Cliente = rtrim(ltrim(cliente.nome_Cliente)),
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.Codigo_Cliente, cliente.nome_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

-- set @string = @string + 'Documento = rtrim(ltrim(documento)), '
--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
--set @string = @string + @tipo + '= '+ @tipo + ', '
--set @string = @string + 'Total = valor - isnull(desconto,0), '
--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
--set @string = @string + 'From Conta_a_pagar '
--set @string = @string + @where
--set @string = @string + ' Order By convert('+ @Tipo +',102) '
-- Print @string
Exec(@string)
End


GO
/****** Object:  StoredProcedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido]    Script Date: 11/26/2015 16:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  procedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@Codigo_cliente varchar(50),
@status varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples	as varchar(5)	
)
--sp_rel_fin_Ficha_Cliente_Pedido 'MATRIZ', '20151101', '20151126', 'EMISSAO', '1213', '', '', '', ''
As

Declare @String as nvarchar(4000)
Declare @Where as nvarchar(4000)

SET @AVista = CASE
WHEN @AVista = 'AVISTA' THEN 'AV'
WHEN @AVista = 'PRAZO' THEN 'PZ'
ELSE ''
END
Begin
--Monta Clausula Where da Procura
set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo
if len(rtrim(ltrim(@Codigo_cliente))) > 0
Begin
set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
End

if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE  ' and Conta_a_receber.status =1' end --'status like '+CHAR(39)+'%'+CHAR(39) END

)

end
if LEN(@status)=0
begin
set @Where = @Where + ' and Conta_a_receber.status <>3'

end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF LEN(@AVista ) >0
begin
SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) And CONVERT(VARCHAR, PD.pedido_simples) LIKE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'SIM' + CHAR(39) + ' THEN  '+ CHAR(39) +  '1' + CHAR(39) + '  ELSE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'NAO' + CHAR(39) + ' THEN  '+ CHAR(39) +  '0' + CHAR(39) + ' ELSE ' + CHAR(39) +  '%' + CHAR(39) + 'END END '



--Monta Select
set @string = 'select
Simples_Cliente = Ltrim(Rtrim(Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ' + ' + CHAR(39) + SPACE(1) + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente)))) ' + ', 
Documento = rtrim(ltrim(documento)),
Codigo = Ltrim(Rtrim(cliente.Codigo_Cliente)),
Cliente = Substring(Ltrim(Rtrim(cliente.nome_Cliente)),1,1),
Nome = ' + CHAR(39) + 'VALOR TOTAL DO CLIENTE ' + CHAR(39) + ',
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
Prazo = Convert(Varchar,DATEDIFF(DAY,GETDATE(), vencimento )) ,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' into #t from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.nome_Cliente, cliente.Codigo_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

--Set @String = @string + 'insert into #t select Simples_Cliente, ' + CHAR(39) + CHAR(39) + ',Codigo,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome order by 3 '
Set @String = @string + 'insert into #t select Simples_Cliente + ' + CHAR(39) + ' - Sub Total' + CHAR(39) + ', ' + CHAR(39) +  CHAR(39) + ',Codigo,cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select Nome, ' + CHAR(39) + CHAR(39) + ',Codigo,Cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select ' + CHAR(39) + 'TOTAL GERAL - FICHA DE CLIENTE' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' +  CHAR(39) + '999' + CHAR(39) + ',' + CHAR(39) + 'W' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + 'SUM(VlrReceber),Sum(VlrAberto),' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t WHERE NOME = '+ CHAR(39) + 'VALOR TOTAL DO CLIENTE' + CHAR(39) 
Set @String = @string + 'Select Simples_Cliente , Documento, VlrReceber, VlrAberto, Emissao, Vencimento, Convert(Varchar,Prazo) Prazo, TipoRecebimento, Codigo, Cliente From #t Where Simples_Cliente <> ' + CHAR(39) + CHAR(39) + ' Order by 10,9,1,3,2'
--Union Select ' + CHAR(39) + 'TOTAL CLIENTE' + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente)))) ' + ', ' + CHAR(39) + CHAR(39) + ',sum(VlrReceber), SUM(VlrAberto),'+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ', Codigo from #t group by Codigo Order by Simples_Cliente '
-- set @string = @string + 'Documento = rtrim(ltrim(documento)), '
--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
--set @string = @string + @tipo + '= '+ @tipo + ', '
--set @string = @string + 'Total = valor - isnull(desconto,0), '
--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
--set @string = @string + 'From Conta_a_pagar '
--set @string = @string + @where
--set @string = @string + ' Order By convert('+ @Tipo +',102) '
--Print @string
Exec(@string)
End
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_FLUXO_CAIXA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_FLUXO_CAIXA]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10),@STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20),@tipo varchar(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo;

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo1]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo1;

   select A.DATA,A.dia_da_Semana,FORNEC_CLIENTE, [RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo FROM (
	select 
			Data = case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end,
			Dia_da_Semana = case 
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 2 then 'Segunda'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 3 then 'Terça'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 4 then 'Quarta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 5 then 'Quinta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 6 then 'Sexta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 7 then 'Sabado'
									Else 'Domingo' end,
									
			FORNEC_CLIENTE= Fornecedor,
			Receber = 0,
			Pagar = sum(Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0))
			
		from conta_a_pagar 
		where filial=@FILIAL and 
			case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end between @datade and @dataate AND (Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
																	 WHEN @STATUS='CONCLUIDO' THEN '2'
																	 WHEN @STATUS='CANCELADO' THEN '3'
																	 WHEN @STATUS='LANCADO' THEN '4'
																	ELSE '%' END
																	) )
														AND (Status <> (CASE WHEN @STATUS<>'CANCELADO' THEN '3' ELSE '1' END)) 
																	
																	and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
																							  when LEN (@CLIENTE)>0 then ''
																						  		else '%' 
																								end)
			group by case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end,conta_a_pagar.Fornecedor 
	union 		
			
	select 
			Data = case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end,
			Dia_da_Semana = case 
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 2 then 'Segunda'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 3 then 'Terça'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 4 then 'Quarta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 5 then 'Quinta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 6 then 'Sexta'
									when datepart(dw, case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end) = 7 then 'Sabado'
									Else 'Domingo' end,
			FORNEC_CLIENTE= ISNULL(Cliente.Nome_Cliente,'Lanc. Aut. PDV'),						
			Receber =sum(Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0)),
			Pagar = 0
			
		from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
		where filial=@FILIAL and 
			(case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end between @datade and @dataate ) AND (Status like(CASE WHEN @STATUS='ABERTO' THEN '1'
																	 WHEN @STATUS='CONCLUIDO' THEN '2'
																	 WHEN @STATUS='CANCELADO' THEN '3'
																	 WHEN @STATUS='LANCADO' THEN '4'
																	ELSE '%' END
																	)
																	)
																	AND (Status <> (CASE WHEN @STATUS<>'CANCELADO' THEN '3' ELSE '1' END))
																	and( ISNULL(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
																									 when LEN(@FORNECEDOR)>0 then ''	
																								else '%' 
																								end
																								)
			group by case when @tipo ='vencimento' then  vencimento  when @tipo ='emissao' then emissao when @tipo ='entrada' then entrada end,Cliente.Nome_Cliente
	)A GROUP BY DATA,Dia_da_Semana,FORNEC_CLIENTE

create table #tmp_Fluxo1 (ID INT IDENTITY, DATA DATETIME,DIA_DA_SEMANA VARCHAR(20),FORNEC_CLIENTE VARCHAR(50),RECEBER DECIMAL(12,2) ,PAGAR DECIMAL(12,2) , SALDO_DIA DECIMAL(12,2) )

INSERT INTO #tmp_Fluxo1 		
				select * from #tmp_Fluxo 
		 
		
	--select * from 	#tmp_Fluxo1
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [FORNEC/CLIENTE]=FORNEC_CLIENTE,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo1 B WHERE b.ID <a.ID)+A.SALDO_DIA) from #tmp_Fluxo1 A ORDER BY CONVERT(VARCHAR,A.DATA,102)  
			  
end


go


--PROCEDURES =======================================================================================
ALTER   PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@plu				AS Varchar(20),
	@ean				as varchar(20),
	@ref				as Varchar(20),
	@descricao			as Varchar(40),
	@Movimentacao	As Varchar(30) = ''
AS
	Declare @String	As nVarchar(1024)
		
BEGIN
	SET @String = ' SELECT'
	SET @String = @String + ' i.Codigo_inventario As Codigo,'
	SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
	SET @String = @String + ' i.tipoMovimentacao,'
	SET @String = @String + ' i.Usuario,'
	SET @String = @String + ' ii.plu,'
	SET @String = @String + ' m.descricao,'
	SET @String = @String + ' ii.Contada AS Qtde,'
	SET @String = @String + ' ii.custo as Vlr_Custo,'
	SET @String = @String + ' Total_Custo = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada )),'
	SET @String = @String + ' ii.venda as Vlr_Venda,'
	SET @String = @String + ' Total_Venda = CONVERT(DECIMAL(12,2), (m.preco * ii.Contada ))'
	
	SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
	SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
	SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
	SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
	BEGIN
		IF LEN(ISNULL(@Movimentacao ,'')) > 0 
			SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@plu ,'')) > 0 
			SET @String = @String + ' AND ii.plu = ' + CHAR(39) + @plu + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ean ,'')) > 0 
			SET @String = @String + ' AND ean = ' + CHAR(39) + @ean + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ref ,'')) > 0 
			SET @String = @String + ' AND m.Ref_fornecedor = ' + CHAR(39) + @ref + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@descricao ,'')) > 0 
			SET @String = @String + ' AND m.descricao like ' + CHAR(39) + '%'+@descricao +'%'+ CHAR(39)
	END
	SET @String = @String + ' ORDER BY 1'

--PRINT @STRING
  EXECUTE(@String)

END
