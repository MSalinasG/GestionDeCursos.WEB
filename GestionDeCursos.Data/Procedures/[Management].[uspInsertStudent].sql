CREATE OR ALTER PROCEDURE [Management].[uspInsertStudent]
(
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

	INSERT INTO Management.Students(StudentName, CourseId, CourseFee,
	CourseDuration, CourseStartDate, BatchTime, InstructorId)
	VALUES (@pName,
	@pCourseId,
	@pCourseFee,
	@pCourseDuration,
	@pCourseStartDate,
	@pBatchTime,
	@pInstructorId)

END