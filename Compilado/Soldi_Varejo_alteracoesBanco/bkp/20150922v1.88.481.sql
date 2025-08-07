go
create table agenda_horarios ( horario varchar(5),intervalo int)
go 

alter table funcionario add utiliza_agenda tinyint

go 


GO
/****** Object:  StoredProcedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido]    Script Date: 09/21/2015 10:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido](
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
--sp_rel_fin_Ficha_Cliente_Pedido 'MATRIZ', '20150918', '20150918', 'EMISSAO', '', '', '', '', ''
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
if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END

)
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
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro	
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.nome_Cliente, cliente.Codigo_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

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





GO

/****** Object:  Index [IX_Cliente_Codigo_Nome]    Script Date: 09/18/2015 21:51:45 ******/
CREATE NONCLUSTERED INDEX [IX_Cliente_Codigo_Nome] ON [dbo].[Cliente] 
(
	[Codigo_Cliente] ASC,
	[Nome_Cliente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperadorCancelamento]    Script Date: 09/22/2015 09:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

 
--sp_Rel_Fin_PorOperadorCancelamento 'MATRIZ', '20150701', '20150701', '', ''
ALTER PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

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

      , Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and c.cancelado is  null), 0) as  Vendas

      , isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   c.cancelado is not null),0)as  Cancelados
      

      from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador

 

      where a.filial=@FILIAL and b.nome like @Operador and a.emissao between @Datade and @Dataate And a.pdv like @Pdv

group by a.operador,a.Pdv, b.nome,a.emissao

order by a.Pdv, b.nome

 

 

 



