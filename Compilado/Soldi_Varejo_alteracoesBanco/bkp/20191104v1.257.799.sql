
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas_hora_media]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas_hora_media]
END
GO
Create  Procedure [dbo].[sp_Rel_Resumo_Vendas_hora_media]
            @Filial           As Varchar(20),
            @DataDe           As Varchar(8),
			@DataAte          As Varchar(8),
			@ini_periodo	  as varchar(5),
			@fim_periodo	  as varchar(5),
            @plu			  as Varchar (20),
            @descricao		  as varchar(50),
            @grupo			  as Varchar(50),
            @subGrupo		  as varchar(50),
            @departamento	  as varchar(50),
            @relatorio		  as varchar(40),
			@pdv			  as varchar(5),
			@diasSemana       as nvarchar(max)

 

AS
-- EXEC sp_Rel_Resumo_Vendas_hora_media 'MATRIZ','20190122','20190122','00:00','23:59','','','','','','TODOS','TODOS'
--@relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
	
CREATE TABLE #tmpVendas
(
	hora varchar(20),
	Venda Decimal(18,2),
	Qtde Decimal(18,2),
	Clientes int
	
)

CREATE TABLE #tmpDiasSemanas
(
	dia  int 
)
if(@diasSemana like '%Domingo%')
begin 
	insert into #tmpDiasSemanas values(1)
end 
if(@diasSemana like '%Segunda%')
begin 
	insert into #tmpDiasSemanas values(2)
end 
if(@diasSemana like '%Terca%')
begin 
	insert into #tmpDiasSemanas values(3)
end 
if(@diasSemana like '%Quarta%')
begin 
	insert into #tmpDiasSemanas values(4)
end 
if(@diasSemana like '%Quinta%')
begin 
	insert into #tmpDiasSemanas values(5)
end 
if(@diasSemana like '%Sexta%')
begin 
	insert into #tmpDiasSemanas values(6)
end 
if(@diasSemana like '%Sabado%')
begin 
	insert into #tmpDiasSemanas values(7)
end 


	declare @dias int 
	declare @nPdv  int 

	if(@pdv <> 'TODOS')
	begin
	  set @nPdv = Convert(int,@pdv);
	end
	else
	begin
	 set @nPdv =0;
	end

	  print('n'+convert(varchar,@nPdv))
	  PRINT (@PDV)

	Select @dias = DATEDIFF(day,@DataDe,@DataAte)+1
	--print(convert(varchar,@dias))

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT      saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            Qtde =SUM(ISNULL(Saida_estoque.Qtde,0) ),
			CPF_CNPJ,
			hora_venda
		  INTO
				#Lixo
      FROM
            Saida_Estoque with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
      WHERE
	          saida_estoque.Filial = @Filial
			  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
			  and   hora_venda between @ini_periodo+':00' and @fim_periodo+':59'
			  AND   Data_Movimento  between @DataDe and @DataAte
			  And  (@diasSemana='TODOS' OR DATEPART (weekday, Data_Movimento ) IN (Select dia from #tmpDiasSemanas))
			  AND   Data_Cancelamento IS NULL
			  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
			  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
			  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
			 AND (@nPdv = 0 OR Saida_estoque.Caixa_Saida = @nPdv)
      
      GROUP BY
            Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

PRINT('#TEMPVENDAS')
insert into #tmpVendas
 SELECT
	  substring(Hora_venda,1,2)+':00 - '+substring(Hora_venda,1,2)+':59',
      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
      Qtde = (SELECT Convert(Decimal(18,2),SUM(ISNULL(Qtde,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
	  Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) 
    
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE
            Saida_Estoque.Filial = @Filial
		  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
		  AND (Data_Movimento between @DataDe and @DataAte)
		  AND (hora_venda between @ini_periodo+':00' and @fim_periodo+':59')
		  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
		  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
		  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
		  AND (@nPdv = 0 OR Saida_estoque.Caixa_Saida = @nPdv)
	  GROUP BY    Saida_Estoque.Data_movimento , substring(Hora_venda,1,2)
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
		substring(N.emissao_hora,1,2)+':00 - '+substring(N.emissao_hora,1,2)+':59'  as horas,
		Venda = SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) ,
		Qtde = SUM(isnull(ni.Qtde,0) * isnull(ni.Embalagem,0)) ,
		Clientes = (
		  				Select COUNT(*) 
					from nf 
						inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
							AND  (NF.Emissao = n.Emissao )
							and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
							AND   NF.TIPO_NF = 1 
							AND ISNULL(NF.nf_Canc,0)=0	
							and nf.status='AUTORIZADO'
							AND (NF.Codigo IN (SELECT DISTINCT CODIGO 
														FROM NF_Item as	nii										
														inner join mercadoria  as mi on mi.PLU = nii.PLU
														INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											

														 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
																 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
																 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
																 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
													)
										)
				  ) 
       
      

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao between @DataDe and @DataAte )
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 and  (n.emissao_hora between @ini_periodo+':00' and @fim_periodo+':59')
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY substring(N.emissao_hora,1,2),n.emissao
END


print 'media'
 Select Hora
		,Sum(Venda) as [Venda TT]
		,case when Sum(Venda)>0 then  (Sum(Venda)/@dias) else 0 end as [Venda MD]
		,Sum(Qtde) as [Qtde TT]
		,case when Sum(Qtde)>0 then  (Sum(Qtde)/@dias) else 0 end as [Qtde MD]
		,Sum(clientes) as [Clientes TT]
		,case when SUM(Clientes) >0 then (SUM(Clientes)/@dias) else 0 end as [Clientes MD] 
		
from 
	#tmpVendas
GROUP BY hora
ORDER BY hora ;


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
   @tipoData varchar(20)
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
	into #vacinas from cliente as c
			inner join Cliente_Pet as cp on cp.Codigo_Cliente = c.Codigo_Cliente 
			inner join Cliente_Pet_Vacina as cv on c.Codigo_Cliente = cv.Codigo_Cliente
			inner join vacina as v on cv.Vacina = v.Vacina 
	where (len(@Codigo_Cliente)= 0 or c.Codigo_Cliente = @codigo_cliente)
	     and (len(@nome_cliente)=0 or c.Nome_Cliente like '%'+@nome_cliente+'%')
		 and (len(@codigo_pet) =0 or cp.codigo_pet = @codigo_pet)
		 and (len(@nome_pet)=0 or cp.Nome_Pet like '%'+@nome_pet+'%')
		 

	Select Codigo_Cliente
		 , Nome_Cliente
		 , codigo_pet
		 ,Nome_Pet
		 ,Vacina
		 ,Data_Vacinacao = convert(varchar,Data_Vacinacao,103)
		 ,Validade_Vacina = Convert(varchar,Validade_Vacina,103)
	, Vencida = case when Validade_Vacina < Convert(Date,Getdate()) then 'VENCIDA' ELSE 'NAO' END  from #vacinas 
	where case when @tipoData='Vacinacao' then Data_Vacinacao else Validade_Vacina end between @dataDe and @dataAte
end 


go 
insert into Versoes_Atualizadas select 'Versão:1.257.799', getdate();