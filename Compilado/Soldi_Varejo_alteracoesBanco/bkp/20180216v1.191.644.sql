IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Curva_ABC]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_Curva_ABC]
GO

--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial         As Varchar(20),
                @DataDe         As Varchar(8),
                @DataAte        As Varchar(8),
                @nLinhas        As Integer,
                @Descricao      As Varchar(60),
                @Grupo          As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento	as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40),
                @ini_periodo	as varchar(5),
				@fim_periodo	as varchar(5)
                
AS
  Declare @String As nVarchar(2000)
  Declare @Where  As nVarchar(1024)
BEGIN
	
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And  Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And  data_cancelamento is null and (isnull(mercadoria.curva_a,0)=1 and isnull(mercadoria.curva_b,0)=1 and isnull(mercadoria.curva_c,0) = 1) '
		Set @Where = @Where + ' And  (saida_estoque.hora_venda between '+CHAR(39)+@ini_periodo +CHAR(39)+' and '+CHAR(39)+@fim_periodo+CHAR(39)+')'  
		
		
		--** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver, 
                --** o sistema faz um LIKE no campo descrição recebido no parametro.
                BEGIN
                               IF LEN(ISNULL(@DESCRICAO,'')) > 0 
                                               SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)
                END       
                --** Checa se o parametro @Grupo contem alguma informação. Se tiver, 
                
                BEGIN
                               IF LEN(ISNULL(@GRUPO,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_GRUPO='+ CHAR(39) + @GRUPO + CHAR(39) 
                               IF LEN(ISNULL(@subGrupo,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_SUBGRUPO='+ CHAR(39) + @subGrupo + CHAR(39) 
							   IF LEN(ISNULL(@Departamento,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_DEPARTAMENTO='+ CHAR(39) + @Departamento + CHAR(39) 
							   IF LEN(ISNULL(@Familia,'')) > 0 
                                               SET @Where = @Where + ' AND familia.DESCRICAO_familia='+ CHAR(39) + @Familia + CHAR(39) 
                
                END       
		
		Begin
			if LEN(ISNULL(@Ordem,'')) = 0 
			Set @Ordem ='Qtde'
		end
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                  if ISNULL(@nLinhas, 0) > 0 
                     SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' ROW_NUMBER() over(order by Convert(Numeric(10,3), Sum(Qtde)) desc) as RANK, Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,'
                SET @String = @String + ' [Custo] = MERCADORIA.PRECO_CUSTO, '
                SET @String = @String + ' [Margem] = MERCADORIA.MARGEM, '
                SET @String = @String + ' [Preco] = MERCADORIA.PRECO, '
                                 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                --SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Valor] = Convert(Numeric(12,2), Sum(isnull(VLR,0)-isnull(desconto,0))), '
                set @String = @String + ' [Rentabilidade] = Convert(Numeric(12,2),SUM((isnull(VLR,0)-isnull(desconto,0)) - (Qtde * isnull(Mercadoria.Preco_Custo,0))))'
  --              Set @String = @String + ', [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2),'
		--if(@Ordem='VALOR')
		--BEGIN
		--		 Set @String = @String + ' Sum(VLR-desconto)) / SELECT SUM(VLR)FROM ((select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' convert(numeric(12,2), (vlr-desconto)) AS VLR from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia) AS T'
		--END
		--ELSE if(@Ordem='QTDE')
		--BEGIN
		--		Set @String = @String + ' Sum(Qtde)) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' Sum(ISNULL(saida_estoque.Qtde,0)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		
		--ELSE if(@Ordem='RENTABILIDADE')
		--BEGIN
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		----** Inserindo Clausula Subquery
		--Set @String = @String + @Where + ' )  * 100)) + '+ char(39)+ '%'+ char(39)
        SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque  WITH (INDEX(Ix_Fluxo_Caixa)) ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
	
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao, MERCADORIA.PRECO_CUSTO,MERCADORIA.MARGEM,MERCADORIA.PRECO  Order by ['+@Ordem+'] Desc'
 
  --PRINT @STRING
                EXECUTE(@String)
END


go 
alter table mercadoria add CONSTRAINT DF_saldo_atual DEFAULT 0 FOR saldo_atual;
go 

ALTER TABLE [dbo].[Mercadoria_loja] ADD  CONSTRAINT [DF_loja_saldo_atual]  DEFAULT ((0)) FOR [saldo_atual]
GO





/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_nf]    Script Date: 03/07/2018 11:05:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Cliente_nf]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Cliente_nf]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_nf]    Script Date: 03/07/2018 11:05:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_Rel_Cliente_nf] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cnpj       as Varchar(20),
		@tipo       as varchar(20)
		as
