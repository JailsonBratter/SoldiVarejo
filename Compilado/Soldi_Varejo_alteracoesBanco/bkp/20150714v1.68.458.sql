IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nfe_correcao]') AND type in (N'U'))
DROP TABLE [dbo].[nfe_correcao]
GO


/****** Object:  Table [dbo].[nfe_correcao]    Script Date: 07/14/2015 16:27:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[nfe_correcao](
	[codigo] [varchar](20) NULL,
	[correcao] [text] NULL,
	[seq] [varchar](3) NULL,
	[protocolo] [varchar](20) NULL,
	[usuario] [varchar](20) NULL,
	[data] [datetime] NULL,
	[filial] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


