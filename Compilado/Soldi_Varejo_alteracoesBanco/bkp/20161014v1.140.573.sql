
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('nao_imprime'))
begin
	alter table pedido_itens alter column nao_imprime tinyint  
end
else
begin
		alter table pedido_itens add nao_imprime tinyint default 0
end 

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].sp_Cons_Cadastro_Mercadoria') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Cons_Cadastro_Mercadoria
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE sp_Cons_Cadastro_Mercadoria
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(max)
    Declare @StringSQL2 As nVarChar(max)
    Declare @Where        As nVarChar(max)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	DELETE FROM BUSCA_PRECO;
	insert into Busca_Preco 
	SELECT DISTINCT  PLU,Descricao_resumida,PRECO = CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END  
	FROM (

	SELECT [plu] = CONVERT(BIGINT, MERCADORIA.PLU), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim  
		FROM Mercadoria INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU  
		WHERE ISNULL(Inativo, 0) <= 0    AND MERCADORIA_LOJA.Filial = @Filial

	UNION ALL 


	SELECT [plu] = CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim FROM Mercadoria  INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU    INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU 
	 WHERE ISNULL(Inativo, 0) <= 0   AND MERCADORIA_LOJA.Filial = @Filial
	   
	 GROUP BY CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim 
	 HAVING CONVERT(BIGINT, EAN.EAN) <=99999999999999 
	) A
	GROUP BY PLU,Descricao_resumida,CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END ;


    
    
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Peso_Variavel.Codigo,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
        + '   ,und '+
        + '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
        + '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
        + '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' WHERE ISNULL(Inativo, 0) <= 0  '

    IF @TipoCadastro < 100
    BEGIN
            SET @Where = ' '
        END
    ELSE
        BEGIN
            SET @Where = ' AND Peso_Variavel.Codigo > 0 '
        END
    IF @Alterados =1
        begin
            SET @Where = @Where+ ' AND estado_mercadoria=1'
        end

    SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)

    -- Adicionar o EAN
    IF @TipoCadastro < 100
        BEGIN
            SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
                + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV =  Peso_Variavel.Codigo ,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
				+ '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
				+ '   ,und '+
				+ '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
				+ '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
				+ '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
				+ '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' WHERE ISNULL(Inativo, 0) <= 0 '

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 



go 
IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CEP_Brasil_Delivery]') AND type in (N'U'))
begin

CREATE TABLE [dbo].[CEP_Brasil_Delivery](
	[Logradouro] [varchar](60) NOT NULL,
	[Bairro] [varchar](50) NOT NULL,
	[Cidade] [varchar](50) NOT NULL,
	[UF] [varchar](2) NOT NULL,
	[CEP] [varchar](8) NOT NULL,
	num_inicio int,
	num_fim int
)
end


go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_extrato_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_extrato_a_pagar]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_extrato_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@tipo  varchar(20)

)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where +' vencimento between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And Conta_a_pagar.fornecedor = ' + char(39) +@fornecedor +char(39) 
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	
	
	set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
											 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
											 WHEN @STATUS='CANCELADO' THEN ' status =3'
											 WHEN @STATUS='LANCADO' THEN ' status =4'
										ELSE 'status <> 3' END
																) 
	
	
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			
			convert(varchar ,vencimento,103) as  Vencimento, 
			Dia_da_Semana = case 
									when datepart(dw, vencimento) = 2 then '+ char(39) +'Segunda'+ char(39) +'
									when datepart(dw, vencimento) = 3 then '+ char(39) +'Terça'+ char(39) +'
									when datepart(dw, vencimento) = 5 then '+ char(39) +'Quinta'+ char(39) +'
									when datepart(dw, vencimento) = 6 then '+ char(39) +'Sexta'+ char(39) +'
									when datepart(dw, vencimento) = 7 then '+ char(39) +'Sabado'+ char(39) +'
									Else '+ char(39) +'Domingo'+ char(39) +' end,
			Documento = rtrim(ltrim(documento)), 
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			
			[Valor] = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0)
		
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc and conta_a_pagar.filial=Conta_corrente.filial  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
			left outer join Fornecedor on Conta_a_pagar.Fornecedor = Fornecedor.Fornecedor
		'+@where+'  Order By convert(varchar ,vencimento,102) '
		
	
	--Print @string
	Exec(@string)
