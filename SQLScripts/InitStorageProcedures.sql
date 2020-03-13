GO
IF OBJECT_ID('get_people', 'P') IS NOT NULL
    DROP PROCEDURE get_people
GO
CREATE PROCEDURE get_people
(
	@status int,
	@dep int,
	@post int,
	@last_name varchar(100)
)
AS
BEGIN
	SELECT * 
	FROM [dbo].[persons]
	WHERE	((@status IS NOT NULL AND [id_status] = @status) OR (@status is null)) AND 
			((@dep IS NOT NULL AND [id_dep] = @dep) OR (@dep IS NULL)) AND
			((@post IS NOT NULL AND [id_post] = @post) OR (@post IS NULL)) AND 
			((@last_name IS NOT NULL AND [last_name] LIKE '%' + @last_name + '%') OR (@last_name IS NULL))
END
GO
IF OBJECT_ID('get_statistic', 'P') IS NOT NULL
    DROP PROCEDURE get_statistic
GO
CREATE PROCEDURE get_statistic
(
	@status INT,
	@is_date_employ BIT,
	@start_date DATETIME,
	@end_date DATETIME
)
AS
BEGIN
	IF @is_date_employ = 1
	BEGIN
		SELECT CAST ([date_employ] AS DATE), count(*)
		FROM [dbo].[persons]
		WHERE (@status IS NOT NULL AND [id_status] = @status) AND ([date_employ] BETWEEN @start_date AND @end_date)
		GROUP BY CAST ([date_employ] AS DATE)
	END
	ELSE
	BEGIN
		SELECT CAST ([date_unemploy] AS DATE), count(*)
		FROM [dbo].[persons]
		WHERE (@status IS NOT NULL AND [id_status] = @status) AND ([date_unemploy] BETWEEN @start_date AND @end_date)
		GROUP BY CAST ([date_unemploy] AS DATE)
	END
END
GO
IF OBJECT_ID('get_status', 'P') IS NOT NULL
    DROP PROCEDURE get_status
GO
CREATE PROCEDURE get_status
AS
BEGIN
	SELECT *
	FROM [dbo].[status]
END
GO
IF OBJECT_ID('get_deps', 'P') IS NOT NULL
    DROP PROCEDURE get_deps
GO
CREATE PROCEDURE get_deps
AS
BEGIN
	SELECT *
	FROM [dbo].[deps]
END
GO
IF OBJECT_ID('get_posts', 'P') IS NOT NULL
    DROP PROCEDURE get_posts
GO
CREATE PROCEDURE get_posts
AS
BEGIN
	SELECT *
	FROM [dbo].[posts]
END
Go