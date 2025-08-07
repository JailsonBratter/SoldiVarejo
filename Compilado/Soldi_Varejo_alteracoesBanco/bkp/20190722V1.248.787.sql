 go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas_hora_media]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas_hora_media]
GO

--PROCEDURES =======================================================================================
create Procedure [dbo].[sp_Rel_Resumo_Vendas_hora_media]
            @Filial           As Varchar(20),
            @DataDe           As Varchar(8),
			@DataAte          As Varchar(8),
			@ini_periodo	  as varchar(5),
			@fim_periodo	  as varchar(5),
            @plu			  as Varchar (20),
            @descricao		  as varchar(50),
            @grupo			  as Varchar(50),
            @subGrupo		  as varchar(50),
            @departamento	  as varchar(50),
            @relatorio		  as varchar(40),
			@pdv			  as varchar(5)

 

AS
-- EXEC sp_Rel_Resumo_Vendas_hora_media 'MATRIZ','20190122','20190122','00:00','23:59','','','','','','TODOS','TODOS'
--@relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
	
CREATE TABLE #tmpVendas
(
	hora varchar(20),
	Venda Decimal(18,2),
	Qtde Decimal(18,2),
	Clientes int
	
)

	declare @dias int 
	declare @nPdv  int 

	if(@pdv <> 'TODOS')
	begin
	  set @nPdv = Convert(int,@pdv);
	end
	else
	begin
	 set @nPdv =0;
	end

	  print('n'+convert(varchar,@nPdv))
	  PRINT (@PDV)

	Select @dias = DATEDIFF(day,@DataDe,@DataAte)+1
	--print(convert(varchar,@dias))

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT      saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            Qtde =SUM(ISNULL(Saida_estoque.Qtde,0) ),
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
			  and   hora_venda between @ini_periodo+':00' and @fim_periodo+':59'
			  AND   Data_Movimento  between @DataDe and @DataAte
			  AND   Data_Cancelamento IS NULL
			  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
			  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
			  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
			 AND (@nPdv = 0 OR Saida_estoque.Caixa_Saida = @nPdv)
      
      GROUP BY
            Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

PRINT('#TEMPVENDAS')
insert into #tmpVendas
 SELECT
	  substring(Hora_venda,1,2)+':00 - '+substring(Hora_venda,1,2)+':59',
      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
      Qtde = (SELECT Convert(Decimal(18,2),SUM(ISNULL(Qtde,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
	  Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) 
    
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE
            Saida_Estoque.Filial = @Filial
		  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
		  AND (Data_Movimento between @DataDe and @DataAte)
		  AND (hora_venda between @ini_periodo+':00' and @fim_periodo+':59')
		  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
		  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
		  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
		  AND (@nPdv = 0 OR Saida_estoque.Caixa_Saida = @nPdv)
	  GROUP BY    Saida_Estoque.Data_movimento , substring(Hora_venda,1,2)
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
		substring(N.emissao_hora,1,2)+':00 - '+substring(N.emissao_hora,1,2)+':59'  as horas,
		Venda = SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) ,
		Qtde = SUM(isnull(ni.Qtde,0) * isnull(ni.Embalagem,0)) ,
		Clientes = (
		  				Select COUNT(*) 
					from nf 
						inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
							AND  (NF.Emissao = n.Emissao )
							and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
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
       
      

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao between @DataDe and @DataAte )
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 and  (n.emissao_hora between @ini_periodo+':00' and @fim_periodo+':59')
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY substring(N.emissao_hora,1,2),n.emissao
END


print 'media'
 Select Hora
		,Sum(Venda) as [Venda TT]
		,case when Sum(Venda)>0 then  (Sum(Venda)/@dias) else 0 end as [Venda MD]
		,Sum(Qtde) as [Qtde TT]
		,case when Sum(Qtde)>0 then  (Sum(Qtde)/@dias) else 0 end as [Qtde MD]
		,Sum(clientes) as [Clientes TT]
		,case when SUM(Clientes) >0 then (SUM(Clientes)/@dias) else 0 end as [Clientes MD] 
		
from 
	#tmpVendas
GROUP BY hora
ORDER BY hora ;



go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Curva_ABC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Curva_ABC]
GO

--PROCEDURES =======================================================================================
create    Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial         As Varchar(20),
                @DataDe         As Varchar(8),
                @DataAte        As Varchar(8),
                @nLinhas        As Integer,
                @Descricao      As Varchar(60),
                @Grupo          As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento	as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40),
                @ini_periodo	as varchar(5),
				@fim_periodo	as varchar(5),
				@pdv			as varchar(5)
                
AS
  Declare @String As nVarchar(2000)
  Declare @Where  As nVarchar(1024)
