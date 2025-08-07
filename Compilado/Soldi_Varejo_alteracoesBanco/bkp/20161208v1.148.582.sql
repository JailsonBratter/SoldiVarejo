
/****** Object:  StoredProcedure [dbo].[sp_rel_Tesouraria]    Script Date: 12/09/2016 14:35:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Tesouraria]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_Tesouraria]
GO

--sp_rel_Tesouraria 'MATRIZ',20161202,20161202,'TODOS','FECHADO'

CREATE Procedure [dbo].[sp_rel_Tesouraria]
		  @Filial       As Varchar(17),
		  @DataDe             As Varchar(8),
		  @DataAte      As Varchar(8),
		  @Operador     As Varchar(20),
		  @Status_Pdv     As varchar(20)
    
As    

Declare @String as nvarchar(max)
Declare @Where as nvarchar(max)
Declare @And as nvarchar(max)
Declare @WhereReceber as nvarchar(max);
      
SET @Where= '';
set @WhereReceber = '';
SET @And = '';

            Set @Where = ' And l.Filial = '+ char(39) + @filial + char(39) + ' and '
            Set @Where = @Where + ' l.Emissao between ' + char(39) + @DataDe + char(39) + ' and ' + char(39) + @DataAte + char(39)
            
            Set @WhereReceber = ' And c.Filial = '+ char(39) + @filial + char(39) + ' and '
            Set @WhereReceber = @WhereReceber + ' c.Emissao between ' + char(39) + @DataDe + char(39) + ' and ' + char(39) + @DataAte + char(39)

IF @Operador <> 'TODOS'
  Begin
            Set @Where = @Where + ' And op.ID_Operador = '+ char(39) + @Operador + char(39)
            Set @WhereReceber = @WhereReceber + ' And c.Operador = '+ char(39) + @Operador + char(39)
  End
IF @Status_Pdv <> 'TODOS'
  Begin
            Set @And = ' And sp.Status = '+ char(39) + @Status_Pdv + char(39)                 
  End         
      

Set @String = 'Select NroFinalizadora,Finalizadora,
						[Valores Caixas]= sum(isnull(ValoresCaixas,0)) , 
						[Valores Tesouraria]= sum(isnull(ValoresTesouraria,0)),
						[Dif] = (sum(isnull(ValoresTesouraria,0))-sum(isnull(ValoresCaixas,0)))
				from (

				Select 
					  NroFinalizadora = Isnull(l.Finalizadora,0), 
					  Finalizadora = Isnull(l.id_Finalizadora,' + CHAR(39) + CHAR(39)  +'), 
					  [ValoresCaixas] = Sum(ISNULL(l.TOTAL,0)), 
					  [ValoresTesouraria] = 0
					  
				From 
					  Lista_Finalizadora l Inner Join Operadores as op on op.ID_Operador=l.operador 
					  Left Join Status_Pdv as sp on sp.Id_Operador = op.ID_Operador And sp.pdv= l.pdv And sp.Data_Abertura = l.Emissao  
				Where 
					  Isnull(l.Cancelado,0) = 0
				And
					  Not l.id_Finalizadora = ' + CHAR(39) + CHAR(39)  +' '+@Where +' '+@And +'
				Group by 
					  l.Finalizadora, l.id_Finalizadora, l.Filial

				union all
				Select c.Finalizadora ,id_Finalizadora,  0, Isnull(Sum(c.Valor),0) From Conta_a_Receber c 
							Where  isnull(finalizadora,0)<> 0  '+@WhereReceber+'
							group by id_Finalizadora, c.Finalizadora


				) as a

				group by NroFinalizadora,Finalizadora
				Order by 
					  1, 2 
';






--Set @String = 'Select 
--      NroFinalizadora = Isnull(l.Finalizadora,0), 
--      Finalizadora = Isnull(l.id_Finalizadora,' + CHAR(39) + CHAR(39) + '), 
--      [Valores Caixas] = Sum(ISNULL(l.TOTAL,0)), 
--      [Valores Tesouraria] = (Select Isnull(Sum(c.Valor),0) From Conta_a_Receber c 
--            Where c.id_Finalizadora = l.id_Finalizadora And c.Finalizadora = l.Finalizadora      '     
--            +@WhereReceber+'),
--	  [Dif]=(isnull((Select Isnull(Sum(c.Valor),0) From Conta_a_Receber c 
--            Where c.id_Finalizadora = l.id_Finalizadora And c.Finalizadora = l.Finalizadora      '     
--            +@WhereReceber+'),0)-Sum(ISNULL(l.TOTAL,0)))
--		From 
--			  Lista_Finalizadora l Inner Join Operadores as op on op.ID_Operador=l.operador 
--			  Left Join Status_Pdv as sp on sp.Id_Operador = op.ID_Operador And sp.pdv= l.pdv And sp.Data_Abertura = l.Emissao  
--		Where 
--			  Isnull(l.Cancelado,0) = 0
--		And
--			  Not l.id_Finalizadora = ' + CHAR(39) + CHAR(39)  +' '+@Where +' '+@And +'
--		Group by 
--			  l.Finalizadora, l.id_Finalizadora, l.Filial
	
--		Order by 
--			  1, 2 ';

	 -- print (@string)
	exec (@string);







insert into Versoes_Atualizadas select 'Versão:1.148.582', getdate();
GO


