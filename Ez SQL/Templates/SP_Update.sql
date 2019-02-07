CREATE PROCEDURE @TableName@_Update @Params@ AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION @TableName@_Update WITH MARK N'Updating existing record in @TableName@';  
		BEGIN TRY
			UPDATE
				[@Schema@].[@TableName@]
			SET
				@Updates@
			WHERE
				@Filter@
			
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
		        ROLLBACK TRANSACTION @TableName@_Update;
		END CATCH
		
		IF @@TRANCOUNT > 0
		    COMMIT TRANSACTION @TableName@_Update;
END