BEGIN
	-- exec [sp_Rel_Cliente_nf] 'MATRIZ','20180301','20180307','49.930.514/0001-35','ANALITICO'
	
	IF(LEN(@CNPJ)>0)
	BEGIN 
		SET @CNPJ = LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(@CNPJ,'.',''),'/',''),'-','')))
	END 
	
	if(@tipo='ANALITICO')
	BEGIN
		Select 
			CONVERT(VARCHAR,NF.EMISSAO,103) EMISSAO,
			nf.Cliente_Fornecedor Cod,
			cliente.Nome_Cliente,
			Total = Sum(nf_item.Total) 
		 from NF inner join cliente on NF.Cliente_Fornecedor= cliente.Codigo_Cliente 
			     INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 		
		 where NF.Tipo_NF = 1 
			and (NF.Data between @DataDe and @DataAte )
			AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
			AND (LEN(@CNPJ)=0 OR LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(CLIENTE.CNPJ,'.',''),'/',''),'-','')))= @CNPJ)
		 group by NF.EMISSAO,nf.Cliente_Fornecedor,cliente.Nome_Cliente
		 ORDER BY CONVERT(VARCHAR,NF.EMISSAO,102) 
	END
	ELSE	
	BEGIN  
		Select 
			CONVERT(VARCHAR,NF.EMISSAO,103) EMISSAO,
			Cliente =(Select top 1 nome_fantasia from cliente where CNPJ = C.CNPJ),
			Total = Sum(nf_item.Total) 
		 from NF inner join cliente AS C on NF.Cliente_Fornecedor= C.Codigo_Cliente 
			     INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 		
		 where NF.Tipo_NF = 1 
			and (NF.Data between @DataDe and @DataAte )
			AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
			AND (LEN(@CNPJ)=0 OR LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(C.CNPJ,'.',''),'/',''),'-','')))= @CNPJ)
		 group by NF.EMISSAO,C.cnpj
		 ORDER BY CONVERT(VARCHAR,NF.EMISSAO,102) 
	END
END

GO






/****** Object:  StoredProcedure [dbo].[SP_REL_COMANDA_HISTORICO]    Script Date: 03/12/2018 11:07:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_VENDA]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[SP_REL_COMANDA_VENDA]

GO

--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_VENDA] (
		@filial varchar(30),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		@usuario varchar(20)
		)
		 AS


Declare @totalVenda Numeric(18,3)
	 
--set @dataInicio ='20170101';
--set @datafim = '20180122'

-- set @horaInicio='00:00:59' 
--set @horafim='23:59' 
		
-- drop table #tempCom
if(@usuario = 'TODOS')
BEGIN 
	select  
		   i.Usuario,
		   Qtde = sum(se.Qtde),
		   Total = Sum(se.vlr)
		   into #tempCom
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
			inner join saida_estoque se on se.documento = i.cupom and se.pedido =i.comanda and i.plu=se.plu
	where ISNULL(i.cupom,0) <>0
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and i.data_cancelamento IS NULL 
		  and se.data_cancelamento is null 
	group by i.usuario   
	order by    Sum(se.vlr) desc
	  
	 
	 Select @totalVenda = sum(total) from #tempCom
	 
	 
	 Select Usuario, Qtde, Total, [%] = convert(numeric(12,2), (total/@totalVenda)*100) from #tempCom
END	  
ELSE
BEGIN 
	select  
		Data=convert(varchar,i.data,103),
		Hora = convert(varchar,i.hora_evento,108),
		PLU = 'PLU'+i.Plu,
	    m.Descricao,
	    se.Qtde,
	    i.Unitario,
	    Total = se.vlr
	into #tempCom1
	from Comanda_Item i  WITH (INDEX(index_historico_comanda)) 
		  inner join mercadoria m on i.plu=m.PLU
		  inner join saida_estoque se on se.documento = i.cupom and se.pedido =i.comanda and i.plu=se.plu
	where ISNULL(i.cupom,0) <>0
		  AND I.USUARIO = @USUARIO	
		  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
		  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
		  and i.data_cancelamento IS NULL 
		  and se.data_cancelamento is null 
	order by se.vlr desc

	Select @totalVenda = sum(total) from #tempCom1
	 
	Select Data,Hora,Plu,Descricao,Qtde,Unitario,Total,[%] = convert(numeric(12,2), (total/@totalVenda)*100)  from #tempCom1

END	  
	  
	  
	  
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.191.644', getdate();
GO