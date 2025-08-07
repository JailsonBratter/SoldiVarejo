go

	alter table filial add diretorio_etiqueta varchar(50)
go




CREATE NONCLUSTERED INDEX [index_comanda_cupom] ON [dbo].[Comanda_item] 
(
	[comanda] ASC,
	[cupom] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_estoque]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_produtos_estoque
end
GO

CREATE procedure [dbo].[sp_rel_produtos_estoque]
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
as
Declare @String	As Varchar(MAX)
begin

    SET @String = ' Select a.PLU,'
    SET @String = @String + 'a.Descricao,'
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
    SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
    SET @String = @String + 'isnull(l.Preco_Custo,0) AS [PRECO CUSTO],'
    SET @String = @String + 'isnull(l.saldo_atual,0) AS [SALDO ATUAL],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco_Custo,0)))[VALOR ESTOQUE]'
    SET @String = @String + ' from '
    SET @String = @String + ' Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b '
    SET @String = @String + ' on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial) '
    SET @String = @String + ' inner join mercadoria_loja l on a.plu=l.PLU '
    SET @String = @String + ' left join Familia c on  a.Codigo_familia =c.Codigo_familia '
    SET @String = @String + ' left join EAN on a.plu = ean.plu '
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
	--PRINT @STRING	
	 EXECUTE(@String)	
  end
     GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Outras_Movimentacoes]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Outras_Movimentacoes
end
GO
CREATE  PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
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
	SET @String = @String + ' CONVERT(VARCHAR, i.Data, 102) AS Data,'
	SET @String = @String + ' i.tipoMovimentacao,'
	SET @String = @String + ' i.Usuario,'
	SET @String = @String + ' ii.plu,'
	SET @String = @String + ' m.descricao,'
	SET @String = @String + ' ii.Contada AS Qtde,'
	SET @String = @String + ' ii.custo as Vlr_Unit,'
	SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
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
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Fin_PorOperador
end
GO

CREATE  PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(8),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40) 
            )
as
	
if len(@Caixa)= 0
	begin
		set @Caixa = '%'
	end
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

if(LEN(@fornecedor)=0)
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 

	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 		Data_movimento <=@Dataate and isnull(caixa_saida,'') like @Caixa
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
	from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
		 inner join Fornecedor_Mercadoria fm on a.PLU = fm.plu 				
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 		Data_movimento <=@Dataate and isnull(caixa_saida,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and fm.fornecedor = @fornecedor
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end

go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_NotasFiscais]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_NotasFiscais
end
GO

CREATE  procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial           varchar(20),                -- Loja

      @DataDe           varchar(8),                 -- Data Inicial

      @DataAte          varchar(8),                 -- Data Fim

      @Tipo             varchar(20),                -- Tipo = 1 - Saidas, Tipo = 2 - Entrada

      @Nota             varchar(20),                -- Nmero Nota Fiscal

      @Fornecedor       varchar(20),                -- Nome Fornecedor
      
      @NF_CANC          varchar(20),		    -- Normal = NAO, Cancelada = SIM
      @plu			   varchar(20),
      @ean			   varchar(20),
      @ref             varchar(20),
      @descricao		varchar(40)	

As

      DECLARE @NF varchar(5000)
      --Verifica se sera aplicado filtro por Nota

      IF LTRIM(RTRIM(@Nota)) <> ''
		begin
            SET @NF = 'Select isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor, Plu, Ref=CODIGO_REFERENCIA,Descricao,nf_item.Qtde,nf_item.Embalagem,nf_item.Unitario,nf_item.Total '
            SET @NF = @nf +' from NF_Item INNER JOIN NF ON NF.CODIGO=NF_ITEM.CODIGO and nf.Tipo_NF = nf_item.Tipo_NF and nf.Cliente_Fornecedor= nf_item.Cliente_Fornecedor '
			SET @NF = @nf+'	where nf.codigo = '+@nota
			
			IF LTRIM(RTRIM(@Tipo)) = '1-Saida'
				SET @NF = @NF + 'AND NF.Tipo_NF = 1 '
			IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'
				SET @NF = @NF + 'AND NF.Tipo_NF = 2 '
			IF LTRIM(RTRIM(@Fornecedor)) <> ''
                SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

		end
	else
	begin
     

      SET @NF = 'SELECT NF.Filial,Tipo= case when nf.tipo_nf=1 then '+CHAR(39)+'SAIDA'+CHAR(39)+' else '+CHAR(39)+'ENTRADA'+CHAR(39)+' END, NF.Codigo ' + 'Nota' + ', isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emisso'

      SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ',Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) ' + 'VlrProd' + ', NF.Total ' + 'VlrNota'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '

      SET @NF = @NF + 'AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor AND NF.Filial = NF_Item.Filial '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '
      
      
      --Exibe todas as notas fiscais (canceladas)
      IF LTRIM(RTRIM(@NF_CANC)) = 'SIM'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 1 '
            

      --Exibe todas as notas fiscais (exceto canceladas)     
      IF LTRIM(RTRIM(@NF_CANC)) = 'NAO'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 0 '
      

      --Verifica se sera aplicado filtro por Filial

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 


 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 
	  begin
		if(LEN(@plu)>0)
		 SET @NF = @NF + ' and nf_item.plu='+CHAR(39)+@plu+CHAR(39)	
	  end
		


      SET @NF = @NF + 'Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Total '

      SET @NF = @NF + 'Order By NF.Filial, NF.Entrada '
	end
     
	
   execute(@NF)
--print @nf

