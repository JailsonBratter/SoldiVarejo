Create Table Nf_manifestar(
	Filial varchar(20),
	NSU varchar(15),
	vNf Numeric(18,2),
	CNPJ VARCHAR(20),
	RazaoSocial varchar(200),
	Chave varchar(45),
	Emissao Datetime,
	status varchar(30),
	nfeXML nvarchar(max)
)
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria_obs') 
            AND  UPPER(COLUMN_NAME) = UPPER('obrigatorioOrdem'))
begin
	alter table mercadoria_obs alter column obrigatorioOrdem tinyint
end
else
begin
	alter table mercadoria_obs add obrigatorioOrdem tinyint
end 
go 



go
	insert into Versoes_Atualizadas select 'Versão:1.240.751', getdate();
GO
