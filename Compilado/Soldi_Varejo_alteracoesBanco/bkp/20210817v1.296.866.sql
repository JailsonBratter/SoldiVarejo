IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_autorizadora'))
begin
	alter table finalizadora alter column id_autorizadora int
end
else
begin
	alter table finalizadora add id_autorizadora int
end 
go 


      
--Criando colunas QtdeDia (90colunas), ValorDia (90colunas) e Estoquedia90 (1Coluna)
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA01')) begin alter table Acumulado_Geral alter column VQDIA01 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA01 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA02')) begin alter table Acumulado_Geral alter column VQDIA02 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA02 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA03')) begin alter table Acumulado_Geral alter column VQDIA03 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA03 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA04')) begin alter table Acumulado_Geral alter column VQDIA04 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA04 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA05')) begin alter table Acumulado_Geral alter column VQDIA05 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA05 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA06')) begin alter table Acumulado_Geral alter column VQDIA06 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA06 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA07')) begin alter table Acumulado_Geral alter column VQDIA07 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA07 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA08')) begin alter table Acumulado_Geral alter column VQDIA08 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA08 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA09')) begin alter table Acumulado_Geral alter column VQDIA09 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA09 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA10')) begin alter table Acumulado_Geral alter column VQDIA10 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA10 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA11')) begin alter table Acumulado_Geral alter column VQDIA11 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA11 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA12')) begin alter table Acumulado_Geral alter column VQDIA12 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA12 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA13')) begin alter table Acumulado_Geral alter column VQDIA13 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA13 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA14')) begin alter table Acumulado_Geral alter column VQDIA14 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA14 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA15')) begin alter table Acumulado_Geral alter column VQDIA15 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA15 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA16')) begin alter table Acumulado_Geral alter column VQDIA16 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA16 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA17')) begin alter table Acumulado_Geral alter column VQDIA17 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA17 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA18')) begin alter table Acumulado_Geral alter column VQDIA18 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA18 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA19')) begin alter table Acumulado_Geral alter column VQDIA19 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA19 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA20')) begin alter table Acumulado_Geral alter column VQDIA20 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA20 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA21')) begin alter table Acumulado_Geral alter column VQDIA21 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA21 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA22')) begin alter table Acumulado_Geral alter column VQDIA22 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA22 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA23')) begin alter table Acumulado_Geral alter column VQDIA23 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA23 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA24')) begin alter table Acumulado_Geral alter column VQDIA24 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA24 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA25')) begin alter table Acumulado_Geral alter column VQDIA25 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA25 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA26')) begin alter table Acumulado_Geral alter column VQDIA26 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA26 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA27')) begin alter table Acumulado_Geral alter column VQDIA27 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA27 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA28')) begin alter table Acumulado_Geral alter column VQDIA28 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA28 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA29')) begin alter table Acumulado_Geral alter column VQDIA29 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA29 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA30')) begin alter table Acumulado_Geral alter column VQDIA30 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA30 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA31')) begin alter table Acumulado_Geral alter column VQDIA31 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA31 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA32')) begin alter table Acumulado_Geral alter column VQDIA32 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA32 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA33')) begin alter table Acumulado_Geral alter column VQDIA33 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA33 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA34')) begin alter table Acumulado_Geral alter column VQDIA34 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA34 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA35')) begin alter table Acumulado_Geral alter column VQDIA35 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA35 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA36')) begin alter table Acumulado_Geral alter column VQDIA36 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA36 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA37')) begin alter table Acumulado_Geral alter column VQDIA37 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA37 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA38')) begin alter table Acumulado_Geral alter column VQDIA38 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA38 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA39')) begin alter table Acumulado_Geral alter column VQDIA39 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA39 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA40')) begin alter table Acumulado_Geral alter column VQDIA40 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA40 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA41')) begin alter table Acumulado_Geral alter column VQDIA41 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA41 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA42')) begin alter table Acumulado_Geral alter column VQDIA42 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA42 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA43')) begin alter table Acumulado_Geral alter column VQDIA43 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA43 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA44')) begin alter table Acumulado_Geral alter column VQDIA44 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA44 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA45')) begin alter table Acumulado_Geral alter column VQDIA45 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA45 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA46')) begin alter table Acumulado_Geral alter column VQDIA46 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA46 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA47')) begin alter table Acumulado_Geral alter column VQDIA47 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA47 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA48')) begin alter table Acumulado_Geral alter column VQDIA48 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA48 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA49')) begin alter table Acumulado_Geral alter column VQDIA49 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA49 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA50')) begin alter table Acumulado_Geral alter column VQDIA50 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA50 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA51')) begin alter table Acumulado_Geral alter column VQDIA51 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA51 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA52')) begin alter table Acumulado_Geral alter column VQDIA52 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA52 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA53')) begin alter table Acumulado_Geral alter column VQDIA53 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA53 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA54')) begin alter table Acumulado_Geral alter column VQDIA54 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA54 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA55')) begin alter table Acumulado_Geral alter column VQDIA55 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA55 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA56')) begin alter table Acumulado_Geral alter column VQDIA56 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA56 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA57')) begin alter table Acumulado_Geral alter column VQDIA57 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA57 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA58')) begin alter table Acumulado_Geral alter column VQDIA58 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA58 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA59')) begin alter table Acumulado_Geral alter column VQDIA59 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA59 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA60')) begin alter table Acumulado_Geral alter column VQDIA60 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA60 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA61')) begin alter table Acumulado_Geral alter column VQDIA61 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA61 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA62')) begin alter table Acumulado_Geral alter column VQDIA62 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA62 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA63')) begin alter table Acumulado_Geral alter column VQDIA63 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA63 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA64')) begin alter table Acumulado_Geral alter column VQDIA64 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA64 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA65')) begin alter table Acumulado_Geral alter column VQDIA65 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA65 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA66')) begin alter table Acumulado_Geral alter column VQDIA66 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA66 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA67')) begin alter table Acumulado_Geral alter column VQDIA67 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA67 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA68')) begin alter table Acumulado_Geral alter column VQDIA68 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA68 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA69')) begin alter table Acumulado_Geral alter column VQDIA69 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA69 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA70')) begin alter table Acumulado_Geral alter column VQDIA70 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA70 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA71')) begin alter table Acumulado_Geral alter column VQDIA71 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA71 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA72')) begin alter table Acumulado_Geral alter column VQDIA72 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA72 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA73')) begin alter table Acumulado_Geral alter column VQDIA73 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA73 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA74')) begin alter table Acumulado_Geral alter column VQDIA74 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA74 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA75')) begin alter table Acumulado_Geral alter column VQDIA75 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA75 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA76')) begin alter table Acumulado_Geral alter column VQDIA76 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA76 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA77')) begin alter table Acumulado_Geral alter column VQDIA77 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA77 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA78')) begin alter table Acumulado_Geral alter column VQDIA78 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA78 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA79')) begin alter table Acumulado_Geral alter column VQDIA79 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA79 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA80')) begin alter table Acumulado_Geral alter column VQDIA80 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA80 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA81')) begin alter table Acumulado_Geral alter column VQDIA81 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA81 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA82')) begin alter table Acumulado_Geral alter column VQDIA82 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA82 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA83')) begin alter table Acumulado_Geral alter column VQDIA83 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA83 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA84')) begin alter table Acumulado_Geral alter column VQDIA84 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA84 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA85')) begin alter table Acumulado_Geral alter column VQDIA85 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA85 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA86')) begin alter table Acumulado_Geral alter column VQDIA86 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA86 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA87')) begin alter table Acumulado_Geral alter column VQDIA87 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA87 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA88')) begin alter table Acumulado_Geral alter column VQDIA88 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA88 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA89')) begin alter table Acumulado_Geral alter column VQDIA89 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA89 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VQDIA90')) begin alter table Acumulado_Geral alter column VQDIA90 numeric(14,3) end else begin  alter table Acumulado_Geral add VQDIA90 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA01')) begin alter table Acumulado_Geral alter column VVDIA01 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA01 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA02')) begin alter table Acumulado_Geral alter column VVDIA02 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA02 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA03')) begin alter table Acumulado_Geral alter column VVDIA03 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA03 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA04')) begin alter table Acumulado_Geral alter column VVDIA04 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA04 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA05')) begin alter table Acumulado_Geral alter column VVDIA05 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA05 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA06')) begin alter table Acumulado_Geral alter column VVDIA06 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA06 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA07')) begin alter table Acumulado_Geral alter column VVDIA07 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA07 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA08')) begin alter table Acumulado_Geral alter column VVDIA08 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA08 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA09')) begin alter table Acumulado_Geral alter column VVDIA09 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA09 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA10')) begin alter table Acumulado_Geral alter column VVDIA10 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA10 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA11')) begin alter table Acumulado_Geral alter column VVDIA11 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA11 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA12')) begin alter table Acumulado_Geral alter column VVDIA12 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA12 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA13')) begin alter table Acumulado_Geral alter column VVDIA13 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA13 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA14')) begin alter table Acumulado_Geral alter column VVDIA14 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA14 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA15')) begin alter table Acumulado_Geral alter column VVDIA15 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA15 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA16')) begin alter table Acumulado_Geral alter column VVDIA16 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA16 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA17')) begin alter table Acumulado_Geral alter column VVDIA17 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA17 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA18')) begin alter table Acumulado_Geral alter column VVDIA18 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA18 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA19')) begin alter table Acumulado_Geral alter column VVDIA19 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA19 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA20')) begin alter table Acumulado_Geral alter column VVDIA20 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA20 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA21')) begin alter table Acumulado_Geral alter column VVDIA21 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA21 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA22')) begin alter table Acumulado_Geral alter column VVDIA22 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA22 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA23')) begin alter table Acumulado_Geral alter column VVDIA23 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA23 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA24')) begin alter table Acumulado_Geral alter column VVDIA24 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA24 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA25')) begin alter table Acumulado_Geral alter column VVDIA25 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA25 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA26')) begin alter table Acumulado_Geral alter column VVDIA26 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA26 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA27')) begin alter table Acumulado_Geral alter column VVDIA27 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA27 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA28')) begin alter table Acumulado_Geral alter column VVDIA28 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA28 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA29')) begin alter table Acumulado_Geral alter column VVDIA29 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA29 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA30')) begin alter table Acumulado_Geral alter column VVDIA30 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA30 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA31')) begin alter table Acumulado_Geral alter column VVDIA31 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA31 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA32')) begin alter table Acumulado_Geral alter column VVDIA32 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA32 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA33')) begin alter table Acumulado_Geral alter column VVDIA33 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA33 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA34')) begin alter table Acumulado_Geral alter column VVDIA34 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA34 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA35')) begin alter table Acumulado_Geral alter column VVDIA35 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA35 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA36')) begin alter table Acumulado_Geral alter column VVDIA36 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA36 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA37')) begin alter table Acumulado_Geral alter column VVDIA37 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA37 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA38')) begin alter table Acumulado_Geral alter column VVDIA38 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA38 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA39')) begin alter table Acumulado_Geral alter column VVDIA39 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA39 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA40')) begin alter table Acumulado_Geral alter column VVDIA40 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA40 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA41')) begin alter table Acumulado_Geral alter column VVDIA41 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA41 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA42')) begin alter table Acumulado_Geral alter column VVDIA42 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA42 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA43')) begin alter table Acumulado_Geral alter column VVDIA43 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA43 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA44')) begin alter table Acumulado_Geral alter column VVDIA44 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA44 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA45')) begin alter table Acumulado_Geral alter column VVDIA45 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA45 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA46')) begin alter table Acumulado_Geral alter column VVDIA46 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA46 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA47')) begin alter table Acumulado_Geral alter column VVDIA47 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA47 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA48')) begin alter table Acumulado_Geral alter column VVDIA48 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA48 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA49')) begin alter table Acumulado_Geral alter column VVDIA49 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA49 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA50')) begin alter table Acumulado_Geral alter column VVDIA50 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA50 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA51')) begin alter table Acumulado_Geral alter column VVDIA51 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA51 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA52')) begin alter table Acumulado_Geral alter column VVDIA52 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA52 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA53')) begin alter table Acumulado_Geral alter column VVDIA53 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA53 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA54')) begin alter table Acumulado_Geral alter column VVDIA54 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA54 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA55')) begin alter table Acumulado_Geral alter column VVDIA55 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA55 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA56')) begin alter table Acumulado_Geral alter column VVDIA56 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA56 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA57')) begin alter table Acumulado_Geral alter column VVDIA57 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA57 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA58')) begin alter table Acumulado_Geral alter column VVDIA58 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA58 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA59')) begin alter table Acumulado_Geral alter column VVDIA59 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA59 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA60')) begin alter table Acumulado_Geral alter column VVDIA60 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA60 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA61')) begin alter table Acumulado_Geral alter column VVDIA61 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA61 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA62')) begin alter table Acumulado_Geral alter column VVDIA62 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA62 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA63')) begin alter table Acumulado_Geral alter column VVDIA63 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA63 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA64')) begin alter table Acumulado_Geral alter column VVDIA64 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA64 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA65')) begin alter table Acumulado_Geral alter column VVDIA65 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA65 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA66')) begin alter table Acumulado_Geral alter column VVDIA66 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA66 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA67')) begin alter table Acumulado_Geral alter column VVDIA67 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA67 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA68')) begin alter table Acumulado_Geral alter column VVDIA68 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA68 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA69')) begin alter table Acumulado_Geral alter column VVDIA69 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA69 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA70')) begin alter table Acumulado_Geral alter column VVDIA70 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA70 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA71')) begin alter table Acumulado_Geral alter column VVDIA71 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA71 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA72')) begin alter table Acumulado_Geral alter column VVDIA72 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA72 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA73')) begin alter table Acumulado_Geral alter column VVDIA73 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA73 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA74')) begin alter table Acumulado_Geral alter column VVDIA74 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA74 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA75')) begin alter table Acumulado_Geral alter column VVDIA75 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA75 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA76')) begin alter table Acumulado_Geral alter column VVDIA76 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA76 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA77')) begin alter table Acumulado_Geral alter column VVDIA77 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA77 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA78')) begin alter table Acumulado_Geral alter column VVDIA78 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA78 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA79')) begin alter table Acumulado_Geral alter column VVDIA79 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA79 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA80')) begin alter table Acumulado_Geral alter column VVDIA80 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA80 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA81')) begin alter table Acumulado_Geral alter column VVDIA81 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA81 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA82')) begin alter table Acumulado_Geral alter column VVDIA82 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA82 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA83')) begin alter table Acumulado_Geral alter column VVDIA83 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA83 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA84')) begin alter table Acumulado_Geral alter column VVDIA84 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA84 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA85')) begin alter table Acumulado_Geral alter column VVDIA85 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA85 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA86')) begin alter table Acumulado_Geral alter column VVDIA86 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA86 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA87')) begin alter table Acumulado_Geral alter column VVDIA87 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA87 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA88')) begin alter table Acumulado_Geral alter column VVDIA88 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA88 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA89')) begin alter table Acumulado_Geral alter column VVDIA89 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA89 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('VVDIA90')) begin alter table Acumulado_Geral alter column VVDIA90 numeric(14,3) end else begin  alter table Acumulado_Geral add VVDIA90 numeric(14,3) end 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE UPPER(TABLE_NAME) = UPPER('Acumulado_Geral') AND UPPER(COLUMN_NAME) = UPPER('EQDIA90')) begin alter table Acumulado_Geral alter column EQDIA90 numeric(14,3) end else begin  alter table Acumulado_Geral add EQDIA90 numeric(14,3) end 
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].sp_Rel_Vendas90Dias') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Vendas90Dias
end
GO
create procedure [dbo].[sp_Rel_Vendas90Dias]

