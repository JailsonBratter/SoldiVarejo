
ALTER    view [dbo].[w_br_movimento] as     
select a.filial, a.data_movimento, a.documento, a.origem, cast(a.plu as decimal(18)) as plu, cast(a.codigo_cliente as decimal(11)) as codigo_cliente, a.hora_venda, a.qtde, a.vlr, a.caixa_saida, a.codigo_funcionario, a.ean,     
   c.codigo_grupo, c.descricao_grupo , c.codigo_subgrupo, c.descricao_subgrupo, b.codigo_departamento, c.descricao_departamento, b.descricao, b.codigo_familia,
   a.vlr vlr_vendido, a.qtde * b.preco_custo vlr_comprado, ultimo_fornecedor, b.tipo, a.VENDEDOR, Incide_Pis = case when isnull(b.incide_pis,0) = 0 then 4 Else b.Incide_pis end, b.codigo_tributacao,  d.descricao_tributacao, A.PEDIDO, 
   Descricao_PisC = Case   When IsNull(Incide_Pis,0) = 1 Then 'TRIBUTADOS INTEGRALMENTE'
																								When IsNull(Incide_Pis,0) = 2 Then 'SUJEITOS A ALIQUOTA 0%'
																								When IsNull(Incide_Pis,0) = 3 Then 'MONOFASICOS'
																								When IsNull(Incide_Pis,0) = 5 Then 'SUBSTITUICAO TRIBUTARIA'
																							 Else   'ISENTOS' END, id_finalizadora
																							 
																			
 from saida_estoque a    
  left outer join mercadoria b on (a.filial = b.filial and a.plu = b.plu)    
  left outer join w_br_cadastro_departamento c on (a.filial = c.filial and b.codigo_departamento = c.codigo_departamento)    
  left outer join tributacao d on (b.codigo_tributacao = d.codigo_tributacao)     
  left outer join lista_finalizadora l on (a.documento=l.documento AND A.Filial=L.filial )
 where data_cancelamento is null    
union all    
select filial, data_venda, cupom_venda, 'ZAN', '999999', null, '00:00', 1, valor_venda, caixa_venda, null, null,    
   99, 'DIRETO', '099999', 'DIRETO', '099999999', 'DIRETO', 'VENDA DIRETA', null, valor_venda, valor_venda, '', 'PRINCIPAL','', 4, 0, '', 0,'',''
 from venda_grupo     
 where data_cancelamento is null    