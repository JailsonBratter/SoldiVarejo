IF not  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Acumulado_Geral]') AND type in (N'U'))
begin


CREATE TABLE [dbo].[Acumulado_Geral](
	[FILIAL] [varchar](20) NOT NULL,
	[PLU] [varchar](20) NOT NULL,
	[EAN] [varchar](17) NULL,
	[DESCRICAO] [varchar](60) NULL,
	[GRUPO] [int] NULL,
	[GRUPO_DESCRICAO] [varchar](40) NULL,
	[SUBGRUPO] [varchar](6) NULL,
	[SUBGRUPO_DESCRICAO] [varchar](40) NULL,
	[DEPARTAMENTO] [varchar](9) NULL,
	[DEPARTAMENTO_DESCRICAO] [varchar](40) NULL,
	[CST_ICMS] [varchar](2) NULL,
	[ALIQUOTA_ICMS] [numeric](6, 2) NULL,
	[CST_PISCOFINS_ENTRADA] [varchar](2) NULL,
	[CST_PISCOFINS_SAIDA] [varchar](2) NULL,
	[ALIQUOTA_PIS] [numeric](6, 2) NULL,
	[ALIQUOTA_COFINS] [numeric](6, 2) NULL,
	[PRECO_CUSTO] [numeric](12, 2) NULL,
	[PRECO_VENDA] [numeric](12, 2) NULL,
	[SALDO_ATUAL] [numeric](14, 3) NULL,
	[PEDIDO_PENDENETE] [numeric](14, 3) NULL,
	[CUSTOMES13] [numeric](14, 3) NULL,
	[CUSTOMES12] [numeric](14, 3) NULL,
	[CUSTOMES11] [numeric](14, 3) NULL,
	[CUSTOMES10] [numeric](14, 3) NULL,
	[CUSTOMES09] [numeric](14, 3) NULL,
	[CUSTOMES08] [numeric](14, 3) NULL,
	[CUSTOMES07] [numeric](14, 3) NULL,
	[CUSTOMES06] [numeric](14, 3) NULL,
	[CUSTOMES05] [numeric](14, 3) NULL,
	[CUSTOMES04] [numeric](14, 3) NULL,
	[CUSTOMES03] [numeric](14, 3) NULL,
	[CUSTOMES02] [numeric](14, 3) NULL,
	[CUSTOMES01] [numeric](14, 3) NULL,
	[VQMES13] [numeric](14, 3) NULL,
	[VQMES12] [numeric](14, 3) NULL,
	[VQMES11] [numeric](14, 3) NULL,
	[VQMES10] [numeric](14, 3) NULL,
	[VQMES09] [numeric](14, 3) NULL,
	[VQMES08] [numeric](14, 3) NULL,
	[VQMES07] [numeric](14, 3) NULL,
	[VQMES06] [numeric](14, 3) NULL,
	[VQMES05] [numeric](14, 3) NULL,
	[VQMES04] [numeric](14, 3) NULL,
	[VQMES03] [numeric](14, 3) NULL,
	[VQMES02] [numeric](14, 3) NULL,
	[VQMES01] [numeric](14, 3) NULL,
	[VVMES13] [numeric](14, 2) NULL,
	[VVMES12] [numeric](14, 2) NULL,
	[VVMES11] [numeric](14, 2) NULL,
	[VVMES10] [numeric](14, 2) NULL,
	[VVMES09] [numeric](14, 2) NULL,
	[VVMES08] [numeric](14, 2) NULL,
	[VVMES07] [numeric](14, 2) NULL,
	[VVMES06] [numeric](14, 2) NULL,
	[VVMES05] [numeric](14, 2) NULL,
	[VVMES04] [numeric](14, 2) NULL,
	[VVMES03] [numeric](14, 2) NULL,
	[VVMES02] [numeric](14, 2) NULL,
	[VVMES01] [numeric](14, 2) NULL,
	[EQMES13] [numeric](14, 3) NULL,
	[EQMES12] [numeric](14, 3) NULL,
	[EQMES11] [numeric](14, 3) NULL,
	[EQMES10] [numeric](14, 3) NULL,
	[EQMES09] [numeric](14, 3) NULL,
	[EQMES08] [numeric](14, 3) NULL,
	[EQMES07] [numeric](14, 3) NULL,
	[EQMES06] [numeric](14, 3) NULL,
	[EQMES05] [numeric](14, 3) NULL,
	[EQMES04] [numeric](14, 3) NULL,
	[EQMES03] [numeric](14, 3) NULL,
	[EQMES02] [numeric](14, 3) NULL,
	[EQMES01] [numeric](14, 3) NULL,
	[EVMES13] [numeric](14, 2) NULL,
	[EVMES12] [numeric](14, 2) NULL,
	[EVMES11] [numeric](14, 2) NULL,
	[EVMES10] [numeric](14, 2) NULL,
	[EVMES09] [numeric](14, 2) NULL,
	[EVMES08] [numeric](14, 2) NULL,
	[EVMES07] [numeric](14, 2) NULL,
	[EVMES06] [numeric](14, 2) NULL,
	[EVMES05] [numeric](14, 2) NULL,
	[EVMES04] [numeric](14, 2) NULL,
	[EVMES03] [numeric](14, 2) NULL,
	[EVMES02] [numeric](14, 2) NULL,
	[EVMES01] [numeric](14, 2) NULL,
	[FLAG] [char](1) NULL,
	[INICIO] [date] NULL,
	[FIM] [date] NULL,
 CONSTRAINT [PK_Acumulado_Geral] PRIMARY KEY CLUSTERED 
(
	[FILIAL] ASC,
	[PLU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

end 
GO





IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_Fin_PorOperador]
GO


--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(30),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40),
    @plu	      as Varchar(20),
    @descricao    as Varchar(50) 
            )
