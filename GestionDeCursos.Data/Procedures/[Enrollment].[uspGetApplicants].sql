CREATE OR ALTER PROCEDURE [Enrollment].[uspGetApplicants]
(
	@pId UNIQUEIDENTIFIER = NULL
)
AS
BEGIN 
 

	SELECT [Id]
		  ,[Nombre]
		  ,[Apellido]
		  ,[Dni]
		  ,[Nacimiento]
		  ,[FichaPdfMongoFileId]
	  FROM [Enrollment].[Applicants]
	  WHERE @pId IS NULL OR Id = @pId 

END