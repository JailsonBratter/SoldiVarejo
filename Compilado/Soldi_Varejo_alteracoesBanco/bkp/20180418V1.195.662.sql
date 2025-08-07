
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Spool') 
            AND  UPPER(COLUMN_NAME) = UPPER('PDV_Origem'))
begin
	alter table Spool alter column PDV_Origem INTEGER
end
else
begin
	ALTER TABLE Spool ADD PDV_Origem INTEGER DEFAULT 0
	
end
GO 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Spool') 
            AND  UPPER(COLUMN_NAME) = UPPER('NaoImp'))
begin
	alter table Spool alter column NaoImp TINYINT
end
else
begin
	ALTER TABLE Spool ADD NaoImp TINYINT DEFAULT 0
	
end 
 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Comanda_Item_Obs') 
            AND  UPPER(COLUMN_NAME) = UPPER('PDV_Origem'))
begin
	alter table Comanda_Item_Obs alter column PDV_Origem INTEGER
	alter table Comanda_Item_Obs alter column Imp INTEGER
end
else
begin
	ALTER TABLE Comanda_Item_Obs ADD PDV_Origem INTEGER DEFAULT  0, Imp INTEGER DEFAULT 0
end 
go 





IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[w_br_spool_CP]'))
DROP VIEW [dbo].[w_br_spool_CP]
GO


CREATE VIEW [dbo].[w_br_spool_CP]
AS
SELECT     TOP (100) PERCENT a.id, a.filial, a.comanda, a.data, a.PLU AS codigo, b.PLU, b.Descricao_resumida AS descricao, a.qtde, a.IMP, ISNULL(a.loc, 0) AS loc, b.Codigo_departamento, 
                      ISNULL(l.Impressora_Remota, 0) AS impressora_remota, e.id_trm, a.vendedor, 0 AS erro, a.obs, a.id_m, a.delivery, e.Porta, b.Und, a.Loja, a.PDV_Origem
FROM         dbo.SPOOL AS a 
		LEFT OUTER JOIN
                          (SELECT DISTINCT Filial, PLU, Codigo_departamento, Descricao_resumida, ISNULL(und, 'UN') AS Und
                            FROM          dbo.Mercadoria
                            WHERE      (Inativo = 0)) AS b ON a.filial = b.Filial AND a.PLU = b.PLU 
                                                                               LEFT OUTER JOIN dbo.Spool_Loja_Impressoras AS l ON b.Codigo_departamento = l.Codigo_Departamento
                    INNER JOIN dbo.Spool_impressoras AS e ON e.impressora_remota = l.Impressora_Remota AND a.Loja = l.Loja
WHERE     (a.IMP = 0) AND (e.Ativo = 1) AND (ISNULL(a.NaoImp, 0) = 0)
AND      b.plu NOT IN(SELECT dbo.Spool_Loja_Impressoras.PLU FROM dbo.Spool_Loja_Impressoras WHERE dbo.Spool_Loja_Impressoras.Codigo_Departamento = '' AND dbo.Spool_Loja_Impressoras.Observacao = '' AND dbo.Spool_Loja_Impressoras.PLU <> '')
UNION ALL
SELECT     TOP (100) PERCENT a.id, a.filial, a.comanda, a.data, a.PLU AS codigo, b.PLU, b.Descricao_resumida AS descricao, a.qtde, a.IMP, ISNULL(a.loc, 0) AS loc, b.Codigo_departamento, 
                      ISNULL(l.Impressora_Remota, 0) AS impressora_remota, e.id_trm, a.vendedor, 0 AS erro, a.obs, a.id_m, a.delivery, e.Porta, b.Und, a.Loja, a.PDV_Origem
FROM         dbo.SPOOL AS a 
                                                                               LEFT OUTER JOIN
                          (SELECT DISTINCT Filial, PLU, Codigo_departamento, Descricao_resumida, ISNULL(und, 'UN') AS Und
                            FROM          dbo.Mercadoria
                            WHERE      (Inativo = 0)) AS b ON a.filial = b.Filial AND a.PLU = b.PLU 
                                                                               LEFT OUTER JOIN dbo.Spool_Loja_Impressoras AS l ON b.PLU = l.PLU
                    INNER JOIN dbo.Spool_impressoras AS e ON e.impressora_remota = l.Impressora_Remota AND a.Loja = l.Loja
WHERE     (a.IMP = 0) AND (e.Ativo = 1) AND (ISNULL(a.NaoImp, 0) = 0)
ORDER BY a.filial, a.comanda, impressora_remota, a.id



GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.195.662', getdate();
GO