End


GO
/****** Object:  StoredProcedure [dbo].[sp_rel_balanceteFinanceiro]    Script Date: 09/27/2016 12:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--PROCEDURES =======================================================================================
-- sp_rel_balancetefinanceiro 'matriz', '20160801', '20160830', 'emissao', 'previsto', 'sintetico', 'todos'
ALTER  procedure [dbo].[sp_rel_balanceteFinanceiro](
      @filial           varchar(20),
      @datade           varchar(8),
      @dataate    varchar(8),
      @tipo       varchar(50),
      @status         varchar(10),
      @visualizar varchar(50),
      @Modalidade varchar(8)
)As

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


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria_loja') 
            AND  UPPER(COLUMN_NAME) = UPPER('qtde_atacado'))
begin
	alter table mercadoria_loja alter column qtde_atacado numeric (9,3);
end
else
begin
		alter table mercadoria_loja add qtde_atacado numeric (9,3);
end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria_loja') 
            AND  UPPER(COLUMN_NAME) = UPPER('margem_atacado'))
begin
	alter table mercadoria_loja alter column margem_atacado numeric (12,2);
end
else
begin
		alter table mercadoria_loja add margem_atacado numeric (12,2);
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria_loja') 
            AND  UPPER(COLUMN_NAME) = UPPER('preco_atacado'))
begin
	alter table mercadoria_loja alter column preco_atacado numeric (12,2);
end
else
begin
		alter table mercadoria_loja add preco_atacado numeric (12,2);
end 
go



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('qtde_atacado'))
begin
	alter table mercadoria alter column qtde_atacado numeric (9,3);
end
else
begin
		alter table mercadoria add qtde_atacado numeric (9,3);
end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('margem_atacado'))
begin
	alter table mercadoria alter column margem_atacado numeric (12,2);
end
else
begin
		alter table mercadoria add margem_atacado numeric (12,2);
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('preco_atacado'))
begin
	alter table mercadoria alter column preco_atacado numeric (12,2);
end
else
begin
		alter table mercadoria add preco_atacado numeric (12,2);
end 
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Cons_Cadastro_Mercadoria]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(max)
    Declare @StringSQL2 As nVarChar(max)
    Declare @Where        As nVarChar(max)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	DELETE FROM BUSCA_PRECO;
	insert into Busca_Preco 
	SELECT DISTINCT  PLU,Descricao_resumida,PRECO = CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END  
	FROM (

	SELECT [plu] = CONVERT(BIGINT, MERCADORIA.PLU), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim  
		FROM Mercadoria INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU  
		WHERE ISNULL(Inativo, 0) <= 0    AND MERCADORIA_LOJA.Filial = @Filial

	UNION ALL 


	SELECT [plu] = CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim FROM Mercadoria  INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU    INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU 
	 WHERE ISNULL(Inativo, 0) <= 0   AND MERCADORIA_LOJA.Filial = @Filial
	   
	 GROUP BY CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim 
	 HAVING CONVERT(BIGINT, EAN.EAN) <=99999999999999 
	) A
	GROUP BY PLU,Descricao_resumida,CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END ;


    
    
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
        + '   ,und '+
        + '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
        + '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
        + '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
        + '   ,mercadoria_loja.preco_atacado '
        + '   ,mercadoria_loja.qtde_atacado'
        + '   ,mercadoria.embalagem '
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' WHERE ISNULL(Inativo, 0) <= 0  '

    IF @TipoCadastro < 100
    BEGIN
            SET @Where = ' '
        END
    ELSE
        BEGIN
            SET @Where = ' AND Peso_Variavel.Codigo > 0 '
        END
    IF @Alterados =1
        begin
            SET @Where = @Where+ ' AND estado_mercadoria=1'
        end

    SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)

    -- Adicionar o EAN
    IF @TipoCadastro < 100
        BEGIN
            SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
                + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
				+ '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
				+ '   ,und '+
				+ '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
				+ '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
				+ '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
				+ '   ,mercadoria_loja.preco_atacado '
				+ '   ,mercadoria_loja.qtde_atacado'
				+ '   ,mercadoria.embalagem '
				+ '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' WHERE ISNULL(Inativo, 0) <= 0 '

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 




go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NFe_Det]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_NFe_Det]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_NFe_Det]
	@id varchar(47)
	as

	select ID,
		ide_nNF,
		emit_cnpj,
		ide_dEmi,
		det_prod_cProd,
		det_prod_cEAN,
		det_prod_xProd,
		det_prod_NCM,
		det_prod_EXTIPI,
		det_prod_genero,
		det_prod_CFOP,
		det_prod_uCOM,
		det_prod_qCOM,
		det_prod_vUnCOM,
		det_prod_vProd,
		det_prod_cEANTrib,
		det_prod_uTrib,
		det_prod_qTrib,
		det_prod_vUnTrib,
		det_prod_vFrete,
		det_prod_vSeg,
		det_prod_vDesc,
		det_prod_DI,
		det_prod_DetEspecifico,
		det_icms_orig,
		det_icms_CST,
		det_icms_modBC,
		det_icms_pRedBC,
		det_icms_vBC,
		det_icms_pICMS,
		det_icms_vICMS,
		det_icms_modBCST,
		det_icms_pMVAST,
		det_icms_pRedBCST,
		det_icms_vBCST,
		det_icms_pICMSST,
		det_icms_vICMSST,
		det_ipi_clEnq,
		det_ipi_CNPJProd,
		det_ipi_cSelo,
		det_ipi_qSelo,
		det_ipi_cEnq,
		det_ipi_CST,
		det_ipi_vBC,
		det_ipi_pIPI,
		det_ipi_vIPI,
		det_ipi_qUnid,
		det_ipi_vUnid,
		det_II_vBC,
		det_II_vDespAdu,
		det_II_vII,
		det_II_vIOF,
		det_pis_CST,
		det_pis_vBC,
		det_pis_pPIS,
		det_pis_vPIS,
		det_pis_qBCProd,
		det_pis_vAliqProd,
		det_pisst_vBC,
		det_pisst_pPIS,
		det_pisst_vPIS,
		det_pisst_qBCProd,
		det_pisst_vAliqProd,
		det_cofins_CST,
		det_cofins_vBC,
		det_cofins_pCOFINS,
		det_cofins_vCOFINS,
		det_cofins_qBCProd,
		det_cofins_vAliqProd,
		det_cofinsst_vBC,
		det_cofinsst_pCOFINS,
		det_cofinsst_vCOFINS,
		det_cofinsst_qBCProd,
		det_cofinsst_vAliqProd,
		det_issqn2g_vBC,
		det_issqn2g_vAliq,
		det_issqn2g_vISSQN,
		det_issqn2g_cMunFG,
		det_issqn2g_cListServ,
		det_issqn2g_cSitTrib,
		det_icms_CSOSN,
		det_icms_pCredSN,
		det_icms_vCredICMSSN,
		det_nItem,
		det_prod_vOutro,
		det_prod_indTot,
		det_prod_CEST 
	from 
		nfe_xml 
	where 
		id=@id 
		and det_prod_cprod is not null
	Order By
		det_nItem

go 

ALTER TABLE AGENDA ALTER COLUMN obs_veterinario varchar(30);
go
ALTER TABLE AGENDA ALTER COLUMN obs_veterinario text

go



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('agenda') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuario_cadastro'))
begin
	alter table agenda alter column usuario_cadastro varchar(40);
end
else
begin
		alter table agenda add usuario_cadastro varchar(40);
end 

go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('agenda') 
            AND  UPPER(COLUMN_NAME) = UPPER('sigla'))
begin
	alter table agenda alter column sigla text;
end
else
begin
		alter table agenda add sigla text;
end 

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0400]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_EFD_PisCofins_0400]
end
GO
--PROCEDURES =======================================================================================
CREATE        PROC [dbo].[sp_EFD_PisCofins_0400]
	@FILIAL VARCHAR(20),
	@DATAINI VARCHAR(8),
	@DATAFIM VARCHAR (8)
as

--SP_EFD_PISCOFINS_0400 'MATRIZ', '20141001', '20141031'
SELECT DISTINCT '|0400|' + convert(varchar (4),CODIGO_OPERACAO) + '|' + rtrim(ltrim(descricao)) + '|'  
FROM NATUREZA_OPERACAO
WHERE CODIGO_OPERACAO IN (

SELECT DISTINCT 
	a.CODIGO_OPERACAO
	FROM
		NF a INNER JOIN NF_Item b ON 
					a.FILIAL = b.FILIAL 
					AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR 
					AND a.Codigo = b.Codigo		
		INNER JOIN Mercadoria M ON b.PLU = m.PLU
		LEFT OUTER JOIN Tributacao c ON c.Codigo_Tributacao = b.Codigo_Tributacao AND c.Filial = b.Filial
		

	WHERE
		a.Filial = @FILIAL
		AND a.Codigo_Operacao not in (5929, 6929)
		AND a.Data BETWEEN @DATAINI AND @DATAFIM)
GO





IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Vendedor]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[sp_Rel_Vendedor]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Vendedor](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Vendedor	  as varchar(40),
	@tipo		  as Varchar(20)
	)
as
begin 
-- exec sp_rel_Vendedor 'MATRIZ', '20160421', '20160421','TODOS','ANALITICO'


IF(@Vendedor='TODOS')
BEGIN
	if(@tipo='SINTETICO')
	BEGIN
		SELECT  Funcionario.Codigo,
				Funcionario.Nome,
				Funcionario.Funcao,
				CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],    
				sum(se.vlr - se.Desconto) as Total,
				(sum(se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100 as [Vlr Comissao]
		FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
		where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
			-- and (@Vendedor='TODOS' or Funcionario.Nome = @Vendedor)
		GROUP BY Funcionario.codigo, Funcionario.Nome,Funcionario.Funcao,Funcionario.Comissao
	END
	ELSE
	BEGIN
		SELECT  Funcionario.Codigo,
				Funcionario.Nome,
				Funcionario.Funcao,
				'PLU'+SE.PLU AS PLU,
				M.DESCRICAO,
				CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],    
				(se.vlr - se.Desconto) as Total,
				((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100 as [Vlr Comissao]
		FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
				INNER JOIN Mercadoria AS M ON M.PLU=SE.PLU
		where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
	END
END
ELSE
BEGIN
	if(@tipo='SINTETICO')
		BEGIN
			SELECT  
					CONVERT(VARCHAR,SE.Data_movimento,103)AS DATA,
					
					CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],
					SUM(se.vlr - se.Desconto) as TOTAL,
					SUM(((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100) as [Vlr Comissao]
			FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
			
			where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
				and ( Funcionario.Nome = @Vendedor)
			GROUP BY SE.Data_movimento,Funcionario.Comissao
		END
		ELSE
		BEGIN 
			SELECT  
					'PLU'+SE.PLU AS PLU,
				M.DESCRICAO,
					CONVERT(VARCHAR,SE.Data_movimento,103)AS DATA,
					
					CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],
					(se.vlr - se.Desconto) as TOTAL,
					(((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100) as [Vlr Comissao]
			FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
				INNER JOIN Mercadoria AS M ON M.PLU=SE.PLU
			where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
				and ( Funcionario.Nome = @Vendedor)
			
		END
END
end


go





IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('centro_custo'))
begin
	alter table fornecedor alter column centro_custo varchar(30)
end
else
begin
		alter table fornecedor add centro_custo varchar(30);
end 



GO

/****** Object:  Index [PK_Cliente]    Script Date: 08/04/2016 15:24:46 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND name = N'PK_Cliente')
ALTER TABLE [dbo].[Cliente] DROP CONSTRAINT [PK_Cliente]
GO


alter table cliente alter column codigo_cliente varchar(11) not null;

GO

/****** Object:  Index [PK_Cliente]    Script Date: 08/04/2016 15:24:46 ******/
ALTER TABLE [dbo].[Cliente] ADD  CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Codigo_Cliente] ASC
	
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]


GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente_contato]') AND name = N'PK__Cliente___607F28E621504D50')
ALTER TABLE [dbo].[Cliente_contato] DROP CONSTRAINT [PK__Cliente___607F28E621504D50]
GO

alter table cliente_contato alter column codigo_cliente varchar(11) not null;
go



/****** Object:  Index [PK__Cliente___607F28E621504D50]    Script Date: 08/02/2016 14:56:59 ******/
ALTER TABLE [dbo].[Cliente_contato] ADD PRIMARY KEY CLUSTERED 
(
	[Codigo_Cliente] ASC,
	[Meio_comunicacao] ASC,
	[id_Meio_Comunicacao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('agenda') 
            AND  UPPER(COLUMN_NAME) = UPPER('funcionario_retira'))
begin
	alter table agenda alter column funcionario_retira varchar(20)
end
else
begin
		alter table agenda add funcionario_retira varchar(20)
end 

go

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
	@Caixa   	  As varchar(30),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40),
    @plu	      as Varchar(20),
    @descricao    as Varchar(50) 
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
		and (LEN(@plu)=0 or a.PLU=@plu)
		and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
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
				and (LEN(@plu)=0 or a.PLU=@plu)
				and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
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
		and (LEN(@plu)=0 or a.PLU=@plu)
		and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