BEGIN
	

	--exec  sp_Rel_Curva_ABC @Filial='MATRIZ', @datade = '20190722',  @dataate = '20190722',  @ini_periodo = '00:00',  @fim_periodo = '23:59',  @Pdv = 'TODOS',  @Ordem = 'QTDE',  @nlinhas = '0',  @Descricao = '',  @grupo = '',  @subGrupo = '',  @departamento = '',  @Familia = '' 
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) +
					   ' And  Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And  data_cancelamento is null 
						and (isnull(mercadoria.curva_a,0)=1 
						and isnull(mercadoria.curva_b,0)=1 
						and isnull(mercadoria.curva_c,0) = 1) '
		Set @Where = @Where + ' And  (saida_estoque.hora_venda between '+CHAR(39)+@ini_periodo +CHAR(39)+' and '+CHAR(39)+@fim_periodo+CHAR(39)+')'  
		
		
		--** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver, 
                --** o sistema faz um LIKE no campo descrição recebido no parametro.
                BEGIN
                               IF LEN(ISNULL(@DESCRICAO,'')) > 0 
                                               SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)
                END       
                --** Checa se o parametro @Grupo contem alguma informação. Se tiver, 
                
                BEGIN
                               IF LEN(ISNULL(@GRUPO,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_GRUPO='+ CHAR(39) + @GRUPO + CHAR(39) 
                               IF LEN(ISNULL(@subGrupo,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_SUBGRUPO='+ CHAR(39) + @subGrupo + CHAR(39) 
							   IF LEN(ISNULL(@Departamento,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_DEPARTAMENTO='+ CHAR(39) + @Departamento + CHAR(39) 
							   IF LEN(ISNULL(@Familia,'')) > 0 
                                               SET @Where = @Where + ' AND familia.DESCRICAO_familia='+ CHAR(39) + @Familia + CHAR(39) 
                
                END       
		
		Begin
			if LEN(ISNULL(@Ordem,'')) = 0 
			Set @Ordem ='Qtde'
		end


		if(@pdv<>'TODOS')
		begin 
				SET @Where = @Where +  ' and saida_estoque.caixa_saida ='+@pdv
		end
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                  if ISNULL(@nLinhas, 0) > 0 
                     SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' ROW_NUMBER() over(order by Convert(Numeric(10,3), Sum(Qtde)) desc) as RANK,
										  Mercadoria.PLU, 
										  EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '),
										  Mercadoria.Descricao,'
                SET @String = @String + ' [Custo] = MERCADORIA.PRECO_CUSTO, '
                SET @String = @String + ' [Margem] = MERCADORIA.MARGEM, '
                SET @String = @String + ' [Preco] = MERCADORIA.PRECO, '
                                 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
               
                SET @String = @String + ' [Valor] = Convert(Numeric(12,2), Sum(isnull(VLR,0)-isnull(desconto,0))), '
                set @String = @String + ' [Rentabilidade] = Convert(Numeric(12,2),SUM((isnull(VLR,0)-isnull(desconto,0)) - (Qtde * isnull(Mercadoria.Preco_Custo,0))))'
 
	
				SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque  WITH (INDEX(Ix_Fluxo_Caixa)) ON Mercadoria.PLU = Saida_Estoque.PLU 
												  inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento 
												  left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 	
				set @String = @String + @Where
	
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao, MERCADORIA.PRECO_CUSTO,MERCADORIA.MARGEM,MERCADORIA.PRECO '
			
				if(@pdv<>'TODOS')
					SET @String = @string + ', saida_estoque.caixa_saida '

				SET @String = @string + 'Order by ['+@Ordem+'] Desc'
 
--  PRINT @STRING
                EXECUTE(@String)
END



go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO]
GO

--PROCEDURES =======================================================================================
create  PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10), @tipo varchar(10), @STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo_simples]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo_simples;
select A.DATA,A.dia_da_Semana,[RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo_simples FROM (

select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
		,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber = 0,
		Pagar = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from conta_a_pagar 
	where Filial =@FILIAL and
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
				between @datade and @dataate 
		
		
		
		AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>3))
							and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
														  when LEN (@CLIENTE)>0 then ''
																						  	else '%' 
																							end)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
union 		
		
select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber =convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0)))),
		Pagar = 0
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where Filial = @FILIAL and  case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate  
				
				AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>'3'))
																and( isnull(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
																								 when LEN(@FORNECEDOR)>0 then ''	
																							else '%' 
																							end
																							)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
)A GROUP BY DATA,Dia_da_Semana
		 
		
		
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO_GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo_simples B WHERE B.Data <A.Data)+A.SALDO_DIA) from #tmp_Fluxo_simples A ORDER BY CONVERT(VARCHAR,A.DATA,102)  ;
			  
END


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_FLUXO_CAIXA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA]
GO

--PROCEDURES =======================================================================================
create  PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10),@STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20),@tipo varchar(20)
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_balanceteFinanceiro]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_balanceteFinanceiro]
GO

--PROCEDURES =======================================================================================
create   procedure [dbo].[sp_rel_balanceteFinanceiro](
      @filial           varchar(20),
      @datade           varchar(8),
      @dataate    varchar(8),
      @tipo       varchar(50),
      @status         varchar(10),
      @visualizar varchar(50),
      @Modalidade varchar(8)
)As
-- sp_rel_balancetefinanceiro 'matriz', '20160801', '20160830', 'emissao', 'previsto', 'sintetico', 'todos'

Declare @String as varchar(8000)
Declare @Where as varchar(1024)
Declare @codStatus as varchar(50)
DECLARE @String2 as varchar(8000)
DECLARE @String3 as varchar(8000) 
Declare @Tabela as varchar(15)

Begin

      if (@status ='ABERTO')
            begin
            set @codStatus= ' = 1'
            end
      else 
            begin
                  if (@status ='CONCLUIDO')
                        begin
                              set @codStatus= ' = 2'
                        end
                  else 
                        begin
                              if (@status ='CANCELADO')
                                   begin
                                         set @codStatus= ' = 3'
                                   end
                              else 
                                   begin
                                         set @codStatus= ' <> 3'
                                   end
                        end
            end
            if (@tipo ='')
            begin
                  set @tipo= 'emissao'
            end

IF @Modalidade = 'DESPESAS'
      Set @Tabela = 'Conta_a_Pagar'


IF @Modalidade = 'RECEITAS'
      Set @Tabela = 'Conta_a_Receber'

IF @Modalidade = 'TODOS'                 
      Set @Tabela = 'W_br_contas'


IF @visualizar = 'ANALITICO'
      Goto CentroCusto
Else
      IF @visualizar = 'SINTETICO'
            Goto Grupo        
            

CentroCusto:      
      --Monta Clausula Where da Procura
      set @where = 'Where W_br_contas.Filial = '+ char(39) + @filial + char(39) + '   and status '+@codStatus+' and '
      set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
      
      IF @Modalidade <> 'TODOS'
                  Set @where = @where +' and modalidade = ' + char(39) + @Modalidade + char(39) + ' '
      --Monta Select
      set @string = 'Select Codigo,Descricao,replace(isnull(debito,'+ char(39) +'0,00'+ char(39) +'),'+ char(39) +'.'+ char(39) +','+ char(39) +','+ char(39) +') as Debito,Replace(isnull(credito,'+ char(39) +'0,00'+ char(39) +'),'+ char(39) +'.'+ char(39) +','+ char(39) +','+ char(39) +') as Credito from ( '+
                        /*'Select modalidade, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as ordem, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as codigo, descricao_Grupo as descricao , ' + char(39) +' ' + char(39) +' as Debito , ' + char(39) +' ' + char(39) +' as Credito, '+char(39)+' '+char(39)+'as saldo from  W_br_contas '+@where+' group by modalidade, codigo_grupo,descricao_Grupo '+
                        'union '+
                        'select modalidade,codigo_subgrupo as ordem, codigo_subgrupo as codigo, descricao_subgrupo as descricao ,' + char(39) +' ' + char(39) +' as Debito , ' + char(39) +' ' + char(39) +' as Credito, '+char(39)+' '+char(39)+'as saldo  from  W_br_contas '+@where+' group by modalidade,codigo_grupo,codigo_subgrupo,descricao_subgrupo '+
                        'union '+
                        */
                        'Select  modalidade,codigo_centro_custo as ordem, codigo_centro_custo as codigo ,descricao_centro_custo as descricao, '+
                        '     Debito = convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) , '+
                        '     Credito = convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10), '+
                        '           Saldo = '+char(39) +char(39) +
                        'from  W_br_contas '+@where+' '+
                        'group by modalidade,codigo_grupo ,codigo_centro_custo,descricao_centro_custo '+
                        'union '+
                        
                        'Select modalidade, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end+'+CHAR(39)+' '+CHAR(39)+' as ordem,'+CHAR(39)+CHAR(39)+' as codigo , '+CHAR(39)+CHAR(39)+', '+
                        '     Debito = '+CHAR(39)+CHAR(39)+','+
                        '     Credito = '+CHAR(39)+CHAR(39)+', '+
                        '    Saldo = '+CHAR(39)+CHAR(39)+' '+
                        '     from  W_br_contas '+@where+' '+
                        'group by modalidade,codigo_grupo,descricao_Grupo '+
                        'union '+
                        
                        'Select modalidade, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as ordem, ' + char(39) +'|-' + char(39) +'+CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end +' + char(39) +'-|' + char(39) +' as codigo , ' + char(39) +'|-' + char(39) +'+ descricao_Grupo +' + char(39) +'-|' + char(39) +', '+
                        '     Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
                        '     Credito = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +', '+
                        '           Saldo = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
                        '           sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' '+
                        '     from  W_br_contas '+@where+' '+
                        'group by modalidade,codigo_grupo,descricao_Grupo '+
                        'union '+
                        'Select modalidade, rtrim(codigo_subgrupo) as ordem,codigo_subgrupo as codigo, descricao_subgrupo,  '+
                        '     Debito =  convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) , '+
                        '     Credito =  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) ,  '+
                        '           Saldo = '+char(39) +char(39) +
                        '     from  W_br_contas '+@where+' '+
                        'group by modalidade, codigo_subgrupo,descricao_subgrupo '
                        
                  set @String2=     'union '+
                        'Select modalidade ='+char(39) +char(39) +',  '+char(39) +'999' + char(39) +' as ordem,' + char(39) +'|-TOTAL-| ' + char(39) +' as codigo, ' + char(39) + char(39) +',  '+
                        '     Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
                        '     Credito = ' + char(39) +'|-' + char(39) +'+ convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' ,  '+
                        '           Saldo = ' + char(39) +'|-' + char(39) +'+convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
                        '           sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10) +' + char(39) +'-|' + char(39) +''+
                        '     from  W_br_contas '+@where+' '+
                        ''+
                  
                        
            ') as a '+
                  'order by modalidade desc,ordem'
