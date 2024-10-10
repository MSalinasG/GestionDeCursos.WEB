CREATE OR ALTER PROCEDURE [Management].[uspUpdateStudent]
(
	@pId INT,
	@pName NVARCHAR(100),
	@pCourseId INT,
	@pCourseFee INT,
	@pCourseDuration INT,
	@pCourseStartDate DATETIME,
	@pBatchTime DATETIME,
	@pInstructorId INT
)
AS
BEGIN

	UPDATE s
	SET s.StudentName = @pName, 
		s.CourseId = @pCourseId, 
		s.CourseFee = @pCourseFee,
		s.CourseDuration = @pCourseDuration, 
		s.CourseStartDate = @pCourseStartDate, 
		s.BatchTime = @pBatchTime, 
		s.InstructorId = @pInstructorId
	FROM Management.Students s
	WHERE s.Id = @pId

	 

END