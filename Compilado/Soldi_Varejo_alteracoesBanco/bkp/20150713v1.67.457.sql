

go
if exists(select 1 from syscolumns where id =OBJECT_ID('nf_item') and name='inativo')
	alter table nf_item alter column inativo tinyint
else	
	alter table nf_item add inativo tinyint





GO

 
--sp_Rel_Fin_PorOperadorCancelamento 'MATRIZ', '20150701', '20150701', '', ''
ALTER PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As DATETIME,
      @Dataate			As DATETIME,
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as

if len(@Operador) =0

      begin

            set @Operador ='%'     

      end
      
if LEN(@Pdv) = 0

		begin
			set @Pdv = '%'
			end
 

select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data  

      , Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and c.cancelado is  null), 0) as  Vendas

      , isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and   c.cancelado is not null),0)as  Cancelados
      

      from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador

 

      where a.filial=@FILIAL and b.nome like @Operador and a.emissao between @Datade and @Dataate And a.pdv like @Pdv

group by a.operador,a.Pdv, b.nome,a.emissao

order by a.Pdv, b.nome

 

 

 