Goto Fim                
      
Grupo:
      Set @string = 'SELECT Codigo = case when Len(Convert(Varchar,E.codigo_grupo)) = 1 Then ' + char(39) + '00' + char(39) + ' + Convert(Varchar,E.codigo_grupo) when  Len(Convert(Varchar,E.codigo_grupo)) = 2 Then ' + char(39) + '0' + char(39) + ' + Convert(Varchar,E.codigo_grupo) Else Convert(Varchar,E.codigo_grupo) End, '
      Set @string = @string +' Modalidade = c.Modalidade, '
      Set @string = @string +' Descricao = E.Descricao_grupo , '
      Set @string = @string +' Valor = case when a.status = 1 then Sum(Isnull(Valor,0)-ISNULL(Desconto,0)+Isnull(Acrescimo,0)) when a.status = 2 then Sum(Valor_Pago) Else 0 end into #Balancete'
      Set @string = @string +' FROM ' + @Tabela + ' a INNER JOIN '
    Set @string = @string +' centro_custo c ON (a.codigo_centro_custo = c.codigo_centro_custo AND a.filial = c.filial) INNER JOIN '
    Set @string = @string +' subgrupo_cc d ON (c.codigo_subgrupo = d .codigo_subgrupo AND c.filial = d .filial) INNER JOIN '
    Set @string = @string +' grupo_cc e ON (d .codigo_grupo = e.codigo_grupo AND d .filial = e.filial '
    
    IF @Modalidade <> 'TODOS'
            Set @string = @string +' and c.modalidade = ' + char(39) + @Modalidade + char(39) + ' '

      set @String2 = ' ) Where a.Filial = '+ char(39) + @filial + char(39) + ' and a.status '+@codStatus+' and '
      set @String2 = @String2 + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)

      set @String2 = @String2 + ' GROUP BY E.codigo_grupo , E.Descricao_grupo,c.Modalidade, a.status order by codigo ' 
      set @String2 = @String2 + ' Select Distinct Codigo, Modalidade, Descricao, Valor = replace(convert(varchar(20),Sum(Valor * CASE WHEN MODALIDADE = ' + CHAR(39) + 'DESPESAS' + CHAR(39) + ' THEN -1 ELSE 1 END)),' + char(39) + '.' + char(39) + ',' + char(39) + ',' + char(39) + ') from #Balancete Group by Codigo, Modalidade, Descricao '
      set @String2 = @String2 + ' UNION all Select '+ char(39) + '|-TOTAL-|' + char(39) + ','+ char(39) + '' + char(39) + ',' + char(39) + '' + char(39) + ','+ char(39)+'|-'+ char(39)+' + replace(convert(varchar(20),Sum(isnull(Valor,0) * CASE WHEN MODALIDADE = ' + CHAR(39) + 'DESPESAS' + CHAR(39) + ' THEN -1 ELSE 1 END)),' + char(39) + '.' + char(39) + ',' + char(39) + ',' + char(39) + ') +'+ char(39)+'-|'+ char(39)+' from #Balancete '
      /*
      set @String2 = @String2 + ' UNION Select '+ char(39) + 'TOTAL' + char(39) + ','+ char(39) + '' + char(39) + ',Sum(Isnull(Valor,0)-Isnull(Desconto,0)+Isnull(Acrescimo,0)),SUM(Valor_Pago) '
      Set @String2 = @String2 +' FROM ' + @Tabela + ' a INNER JOIN '
    Set @String2 = @String2 +' centro_custo c ON (a.codigo_centro_custo = c.codigo_centro_custo AND a.filial = c.filial) INNER JOIN '
    Set @String2 = @String2 +' subgrupo_cc d ON (c.codigo_subgrupo = d .codigo_subgrupo AND c.filial = d .filial) INNER JOIN '
    Set @String2 = @String2 +' grupo_cc e ON (d .codigo_grupo = e.codigo_grupo AND d .filial = e.filial '
    
    IF @Operacao <> 'TODOS'
            Set @String2 = @String2 +' and C.modalidade = ' + char(39) + @operacao + char(39) + ' '

      Set @String2 = @String2 +' ) Where a.Filial = '+ char(39) + @filial + char(39) + ' and a.status '+@codStatus+' and '
      set @String2 = @String2 + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)    
      */
      set @String2 = @String2 + '  '
Goto Fim          

Fim:
      --Print @string+@String2
      Exec(@string+@String2)
End


 

 go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_validade_produtos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_validade_produtos]
GO

--PROCEDURES =======================================================================================
create  procedure [dbo].[sp_rel_validade_produtos]
	@filial varchar(20),
	@dataInicio varchar(8),
	@dataFim varchar(8)
