



SET QUOTED_IDENTIFIER ON

GO

SET ANSI_NULLS ON

GO

 

ALTER PROCEDURE sp_Rel_Fin_PorOperadorCancelamento(

      @FILIAL     AS VARCHAR(17),

      @Datade           As DATETIME,

      @Dataate    As DATETIME,

      @Operador         As varchar(20))

as

if len(@Operador) =0

      begin

            set @Operador ='%'     

      end

 

select b.Nome,convert(varchar,a.emissao,103)Data 

      , Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and c.cancelado is  null), 0) as  Vendas

      , isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and   c.cancelado is not null),0)as  Cancelados

      from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador

 

      where a.filial=@FILIAL and b.nome like @Operador and a.emissao between @Datade and @Dataate

group by a.operador,b.nome,a.emissao

order by b.nome

 

 

 

GO

SET QUOTED_IDENTIFIER OFF

GO

SET ANSI_NULLS ON

GO


---=============================================================================================================================

---=============================================================================================================================

---=============================================================================================================================

USE [BratterWeb]
GO
/****** Object:  StoredProcedure [dbo].[sp_rel_balanceteFinanceiro]    Script Date: 01/19/2015 13:28:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec sp_rel_balanceteFinanceiro 'MATRIZ','20141101','20141130','vencimento','ABERTO'

ALTER  procedure [dbo].[sp_rel_balanceteFinanceiro](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@status	    varchar(10)
)As

Declare @String as nvarchar(4000)
Declare @Where as nvarchar(1024)
Declare @codStatus as varchar(50)
DECLARE @String2 as nvarchar(4000)

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
	set @string = 'Select Codigo,Descricao,replace(isnull(debito,'+ char(39) +'0,00'+ char(39) +'),'+ char(39) +'.'+ char(39) +','+ char(39) +','+ char(39) +') as Debito,Replace(isnull(credito,'+ char(39) +'0,00'+ char(39) +'),'+ char(39) +'.'+ char(39) +','+ char(39) +','+ char(39) +') as Credito,Replace(isnull(saldo,'+ char(39) +'0,00'+ char(39) +'),'+ char(39) +'.'+ char(39) +','+ char(39) +','+ char(39) +') as Saldo from ( '+
				'Select  CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as ordem, CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end as codigo, descricao_Grupo as descricao , ' + char(39) +' ' + char(39) +' as Debito , ' + char(39) +' ' + char(39) +' as Credito, '+char(39)+' '+char(39)+'as saldo from  W_br_contas '+@where+' group by codigo_grupo,descricao_Grupo '+
				'union '+
				'select codigo_subgrupo as ordem, codigo_subgrupo as codigo, descricao_subgrupo as descricao ,' + char(39) +' ' + char(39) +' as Debito , ' + char(39) +' ' + char(39) +' as Credito, '+char(39)+' '+char(39)+'as saldo  from  W_br_contas '+@where+' group by codigo_grupo,codigo_subgrupo,descricao_subgrupo '+
				'union '+
				'Select  codigo_centro_custo as ordem, codigo_centro_custo,descricao_centro_custo, '+
				'	Debito = convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) , '+
				'	Credito = convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10), '+
				'      	Saldo = convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10) '+
				'from  W_br_contas '+@where+' '+
				'group by codigo_grupo ,codigo_centro_custo,descricao_centro_custo '+
				'union '+
				'Select CASE WHEN CODIGO_GRUPO >9  THEN ' + char(39) +'0' + char(39) +'+codigo_grupo ELSE ' + char(39) +'00' + char(39) +'+codigo_grupo end+' + char(39) +'999' + char(39) +' as ordem,' + char(39) +' ' + char(39) +' as codigo , ' + char(39)  + char(39) +', '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +', '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+  convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' '+
				'	from  W_br_contas '+@where+' '+
				'group by codigo_grupo,descricao_Grupo '+
				'union '+
				'Select rtrim(codigo_subgrupo)+' + char(39) +'999' + char(39) +' as ordem,' + char(39) +' ' + char(39) +' as codigo, ' + char(39) + char(39) +',  '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+ convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' ,  '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10) +' + char(39) +'-|' + char(39) +''+
				'	from  W_br_contas '+@where+' '+
				'group by codigo_subgrupo,descricao_subgrupo '
				
			set @String2=	'union '+
				'Select  '+char(39) +'999' + char(39) +' as ordem,' + char(39) +'TOTAL ' + char(39) +' as codigo, ' + char(39) + char(39) +',  '+
				'	Debito = ' + char(39) +'|-' + char(39) +'+ convert(varchar, sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' , '+
				'	Credito = ' + char(39) +'|-' + char(39) +'+ convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end),10)+' + char(39) +'-|' + char(39) +' ,  '+
				'      	Saldo = ' + char(39) +'|-' + char(39) +'+convert(varchar,sum(case when modalidade=' + char(39) +'RECEITAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) else valor_pago end else 0 end)- '+
				'		sum(case when modalidade=' + char(39) +'DESPESAS' + char(39) +' then case when status=1 then ((isnull(valor,0)-isnull(desconto,0))+isnull(acrescimo,0))else valor_pago end else 0 end),10) +' + char(39) +'-|' + char(39) +''+
				'	from  W_br_contas '+@where+' '+
				''+
				
				
		') as a '+
			'order by ordem'
			
			

	--Print @string+@String2
	Exec(@string+@String2)
End

