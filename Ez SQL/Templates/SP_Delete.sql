CREATE PROCEDURE @TableName@_Delete @Params@ AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION @TableName@_Delete WITH MARK N'Deleting record from @TableName@';  
		BEGIN TRY
			
			DELETE 
				[@Schema@].[@TableName@]
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
		        ROLLBACK TRANSACTION @TableName@_Delete;
		END CATCH
		
		IF @@TRANCOUNT > 0
		    COMMIT TRANSACTION @TableName@_Delete;
END