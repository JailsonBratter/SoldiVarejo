

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria_obs') 
            AND  UPPER(COLUMN_NAME) = UPPER('IndisponivelAte'))
begin
	alter table mercadoria_obs alter column IndisponivelAte DateTime
	alter table mercadoria_obs alter column Obrigatorio tinyint
end
else
begin
		
	alter table mercadoria_obs add IndisponivelAte DateTime
	alter table mercadoria_obs add Obrigatorio tinyint

end 
go 


/****** Object:  StoredProcedure [dbo].[sp_estoque_solicitacao_compra]    Script Date: 06/14/2017 10:52:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_estoque_solicitacao_compra]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_estoque_solicitacao_compra]
GO



/****** Object:  StoredProcedure [dbo].[sp_estoque_solicitacao_compra]    Script Date: 06/14/2017 10:52:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE  procedure [dbo].[sp_estoque_solicitacao_compra](
            @FILIAL VARCHAR(30),
            @plu varchar(max),
            @ean varchar(20),
            @descricao varchar(50),
            @ncm varchar(20),
            @campo1 varchar(20),
            @onde varchar(5),
            @campo2 varchar(20),
            @refForn varchar(30),
            @grupo varchar(40),
            @subgrupo varchar(50),
            @departamento varchar(50),
            @familia varchar(50),
            @tipoData Varchar(30),
            @dtDe Varchar(15),
            @dtAte Varchar(15),
            @ordem varchar(30),
            @prazo int,
            @conEmbalagem int,
            @ultimo_fornec varchar(40)
            
           
           
)
as

-- execute sp_estoque_solicitacao_compra 'MATRIZ','28755,27161' ,'','','','','','','','','','','','','','',null,0,0,''
           

begin
        Declare @strSqlItens  As nVarchar(MAX)
    
         
        SET @strSqlItens= 'Select  a.plu as PLU,b.EAN,REF =ISNULL(a.Ref_Fornecedor,'+ CHAR(39) ++ CHAR(39) +'), a.DESCRICAO,UN=A.UND  ,EMB =A.EMBALAGEM , SALDO=ISNULL(l.Saldo_atual,0), PRC_CUSTO= l.Preco_Custo,PRC_VENDA=l.Preco , COB_CAD = ISNULL(A.COBERTURA,0) '
        SET @strSqlItens= @strSqlItens+' INTO ##ITENS from mercadoria a  LEFT join ean b on a.plu=b.plu  inner join mercadoria_loja l on l.plu=a.plu    inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao '
        SET @strSqlItens= @strSqlItens+' left join W_BR_CADASTRO_DEPARTAMENTO d on (a.codigo_departamento= d.codigo_Departamento and a.filial=l.filial) left join Familia f on a.Codigo_familia = f.Codigo_familia '
        if(LEN(@ultimo_fornec)>0)
        begin
			SET @strSqlItens= @strSqlItens+' inner join  fornecedor_mercadoria fmerc on (a.plu= fmerc.plu and a.filial=fmerc.filial)'
        end
        
        SET @strSqlItens= @strSqlItens+' WHERE L.FILIAL = '+ CHAR(39) + @FILIAL+ CHAR(39) + ' and a.inativo<>1 '
		if(LEN(@plu)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu in ( ' + @plu +')'
        end
        if(LEN(@ean)>0)
        
        begin
            SET @strSqlItens= @strSqlItens+' and b.ean= '+ CHAR(39) + @ean+ CHAR(39)
        end
        if(LEN(@descricao)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.descricao like '+ CHAR(39)+'%' + @descricao+'%' + CHAR(39)
        end
        if(LEN(@campo1)>0 and LEN(@campo2)>0 and len(@onde)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and ' + @campo1+ ' ' + @onde+ ' ' + @campo2;
        end
        if(LEN(@ncm)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and cf= '+ CHAR(39) + @ncm+ CHAR(39)
        end
        if(LEN(@grupo)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_grupo = '+ CHAR(39) + @grupo+ CHAR(39)
        end
        if(LEN(@subgrupo)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_subgrupo = '+ CHAR(39) + @subgrupo+ CHAR(39)
        end
       
        if(LEN(@departamento)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and d.Descricao_departamento = '+ CHAR(39) + @departamento+ CHAR(39)
        end
        if(LEN(@familia)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and f.descricao_familia = '+ CHAR(39) + @familia+ CHAR(39)
        end
        if(LEN(@tipoData)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and '+@tipoData+' between '+ CHAR(39) + @dtDe+ CHAR(39)+ ' and ' + CHAR(39) + @dtAte+ CHAR(39)
        end
        if(LEN(@refForn)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and  a.Ref_Fornecedor LIKE  '+ CHAR(39) + @dtDe+'%' +CHAR(39)
        end
        if(LEN(@ultimo_fornec)>0)
        begin
			SET @strSqlItens= @strSqlItens+' and fmerc.fornecedor='+ CHAR(39) +@ultimo_fornec+ CHAR(39) 
        end
        
       
       --PRINT(@strSqlItens) 
        
         EXECUTE (@strSqlItens)
       
         SELECT ##ITENS.*,MES5=AG.VQMES05,MES4=AG.VQMES04,MES3=AG.VQMES03,MES2=AG.VQMES02,ULT_30D=ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S (INDEX=IX_SAIDA_ESTOQUE_01) WHERE S.FILIAL = @FILIAL AND S.PLU = ##ITENS.PLU  AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102)BETWEEN CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)and CONVERT(VARCHAR, GETDATE(),102) AND S.DATA_CANCELAMENTO IS NULL ),0),VDA_MED =CONVERT(NUMERIC(12,2),0.00), COB_DIAS=CONVERT(NUMERIC(12,2),0.00),SUG_UNID=CONVERT(NUMERIC(12,2),0.00),QTDE_COMPRA=CONVERT(NUMERIC(12,2),0.00)
         INTO ##QFinal FROM ##ITENS INNER JOIN ACUMULADO_GERAL AG (INDEX=PK_Acumulado_Geral) ON ##ITENS.PLU=AG.PLU AND AG.FILIAL=@FILIAL
        
         update ##QFinal set vda_med = CONVERT(NUMERIC(12,2),(mes5+mes4+mes3+mes2+ult_30d)/5,2) 
		 where (mes5+mes4+mes3+mes2+ult_30d)>0;
		 
		 update ##QFinal set COB_DIAS =  case when saldo <0 then CONVERT(NUMERIC(12,2),0.00) else  CONVERT(NUMERIC(12,2),( CONVERT(NUMERIC(12,2),SALDO) /(VDA_MED/30)))end 
				,sug_unid =  round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) 
				,qtde_compra = round(round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) /case when @conEmbalagem =1 then emb else 1 end  ,0)*
								case when @conEmbalagem =1 then emb else 1 end
		 where  Saldo <>0 and  VDA_MED <>0 and emb >0;
        
        
        
        SET @strSqlItens= ' Select PLU,EAN,REF,DESCRICAO, '
		SET @strSqlItens= @strSqlItens+ '	CTR =ISNULL(( '
		SET @strSqlItens= @strSqlItens+	'	Select '+CHAR(39)+'C'+CHAR(39)
		SET @strSqlItens= @strSqlItens+ '		from Contrato_fornecedor_item as cfi '
        SET @strSqlItens= @strSqlItens+ '            	INNER JOIN Contrato_fornecedor AS  CF ON CFI.id_contrato =CF.id_contrato '
        SET @strSqlItens= @strSqlItens+ '              inner join  Contrato_Fornecedor_Filial cff  on cf.id_contrato = cff.id_contrato   '
        SET @strSqlItens= @strSqlItens+ '             where cff.Filial ='+CHAR(39)+@FILIAL+CHAR(39)+' AND CF.data_validade >= GETDATE() AND CFI.plu=##QFinal.plu '			
		SET @strSqlItens= @strSqlItens+ '	),'+CHAR(39)+CHAR(39)+') '
        SET @strSqlItens= @strSqlItens+ ' ,UN,EMB,SALDO, '
        SET @strSqlItens= @strSqlItens+ 'PRC_CUSTO=('
        SET @strSqlItens= @strSqlItens+ 'isnull(('
        SET @strSqlItens= @strSqlItens+	'	Select top 1 CFI.vlr '
		SET @strSqlItens= @strSqlItens+ '		from Contrato_fornecedor_item as cfi '
        SET @strSqlItens= @strSqlItens+ '            	INNER JOIN Contrato_fornecedor AS  CF ON CFI.id_contrato =CF.id_contrato '
        SET @strSqlItens= @strSqlItens+ '              inner join  Contrato_Fornecedor_Filial cff  on cf.id_contrato = cff.id_contrato   '
        SET @strSqlItens= @strSqlItens+ '             where cff.Filial ='+CHAR(39)+@FILIAL+CHAR(39)+' AND CF.data_validade >= GETDATE() AND CFI.plu=##QFinal.plu '			
		
		SET @strSqlItens= @strSqlItens+ '	),PRC_CUSTO)'
        SET @strSqlItens= @strSqlItens+ '),'
        SET @strSqlItens= @strSqlItens+ 'PRC_VENDA,COB_CAD,MES5=CONVERT(INT,MES5),MES4=CONVERT(INT,MES4),MES3=CONVERT(INT,MES3),MES2=CONVERT(INT,MES2),ULT_30D= CONVERT(INT,ULT_30D),VDA_MED=CONVERT(INT,VDA_MED),COB_DIAS=CONVERT(INT,COB_DIAS),SUG_UNID= CONVERT(INT,SUG_UNID),QTDE_COMPRA=CONVERT(INT,QTDE_COMPRA) from ##QFinal  '
         --SET @strSqlItens= @strSqlItens+' where sug_unid >0  '
         if(LEN(@ordem)>0)
			begin
				SET @strSqlItens= @strSqlItens+' order by '+@ordem
			end
          EXECUTE (@strSqlItens)
        
        
         drop table ##ITENS
         drop table ##Qfinal
      
	      
end



GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_m_Local_Comanda]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_m_Local_Comanda]
GO

CREATE FUNCTION [dbo].[F_m_Local_Comanda](@COMANDA VARCHAR(10))
	RETURNS VARCHAR(3)
AS
	BEGIN

		DECLARE @RETORNO     VARCHAR(3)
	
		SELECT TOP 1 @RETORNO = CONVERT(VARCHAR(5), ISNULL(Comanda_Item.Localizacao, 0)) FROM Comanda_Item WHERE
		Comanda_Item.Comanda = @COMANDA
		AND Comanda_Item.Origem  = 'MBL'
		AND Comanda_Item.Cupom = 0
		AND Data_Cancelamento IS NULL
		ORDER BY Data DESC
		IF @RETORNO = ''
			SET @RETORNO = '0'

		RETURN @RETORNO

	END


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_m_Comandas_Local]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_m_Comandas_Local]
GO

CREATE FUNCTION [dbo].[F_m_Comandas_Local](@LOCAL INTEGER)
	RETURNS VARCHAR(100)
AS
	BEGIN

	DECLARE @RETORNO     VARCHAR(100)
	DECLARE @Comanda	VARCHAR(10)

	SET @RETORNO = ''

	DECLARE x_cur CURSOR FOR 
			SELECT DISTINCT Comanda_Item.comanda FROM Comanda_Item 
					WHERE ISNULL(Comanda_Item.Localizacao, 0) = @LOCAL AND Comanda_Item.origem = 'MBL' AND Comanda_Item.Cupom = 0 AND Comanda_Item.data_cancelamento is null
					ORDER BY 1
		open x_cur

		FETCH NEXT FROM x_cur
		 INTO @comanda

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @RETORNO = @RETORNO + @Comanda + ','
	
			FETCH NEXT FROM x_cur
			 INTO @comanda
		END

		close x_cur
		deallocate x_cur
		IF LEN(@RETORNO) > 1
			SET @RETORNO = SUBSTRING(@RETORNO, 1, LEN(@RETORNO) - 1)
		
		RETURN @RETORNO
	END


GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_insere_item_comanda]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_m_insere_item_comanda]
GO

CREATE PROCEDURE [dbo].[sp_m_insere_item_comanda]
 @comanda varchar(7), @filial varchar(20), @plu varchar(17), @usuario varchar(20), @data datetime, @localizacao decimal(5), @qtde decimal(9,3), @unitario decimal(12,2), @total decimal(12,2), @status tinyint, @cData varchar(6) 
as

declare @itemid int,
 @resp varchar(3),
 @idSpool int,
 @ins_rr int

select @itemid = isnull(max(id),0)+1 from comanda_item  where  comanda = @comanda and cupom = 0 and filial = @filial

BEGIN TRANSACTION

insert into comanda_item (comanda, filial, plu, origem, usuario, data,  localizacao, qtde, unitario, total, status, id, cupom, pdv)
 values (@comanda, @filial, @plu, 'TM1', @usuario, GETDATE(),  @localizacao, @qtde, @unitario, @total, @status, @itemid, 0, 0) 

SET @ins_rr = @@ERROR

if @ins_rr = 0
begin
	COMMIT TRANSACTION

	--select @idSpool = isnull(max(id),0) + 1 from spool
	--insert into spool (id, filial, comanda, data, plu, qtde, imp, loc, vendedor, descricao, id_m) values(@idSpool, @filial, @comanda, getdate(), @plu, @qtde, 0, @localizacao, @usuario,'',@itemid)

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
end
else
begin
	ROLLBACK TRANSACTION
	select '0' RESP
end

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_Transfere_Comanda]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_m_Transfere_Comanda]
GO

CREATE procedure [dbo].[sp_m_Transfere_Comanda]
	@ComandaOrig bigint, 
	@ComandaDest bigint

as

declare @Local as Integer

Select  
	TOP 1 @Local = ISNULL(comanda_item.localizacao,0) 
	From Comanda_item 
	Where comanda = @ComandaDest and cupom = 0 and pdv = 0
	AND Comanda_Item.data_cancelamento IS NULL
	AND Comanda_Item.origem = 'MBL'
	Order By Comanda_Item.data DESC

begin
	

            update comanda_controle set status = '00' where comanda = @ComandaOrig
            update comanda_controle set status = '02' where comanda = @ComandaDest
	
            update comanda set comanda = @ComandaDest where comanda = @comandaOrig and cupom = 0 and pdv = 0
            update comanda_item set comanda = @ComandaDest, localizacao = @Local  where comanda = @ComandaOrig and cupom = 0 and pdv = 0
            update comanda_item_obs set comanda = @ComandaDest where comanda = @ComandaOrig and cupom = 0 and pdv = 0

            return 1

end

return 0


GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_m_grupo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_m_grupo]
GO



CREATE    PROCEDURE [dbo].[sp_m_grupo]
	@filial varchar(20),
	@tipoPizza varchar(20)=null
as
	select distinct top 24 codigo_grupo codigo, descricao_grupo descricao from grupo 
	where filial = @filial 
	--AND (@tipoPizza IS NULL  OR Descricao_Grupo like '%PIZZA%' )
	--AND Not codigo_grupo In (3)


GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_SoldiGusto_PizzaInteira]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Cons_SoldiGusto_PizzaInteira]
GO


CREATE PROCEDURE [dbo].[sp_Cons_SoldiGusto_PizzaInteira]
AS
	--sp_Cons_SoldiGusto_Grupos '001001001'
	SELECT 
		DISTINCT TOP 63 PLU, Descricao, Preco 
	FROM 
		Mercadoria 
	WHERE 
		Mercadoria.Codigo_Departamento = '006001001' 
		AND 
		CONVERT(NUMERIC, Mercadoria.PLU)  NOT BETWEEN 60000 AND 81999

	ORDER BY 2

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_SoldiGusto_PizzaVariacoes]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Cons_SoldiGusto_PizzaVariacoes]
GO

CREATE PROCEDURE [dbo].[sp_Cons_SoldiGusto_PizzaVariacoes]
	@Codigo	as Varchar(6)
AS
	--sp_Cons_SoldiGusto_PizzaVariacoes '70'
	SELECT DISTINCT TOP 63 PLU, Descricao, Preco FROM Mercadoria 
	WHERE SUBSTRING(Mercadoria.PLU, 1,2) = @Codigo
	AND LEN(RTRIM(LTRIM(CONVERT(VARCHAR(6), CONVERT(NUMERIC, Mercadoria.PLU))))) = 5  
	ORDER BY 2

GO




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Tipo') 
            AND  UPPER(COLUMN_NAME) = UPPER('Movimenta_Estoque_Item '))
begin
	alter table Tipo alter column Movimenta_Estoque_Item TINYINT 
end
else
begin
	alter table Tipo add Movimenta_Estoque_Item TINYINT DEFAULT 0
end 

go 



IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[utg_atualiza_saldo_atual_tipo]'))
DROP TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo]
GO

CREATE TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo] ON [dbo].[Mercadoria] 
--INSTEAD OF UPDATE
FOR UPDATE
AS

declare
  @filial varchar(20),
  @plu varchar(17),
  @qtdeSubtrai decimal(9,3),
  @Fator_Conv Decimal(9,3),
  @QtdeSoma Decimal(9,3),
  @Qtde Decimal(9,3)


declare x_update cursor for select DELETED.filial, Item.Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Item.fator_conversao 
   from DELETED
   inner join INSERTED
      on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)
       inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU 
       inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.Movimenta_Estoque_Item,0) = 1
       inner join Item ON Item.Plu = DELETED.PLU

Open x_update

FETCH NEXT FROM x_update
INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv


WHILE @@FETCH_STATUS = 0

BEGIN
       BEGIN
             if update(saldo_Atual)

                    SET @Qtde = @qtdeSubtrai + @QtdeSoma 

                    begin
                           update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu 
                           update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu and filial = @filial
                    end 
       end

FETCH NEXT FROM x_update
INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv
END
close x_update

deallocate x_update

GO





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.164.604', getdate();
GO