alter table tipo_pagamento add sem_pagamento tinyint
GO 

if not exists(select 1 from PARAMETROS where PARAMETRO='NF_EMISSAO_NATUREZA')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('NF_EMISSAO_NATUREZA'
           ,GETDATE()
           ,'5102'
           ,GETDATE()
           ,'5102'
           ,'NATUREZA PADRÃO EMISSAO DE NOTA'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_desconto_tabela_cliente]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_desconto_tabela_cliente]
END
GO
Create  procedure sp_rel_desconto_tabela_cliente (
	@filial as varchar(50),
	@datade varchar(8),
	@dataate varchar(8),
	@Codigo_Cliente varchar(50),
	@tipo varchar(20),
	@plu varchar(20)
)
as 
begin
-- sp_rel_desconto_tabela_cliente 'MATRIZ', '20191001','20191023','','SINTETICO',''
	if(@tipo='ANALITICO')
	BEGIN  
	Select c.Codigo_Cliente,c.Nome_Cliente,Tabela =Replace(t.Codigo_tabela,'PLU','PL U'),Bruto =sum(isnull(s.vlr,0)) ,[%]=convert(varchar,isnull(t.porc,0))+'%', Valor_Desconto = sum(isnull(s.desconto,0)) from tabela_preco as t
		inner join cliente as c on t.Codigo_tabela = c.Codigo_tabela
		left join saida_estoque as s on c.Codigo_Cliente = s.Codigo_Cliente
		WHERE s.Filial= @filial 
		and  s.Data_movimento between @datade and @dataate
		and (len(@plu) =0 or s.PLU = @plu)
		and (len(@Codigo_Cliente) =0 or c.Codigo_Cliente = @Codigo_Cliente)
		group by c.Codigo_Cliente,c.Nome_Cliente, t.Codigo_tabela, t.porc
		having sum(isnull(s.vlr,0)) >0
		ORDER BY T.Porc DESC ,C.Nome_Cliente  
		
	END 
	ELSE
	BEGIN
		
		Select Tabela =Replace(t.Codigo_tabela,'PLU','PL U'),Bruto =sum(isnull(s.vlr,0)),[%]=convert(varchar,isnull(t.porc,0))+'%' , Valor_Desconto = sum(isnull(s.desconto,0)) from tabela_preco as t
		inner join cliente as c on t.Codigo_tabela = c.Codigo_tabela
		left join saida_estoque as s on c.Codigo_Cliente = s.Codigo_Cliente
		where s.Filial= @filial 
		and  s.Data_movimento between @datade and @dataate
		and (len(@plu) =0 or s.PLU = @plu)
		and (len(@Codigo_Cliente) =0 or c.Codigo_Cliente = @Codigo_Cliente)
		group by t.Codigo_tabela, t.porc
		having sum(isnull(s.vlr,0)) >0
		ORDER BY isnull(t.porc,0) desc

	END 
end 
GO 
insert into Versoes_Atualizadas select 'Versão:1.255.796', getdate();