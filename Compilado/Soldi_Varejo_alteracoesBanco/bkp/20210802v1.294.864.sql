IF OBJECT_ID (N'autorizadora', N'U') IS NULL
begin
	Create Table autorizadora (
	id int,
	descricao varchar(50)
);
end

go
drop trigger utg_atualiza_saldo_atual_tipo
go 

/****** Object:  Trigger [dbo].[dtg_atualiza_saldo_atual]    Script Date: 06/28/2021 15:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  TRIGGER [dbo].[dtg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR DELETE 
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @dt_canc datetime,
 @tipo_p varchar(20)

--select @plu = plu, @filial = filial, @qtde = qtde, @dt_canc = data_cancelamento from DELETED

declare x_deleted cursor for select plu, filial, qtde, data_cancelamento from DELETED

open x_deleted

FETCH NEXT FROM x_deleted
 INTO @plu, @filial, @qtde, @dt_canc

 WHILE @@FETCH_STATUS = 0
 BEGIN
  select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
  if @tipo_p  != 'PRODUCAO'
  BEGIN
   if @dt_canc is null
   begin 
	update mercadoria_loja set saldo_atual = isnull(saldo_atual, 0) + @qtde where filial = @filial and plu = @plu
   end
   FETCH NEXT FROM x_deleted
    INTO @plu, @filial, @qtde, @dt_canc
  end
 END

close x_deleted
deallocate x_deleted

go

/****** Object:  Trigger [dbo].[itg_atualiza_saldo_atual]    Script Date: 06/28/2021 15:08:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER TRIGGER [dbo].[itg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR INSERT
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @plu_item varchar(17),
 @fator_conversao decimal(9,3),
 @tipo_p varchar(20)

select @plu = plu, @filial = filial, @qtde = qtde from INSERTED where data_cancelamento is null

select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
if @tipo_p  != 'PRODUCAO'
BEGIN
update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu and filial = @filial 
end
go
go

ALTER TRIGGER [dbo].[utg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR UPDATE
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @odt_canc datetime,
 @ndt_canc datetime,
 @tipo_p varchar(20)


declare x_update cursor for select DELETED.plu, DELETED.filial, DELETED.qtde, DELETED.data_cancelamento odt_canc, INSERTED.data_cancelamento ndt_canc 
   from DELETED
   inner join INSERTED
      on (DELETED.filial = INSERTED.filial and DELETED.data_movimento = INSERTED.data_movimento and DELETED.documento = INSERTED.documento and DELETED.origem = INSERTED.origem and DELETED.plu = INSERTED.plu and DELETED.caixa_saida = INSERTED.caixa_saida and DELETED.codigo_funcionario = INSERTED.codigo_funcionario and DELETED.ean = INSERTED.ean and DELETED.pedido = INSERTED.pedido)

open x_update

FETCH NEXT FROM x_update
 INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc

WHILE @@FETCH_STATUS = 0
BEGIN
 select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
 if @tipo_p  != 'PRODUCAO'
 BEGIN
  if update(data_cancelamento)
  begin
   if @odt_canc is null and @ndt_canc is not null 
   begin
    update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + @qtde where plu =  @plu and filial = @filial
   end 
  end
 end
 FETCH NEXT FROM x_update
  INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc 
END

close x_update

deallocate x_update

go
CREATE  TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo] ON [dbo].[Mercadoria_Loja]

--INSTEAD OF UPDATE

FOR UPDATE

AS

 

declare

  @filial varchar(20),

  @PLUOriginal Varchar(17),

  @plu varchar(17),

  @qtdeSubtrai decimal(9,3),

  @Fator_Conv Decimal(9,3),

  @QtdeSoma Decimal(9,3),

  @Qtde Decimal(9,3),

  @PLUVinculado VARCHAR(17),
  
  @ContemItem	int


 if UPDATE(Saldo_Atual)
	 BEGIN
 
		 BEGIN
			  SELECT 
				@PLUVinculado = ISNULL(Mercadoria.PLU_Vinculado, ''),
				@Fator_Conv = ISNULL(Mercadoria.fator_Estoque_Vinculado, 0),
				@PLUOriginal = DELETED.PLU,
				@ContemItem =  ISNULL(Tipo.Movimenta_Estoque_Item,0)
			  FROM 
				MERCADORIA INNER JOIN DELETED ON MERCADORIA.PLU = DELETED.PLU
				INNER JOIN Tipo ON Mercadoria.TIPO = Tipo.TIPO 
		 END 



		  if @PLUVinculado <>''

						BEGIN

						   declare x_updatem cursor for select DELETED.filial, @PLUVinculado, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, @Fator_Conv

						   from DELETED

						   inner join INSERTED

										 on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)

										  inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU

										  inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.PLUAssociado,0) = 1

						END

		  ELSE
				IF @ContemItem > 0

						BEGIN

						   declare x_updatem cursor for select DELETED.filial, Item.Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Item.fator_conversao

						   from DELETED

						   inner join INSERTED

										 on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)

										  inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU

										  inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.Movimenta_Estoque_Item,0) = 1

										  inner join Item ON Item.Plu = DELETED.PLU

						END
				ELSE
						BEGIN

						   declare x_updatem cursor for select DELETED.filial, DELETED.PLU AS Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, 1 AS fator_conversao

						   from DELETED

						   inner join INSERTED

										 on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)

										  inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU

						END


		Open x_updatem

	 

		FETCH NEXT FROM x_updatem

		INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv

	 

		WHILE @@FETCH_STATUS = 0

	 

		BEGIN

			   BEGIN

						SET @Qtde = @qtdeSubtrai + @QtdeSoma

 

						begin

							   update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu
							   
							   IF (@ContemItem > 0 or @PLUVinculado <> '')
									update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu and filial = @filial
								
							   IF @PLUOriginal <> @plu 
								   update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde) where plu =  @PLUOriginal


						end

			   end

	 

		FETCH NEXT FROM x_updatem

		INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv

	END

	close x_updatem

 

	deallocate x_updatem

 

END

GO


GO
/****** Object:  StoredProcedure [dbo].[sp_estoque_sugestao_compra]    Script Date: 07/08/2021 11:41:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER    procedure [dbo].[sp_estoque_sugestao_compra](
            @FILIAL VARCHAR(30),
            @plu varchar(20),
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
            @ultimo_fornec varchar(40),
            @marca varchar(100)
            
           
           
)
as
-- execute sp_estoque_sugestao_compra 'MATRIZ','','','','','','=','0','','','','','','','','',null,5,0,'', 'ROYAL CANIN'
           
begin
        Declare @strSqlItens  As nVarchar(1024)
        -- SELECT* FROM MERCADORIA sp_help fornecedor_mercadoria
        -- SELECT * FROM Acumulado_Geral
        --    SELECT CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)
         
        SET @strSqlItens= 'Select  a.plu as PLU,ean =(Select top 1 ean from ean where plu = a.plu),REF =ISNULL(a.Ref_Fornecedor,'+ CHAR(39) ++ CHAR(39) +'), a.DESCRICAO  ,EMB =A.EMBALAGEM , SALDO=ISNULL(l.Saldo_atual,0), PRC_CUSTO= l.Preco_Custo,l.margem,PRC_VENDA=l.Preco , COB_CAD = ISNULL(A.COBERTURA,0) '
        SET @strSqlItens= @strSqlItens+' INTO ##ITENS from mercadoria a  inner join mercadoria_loja l on l.plu=a.plu    inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao '
        SET @strSqlItens= @strSqlItens+' left join W_BR_CADASTRO_DEPARTAMENTO d on (a.codigo_departamento= d.codigo_Departamento and a.filial=l.filial) left join Familia f on a.Codigo_familia = f.Codigo_familia '
      
        
        
        SET @strSqlItens= @strSqlItens+' WHERE L.FILIAL = '+ CHAR(39) + @FILIAL+ CHAR(39) + ' and a.inativo<>1 '
		if(LEN(@plu)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu= '+ CHAR(39) + @plu+ CHAR(39)
        end
        if(LEN(@ean)>0)
        begin
            SET @strSqlItens= @strSqlItens+' and a.plu = (Select plu from ean where plu = '+ CHAR(39) + @ean+ CHAR(39)+' ) '
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
			SET @strSqlItens= @strSqlItens+' and a.plu in ( Select distinct fmerc.plu from fornecedor_mercadoria fmerc where fmerc.fornecedor='+ CHAR(39) +@ultimo_fornec+ CHAR(39) +' union  select distinct plu from mercadoria where ultimo_fornecedor ='+ CHAR(39) +@ultimo_fornec+ CHAR(39) + ')'
        end
        
        if(LEN(@marca)>0)
        begin
                SET @strSqlItens= @strSqlItens+' and a.marca LIKE ' + CHAR(39) + '%'+ @marca + '%' +CHAR(39)
        end
        
       
       --PRINT(@strSqlItens) 
         EXECUTE (@strSqlItens)
       
         SELECT ##ITENS.*,MES5=AG.VQMES05,MES4=AG.VQMES04,MES3=AG.VQMES03,MES2=AG.VQMES02,ULT_30D=ISNULL(
			isnull((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S with (INDEX=IX_SAIDA_ESTOQUE_01) WHERE S.FILIAL = @FILIAL AND S.PLU = ##ITENS.PLU  AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102)BETWEEN CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 102)and CONVERT(VARCHAR, GETDATE(),102) AND S.DATA_CANCELAMENTO IS NULL ),0)
			+Isnull((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL And I.FILIAL = @FILIAL AND I.PLU = ##ITENS.PLU AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = @FILIAL AND CONVERT(VARCHAR, N.DATA, 112) >= convert(varchar,GETDATE()-30,112) AND I.PLU =  ##ITENS.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0),0)
			,0),
			VDA_MED =CONVERT(NUMERIC(12,2),0.00), COB_DIAS=CONVERT(NUMERIC(12,2),0.00),SUG_UNID=CONVERT(NUMERIC(12,2),0.00),QTDE_COMPRA=CONVERT(NUMERIC(12,2),0.00)
			
         INTO ##QFinal 
		 FROM ##ITENS 
			INNER JOIN ACUMULADO_GERAL AG  with (  INDEX=PK_Acumulado_Geral) 
				ON ##ITENS.PLU=AG.PLU AND AG.FILIAL=@FILIAL
        
         update ##QFinal set vda_med = CONVERT(NUMERIC(12,2),(mes5+mes4+mes3+mes2+ult_30d)/5,2) 
		 where (mes5+mes4+mes3+mes2+ult_30d)>0;
		 
		 update ##QFinal set COB_DIAS =  case when saldo <0 then CONVERT(NUMERIC(12,2),0.00) else  CONVERT(NUMERIC(12,2),( CONVERT(NUMERIC(12,2),SALDO) /(VDA_MED/30)))end 
				,sug_unid =  round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) 
				,qtde_compra = round(round(((vda_med/30)*(cob_cad+@prazo))-case when saldo<0 then 0 else Saldo end,0) /case when @conEmbalagem =1 then emb else 1 end  ,0)*
								case when @conEmbalagem =1 then emb else 1 end
		 where  Saldo <>0 and  VDA_MED <>0 and emb >0;
        
        update ##QFinal  SET QTDE_COMPRA =0 WHERE QTDE_COMPRA <0
        
         SET @strSqlItens= ' Select top 1000 PLU,EAN,REF,DESCRICAO,EMB,SALDO,PRC_CUSTO,MARGEM,PRC_VENDA,COB_CAD,MES5=CONVERT(INT,MES5),MES4=CONVERT(INT,MES4),MES3=CONVERT(INT,MES3),MES2=CONVERT(INT,MES2),ULT_30D= CONVERT(INT,ULT_30D),VDA_MED=CONVERT(INT,VDA_MED),COB_DIAS=CONVERT(INT,COB_DIAS),SUG_UNID= CONVERT(INT,SUG_UNID),QTDE_COMPRA=CONVERT(INT,QTDE_COMPRA) from ##QFinal  '
         
         DECLARE @vSug varchar(5) 
         SELECT @vSug= VALOR_ATUAL FROM PARAMETROS WHERE PARAMETRO ='SUGESTAO_ZERADA';
         
         IF(@vSug <> 'TRUE')
         BEGIN 
			SET @strSqlItens= @strSqlItens+' where sug_unid >0  '
         END 
         
         
         if(LEN(@ordem)>0)
			begin
				SET @strSqlItens= @strSqlItens+' order by '+@ordem
			end
          EXECUTE (@strSqlItens)
        
        
         drop table ##ITENS
         drop table ##Qfinal
      
	      
end

go

/****** Object:  Index [ix_doc_Eletronico]    Script Date: 07/13/2021 09:21:03 ******/
CREATE NONCLUSTERED INDEX [ix_doc_Eletronico] ON [dbo].[Documento_Eletronico] 
(
	[Filial] ASC,
	[Data] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


insert into Versoes_Atualizadas select 'Versão:1.294.864', getdate();