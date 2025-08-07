ALTER procedure [dbo].[sp_NFe_Total]
	@Id varchar(47)
as

select ID,emit_cnpj=ISNULL(emit_cnpj,emit_cpf),
ide_dEmi,
ide_nNF,
Total_ICMSTot_vBC,
Total_ICMSTot_vICMS,
Total_ICMSTot_vBCST,
Total_ICMSTot_vST,
Total_ICMSTot_vProd,
Total_ICMSTot_vFrete,
Total_ICMSTot_vSeg,
Total_ICMSTot_vDesc,
Total_ICMSTot_vII,
Total_ICMSTot_vIPI,
Total_ICMSTot_vPIS,
Total_ICMSTot_vCOFINS,
Total_ICMSTot_vOutro,
Total_ICMSTot_vNF,
Total_ISSQNTot_vServ,
Total_ISSQNTot_vBC,
Total_ISSQNTot_vISS,
Total_ISSQNTot_vPIS,
Total_ISSQNTot_vCOFINS,
Total_reTrib_vRetPIS,
Total_reTrib_vRetCOFINS,
Total_reTrib_vRetCSLL,
Total_reTrib_vBCIRRF,
Total_reTrib_vIRRF,
Total_reTrib_vBCRetPrev,
Total_reTrib_vRetPrev,
Total_reTrib_modFrete
 from nfe_xml where  id=@id and total_icmstot_vNf is not null


go


ALTER     procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial                 varchar(20),                 -- Loja

      @DataDe                 varchar(8),                  -- Data Inicial

      @DataAte          varchar(8),                  -- Data Fim

      @Tipo             varchar(20),                       -- Tipo = 1 - Saidas, Tipo = 2 - Entrada

      @Nota             varchar(20),                 -- N�mero Nota Fiscal

      @Fornecedor       varchar(20)                  -- Nome Fornecedor

 

As

      DECLARE @NF varchar(5000)

     

      SET @NF = 'SELECT NF.Filial, NF.Codigo ' + 'Nota' + ', (Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) >From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End) ' + 'Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emiss�o'

      SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ', NF.Status ' + ',Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) ' + 'VlrNota'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '

      SET @NF = @NF + 'AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor AND NF.Filial = NF_Item.Filial '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '

     

      --Verifica se sera aplicado filtro por Fiali

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 

      --Verifica se sera aplicado filtro por Nota

      IF LTRIM(RTRIM(@Nota)) <> ''

            SET @NF = @NF + 'AND NF.Codigo = ' + CAST(@Nota as Varchar) + ' '

 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 

      SET @NF = @NF + 'Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Status '

      SET @NF = @NF + 'Order By NF.Filial, NF.Entrada '

     

    exec(@NF)

--print @nf

 

 

go





