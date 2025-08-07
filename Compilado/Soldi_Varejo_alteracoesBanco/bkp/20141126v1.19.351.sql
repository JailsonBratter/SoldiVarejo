CREATE procedure [dbo].[sp_Rel_NotasFiscais]
	
	@Filial			varchar(20),			-- Loja 
	@DataDe			varchar(8),				-- Data Inicial
	@DataAte		varchar(8),				-- Data Fim
	@Tipo			varchar(20),				-- Tipo = 1 - Saidas, Tipo = 2 - Entrada
	@Nota			varchar(20),			-- Número Nota Fiscal
	@Fornecedor		varchar(20)				-- Nome Fornecedor

As
	DECLARE @NF varchar(5000)
	
	SET @NF = 'SELECT NF.Filial, NF.Codigo ' + 'Nota' + ', NF.Cliente_Fornecedor ' + 'Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emissão' 
	SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ', Convert(decimal(15,2),Sum(NF_Item.Total)) ' + 'VlrNota'
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

	SET @NF = @NF + 'Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data '
	SET @NF = @NF + 'Order By NF.Filial, NF.Entrada '
	
	exec(@NF)


GO 



CREATE Procedure [dbo].[sp_Rel_Mapa_Resumo]

	@Filial		varchar(20),
	@DataDe		varchar(8),
	@DataAte	varchar(8)
	
As

Begin
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tbFiscal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table tbFiscal

End
	
Select 
	Filial,
	Convert(varchar,Data,103) 'DataMovimento', 
	Caixa,
	GTInicial, 
	GTFinal, 
	Num_Seq_Primeiro_coo 'CupomInicial', 
	Cupom 'CupomFinal' ,  
	Sum(GTFINAL-GTINICIAL) 'VendaBruta', 
	TotCancelado 'TotalCanc', 
	Sum(GTFINAL-GTINICIAL-Isnull(TotCancelado,0)-Isnull(TotDesconto,0)) 'VendaContabil', 
	TotSubstituicao 'TotalSubs', 
	TotIsenta 'TotalIsento', 
	TotNaoTrib,
	(Select Reg6 From Fiscal F Where F.GTInicial is null And F.Data = Fiscal.Data And F.Cupom = Fiscal.Cupom And F.Caixa = Fiscal.Caixa And F.Filial = Fiscal.Filial) REG6
Into tbFiscal
From 
	Fiscal 
Where 
	Data >= @DataDe
And
	Data <= @DataAte
And
	Filial = @Filial	
And 
	Not GTInicial Is Null
Group by 
	Filial, Data, Caixa, GTInicial, GTFinal, Num_Seq_Primeiro_coo, Cupom, TotCancelado, TotSubstituicao, TotIsenta, TotNaoTrib 
Order by 
	Filial, Data, Caixa
	
	
Select 
	Filial,
	DataMovimento, 
	Caixa,
	GTInicial, 
	GTFinal, 
	CupomInicial, 
	CupomFinal ,  
	VendaBruta, 
	TotalCanc, 
	VendaContabil, 
	TotalSubs, 
	TotalIsento, 
	TotNaoTrib,
	cast(Substring(Reg6,12,6) as decimal)/100 T18,
	cast(Substring(Reg6,28,7) as decimal)/100 T07,
	cast(Substring(Reg6,45,7) as decimal)/100 T12,
	cast(Substring(Reg6,62,7) as decimal)/100 T25
Into #tbMapaResumo
From 
	tbFiscal 
--Where 
	--DataMovimento >= @DataDe
--And
	--DataMovimento <= @DataAte
--And
	--Filial = @Filial	


Select 
	Filial,               
	DataMovimento,                  
	Caixa,       
	GTInicial,                               
	GTFinal,                                 
	CupomInicial,           
	CupomFinal, 
	VendaBruta,                              
	TotalCanc,                               
	VendaContabil,                           
	TotalSubs,                               
	TotalIsento,                             
	TotNaoTrib,                              
	Convert(Decimal(12,2),T18) T18,                                     
	Convert(Decimal(12,2),T07) T07,                                     
	Convert(Decimal(12,2),T12) T12,                                     
	Convert(Decimal(12,2),T25) T25
From 
	#tbMapaResumo

Drop Table tbFiscal

End




