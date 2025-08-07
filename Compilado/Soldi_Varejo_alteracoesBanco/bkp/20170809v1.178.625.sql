go

ALTER TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo] ON [dbo].[Mercadoria] 
--INSTEAD OF UPDATE
FOR UPDATE
AS

declare
  @filial varchar(20),
  @plu varchar(17),
  @qtdeSubtrai decimal(9,3),
  @Fator_Conv Decimal(9,3),
  @QtdeSoma Decimal(9,3),
  @Qtde Decimal(9,3)


declare x_updatem cursor for select DELETED.filial, Item.Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Item.fator_conversao 
   from DELETED
   inner join INSERTED
      on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)
       inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU 
       inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.Movimenta_Estoque_Item,0) = 1
       inner join Item ON Item.Plu = DELETED.PLU

Open x_updatem

FETCH NEXT FROM x_updatem
INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv


WHILE @@FETCH_STATUS = 0

BEGIN
       BEGIN
             if update(saldo_Atual)

                    SET @Qtde = @qtdeSubtrai + @QtdeSoma 

                    begin
                           update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu 
                           update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu and filial = @filial
                    end 
       end

FETCH NEXT FROM x_updatem
INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv
END
close x_updatem

deallocate x_updatem


go 

-- novos ========

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Spool') 
            AND  UPPER(COLUMN_NAME) = UPPER('delivery'))
begin
	alter table Spool alter column delivery Tinyint
end
else
begin
	ALTER TABLE Spool ADD delivery Tinyint
	
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Spool_impressoras') 
            AND  UPPER(COLUMN_NAME) = UPPER('porta'))
begin
	alter table Spool_impressoras alter column porta varchar(100)
end
else
begin
	ALTER TABLE Spool_impressoras ADD porta varchar(100)
	
end 
go 

GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Comanda_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('Loja'))
begin
	alter table Comanda_item alter column Loja Tinyint
end
else
begin
	ALTER TABLE Comanda_item ADD Loja Tinyint		
	
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Comanda_item_Obs') 
            AND  UPPER(COLUMN_NAME) = UPPER('Loja'))
begin
	alter table Comanda_item_Obs alter column Loja Tinyint
end
else
begin
	ALTER TABLE Comanda_item_Obs ADD Loja Tinyint
	
end 
go 
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Spool') 
            AND  UPPER(COLUMN_NAME) = UPPER('Loja'))
begin
	alter table Spool alter column Loja Tinyint
end
else
begin
	ALTER TABLE Spool ADD Loja Tinyint
	
end 
go 






IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_insere_item_comanda]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_m_insere_item_comanda]
GO

CREATE PROCEDURE [dbo].[sp_m_insere_item_comanda]
 @comanda varchar(7), @filial varchar(20), @plu varchar(17), @usuario varchar(20), @data datetime, @localizacao decimal(5), @qtde decimal(9,3), @unitario decimal(12,2), @total decimal(12,2), @status tinyint, @cData varchar(6), @Loja TinyInt = 1
as

declare @itemid int,
 @resp varchar(3),
 @idSpool int,
 @ins_rr int

select @itemid = isnull(max(id),0)+1 from comanda_item  where  comanda = @comanda and cupom = 0 and filial = @filial

BEGIN TRANSACTION

insert into comanda_item (comanda, filial, plu, origem, usuario, data,  localizacao, qtde, unitario, total, status, id, cupom, pdv, Loja)
 values (@comanda, @filial, @plu, 'TM1', @usuario, GETDATE(),  @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0, @Loja) 

SET @ins_rr = @@ERROR

if @ins_rr = 0
begin
	COMMIT TRANSACTION

	--select @idSpool = isnull(max(id),0) + 1 from spool
	--insert into spool (id, filial, comanda, data, plu, qtde, imp, loc, vendedor, descricao, id_m) values(@idSpool, @filial, @comanda, getdate(), @plu, @qtde, 0, @localizacao, @usuario,'',@itemid)

	declare @codf varchar(9),
	 @t varchar(12),
	 @q varchar(9)
 
	select @codf = codigo from funcionario where nome = @usuario

	select @t = cast(@unitario as varchar(12)), @q= cast(@qtde as varchar(9))

	exec sp_ce_emap  @comanda, @cData, @codf, @plu, @t, @q, @resp output

	if @resp = 0
	begin
	 select cast(@itemid as varchar(3)) RESP
	end
	else
	begin
	 select '0' RESP
	end
end
else
begin
	ROLLBACK TRANSACTION
	select '0' RESP
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_insere_item_comanda_ss]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_m_insere_item_comanda_ss]
GO


