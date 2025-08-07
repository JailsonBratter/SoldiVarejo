
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_pagar') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table conta_a_pagar alter column serie int
end
else
begin
	alter table conta_a_pagar add serie int
end 
go 


insert into Versoes_Atualizadas select 'Versão:1.270.818', getdate();