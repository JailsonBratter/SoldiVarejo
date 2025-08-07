
ALTER TABLE historico_mov_conta ALTER COLUMN DOCUMENTO VARCHAR(50)

go

ALTER TABLE CENTRO_CUSTO ADD CONTABIL VARCHAR(20)

go

insert into Versoes_Atualizadas select 'Versão:1.281.832', getdate();