CREATE PROCEDURE [dbo].[sp_m_insere_item_comanda_ss]
 @comanda varchar(7), @filial varchar(20), @plu varchar(17), @usuario varchar(20), @data datetime, @localizacao decimal(5), @qtde decimal(9,3), @unitario decimal(12,2), @total decimal(12,2), @status tinyint, @cData varchar(6), @Loja TinyInt = 1 
as

declare @itemid int,
 @resp varchar(3)

select @itemid = isnull(max(id),0)+1 from comanda_item  where  comanda = @comanda and cupom = 0 and filial = @filial

insert into comanda_item (comanda, filial, plu, origem, usuario, data, localizacao, qtde, unitario, total, status, id, cupom, pdv, Loja)
 values (@comanda, @filial, @plu, 'TM1', @usuario, @data, @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0, @Loja) 
-- Alterado em 01/06/2017 - Pra não trazer lançamento já finalizado
-- values (@comanda, @filial, @plu, 'MBL', @usuario, @data, @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0) 

declare @codf varchar(9),
 @t varchar(12),
 @q varchar(9)
 
select @codf = codigo from funcionario where nome = @usuario

select @t = cast(@unitario as varchar(12)), @q= cast(@qtde as varchar(9))

exec sp_ce_emap  @comanda, @cData, @codf, @plu, @t, @q, @resp output

if @resp = 0
begin
 select cast(@itemid as varchar(3)) RESP
end
else
begin
 select '0' RESP
end



GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_inclui_obs]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_m_inclui_obs]
GO


CREATE PROCEDURE [dbo].[sp_m_inclui_obs]
 @plu varchar(17), @id int, @obs varchar(30), @filial varchar(20), @comanda varchar(7), @usuario varchar(20), @localizacao decimal(5), @cData varchar(6), @mod varchar(1), @qtde decimal(9,3), @Loja TinyInt = 1
as
 declare @pluad varchar(17),
  @unitario decimal(12,2),
  @t decimal(12,2),
  @data datetime

 select @pluad = isnull(case when plu_item_adc = '' then '0' else plu_item_adc end,'0') from mercadoria_obs where filial = @filial and plu = @plu and obs = @obs 

 insert into comanda_item_obs (comanda, cupom, pdv, filial, plu, id, obs, modificador, Loja) values(@comanda, 0, 0, @filial, @plu, @id, @obs, @mod, @Loja)

 if (@pluad != '0') and @mod = 'C'
 begin
  select @unitario = preco from mercadoria where plu = @pluad and filial = @filial
  select @data = getdate(), @t =  @qtde * @unitario
  exec sp_m_insere_item_comanda_ss  @comanda , @filial, @pluad, @usuario, @data, @localizacao, @qtde, @unitario, @t, 0, @cData, @Loja 
 end


GO


IF  not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Spool_Loja_Impressoras]') AND type in (N'U'))
begin
	CREATE TABLE [dbo].[Spool_Loja_Impressoras](
		[Loja] [tinyint] NULL,
		[Codigo_Departamento] [varchar](9) NULL,
		[Impressora_Remota] [tinyint] NULL,
		[PLU] [varchar](17) NULL,
		[Observacao] [varchar](20) NULL
	) ON [PRIMARY]
end
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[w_br_spool_CP]'))
	DROP VIEW [dbo].[w_br_spool_CP]
GO


CREATE VIEW [dbo].[w_br_spool_CP]
AS
SELECT     TOP (100) PERCENT a.id, a.filial, a.comanda, a.data, a.PLU AS codigo, b.PLU, b.Descricao_resumida AS descricao, a.qtde, a.IMP, ISNULL(a.loc, 0) AS loc, b.Codigo_departamento, 
                      ISNULL(l.Impressora_Remota, 0) AS impressora_remota, e.id_trm, a.vendedor, 0 AS erro, a.obs, a.id_m, a.delivery, e.Porta, b.Und, a.Loja
FROM         dbo.SPOOL AS a 
					LEFT OUTER JOIN
                          (SELECT DISTINCT Filial, PLU, Codigo_departamento, Descricao_resumida, ISNULL(und, 'UN') AS Und
                            FROM          dbo.Mercadoria
                            WHERE      (Inativo = 0)) AS b ON a.filial = b.Filial AND a.PLU = b.PLU 
					LEFT OUTER JOIN dbo.Spool_Loja_Impressoras AS l ON b.Codigo_departamento = l.Codigo_Departamento
                    INNER JOIN dbo.Spool_impressoras AS e ON e.impressora_remota = l.Impressora_Remota AND a.Loja = l.Loja
