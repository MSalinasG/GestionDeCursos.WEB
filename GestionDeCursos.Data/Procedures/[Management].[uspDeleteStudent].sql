CREATE OR ALTER PROCEDURE [Management].[uspDeleteStudent]
(
	@pId INT
)
AS
BEGIN

	DELETE s
	FROM Management.Students s
	WHERE s.Id = @pId 

END