as

if LEN(@Grupo)=0
	begin
		set @Grupo ='%'
	end
if LEN(@subGrupo)=0
	begin
		set @subGrupo = '%'
	end
if LEN(@Departamento)=0
	begin 
		set @Departamento = '%'
	end
if LEN(@Familia)=0
	begin
		set @Familia = '%'
	end

if(LEN(@fornecedor)=0 and @Caixa='TODOS')
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
	inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
	--inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
	--	and isnull(Operadores.Nome ,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and (LEN(@plu)=0 or a.PLU=@plu)
		and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
else
begin 
	if(@Caixa<>'TODOS')
		begin
				select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
			from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
			inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
			--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
			--inner join Operadores on l.operador = Operadores.ID_Operador	
			where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade 
			    and Data_movimento <=@Dataate 
				and (Select TOP 1 Operadores.Nome from Operadores inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador where  l.filial =a.Filial  and l.Emissao = a.Data_movimento and a.Caixa_Saida = l.pdv and a.Documento=L.Documento) = @Caixa 
				and c.Descricao_grupo like @Grupo
				and c.descricao_subgrupo like @subGrupo
				and c.descricao_departamento like @Departamento
				and isnull(f.Descricao_Familia,'') like @Familia
				and (LEN(@plu)=0 or a.PLU=@plu)
				and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
			group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
			order by b.descricao
		end
else
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque a with(index(ix_Rel_fin_porOperador))  inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
		 inner join Fornecedor_Mercadoria fm on a.PLU = fm.plu 	
		 INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON  a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
		inner join Operadores on l.operador = Operadores.ID_Operador	
				
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		and (@Caixa='TODOS' OR isnull(Operadores.Nome ,'') like @Caixa)
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and fm.fornecedor = @fornecedor
		and (LEN(@plu)=0 or a.PLU=@plu)
		and (LEN(@descricao)=0 or  b.descricao like '%'+@descricao+'%')
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
end



go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_GRUPO]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO]
GO

--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] 
	@FILIAL VARCHAR(20),
	@datade varchar(10), 
	@dataate varchar(10),
	@grupo varchar(max),
	@relatorio varchar(40)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20160601','20170308','','CUPOM'
	-- select top 10 * from saida_estoque
	-- @relatorio = TODOS ,CUPONS , NOTA SAIDA 	
declare @sql nvarchar(max);