WHERE     (a.IMP = 0) AND (e.Ativo = 1)
AND	b.plu NOT IN(SELECT dbo.Spool_Loja_Impressoras.PLU FROM dbo.Spool_Loja_Impressoras WHERE dbo.Spool_Loja_Impressoras.Codigo_Departamento = '' AND dbo.Spool_Loja_Impressoras.Observacao = '' AND dbo.Spool_Loja_Impressoras.PLU <> '')
UNION ALL
SELECT     TOP (100) PERCENT a.id, a.filial, a.comanda, a.data, a.PLU AS codigo, b.PLU, b.Descricao_resumida AS descricao, a.qtde, a.IMP, ISNULL(a.loc, 0) AS loc, b.Codigo_departamento, 
                      ISNULL(l.Impressora_Remota, 0) AS impressora_remota, e.id_trm, a.vendedor, 0 AS erro, a.obs, a.id_m, a.delivery, e.Porta, b.Und, a.Loja
FROM         dbo.SPOOL AS a 
					LEFT OUTER JOIN
                          (SELECT DISTINCT Filial, PLU, Codigo_departamento, Descricao_resumida, ISNULL(und, 'UN') AS Und
                            FROM          dbo.Mercadoria
                            WHERE      (Inativo = 0)) AS b ON a.filial = b.Filial AND a.PLU = b.PLU 
					LEFT OUTER JOIN dbo.Spool_Loja_Impressoras AS l ON b.PLU = l.PLU
                    INNER JOIN dbo.Spool_impressoras AS e ON e.impressora_remota = l.Impressora_Remota AND a.Loja = l.Loja
WHERE     (a.IMP = 0) AND (e.Ativo = 1)
ORDER BY a.filial, a.comanda, impressora_remota, a.id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 298
               Bottom = 136
               Right = 506
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 6
               Left = 544
               Bottom = 126
               Right = 741
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 169
               Left = 285
               Bottom = 288
               Right = 479
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 21
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 11' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'w_br_spool_CP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'70
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'w_br_spool_CP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'w_br_spool_CP'
GO