end

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_cartao_Extrato]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_cartao_Extrato]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_fin_cartao_Extrato]
	(
	@filial     varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@cartao		varchar(50),
	@Visualizar varchar(20)
	)
	as
if(@Visualizar = 'ANALITICO')
begin
	Select Documento,
		Cartao=ca.id_cartao, 
		Pdv = 'SFT_'+ convert(varchar,cr.pdv),
		Emissao=convert(varchar,Emissao,103),
		Vencimento=CONVERT(varchar,vencimento,103),
		
		Total=((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0) 
		from Conta_a_receber cr
			inner join Cartao ca on
			(			cr.finalizadora = ca.nro_Finalizadora 
					and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
					and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
					)
	where CR.FILIAL =@filial AND
		(
			(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
			OR
			(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
		  )
		  AND (@cartao='TODOS' OR CA.ID_CARTAO = @cartao )
		  
	order by  CASE WHEN @tipo='EMISSAO' THEN CONVERT(varchar,Emissao,102)  ELSE CONVERT(varchar,vencimento,102) END , ca.id_cartao,cr.pdv
end
else
begin
Select 
		Cartao=ca.id_cartao, 
		Vencimento='SFT_'+CONVERT(varchar,vencimento,103),
		Total=Sum(((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0)) 
		from Conta_a_receber cr
			inner join Cartao ca on
			(			cr.finalizadora = ca.nro_Finalizadora 
					and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
					and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
					)
	where CR.FILIAL =@filial AND
		(
			(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
			OR
			(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
		  )
		  AND (@cartao='TODOS' OR CA.ID_CARTAO = @cartao )
	group by ca.id_cartao,Vencimento
	order by CONVERT(varchar,vencimento,102) ,ca.id_cartao 

end




go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_cartao]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_cartao]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_fin_cartao]
	(
	@filial     varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@cartao		varchar(50),
	@Visualizar varchar(20)
	)
	as
if(@Visualizar = 'ANALITICO')
begin
	Select Documento,
		Cartao=ca.id_cartao, 
		Pdv = 'SFT_'+ convert(varchar,cr.pdv),
		Emissao=convert(varchar,Emissao,103),
		Vencimento=CONVERT(varchar,vencimento,103),
		Valor,
		[Valor da Taxa]=cr.taxa,
		Total=((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0) 
		from Conta_a_receber cr
			inner join Cartao ca on
			(			cr.finalizadora = ca.nro_Finalizadora 
					and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
					and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
					)
	where CR.FILIAL =@filial AND
		(
			(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
			OR
			(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
		  )
		  AND (@cartao='TODOS' OR CA.ID_CARTAO = @cartao )
		  
	order by  CASE WHEN @tipo='EMISSAO' THEN CONVERT(varchar,Emissao,102)  ELSE CONVERT(varchar,vencimento,102) END , ca.id_cartao,cr.pdv
end
else
begin
Select 
		Cartao=ca.id_cartao, 
		ca.taxa as Taxa,
		'SFT_' +convert(varchar,ca.dias) as Prazo,
		Emissao ='SFT_'+CONVERT(varchar,Emissao,103),
		Vencimento='SFT_'+CONVERT(varchar,vencimento,103),
		
		Sum(Valor) as Valor,
		[Valor da Taxa]=Sum(cr.taxa),
		Total=Sum(((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0)) 
		from Conta_a_receber cr
			inner join Cartao ca on
			(			cr.finalizadora = ca.nro_Finalizadora 
					and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
					and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
					)
	where CR.FILIAL =@filial AND
		(
			(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
			OR
			(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
		  )
		  AND (@cartao='TODOS' OR CA.ID_CARTAO = @cartao )
	group by ca.id_cartao,ca.taxa,ca.dias, Emissao,Vencimento
	order by  CASE WHEN @tipo='EMISSAO' THEN CONVERT(varchar,Emissao,102)  ELSE CONVERT(varchar,vencimento,102) END ,ca.id_cartao 

end




go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('inativo'))
begin
	alter table cliente alter column inativo tinyint
end
else
begin
	alter table cliente add inativo tinyint
end 
go 

update mercadoria set curva_a=1, curva_b=1,curva_c=1
	
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Curva_ABC]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Curva_ABC]
end
GO
--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40)
AS
                
                Declare @String               As nVarchar(2000)
		Declare @Where               As nVarchar(1024)
BEGIN
	
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And  Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And  data_cancelamento is null and (isnull(mercadoria.curva_a,0)=1 and isnull(mercadoria.curva_b,0)=1 and isnull(mercadoria.curva_c,0) = 1) '
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
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                               if ISNULL(@nLinhas, 0) > 0 
                                               SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' ROW_NUMBER() over(order by Convert(Numeric(10,3), Sum(Qtde)) desc) as RANK, Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,' 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Total Venda] = Convert(Numeric(12,2), Sum(VLR-desconto)), '
                set @String = @String + ' [Lucro Bruto] = Convert(Numeric(12,2),SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))),'
		Set @String = @String + ' [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2), Sum(VLR-desconto)) / (select sum(convert(numeric(12,2), (vlr-desconto))) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--** Inserindo Clausula Subquery
		Set @String = @String + @Where + ' )* 100)) + '+ char(39)+ '%'+ char(39)
                SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque  WITH (INDEX(Ix_Fluxo_Caixa)) ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
	
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao  Order by ['+@Ordem+'] Desc'
 
 PRINT @STRING
                EXECUTE(@String)
END





go 


alter table natureza_operacao alter column descricao varchar(60);

go

UPDATE SEQUENCIAIS SET SEQUENCIA = (SELECT MAX(CONVERT(INT,CODIGO)) FROM Funcionario ) WHERE TABELA_COLUNA='FUNCIONARIO.CODIGO'

go

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__1F5986F6]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__1F5986F6]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__3039FA35]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__3039FA35]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__38EF3BC7]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__38EF3BC7]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__39C3646F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__39C3646F]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Vacin__3DB3F0E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet_Vacina]'))
ALTER TABLE [dbo].[Cliente_Pet_Vacina] DROP CONSTRAINT [FK__Cliente_P__Vacin__3DB3F0E4]
GO



IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__434CCEA9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__434CCEA9]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Vacin__6C4EE43C]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet_Vacina]'))
ALTER TABLE [dbo].[Cliente_Pet_Vacina] DROP CONSTRAINT [FK__Cliente_P__Vacin__6C4EE43C]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet_Vaci__3EA8151D]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet_Vacina]'))
ALTER TABLE [dbo].[Cliente_Pet_Vacina] DROP CONSTRAINT [FK__Cliente_Pet_Vaci__3EA8151D]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__38EF3BC7]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__38EF3BC7]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Pelag__678A2F1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Pelag__678A2F1F]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__2141CF68]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__2141CF68]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__3AD78439]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__3AD78439]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__3BABACE1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__3BABACE1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__4535171B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__4535171B]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__69727791]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__69727791]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__3AD78439]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__3AD78439]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_P__Porte__4535171B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_P__Porte__4535171B]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet_Vaci__6D430875]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet_Vacina]'))
ALTER TABLE [dbo].[Cliente_Pet_Vacina] DROP CONSTRAINT [FK__Cliente_Pet_Vaci__6D430875]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Agenda__52AF0DCA]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agenda]'))
ALTER TABLE [dbo].[Agenda] DROP CONSTRAINT [FK__Agenda__52AF0DCA]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Agenda__4CEB477C]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agenda]'))
ALTER TABLE [dbo].[Agenda] DROP CONSTRAINT [FK__Agenda__4CEB477C]
GO
GO