as
begin 
Select ordem = dep.descricao_departamento
		,PLU =convert(varchar(100),ni.PLU)
	   ,ni.EAN
	   ,ni.Descricao
	   ,Fornecedor = nf.Cliente_Fornecedor
	   ,NF =ni.Codigo
	   ,Qtde =(ni.Qtde*ni.Embalagem) 
	   ,Emissao = convert(varchar,nf.Emissao,103)
	   ,Validade = convert(varchar,ni.data_validade,103)
 into #tbValidade from nf_item as ni 
 inner join nf on ni.Codigo = nf.Codigo 
			  and ni.Tipo_NF = nf.Tipo_NF
			  and ni.Filial =nf.Filial
			  and ni.Cliente_Fornecedor = nf.Cliente_Fornecedor

inner join mercadoria as m on m.plu =ni.plu
inner join W_BR_CADASTRO_DEPARTAMENTO as dep on m.Codigo_departamento = dep.codigo_departamento
where nf.Tipo_NF = 2 
    and nf.filial = @filial 
	and data_validade between @dataInicio and @dataFim
order by convert(varchar,data_validade,102)



	Select PLU
	   ,EAN
	   ,Descricao
	   ,Fornecedor
	   ,NF 
	   ,Qtde 
	   ,Emissao 
	   ,Validade 
	 from (
	 Select ORDEM = ORDEM + '11111'
		, PLU='PLU'+PLU
	   ,EAN ='PLU'+EAN
	   ,Descricao
	   ,Fornecedor
	   ,NF 
	   ,Qtde 
	   ,Emissao 
	   ,Validade  
	 from #tbValidade
	 union all 
	 select ordem+'00000'
	 ,'|-TITULO-||CONCAT|'+ordem
	   ,''
	   ,''
	   ,''
	   ,'' 
	   ,0
	   ,''
	   ,''
	   from #tbValidade
	   group by ordem
	 )as a
	 order by ordem
end



  go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA]
GO

--PROCEDURES =======================================================================================
create PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA](
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@plu as Varchar(20), 
@ean as varchar(20),
@Ref as Varchar(20),
@descricao as varchar(40)


)
As


DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

if(LEN(@ean)>0)
begin
	set @plu= (Select top 1 PLU from EAN where EAN = @ean);
end
--set @DataInicio ='20140401'
--set @DataFim ='20140910'
--Set @plu ='1'
-- exec SP_REL_HISTORIO_ENTRADA_SAIDA @FILIAL ='MATRIZ',@DataInicio ='20180101',@DataFim ='20180101',@plu ='42635',@ean ='', @Ref ='', @descricao =''
Create table #OUTROS
(
	DATA  DATETIME ,
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
	OUTRAS_MOVIMENTACOES DECIMAL(18,2),
	NF_Saida DECIMAL(18,2),
	NF_Saida_Preco	 DECIMAL(18,2),
	Cupom DECIMAL(18,2),  
	Cupom_Preco DECIMAL(18,2)
);

--select * from pedido_itens

IF @PedEstoque = 'TRUE'
INSERT INTO  #OUTROS select Data =pd.Data_cadastro  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,OUTRAS_MOVIMENTACOES =0 
		   ,[NF Saida] =Sum(itemPd.Qtde*itemPd.Embalagem)
		   ,[NF Saida Preco]	= Sum(itemPd.total)
		  ,[Cupom] = 0  
		  ,[Cupom Preco]=0
	 from pedido_itens itemPd inner join pedido pd on pd.Filial=@FILIAL and  pd.Pedido = itempd.Pedido and  pd.Tipo = itemPd.tipo
		inner join Natureza_operacao Eop on pd.cfop = Eop.Codigo_operacao
		INNER JOIN Mercadoria M ON M.PLU=itemPd.PLU
	where (itemPd.PLU=@plu OR (LEN(@REF)>0 AND  M.Ref_fornecedor=@Ref)) and  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
	group by pd.Data_cadastro,itemPd.total
ELSE
begin
INSERT INTO  #OUTROS 
	select Data =se.Data_movimento  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,OUTRAS_MOVIMENTACOES =0 
		   ,[NF Saida] =0
		   ,[NF Saida Preco]	= 0
		  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
		  ,[Cupom Preco]=Sum(se.vlr)	
	from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	where (se.PLU=@plu OR (LEN(@REF)>0 AND M.Ref_fornecedor=@Ref)) and se.Filial=@FILIAL and se.Data_movimento between @DataInicio AND @DataFim
	group by se.Data_movimento
END;


Select Data = CONVERT(varchar,todos.data,103)
		,[Entrada]=Sum(todos.[NF Entrada])
		,[Entrada_Valor]=Sum(todos.[NF Compra Preco])
		,[Outras_Mov]=SUM(OUTRAS_MOVIMENTACOES)
		,[Saida_Outros] = SUM(todos.[NF Saida])
		,[Saida_Valor] = Sum(todos.[NF Saida Preco])
		,[Cupom] = SUM(todos.[Cupom])
		,[Cupom_Valor] = Sum(todos.[Cupom Preco])
	 from (
select Data =b.data ,
	   [NF Entrada] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (Eitem.PLU = @plu or  (LEN(@REF)>0 AND eitem.codigo_referencia=@Ref)  )AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF Compra Preco]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (Eitem.PLU = @plu or (LEN(@REF)>0 AND eitem.codigo_referencia=@Ref))AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1   ),0) 				
	   ,OUTRAS_MOVIMENTACOES =0
		   
	   ,[NF Saida] =isnull((Select SUM (isnull(sitem.Qtde,0)*ISNULL(sitem.Embalagem,0)) 
								from NF_Item Sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							
							WHERE (Sitem.PLU = @plu or (LEN(@REF)>0 AND Sitem.codigo_referencia=@Ref ) ) AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 ),0) 
	   ,[NF Saida Preco]	= isnull((Select Sum(isnull(sitem.Total,0)) 
							from NF_Item sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							WHERE (Sitem.PLU = @plu or (LEN(@REF)>0 AND Sitem.codigo_referencia=@Ref) )AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and Sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1   ),0) 				
	  ,[Cupom] = 0.00-- isnull((Select SUM(isnull(se.Qtde,0)) from Saida_estoque se with( index(IX_Saida_Estoque_01)) where se.plu =@plu and se.Data_movimento = b.data),0) 
	  ,[Cupom Preco]=0.00	
		
from NF_Item a inner join nf b on a.codigo = b.Codigo 
				inner join Natureza_operacao op on b.Codigo_operacao = op.Codigo_operacao 
		
WHERE a.Filial=@FILIAL and A.PLU = @plu AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
group by Data 

UNION ALL

Select
	DATA   ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	OUTRAS_MOVIMENTACOES =SUM(OUTRAS_MOVIMENTACOES) ,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by DATA