as 

Begin
Select 
	a.PLU, 
	a.descricao, 
	Preco_Custo = CONVERT(FLOAT,a.preco_custo),
	VQDIA01 = convert(FLOAT,a.VQDIA01), 
	VQDIA02 = convert(FLOAT,a.VQDIA02), 
	VQDIA03 = convert(FLOAT,a.VQDIA03), 
	VQDIA04 = convert(FLOAT,a.VQDIA04), 
	VQDIA05 = convert(FLOAT,a.VQDIA05), 
	VQDIA06 = convert(FLOAT,a.VQDIA06), 
	VQDIA07 = convert(FLOAT,a.VQDIA07), 
	VQDIA08 = convert(FLOAT,a.VQDIA08), 
	VQDIA09 = convert(FLOAT,a.VQDIA09), 
	VQDIA10 = convert(FLOAT,a.VQDIA10), 
	VQDIA11 = convert(FLOAT,a.VQDIA11), 
	VQDIA12 = convert(FLOAT,a.VQDIA12), 
	VQDIA13 = convert(FLOAT,a.VQDIA13), 
	VQDIA14 = convert(FLOAT,a.VQDIA14), 
	VQDIA15 = convert(FLOAT,a.VQDIA15), 
	VQDIA16 = convert(FLOAT,a.VQDIA16), 
	VQDIA17 = convert(FLOAT,a.VQDIA17), 
	VQDIA18 = convert(FLOAT,a.VQDIA18), 
	VQDIA19 = convert(FLOAT,a.VQDIA19), 
	VQDIA20 = convert(FLOAT,a.VQDIA20), 
	VQDIA21 = convert(FLOAT,a.VQDIA21), 
	VQDIA22 = convert(FLOAT,a.VQDIA22), 
	VQDIA23 = convert(FLOAT,a.VQDIA23), 
	VQDIA24 = convert(FLOAT,a.VQDIA24), 
	VQDIA25 = convert(FLOAT,a.VQDIA25), 
	VQDIA26 = convert(FLOAT,a.VQDIA26), 
	VQDIA27 = convert(FLOAT,a.VQDIA27), 
	VQDIA28 = convert(FLOAT,a.VQDIA28), 
	VQDIA29 = convert(FLOAT,a.VQDIA29), 
	VQDIA30 = convert(FLOAT,a.VQDIA30), 
	VQDIA31 = convert(FLOAT,a.VQDIA31), 
	VQDIA32 = convert(FLOAT,a.VQDIA32), 
	VQDIA33 = convert(FLOAT,a.VQDIA33), 
	VQDIA34 = convert(FLOAT,a.VQDIA34), 
	VQDIA35 = convert(FLOAT,a.VQDIA35), 
	VQDIA36 = convert(FLOAT,a.VQDIA36), 
	VQDIA37 = convert(FLOAT,a.VQDIA37), 
	VQDIA38 = convert(FLOAT,a.VQDIA38), 
	VQDIA39 = convert(FLOAT,a.VQDIA39), 
	VQDIA40 = convert(FLOAT,a.VQDIA40), 
	VQDIA41 = convert(FLOAT,a.VQDIA41), 
	VQDIA42 = convert(FLOAT,a.VQDIA42), 
	VQDIA43 = convert(FLOAT,a.VQDIA43), 
	VQDIA44 = convert(FLOAT,a.VQDIA44), 
	VQDIA45 = convert(FLOAT,a.VQDIA45), 
	VQDIA46 = convert(FLOAT,a.VQDIA46), 
	VQDIA47 = convert(FLOAT,a.VQDIA47), 
	VQDIA48 = convert(FLOAT,a.VQDIA48), 
	VQDIA49 = convert(FLOAT,a.VQDIA49), 
	VQDIA50 = convert(FLOAT,a.VQDIA50), 
	VQDIA51 = convert(FLOAT,a.VQDIA51), 
	VQDIA52 = convert(FLOAT,a.VQDIA52), 
	VQDIA53 = convert(FLOAT,a.VQDIA53), 
	VQDIA54 = convert(FLOAT,a.VQDIA54), 
	VQDIA55 = convert(FLOAT,a.VQDIA55), 
	VQDIA56 = convert(FLOAT,a.VQDIA56), 
	VQDIA57 = convert(FLOAT,a.VQDIA57), 
	VQDIA58 = convert(FLOAT,a.VQDIA58), 
	VQDIA59 = convert(FLOAT,a.VQDIA59), 
	VQDIA60 = convert(FLOAT,a.VQDIA60), 
	VQDIA61 = convert(FLOAT,a.VQDIA61), 
	VQDIA62 = convert(FLOAT,a.VQDIA62), 
	VQDIA63 = convert(FLOAT,a.VQDIA63), 
	VQDIA64 = convert(FLOAT,a.VQDIA64), 
	VQDIA65 = convert(FLOAT,a.VQDIA65), 
	VQDIA66 = convert(FLOAT,a.VQDIA66), 
	VQDIA67 = convert(FLOAT,a.VQDIA67), 
	VQDIA68 = convert(FLOAT,a.VQDIA68), 
	VQDIA69 = convert(FLOAT,a.VQDIA69), 
	VQDIA70 = convert(FLOAT,a.VQDIA70), 
	VQDIA71 = convert(FLOAT,a.VQDIA71), 
	VQDIA72 = convert(FLOAT,a.VQDIA72), 
	VQDIA73 = convert(FLOAT,a.VQDIA73), 
	VQDIA74 = convert(FLOAT,a.VQDIA74), 
	VQDIA75 = convert(FLOAT,a.VQDIA75), 
	VQDIA76 = convert(FLOAT,a.VQDIA76), 
	VQDIA77 = convert(FLOAT,a.VQDIA77), 
	VQDIA78 = convert(FLOAT,a.VQDIA78), 
	VQDIA79 = convert(FLOAT,a.VQDIA79), 
	VQDIA80 = convert(FLOAT,a.VQDIA80), 
	VQDIA81 = convert(FLOAT,a.VQDIA81), 
	VQDIA82 = convert(FLOAT,a.VQDIA82), 
	VQDIA83 = convert(FLOAT,a.VQDIA83), 
	VQDIA84 = convert(FLOAT,a.VQDIA84), 
	VQDIA85 = convert(FLOAT,a.VQDIA85), 
	VQDIA86 = convert(FLOAT,a.VQDIA86), 
	VQDIA87 = convert(FLOAT,a.VQDIA87), 
	VQDIA88 = convert(FLOAT,a.VQDIA88), 
	VQDIA89 = convert(FLOAT,a.VQDIA89), 
	VQDIA90 = convert(FLOAT,a.VQDIA90), 

	VVDIA01 = convert(FLOAT,a.VVDIA01), 
	VVDIA02 = convert(FLOAT,a.VVDIA02), 
	VVDIA03 = convert(FLOAT,a.VVDIA03), 
	VVDIA04 = convert(FLOAT,a.VVDIA04), 
	VVDIA05 = convert(FLOAT,a.VVDIA05), 
	VVDIA06 = convert(FLOAT,a.VVDIA06), 
	VVDIA07 = convert(FLOAT,a.VVDIA07), 
	VVDIA08 = convert(FLOAT,a.VVDIA08), 
	VVDIA09 = convert(FLOAT,a.VVDIA09), 
	VVDIA10 = convert(FLOAT,a.VVDIA10), 
	VVDIA11 = convert(FLOAT,a.VVDIA11), 
	VVDIA12 = convert(FLOAT,a.VVDIA12), 
	VVDIA13 = convert(FLOAT,a.VVDIA13), 
	VVDIA14 = convert(FLOAT,a.VVDIA14), 
	VVDIA15 = convert(FLOAT,a.VVDIA15), 
	VVDIA16 = convert(FLOAT,a.VVDIA16), 
	VVDIA17 = convert(FLOAT,a.VVDIA17), 
	VVDIA18 = convert(FLOAT,a.VVDIA18), 
	VVDIA19 = convert(FLOAT,a.VVDIA19), 
	VVDIA20 = convert(FLOAT,a.VVDIA20), 
	VVDIA21 = convert(FLOAT,a.VVDIA21), 
	VVDIA22 = convert(FLOAT,a.VVDIA22), 
	VVDIA23 = convert(FLOAT,a.VVDIA23), 
	VVDIA24 = convert(FLOAT,a.VVDIA24), 
	VVDIA25 = convert(FLOAT,a.VVDIA25), 
	VVDIA26 = convert(FLOAT,a.VVDIA26), 
	VVDIA27 = convert(FLOAT,a.VVDIA27), 
	VVDIA28 = convert(FLOAT,a.VVDIA28), 
	VVDIA29 = convert(FLOAT,a.VVDIA29), 
	VVDIA30 = convert(FLOAT,a.VVDIA30), 
	VVDIA31 = convert(FLOAT,a.VVDIA31), 
	VVDIA32 = convert(FLOAT,a.VVDIA32), 
	VVDIA33 = convert(FLOAT,a.VVDIA33), 
	VVDIA34 = convert(FLOAT,a.VVDIA34), 
	VVDIA35 = convert(FLOAT,a.VVDIA35), 
	VVDIA36 = convert(FLOAT,a.VVDIA36), 
	VVDIA37 = convert(FLOAT,a.VVDIA37), 
	VVDIA38 = convert(FLOAT,a.VVDIA38), 
	VVDIA39 = convert(FLOAT,a.VVDIA39), 
	VVDIA40 = convert(FLOAT,a.VVDIA40), 
	VVDIA41 = convert(FLOAT,a.VVDIA41), 
	VVDIA42 = convert(FLOAT,a.VVDIA42), 
	VVDIA43 = convert(FLOAT,a.VVDIA43), 
	VVDIA44 = convert(FLOAT,a.VVDIA44), 
	VVDIA45 = convert(FLOAT,a.VVDIA45), 
	VVDIA46 = convert(FLOAT,a.VVDIA46), 
	VVDIA47 = convert(FLOAT,a.VVDIA47), 
	VVDIA48 = convert(FLOAT,a.VVDIA48), 
	VVDIA49 = convert(FLOAT,a.VVDIA49), 
	VVDIA50 = convert(FLOAT,a.VVDIA50), 
	VVDIA51 = convert(FLOAT,a.VVDIA51), 
	VVDIA52 = convert(FLOAT,a.VVDIA52), 
	VVDIA53 = convert(FLOAT,a.VVDIA53), 
	VVDIA54 = convert(FLOAT,a.VVDIA54), 
	VVDIA55 = convert(FLOAT,a.VVDIA55), 
	VVDIA56 = convert(FLOAT,a.VVDIA56), 
	VVDIA57 = convert(FLOAT,a.VVDIA57), 
	VVDIA58 = convert(FLOAT,a.VVDIA58), 
	VVDIA59 = convert(FLOAT,a.VVDIA59), 
	VVDIA60 = convert(FLOAT,a.VVDIA60), 
	VVDIA61 = convert(FLOAT,a.VVDIA61), 
	VVDIA62 = convert(FLOAT,a.VVDIA62), 
	VVDIA63 = convert(FLOAT,a.VVDIA63), 
	VVDIA64 = convert(FLOAT,a.VVDIA64), 
	VVDIA65 = convert(FLOAT,a.VVDIA65), 
	VVDIA66 = convert(FLOAT,a.VVDIA66), 
	VVDIA67 = convert(FLOAT,a.VVDIA67), 
	VVDIA68 = convert(FLOAT,a.VVDIA68), 
	VVDIA69 = convert(FLOAT,a.VVDIA69), 
	VVDIA70 = convert(FLOAT,a.VVDIA70), 
	VVDIA71 = convert(FLOAT,a.VVDIA71), 
	VVDIA72 = convert(FLOAT,a.VVDIA72), 
	VVDIA73 = convert(FLOAT,a.VVDIA73), 
	VVDIA74 = convert(FLOAT,a.VVDIA74), 
	VVDIA75 = convert(FLOAT,a.VVDIA75), 
	VVDIA76 = convert(FLOAT,a.VVDIA76), 
	VVDIA77 = convert(FLOAT,a.VVDIA77), 
	VVDIA78 = convert(FLOAT,a.VVDIA78), 
	VVDIA79 = convert(FLOAT,a.VVDIA79), 
	VVDIA80 = convert(FLOAT,a.VVDIA80), 
	VVDIA81 = convert(FLOAT,a.VVDIA81), 
	VVDIA82 = convert(FLOAT,a.VVDIA82), 
	VVDIA83 = convert(FLOAT,a.VVDIA83), 
	VVDIA84 = convert(FLOAT,a.VVDIA84), 
	VVDIA85 = convert(FLOAT,a.VVDIA85), 
	VVDIA86 = convert(FLOAT,a.VVDIA86), 
	VVDIA87 = convert(FLOAT,a.VVDIA87), 
	VVDIA88 = convert(FLOAT,a.VVDIA88), 
	VVDIA89 = convert(FLOAT,a.VVDIA89), 
	VVDIA90 = convert(FLOAT,a.VVDIA90), 
	m.marca,
	EQDIA90 = convert(FLOAT,a.EQDIA90),
	DG = '',
	A.DEPARTAMENTO_DESCRICAO,
	Comentarios1 = '',
	Comentarios2 = '',
	Comentarios3 = ''