/****** Object:  Index [PK__Cliente_Pet__0D3AD6BB]    Script Date: 06/24/2016 14:12:44 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]') AND name = N'PK__Cliente_Pet__0D3AD6BB')
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [PK__Cliente_Pet__0D3AD6BB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__204DAB2F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__204DAB2F]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__312E1E6E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__312E1E6E]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__39E36000]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__39E36000]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__3AB788A8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__3AB788A8]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__4440F2E2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__4440F2E2]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pe__Sexo__687E5358]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pe__Sexo__687E5358]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__2235F3A1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__2235F3A1]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__331666E0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__331666E0]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__3BCBA872]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__3BCBA872]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__3C9FD11A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__3C9FD11A]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__6A669BCA]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__6A669BCA]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__1E6562BD]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__1E6562BD]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__2F45D5FC]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__2F45D5FC]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__37FB178E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__37FB178E]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__38CF4036]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__38CF4036]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__4258AA70]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__4258AA70]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__Cor__66960AE6]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__Cor__66960AE6]

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__46293B54]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__46293B54]
GO




IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__706493E0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__706493E0]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__743524C4]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__743524C4]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Cliente_Pet__7805B5A8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]'))
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [FK__Cliente_Pet__7805B5A8]
GO
go


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cliente_Pet') 
            AND  UPPER(COLUMN_NAME) = UPPER('codigo_pet'))
begin
	alter table cliente_pet alter column codigo_pet varchar(20)
end
else
begin
		alter table cliente_pet add codigo_pet varchar(20)
end 

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cliente_Pet_Vacina') 
            AND  UPPER(COLUMN_NAME) = UPPER('codigo_pet'))
begin
	alter table Cliente_Pet_Vacina alter column codigo_pet varchar(20)
end
else
begin
		alter table Cliente_Pet_Vacina add codigo_pet varchar(20)
end 

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_conta_a_pagar
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10),
	@tipoPagamento varchar(50)
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
			set @where = @where + ' And conta_a_pagar.fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if(@Conferido<>'TODOS')
	begin
		IF(@Conferido='SIM')
		BEGIN
			set @where = @where + ' And conferido =1 '	
		END
		ELSE
		BEGIN
			set @where = @where + ' And ISNULL(conferido,0) =0 '	
		END
		 
	end 
	
	set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
											 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
											 WHEN @STATUS='CANCELADO' THEN ' status =3'
											 WHEN @STATUS='LANCADO' THEN ' status =4'
										ELSE 'status <> 3' END
																) 
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	
	if(@tipoPagamento <> 'TODOS')
	BEGIN
		set @Where = @Where + ' and Conta_a_pagar.TIPO_PAGAMENTO= '+ char(39)+ @tipoPagamento+ char(39) 
	END
	
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			convert(varchar ,pagamento,103) as  Pagamento, 
			Documento = rtrim(ltrim(documento)), 
			Dupl = case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			
			[Tipo pag]=tipo_pagamento,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			[Valor Pagar] = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[Centro Custo]= Conta_a_pagar.codigo_centro_custo,
			Banco = Conta_a_pagar.id_cc					
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc and conta_a_pagar.filial=Conta_corrente.filial  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_receber]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_conta_a_receber]
end
GO
--PROCEDURES =======================================================================================
CREATE   procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250), 
	@status	   varchar(10),
	@centrocusto varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + '  and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Cliente))) > 1
		Begin
			set @where = @where + ' And codigo_cliente = (' + char(39) +replace(@Cliente,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if LEN(@status)>0
	begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
																 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
																 WHEN @STATUS='CANCELADO' THEN ' status =3'
																 WHEN @STATUS='LANCADO' THEN ' status =4'
																ELSE 'status <> '+CHAR(39)+'3'+CHAR(39) END
																) 
	end
	
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
		
	--Monta Select
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Cliente = rtrim(ltrim(Codigo_Cliente)), 
			Obs ,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = convert(numeric,Isnull(Desconto,0)),
			Acrescimo = convert(numeric,Isnull(Acrescimo,0)),
			ValorReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_receber.codigo_centro_custo						
		from dbo.Conta_a_receber Conta_a_receber  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_receber.id_cc = Conta_corrente.id_cc and conta_a_receber.filial=Conta_corrente.filial  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_receber.Filial = Centro_custo.filial
		
		'+@where+'  Order By convert(varchar,'+ @tipo +',102)  '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
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
		--set @string = @string + ' Order By convert('+ @Tipo +',102) '
			Print @string
	Exec(@string)
End

go 

GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND name = N'PK__Cliente__0B528E49')
ALTER TABLE [dbo].[Cliente] DROP CONSTRAINT [PK__Cliente__0B528E49]
GO




/****** Object:  Index [PK_Cliente]    Script Date: 08/04/2016 15:24:46 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND name = N'PK_Cliente')
ALTER TABLE [dbo].[Cliente] DROP CONSTRAINT [PK_Cliente]
GO



/****** Object:  Index [IX_Cliente_Codigo_Nome]    Script Date: 08/04/2016 16:19:43 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND name = N'IX_Cliente_Codigo_Nome')
DROP INDEX [IX_Cliente_Codigo_Nome] ON [dbo].[Cliente] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Pedido_ve__Codig__15A5FF8A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pedido_venda]'))
ALTER TABLE [dbo].[Pedido_venda] DROP CONSTRAINT [FK__Pedido_ve__Codig__15A5FF8A]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente_Pet]') AND name = N'PK__Cliente_Pet__0D3AD6BB')
ALTER TABLE [dbo].[Cliente_Pet] DROP CONSTRAINT [PK__Cliente_Pet__0D3AD6BB]
GO




/****** Object:  Index [IX_Cliente_Codigo_Nome]    Script Date: 08/04/2016 16:19:43 ******/

alter table cliente alter column codigo_cliente varchar(11) not null;


/****** Object:  Index [PK__Cliente_Pet__0D3AD6BB]    Script Date: 08/04/2016 16:28:09 ******/
ALTER TABLE [dbo].[Cliente_Pet] ADD  CONSTRAINT [PK__Cliente_Pet__0D3AD6BB] PRIMARY KEY CLUSTERED 
(
	[Nome_Pet] ASC,
	[Codigo_Cliente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


GO
CREATE NONCLUSTERED INDEX [IX_Cliente_Codigo_Nome] ON [dbo].[Cliente] 
(
	[Codigo_Cliente] ASC,
	[Nome_Cliente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




GO






IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
begin
	create table Versoes_Atualizadas
	(Versao varchar(20), Data_Script Datetime)
end



go
	insert into Versoes_Atualizadas select 'Versão:1.140.573', getdate();
go

























