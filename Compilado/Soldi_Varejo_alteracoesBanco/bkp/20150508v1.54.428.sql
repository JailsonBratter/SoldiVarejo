
CREATE TABLE [dbo].[nf_inutilizadas](
	[serie] [varchar](3) NULL,
	[N_inicio] [int] NULL,
	[N_fim] [int] NULL,
	[protocolo] [varchar](15) NULL,
	[justificativa] [varchar](255) NULL,
	[data] [datetime] NULL,
	[usuario] [varchar](50) NULL
) ON [PRIMARY]

GO

insert into PARAMETROS (PARAMETRO,PENULT_ATUALIZACAO,ULT_ATUALIZACAO,VALOR_ATUAL,POR_USUARIO_OK,PERMITE_POR_EMPRESA,GLOBAL,DESC_PARAMETRO)
			     values ('PED_N_PRECO_MENOR',GETDATE() ,GETDATE() ,'TRUE',0,0,0,'PEDIDO NÃO PERMITE PREÇO MENOR QUE CADASTRADO')

GO



