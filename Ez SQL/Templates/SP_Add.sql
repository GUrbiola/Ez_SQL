CREATE PROCEDURE @TableName@_Insert @Params@ AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION @TableName@_Insert WITH MARK N'Inserting new record into @TableName@';  
		BEGIN TRY
			
			INSERT INTO [@Schema@].[@TableName@]@Fields@
			VALUES @Values@
			
			SELECT
				@Id@ AS Result,
				'Success' AS Message
		
		END TRY
		BEGIN CATCH
			--return the data of the error
			SELECT 
				-1 AS Result,
				ERROR_MESSAGE() AS Message
		        
			IF @@TRANCOUNT > 0
		        ROLLBACK TRANSACTION @TableName@_Insert;
		END CATCH
		
		IF @@TRANCOUNT > 0
		    COMMIT TRANSACTION @TableName@_Insert;
END