UNION ALL
Select INV.Data,
		NF_ENTRADA = 0,
		NF_ENTRADA_PRECO = 0 ,
		OUTRAS_MOVIMENTACOES =  SUM(case when tm.Saida =0 then ISNULL(item.Contada,0) 
													 when tm.saida=1 then (ISNULL(item.contada,0)*-1) 
													 when tm.saida=2 then (ISNULL(item.Contada,0)-ISNULL(item.Saldo_atual,0))
													end 
													   )  ,
		NF_Saida= 0,
		NF_Saida_Preco= 0	 ,
		Cupom = 0,  
		Cupom_Preco= 0 								   
	from Inventario inv 
			inner join Tipo_movimentacao tm on inv.tipoMovimentacao = tm.Movimentacao
			inner join Inventario_itens item on inv.Codigo_inventario = item.Codigo_inventario and inv.Filial = item.Filial
	where item.PLU =@plu and inv.Data between @DataInicio AND @DataFim   and status='ENCERRADO' AND INV.Filial = @FILIAL
	group by inv.Data
	
) as todos
group by todos.Data
order by convert(varchar ,todos.Data,102) desc


  go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_APURACAO_ATENDENTE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_APURACAO_ATENDENTE]
GO

--PROCEDURES =======================================================================================
create PROCEDURE [dbo].[SP_REL_APURACAO_ATENDENTE]
				@Filial      As Varchar(20),
                @DataDe      As Varchar(8),
                @DataAte     As Varchar(8),
                @Vendedor     As Varchar(40)                
AS
begin
-- exec SP_REL_APURACAO_ATENDENTE 'MATRIZ','20190601','20190630'
Declare crFinalizadora cursor
for  Select Distinct  m.Descricao_departamento From comanda_item i Inner Join mercadoria m on i.plu = m.plu Where i.usuario = 'AILDO' and cupom > 0 and pdv > 0
And data_cancelamento is null
And m.Descricao_departamento in ('PRATOS RAPIDOS','PRATO DO DIA','SUCOS','SOBREMESA','PORCOES','SANDUICHES','CAIPIRINHA')
Group by m.Descricao_departamento 
--SELECT Distinct Top 5  Descricao_Departamento FROM Departamento where Codigo_Departamento in(select distinct d.Codigo_Departamento
--From comanda_item i Inner join Mercadoria m on m.plu = i.plu 
--Inner join Departamento d on d.Codigo_departamento = m.Codigo_departamento  
--Where cupom > 0 and pdv > 0 and data >= @DataDe and data <= @DataAte 
--and data_cancelamento is null) order by Descricao_Departamento
declare @SqlString Nvarchar(max)
SET @SqlString = ''
DECLARE @FINALIZA VARCHAR(50)

	open crFinalizadora
	FETCH NEXT FROM crFinalizadora INTO @FINALIZA;
	WHILE @@FETCH_STATUS = 0
	   BEGIN
		
			IF(@Vendedor='TODOS')
			Begin
				SET  @SqlString = @SqlString + ',[' + @FINALIZA +'] =Isnull((select ISNULL(SUM(ci.qtde),0)
							From comanda_item ci Inner join Mercadoria m on m.plu = ci.plu 
								Inner join Departamento d on d.Codigo_departamento = m.Codigo_departamento  
								Where ci.cupom > 0 and ci.pdv > 0 and ci.data_cancelamento is null 
								And m.Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data >='+ CHAR(39) +@DataDe+ CHAR(39) + ' And data <='+ CHAR(39) +@DataAte+ CHAR(39) + ' And ci.usuario = i.usuario And d.Descricao_Departamento ='+CHAR(39) + @FINALIZA+ CHAR(39) + 'Group by ci.usuario, d.Descricao_departamento),0) '
			End
			IF(@Vendedor<>'TODOS') 
			bEGIN
				SET  @SqlString = @SqlString + ',[' + @FINALIZA +'] =Isnull((select ISNULL(SUM(ci.qtde),0)
							From comanda_item ci Inner join Mercadoria m on m.plu = ci.plu 
								Inner join Departamento d on d.Codigo_departamento = m.Codigo_departamento  
								Where ci.cupom > 0 and ci.pdv > 0 and ci.data_cancelamento is null 
								And ci.Usuario ='+ CHAR(39) +@Vendedor+ CHAR(39) + ' And m.Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data >='+ CHAR(39) +@DataDe+ CHAR(39) + ' And data <='+ CHAR(39) +@DataAte+ CHAR(39) + ' And ci.usuario = i.usuario And d.Descricao_Departamento ='+CHAR(39) + @FINALIZA+ CHAR(39) + 'Group by ci.usuario, d.Descricao_departamento),0) '			
			End
		  --SET @SqlString = @SqlString + CONVERT(VARCHAR(10),@TRIB)
			 
		  FETCH  NEXT FROM crFinalizadora INTO @FINALIZA;
	      
	   END;

	CLOSE crFinalizadora;
	DEALLOCATE crFinalizadora;
	IF(@Vendedor='TODOS')
	Begin
		SET @SqlString = 'Select Nome = i.usuario '+@SqlString +'  From comanda_item i Inner join Mercadoria m on m.plu = i.plu 
							Inner join Departamento d on d.Codigo_departamento = m.Codigo_departamento  
								Where cupom > 0 and pdv > 0 and data_cancelamento is null
							' + ' and m.Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) +
							' Group by i.usuario ' ; 
	End
	IF(@Vendedor<>'TODOS')
		Begin
		SET @SqlString = 'Select Nome = i.usuario '+@SqlString +'  From comanda_item i Inner join Mercadoria m on m.plu = i.plu 
							Inner join Departamento d on d.Codigo_departamento = m.Codigo_departamento  
								Where cupom > 0 and pdv > 0 and data_cancelamento is null And i.usuario = '+ CHAR(39) +@Vendedor+ CHAR(39) + '
							' + ' and m.Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) +
							' Group by i.usuario ' ; 								
		End
  --print(@sqlString);
 EXECUTE (@SqlString);


END
  go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Outras_Movimentacoes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
GO


CREATE  PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@plu				AS Varchar(20),
	@ean				as varchar(20),
	@ref				as Varchar(20),
	@descricao			as Varchar(40),
	@Movimentacao	As Varchar(30) = '',
	@tipo			 as varchar(20)
AS
	Declare @String	As nVarchar(1024)
if @tipo = 'PRODUTO'
BEGIN		
		SET @String = ' SELECT'
		SET @String = @String + ' i.Codigo_inventario As Codigo,'
		SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
		SET @String = @String + ' i.tipoMovimentacao,'
		SET @String = @String + ' i.Usuario,'
		SET @String = @String +   CHAR(39) +'PLU' +CHAR(39) +'+ ii.plu as PLU,'
		SET @String = @String + ' m.descricao,'
		SET @String = @String + ' ii.Contada AS Qtde,'
		SET @String = @String + ' ii.custo as Vlr_Unit,'
		SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
		SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
		SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
		SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
		SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		BEGIN
			IF @Movimentacao<> 'TODOS' 
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

	-- PRINT @STRING
	 EXECUTE(@String)

