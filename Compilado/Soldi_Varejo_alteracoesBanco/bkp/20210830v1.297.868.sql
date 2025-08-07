IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('autorizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('padrao'))
begin
	alter table autorizadora alter column padrao tinyint
end
else
begin
	alter table autorizadora add padrao tinyint
end 
go 



DROP INDEX IF EXISTS ix_lf_tesouraria ON [dbo].[Lista_finalizadora]
GO

SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [ix_lf_tesouraria] ON [dbo].[Lista_finalizadora]
(
	[operador] ASC,
	[pdv] ASC,
	[id_movimento] ASC,
	[Cancelado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO




GO
insert into Versoes_Atualizadas select 'Versão:1.297.868', getdate();