From
	Acumulado_Geral a inner join mercadoria m on a.PLU = m.plu
End	

GO
/****** Object:  StoredProcedure [dbo].[sp_br_Acumulado_Geral]    Script Date: 08/16/2021 10:56:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  PROCEDURE [dbo].[sp_br_Acumulado_Geral]

 

AS
                DECLARE @FILIAL VARCHAR(20) = 'MATRIZ'

                DECLARE @PLU VARCHAR(17) 

				DECLARE @DIA int = 90                

	--Salva conteudo atual em uma tabela temp
	Select * into #Acumulado_Geral FROM Acumulado_Geral
	
	--Deleta Conteudo tabela temp
	DELETE FROM #Acumulado_Geral

BEGIN

INSERT INTO

   #Acumulado_Geral

SELECT

	FILIAL = ML.FILIAL,

	PLU = M.PLU,

	EAN = ISNULL((SELECT MAX(EAN.EAN) FROM EAN WHERE EAN.PLU = M.PLU),''),

	DESCRICAO = M.DESCRICAO,

	GRUPO = CONVERT(INT, SUBSTRING(M.CODIGO_DEPARTAMENTO, 1, 3)),

	GRUPO_DESCRICAO = ISNULL((SELECT DESCRICAO_GRUPO FROM GRUPO WHERE GRUPO.CODIGO_GRUPO = CONVERT(INT, SUBSTRING(M.CODIGO_DEPARTAMENTO,1,3))),''),

	SUBGRUPO = SUBSTRING(M.CODIGO_DEPARTAMENTO, 1, 6),

	SUBGRUPO_DESCRICAO = ISNULL((SELECT DESCRICAO_SUBGRUPO FROM SUBGRUPO WHERE SUBGRUPO.CODIGO_SUBGRUPO = SUBSTRING(M.CODIGO_DEPARTAMENTO,1,6)),''),

	DEPARTAMENTO = M.CODIGO_DEPARTAMENTO,

	DEPARTAMENTO_DESCRICAO = ISNULL((SELECT DESCRICAO_DEPARTAMENTO FROM DEPARTAMENTO WHERE DEPARTAMENTO.CODIGO_DEPARTAMENTO = M.CODIGO_DEPARTAMENTO),''),

	CST_ICMS = ISNULL(M.ORIGEM,'0') + ISNULL(T.INDICE_ST,'00'),

	ALIQUOTA_ICMS = ISNULL(T.ICMS_EFETIVO, 0),

	CST_PISCOFINS_ENTRADA = ISNULL(M.CST_ENTRADA, '50'),

	CST_PISCOFINS_SAIDA = ISNULL(M.CST_SAIDA, '01'),

	ALIQUOTA_PIS = CASE WHEN CONVERT(INT, ISNULL(M.CST_ENTRADA, 50)) BETWEEN 50 AND 66 THEN FILIAL.PIS ELSE 0 END,

	ALIQUOTA_COFINS = CASE WHEN CONVERT(INT, ISNULL(M.CST_ENTRADA, 50)) BETWEEN 50 AND 66 THEN FILIAL.COFINS ELSE 0 END,

	PRECO_CUSTO = ML.PRECO_CUSTO,

	PRECO_VENDA = ML.PRECO,

	SALDO_ATUAL = ML.SALDO_ATUAL,

	PEDIDO_PENDENTE = 0,

	CUSTOMES13 = ML.PRECO_CUSTO,

	CUSTOMES12 = ML.PRECO_CUSTO,

	CUSTOMES11 = ML.PRECO_CUSTO,

	CUSTOMES10 = ML.PRECO_CUSTO,

	CUSTOMES09 = ML.PRECO_CUSTO,

	CUSTOMES08 = ML.PRECO_CUSTO,

	CUSTOMES07 = ML.PRECO_CUSTO,

	CUSTOMES06 = ML.PRECO_CUSTO,

	CUSTOMES05 = ML.PRECO_CUSTO,

	CUSTOMES04 = ML.PRECO_CUSTO,

	CUSTOMES03 = ML.PRECO_CUSTO,

	CUSTOMES02 = ML.PRECO_CUSTO,

	CUSTOMES01 = ML.PRECO_CUSTO,

	VQMES13 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -12, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -12, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0 ), 0),

	VQMES12 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -11, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -11, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES11 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -10, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -10, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES10 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -9, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -9, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0    ), 0),

	VQMES09 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -8, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -8, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES08 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -7, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -7, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES07 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -6, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -6, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES06 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -5, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -5, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES05 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -4, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -4, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES04 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -3, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -3, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES03 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -2, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -2, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES02 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -1, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -1, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VQMES01 = ISNULL((SELECT SUM(S.QTDE) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, 0, GETDATE()), 102), 1, 7)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO inner join Natureza_operacao as np on np.Codigo_operacao = N.Codigo_operacao WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, 0, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1 AND I.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' AND np.Saida = 1  and isnull(np.NF_devolucao,0) =0   ), 0),

	VVMES13 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -12, GETDATE()), 102), 1, 7)), 0),

	VVMES12 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -11, GETDATE()), 102), 1, 7)), 0),

	VVMES11 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -10, GETDATE()), 102), 1, 7)), 0),

	VVMES10 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -9, GETDATE()), 102), 1, 7)), 0),

	VVMES09 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -8, GETDATE()), 102), 1, 7)), 0),

	VVMES08 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -7, GETDATE()), 102), 1, 7)), 0),

	VVMES07 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -6, GETDATE()), 102), 1, 7)), 0),

	VVMES06 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -5, GETDATE()), 102), 1, 7)), 0),

	VVMES05 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -4, GETDATE()), 102), 1, 7)), 0),

	VVMES04 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -3, GETDATE()), 102), 1, 7)), 0),

	VVMES03 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -2, GETDATE()), 102), 1, 7)), 0),

	VVMES02 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -1, GETDATE()), 102), 1, 7)), 0),

	VVMES01 = ISNULL((SELECT SUM(S.VLR - ISNULL(DESCONTO, 0)) FROM SAIDA_ESTOQUE S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND SUBSTRING(CONVERT(VARCHAR, S.DATA_MOVIMENTO, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, 0, GETDATE()), 102), 1, 7)), 0),

	EQMES13 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -12, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES12 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -11, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES11 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -10, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES10 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -9, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES09 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -8, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES08 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -7, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES07 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -6, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES06 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -5, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES05 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -4, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES04 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -3, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES03 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -2, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES02 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -1, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EQMES01 = ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, 0, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES13 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -12, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES12 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -11, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES11 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -10, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES10 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -9, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES09 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -8, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES08 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -7, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES07 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -6, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES06 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -5, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES05 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -4, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES04 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -3, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES03 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -2, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES02 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, -1, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	EVMES01 = ISNULL((SELECT SUM(I.TOTAL + ISNULL(I.IVA, 0) + ISNULL(I.IPIV, 0)) FROM NF N (INDEX=IX_NF_01) INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01) ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO WHERE N.FILIAL = ML.FILIAL AND SUBSTRING(CONVERT(VARCHAR, N.DATA, 102), 1, 7) = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, 0, GETDATE()), 102), 1, 7) AND I.PLU = M.PLU AND ISNULL(N.NF_CANC,0) <> 1), 0),

	FLAG = '',

	INICIO = '1900-01-01',

	FIM = '1900-01-01',
	VQDIA01 = 0, 
	VQDIA02 = 0, 
	VQDIA03 = 0, 
	VQDIA04 = 0, 
	VQDIA05 = 0, 
	VQDIA06 = 0, 
	VQDIA07 = 0, 
	VQDIA08 = 0, 
	VQDIA09 = 0, 
	VQDIA10 = 0, 
	VQDIA11 = 0, 
	VQDIA12 = 0, 
	VQDIA13 = 0, 
	VQDIA14 = 0, 
	VQDIA15 = 0, 
	VQDIA16 = 0, 
	VQDIA17 = 0, 
	VQDIA18 = 0, 
	VQDIA19 = 0, 
	VQDIA20 = 0, 
	VQDIA21 = 0, 
	VQDIA22 = 0, 
	VQDIA23 = 0, 
	VQDIA24 = 0, 
	VQDIA25 = 0, 
	VQDIA26 = 0, 
	VQDIA27 = 0, 
	VQDIA28 = 0, 
	VQDIA29 = 0, 
	VQDIA30 = 0, 
	VQDIA31 = 0, 
	VQDIA32 = 0, 
	VQDIA33 = 0, 
	VQDIA34 = 0, 
	VQDIA35 = 0, 
	VQDIA36 = 0, 
	VQDIA37 = 0, 
	VQDIA38 = 0, 
	VQDIA39 = 0, 
	VQDIA40 = 0, 
	VQDIA41 = 0, 
	VQDIA42 = 0, 
	VQDIA43 = 0, 
	VQDIA44 = 0, 
	VQDIA45 = 0, 
	VQDIA46 = 0, 
	VQDIA47 = 0, 
	VQDIA48 = 0, 
	VQDIA49 = 0, 
	VQDIA50 = 0, 
	VQDIA51 = 0, 
	VQDIA52 = 0, 
	VQDIA53 = 0, 
	VQDIA54 = 0, 
	VQDIA55 = 0, 
	VQDIA56 = 0, 
	VQDIA57 = 0, 
	VQDIA58 = 0, 
	VQDIA59 = 0, 
	VQDIA60 = 0, 
	VQDIA61 = 0, 
	VQDIA62 = 0, 
	VQDIA63 = 0, 
	VQDIA64 = 0, 
	VQDIA65 = 0, 
	VQDIA66 = 0, 
	VQDIA67 = 0, 
	VQDIA68 = 0, 
	VQDIA69 = 0, 
	VQDIA70 = 0, 
	VQDIA71 = 0, 
	VQDIA72 = 0, 
	VQDIA73 = 0, 
	VQDIA74 = 0, 
	VQDIA75 = 0, 
	VQDIA76 = 0, 
	VQDIA77 = 0, 
	VQDIA78 = 0, 
	VQDIA79 = 0, 
	VQDIA80 = 0, 
	VQDIA81 = 0, 
	VQDIA82 = 0, 
	VQDIA83 = 0, 
	VQDIA84 = 0, 
	VQDIA85 = 0, 
	VQDIA86 = 0, 
	VQDIA87 = 0, 
	VQDIA88 = 0, 
	VQDIA89 = 0, 
	VQDIA90 = 0, 
	VVDIA01 = 0, 
	VVDIA02 = 0, 
	VVDIA03 = 0, 
	VVDIA04 = 0, 
	VVDIA05 = 0, 
	VVDIA06 = 0, 
	VVDIA07 = 0, 
	VVDIA08 = 0, 
	VVDIA09 = 0, 
	VVDIA10 = 0, 
	VVDIA11 = 0, 
	VVDIA12 = 0, 
	VVDIA13 = 0, 
	VVDIA14 = 0, 
	VVDIA15 = 0, 
	VVDIA16 = 0, 
	VVDIA17 = 0, 
	VVDIA18 = 0, 
	VVDIA19 = 0, 
	VVDIA20 = 0, 
	VVDIA21 = 0, 
	VVDIA22 = 0, 
	VVDIA23 = 0, 
	VVDIA24 = 0, 
	VVDIA25 = 0, 
	VVDIA26 = 0, 
	VVDIA27 = 0, 
	VVDIA28 = 0, 
	VVDIA29 = 0, 
	VVDIA30 = 0, 
	VVDIA31 = 0, 
	VVDIA32 = 0, 
	VVDIA33 = 0, 
	VVDIA34 = 0, 
	VVDIA35 = 0, 
	VVDIA36 = 0, 
	VVDIA37 = 0, 
	VVDIA38 = 0, 
	VVDIA39 = 0, 
	VVDIA40 = 0, 
	VVDIA41 = 0, 
	VVDIA42 = 0, 
	VVDIA43 = 0, 
	VVDIA44 = 0, 
	VVDIA45 = 0, 
	VVDIA46 = 0, 
	VVDIA47 = 0, 
	VVDIA48 = 0, 
	VVDIA49 = 0, 
	VVDIA50 = 0, 
	VVDIA51 = 0, 
	VVDIA52 = 0, 
	VVDIA53 = 0, 
	VVDIA54 = 0, 
	VVDIA55 = 0, 
	VVDIA56 = 0, 
	VVDIA57 = 0, 
	VVDIA58 = 0, 
	VVDIA59 = 0, 
	VVDIA60 = 0, 
	VVDIA61 = 0, 
	VVDIA62 = 0, 
	VVDIA63 = 0, 
	VVDIA64 = 0, 
	VVDIA65 = 0, 
	VVDIA66 = 0, 
	VVDIA67 = 0, 
	VVDIA68 = 0, 
	VVDIA69 = 0, 
	VVDIA70 = 0, 
	VVDIA71 = 0, 
	VVDIA72 = 0, 
	VVDIA73 = 0, 
	VVDIA74 = 0, 
	VVDIA75 = 0, 
	VVDIA76 = 0, 
	VVDIA77 = 0, 
	VVDIA78 = 0, 
	VVDIA79 = 0, 
	VVDIA80 = 0, 
	VVDIA81 = 0, 
	VVDIA82 = 0, 
	VVDIA83 = 0, 
	VVDIA84 = 0, 
	VVDIA85 = 0, 
	VVDIA86 = 0, 
	VVDIA87 = 0, 
	VVDIA88 = 0, 
	VVDIA89 = 0, 
	VVDIA90 = 0, 
	EQDIA90 = 0 
FROM
   MERCADORIA M

   INNER JOIN MERCADORIA_LOJA ML ON M.PLU = ML.PLU

   INNER JOIN FILIAL ON FILIAL.FILIAL = ML.FILIAL

   LEFT OUTER JOIN TRIBUTACAO T ON M.CODIGO_TRIBUTACAO = T.CODIGO_TRIBUTACAO
WHERE
	M.PLU <> '999999'
And 
	M.Inativo = 0
And 
	M.FILIAL = 'MATRIZ'
	
	
IF (SELECT COUNT(*) FROM #Acumulado_Geral) > 0
BEGIN	
	delete Acumulado_Geral
	
	insert into Acumulado_Geral
	Select * FROM #Acumulado_Geral 							
-------------------******************************************************************************----------------------------

--DROP TABLE #venda
-- Tabela Temp Saida Estoque
SELECT S.* into #venda 
FROM SAIDA_ESTOQUE S (INDEX=IX_SAIDA_ESTOQUE_01), MERCADORIA_LOJA ML WHERE S.PLU = ML.PLU
AND S.FILIAL = ML.FILIAL  AND S.PLU = ML.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) >= CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112) 
AND S.DATA_CANCELAMENTO IS NULL

-- Tabela Temp VendasNF
--drop table #vendaNF
SELECT 
	I.*,N.DATA into #vendaNF
FROM
	NF N (INDEX=IX_NF_01)	INNER JOIN NF_ITEM I (INDEX=IX_NF_ITEM_01)  
								ON N.FILIAL = I.FILIAL AND N.CLIENTE_FORNECEDOR = I.CLIENTE_FORNECEDOR AND N.CODIGO = I.CODIGO 
							INNER JOIN Natureza_operacao as np 
								ON np.Codigo_operacao = N.Codigo_operacao 
							INNER JOIN MERCADORIA_LOJA ML 								
								ON I.PLU = ML.PLU 								
Where N.FILIAL = ML.FILIAL 
AND I.PLU = ML.PLU 
AND ISNULL(N.NF_CANC,0) <> 1 
AND N.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') AND N.TIPO_NF = 1 AND N.status='AUTORIZADO' 
AND CONVERT(VARCHAR, N.DATA, 112) >= CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112) 
AND np.Saida = 1 
AND isnull(np.NF_devolucao,0) = 0

Update 
	Acumulado_Geral
Set
	VQDIA01 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA02 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -2, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -2, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA03 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -3, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -3, GETDATE()), 112) AND I.PLU = M.PLU),0),		
	VQDIA04 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -4, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -4, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA05 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -5, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -5, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA06 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -6, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -6, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA07 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -7, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -7, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA08 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -8, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -8, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA09 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -9, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -9, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA10 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -10, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -10, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA11 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -11, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -11, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA12 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -12, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -12, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA13 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -13, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -13, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA14 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -14, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -14, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA15 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -15, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -15, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA16 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -16, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -16, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA17 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -17, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -17, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA18 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -18, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -18, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA19 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -19, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -19, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA20 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -20, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -20, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA21 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -21, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -21, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA22 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -22, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -22, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA23 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -23, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -23, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA24 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -24, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -24, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA25 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -25, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -25, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA26 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -26, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -26, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA27 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -27, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -27, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA28 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -28, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -28, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA29 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -29, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -29, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA30 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA31 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -31, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -31, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA32 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -32, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -32, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA33 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -33, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -33, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA34 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -34, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -34, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA35 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -35, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -35, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA36 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -36, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -36, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA37 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -37, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -37, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA38 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -38, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -38, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA39 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -39, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -39, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA40 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -40, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -40, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA41 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -41, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -41, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA42 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -42, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -42, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA43 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -43, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -43, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA44 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -44, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -44, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA45 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -45, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -45, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA46 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -46, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -46, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA47 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -47, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -47, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA48 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -48, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -48, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA49 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -49, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -49, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA50 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -50, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -50, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA51 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -51, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -51, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA52 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -52, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -52, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA53 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -53, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -53, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA54 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -54, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -54, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA55 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -55, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -55, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA56 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -56, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -56, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA57 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -57, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -57, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA58 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -58, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -58, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA59 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -59, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -59, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA60 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -60, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -60, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA61 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -61, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -61, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA62 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -62, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -62, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA63 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -63, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -63, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA64 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -64, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -64, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA65 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -65, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -65, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA66 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -66, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -66, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA67 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -67, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -67, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA68 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -68, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -68, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA69 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -69, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -69, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA70 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -70, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -70, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA71 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -71, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -71, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA72 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -72, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -72, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA73 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -73, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -73, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA74 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -74, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -74, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA75 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -75, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -75, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA76 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -76, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -76, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA77 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -77, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -77, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA78 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -78, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -78, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA79 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -79, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -79, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA80 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -80, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -80, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA81 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -81, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -81, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA82 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -82, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -82, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA83 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -83, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -83, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA84 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -84, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -84, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA85 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -85, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -85, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA86 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -86, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -86, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA87 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -87, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -87, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA88 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -88, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -88, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA89 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -89, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -89, GETDATE()), 112) AND I.PLU = M.PLU),0),	
	VQDIA90 = ISNULL((SELECT SUM(S.QTDE) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = M.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112)), 0) + ISNULL((SELECT SUM(I.QTDE * I.EMBALAGEM) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112) AND I.PLU = M.PLU),0)	
FROM 
	MERCADORIA M
	INNER JOIN MERCADORIA_LOJA ML ON M.PLU = ML.PLU
	INNER JOIN FILIAL ON FILIAL.FILIAL = ML.FILIAL
WHERE
	ML.Filial = 'MATRIZ'
AND	
	M.PLU <> '999999'
AND 
	Acumulado_Geral.plu = ml.plu	
And
	inativo = 0	


-------------------------------------*****************************************************--------------------------------------------									

SELECT 
	a.PLU,
	VVDIA01 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -1, GETDATE()), 112)), 0),
	VVDIA02 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -2, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -2, GETDATE()), 112)), 0),
	VVDIA03 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -3, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -3, GETDATE()), 112)), 0),
	VVDIA04 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -4, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -4, GETDATE()), 112)), 0),
	VVDIA05 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -5, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -5, GETDATE()), 112)), 0),
	VVDIA06 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -6, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -6, GETDATE()), 112)), 0),
	VVDIA07 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -7, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -7, GETDATE()), 112)), 0),
	VVDIA08 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -8, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -8, GETDATE()), 112)), 0),
	VVDIA09 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -9, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -9, GETDATE()), 112)), 0),
	VVDIA10 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -10, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -10, GETDATE()), 112)), 0),
	VVDIA11 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -11, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -11, GETDATE()), 112)), 0),
	VVDIA12 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -12, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -12, GETDATE()), 112)), 0),
	VVDIA13 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -13, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -13, GETDATE()), 112)), 0),
	VVDIA14 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -14, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -14, GETDATE()), 112)), 0),
	VVDIA15 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -15, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -15, GETDATE()), 112)), 0),
	VVDIA16 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -16, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -16, GETDATE()), 112)), 0),
	VVDIA17 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -17, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -17, GETDATE()), 112)), 0),
	VVDIA18 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -18, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -18, GETDATE()), 112)), 0),
	VVDIA19 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -19, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -19, GETDATE()), 112)), 0),
	VVDIA20 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -20, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -20, GETDATE()), 112)), 0),
	VVDIA21 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -21, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -21, GETDATE()), 112)), 0),
	VVDIA22 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -22, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -22, GETDATE()), 112)), 0),
	VVDIA23 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -23, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -23, GETDATE()), 112)), 0),
	VVDIA24 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -24, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -24, GETDATE()), 112)), 0),
	VVDIA25 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -25, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -25, GETDATE()), 112)), 0),
	VVDIA26 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -26, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -26, GETDATE()), 112)), 0),
	VVDIA27 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -27, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -27, GETDATE()), 112)), 0),
	VVDIA28 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -28, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -28, GETDATE()), 112)), 0),
	VVDIA29 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -29, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -29, GETDATE()), 112)), 0),
	VVDIA30 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -30, GETDATE()), 112)), 0),
	VVDIA31 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -31, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -31, GETDATE()), 112)), 0),
	VVDIA32 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -32, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -32, GETDATE()), 112)), 0),
	VVDIA33 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -33, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -33, GETDATE()), 112)), 0),
	VVDIA34 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -34, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -34, GETDATE()), 112)), 0),
	VVDIA35 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -35, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -35, GETDATE()), 112)), 0),
	VVDIA36 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -36, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -36, GETDATE()), 112)), 0),
	VVDIA37 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -37, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -37, GETDATE()), 112)), 0),
	VVDIA38 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -38, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -38, GETDATE()), 112)), 0),
	VVDIA39 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -39, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -39, GETDATE()), 112)), 0),
	VVDIA40 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -40, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -40, GETDATE()), 112)), 0),
	VVDIA41 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -41, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -41, GETDATE()), 112)), 0),
	VVDIA42 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -42, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -42, GETDATE()), 112)), 0),
	VVDIA43 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -43, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -43, GETDATE()), 112)), 0),
	VVDIA44 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -44, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -44, GETDATE()), 112)), 0),
	VVDIA45 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -45, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -45, GETDATE()), 112)), 0),
	VVDIA46 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -46, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -46, GETDATE()), 112)), 0),
	VVDIA47 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -47, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -47, GETDATE()), 112)), 0),
	VVDIA48 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -48, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -48, GETDATE()), 112)), 0),
	VVDIA49 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -49, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -49, GETDATE()), 112)), 0),
	VVDIA50 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -50, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -50, GETDATE()), 112)), 0),
	VVDIA51 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -51, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -51, GETDATE()), 112)), 0),
	VVDIA52 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -52, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -52, GETDATE()), 112)), 0),
	VVDIA53 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -53, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -53, GETDATE()), 112)), 0),
	VVDIA54 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -54, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -54, GETDATE()), 112)), 0),
	VVDIA55 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -55, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -55, GETDATE()), 112)), 0),
	VVDIA56 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -56, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -56, GETDATE()), 112)), 0),
	VVDIA57 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -57, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -57, GETDATE()), 112)), 0),
	VVDIA58 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -58, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -58, GETDATE()), 112)), 0),
	VVDIA59 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -59, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -59, GETDATE()), 112)), 0),
	VVDIA60 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -60, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -60, GETDATE()), 112)), 0),
	VVDIA61 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -61, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -61, GETDATE()), 112)), 0),
	VVDIA62 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -62, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -62, GETDATE()), 112)), 0),
	VVDIA63 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -63, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -63, GETDATE()), 112)), 0),
	VVDIA64 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -64, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -64, GETDATE()), 112)), 0),
	VVDIA65 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -65, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -65, GETDATE()), 112)), 0),
	VVDIA66 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -66, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -66, GETDATE()), 112)), 0),
	VVDIA67 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -67, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -67, GETDATE()), 112)), 0),
	VVDIA68 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -68, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -68, GETDATE()), 112)), 0),
	VVDIA69 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -69, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -69, GETDATE()), 112)), 0),
	VVDIA70 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -70, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -70, GETDATE()), 112)), 0),
	VVDIA71 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -71, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -71, GETDATE()), 112)), 0),
	VVDIA72 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -72, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -72, GETDATE()), 112)), 0),
	VVDIA73 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -73, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -73, GETDATE()), 112)), 0),
	VVDIA74 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -74, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -74, GETDATE()), 112)), 0),
	VVDIA75 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -75, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -75, GETDATE()), 112)), 0),
	VVDIA76 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -76, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -76, GETDATE()), 112)), 0),
	VVDIA77 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -77, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -77, GETDATE()), 112)), 0),
	VVDIA78 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -78, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -78, GETDATE()), 112)), 0),
	VVDIA79 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -79, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -79, GETDATE()), 112)), 0),
	VVDIA80 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -80, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -80, GETDATE()), 112)), 0),
	VVDIA81 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -81, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -81, GETDATE()), 112)), 0),
	VVDIA82 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -82, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -82, GETDATE()), 112)), 0),
	VVDIA83 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -83, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -83, GETDATE()), 112)), 0),
	VVDIA84 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -84, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -84, GETDATE()), 112)), 0),
	VVDIA85 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -85, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -85, GETDATE()), 112)), 0),
	VVDIA86 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -86, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -86, GETDATE()), 112)), 0),
	VVDIA87 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -87, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -87, GETDATE()), 112)), 0),
	VVDIA88 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -88, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -88, GETDATE()), 112)), 0),
	VVDIA89 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -89, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -89, GETDATE()), 112)), 0),
	VVDIA90 = ISNULL((SELECT (Isnull(SUM(S.VLR - ISNULL(DESCONTO,0)),0)+ISNULL((SELECT ISNULL(SUM(I.TOTAL),0) FROM #vendaNF I WHERE I.FILIAL = ML.FILIAL AND CONVERT(VARCHAR, I.DATA, 112) = CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112) AND I.PLU = a.PLU),0)) FROM #venda S WHERE S.FILIAL = ML.FILIAL  AND S.PLU = a.PLU AND CONVERT(VARCHAR, S.DATA_MOVIMENTO, 112) = CONVERT(VARCHAR, DATEADD(DAY, -90, GETDATE()), 112)), 0)	
INTO 	
	#VVDIA
FROM 
	 MERCADORIA_LOJA ML INNER JOIN Acumulado_Geral a ON a.plu = ml.plu
WHERE 
	ML.Filial = 'MATRIZ'
And	
	ML.PLU <> '999999'
GROUP BY 
	ml.filial, ml.plu,a.plu

	
UPDATE
	Acumulado_Geral
Set
	VVDIA01= case when (Acumulado_Geral.VQDIA01 > 0 And v.VVDIA01 > 0) then v.VVDIA01/VQDIA01 ELSE 0 END, 
	VVDIA02= case when (Acumulado_Geral.VQDIA02 > 0 And v.VVDIA02 > 0) then v.VVDIA02/VQDIA02 ELSE 0 END, 
	VVDIA03= case when (Acumulado_Geral.VQDIA03 > 0 And v.VVDIA03 > 0) then v.VVDIA03/VQDIA03 ELSE 0 END, 
	VVDIA04= case when (Acumulado_Geral.VQDIA04 > 0 And v.VVDIA04 > 0) then v.VVDIA04/VQDIA04 ELSE 0 END, 
	VVDIA05= case when (Acumulado_Geral.VQDIA05 > 0 And v.VVDIA05 > 0) then v.VVDIA05/VQDIA05 ELSE 0 END, 
	VVDIA06= case when (Acumulado_Geral.VQDIA06 > 0 And v.VVDIA06 > 0) then v.VVDIA06/VQDIA06 ELSE 0 END, 
	VVDIA07= case when (Acumulado_Geral.VQDIA07 > 0 And v.VVDIA07 > 0) then v.VVDIA07/VQDIA07 ELSE 0 END, 
	VVDIA08= case when (Acumulado_Geral.VQDIA08 > 0 And v.VVDIA08 > 0) then v.VVDIA08/VQDIA08 ELSE 0 END, 
	VVDIA09= case when (Acumulado_Geral.VQDIA09 > 0 And v.VVDIA09 > 0) then v.VVDIA09/VQDIA09 ELSE 0 END, 
	VVDIA10= case when (Acumulado_Geral.VQDIA10 > 0 And v.VVDIA10 > 0) then v.VVDIA10/VQDIA10 ELSE 0 END, 
	VVDIA11= case when (Acumulado_Geral.VQDIA11 > 0 And v.VVDIA11 > 0) then v.VVDIA11/VQDIA11 ELSE 0 END, 
	VVDIA12= case when (Acumulado_Geral.VQDIA12 > 0 And v.VVDIA12 > 0) then v.VVDIA12/VQDIA12 ELSE 0 END, 
	VVDIA13= case when (Acumulado_Geral.VQDIA13 > 0 And v.VVDIA13 > 0) then v.VVDIA13/VQDIA13 ELSE 0 END, 
	VVDIA14= case when (Acumulado_Geral.VQDIA14 > 0 And v.VVDIA14 > 0) then v.VVDIA14/VQDIA14 ELSE 0 END, 
	VVDIA15= case when (Acumulado_Geral.VQDIA15 > 0 And v.VVDIA15 > 0) then v.VVDIA15/VQDIA15 ELSE 0 END, 
	VVDIA16= case when (Acumulado_Geral.VQDIA16 > 0 And v.VVDIA16 > 0) then v.VVDIA16/VQDIA16 ELSE 0 END, 
	VVDIA17= case when (Acumulado_Geral.VQDIA17 > 0 And v.VVDIA17 > 0) then v.VVDIA17/VQDIA17 ELSE 0 END, 
	VVDIA18= case when (Acumulado_Geral.VQDIA18 > 0 And v.VVDIA18 > 0) then v.VVDIA18/VQDIA18 ELSE 0 END, 
	VVDIA19= case when (Acumulado_Geral.VQDIA19 > 0 And v.VVDIA19 > 0) then v.VVDIA19/VQDIA19 ELSE 0 END, 
	VVDIA20= case when (Acumulado_Geral.VQDIA20 > 0 And v.VVDIA20 > 0) then v.VVDIA20/VQDIA20 ELSE 0 END, 
	VVDIA21= case when (Acumulado_Geral.VQDIA21 > 0 And v.VVDIA21 > 0) then v.VVDIA21/VQDIA21 ELSE 0 END, 
	VVDIA22= case when (Acumulado_Geral.VQDIA22 > 0 And v.VVDIA22 > 0) then v.VVDIA22/VQDIA22 ELSE 0 END, 
	VVDIA23= case when (Acumulado_Geral.VQDIA23 > 0 And v.VVDIA23 > 0) then v.VVDIA23/VQDIA23 ELSE 0 END, 
	VVDIA24= case when (Acumulado_Geral.VQDIA24 > 0 And v.VVDIA24 > 0) then v.VVDIA24/VQDIA24 ELSE 0 END, 
	VVDIA25= case when (Acumulado_Geral.VQDIA25 > 0 And v.VVDIA25 > 0) then v.VVDIA25/VQDIA25 ELSE 0 END, 
	VVDIA26= case when (Acumulado_Geral.VQDIA26 > 0 And v.VVDIA26 > 0) then v.VVDIA26/VQDIA26 ELSE 0 END, 
	VVDIA27= case when (Acumulado_Geral.VQDIA27 > 0 And v.VVDIA27 > 0) then v.VVDIA27/VQDIA27 ELSE 0 END, 
	VVDIA28= case when (Acumulado_Geral.VQDIA28 > 0 And v.VVDIA28 > 0) then v.VVDIA28/VQDIA28 ELSE 0 END, 
	VVDIA29= case when (Acumulado_Geral.VQDIA29 > 0 And v.VVDIA29 > 0) then v.VVDIA29/VQDIA29 ELSE 0 END, 
	VVDIA30= case when (Acumulado_Geral.VQDIA30 > 0 And v.VVDIA30 > 0) then v.VVDIA30/VQDIA30 ELSE 0 END, 
	VVDIA31= case when (Acumulado_Geral.VQDIA31 > 0 And v.VVDIA31 > 0) then v.VVDIA31/VQDIA31 ELSE 0 END, 
	VVDIA32= case when (Acumulado_Geral.VQDIA32 > 0 And v.VVDIA32 > 0) then v.VVDIA32/VQDIA32 ELSE 0 END, 
	VVDIA33= case when (Acumulado_Geral.VQDIA33 > 0 And v.VVDIA33 > 0) then v.VVDIA33/VQDIA33 ELSE 0 END, 
	VVDIA34= case when (Acumulado_Geral.VQDIA34 > 0 And v.VVDIA34 > 0) then v.VVDIA34/VQDIA34 ELSE 0 END, 
	VVDIA35= case when (Acumulado_Geral.VQDIA35 > 0 And v.VVDIA35 > 0) then v.VVDIA35/VQDIA35 ELSE 0 END, 
	VVDIA36= case when (Acumulado_Geral.VQDIA36 > 0 And v.VVDIA36 > 0) then v.VVDIA36/VQDIA36 ELSE 0 END, 
	VVDIA37= case when (Acumulado_Geral.VQDIA37 > 0 And v.VVDIA37 > 0) then v.VVDIA37/VQDIA37 ELSE 0 END, 
	VVDIA38= case when (Acumulado_Geral.VQDIA38 > 0 And v.VVDIA38 > 0) then v.VVDIA38/VQDIA38 ELSE 0 END, 
	VVDIA39= case when (Acumulado_Geral.VQDIA39 > 0 And v.VVDIA39 > 0) then v.VVDIA39/VQDIA39 ELSE 0 END, 
	VVDIA40= case when (Acumulado_Geral.VQDIA40 > 0 And v.VVDIA40 > 0) then v.VVDIA40/VQDIA40 ELSE 0 END, 
	VVDIA41= case when (Acumulado_Geral.VQDIA41 > 0 And v.VVDIA41 > 0) then v.VVDIA41/VQDIA41 ELSE 0 END, 
	VVDIA42= case when (Acumulado_Geral.VQDIA42 > 0 And v.VVDIA42 > 0) then v.VVDIA42/VQDIA42 ELSE 0 END, 
	VVDIA43= case when (Acumulado_Geral.VQDIA43 > 0 And v.VVDIA43 > 0) then v.VVDIA43/VQDIA43 ELSE 0 END, 
	VVDIA44= case when (Acumulado_Geral.VQDIA44 > 0 And v.VVDIA44 > 0) then v.VVDIA44/VQDIA44 ELSE 0 END, 
	VVDIA45= case when (Acumulado_Geral.VQDIA45 > 0 And v.VVDIA45 > 0) then v.VVDIA45/VQDIA45 ELSE 0 END, 
	VVDIA46= case when (Acumulado_Geral.VQDIA46 > 0 And v.VVDIA46 > 0) then v.VVDIA46/VQDIA46 ELSE 0 END, 
	VVDIA47= case when (Acumulado_Geral.VQDIA47 > 0 And v.VVDIA47 > 0) then v.VVDIA47/VQDIA47 ELSE 0 END, 
	VVDIA48= case when (Acumulado_Geral.VQDIA48 > 0 And v.VVDIA48 > 0) then v.VVDIA48/VQDIA48 ELSE 0 END, 
	VVDIA49= case when (Acumulado_Geral.VQDIA49 > 0 And v.VVDIA49 > 0) then v.VVDIA49/VQDIA49 ELSE 0 END, 
	VVDIA50= case when (Acumulado_Geral.VQDIA50 > 0 And v.VVDIA50 > 0) then v.VVDIA50/VQDIA50 ELSE 0 END, 
	VVDIA51= case when (Acumulado_Geral.VQDIA51 > 0 And v.VVDIA51 > 0) then v.VVDIA51/VQDIA51 ELSE 0 END, 
	VVDIA52= case when (Acumulado_Geral.VQDIA52 > 0 And v.VVDIA52 > 0) then v.VVDIA52/VQDIA52 ELSE 0 END, 
	VVDIA53= case when (Acumulado_Geral.VQDIA53 > 0 And v.VVDIA53 > 0) then v.VVDIA53/VQDIA53 ELSE 0 END, 
	VVDIA54= case when (Acumulado_Geral.VQDIA54 > 0 And v.VVDIA54 > 0) then v.VVDIA54/VQDIA54 ELSE 0 END, 
	VVDIA55= case when (Acumulado_Geral.VQDIA55 > 0 And v.VVDIA55 > 0) then v.VVDIA55/VQDIA55 ELSE 0 END, 
	VVDIA56= case when (Acumulado_Geral.VQDIA56 > 0 And v.VVDIA56 > 0) then v.VVDIA56/VQDIA56 ELSE 0 END, 
	VVDIA57= case when (Acumulado_Geral.VQDIA57 > 0 And v.VVDIA57 > 0) then v.VVDIA57/VQDIA57 ELSE 0 END, 
	VVDIA58= case when (Acumulado_Geral.VQDIA58 > 0 And v.VVDIA58 > 0) then v.VVDIA58/VQDIA58 ELSE 0 END, 
	VVDIA59= case when (Acumulado_Geral.VQDIA59 > 0 And v.VVDIA59 > 0) then v.VVDIA59/VQDIA59 ELSE 0 END, 
	VVDIA60= case when (Acumulado_Geral.VQDIA60 > 0 And v.VVDIA60 > 0) then v.VVDIA60/VQDIA60 ELSE 0 END, 
	VVDIA61= case when (Acumulado_Geral.VQDIA61 > 0 And v.VVDIA61 > 0) then v.VVDIA61/VQDIA61 ELSE 0 END, 
	VVDIA62= case when (Acumulado_Geral.VQDIA62 > 0 And v.VVDIA62 > 0) then v.VVDIA62/VQDIA62 ELSE 0 END, 
	VVDIA63= case when (Acumulado_Geral.VQDIA63 > 0 And v.VVDIA63 > 0) then v.VVDIA63/VQDIA63 ELSE 0 END, 
	VVDIA64= case when (Acumulado_Geral.VQDIA64 > 0 And v.VVDIA64 > 0) then v.VVDIA64/VQDIA64 ELSE 0 END, 
	VVDIA65= case when (Acumulado_Geral.VQDIA65 > 0 And v.VVDIA65 > 0) then v.VVDIA65/VQDIA65 ELSE 0 END, 
	VVDIA66= case when (Acumulado_Geral.VQDIA66 > 0 And v.VVDIA66 > 0) then v.VVDIA66/VQDIA66 ELSE 0 END, 
	VVDIA67= case when (Acumulado_Geral.VQDIA67 > 0 And v.VVDIA67 > 0) then v.VVDIA67/VQDIA67 ELSE 0 END, 
	VVDIA68= case when (Acumulado_Geral.VQDIA68 > 0 And v.VVDIA68 > 0) then v.VVDIA68/VQDIA68 ELSE 0 END, 
	VVDIA69= case when (Acumulado_Geral.VQDIA69 > 0 And v.VVDIA69 > 0) then v.VVDIA69/VQDIA69 ELSE 0 END, 
	VVDIA70= case when (Acumulado_Geral.VQDIA70 > 0 And v.VVDIA70 > 0) then v.VVDIA70/VQDIA70 ELSE 0 END, 
	VVDIA71= case when (Acumulado_Geral.VQDIA71 > 0 And v.VVDIA71 > 0) then v.VVDIA71/VQDIA71 ELSE 0 END, 
	VVDIA72= case when (Acumulado_Geral.VQDIA72 > 0 And v.VVDIA72 > 0) then v.VVDIA72/VQDIA72 ELSE 0 END, 
	VVDIA73= case when (Acumulado_Geral.VQDIA73 > 0 And v.VVDIA73 > 0) then v.VVDIA73/VQDIA73 ELSE 0 END, 
	VVDIA74= case when (Acumulado_Geral.VQDIA74 > 0 And v.VVDIA74 > 0) then v.VVDIA74/VQDIA74 ELSE 0 END, 
	VVDIA75= case when (Acumulado_Geral.VQDIA75 > 0 And v.VVDIA75 > 0) then v.VVDIA75/VQDIA75 ELSE 0 END, 
	VVDIA76= case when (Acumulado_Geral.VQDIA76 > 0 And v.VVDIA76 > 0) then v.VVDIA76/VQDIA76 ELSE 0 END, 
	VVDIA77= case when (Acumulado_Geral.VQDIA77 > 0 And v.VVDIA77 > 0) then v.VVDIA77/VQDIA77 ELSE 0 END, 
	VVDIA78= case when (Acumulado_Geral.VQDIA78 > 0 And v.VVDIA78 > 0) then v.VVDIA78/VQDIA78 ELSE 0 END, 
	VVDIA79= case when (Acumulado_Geral.VQDIA79 > 0 And v.VVDIA79 > 0) then v.VVDIA79/VQDIA79 ELSE 0 END, 
	VVDIA80= case when (Acumulado_Geral.VQDIA80 > 0 And v.VVDIA80 > 0) then v.VVDIA80/VQDIA80 ELSE 0 END, 
	VVDIA81= case when (Acumulado_Geral.VQDIA81 > 0 And v.VVDIA81 > 0) then v.VVDIA81/VQDIA81 ELSE 0 END, 
	VVDIA82= case when (Acumulado_Geral.VQDIA82 > 0 And v.VVDIA82 > 0) then v.VVDIA82/VQDIA82 ELSE 0 END, 
	VVDIA83= case when (Acumulado_Geral.VQDIA83 > 0 And v.VVDIA83 > 0) then v.VVDIA83/VQDIA83 ELSE 0 END, 
	VVDIA84= case when (Acumulado_Geral.VQDIA84 > 0 And v.VVDIA84 > 0) then v.VVDIA84/VQDIA84 ELSE 0 END, 
	VVDIA85= case when (Acumulado_Geral.VQDIA85 > 0 And v.VVDIA85 > 0) then v.VVDIA85/VQDIA85 ELSE 0 END, 
	VVDIA86= case when (Acumulado_Geral.VQDIA86 > 0 And v.VVDIA86 > 0) then v.VVDIA86/VQDIA86 ELSE 0 END, 
	VVDIA87= case when (Acumulado_Geral.VQDIA87 > 0 And v.VVDIA87 > 0) then v.VVDIA87/VQDIA87 ELSE 0 END, 
	VVDIA88= case when (Acumulado_Geral.VQDIA88 > 0 And v.VVDIA88 > 0) then v.VVDIA88/VQDIA88 ELSE 0 END, 
	VVDIA89= case when (Acumulado_Geral.VQDIA89 > 0 And v.VVDIA89 > 0) then v.VVDIA89/VQDIA89 ELSE 0 END, 
	VVDIA90= case when (Acumulado_Geral.VQDIA90 > 0 And v.VVDIA90 > 0) then v.VVDIA90/VQDIA90 ELSE 0 END
FROM 
	#VVDIA v
WHERE
	v.plu = Acumulado_Geral.plu

	
--- Calculo Estoque 90 Dias

SELECT 
	M.PLU,
	M.SALDO_ATUAL,
	SaidaPDV = isnull((Select Sum(s.Qtde) From saida_estoque s with (index=ix_saida_estoque) inner join mercadoria_loja l on s.plu = l.plu
						Where s.Filial = M.FILIAL 
						and convert(varchar,s.Data_movimento,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)						
						And s.data_cancelamento is null
						And S.PLU = M.PLU), 0),
	SaidaNF = isnull((Select Sum( ((i.qtde * i.embalagem)  )  )
						From nf Inner Join Natureza_operacao n on n.codigo_operacao = nf.Codigo_operacao 
								Inner Join nf_item i on nf.Cliente_Fornecedor = i.Cliente_Fornecedor 
								And nf.Tipo_NF = i.Tipo_NF 
								And nf.codigo = i.codigo 
						Where isnull(nf.nf_canc,0) <> 1
						And nf.tipo_nf = 1
						And n.baixa_estoque = 1
						and convert(varchar,nf.Data,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)												
						And NF.FIlial = M.FILIAL
						And i.PLU = m.plu), 0),
	OutrasSaidas = Isnull((
			(select Isnull(SUM(Contada),0) from inventario_itens i inner join inventario v on i.Codigo_inventario = v.Codigo_inventario 
			and v.status = 'ENCERRADO'
			and convert(varchar,v.Data,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)															
			and i.PLU = m.plu
			And i.Filial = M.FILIAL
			and exists(select * from Tipo_movimentacao t where t.Movimentacao = v.tipoMovimentacao and t.saida = 1)) 
		),0),								
	OutrasEntradas = Isnull((
			(select Isnull(SUM(Contada),0) from inventario_itens i inner join inventario v on i.Codigo_inventario = v.Codigo_inventario 
			and v.status = 'ENCERRADO'
			and convert(varchar,v.Data,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)															
			and i.PLU = m.plu
			And i.Filial = M.FILIAL
			and exists(select * from Tipo_movimentacao t where t.Movimentacao = v.tipoMovimentacao and t.saida = 0)) 
		),0),						
	EntradaNF = isnull((Select Sum( ((i.qtde * i.embalagem)  )  )
						From nf Inner Join Natureza_operacao n on n.codigo_operacao = nf.Codigo_operacao 
								Inner Join nf_item i on nf.Cliente_Fornecedor = i.Cliente_Fornecedor 
								And nf.Tipo_NF = i.Tipo_NF 
								And nf.codigo = i.codigo 
						Where isnull(nf.nf_canc,0) <> 1
						And nf.tipo_nf = 2
						And n.baixa_estoque = 1
						and convert(varchar,nf.Data,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)																		
						And NF.Filial = M.FILIAL
						And i.PLU = m.plu), 0),
	DivInv = Isnull((
			(select Isnull(SUM((i.Contada-i.Saldo_Atual)),0) from inventario_itens i inner join inventario v on i.Codigo_inventario = v.Codigo_inventario 
			and v.status = 'ENCERRADO' and v.tipoMovimentacao  = 'INVENTARIO'
			and convert(varchar,v.Data,112) between CONVERT(VARCHAR, DATEADD(DAY, -@Dia, GETDATE()), 112) And convert(varchar,GETDATE(),112)															
			and i.PLU = m.plu)),0),
	EQDIA90 = 0					
into #ColunasEstoque90Dias
From 
	Mercadoria_loja m
Where
	m.Filial = 'MATRIZ'	

--faz o calculo E SETA A COLUNA EQDIA90
Update #ColunasEstoque90Dias set EQDIA90 = (Isnull(saldo_atual,0) + Isnull(OutrasSaidas,0) + Isnull(SaidaPDV,0) + Isnull(SaidaNF,0) - Isnull(EntradaNF,0) - Isnull(OutrasEntradas,0)+(Isnull(DivInv,0)*-1) )	

--Altera na tabela Acumulado_Geral
Update 
	Acumulado_Geral 
Set 
	Acumulado_Geral.EQDIA90 = e.EQDIA90
From
	#ColunasEstoque90Dias e 
Where
	e.plu = Acumulado_Geral.Plu
	
	
END	

END
                           


GO
insert into Versoes_Atualizadas select 'Versão:1.296.866', getdate();