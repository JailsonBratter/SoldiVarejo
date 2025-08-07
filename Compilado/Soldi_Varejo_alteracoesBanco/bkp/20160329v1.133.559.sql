
Create Table tesouraria_detalhes (
 emissao datetime,
 pdv int,
 operador varchar(9),
 finalizadora int,
 cupom varchar(20),
 total numeric(18,2),
 id_finalizadora varchar(40),
 id_cartao varchar(20),
 autorizacao varchar(9),
 id_bandeira varchar(3),
 rede_cartao varchar(3)
 );
 
 go


alter table usuarios_web add id_operador int , codigo_funcionario varchar(9)
GO 

ALTER TABLE  TESOURARIA ADD FINALIZADORA INT , FILIAL VARCHAR(20), DATA_ABERTURA DATETIME ,PDV INT ,ID_OPERADOR INT
GO 
ALTER TABLE TESOURARIA ALTER COLUMN ID_FINALIZADORA VARCHAR(20)
GO
alter table tesouraria alter column Total_Sistema decimal(18,2) ;
go
alter table tesouraria alter column Total_Entregue  decimal(18,2);
go
alter table tesouraria drop column id
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_balanceteFinanceiro]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_balanceteFinanceiro]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_rel_balanceteFinanceiro](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@status	    varchar(10),
	@visualizar varchar(50),
	@Modalidade	varchar(8)
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
				'	Debito = convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) , '+
				'	Credito = convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10), '+
				'      	Saldo = '+char(39) +char(39) +
				'from  W_br_contas '+@where+' '+
				'group by modalidade,codigo_grupo ,codigo_centro_custo,descricao_centro_custo '+
				'union '+
				
				'Select modalidade, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end+'+CHAR(39)+' '+CHAR(39)+' as ordem,'+CHAR(39)+CHAR(39)+' as codigo , '+CHAR(39)+CHAR(39)+', '+
				'	Debito = '+CHAR(39)+CHAR(39)+','+
				'	Credito = '+CHAR(39)+CHAR(39)+', '+
				'    Saldo = '+CHAR(39)+CHAR(39)+' '+
				'	from  W_br_contas '+@where+' '+
				'group by modalidade,codigo_grupo,descricao_Grupo '+
				'union '+
				
				'Select modalidade, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as ordem, ' + char(39) +'|-' + char(39) +'+CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end +' + char(39) +'-|' + char(39) +' as codigo , ' + char(39) +'|-' + char(39) +'+ descricao_Grupo +' + char(39) +'-|' + char(39) +', '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +', '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' '+
				'	from  W_br_contas '+@where+' '+
				'group by modalidade,codigo_grupo,descricao_Grupo '+
				'union '+
				'Select modalidade, rtrim(codigo_subgrupo) as ordem,codigo_subgrupo as codigo, descricao_subgrupo,  '+
				'	Debito =  convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) , '+
				'	Credito =  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) ,  '+
				'      	Saldo = '+char(39) +char(39) +
				'	from  W_br_contas '+@where+' '+
				'group by modalidade, codigo_subgrupo,descricao_subgrupo '
				
			set @String2=	'union '+
				'Select modalidade ='+char(39) +char(39) +',  '+char(39) +'999' + char(39) +' as ordem,' + char(39) +'|-TOTAL-| ' + char(39) +' as codigo, ' + char(39) + char(39) +',  '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+ convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' ,  '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10) +' + char(39) +'-|' + char(39) +''+
				'	from  W_br_contas '+@where+' '+
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
	set @String2 = @String2 + ' Select Distinct Codigo, Modalidade, Descricao, Valor = replace(convert(varchar(20),Sum(Valor)),' + char(39) + '.' + char(39) + ',' + char(39) + ',' + char(39) + ') from #Balancete Group by Codigo, Modalidade, Descricao '
	set @String2 = @String2 + ' UNION all Select '+ char(39) + '|-TOTAL-|' + char(39) + ','+ char(39) + '' + char(39) + ',' + char(39) + '' + char(39) + ','+ char(39)+'|-'+ char(39)+' + replace(convert(varchar(20),Sum(isnull(Valor,0))),' + char(39) + '.' + char(39) + ',' + char(39) + ',' + char(39) + ') +'+ char(39)+'-|'+ char(39)+' from #Balancete '
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
