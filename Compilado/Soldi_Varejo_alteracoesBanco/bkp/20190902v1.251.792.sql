
IF OBJECT_ID('TempDB.dbo.#AU') IS NOT NULL
begin
  drop table #AU
end


SELECT CODIGO_CLIENTE , TOTAL= SUM(TOTAL) INTO #AU FROM (
Select codigo_cliente, TOTAL= sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) 
 from Conta_a_receber
 where status =1
 group by Codigo_Cliente
 UNION ALL
 SELECT CODIGO_CLIENTE,
 TOTAL =SUM(CASE WHEN TIPO ='DEBITO' THEN Total_Caderneta  ELSE Total_Caderneta *-1 END)
  FROM Caderneta
 
  GROUP BY Codigo_Cliente
  ) AS A 
  GROUP BY Codigo_Cliente

  UPDATE CLIENTE SET Utilizado = #AU.TOTAL 
  FROM #AU
  WHERE CLIENTE.Codigo_Cliente = #AU.CODIGO_CLIENTE
go 



GO 
insert into Versoes_Atualizadas select 'Versão:1.251.792', getdate();