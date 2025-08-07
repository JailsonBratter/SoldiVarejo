

if object_id('cliente_grupo') is null
begin
	create table cliente_grupo (
		id int identity,
		grupo varchar(40)
	)
end 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('grupo_empresa'))
begin
	alter table cliente alter column grupo_empresa int
end
else
begin
	alter table cliente add grupo_empresa int
end 
go 

IF EXISTS(SELECT * FROM FILIAL WHERE CNPJ = '02795831000122')  -- EXECUTA APENAS NO PORTAL DO PADEIRO
BEGIN
	insert into cliente_grupo values
	('GR'),
	('SODEXO'),
	('SAPORE'),
	('OUTROS')
			
	UPDATE CLIENTE SET grupo_empresa = 1 WHERE CNPJ = '02.905.110/0001-28'

	UPDATE CLIENTE SET grupo_empresa = 2 WHERE CNPJ = '49.930.514/0001-35'

	UPDATE CLIENTE SET grupo_empresa = 3 WHERE CNPJ = '67.945.071/0001-38'

	UPDATE CLIENTE SET grupo_empresa = 4 WHERE grupo_empresa IS NULL

END 

go 
IF OBJECT_ID('sp_Rel_Cliente_nf', 'P') IS NOT NULL
begin 
	drop procedure [sp_Rel_Cliente_nf]
end 
go 
create  PROCEDURE [dbo].[sp_Rel_Cliente_nf] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cnpj       as Varchar(20),
		@tipo       as varchar(20)
		as
BEGIN
	-- exec [sp_Rel_Cliente_nf] 'MATRIZ','20180301','20180307','49.930.514/0001-35','ANALITICO'
	
	IF(LEN(@CNPJ)>0)
	BEGIN 
		SET @CNPJ = LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(@CNPJ,'.',''),'/',''),'-','')))
	END 
	

	Select 
			Emissao,
			nf.Cliente_Fornecedor Cod,
			cliente.Nome_Cliente,
			cliente.nome_fantasia,
			cg.grupo,
			Total = Sum(nf_item.Total) 
		into #tempVendaNF from NF inner join cliente on NF.Cliente_Fornecedor= cliente.Codigo_Cliente 
			     INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 
				 inner join cliente_grupo as cg on cg.id= cliente.grupo_empresa		
		 where NF.Tipo_NF = 1 
			and (NF.Data between @DataDe and @DataAte )
			AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
			AND (LEN(@CNPJ)=0 OR LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(CLIENTE.CNPJ,'.',''),'/',''),'-','')))= @CNPJ)
		 group by NF.EMISSAO,nf.Cliente_Fornecedor,cliente.Nome_Cliente,cliente.nome_fantasia, cg.grupo
		 ORDER BY CONVERT(VARCHAR,NF.EMISSAO,102) 



	if(@tipo='ANALITICO')
	BEGIN
		Select 
		CONVERT(VARCHAR,EMISSAO,103) EMISSAO,
		COD='SFT_'+Cod,
		Cliente= Nome_Cliente,
		Total 
		from #tempVendaNF		
	END
	ELSE	
	BEGIN
		if(@tipo='SINTETICO')
		begin  
			Select 
				CONVERT(VARCHAR,EMISSAO,103) EMISSAO,
				Cliente =nome_fantasia,
				Total = Sum(Total) 
			 from
			 #tempVendaNF
			 group by Emissao , nome_fantasia
		end
		else
		begin 
			if(len(@cnpj)=0)
			begin
				Select 
					Grupo,
					Total = Sum(total)
				from #tempVendaNF
				group by grupo
				order by sum(total) desc
			end
			else
			begin 
				Select 
					COD='SFT_'+Cod,
					Cliente=Nome_Cliente,
					Total = Sum(total)
				from #tempVendaNF
				group by COD, Nome_Cliente
				order by sum(total) desc
			end 

		end 
	END
END




GO
insert into Versoes_Atualizadas select 'Versão:1.220.716', getdate();
GO
