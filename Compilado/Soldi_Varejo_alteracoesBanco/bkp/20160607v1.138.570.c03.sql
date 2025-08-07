IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[utgr_atualiza_saldo_cr_cc]'))
DROP TRIGGER [dbo].[utgr_atualiza_saldo_cr_cc]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[dtgr_atualiza_saldo_cr_cc]'))
DROP TRIGGER [dbo].[dtgr_atualiza_saldo_cr_cc]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[itgr_atualiza_saldo_cr_cc]'))
DROP TRIGGER [dbo].[itgr_atualiza_saldo_cr_cc]
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[dtgr_atualiza_saldo_cp_cc]'))
DROP TRIGGER [dbo].[dtgr_atualiza_saldo_cp_cc]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[itgr_atualiza_saldo_cp_cc]'))
DROP TRIGGER [dbo].[itgr_atualiza_saldo_cp_cc]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[utgr_atualiza_saldo_cp_cc]'))
DROP TRIGGER [dbo].[utgr_atualiza_saldo_cp_cc]
GO
