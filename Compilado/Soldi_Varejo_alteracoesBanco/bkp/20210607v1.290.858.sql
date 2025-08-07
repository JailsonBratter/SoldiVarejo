IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NF_CentroCusto') 
            AND  UPPER(COLUMN_NAME) = UPPER('porc'))
begin
	alter table NF_CentroCusto alter column porc numeric(18,2)
end
else
begin
	alter table NF_CentroCusto add porc numeric(18,2)
end 
go 

insert into Versoes_Atualizadas select 'Versão:1.290.858', getdate();