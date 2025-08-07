IF OBJECT_ID (N'cliente_pet_Imagem', N'U') IS NULL 
begin

Create table cliente_pet_Imagem(
	codigo_pet varchar(20),
	codigo_cliente varchar(11),
	Imagem varchar(50),
	urlstr varchar(500)
)
end
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Saida_Estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('CancelamentoAutorizador'))
begin
	alter table Saida_Estoque alter column CancelamentoAutorizador	varchar(30)
end
else
begin
	alter table Saida_Estoque add CancelamentoAutorizador	varchar(30)
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Saida_Estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('MotivoCancelamento'))
begin
	alter table Saida_Estoque alter column MotivoCancelamento	varchar(30)
end
else
begin
	alter table Saida_Estoque add MotivoCancelamento	varchar(30)
end 
go 



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_cancelados]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].sp_rel_produtos_cancelados
END
GO

Create procedure [dbo].[sp_rel_produtos_cancelados] (
@filial varchar(20),
@datade varchar(11),
@dataate varchar(11),
@plu varchar(17),
@pdv varchar(10),
@documento varchar(20)

)
as
begin 
-- exec sp_rel_produtos_cancelados 'MATRIZ','20190122','20190123','','TODOS',''
			Select 
				se.Documento
				,pdv =Caixa_Saida 
				,se.plu
				,m.Descricao
				,Data_Cancelamento = Convert(varchar,se.data_cancelamento,103)
				,se.Qtde
				,se.vlr
				,se.CancelamentoAutorizador
				,se.MotivoCancelamento
			From Saida_estoque as se inner join mercadoria as m on se.plu=m.PLU
			where data_cancelamento is not null
			 and se.Filial = @filial
			 and (data_cancelamento between @datade and @dataate)
			 and (len(@plu) =0 or se.plu=@plu)
			 and (len(@documento)=0 or se.Documento = @documento)
			 and (@pdv='TODOS'  or CONVERT(VARCHAR,se.Caixa_Saida) = @pdv)
end 
go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_produtos_cancelados_comanda]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].sp_rel_produtos_cancelados_comanda
END
GO

Create procedure sp_rel_produtos_cancelados_comanda (
@filial varchar(20),
@datade varchar(11),
@dataate varchar(11),
@plu varchar(17),
@comanda varchar(20)

)
as
begin 
			Select 
				ci.Comanda
				,ci.PLU
				,m.Descricao
				,Data_Cancelamento = Convert(varchar,ci.data_cancelamento,103)
				,ci.Qtde
				,ci.Total
				,ci.Usuario_Cancelamento
				,ci.Motivo_Cancelamento
			From comanda_item as ci inner join mercadoria as m on ci.plu=m.PLU
			where data_cancelamento is not null
			 and ci.Filial = @filial
			 and (data_cancelamento between @datade and @dataate)
			 and (len(@plu) =0 or ci.plu=@plu)
			 and (len(@comanda)=0 or ci.comanda = @comanda)
			 
end 
go
insert into Versoes_Atualizadas select 'Versão:1.263.807', getdate();





