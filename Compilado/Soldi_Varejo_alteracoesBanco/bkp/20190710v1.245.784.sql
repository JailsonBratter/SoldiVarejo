IF OBJECT_ID (N'Funcionario_metas', N'U') IS NULL 
begin
	Create table Funcionario_metas(
		filial varchar(20),
		codigo_funcionario varchar(9),
		codigo_departamento varchar(9),
		meta numeric(18,2)
		)

end
	
	
	
go 

	insert into Versoes_Atualizadas select 'Versão:1.245.784', getdate();
GO
