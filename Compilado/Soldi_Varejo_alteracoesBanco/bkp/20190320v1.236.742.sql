
IF OBJECT_ID('sp_EFD_ICMSIPI_C800', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800]
end 

go
-- exec [sp_EFD_ICMSIPI_C800] 'MATRIZ','20190207','509446'
create PROC [dbo].[sp_EFD_ICMSIPI_C800]
	@Filial		AS Varchar(20),
	@Data		As Varchar(8),
	@NR_SAT  	As Varchar(8)
	
As
BEGIN
	

		select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '00',
			NUM_CFE = S.coo,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/',''),
			VL_CFE = (SUM(S.vlr)-SUM(CONVERT(NUMERIC(18,2),S.Desconto)))+SUM(S.Acrescimo),
			VL_PIS = SUM(S.TotalPis),
			VL_COFINS = SUM(S.TotalCofins),
			CNPJ_CPF = REPLACE(REPLACE(REPLACE(c.CNPJ,'.',''),'-',''),'/',''),
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =SUM(S.Desconto),
			VL_MERC = SUM(S.VLR),
			VL_OUT_DA =SUM(s.acrescimo),
			VL_ICMS = SUM(S.TotalICMS),
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is null  
		  and (CONVERT(DATE,S.Data_Movimento) =@Data)
		  AND S.numero_serie = @NR_SAT
		 AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				,c.CNPJ
				,S.numero_serie
				,id_Chave
	UNION ALL 
			select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '02',
			NUM_CFE = S.coo,
			DT_DOC = NULL,
			VL_CFE = NULL,
			VL_PIS = NULL,
			VL_COFINS = NULL,
			CNPJ_CPF = NULL,
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =NULL,
			VL_MERC = NULL,
			VL_OUT_DA =NULL,
			VL_ICMS = NULL,
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is NOT null  
		  and (CONVERT(DATE,S.Data_Movimento) =@Data)
		  AND S.numero_serie = @NR_SAT
		  AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,CONVERT(DATE,S.Data_Movimento)
				,c.CNPJ
				,S.numero_serie
				,id_Chave

			
END
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('tipo_fornecedor'))
begin
	alter table fornecedor alter column tipo_fornecedor tinyint
end
else
begin
	alter table fornecedor add tipo_fornecedor tinyint
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_pagar') 
            AND  UPPER(COLUMN_NAME) = UPPER('qtde_parcelas'))
begin
	alter table conta_a_pagar alter column qtde_parcelas int
	alter table conta_a_pagar alter column tipoParcela varchar(20)
	alter table conta_a_pagar alter column qtde_Dias int
	alter table conta_a_pagar alter column forcaDiaUltiVencimento tinyint
end
else
begin
	alter table conta_a_pagar add qtde_parcelas int
	alter table conta_a_pagar add tipoParcela varchar(20)
	alter table conta_a_pagar add qtde_Dias int
	alter table conta_a_pagar add forcaDiaUltiVencimento tinyint

end 
go 


update Fornecedor_Mercadoria set ean =isnull((Select top 1 ean from ean where plu = Fornecedor_Mercadoria.plu  ),'')
where isnull(ean,'') = ''

go
	insert into Versoes_Atualizadas select 'Versão:1.236.742', getdate();
GO
