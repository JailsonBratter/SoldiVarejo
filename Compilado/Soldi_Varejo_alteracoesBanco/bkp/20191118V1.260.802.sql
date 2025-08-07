

 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_desconto_tabela_cliente]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_desconto_tabela_cliente]
END
GO
Create procedure [dbo].[sp_rel_desconto_tabela_cliente] (
	@filial as varchar(50),
	@datade varchar(8),
	@dataate varchar(8),
	@Codigo_Cliente varchar(50),
	@tipo varchar(20),
	@plu varchar(20),
	@tabelas nvarchar(max),
	@pdv as varchar(5)
)
as 
begin
-- sp_rel_desconto_tabela_cliente 'MATRIZ', '20191023','20191023','','ANALITICO','','TODOS','TODOS'

	 Select s.Documento
		   ,Codigo_Cliente = 'SFT_' +c.Codigo_Cliente
		   ,c.Nome_Cliente
		   ,Codigo_tabela ='SFT_' + case when isnull(t.Codigo_tabela,'') ='' then 'SEM TABELA' ELSE t.Codigo_tabela END 
		   ,Porc_desc='SFT_' + convert(varchar,isnull(t.porc,0))
		   ,Qtde = sum(isnull(s.qtde,0))
		   ,Valor =sum(isnull(s.vlr,0)) 
		   ,Valor_Desconto = sum(isnull(s.desconto,0))
	 into #tbDesconto from Saida_estoque  as s 
	 left join cliente as c on s.Codigo_Cliente = c.Codigo_Cliente 
	 left join  tabela_preco as t on  t.Codigo_tabela = c.Codigo_tabela
	 WHERE s.Filial= @filial 
		and  s.Data_movimento between @datade and @dataate
		and (len(@plu) =0 or s.PLU = @plu)
		and (len(@Codigo_Cliente) =0 or c.Codigo_Cliente = @Codigo_Cliente)
		and (@tabelas='TODOS' OR   @tabelas like '%|'+case when isnull(t.Codigo_tabela,'') ='' then 'SEM TABELA' ELSE t.Codigo_tabela END+'|%' )
		and (@pdv = 'TODOS' OR CONVERT(VARCHAR,S.Caixa_Saida) = @PDV)
		group by s.Documento
				,c.Codigo_Cliente
				,c.Nome_Cliente
				,case when isnull(t.Codigo_tabela,'') ='' then 'SEM TABELA' ELSE t.Codigo_tabela END
				,t.porc 	


	if(@tipo='ANALITICO')
	BEGIN  
		Select Codigo_Cliente
			  ,Nome_Cliente
			  ,Codigo_tabela
			  ,[%]=Porc_desc
			  ,Valor  = Sum(Valor)
			  ,Qtde = Sum(qtde)
			  ,Valor_Desconto  =Sum(Valor_desconto)
			  ,[Valor Liq] = Sum(Valor) -Sum(Valor_desconto)
		from #tbDesconto
		group by Codigo_Cliente,Nome_Cliente,Codigo_tabela,Porc_desc
	END 
	ELSE
	BEGIN
		
		Select   Codigo_tabela
				,[%]=isnull(Porc_desc,0)
				,[Qtde Clientes]=count(Documento)
				,[Qtde Vendia]= sum(qtde)
				,Valor =sum(isnull(Valor,0)) 
				,Valor_Desconto = sum(isnull(Valor_Desconto,0))
				,[Valor Liq] = Sum(Valor) -Sum(Valor_desconto) 
		from #tbDesconto
		group by Codigo_tabela, isnull(Porc_desc,0)
		

	END 
end 
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperadorCancelamento]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento]
END
GO
Create PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](
      @FILIAL           AS VARCHAR(17),
      @Datade           As varchar(8),
      @Dataate          As varchar(8),
      @Operador         As varchar(20),
      @Pdv              As varchar(20))
as

  
-- exec sp_Rel_Fin_PorOperadorCancelamento 'MATRIZ','20191118','20191118','TODOS','TODOS'              declare @strQuery as nvarchar(3000)

    declare @strWhere As nVarchar(1024)
	declare @strQuery As nVarchar(1024)

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

 

set @strQuery = @strQuery + ',ISNULL((select COUNT(*) From (select distinct d.documento, d.Emissao, d.pdv, d.operador, d.id_movimento from Lista_finalizadora d) e where e.emissao=a.emissao and a.operador=e.operador and a.pdv= e.pdv), 0) as  NroCupons '

 

set @strQuery = @strQuery + ' from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador '

set @strQuery = @strQuery + ' where a.filial= ' + char(39) + @FILIAL + char(39) + ' and a.emissao  BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)

set @strQuery = @strQuery + @strWhere

set @strQuery = @strQuery + ' group by a.operador,a.Pdv, b.nome,a.emissao '

set @strQuery = @strQuery + ' order by a.Pdv, b.nome'

 

--print @strQuery

execute(@strQuery)

 
go
 
insert into Versoes_Atualizadas select 'Versão:1.260.802', getdate();

 