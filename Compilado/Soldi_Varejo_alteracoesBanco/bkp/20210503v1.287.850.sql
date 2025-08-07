IF OBJECT_ID (N'Etiqueta', N'U') IS NULL
begin

CREATE TABLE [dbo].[Etiqueta](

      [PLU] [varchar](17) NOT NULL,

      [Quantidade] [int] NULL

) ON [PRIMARY]

 

ALTER TABLE [dbo].[Etiqueta] ADD  DEFAULT ((1)) FOR [Quantidade]

end

GO

insert into Versoes_Atualizadas select 'Versão:1.287.850', getdate();