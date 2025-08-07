
CREATE TABLE FUNCOES_NAO_FISCAIS 
	(CODIGO INT,
     OPERACAO VARCHAR(20),
     CREDITO TINYINT
	
	)
	
	

CREATE TABLE [dbo].[Tesouraria](
	[id] [bigint] NULL,
	[id_finalizadora] [int] NULL,
	[Total_Sistema] [decimal](18, 0) NULL,
	[Total_Entregue] [decimal](18, 0) NULL,
	[op_Financeira] [int] NOT NULL
) ON [PRIMARY]