END
ELSE
	BEGIN
			
		SET @String = ' SELECT'
		SET @String = @String + ' i.Codigo_inventario As Codigo,'
		SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
		SET @String = @String + ' i.tipoMovimentacao,'
		SET @String = @String + ' i.Usuario,'
		SET @String = @String + ' Total_Item = SUM(CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada )))'
		SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
		SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
		SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
		SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		BEGIN
			IF @Movimentacao<> 'TODOS' 
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
		SET @String = @String + ' GROUP BY i.Codigo_inventario,i.Data,i.tipoMovimentacao,i.Usuario ORDER BY 1'

	-- PRINT @STRING
	 EXECUTE(@String)	
	END

go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_estoque]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_produtos_estoque]
GO


CREATE   procedure [dbo].[sp_rel_produtos_estoque]
@filial varchar(20) ,
@plu varchar(20)
,@ean varchar(20)
,@ref varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@saldo varchar(14)
,@LINHA VARCHAR(30)
as
Declare @String	As Varchar(MAX)
Declare @Fantasia as varchar(50)

--'Depois tem que tirar esse parametro direto 
Set @Fantasia = (select Max(Fantasia)  from filial)

begin

    SET @String = ' Select distinct a.PLU,'
    --begin 
    --'Depois tem que tirar esse parametro direto 
    --if(@Fantasia='CHAMANA')
		SET @String = @String + 'Ref = a.Ref_Fornecedor,'    
	--end	

    SET @String = @String + 'a.Descricao,'    
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    -- SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
   -- begin 
    
    --'Depois tem que tirar esse parametro direto 
   -- if(@Fantasia <> 'CHAMANA')
	--	SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
	-- End		
    SET @String = @String + 'isnull(l.Preco_Custo,0) AS [PRECO CUSTO],'
    SET @String = @String + 'isnull(l.Preco,0) AS [PRECO VENDA],'    
    SET @String = @String + 'isnull(l.saldo_atual,0) AS [SALDO ATUAL],'
	SET @String = @String + 'isnull(a.estoque_minimo,0) AS [ESTOQUE MINIMO],'

    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco_Custo,0)))[VALOR ESTOQUE CUSTO],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco,0)))[VALOR ESTOQUE VENDA]'    
    SET @String = @String + ' from '
    SET @String = @String + ' Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b '
    SET @String = @String + ' on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial) '
    SET @String = @String + ' inner join mercadoria_loja l on a.plu=l.PLU '
    SET @String = @String + ' left join Familia c on  a.Codigo_familia =c.Codigo_familia '
    SET @String = @String + ' left join EAN on a.plu = ean.plu '
    SET @String = @String + ' left join LINHA on a.Cod_Linha = LINHA.codigo_linha'
    
    SET @String = @String + ' where a.Inativo <>1 '
    begin 
    if(len(@plu)>0)
		SET @String = @String + ' AND a.PLU= ' + CHAR(39) + @plu + CHAR(39)
	end	
	begin
		if(LEN(@descricao)>0) 
			SET @String = @String + ' and a.Descricao like '+ CHAR(39) +'%'+@descricao+'%'+ CHAR(39) 
	end
	begin 
		if(LEN(@grupo)>0)
		SET @String = @String + ' and (b.Descricao_grupo = '+ CHAR(39) +@grupo+ CHAR(39) +')'
	end	
	begin
		if(LEN(@subGrupo)>0)
		SET @String = @String + ' and (b.descricao_subgrupo = '+CHAR(39)+@subGrupo+CHAR(39)+')'
	end
	begin 
		if(LEN(@departamento)>0)
			SET @String = @String + ' and (b.descricao_departamento = '+CHAR(39)+ @departamento+CHAR(39)+')'
	end
	begin
		if(LEN(@familia)>0)
			SET @String = @String + ' and (a.Descricao_familia = '+CHAR(39)+@familia+CHAR(39)+')' 
	end

		SET @String = @String + ' and l.Filial = '+char(39)+@filial+CHAR(39)
	begin
		if(LEN(@ean)>0)
		SET @String = @String + ' and (ean.EAN ='+char(39)+@ean+char(39)+')'
	end
	begin 
		if(LEN(@ean)>0)
		 SET @String = @String + ' 	and (a.Ref_fornecedor = '+char(39)+@ref+CHAR(39)+')'
	end
	begin
	if(@saldo='Igual a Zero')
		 SET @String = @String + ' 	and (l.saldo_atual=0)'
	end
	begin
	if(@saldo='Menor que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual<0)'
	end
	begin
	if(@saldo='Maior que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual>0)'
	end

	if(@saldo='Estoque minimo')
	begin
		 SET @String = @String + ' 	and (l.saldo_atual<=a.estoque_minimo) AND ISNULL(A.ESTOQUE_MINIMO,0)>0'
	end


	if(LEN(@LINHA)>0 )
	BEGIN
		SET @String = @String + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
	END
	
	--PRINT @STRING	
	 EXECUTE(@String)	
  end




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cotacao_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('EAN'))
begin
	alter table cotacao_item alter column EAN varchar(20)
end
else
begin
	alter table cotacao_item add EAN varchar(20)
end 


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_VENDA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_COMANDA_VENDA]
GO


CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_VENDA] (
		@filial varchar(30),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		@usuario varchar(20),
		@plu varchar(20),
		@descricao varchar(50)
		)
		 AS


Declare @totalVenda Numeric(18,3)
	 
--set @dataInicio ='20170101';
--set @datafim = '20180122'

-- set @horaInicio='00:00:59' 
--set @horafim='23:59' 
		
-- drop table #tempCom
if(@usuario = 'TODOS')
BEGIN 
	select  
		   i.Usuario,
		   Qtde = sum(i.Qtde),
		   Total = Sum(i.Total)
		   into #tempCom
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
			
	where ISNULL(i.cupom,0) <>0
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and i.data_cancelamento IS NULL 
		  and (len(@plu)=0 or i.plu=@plu)
	group by i.usuario   
	order by    Sum(i.Total) desc
	  
	 
	 Select @totalVenda = sum(total) from #tempCom
	 
	 
	 Select Usuario, Qtde, Total, [%] = convert(numeric(12,2), (total/@totalVenda)*100) from #tempCom
	 
