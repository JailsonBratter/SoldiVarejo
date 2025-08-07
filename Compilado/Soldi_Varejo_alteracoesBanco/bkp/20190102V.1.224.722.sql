

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tipo_pagamento') 
            AND  UPPER(COLUMN_NAME) = UPPER('parcelamento'))
begin
	alter table tipo_pagamento alter column parcelamento tinyint
end
else
begin
	alter table tipo_pagamento add parcelamento tinyint
end 
go 



go

insert into Versoes_Atualizadas select 'v.1.224.722', getdate();
GO




