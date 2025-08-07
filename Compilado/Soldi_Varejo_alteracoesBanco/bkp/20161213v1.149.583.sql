
/****** Object:  StoredProcedure [dbo].[SP_REL_VENDAS_POR_GRUPO]    Script Date: 12/13/2016 14:29:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_GRUPO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO]
GO

--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] @FILIAL VARCHAR(20),@datade varchar(10), @dataate varchar(10),@grupo varchar(max)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20141001','20141001','ACOUGUE'
	-- select top 10 * from saida_estoque

declare @sql nvarchar(max);



if len(@grupo)>0 
	begin
		
		
		set @grupo = REPLACE(@grupo,'|',CHAR(39));
		
		
		set @sql ='declare @total decimal(12,2);
		declare @totalnf decimal(12,2);	
		select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+') and	a.data_cancelamento is null;			
		
		select @totalnf =  isnull((SUM(a.Total)),0)
		 from NF_Item a inner join mercadoria b on a.PLU = b.plu 
							 inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+')	and nf.nf_Canc <>1;			
		
		
		Select COD,DESCRICAO , Venda = sum(Venda),[%]=CONVERT(DECIMAL(12,2),((sum(venda)/(@total+@totalnf))*100)) 
		from 
		(
			select	COD=c.codigo_departamento,
					DESCRICAO= c.descricao_departamento , 
					Venda = (SUM(ISNULL(a.vlr,0))-SUM(isnull(a.desconto,0)))+SUM(isnull(a.acrescimo,0))
				from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and a.data_cancelamento is null				
			group by c.codigo_departamento,c.descricao_departamento
		  union all
			select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(isnull(a.Total,0))
				from NF_Item a inner join mercadoria b on a.PLU = b.plu 
						inner join nf on nf.Filial =a.Filial and nf.Codigo = a.codigo and a.Tipo_NF = nf.Tipo_NF
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and nf.nf_Canc <>1				
			group by c.codigo_departamento,c.descricao_departamento
				
		)as a
		group by COD,DESCRICAO
		'
	end
else
begin
		set @sql ='
		declare @total decimal(12,2);
		declare @totalnf decimal(12,2);	
		select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
		from Saida_estoque 
		where Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and data_cancelamento is null  and Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +';
		
		select @totalnf =  isnull((SUM(a.Total)) ,0)
		from NF_Item a inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
		where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and nf.nf_Canc <>1;			
		
		
		Select COD,DESCRICAO,VENDA =SUM(VENDA),[%]=CONVERT(DECIMAL(12,2),(SUM(VENDA)/(@total+@totalnf))*100)  
		from (
			select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))) 
			from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	and a.data_cancelamento is null				
			group by c.codigo_grupo,c.Descricao_grupo
			UNION ALL
			select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = (SUM(A.Total)) 
			from NF_Item a inner join mercadoria b on a.PLU = b.plu 
						   INNER JOIN NF ON NF.Filial= A.Filial AND NF.Codigo=A.CODIGO AND NF.Tipo_NF=A.Tipo_NF	
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (NF.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	)and NF.nf_Canc<>1		
			group by c.codigo_grupo,c.Descricao_grupo
			
		) as a
		GROUP BY COD,DESCRICAO'
end
--print @sql;
exec(@sql);
end



GO



/****** Object:  StoredProcedure [dbo].[sp_rel_fechamento_fornecedor]    Script Date: 12/13/2016 16:58:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fechamento_fornecedor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_fechamento_fornecedor]
GO


/****** Object:  StoredProcedure [dbo].[sp_rel_fechamento_fornecedor]    Script Date: 12/13/2016 16:58:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_fechamento_fornecedor](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(max),
	@Cheque		varchar(30),
	@Conferido varchar(10),
	@tipoPagamento varchar(50)
) as

Declare @String as nvarchar(max)
Declare @Where as nvarchar(max)

-- exec     sp_rel_fechamento_fornecedor   @Filial='MATRIZ', @datade = '20160301',  @dataate = '20160331',  @tipo = 'emissao',  @Status = 'PREVISTO',  @valor = '',  @fornecedor = '',  @centrocusto = '|001001003|,|003001001|',  @Cheque = '',  @CONFERIDO = 'TODOS',  @tipoPagamento = 'TODOS' 

Begin
	--Monta Clausula Where da Procura
	set @where = 'Where cp.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where + 'cp.'+@tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And cp.fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And cp.valor ='+REPLACE(@valor,',','.')	
	End
	if(@Conferido<>'TODOS')
	begin
		IF(@Conferido='SIM')
		BEGIN
			set @where = @where + ' And cp.conferido =1 '	
		END
		ELSE
		BEGIN
			set @where = @where + ' And ISNULL(cp.conferido,0) =0 '	
		END
		 
	end 
	
	set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
											 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
											 WHEN @STATUS='CANCELADO' THEN ' status =3'
											 WHEN @STATUS='LANCADO' THEN ' status =4'
										ELSE 'cp.status <> 3' END
																) 
	if LEN(@centrocusto)>0
	begin
		set @centrocusto = REPLACE(@centrocusto,'|',CHAR(39));
		set @Where = @Where + ' and cp.codigo_centro_custo in ('+ @centrocusto+ ')' 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and cp.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	
	if(@tipoPagamento <> 'TODOS')
	BEGIN
		set @Where = @Where + ' and cp.TIPO_PAGAMENTO= '+ char(39)+ @tipoPagamento+ char(39) 
	END
	


	set @String = 'SELECT   NOME , [VALOR PAGAR] = replace(VALOR_PAGAR,'+ char(39)+'.'+ char(39)+','+ char(39)+','+ char(39)+') FROM (
					Select CC.CODIGO_CENTRO_CUSTO+'+ char(39)+'AA'+ char(39)+' AS ORDEM, [CENTRO_CUSTO]= '+ char(39)+'SFT_'+ char(39)+'+CC.CODIGO_CENTRO_CUSTO,[NOME]= '+ char(39)+'|-'+ char(39)+'+CC.CODIGO_CENTRO_CUSTO+'+ char(39)+'-'+ char(39)+' +CC.descricao_centro_custo+'+ char(39)+'-|'+ char(39)+' , [Valor_Pagar] = '+ char(39)+'|-'+ char(39)+'+CONVERT(VARCHAR ,sum(Isnull(Valor,0)) - sum(Isnull(Desconto,0)) + (sum(Isnull(Acrescimo,0))))+'+ char(39)+'-|'+ char(39)+' 
					 FROM Centro_Custo AS CC
					  INNER JOIN Conta_a_pagar AS CP ON  CC.Codigo_centro_custo=CP.Codigo_Centro_Custo
					  '+@Where+
					  'GROUP BY CC.CODIGO_CENTRO_CUSTO , CC.descricao_centro_custo 
					UNION 
					Select CP.CODIGO_CENTRO_CUSTO+'+ char(39)+'BB'+ char(39)+' AS ORDEM ,'+ char(39)+ char(39)+',
										Fornecedor,
										[Valor Pagar] = CONVERT(VARCHAR ,sum(Isnull(Valor,0)) - sum(Isnull(Desconto,0)) + (sum(Isnull(Acrescimo,0)))) 
										from Conta_a_pagar AS CP
						'+@Where+'
						group by Codigo_Centro_Custo,Fornecedor
					union
					Select '+ char(39)+'ZZ'+ char(39)+' AS ORDEM ,'+ char(39)+ char(39)+','+
								char(39)+'|-TOTAL GERAL-|'+ char(39)+',
										
										[Valor Pagar] = '+ char(39)+'|-'+ char(39)+'+CONVERT(VARCHAR ,sum(Isnull(Valor,0)) - sum(Isnull(Desconto,0)) + (sum(Isnull(Acrescimo,0))))+'+ char(39)+'-|'+ char(39)+' 
										from Conta_a_pagar AS CP
						'+@Where+'

					)AS A
					 order by ORDEM';


	--set @string = ' Select Codigo_Centro_Custo,
	--				Fornecedor,
	--				[Valor Pagar] = sum(Isnull(Valor,0)) - sum(Isnull(Desconto,0)) + (sum(Isnull(Acrescimo,0))) 
	--				from Conta_a_pagar
	--				'+@Where+'
	--				group by Codigo_Centro_Custo,Fornecedor
	--				order by Codigo_Centro_Custo';
			--print @string;
	Exec(@string)
end
GO







insert into Versoes_Atualizadas select 'Versão:1.149.583', getdate();
GO