 
IF OBJECT_ID (N'Promocao', N'U') IS NULL 
begin
	CREATE TABLE [dbo].[Promocao](
		[Codigo] [int] IDENTITY(1,1) NOT NULL,
		[Tipo] [int] NOT NULL,
		[Inicio] [datetime] NOT NULL,
		[Fim] [datetime] NOT NULL,
		[Descricao] [varchar](50) NULL,
		[Param_Base] [float] NULL,
		[Param_Brinde] [float] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Codigo] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


IF OBJECT_ID (N'Promocao_base', N'U') IS NULL 
begin
	CREATE TABLE [dbo].[Promocao_base](
		[Codigo_promo] [int] NOT NULL,
		[Plu] [int] NULL
	) ON [PRIMARY]
end 

go
IF OBJECT_ID (N'Promocao_brinde', N'U') IS NULL 
begin

	CREATE TABLE [dbo].[Promocao_brinde](
		[Codigo_promo] [int] NOT NULL,
		[Plu] [int] NULL
	) ON [PRIMARY]
end
GO


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('filial') 
            AND  UPPER(COLUMN_NAME) = UPPER('pdv'))
begin
	alter table filial alter column pdv int
end
else
begin
	alter table filial add pdv int
end 
go 

update filial set pdv = 1

GO

 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_HISTORICO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO]
GO

CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO] (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(5),
		@finalizado varchar(5),
		@usuarioCancelamento varchar(20)
		)
		 AS
		 /* 
set @filial='MATRIZ'
set @comanda='1414'
set @dataInicio='20150901' 
set @datafim = '20160122'

set @horaInicio='00:00' 
set @horafim='23:59' 
-- set @status =''
set @usuario ='TODOS'
set @cancelamento='TODOS'
set @finalizado ='TODOS'
		
	*/
--sp_help comanda_item
--select * from Comanda_Item where data between '20150901' and '20160122'
select  i.Comanda, 
	   Data=convert(varchar,i.data,103),
	   Hora = convert(varchar,i.data,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.Usuario,
	   i.Plu,
	   m.Descricao,
	   i.Qtde,
	   i.Unitario,
	   i.Total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	   ,[Usuario Canc]= i.Usuario_Cancelamento  
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and  (len(@comanda)=0 or CONVERT(VARCHAR(5),i.comanda)=@comanda)  
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.data,108) between @horaInicio and @horafim
	  and (@usuario='TODOS' OR  i.usuario = @usuario ) 
	  and (@cancelamento='TODOS' OR  (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento ))
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And(@finalizado='TODOS' OR case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado)
	  and (@usuarioCancelamento = 'TODOS' OR i.Usuario_Cancelamento = @usuarioCancelamento)
order by i.data desc 
	  
	  
	  
go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0200]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0200]
GO
CREATE     procedure [dbo].[sp_EFD_PisCofins_0200]
	@Filial	AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8),
	@Tipo			AS Integer
AS
BEGIN
SELECT Distinct Plu into #0200Plu
					FROM SAIDA_ESTOQUE with (index (IX_Saida_Estoque))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND DATA_MOVIMENTO BETWEEN @DataInicio AND @DataFim AND data_cancelamento IS NULL 
						AND (
						(@TIPO =1 AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial and isnull(Controle_Filial_PDV.NGS,0) = 0 AND isnull(Controle_Filial_PDV.SAT,0) <> 1 ))
						OR @TIPO=2
						)
						
						
SELECT Distinct PLU into #0200NF_Item 
		FROM 
		NF_ITEM A with (index (IX_NF_Item_0200))
		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR AND A.CODIGO = B.CODIGO  
		INNER JOIN Natureza_operacao NatOp ON NatOp.Codigo_operacao=b.Codigo_operacao
WHERE
	A.FILIAL = @Filial
