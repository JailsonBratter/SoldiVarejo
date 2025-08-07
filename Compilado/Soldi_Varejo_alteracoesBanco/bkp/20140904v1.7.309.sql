
ALTER PROCEDURE [dbo].[sp_RetiraOferta]
	
AS	
	Declare @Data1	As varchar(10)
	Declare @Data2	As varchar(10)

	Begin
		Select 
			@Data1 = 	 Convert(Varchar,Dateadd(day,-1,GetDate()),102) 
                     
		Update Mercadoria Set Estado_Mercadoria = 1, Promocao_Automatica = 0, Promocao = 0 Where 
				Convert(Varchar,Data_Fim,102) <= @Data1 and (promocao_automatica = 1 or promocao=1)

		Update Mercadoria_Loja Set  Promocao_Automatica = 0, Promocao = 0 Where 
				Convert(Varchar,Data_Fim,102) <= @Data1 and (promocao_automatica = 1 or promocao=1)


		--Select * from Mercadoria Where Estado_Mercadoria = 1

	End
