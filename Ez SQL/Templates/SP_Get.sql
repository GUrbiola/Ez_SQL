CREATE PROCEDURE @TableName@_Get @Params@ AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION @TableName@_Get WITH MARK N'Extracting records from @TableName@';  
		BEGIN TRY
			SELECT
				@Fields@
			FROM
				[@Schema@].[@TableName@]
			WHERE
				@Filter@
			
		END TRY
		BEGIN CATCH
		        
			IF @@TRANCOUNT > 0
		        ROLLBACK TRANSACTION @TableName@_Update;
		END CATCH
		
		IF @@TRANCOUNT > 0
		    COMMIT TRANSACTION @TableName@_Update;
END