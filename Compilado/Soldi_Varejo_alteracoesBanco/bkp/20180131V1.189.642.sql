
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Descricao_Comercial'))
begin
	alter table mercadoria alter column Descricao_Comercial varchar(500)
end
else
begin
		
	alter table mercadoria add Descricao_Comercial varchar(500)

end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('IntegraWS'))
begin
	alter table mercadoria alter column IntegraWS tinyint

	
end
else
begin
		
	alter table mercadoria add IntegraWS tinyint
	

end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Ativo_Ecommerce'))
begin
	alter table mercadoria alter column Ativo_Ecommerce tinyint

end 
else
begin
	alter table mercadoria add Ativo_Ecommerce tinyint
end

go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Comanda_Item') 
            AND  UPPER(COLUMN_NAME) = UPPER('Motivo_Cancelamento'))
begin
	alter table Comanda_Item alter column Motivo_Cancelamento VARCHAR(30) DEFAULT ''
	
end
else
begin
		
	alter table Comanda_Item add Motivo_Cancelamento VARCHAR(30) DEFAULT ''
	

end 
go 


create table Motivo_Cancelamento
(Modulo Varchar(30), Motivo VARCHAR(30))


go 




/****** Object:  StoredProcedure [dbo].[sp_m_obs]    Script Date: 01/29/2018 11:18:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_obs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_m_obs]
GO



/****** Object:  StoredProcedure [dbo].[sp_m_obs]    Script Date: 01/29/2018 11:18:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_m_obs]
      @plu varchar(17), @filial varchar(20)
as
      select distinct a.obs observacao, a.plu_item_adc codigoCobranca, 
         PrcAd = ISNULL((select top 1 l.preco 
                             from mercadoria l 
                             where l.filial = @filial and l.plu = a.plu_item_adc), 0)  from mercadoria_obs a
            where a.plu = @plu and a.filial = @filial

GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_ProdutosAlterados]    Script Date: 01/29/2018 11:45:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_ProdutosAlterados]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_ProdutosAlterados]
GO

/****** Object:  StoredProcedure [dbo].[sp_Rel_ProdutosAlterados]    Script Date: 01/29/2018 11:45:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
CREATE      Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20),
                @Descricao              As Varchar(60) ,
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40)
				,@custoMargem			as varchar(40)
				,@LINHA VARCHAR(30)
				,@saldo varchar(14)


AS

Declare @String               As Varchar(max)
Declare @Where               As Varchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , '
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin

		SET @String = @String + 'CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO,'
		end		
		if(@custoMargem='CUSTO-MARGEM-VENDA' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
			set @String = @String + ' mercadoria_loja.PRECO_CUSTO,MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
			SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		end
	
	
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu '
		SET @String = @String + ' left join LINHA on mercadoria.Cod_Linha = LINHA.codigo_linha '
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'
		Set @Where = ' WHERE  (Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)+')'
        
         IF (LEN(@DESCRICAO) > 0)
		BEGIN
            SET @Where = @Where + ' AND ( mercadoria.DESCRICAO LIKE '+ CHAR(39) +'%'+ @DESCRICAO + '%'+CHAR(39)+')'

        END      
        
        
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
		
			Set @Where = @Where +' And (convert(date,Mercadoria.Data_Alteracao) BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)+')'
		end
	       
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)+')'

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND (e.ean = '+ CHAR(39) + @ean + CHAR(39)+')'

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)+')'

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)+')'

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)+')'

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)+')'

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end


		Begin
		IF(@saldo='Igual a Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual=0)'
		End
		
		Begin
		IF(@saldo='Menor que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual<0)'
		End
		
		Begin
		IF(@saldo='Maior que Zero')
			 SET @Where = @Where + ' 	and (mercadoria.saldo_atual>0)'
		End
		
		IF(LEN(@LINHA)>0 )
		Begin
			SET @Where = @Where + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
		End
	 

		--print(@String + @Where)
		EXECUTE(@String + @Where)

END

 


GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Comanda_Item') 
            AND  UPPER(COLUMN_NAME) = UPPER('OBs_Orig_PLU'))
begin
	alter table Comanda_Item alter column OBs_Orig_PLU Varchar(17)
	alter table Comanda_Item alter column Obs_Orig_ID int 

end
else
begin
		
	alter table Comanda_Item ADD OBs_Orig_PLU Varchar(17) default '', 
								 Obs_Orig_ID int default 0

end 
go 


go 




/****** Object:  StoredProcedure [dbo].[sp_m_insere_item_comanda_ss]    Script Date: 01/31/2018 11:20:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_insere_item_comanda_ss]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_m_insere_item_comanda_ss]
GO


CREATE PROCEDURE [dbo].[sp_m_insere_item_comanda_ss]
@comanda varchar(7), 
 @filial varchar(20), 
 @plu varchar(17), @usuario varchar(20), @data datetime, 
 @localizacao decimal(5), @qtde decimal(9,3), @unitario decimal(12,2), 
 @total decimal(12,2), @status tinyint, @cData varchar(6), @Loja TinyInt = 1,
@PLUOrigOBS varchar(17) = '',
@IDOrigOBS int = 0 
as

declare @itemid int,
@resp varchar(3)

select @itemid = isnull(max(id),0)+1 from comanda_item  where  comanda = @comanda and cupom = 0 and filial = @filial

insert into comanda_item (comanda, filial, plu, origem, usuario, data, localizacao, qtde, unitario, total, status, id, cupom, pdv, Loja, OBs_Orig_PLU, Obs_Orig_ID )
values (@comanda, @filial, @plu, 'TM1', @usuario, @data, @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0, @Loja, @PLUOrigOBS, @IDOrigOBS) 
-- Alterado em 01/06/2017 - Pra não trazer lançamento já finalizado
-- values (@comanda, @filial, @plu, 'MBL', @usuario, @data, @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0) 

declare @codf varchar(9),
@t varchar(12),
@q varchar(9)

select @codf = codigo from funcionario where nome = @usuario

select @t = cast(@unitario as varchar(12)), @q= cast(@qtde as varchar(9))

exec sp_ce_emap  @comanda, @cData, @codf, @plu, @t, @q, @resp output

if @resp = 0
begin
select cast(@itemid as varchar(3)) RESP
end
else
begin
select '0' RESP
end


GO


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('complemento_ent'))
begin
	alter table cliente alter column complemento_ent Varchar(50)

end
else
begin
		
	alter table cliente ADD complemento_ent Varchar(50) 
end 

go 




if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_KEY_A1')
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
           ('KCW_KEY_A1'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'DEF5E4D4-6EC2-4DC2-A875-7F31F15E07FA'
           ,'RAKUTEN CHAVE A1'
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


if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_KEY_A2')
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
           ('KCW_KEY_A2'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'4E62D844-BD62-4723-B214-A3F3DDD2CA21'
           ,'RAKUTEN CHAVE A2'
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



if not exists(select 1 from PARAMETROS where PARAMETRO='KCW_URL_SERVER')
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
           ('KCW_URL_SERVER'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'http://breeds.ecservice.rakuten.com.br/ikcwebservice/'
           ,'RAKUTEN URL SERVER'
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


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.189.642', getdate();
GO