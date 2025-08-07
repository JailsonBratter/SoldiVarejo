
/****** Object:  UserDefinedFunction [dbo].[BarcodeCode128]    Script Date: 02/09/2017 14:14:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BarcodeCode128]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[BarcodeCode128]
GO


/****** Object:  UserDefinedFunction [dbo].[BarcodeCode128]    Script Date: 02/09/2017 14:14:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--select * from w_br_NF_DANFE
--select dbo.BarcodeCode128('35161219309183000150550010002277091111006129')
CREATE FUNCTION [dbo].[BarcodeCode128](@Barcode VARCHAR(255)	)
	RETURNS VARCHAR(255)
BEGIN
		SET @Barcode = RTRIM(LTRIM(@Barcode))
		-- Define the final holding place of our output string
		DECLARE @finalArray VARCHAR(255)
		
		IF (@Barcode IS NOT NULL)
			BEGIN
				-- Define variavel que recebera o valor do parametro
				DECLARE @asciiString VARCHAR(255)
				                       
				--SELECT @asciiString = '!"#$%&''()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~'
				SELECT @asciiString = 'Ã!"#$%&''()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~¿¡¬√ƒ≈∆«»… '

				-- Define the stop and start characters
				DECLARE @charToEncode		AS VARCHAR(255)
				DECLARE @charPos			AS INTEGER
				DECLARE @checkSum			as INTEGER
				DECLARE @strTemp			AS VARCHAR(255)
				DECLARE @chekDigit			AS VARCHAR(255)
				DECLARE @charVal			AS INTEGER
				DECLARE @CcharSET			AS VARCHAR(255)
				DECLARE @intFOR				AS INTEGER
				DECLARE @intFORATE			AS INTEGER
				DECLARE @Code128C			AS VARCHAR(255)
				
				SET @Code128C = ''
				SELECT @checksum = 105;

				SELECT @intFOR = 1
				SELECT @intFORATE = 44

				WHILE @intFOR <= @intFORATE
					BEGIN
						SET @charToEncode = SUBSTRING(@Barcode, @intFOR, 2)
						SET @charVal = CONVERT(NUMERIC, @charToEncode)
						SET @Code128C = @Code128C + SUBSTRING(@asciiString, @charVal + 1, 1)
						SET @intFOR = @intFOR + 2
					END

				SELECT @intFOR = 1
				WHILE @intFOR <= LEN(@Code128C)
					BEGIN
						SET @charToEncode = SUBSTRING(@Code128C, @intFOR, 1)
						SET @charVal = ASCII(@charToEncode)
						IF @charVal = 204 
							SET @charVal = 0
						ELSE
							IF @charVal >= 33 AND @charVal < 127 
								SET @checkSum = @checkSum + @intFOR * (@charVal - 32)
							ELSE
								SET @checkSum = @checkSum + @intFOR * (@charVal - 97)
						
						SET @intFOR = @intFOR + 1
					END
				SET @checkSum = @checkSum % 103
				SET @chekDigit = SUBSTRING(@asciiString, @checksum + 1, 1)
				SET @Code128C = CHAR(202) + @Code128C + @chekDigit + CHAR(203) + CHAR(205)
				SET @finalArray = @Code128C
			END

	-- The @final Array represents the barcode encoded string
	RETURN @finalArray
END


GO





/****** Object:  UserDefinedFunction [dbo].[F_FORMATAR_CPF_CNPJ]    Script Date: 02/09/2017 14:16:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_FORMATAR_CPF_CNPJ]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_FORMATAR_CPF_CNPJ]
GO


/****** Object:  UserDefinedFunction [dbo].[F_FORMATAR_CPF_CNPJ]    Script Date: 02/09/2017 14:16:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[F_FORMATAR_CPF_CNPJ](@DOCUMENTO NUMERIC(18,0),@TIPO_DOC INTEGER)
	RETURNS VARCHAR(18)
AS
	BEGIN

	  DECLARE @STDOCUMENTO VARCHAR(18)
	  DECLARE @RETORNO     VARCHAR(18)
  
	  IF (@TIPO_DOC = 1)
		  BEGIN
			SET @STDOCUMENTO = REPLICATE('0',14 - LEN(@DOCUMENTO)) + CONVERT(VARCHAR,@DOCUMENTO)
			SET @RETORNO = SUBSTRING(@STDOCUMENTO,1,2) + '.' + SUBSTRING(@STDOCUMENTO,3,3) + '.' + SUBSTRING(@STDOCUMENTO,6,3) + '/' + SUBSTRING(@STDOCUMENTO,9,4) + '-' + SUBSTRING(@STDOCUMENTO,13,2)
		  END
	  ELSE
		IF (@TIPO_DOC = 2)
			BEGIN
			  SET @STDOCUMENTO = REPLICATE('0',11-LEN(@DOCUMENTO)) + CONVERT(VARCHAR,@DOCUMENTO)
			  SET @RETORNO = SUBSTRING(@STDOCUMENTO,1,3) + '.' + SUBSTRING(@STDOCUMENTO,4,3) + '.' + SUBSTRING(@STDOCUMENTO,7,3) + '-' + SUBSTRING(@STDOCUMENTO,10,2)
			END
		RETURN @RETORNO
	END

GO


/****** Object:  UserDefinedFunction [dbo].[funChave]    Script Date: 02/09/2017 14:24:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[funChave]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[funChave]
GO

/****** Object:  UserDefinedFunction [dbo].[funChave]    Script Date: 02/09/2017 14:24:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE   Function [dbo].[funChave](
		@UF As varchar(2),
		@strAnoMes As varchar(4),
		@CNPJ As varchar(15), 
		@strModelo As varchar(2), 
		@strSerie As varchar(3),
                @nNF As varchar(9), 
		@tpEmi As varchar,
		@cNF As varchar(6))
Returns varchar(47) as
Begin
declare @strChave As varchar(47),
	@nFor    As Int,
	@nPeso   As Int,
	@nSoma   As int,
	@nResto  As Int,
	@nDV     As Int

	set @nSoma = 0
	set @nResto = 0
    	set @strChave = @UF + @strAnoMes + @CNPJ + @strModelo + replicate('0', 3 -len(@strSerie))+ @strserie + Replicate('0',9 - len( @nNF)) + @nNF + isnull(@tpEmi, '0') + replicate('0',8 - len( @cNF))+ @cNF
    	set @nPeso = 4
	set @nFor = 1
While @nFor < 44
    Begin	
	set @nSoma = @nSoma + convert(int, substring(@strChave, @nFor, 1))-- * @nPeso
        If @nPeso = 2 
           set @nPeso = 9
        Else
		Begin
	           set @nPeso = @nPeso - 1
	        End
	set @nFor = @nFor + 1
    End


    set @nResto = @nSoma % 11
    If @nResto = 1 Or @nResto = 0
        set @nDV = 0
    Else
	Begin
        	set @nDV = 11 - @nResto
    	End
    set @strChave =  @strChave + rtrim(ltrim(convert(varchar,@nDV)))
Return(@strChave)
End






GO




/****** Object:  UserDefinedFunction [dbo].[F_FORMATAR_IE]    Script Date: 02/09/2017 14:25:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_FORMATAR_IE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_FORMATAR_IE]
GO

/****** Object:  UserDefinedFunction [dbo].[F_FORMATAR_IE]    Script Date: 02/09/2017 14:25:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[F_FORMATAR_IE](@DOCUMENTO VARCHAR(20),@UF VARCHAR(2))
	RETURNS VARCHAR(20)
AS
	BEGIN

	  DECLARE @STDOCUMENTO VARCHAR(20)
	  DECLARE @RETORNO     VARCHAR(20)
  
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


GO



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('numero_protocolo'))
begin
	alter table nf alter column numero_protocolo varchar(20)
end
else
begin
		
		alter table nf add numero_protocolo varchar(20)

end 
go 



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CONTABILIDADE]') AND type in (N'U'))
DROP TABLE CONTABILIDADE
GO

CREATE TABLE CONTABILIDADE
(
	COD VARCHAR(10) NOT NULL,
	FILIAL  VARCHAR(20) NOT NULL,
	NOME_CONTABILISTA VARCHAR(50)NOT NULL,
	CPF VARCHAR(18),
	CRC VARCHAR(20),
	CNPJ VARCHAR(18) ,
	CEP VARCHAR(9) ,
	ENDERECO VARCHAR(60),
	NUMERO VARCHAR(10),
	COMPLEMENTO VARCHAR(30),
	BAIRRO VARCHAR(50),
	FONE VARCHAR(15),
	FAX VARCHAR(15),
	EMAIL VARCHAR(50),
	CODMUN VARCHAR(7)
);

ALTER TABLE CONTABILIDADE ADD  CONSTRAINT [PK_CONTABILIDADE] PRIMARY KEY CLUSTERED 
(
	[COD] ASC,
	[FILIAL] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Vers„o:1.154.589', getdate();
GO
