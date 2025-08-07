



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
			blocoTotaliza varchar(40)
			
	)
end

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
set @tipoArquivo ='PISCOFINS'


delete SP from sped_blocos_parametros AS  sp INNER JOIN SPED_BLOCOS as sb ON  sb.id = sp.id_bloco
where sb.tipoArquivo =@tipoArquivo;
 
delete from sped_blocos where tipoArquivo=@tipoArquivo;

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


-- Registro 0110====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0110','sp_EFD_PisCofins_0110',@id_pai,@ordem,0,'0990');

				

				
				
-- Registro 0140====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0140','sp_EFD_PisCofins_0140',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_ini','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'data_Fim','Data',8,'',0);
					
				
				
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


-- Registro 0500====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='0000' AND tipoArquivo=@tipoArquivo;
Select @ordem =  isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'0500','sp_EFD_PisCofins_0500',@id_pai,@ordem,0,'0990');




insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);


-- Registro A001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'A001','sp_EFD_PisCofins_A001',0,@ordem,1,'','A990');



-- Registro C001====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = 0 and tipoArquivo=@tipoArquivo;


insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C001','sp_EFD_PisCofins_C001',0,@ordem,1,'','C990');
	
-- Registro C010====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C001' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C010','sp_EFD_PisCofins_C010',@id_pai,@ordem,0,NULL,'C990');


insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'FILIAL','Texto',40,'',0);
					
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
					@id,'DataInicio','Data',8,'',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros)
				,@id,'DataFim','Data',8,'',0);
				

-- Registro C100====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C001' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C100','sp_EFD_PisCofins_C100',@id_pai,@ordem,0,'','C990');

				
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


-- Registro C481====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C481','sp_EFD_PisCofins_C481',@id_pai,@ordem,0,NULL,'C990');


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
				@id,'DataMovimento','data',8,'DT_DOC',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'CupomInicial','Numero',10,'COOIni',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'CupomFinal','Numero',10,'COOFim',0);
				

-- Registro C485====================================================================================================
Select @id=isnull(max(id),0)+1 from sped_blocos;
select @id_pai = id from sped_blocos where bloco='C405' and tipoArquivo=@tipoArquivo
Select @ordem = isnull(max(ordem),0)+1 from sped_blocos where id_bloco_pai = @id_pai;

insert into sped_blocos (FILIAL,tipoArquivo,id,bloco,str_procedure,id_bloco_pai,ordem,gerarArquivo,campoArquivo,blocoTotaliza)
				values(@filial,@tipoArquivo,@id,'C485','sp_EFD_PisCofins_C485',@id_pai,@ordem,0,NULL,'C990');


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
				@id,'DataMovimento','data',8,'DT_DOC',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'Caixa','Numero',10,'Caixa',0);

insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'CupomInicial','Numero',10,'COOIni',0);
insert into sped_blocos_parametros (FILIAL,id_parametro,Id_bloco,nome_parametro,tipo_dados,tamanho,campo_pai,herda_pai)
				values(@filial,(Select isnull(max(id_parametro),0)+1 from sped_blocos_parametros),
				@id,'CupomFinal','Numero',10,'COOFim',0);

/*
select * from sped_blocos;
Select * from sped_blocos_parametros where campo_pai ='DT_DOC'; 


Select * from sped_blocos where id_bloco_pai = 0 order by ordem
*/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'CRIA BLOC PISCOFINS', getdate();
GO