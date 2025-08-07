IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('CodigoNotaProdutor'))
begin
	alter table nf alter column CodigoNotaProdutor varchar(20)
end
else
begin
		alter table nf add CodigoNotaProdutor varchar(20)
end 
go

IF NOT EXISTS (SELECT * FROM PIS_CST_Saida WHERE PIS_CST_Saida='99')
BEGIN

insert into PIS_CST_Saida values(99,'OUTRAS OPERAÇÕES','MATRIZ','99')
END 
GO 


IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NATUREZA_OPERACAO') 
            AND  UPPER(COLUMN_NAME) = UPPER('incide_pisCofins'))
begin
	alter table natureza_operacao add incide_pisCofins tinyint;
	go
	update natureza_operacao set incide_pisCofins =1
END 
go