if len(@grupo)>0 
	begin
		
		
		set @grupo = REPLACE(@grupo,'|',CHAR(39));
		
		
		set @sql ='declare @total decimal(12,2);
				  declare @totalnf decimal(12,2); '
		IF(@relatorio='CUPONS' OR @relatorio = 'TODOS')
		BEGIN	
		set @sql =@SQL+' 
						select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
							from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
												 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+') and	a.data_cancelamento is null;	'		
		END 
		ELSE
		BEGIN
			set @sql =@SQL+' SET @TOTAL =0;'
		
		END
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
		set @sql =@SQL+' 
						select @totalnf =  isnull((SUM(a.Total)),0)
						 from NF_Item a inner join mercadoria b on a.PLU = b.plu 
											 inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
											 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
											 INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
						where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+')	and nf.nf_Canc <>1;'			
		END
		ELSE
		BEGIN
				set @sql =@SQL+' SET @totalnf =0;'
		END
		
	 set @sql =@SQL+'	Select COD,DESCRICAO , Venda = sum(Venda),[%]=CONVERT(DECIMAL(12,2),((sum(venda)/(@total+@totalnf))*100)) 
		from 
		('
		IF(@relatorio='TODOS' OR @relatorio='CUPOM')
		BEGIN
			SET @sql =@SQL+'	select	COD=c.codigo_departamento,
					DESCRICAO= c.descricao_departamento , 
					Venda = (SUM(ISNULL(a.vlr,0))-SUM(isnull(a.desconto,0)))+SUM(isnull(a.acrescimo,0))
				from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and a.data_cancelamento is null				
			group by c.codigo_departamento,c.descricao_departamento '
		END
		IF(@relatorio='TODOS')
		BEGIN
		  SET @sql =@SQL+' union all '
		END
		IF(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
		BEGIN
		
		SET @sql =@SQL+'	select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(isnull(a.Total,0))
				from NF_Item a inner join mercadoria b on a.PLU = b.plu 
							   inner join nf on nf.Filial =a.Filial and nf.Codigo = a.codigo and a.Tipo_NF = nf.Tipo_NF
							   inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							   INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
			where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							  and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							  and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and nf.nf_Canc <>1				
			group by c.codigo_departamento,c.descricao_departamento '
		END
		SET @sql =@SQL+')as a
		group by COD,DESCRICAO
		'
	end
else
begin
		set @sql ='
		declare @total decimal(12,2);
		declare @totalnf decimal(12,2);	'
		IF(@relatorio='CUPOM' OR @relatorio = 'TODOS')
		BEGIN	
			set @sql =@SQL+'
						select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
						from Saida_estoque 
						where Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and data_cancelamento is null  and Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +';'
		END
		ELSE
		BEGIN
			SET	@sql =@SQL+' SET @TOTAL=0; '
		END
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
		set @sql =@SQL+' 
							select @totalnf =  isnull((SUM(a.Total)) ,0)
							from NF_Item a inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
								INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
							where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							    and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							    and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' 
								and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') 
								and nf.nf_Canc <>1
								
								;			
							'
		END
		ELSE
		BEGIN
			SET	@sql =@SQL+' SET @totalnf=0; '
		END
		
		SET	@sql =@SQL+' Select COD,DESCRICAO,VENDA =SUM(VENDA),[%]=CONVERT(DECIMAL(12,2),(SUM(VENDA)/(@total+@totalnf))*100)  
		from ('
		IF(@relatorio='CUPOM' OR @relatorio = 'TODOS')
		BEGIN
		set @sql =@SQL+'	
			select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))) 
			from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			WHERE a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	and a.data_cancelamento is null				
			group by c.codigo_grupo,c.Descricao_grupo '
		END
		IF(@relatorio='TODOS')
		BEGIN
		  SET @sql=@sql+'	UNION ALL '
		END 
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN
			SET	@sql =@sql+' select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = (SUM(A.Total)) 
			from NF_Item a inner join mercadoria b on a.PLU = b.plu 
						   INNER JOIN NF ON NF.Filial= A.Filial AND NF.Codigo=A.CODIGO AND NF.Tipo_NF=A.Tipo_NF	
						   inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
						   INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
			where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							    and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							    and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' 
				and (NF.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	)
				and NF.nf_Canc<>1		
			group by c.codigo_grupo,c.Descricao_grupo'
		END
			
		SET @sql=@sql+') as a
		GROUP BY COD,DESCRICAO'
end
 --print @sql;
 exec(@sql);
end





GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.168.609', getdate();
GO