CREATE OR ALTER PROCEDURE [Management].[uspGetStudents]
(
	@pId INT = NULL
)
AS
BEGIN

	SELECT 
		s.Id,
		s.StudentName,
		s.CourseFee,
		s.CourseDuration,
		s.CourseStartDate,
		s.BatchTime,
		i.id as InstructorId, 
		i.InstructorName,		 
		c.Id as CourseId,
		c.CourseName

	FROM 
	Management.Students s
	JOIN Management.Instructors i on i.Id = s.InstructorId
	JOIN Management.Courses c on c.id = s.CourseId
	WHERE @pId IS NULL OR s.id = @pId
END

EXEC [Management].[uspGetStudents] 1