GO
/****** Object:  Trigger [U_INSERE_SPOOL]    Script Date: 08/09/2017 10:19:54 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[U_INSERE_SPOOL]'))
DROP TRIGGER [dbo].[U_INSERE_SPOOL]
GO


CREATE TRIGGER [dbo].[U_INSERE_SPOOL] ON [dbo].[Comanda_item] 
FOR UPDATE
AS

declare  @idSpool int,
	@comanda varchar(7),
	@filial varchar(20),
	@plu varchar(17), 
	@usuario varchar(20), 
	@data datetime, 
	@localizacao decimal(5), 
	@qtde decimal(9,3), 
	@itemid int,
	@origem_a varchar(3),
	@origem_n varchar(3),
	@data_cancelamento datetime,
	@Loja Tinyint


declare x_cur cursor for 
	select INSERTED.comanda, INSERTED.filial, INSERTED.plu, INSERTED.usuario, INSERTED.data, INSERTED.localizacao, INSERTED.qtde, INSERTED.id, INSERTED.Loja  from INSERTED  
			Left Outer join DELETED on (INSERTED.comanda = DELETED.comanda and INSERTED.FILIAL = DELETED.FILIAL and INSERTED.plu = DELETED.plu and INSERTED.id = DELETED.id and INSERTED.Loja = DELETED.Loja) 
			INNER JOIN Mercadoria ON Mercadoria.PLU = INSERTED.PLU AND Mercadoria.Tipo <> 'KIT'
			where INSERTED.origem = 'MBL' and DELETED.ORIGEM = 'TM1' and inserted.data_cancelamento is null
	UNION ALL
	select INSERTED.comanda, INSERTED.filial, Item.plu_item, INSERTED.usuario, INSERTED.data, INSERTED.localizacao, INSERTED.qtde, INSERTED.id, INSERTED.Loja  from INSERTED  
			Left Outer join DELETED on (INSERTED.comanda = DELETED.comanda and INSERTED.FILIAL = DELETED.FILIAL and INSERTED.plu = DELETED.plu and INSERTED.id = DELETED.id and INSERTED.Loja = DELETED.Loja) 
			INNER JOIN Mercadoria ON Mercadoria.PLU = INSERTED.PLU AND Mercadoria.Tipo = 'KIT'
			INNER JOIN Item ON Item.PLU = Mercadoria.PLU
			where INSERTED.origem = 'MBL' and DELETED.ORIGEM = 'TM1' and inserted.data_cancelamento is null

open x_cur

FETCH NEXT FROM x_cur
 INTO @comanda, @filial, @plu, @usuario, @data, @localizacao, @qtde, @itemid, @Loja

WHILE @@FETCH_STATUS = 0
BEGIN
	
	select @idSpool = isnull(max(id),0) + 1 from spool
	insert into spool (id, filial, comanda, data, plu, qtde, imp, loc, vendedor, descricao, id_m, Loja) values(@idSpool, @filial, @comanda, getdate(), @plu, @qtde, 0, @localizacao, @usuario,'',@itemid, @Loja)
	
	
	FETCH NEXT FROM x_cur
	 INTO @comanda, @filial, @plu, @usuario, @data, @localizacao, @qtde, @itemid, @Loja
END

close x_cur
deallocate x_cur
GO





-- fim =======


ALTER FUNCTION [dbo].[F_FORMATAR_IE](@DOCUMENTO VARCHAR(20),@UF VARCHAR(2))
	RETURNS VARCHAR(20)
AS
	BEGIN

	  DECLARE @STDOCUMENTO VARCHAR(20)
	  DECLARE @RETORNO     VARCHAR(20)
      Set @DOCUMENTO = REPLACE(@documento,'.','')
		IF (@UF = 'SP' AND LEN(LTRIM(RTRIM(@DOCUMENTO))) >= 12)
			BEGIN
				SET @STDOCUMENTO = REPLICATE('0',12 - LEN(LTRIM(RTRIM(@DOCUMENTO)))) + LTRIM(RTRIM(@DOCUMENTO))
				SET @RETORNO = SUBSTRING(@STDOCUMENTO,1,3) + '.' + SUBSTRING(@STDOCUMENTO,4,3) + '.' + SUBSTRING(@STDOCUMENTO,7,3) + '.' + SUBSTRING(@STDOCUMENTO,10,3) 
			END
		ELSE
			BEGIN
				SET @STDOCUMENTO = RTRIM(LTRIM(@DOCUMENTO))
				SET @RETORNO = @STDOCUMENTO
			END
		RETURN @RETORNO
	END



go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Lista_finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('troco'))
begin
	alter table Lista_finalizadora alter column troco decimal(12,2)
end
else
begin
	alter table Lista_finalizadora add troco decimal(12,2)
end 
go 



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Resumo_Vendas_Cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[sp_rel_Resumo_Vendas_Cliente]
end
GO



CREATE PROCEDURE [dbo].[sp_rel_Resumo_Vendas_Cliente] (
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@Codigo_Cliente varchar(50),
@tipo varchar(20)
)
AS
--SP_REL_RESUMO_VENDAS_CLIENTE 'MATRIZ', '20170501', '20170731', '1','SINTETICO'
if(@tipo= 'SINTETICO') 
begin
	Select
	Nome_Cliente = c.nome_cliente,
		Emissao = CONVERT(VARCHAR, s.data_movimento, 103),
		Vlr = sum(s.vlr)
	FROM
		Saida_estoque s 
			INNER JOIN MERCADORIA m ON S.PLU = M.PLU 
			INNER JOIN CLIENTE C on c.codigo_cliente = s.codigo_cliente 
	WHERE
			s.Filial = @Filial 
		AND s.Data_Movimento between @DataDe and @DataAte
		and (len(@Codigo_Cliente)=0	 OR ((c.Codigo_Cliente = @Codigo_cliente OR replace(replace(replace(c.CNPJ,'.',''),'-',''),'/','') = replace(replace(replace(@Codigo_cliente,'.',''),'-',''),'/',''))) )
	GROUP BY 
		c.codigo_cliente, 
		c.nome_cliente,	
		s.data_movimento
	ORDER BY
		c.codigo_cliente,
		s.data_movimento;


end
else  
begin
	SELECT 
		Nome_Cliente = c.nome_cliente,
		Emissao = CONVERT(VARCHAR, s.data_movimento, 103),
		PDV = '.' + convert(varchar, s.Caixa_Saida),
		'.' + s.Documento as Documento,
		'.' + s.plu as PLU,
		m.descricao,
		Qtde = sum(s.qtde),
		Vlr = sum(s.vlr)
	FROM
		Saida_estoque s 
			INNER JOIN MERCADORIA m ON S.PLU = M.PLU 
			INNER JOIN CLIENTE C on c.codigo_cliente = s.codigo_cliente 
	WHERE
			s.Filial = @Filial 
		AND s.Data_Movimento between @DataDe and @DataAte
		and (len(@Codigo_Cliente)=0	 OR ((c.Codigo_Cliente = @Codigo_cliente OR replace(replace(replace(c.CNPJ,'.',''),'-',''),'/','') = replace(replace(replace(@Codigo_cliente,'.',''),'-',''),'/',''))) )
	GROUP BY 
		c.codigo_cliente, 
		c.nome_cliente, 
		s.data_movimento,
		s.Caixa_Saida,
		s.Documento,
		s.plu,
		m.descricao
	ORDER BY
		c.codigo_cliente,
		s.data_movimento, 
		s.caixa_saida, 
		s.documento;
end

GO





go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.178.625', getdate();
GO

