IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_NotasFiscais]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_NotasFiscais]
GO

--PROCEDURES =======================================================================================
CREATE    procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial           varchar(20),                -- Loja
      @DataDe           varchar(8),                 -- Data Inicial
      @DataAte          varchar(8),                 -- Data Fim
      @Tipo             varchar(20),                -- Tipo = 1 - Saidas, Tipo = 2 - Entrada
	  @Nota             varchar(20),                -- Nmero Nota Fiscal
	  @Fornecedor       varchar(20),                -- Nome Fornecedor
      @NF_CANC          varchar(20),		    -- Normal = NAO, Cancelada = SIM
      @plu			   varchar(20),
      @ean			   varchar(20),
      @ref             varchar(20),
      @descricao		varchar(40),
      @NF_Devolucao    varchar(20),
      @Excluir_Natureza varchar(20),
      @Incluir_Natureza varchar(20),
	  @notaProdutor    varchar(30)

As
begin
      DECLARE @NF varchar(5000)
      --exec sp_Rel_NotasFiscais    @Filial='MATRIZ', @datade = '20170101',  @dataate = '20180801',  @plu = '',  @ean = '',  @Ref = '',  @descricao = '',  @tipo = 'TODOS',  @Nota = '',  @fornecedor = '',  @NF_CANC = 'NAO',  @NF_Devolucao = 'NAO',  @Excluir_Natureza = '',  @Incluir_Natureza = '',  @notaProdutor = 'SIM' 

      IF LTRIM(RTRIM(@Nota)) <> ''
		begin
            SET @NF = 'Select isnull((
										Case When ISNULL(NF.Dest_Fornec,0) = 0 Then (
											Select Distinct Ltrim(Rtrim(Nome_Cliente)) 
											From Cliente c 
											where c.Codigo_Cliente = NF.Cliente_Fornecedor
											) 
										Else  
											NF.Cliente_Fornecedor 
										End
									  ),nf.cliente_fornecedor) Cliente_Fornecedor'
			SET @NF = @nf +',Plu = '+CHAR(39)+'PLU'+CHAR(39)+'+PLU'
			SET @NF = @nf +',Ref=CODIGO_REFERENCIA'
			SET @NF = @nf +',nf_item.Descricao'
			SET @NF = @nf +',nf_item.Qtde'
			SET @NF = @nf +',nf_item.Embalagem'
			SET @NF = @nf +',nf_item.Unitario'
			SET @NF = @nf +',nf_item.Total '
            SET @NF = @nf +' from NF_Item INNER JOIN NF '
			SET @NF = @nf +'     ON NF.CODIGO=NF_ITEM.CODIGO '
			SET @NF = @nf +'    and nf.Tipo_NF = nf_item.Tipo_NF '
			SET @NF = @nf +'    and nf.Cliente_Fornecedor= nf_item.Cliente_Fornecedor '
			SET @NF = @nf +'INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '
			SET @NF = @nf+'	where nf.codigo = '+@nota
			
			IF LTRIM(RTRIM(@Tipo)) = '1-Saida'
				SET @NF = @NF + 'AND NF.Tipo_NF = 1 '
			IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'
				SET @NF = @NF + 'AND NF.Tipo_NF = 2 '
			IF LTRIM(RTRIM(@Fornecedor)) <> ''
                SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

		end
	else
	begin
     

      SET @NF = 'SELECT NF.Filial'
	  SET @NF = @NF + ',Tipo= case when nf.tipo_nf=1 then '+CHAR(39)+'SAIDA'+CHAR(39)+' else '+CHAR(39)+'ENTRADA'+CHAR(39)+' END'
	  SET @NF = @NF + ',Emissao= Convert(Varchar,NF.Emissao,103) '
	  SET @NF = @NF + ',[NF Produtor]= '+CHAR(39)+'SFT_'+CHAR(39)+'+ISNULL(nf.CodigoNotaProdutor,'+CHAR(39)+CHAR(39)+') '
	  SET @NF = @NF + ',Nota= '+CHAR(39)+'SFT_'+CHAR(39)+'+NF.Codigo '
	  SET @NF = @NF + ',[Chave NFE] = NF.ID '
	  SET @NF = @NF + ',Destinatario = isnull((
											Case When ISNULL(NF.Dest_Fornec,0) = 0  Then (
												Select Distinct Ltrim(Rtrim(Nome_Cliente)) 
												 From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor
											 ) 
											 Else  
												NF.Cliente_Fornecedor 
											End
										),nf.cliente_fornecedor
									) '
	  SET @NF = @NF + ',CNPJ = isnull((
											Case When ISNULL(NF.Dest_Fornec,0) = 0  Then (
												Select Distinct Ltrim(Rtrim(CNPJ)) 
												 From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor
											 ) 
											 Else  (
												Select Distinct Ltrim(Rtrim(CNPJ)) 
												 From FORNECEDOR  where FORNECEDOR = NF.Cliente_Fornecedor
												 )
											End
										),nf.cliente_fornecedor
									) '
      SET @NF = @NF + ',Entrada= Convert(Varchar,NF.Data,103) '
      SET @NF = @NF + ',[Vlr Prod]= Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) '
      SET @NF = @NF + ',[Vlr Nota]= NF.Total'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '
      SET @NF = @NF + '							  AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor  '
	  SET @NF = @NF + '							  AND NF.Filial = NF_Item.Filial '
	  SET @NF = @NF + '              INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '
      
      SET @NF = @NF + 'AND Not Exists(Select * From nf_inutilizadas i Where Convert(varchar,i.N_Inicio) >= NF.Codigo And Convert(varchar,i.N_Fim) <= NF.Codigo) '
      
      --Exibe todas as notas fiscais (canceladas)
      IF LTRIM(RTRIM(@NF_CANC)) = 'SIM'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 1 '
            

      --Exibe todas as notas fiscais (exceto canceladas)     
      IF LTRIM(RTRIM(@NF_CANC)) = 'NAO'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 0 '
      

      --Verifica se sera aplicado filtro por Filial

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 


 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 
	  begin
		if(LEN(@plu)>0)
		 SET @NF = @NF + ' and nf_item.plu='+CHAR(39)+@plu+CHAR(39)	
	  end
		
	
		if(@NF_Devolucao='SIM')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 1 '
		end
		if(@NF_Devolucao='NAO')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 0 '
		end
		
		if(LEN(@Excluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO NOT IN('+@Excluir_Natureza+') '
		end
		
		if(LEN(@Incluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO IN('+@Incluir_Natureza+') '
		end
		
		if(@notaProdutor <> 'TODOS')
		BEGIN 
			IF(@notaProdutor ='SIM')
			BEGIN 
				SET @NF = @NF + ' and ISNULL(CodigoNotaProdutor,'+CHAR(39)+CHAR(39)+') <> '+CHAR(39)+CHAR(39)
			END 
			ELSE 
			BEGIN
				SET @NF = @NF + ' and ISNULL(CodigoNotaProdutor,'+CHAR(39)+CHAR(39)+') = '+CHAR(39)+CHAR(39)
			END
		END 

      SET @NF = @NF + ' Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Total,ISNULL(nf.CodigoNotaProdutor,'+CHAR(39)+CHAR(39)+'),NF.ID,NF.Dest_Fornec '

      SET @NF = @NF + ' Order By NF.Filial, Convert(Varchar,NF.Data,103), NF.Codigo '
	end
     
	
  execute(@NF)
--print @nf


end



go


insert into Versoes_Atualizadas select 'Versão:1.207.689', getdate();
GO
