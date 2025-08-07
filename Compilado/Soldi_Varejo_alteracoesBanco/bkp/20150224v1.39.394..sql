ALTER PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] @FILIAL VARCHAR(20),@datade varchar(10), @dataate varchar(10),@grupo varchar(20)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20141001','20141001','ACOUGUE'
	-- select top 10 * from saida_estoque
declare @total decimal(12,2)	

if len(@grupo)>0 
	begin
	
		select @total = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))) from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and (a.Data_movimento between @datade and @dataate) and c.Descricao_grupo = @grupo				
		
		select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(a.vlr),[%]=CONVERT(DECIMAL(12,2),((sum(a.vlr)/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and(a.Data_movimento between @datade and @dataate )and c.Descricao_grupo = @grupo  and a.data_cancelamento is null				
		group by c.codigo_departamento,c.descricao_departamento

	end
else
begin

		select @total = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))) from Saida_estoque where Filial=@FILIAL and Data_movimento between @datade and @dataate
		select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),[%]=CONVERT(DECIMAL(12,2),((((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0)))/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and a.Data_movimento between @datade and @dataate	and a.data_cancelamento is null				
		group by c.codigo_grupo,c.Descricao_grupo
end
end



--=======================================================================================================

--=======================================================================================================

GO
--exec     sp_Rel_Fin_PorOperador @Filial='MATRIZ', @datade = '20141007',  @dataate = '20141007',  @Caixa = '',  @Grupo = '',  @subGrupo = '',  @Departamento = '',  @Familia = '' 

ALTER           PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(8),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60)
            )
as
	
if len(@Caixa)= 0
	begin
		set @Caixa = '%'
	end
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


select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 

where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 	Data_movimento <=@Dataate and isnull(caixa_saida,'') like @Caixa
	and c.Descricao_grupo like @Grupo
	and c.descricao_subgrupo like @subGrupo
	and c.descricao_departamento like @Departamento
	and isnull(f.Descricao_Familia,'') like @Familia
	
group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
order by b.descricao

