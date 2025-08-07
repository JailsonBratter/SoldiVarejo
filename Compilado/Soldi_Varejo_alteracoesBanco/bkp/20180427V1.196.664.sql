/****** Object:  StoredProcedure [dbo].[sp_ins_Spool]    Script Date: 04/27/2018 11:00:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ins_Spool]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ins_Spool]
GO


/****** Object:  StoredProcedure [dbo].[sp_ins_Spool]    Script Date: 04/27/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[sp_ins_Spool] 
                @Filial AS VarChar(20),
                @Comanda AS BigInt,
                @Descricao As Varchar(30),
                @Data AS Datetime,
                @PLU AS Varchar(17),
                @Qtde As Decimal(9,3),
                @Imp As tinyint,
                @Loc As Decimal(5),
                @Vendedor As Varchar(15),
                @Id_m As tinyint,
                @NaoImp           As TinyInt,
                @PdvOrigem    As TinyInt           

As
                Declare @IDSeq As Decimal(5)

                SELECT @IDSeq = isnull(max(id),0)+1 from Spool WHERE Filial = @Filial
                
                if LEN(@Plu) > 6
                               Begin
                                               Set @PLU =(Select MAX(Plu) From ean Where EAN = @Plu)
                               End

                               INSERT INTO 
                                               Spool (
                                               id,
                                               Filial,
                                               Comanda,
                                               Descricao,
                                               Data,
                                               PLU,
                                               Qtde,
                                               Imp,
                                               Loc,
                                               Vendedor,
                                               Id_m,
                                               Loja,
                                               NaoImp,
                                               Pdv_Origem
                               ) VALUES (
                                               @IDSeq,
                                               @Filial,
                                               @Comanda,
                                               @Descricao,
                                               @Data,
                                               @PLU,
                                               @Qtde,
                                               0,
                                               @Loc,
                                               @Vendedor,
                                               @Id_m,
                                               1,
                                               0,
                                               @PdvOrigem 
                               )

GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.196.6624', getdate();
GO