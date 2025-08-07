alter table fornecedor add produtor_rural tinyint;

go


alter table conta_a_receber add nota_servico tinyint 

go

alter table cheque add data_cadastro datetime

go 


CREATE TABLE [dbo].[Nota_Pedido](
	[Codigo_Pedido] [varchar](8) NULL,
	[Codigo_Nota] [varchar](20) NULL
) ON [PRIMARY]
go



go

alter table mercadoria add usuario varchar(40)

go


GO

/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 05/26/2015 14:50:14 ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

ALTER Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),

                @plu               As varchar(17),

                @cupom             As varchar(20),

                @pdv               as varchar(2)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR = convert(decimal(18,2),SUM(Lista_Finalizadora.Total )),

                             FINALIZADORA = id_finalizadora

                        FROM

                             Lista_finalizadora

                             INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE Lista_Finalizadora.Filial = @FILIAL And Isnull(Cancelado,0) = 0 AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                                   and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

           

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR = convert(decimal(18,2),SUM(Lista_Finalizadora.Total )),

                             FINALIZADORA = id_finalizadora

                        FROM

                             Lista_finalizadora

                             INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE Lista_Finalizadora.Filial = @FILIAL And Isnull(Cancelado,0) = 0 AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 

                         and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='')

BEGIN

      SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),

                        pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento  like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                        And s.Data_Cancelamento is null

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         and l.pdv like (case when @pdv <> '' then @pdv else '%' end)

                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo

      END

ELSE

      BEGIN

           

            SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),

                        pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null

                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)

                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo

                        union all

                        select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0))))

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END

 

 

 


 

go




GO

