


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_HISTORICO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_COMANDA_HISTORICO]
end
GO

--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO] (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(5),
		@finalizado varchar(5)
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
select  i.comanda, 
	   data=convert(varchar,i.data,103),
	   hora = convert(varchar,i.hora_evento,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.usuario,
	   i.plu,
	   m.descricao,
	   i.qtde,
	   i.unitario,
	   i.total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	     
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and  (len(@comanda)=0 or CONVERT(VARCHAR(5),i.comanda)=@comanda)  
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
	  and (@usuario='TODOS' OR  i.usuario = @usuario ) 
	  and (@cancelamento='TODOS' OR  (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento ))
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And(@finalizado='TODOS' OR case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado)
order by i.hora_evento desc 
	  
	  	  	  

go 


-- sp_Rel_Fin_PorOperadorCancelamento 'matriz', '2015-12-28', '2015-12-28', 'TODOS', 'TODOS'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperadorCancelamento]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperadorCancelamento]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As DATETIME,
      @Dataate			As DATETIME,
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as
	declare @strQuery as nvarchar(3000)
	declare @strWhere As nVarchar(1024)
    set @strWhere = ''
    
if len(@Operador) > 0 AND @Operador <>'TODOS'

      begin
            set @strWhere = ' AND b.Nome LIKE ' + CHAR(39) + @Operador + '%' + CHAR(39)     
      end
      
if (LEN(@Pdv) > 0 AND @Pdv <>'TODOS')

		begin
			set @strWhere = @strWhere + ' AND a.PDV = ' + @PDV 
		end
 
set @strQuery = 'select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data ' 
set @strQuery = @strQuery + ', Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and isnull(c.cancelado,0)=0), 0) as  Vendas'
set @strQuery = @strQuery + ', isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   isnull(c.cancelado,0)=1),0)as  Cancelados'
set @strQuery = @strQuery + ' from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador '
set @strQuery = @strQuery + ' where a.filial= ' + char(39) + @FILIAL + char(39) + ' and a.emissao between ' + char(39) + REPLACE(CONVERT(VARCHAR, @Datade, 102), '.', '-') + char(39) + ' and ' + + char(39) + REPLACE(CONVERT(VARCHAR, @Dataate, 102), '.', '-') + char(39) 
set @strQuery = @strQuery + @strWhere
set @strQuery = @strQuery + ' group by a.operador,a.Pdv, b.nome,a.emissao '
set @strQuery = @strQuery + ' order by a.Pdv, b.nome'

--print @strQuery 
execute(@strQuery)

go



ALTER PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(3000)
    Declare @StringSQL2 As nVarChar(3000)
    Declare @Where        As nVarChar(1000)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	DELETE FROM BUSCA_PRECO;
	insert into Busca_Preco 
	SELECT DISTINCT  PLU,Descricao_resumida,PRECO = CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END  
	FROM (

	SELECT [plu] = CONVERT(BIGINT, MERCADORIA.PLU), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim  
		FROM Mercadoria INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU  
		WHERE ISNULL(Inativo, 0) <= 0    AND MERCADORIA_LOJA.Filial = @Filial

	UNION ALL 


	SELECT [plu] = CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim FROM Mercadoria  INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU    INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU 
	 WHERE ISNULL(Inativo, 0) <= 0   AND MERCADORIA_LOJA.Filial = @Filial
	   
	 GROUP BY CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim 
	 HAVING CONVERT(BIGINT, EAN.EAN) <=99999999999999 
	) A
	GROUP BY PLU,Descricao_resumida,CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END ;


    
    
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '	  ,CST=convert(varchar(2),ISNULL(mercadoria.Origem,'+ char(39) + char(39) +'))+ convert(varchar(2), ISNULL(tributacao.indice_st,'+ char(39) + char(39) +'))'
        + '   ,und '+
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' WHERE ISNULL(Inativo, 0) <= 0  '

    IF @TipoCadastro < 100
    BEGIN
            SET @Where = ' '
        END
    ELSE
        BEGIN
            SET @Where = ' AND Peso_Variavel.Codigo > 0 '
        END
    IF @Alterados =1
        begin
            SET @Where = @Where+ ' AND estado_mercadoria=1'
        end

    SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)

    -- Adicionar o EAN
    IF @TipoCadastro < 100
        BEGIN
            SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
                + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
				+ '	  ,CST=convert(varchar(2),ISNULL(mercadoria.Origem,'+ char(39) + char(39) +'))+ convert(varchar(2), ISNULL(tributacao.indice_st,'+ char(39) + char(39) +'))'
				+ '   ,und '+
                + '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' WHERE ISNULL(Inativo, 0) <= 0 '

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 
