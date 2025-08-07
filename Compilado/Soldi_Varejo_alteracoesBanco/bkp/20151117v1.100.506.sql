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
	@status	    varchar(10)
)As

Declare @String as varchar(8000)
Declare @Where as varchar(1024)
Declare @codStatus as varchar(50)
DECLARE @String2 as varchar(8000)

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
		
		
	
	--Monta Clausula Where da Procura
	set @where = 'Where W_br_contas.Filial = '+ char(39) + @filial + char(39) + '   and status '+@codStatus+' and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
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
				'Select modalidade ='+char(39) +char(39) +',  '+char(39) +'999' + char(39) +' as ordem,' + char(39) +'TOTAL ' + char(39) +' as codigo, ' + char(39) + char(39) +',  '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+ convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' ,  '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10) +' + char(39) +'-|' + char(39) +''+
				'	from  W_br_contas '+@where+' '+
				''+
			
				
		') as a '+
			'order by modalidade desc,ordem'
			
			

	--Print @string+@String2
	 Exec(@string+@String2)
End
