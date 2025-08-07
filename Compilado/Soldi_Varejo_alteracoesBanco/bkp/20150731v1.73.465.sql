

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_HISTORIO_ENTRADA_SAIDA
end
GO




--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA](
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@plu as Varchar(20), 
@Ref as Varchar(20)

)
As


DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

--set @DataInicio ='20140401'
--set @DataFim ='20140910'
--Set @plu ='1'

Create table #OUTROS
(
	DATA  DATETIME ,
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
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
		   ,[NF Saida] =0
		   ,[NF Saida Preco]	= 0
		  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
		  ,[Cupom Preco]=Sum(se.vlr)	
	from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	where (se.PLU=@plu OR (LEN(@REF)>0 AND M.Ref_fornecedor=@Ref)) and  se.Data_movimento between @DataInicio AND @DataFim
	group by se.Data_movimento
END;


Select Data = CONVERT(varchar,todos.data,103)
		,[Entrada]=Sum(todos.[NF Entrada])
		,[Entrada_Valor]=Sum(todos.[NF Compra Preco])
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
		
WHERE A.PLU = @plu AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
group by Data 

UNION ALL

Select
	DATA   ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by DATA
 



) as todos
group by todos.Data
order by convert(varchar ,todos.Data,102) desc
















go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Caderneta_Utilizado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Caderneta_Utilizado
end
GO

CREATE procedure [dbo].[sp_Rel_Caderneta_Utilizado]

@Filial as varchar(20)

,@codigo_cliente as varchar(8)

,@nome_cliente as varchar(50)

as

Begin

      Select Codigo_Cliente, Nome_Cliente, Limite_Credito = Isnull(Limite_Credito,0), Utilizado = Isnull(Utilizado,0) ,Saldo = Isnull((Limite_Credito-Utilizado),0)  from Cliente

      Where (LEN(@codigo_cliente)=0 or Codigo_Cliente=@codigo_cliente)

      and (LEN(@nome_cliente)=0 or Nome_Cliente like '%'+@nome_cliente+'%' )

      And Limite_Credito > 0

end


GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Ret_Seq_Ref_Fornecedor]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Ret_Seq_Ref_Fornecedor
end
GO

-- sp_Ret_Seq_Ref_Fornecedor 'PPA'
CREATE PROCEDURE [dbo].[sp_Ret_Seq_Ref_Fornecedor]
	(@REF VARCHAR(3))
--WITH EXECUTE AS CALLER
AS

BEGIN
	DECLARE @SeqRef int;
	
	SET @SeqRef = 0
	
	BEGIN TRY
		SELECT 
			@SeqRef = MAX(CONVERT(INT, RTRIM(LTRIM(ISNULL(SUBSTRING(Mercadoria.Ref_Fornecedor,4,4),'0'))))) 
		FROM 
			Mercadoria
		WHERE 
			SUBSTRING(Mercadoria.Ref_Fornecedor, 1, 3) = @REF;
	END TRY
	
	BEGIN CATCH
		IF @@ERROR <> 0
			SET @SeqRef = 9998
	END CATCH
			
	SELECT SEQ = (@SeqRef + 1);

END;



go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Posicao_Cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_fin_Posicao_Cliente
end
GO


CREATE procedure [dbo].[sp_rel_fin_Posicao_Cliente](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@valor VARCHAR(11) ,
@cliente varchar(50),
@status varchar(10),
@centrocusto varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10)
)
--sp_rel_FIN_POSICAO_CLIENTE 'MATRIZ', '20150801', '20150804', 'EMISSAO', '', 'ANTO', '', '', '', ''
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
if len(rtrim(ltrim(@Cliente))) > 1
Begin
set @where = @where + ' And cliente.Nome_Cliente like ' + char(39) +'%' + @Cliente + '%' +char(39)
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
ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
)
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

--Monta Select
set @string = 'select
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
Documento = rtrim(ltrim(documento)),
Cliente = rtrim(ltrim(cliente.Codigo_Cliente)),
Nome_Cliente = rtrim(ltrim(cliente.nome_Cliente)),
Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
VlrReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0) else 0 end,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + '),
Modalidade = CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END ' +

' from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By convert(varchar,'+ @tipo +',102) '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

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