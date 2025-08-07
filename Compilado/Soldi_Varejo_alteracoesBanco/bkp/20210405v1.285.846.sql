
Create table manifesto_consultas(
	filial varchar(20),
	resposta varchar(255),
	data_hora datetime,
	ultNSU varchar(15),
	maxNSU varchar(15),
	esperar tinyint
)

go 

insert into manifesto_consultas 
Select filial,'Inicio controle consultas', GETDATE(),max(nsu),max(nsu),0 from Nf_manifestar group by Filial

go 

insert into Versoes_Atualizadas select 'Versão:1.285.846', getdate();