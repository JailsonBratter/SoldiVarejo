CREATE TABLE [dbo].[Mercadoria_Media](

             [PLU] [varchar](17) NOT NULL,

              [NOME_ARQUIVO] [varchar](60) NOT NULL,   -- Nome do arquivo magento-1.9.X altera o nome ao subir o arquivo

             [TIPO] [varchar](17) NOT NULL,           -- extensão do arquivo ex: jpg

             [ORDEM] [INT] NOT NULL,                  -- do 0 (principal) crescente, a ordem que as imagens devem ser apresentadas no ecommerce

             [LINK] [text] NULL,                      -- caso trate-se de uma mídia externa como imagem ou vídeo.

             [BASE] [text] NOT NULL,                  -- imagem em base64 interna

CONSTRAINT [PK_Mercadoria_Media] PRIMARY KEY CLUSTERED

(

       [PLU] ASC,

       [NOME_ARQUIVO] ASC,

       [ORDEM] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

 

ALTER TABLE [dbo].[Mercadoria_Media] ADD  CONSTRAINT [DF_Mercadoria_Media_TIPO]   DEFAULT ('JPEG') FOR [TIPO]

GO

ALTER TABLE [dbo].[Mercadoria_Media] ADD  CONSTRAINT [DF_Mercadoria_Media_ID]   DEFAULT ((0)) FOR [ORDEM]

GO

 
go

if OBJECT_ID(N'plano_de_contas_contabil',N'U') is null
BEGIN
	create table plano_de_contas_contabil(
		id varchar(10),
		codigo_plano_pai varchar(30),
		codigo varchar(30),
		descricao varchar(50)
	);
END

go

GO

/****** Object:  StoredProcedure [dbo].[sp_rel_nf_entrada_imposto]    Script Date: 12/29/2021 11:38:28 ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

 

ALTER procedure [dbo].[sp_rel_nf_entrada_imposto](

                @FILIAL VARCHAR(40)

                ,@Datade VARCHAR(15)

                ,@Dataate varchar(15)

                ,@Fornecedor varchar(40)

                ,@TPData            Varchar(40)

)

as

begin

-- exec sp_rel_nf_entrada_imposto 'MATRIZ','20211101' , '20211130','', 'ENTRADA'

                Select Codigo

                                 ,Fornecedor = Cliente_Fornecedor

                                 ,Emissao = Convert(varchar,Emissao,103)

                                 ,PIS= ISNULL(pisv,0)

                                 ,COFINS=ISNULL(cofinsv,0)

                                 ,ICMS=ISNULL(ICMS_Nota,0)

                                 ,ICMS_ST= ISNULL(ICMS_ST,0) 

                                 ,IPI=ISNULL(IPI_Nota,0)

                                 ,Outras = Isnull(Despesas_financeiras,0)

                                 ,Total_produto = ISNULL(TOTAL_PRODUTO,0)

                                 ,Total

                from nf

                where filial =@filial

                AND  TIPO_NF =2

                AND  CASE WHEN @TPDATA = 'ENTRADA' THEN nf.Data else nf.emissao end between @Datade and @Dataate

                and isnull(nf_Canc,0) = 0

                and (len(@Fornecedor)=0 or Cliente_Fornecedor = @Fornecedor)

end
go

insert into Versoes_Atualizadas select 'Versão:1.307.886', getdate();