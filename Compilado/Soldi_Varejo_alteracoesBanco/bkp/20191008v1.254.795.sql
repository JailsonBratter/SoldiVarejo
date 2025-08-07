

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_nota_entrada_alteracao]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_rel_nota_entrada_alteracao]
END
GO

create procedure sp_rel_nota_entrada_alteracao(
@filial as varchar(40),
@dataDe as varchar(10),
@dataAte as varchar(10),
@tipoData as varchar(20),
@fornecedor as varchar(100),
@Usuario as varchar(100)
) 
as 
begin 
	Select codigo
		 , Cliente_fornecedor
		 , Convert(varchar,Emissao,103) Emissao
		 , usuario_alteracao
		 , Convert(varchar,data_alteracao,103) as Data_Alteracao
		 , Convert(varchar,data_alteracao,24) as  Hora 
	 from  nf_log
	 where filial = @filial
	  and (
			(@tipoData = 'Emissao' and Emissao between @dataDe and @dataAte)
			or (@tipoData = 'Alteracao' and data_alteracao between @dataDe and @dataAte)
			)
	 and (len(@fornecedor)=0 or Cliente_Fornecedor = @fornecedor)
	 and (len(@Usuario)=0 or usuario = @Usuario)
	 order by (case when @tipoData= 'Emissao' then Emissao else data_alteracao end )Desc
			 
end



GO 
insert into Versoes_Atualizadas select 'Versão:1.254.795', getdate();