using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceRecords.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[Changes]
            ON  [dbo].[Schedule]
           AFTER INSERT, UPDATE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;

                DECLARE @ScheduleId int, @SubjectId int, @new nvarchar(max); 

                SELECT TOP 1 @ScheduleId=[ScheduleId], @SubjectId=[SubjectId] FROM INSERTED;

                SET @new = (SELECT [Name] FROM [dbo].[Subject] WHERE [SubjectId]=@SubjectId)
                UPDATE [dbo].[Schedule] SET [SubjectName] = @new WHERE [ScheduleId]=@ScheduleId
               
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "Schedule");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS [dbo].[Changes]");
        }
    }
}