END	  
ELSE
BEGIN 
	select  
		Data=convert(varchar,i.data,103),
		Hora = convert(varchar,i.hora_evento,108),
		PLU = 'PLU'+i.Plu,
	    m.Descricao,
	    I.Qtde,
	    i.Unitario,
	    Total = I.Total
	into #tempCom1
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
		  inner join mercadoria m on i.plu=m.PLU
	where ISNULL(i.cupom,0) <>0
		  AND I.USUARIO = @USUARIO	
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and (len(@plu)=0 or i.plu=@plu)
		  and i.data_cancelamento IS NULL 
	order by i.total desc

	Select @totalVenda = sum(total) from #tempCom1
	 
	Select Plu,Descricao,Qtde=Sum(Qtde),Total=Sum(Total),[%] = convert(numeric(12,2), (sum(total)/@totalVenda)*100)  
	from #tempCom1
	group by plu,descricao
	order by sum(total) desc

END	  
	  


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_HISTORICO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO]
GO


CREATE PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO] (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(5),
		@finalizado varchar(5),
		@usuarioCancelamento varchar(20)
		)
		 AS
		 /* 
set @filial='MATRIZ'
set @comanda='1414'
set @dataInicio='20150901' 
set @datafim = '20160122'

set @horaInicio='00:00' 
set @horafim='23:59' 
-- set @status =''
set @usuario ='TODOS'
set @cancelamento='TODOS'
set @finalizado ='TODOS'
		
	*/
--sp_help comanda_item
--select * from Comanda_Item where data between '20150901' and '20160122'
select  i.Comanda, 
	   Data=convert(varchar,i.data,103),
	   Hora = convert(varchar,i.hora_evento,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.Usuario,
	   i.Plu,
	   m.Descricao,
	   i.Qtde,
	   i.Unitario,
	   i.Total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	   ,[Usuario Canc]= i.Usuario_Cancelamento  
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and  (len(@comanda)=0 or CONVERT(VARCHAR(5),i.comanda)=@comanda)  
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
	  and (@usuario='TODOS' OR  i.usuario = @usuario ) 
	  and (@cancelamento='TODOS' OR  (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento ))
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And(@finalizado='TODOS' OR case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado)
	  and (@usuarioCancelamento = 'TODOS' OR i.Usuario_Cancelamento = @usuarioCancelamento)
order by i.hora_evento desc 
	  
	  	  	  





go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Produto_Fornecedor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Produto_Fornecedor]
GO


CREATE PROCEDURE [dbo].[sp_Rel_Produto_Fornecedor]

      @filial varchar(20),

      @Fornecedor varchar(40)

AS

 

SELECT DISTINCT

      m.PLU,

      m.Ref_fornecedor,

      EAN = ISNULL(e.ean,''),

      m.descricao,

      l.Preco_Custo,

      l.Preco

FROM

      Mercadoria m

      INNER JOIN Mercadoria_Loja l ON m.PLU = l.PLU

      LEFT OUTER JOIN EAN e ON m.PLU = e.PLU

      INNER JOIN NF_ITEM ni ON m.PLU = ni.PLU

      INNER JOIN Fornecedor f ON f.Fornecedor = ni.Cliente_Fornecedor

WHERE

      f.Fornecedor like '%'+@Fornecedor+'%'

      OR

      f.Razao_social like '%' + @Fornecedor + '%'

 

 

go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_ProdutosAlterados]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_ProdutosAlterados]
GO


CREATE Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20),
                @Descricao              As Varchar(60) ,
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40)
				,@custoMargem			as varchar(40)
				,@LINHA VARCHAR(30)
				,@saldo varchar(14)


AS

Declare @String               As Varchar(max)
Declare @Where               As Varchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , '
		SET @String = @String + 'mercadoria.PESO , '

		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin

		SET @String = @String + 'CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO,'
		end		
		if(@custoMargem='CUSTO-MARGEM-VENDA' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
			set @String = @String + ' mercadoria_loja.PRECO_CUSTO,MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
			SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		end
	
	
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu '
		SET @String = @String + ' left join LINHA on mercadoria.Cod_Linha = LINHA.codigo_linha '
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'
		Set @Where = ' WHERE  (Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)+')'
        
         IF (LEN(@DESCRICAO) > 0)
		BEGIN
            SET @Where = @Where + ' AND ( mercadoria.DESCRICAO LIKE '+ CHAR(39) +'%'+ @DESCRICAO + '%'+CHAR(39)+')'

        END      
        
        
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
		
			Set @Where = @Where +' And (convert(date,Mercadoria.Data_Alteracao) BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)+')'
		end
	       
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)+')'

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND (e.ean = '+ CHAR(39) + @ean + CHAR(39)+')'

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)+')'

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)+')'

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)+')'

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)+')'

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end


		Begin
		IF(@saldo='Igual a Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual=0)'
		End
		
		Begin
		IF(@saldo='Menor que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual<0)'
		End
		
		Begin
		IF(@saldo='Maior que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual>0)'
		End
		
		IF(LEN(@LINHA)>0 )
		Begin
			SET @Where = @Where + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
		End
	 

		--print(@String + @Where)
		EXECUTE(@String + @Where)

END

 





go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Cadastro_Produto]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_Cadastro_Produto]
GO


CREATE PROCEDURE [dbo].[sp_rel_Cadastro_Produto] 
@filial varchar(20),
@plu varchar(20),@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
as

/*
set @plu ='';
set @ean ='';
set @descricao ='';
set @grupo ='BEBIDAS';
set @subGrupo ='';
set @departamento ='';
set @familia ='';
*/
    select '|-PLU:-|'+a.plu + ' |-EAN:-|'+isnull(b.ean,'_____________')+
    ' || |- DESCRICAO: -| '+	rtrim(a.descricao)+
    ' || |- DESCRICAO RESUMIDA: -| '+isnull(a.descricao_resumida,'____________________')+
    ' || |- GRUPO: -| '+isnull(convert(varchar,c.codigo_grupo),'______')+' '+ c.descricao_grupo+' |-SUBGRUPO:-|'+c.codigo_subgrupo+' '+c.descricao_subgrupo+
    ' || |- DEPARTAMENTO: -|'+c.codigo_departamento +' '+c.descricao_departamento+
    ' || |- FAMILIA: -| '+isnull(replace(a.codigo_familia,' ','_'),'____')+' ' +isnull(replace(a.descricao_familia,' ','_'),'______________')+
    ' || |- LOCALIZACAO: -| '+isnull(replace(a.localizacao,' ','_'),'___________')+
    ' || |- DATA CADASTRO:-| '+isnull(convert(varchar,data_cadastro,103),'__________')+' |-DATA ALTERACAO:-|'+isnull(convert(varchar,data_alteracao,103),'__________')
    as '  ',
    ' || |- TIPO: -| '+isnull(a.tipo,'________')+
    ' || |- UND: -| '+isnull(a.und,'___')+
    ' || |- TECLA: -| '+isnull(convert(varchar,a.tecla),'___')+
    ' || |- VENDA FRACIONARIA: -| '+isnull(venda_fracionaria,'NAO')+
    ' || |- PESO VARIAVEL: -| '+isnull(peso_variavel,'___')+
    ' || |- CENTRO CUSTO: -| '+isnull(a.codigo_centro_custo,'____________')+
    ' || |- LINHA: -| '+isnull(convert(varchar,a.cod_linha),'____')+' '+isnull(d.descricao_linha,'___________')+
    ' || |- COR LINHA: -| '+isnull(convert(varchar,a.cod_cor_linha),'____')+' '+isnull(e.descricao_cor,  '___________')+  ' || || '
    as ' '
    from mercadoria a  WITH(INDEX(PK_Mercadoria)) left join ean b on a.plu =b.plu
    inner join W_BR_CADASTRO_DEPARTAMENTO c on (a.codigo_departamento= c.codigo_Departamento) -- and a.filial=c.filial)
    left join linha d on a.cod_linha= d.codigo_linha
    left join cor_linha e on a.cod_cor_linha = e.codigo_cor
    where (a.PLU=@plu or len(@plu)=0)
		and (b.EAN=@ean or LEN(@ean)=0)
		and (a.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (a.Descricao_familia = @familia or LEN(@familia)=0)
        


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]
GO


