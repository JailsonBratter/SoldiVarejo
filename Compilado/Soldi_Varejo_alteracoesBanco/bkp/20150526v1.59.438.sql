
alter table nf add Dest_Fornec tinyint 
go

alter table nf add Ref_ECF tinyint
go

alter table nf_devolucao add nECF Varchar(3) , nCOO varchar(6)

go


UPDATE nf set dest_fornec = case when tipo_nf=1 then 0 else 1 end  where dest_fornec is null
go