/****** Object:  StoredProcedure [dbo].[sp_rel_Resumo_Vendas]    Script Date: 05/27/2015 12:34:14 ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

 

--sp_rel_Resumo_Vendas 'MATRIZ', '20140701', '20140701'

 

 

ALTER         Procedure [dbo].[sp_rel_Resumo_Vendas]

            @Filial           As Varchar(20),

            @DataDe           As Varchar(8),

            @DataAte    As Varchar(8)

 

AS

 

      SELECT

            Filial,

            Data_Movimento,

            Caixa_Saida

            Documento,

            Vlr = Convert(Decimal(15,2),SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))),

            CPF_CNPJ

      INTO

            #Lixo

      FROM

            Saida_Estoque

      WHERE

            Filial = @Filial

      AND

            Data_Movimento BETWEEN @DataDe AND @DataAte

      AND

            Data_Cancelamento IS NULL

      and   hora_venda between '00:00:00' and '23:59:59'

      GROUP BY

            Filial,

            Data_Movimento,

            Caixa_Saida,

            Documento,

            CPF_CNPJ

 

     

      SELECT

      Data = Convert(Varchar,Data_Movimento,103),

      Dia_Semana = CASE

            WHEN DATEPART(dw, Data_Movimento) = 1 THEN 'DOMINGO'

            WHEN DATEPART(dw, Data_Movimento) = 2 THEN 'SEGUNDA'

            WHEN DATEPART(dw, Data_Movimento) = 3 THEN 'TERÇA'

            WHEN DATEPART(dw, Data_Movimento) = 4 THEN 'QUARTA'

            WHEN DATEPART(dw, Data_Movimento) = 5 THEN 'QUINTA'

            WHEN DATEPART(dw, Data_Movimento) = 6 THEN 'SEXTA'

            WHEN DATEPART(dw, Data_Movimento) = 7 THEN 'SABADO'

      END,

      --Qtde =        SUM(CASE WHEN Data_Cancelamento IS Null THEN Qtde ELSE 0 END),

      Venda =       SUM(CASE WHEN Data_Cancelamento IS Null THEN Convert(Decimal(18,2),VLR-desconto) ELSE 0 END),

      --Qtde_Cancel = SUM(CASE WHEN Data_Cancelamento IS NOT Null THEN Qtde ELSE 0 END),

      Vlr_Cancel =  SUM(CASE WHEN Data_Cancelamento IS NOT Null THEN Convert(Decimal(18,2),Vlr) ELSE 0 END),

      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento),

      Cupom_MD =    CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) /  (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento)) * 100)

                  ELSE 0 END

      FROM

            Saida_Estoque

      WHERE

            Filial = @Filial

      AND

            Data_Movimento BETWEEN @DataDe AND @DataAte

--    AND

--          Data_Cancelamento IS NULL

      GROUP BY

            Data_Movimento

      ORDER BY

            Data_Movimento



go
CREATE PROCEDURE sp_rel_Cadastro_Produto 
@filial varchar(20),
@plu varchar(20),@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
as

/*
set @plu ='';
set @ean ='';
set @descricao ='';
set @grupo ='BEBIDAS';
set @subGrupo ='';
set @departamento ='';
set @familia ='';
*/
    select '|-PLU:-|'+a.plu + ' |-EAN:-|'+isnull(b.ean,'_____________')+
    ' || |- DESCRICAO: -| '+	rtrim(a.descricao)+
    ' || |- DESCRICAO RESUMIDA: -| '+isnull(a.descricao_resumida,'____________________')+
    ' || |- GRUPO: -| '+isnull(convert(varchar,c.codigo_grupo),'______')+' '+ c.descricao_grupo+' |-SUBGRUPO:-|'+c.codigo_subgrupo+' '+c.descricao_subgrupo+
    ' || |- DEPARTAMENTO: -|'+c.codigo_departamento +' '+c.descricao_departamento+
    ' || |- FAMILIA: -| '+isnull(replace(a.codigo_familia,' ','_'),'____')+' ' +isnull(replace(a.descricao_familia,' ','_'),'______________')+
    ' || |- LOCALIZACAO: -| '+isnull(replace(a.localizacao,' ','_'),'___________')+
    ' || |- DATA CADASTRO:-| '+isnull(convert(varchar,data_cadastro,103),'__________')+' |-DATA ALTERACAO:-|'+isnull(convert(varchar,data_alteracao,103),'__________')
    as '  ',
    ' || |- TIPO: -| '+isnull(a.tipo,'________')+
    ' || |- UND: -| '+isnull(a.und,'___')+
    ' || |- TECLA: -| '+isnull(convert(varchar,a.tecla),'___')+
    ' || |- VENDA FRACIONARIA: -| '+isnull(venda_fracionaria,'NAO')+
    ' || |- PESO VARIAVEL: -| '+isnull(peso_variavel,'___')+
    ' || |- CENTRO CUSTO: -| '+isnull(a.codigo_centro_custo,'____________')+
    ' || |- LINHA: -| '+isnull(convert(varchar,a.cod_linha),'____')+' '+isnull(d.descricao_linha,'___________')+
    ' || |- COR LINHA: -| '+isnull(convert(varchar,a.cod_cor_linha),'____')+' '+isnull(e.descricao_cor,  '___________')+  ' || || '
    as ' '
    from mercadoria a left join ean b on a.plu =b.plu
    inner join W_BR_CADASTRO_DEPARTAMENTO c on (a.codigo_departamento= c.codigo_Departamento and a.filial=c.filial)
    left join linha d on a.cod_linha= d.codigo_linha
    left join cor_linha e on a.cod_cor_linha = e.codigo_cor
    where (a.PLU=@plu or len(@plu)=0)
		and (b.EAN=@ean or LEN(@ean)=0)
		and (a.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (a.Descricao_familia = @familia or LEN(@familia)=0)
        

go 


create PROCEDURE sp_rel_Produto_Promocao 
@filial varchar(20),
@plu varchar(20),@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
as

/*
set @plu ='';
set @ean ='';
set @descricao ='';
set @grupo ='BEBIDAS';
set @subGrupo ='';
set @departamento ='';
set @familia ='';
*/

select PLU=m.plu,e.EAN,GRUPO=c.Descricao_grupo,SUBGRUPO=C.descricao_subgrupo,DEPARTAMETO=C.descricao_departamento,
		PRECO=l.Preco,PROMOCAO= case when l.promocao=1then l.preco_promocao else 0 end,
		INICIO= case when l.promocao=1then l.Data_Inicio else 0 end,
		FIM=case when l.promocao=1then l.Data_Fim else 0 end
		
	
	from mercadoria m inner join mercadoria_loja l on m.plu = l.PLU
								 left join EAN e on m.plu_fut=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (m.codigo_departamento= c.codigo_Departamento and m.filial=c.filial)
		WHERE  (M.PLU=@plu or len(@plu)=0)
		and (E.EAN=@ean or LEN(@ean)=0)
		and (M.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (M.Descricao_familia = @familia or LEN(@familia)=0)										 
		and l.Filial = @filial
		AND L.Promocao=1

go


create PROCEDURE sp_rel_Produto_Alterado 
@filial varchar(20),
@plu varchar(20),@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@DataDe DateTime
,@DataAte DateTime
as

/*
set @plu ='';
set @ean ='';
set @descricao ='';
set @grupo ='BEBIDAS';
set @subGrupo ='';
set @departamento ='';
set @familia ='';
*/

select PLU=m.plu,e.EAN,GRUPO=c.Descricao_grupo,SUBGRUPO=C.descricao_subgrupo,DEPARTAMETO=C.descricao_departamento,
		PRECO=l.Preco,PROMOCAO= case when l.promocao=1then l.preco_promocao else 0 end,
		M.USUARIO ,ALTERADO =CONVERT(VARCHAR,M.Data_Alteracao,103) 		  
	
	from mercadoria m inner join mercadoria_loja l on m.plu = l.PLU
								 left join EAN e on m.plu_fut=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (m.codigo_departamento= c.codigo_Departamento and m.filial=c.filial)
		WHERE M.Data_Alteracao BETWEEN @DataDe AND @DataAte
			AND (M.PLU=@plu or len(@plu)=0)
		and (E.EAN=@ean or LEN(@ean)=0)
		and (M.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (M.Descricao_familia = @familia or LEN(@familia)=0)										 
		and l.Filial = @filial
		
		
		