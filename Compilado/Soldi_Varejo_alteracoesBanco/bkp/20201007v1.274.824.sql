

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].sp_rel_sped_imposta_saida') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_sped_imposta_saida
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure sp_rel_sped_imposta_saida(
		@filial nvarchar(max), 
		@dataDe varchar(11) , 
		@dataAte varchar(11) , 
		@plu varchar(12) ,
		@descricao varchar(50), 
		@tipo varchar(20)
)
as 
begin 
	-- exec sp_rel_sped_imposta_saida @filial = '|MATRIZ|', @dataDe ='20200912', @dataAte = '20200913', @tipo ='ANALITICO', @PLU='186', @descricao ='CANELA'


	--drop table #tempVenda;

	Select ni.Filial
		 , ni.Plu
		 , ni.descricao
		 , sum(ni.total) as Venda
		 , ni.aliquota_pis as [Aliq Pis] 
		 , sum(ISNULL(ni.PISV,0)) as [Vlr Pis] 
		 , ni.aliquota_cofins as [Aliq Cofins] 
		 , sum(ISNULL(ni.COFINSV,0)) as [Vlr Cofings] 
	into #tempVenda from nf_item as ni inner join nf on ni.filial = nf.Filial and ni.Codigo = nf.Codigo and ni.Tipo_NF = nf.Tipo_NF
	 where 
		@filial  like '%|' +ltrim(rtrim(ni.filial)) +'|%' 
		and	convert(date,nf.Emissao)between @dataDe and @dataAte 
		and (len(isnull(@plu,'')) = '' or ni.PLU = @plu)
		and (len(@descricao) = '' or ni.Descricao like '%'+@Descricao+'%')
	group by ni.Filial, ni.Plu, ni.Descricao , ni.aliquota_cofins , ni.aliquota_pis
	order by sum(ni.total) desc

	insert into #tempVenda

	Select s.Filial,
		   s.PLU , 
		   m.Descricao , 
		   sum(s.vlr),  
		   f.pis,
		   sum(ISNULL(s.TotalPis,0)),
		   f.cofins,
		   sum(ISNULL(s.TotalCofins,0))
	from Saida_Estoque as s inner join mercadoria as m  on s.plu = m.PLU 
		inner join filial as f on s.filial = f.Filial 
	WHERE @filial  like '%|' +ltrim(rtrim(s.filial)) +'|%' and 
		convert(date,s.Data_movimento)between @dataDe and @dataAte and 
		(len(isnull(@plu,'')) = '' or s.PLU = @plu)
		and (len(@descricao) = '' or m.Descricao like '%'+@Descricao+'%')
	group by s.Filial, s.Plu, m.Descricao , f.cofins , f.pis

	if(@tipo ='ANALITICO')
	BEGIN
		Select  Plu
			 , descricao
			 , sum(Venda) as Venda
			 , [Aliq Pis] 
			 , sum([Vlr Pis] ) AS [Vlr Pis]
			 , [Aliq Cofins] 
			 , sum([Vlr Cofings] ) AS [Vlr Cofings]
		from #tempVenda
		GROUP BY PLU, Descricao,[Aliq Pis],[Aliq Cofins]
	END 
	ELSE
	BEGIN 
		Select  Filial
			 , sum(Venda) as Venda
			 , sum([Vlr Pis] ) AS [Vlr Pis]
			 , sum([Vlr Cofings] ) AS [Vlr Cofings]
		from #tempVenda
		GROUP BY Filial
	END
	
end


go 
insert into Versoes_Atualizadas select 'Versão:1.274.824', getdate();