AND 
	DATA BETWEEN @DataInicio AND @DataFim
	AND (
				(@TIPO =1 AND  b.Tipo_NF=2 
					and (CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(B.Producao_NFe, 0) = 1 AND B.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 
					and a.codigo_operacao not in (6929,5929,5927,6927)
					and (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 ))
					)
				OR 
				@TIPO = 0 
			)

and ISNULL(b.nf_Canc,0)<>1

--AND 
--	EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR )
	
--SELECT Distinct PLU into #0200NF_ItemCliente 
--		FROM 
--		NF_ITEM A with (index (IX_NF_Item_0200))
--		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO --AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR 
--WHERE
--	A.FILIAL = @Filial
----AND
----	Not A.Codigo_Operacao in ('5102')	
--AND 
--	DATA BETWEEN @DataInicio AND @DataFim
--AND 
--	EXISTS(SELECT * FROM Cliente CI WHERE CI.Codigo_Cliente = B.CLIENTE_FORNECEDOR )


	SELECT
		REG = '0200',
		COD_ITEM = PLU,
		DESCR_ITEM = ISNULL(DESCRICAO,'PRODUTO'),
		COD_BARRA = ISNULL((SELECT TOP 1 EAN FROM Ean WHERE Ean.PLU = Mercadoria.PLU),''), 
		COD_ANT_ITEM = '',
		UNID_INV = CASE WHEN ISNULL(PESO_VARIAVEL, 'NÃO') = 'PESO' THEN 'KG' ELSE 'UN' END,
		TIPO_ITEM = '00',
		COD_NCM = CASE WHEN LEN(RTRIM(LTRIM(REPLACE(CF,'.','')))) = 8 THEN RTRIM(LTRIM(REPLACE(CF,'.',''))) ELSE '' END ,
		EX_IPI = '',
		COD_GEN = '',
		COD_LST = '',
		ALIQ_ICMS = (SELECT TOP 1 SAIDA_ICMS FROM Tributacao WHERE Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao),
		CEST = 
		
		CASE WHEN (SELECT COUNT(*) FROM CEST WHERE CEST.CEST = MERCADORIA.CEST and cest.NCM = mercadoria.cf ) > 0 
		AND LEN(RTRIM(LTRIM(CONVERT(VARCHAR, ISNULL(Mercadoria.CEST, 0))))) = 7 
		THEN CONVERT(VARCHAR, Mercadoria.CEST)  ELSE '' END --CASE WHEN Isnull(Cest,'') = Isnull((Select Max(CEST) From CEST where cest.CEST = Mercadoria.CEST),0) Then Isnull(Cest,'') else '' end
	FROM
		MERCADORIA with (index (PK_Mercadoria))
	Where
		mercadoria.plu in (Select PLU From #0200Plu p )
	Or	
		mercadoria.plu in (Select PLU From #0200NF_Item i)	
	--Or	
		--Exists(Select * From #0200NF_ItemCliente ci Where ci.Plu = mercadoria.Plu)			
		
		/*
	WHERE 
		EXISTS(SELECT * 
					FROM 
					NF_ITEM A 
					INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR
					WHERE
						A.FILIAL = @FILIAL
						AND DATA BETWEEN @DATAINICIO AND @DATAFIM
						AND MERCADORIA.PLU = A.PLU AND EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR ))
	Or 
		EXISTS(SELECT * 
					FROM SAIDA_ESTOQUE 
					WHERE FILIAL = @FILIAL
						AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial)
						AND DATA_MOVIMENTO BETWEEN @DATAINICIO AND @DATAFIM AND data_cancelamento IS NULL) --And Not PLU in(100100,99077))
						*/
	--left outer join mercadoria_loja on mercadoria.plu = mercadoria.loja.plu
	--OR
		--(IsNull(Saldo_Atual, 0) > 0 AND ISNULL(Preco_Custo, 0) > 0)
		
	ORDER BY CONVERT(FLOAT,MERCADORIA.PLU)
END

GO 
insert into Versoes_Atualizadas select 'Versão:1.250.789', getdate();