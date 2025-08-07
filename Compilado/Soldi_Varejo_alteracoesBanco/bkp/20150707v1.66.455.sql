create procedure sp_rel_fin_cartao
	(
	@filial     varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@cartao		varchar(50)
	)
	as
Select Documento,
	Cartao=ca.id_cartao, 
	Pdv = cr.pdv,
	Emissao=convert(varchar,Emissao,103),
	Vencimento=CONVERT(varchar,vencimento,103),
	Valor,
	[Taxa $]=cr.taxa,
	Desconto,
	Acrescimo = isnull(acrescimo,0), 
	Total=((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0) 
	from Conta_a_receber cr
		inner join Cartao ca on
		(			cr.finalizadora = ca.nro_Finalizadora 
				and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
				and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
				)
where CR.FILIAL =@filial AND
	(
		(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
		OR
		(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
	  )
	  AND (LEN(@cartao)=0 OR CA.ID_CARTAO = @cartao )
	  
order by ca.id_cartao,cr.pdv
