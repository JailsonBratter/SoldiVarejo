IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('tributacao_padrao'))
begin
	alter table Natureza_operacao alter column tributacao_padrao varchar(2)
end
else
begin
	alter table Natureza_operacao add tributacao_padrao varchar(2)
end 
go 




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Cliente_pet_vacinas]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_Cliente_pet_vacinas]
END
GO
Create PROCEDURE sp_rel_Cliente_pet_vacinas (
   @filial varchar(40),
   @codigo_cliente varchar(20),
   @nome_cliente varchar(200),
   @codigo_pet varchar(40),
   @nome_pet varchar(50),
   @dataDe varchar(15),
   @dataAte varchar(15),
   @tipoData varchar(20),
   @Vencidas varchar(5)
)
as 
begin
   
	-- exec  sp_rel_Cliente_pet_vacinas 'MATRIZ','','','','','20191105','20191105','Vencimento'
	Select c.Codigo_Cliente
		 , c.Nome_Cliente
		 , cp.codigo_pet
		 ,cp.Nome_Pet
		 ,cv.Vacina
		 ,Data_Vacinacao = cv.Data_ultima_vacinacao
		 ,Validade_Vacina =dateadd(DAY,v.frequencia_vacina,cv.Data_ultima_vacinacao)
		 ,Vencida= '-----'
	into #vacinas from cliente as c
			inner join Cliente_Pet as cp on cp.Codigo_Cliente = c.Codigo_Cliente 
			inner join Cliente_Pet_Vacina as cv on c.Codigo_Cliente = cv.Codigo_Cliente and cv.codigo_pet=cv.codigo_pet
			inner join vacina as v on cv.Vacina = v.Vacina 
	where (len(@Codigo_Cliente)= 0 or c.Codigo_Cliente = @codigo_cliente)
	     and (len(@nome_cliente)=0 or c.Nome_Cliente like '%'+@nome_cliente+'%')
		 and (len(@codigo_pet) =0 or cp.codigo_pet = @codigo_pet)
		 and (len(@nome_pet)=0 or cp.Nome_Pet like '%'+@nome_pet+'%')
		 
	UPDATE #vacinas SET Vencida = case when Validade_Vacina < Convert(Date,Getdate()) then 'VENCIDA' ELSE 'NAO' END
	
	Select Codigo_Cliente
		 , Nome_Cliente
		 , codigo_pet
		 ,Nome_Pet
		 ,Vacina
		 ,Data_Vacinacao = convert(varchar,Data_Vacinacao,103)
		 ,Validade_Vacina = Convert(varchar,Validade_Vacina,103)
	     , Vencida 
	  from #vacinas 
	where 
	   (@Vencidas='TODOS'  OR Vencida = @Vencidas )
	and (case when @tipoData='Vacinacao' then Data_Vacinacao else Validade_Vacina end between @dataDe and @dataAte)
end 

go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_nf_entrada_imposto]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_nf_entrada_imposto]
END
GO

create procedure sp_rel_nf_entrada_imposto(
	@FILIAL VARCHAR(40)
	,@Datade VARCHAR(15)
	,@Dataate varchar(15)
	,@Fornecedor varchar(40)
)
as
begin 
-- exec sp_rel_nf_entrada_imposto 'MATRIZ','20190101' , '20191101',''
	Select Codigo
		  ,Fornecedor = Cliente_Fornecedor
		  ,Emissao = Convert(varchar,Emissao,103)
		  ,PIS= ISNULL(pisv,0)
		  ,COFINS=ISNULL(cofinsv,0)
		  ,ICMS=ISNULL(ICMS_Nota,0)
		  ,ICMS_ST= ISNULL(ICMS_ST,0)  
		  ,IPI=ISNULL(IPI_Nota,0)
		  ,Outras = Isnull(Despesas_financeiras,0)
		  ,Total_produto = ISNULL(TOTAL_PRODUTO,0)
		  ,Total
	from nf 
	where filial =@filial 
	AND  TIPO_NF =2 
	AND  (emissao between @Datade and @Dataate)
	and isnull(nf_Canc,0) = 0
	and (len(@Fornecedor)=0 or Cliente_Fornecedor = @Fornecedor)
end 
go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_desconto_tabela_cliente]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_desconto_tabela_cliente]
END
GO
create procedure [dbo].[sp_rel_desconto_tabela_cliente] (
	@filial as varchar(50),
	@datade varchar(8),
	@dataate varchar(8),
	@Codigo_Cliente varchar(50),
	@tipo varchar(20),
	@plu varchar(20)
)
as 
begin
-- sp_rel_desconto_tabela_cliente 'MATRIZ', '20191001','20191106','','ANALITICO',''

	 Select s.Documento
		   ,c.Codigo_Cliente
		   ,c.Nome_Cliente
		   ,Codigo_tabela =case when isnull(t.Codigo_tabela,'') ='' then 'SEM TABELA' ELSE t.Codigo_tabela END 
		   ,Porc_desc=isnull(t.porc,0)
		   ,Qtde = sum(isnull(s.qtde,0))
		   ,Valor =sum(isnull(s.vlr,0)) 
		   ,Valor_Desconto = sum(isnull(s.desconto,0))
	 into #tbDesconto from Saida_estoque  as s 
	 left join cliente as c on s.Codigo_Cliente = c.Codigo_Cliente 
	 left join  tabela_preco as t on  t.Codigo_tabela = c.Codigo_tabela
	 WHERE s.Filial= @filial 
		and s.data_cancelamento is null
		and  s.Data_movimento between @datade and @dataate
		and (len(@plu) =0 or s.PLU = @plu)
		and (len(@Codigo_Cliente) =0 or c.Codigo_Cliente = @Codigo_Cliente)
		group by s.Documento
				,c.Codigo_Cliente
				,c.Nome_Cliente
				,case when isnull(t.Codigo_tabela,'') ='' then 'SEM TABELA' ELSE t.Codigo_tabela END
				,t.porc 	


	if(@tipo='ANALITICO')
	BEGIN  
		Select Codigo_Cliente
			  ,Nome_Cliente
			  ,Codigo_tabela
			  ,[%]=Porc_desc
			  ,Valor  = Sum(Valor)
			  ,Qtde = '|-NI-|' +CONVERT(VARCHAR,Sum(qtde))
			  ,Valor_Desconto  =Sum(Valor_desconto)
		from #tbDesconto
		group by Codigo_Cliente,Nome_Cliente,Codigo_tabela,Porc_desc
	END 
	ELSE
	BEGIN
		
		Select   Codigo_tabela
				,[%]=isnull(Porc_desc,0)
				,[Qtde Clientes]= '|-NI-|' +CONVERT(VARCHAR,count(Documento))
				,[Qtde Vendia]= sum(qtde)
				,Valor =sum(isnull(Valor,0)) 
				,Valor_Desconto = sum(isnull(Valor_Desconto,0)) 
		from #tbDesconto
		group by Codigo_tabela, isnull(Porc_desc,0)
		

	END 
end 
go 
insert into Versoes_Atualizadas select 'Versão:1.258.800', getdate();