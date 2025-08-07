


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('prato_dia'))
begin
	alter table mercadoria alter column prato_dia tinyint
	alter table mercadoria alter column prato_dia_1 tinyint
	alter table mercadoria alter column prato_dia_2 tinyint
	alter table mercadoria alter column prato_dia_3 tinyint
	alter table mercadoria alter column prato_dia_4 tinyint
	alter table mercadoria alter column prato_dia_5 tinyint
	alter table mercadoria alter column prato_dia_6 tinyint
	alter table mercadoria alter column prato_dia_7 tinyint

end
else
begin
	alter table mercadoria add prato_dia tinyint,
							  prato_dia_1 tinyint,
							  prato_dia_2 tinyint,
							  prato_dia_3 tinyint,
							  prato_dia_4 tinyint,
							  prato_dia_5 tinyint,
							  prato_dia_6 tinyint,
							  prato_dia_7 tinyint
end 
go 



go
insert into Versoes_Atualizadas select 'Versão:1.229.729', getdate();
GO
