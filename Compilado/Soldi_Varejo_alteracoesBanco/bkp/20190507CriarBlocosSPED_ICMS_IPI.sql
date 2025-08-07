


IF not  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sped_blocos]') AND type in (N'U'))
begin
	create table sped_blocos
	(
			filial varchar(40),
			id int,
			tipoArquivo varchar(40),
			bloco varchar(10),
			str_procedure varchar(50),
			id_bloco_pai int,
			ordem int ,
			gerarArquivo tinyint,
			campoArquivo varchar(40),
			blocoTotaliza varchar(40),
			bloco_grupo tinyint 
			
	)
end
else
begin 
			IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
					WHERE UPPER(TABLE_NAME) = UPPER('sped_blocos') 
					AND  UPPER(COLUMN_NAME) = UPPER('bloco_grupo'))
		begin
			alter table sped_blocos alter column bloco_grupo tinyint
		end
		else
		begin
			alter table sped_blocos add bloco_grupo tinyint
		end 


end

go


go 

IF not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sped_blocos_parametros]') AND type in (N'U'))
begin
create table sped_blocos_parametros
	(
		filial varchar(40),
		id_parametro int,
		id_bloco int,
		nome_parametro varchar(50),
		tipo_dados varchar(40),
		tamanho int,
		campo_pai varchar(40),
		herda_pai tinyint	
	)

end

go



-- exec sp_EFD_PisCofins_C405 '','','',0,0
-- select * from sped_blocos
-- select * from sped_blocos_parametros 
--


declare @id int;
declare @id_pai int; 
declare @ordem int ;
declare @filial varchar(40);
declare @tipoArquivo varchar(20);
set @filial ='MATRIZ';
set @tipoArquivo ='ICMSIPI'


delete SP from sped_blocos_parametros AS  sp INNER JOIN SPED_BLOCOS as sb ON  sb.id = sp.id_bloco
where sb.tipoArquivo =@tipoArquivo and sp.filial =@filial;
 
delete from sped_blocos where tipoArquivo=@tipoArquivo and filial = @filial;

-- Registro 0000====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0000','sp_EFD_PisCofins_0000',0,@ordem,1,'0990');

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data_Ini','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data_Fim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Tipo','Numero',1,'',0);			

-- Registro 0001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0001','sp_EFD_PisCofins_0001',@id_pai,@ordem,0,'0990');

-- Registro 0005====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0005','sp_EFD_PisCofins_0005',@id_pai,@ordem,0,'0990');



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);

-- Registro 0100====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0100','sp_EFD_PisCofins_0100',@id_pai,@ordem,0,'0990');


-- Registro 0150====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0150','sp_EFD_PisCofins_0150',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_ini','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_Fim','Data',8,'',0);


-- Registro 0190====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0190','sp_EFD_PisCofins_0190',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataFim','Data',8,'',0);

-- Registro 0200====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem =  isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0200','sp_EFD_PisCofins_0200',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataFim','Data',8,'',0);



-- Registro 0400====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem =  isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0400','sp_EFD_PisCofins_0400',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DATAINI','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DATAFIM','Data',8,'',0);




-- Registro B001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'B001','sp_EFD_PisCofins_B001',0,@ordem,1,'','B990');



-- Registro C001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C001','sp_EFD_PisCofins_C001',0,@ordem,1,'','C990');



-- Registro C100====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C001' and tipoArquivo=@tipoArquivo


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C100','sp_EFD_PisCofins_C100',@id_pai,0,0,'','C990');

				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
-- Registro C170====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C100' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C170','sp_EFD_PisCofins_C170',@id_pai,@ordem,0,NULL,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Fornecedor','Texto',20,'COD_PART',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'NroNota','Texto',10,'NUM_DOC',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Tipo','Numero',1,'',0);			


-- Registro C190====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C100' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C190','sp_EFD_ICMSIPI_C190',@id_pai,@ordem,0,NULL,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Fornecedor','Texto',20,'COD_PART',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'NroNota','Texto',10,'NUM_DOC',0);

				
				
-- Registro C400====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C400','sp_EFD_PisCofins_C400','',@ordem,1,'ECF_CX','C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
-- Registro C405====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C400' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C405','sp_EFD_PisCofins_C405',@id_pai,@ordem,1,'DT_DOC','C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'FabEqu','Texto',20,'ECF_FAB',0);

-- Registro C410====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C410','sp_EFD_PisCofins_C410',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOIni',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOFim',0);


-- Registro C420====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C420','sp_EFD_PisCofins_C420',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COORDZ','Numero',10,'COOFim',0);


-- Registro C460====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C460','sp_EFD_PisCofins_C460',@id_pai,@ordem,0,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOini',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOfim',0);


-- Registro C470====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C460' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C470','sp_EFD_PisCofins_C470',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',1);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Cupom','Numero',10,'NUM_DOC',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Tipo','Numero',1,'',0);
				
				

-- Registro C490====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C490','sp_EFD_ICMSIPI_C490',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'Data','Data',8,'DT_DOC',0);
				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOINI','Numero',10,'COOini',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'COOFIM','Numero',10,'COOfim',0);


-- Registro C800G====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800G','sp_EFD_ICMSIPI_C800_G','',@ordem,1,'NR_SAT','C990',1);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);


				
-- Registro C800G DT====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800G' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai and tipoArquivo=@tipoArquivo;
insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800GDT','sp_EFD_ICMSIPI_C800_GDT',@id_pai,@ordem,1,'DT_DOC','',1);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'NR_SAT','texto',50,'NR_SAT',0);



-- Registro C800====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800GDT' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai and tipoArquivo=@tipoArquivo;
insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza,bloco_grupo)
				values(@filial,@tipoArquivo,@id,'C800','sp_EFD_ICMSIPI_C800',@id_pai,@ordem,0,'','C990',0);



insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data','Data',8,'DT_DOC',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'NR_SAT','texto',50,'NR_SAT',0);


-- Registro C850====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C800' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C850','sp_EFD_ICMSIPI_C850',@id_pai,@ordem,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'CHV_CFE','TEXTO',44,'CHV_CFE',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data','Data',8,'DT_DOC',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'NR_SAT','texto',50,'NR_SAT',0);



					
-- Registro H001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'H001','sp_EFD_ICMSIPI_H001',0,@ordem,1,'','H990');

		
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);

-- Registro H005====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='H001' and tipoArquivo=@tipoArquivo


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'H005','sp_EFD_ICMSIPI_H005',@id_pai,0,0,'','H990');

				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);

					
-- Registro H010====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='H005' and tipoArquivo=@tipoArquivo


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'H010','sp_EFD_ICMSIPI_H010',@id_pai,0,0,'','H990');

				
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'Data','Data',8,'DT_INV',0);