CREATE PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE](
--declare
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@GRUPO as Varchar(40), 
@SUBGRUPO as Varchar(40),
@DEPARTAMENTO AS VARCHAR(40),
@FAMILIA    AS VARCHAR(40),
@FORNECEDOR AS VARCHAR(50)
)
As


Select plu into #mercFornecedor from Fornecedor_Mercadoria where fornecedor = @FORNECEDOR

DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

--set @DataInicio ='20150302'
--set @DataFim ='20150330'
--Set @GRUPO ='PADARIA'
--set @SUBGRUPO ='PAES'
--set @DEPARTAMENTO='PAES'
--set @FAMILIA =''
--drop table #OUTROS

--exec SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE @Filial='MATRIZ', @dataInicio = '20190301',  @dataFim = '20190330',  @grupo = '',  @subGrupo = '',  @departamento = '',  @Familia = '',  @fornecedor = 'REJAN' 


Create table #OUTROS
(
	PLU  VARCHAR(17) COLLATE SQL_Latin1_General_CP1_CI_AS,
	Ref_Fornecedor varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS,
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
	OUTRAS_MOVIMENTACOES DECIMAL(18,2),
	NF_Saida DECIMAL(18,2),
	NF_Saida_Preco	 DECIMAL(18,2),
	Cupom DECIMAL(18,2),  
	Cupom_Preco DECIMAL(18,2)
);

--select * from pedido_itens

IF @PedEstoque = 'TRUE'
INSERT INTO  #OUTROS select PLU =itemPd.PLU  ,Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
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
		and (len(@fornecedor)=0 or m.plu in (Select plu from #mercFornecedor))
		AND  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
		and pd.Filial = @FILIAL
	group by itemPd.PLU , m.Ref_Fornecedor
ELSE
begin
INSERT INTO  #OUTROS 
	select PLU =se.PLU  , Ref_Fornecedor = Isnull(M.Ref_fornecedor,''), 
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
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
		and (len(@fornecedor)=0 or m.plu in (Select plu from #mercFornecedor))
		and  se.Data_movimento between @DataInicio AND @DataFim
		and se.Filial = @FILIAL
	group by se.PLU , m.Ref_Fornecedor
END;



Select PLU =todos.PLU ,Ref_fornecedor= isnull(m.Ref_Fornecedor,'')
		,DESCRICAO = M.DESCRICAO
		,[Entrada]=Sum(todos.NF_ENTRADA)
		,[Entrada_Valor]=Sum(todos.NF_ENTRADA_PRECO)
		,[Outras_Mov]=SUM(OUTRAS_MOVIMENTACOES)
		,[Saida_Outros] = SUM(todos.NF_Saida)
		,[Saida_Valor] = Sum(todos.NF_Saida_Preco)
		,[Cupom] = SUM(todos.Cupom)
		,[Cupom_Valor] = Sum(todos.Cupom_Preco)
	 from (

select PLU =isnull(A.PLU,''), Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
	   [NF_ENTRADA] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (LEN(@FORNECEDOR)=0  OR Enf.Cliente_Fornecedor =@FORNECEDOR ) AND Eitem.PLU = A.PLU and (Enf.Data between @DataInicio AND @DataFim ) 
							    AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF_ENTRADA_PRECO]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							where (LEN(@FORNECEDOR)=0  OR Enf.Cliente_Fornecedor =@FORNECEDOR ) AND  Eitem.PLU = A.PLU  and (Enf.Data between @DataInicio AND @DataFim ) 
									 AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1   ),0) 				
	   ,[OUTRAS_MOVIMENTACOES]=0
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
								AND (LEN(@FORNECEDOR)=0  OR b.Cliente_Fornecedor =@FORNECEDOR ) 
								AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 
								and a.Filial=@FILIAL
					GROUP BY A.PLU , m.Ref_Fornecedor


UNION ALL
Select
	PLU   , Ref_Fornecedor ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	[OUTRAS_MOVIMENTACOES]=0,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by PLU ,Ref_Fornecedor 
 UNION ALL
Select ITEM.PLU, Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
		NF_ENTRADA = 0,
		NF_ENTRADA_PRECO = 0 ,
		OUTRAS_MOVIMENTACOES =  SUM(case when tm.Saida =0 then ISNULL(item.Contada,0) 
													 when tm.saida=1 then (ISNULL(item.contada,0)*-1) 
													 when tm.saida=2 then (ISNULL(item.Contada,0)-ISNULL(item.Saldo_atual,0))
													end 
													   )  ,
		NF_Saida= 0,
		NF_Saida_Preco= 0	 ,
		Cupom = 0,  
		Cupom_Preco= 0 								   
	from Inventario inv 
			inner join Tipo_movimentacao tm on inv.tipoMovimentacao = tm.Movimentacao
			inner join Inventario_itens item on inv.Codigo_inventario = item.Codigo_inventario and inv.Filial = item.Filial
			INNER JOIN Mercadoria M ON M.PLU =ITEM.PLU
							INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
							
	WHERE (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
								AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
								AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
								AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
								and inv.Data between @DataInicio AND @DataFim   and status='ENCERRADO' AND INV.Filial = @FILIAL
								and (len(@fornecedor)=0 or item.plu in (Select plu from #mercFornecedor))
	group by Item.PLU , m.Ref_Fornecedor




) as todos
INNER JOIN Mercadoria M ON M.PLU =todos.PLU
group by todos.PLU, m.Ref_Fornecedor, 
M.Descricao
order by CONVERT(DECIMAL,todos.PLU)  
go 

insert into Versoes_Atualizadas select 'Versão:1.248.787', getdate();