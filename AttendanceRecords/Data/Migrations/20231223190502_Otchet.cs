using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceRecords.Data.Migrations
{
    /// <inheritdoc />
    public partial class Otchet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE Otchet
	        --@id int
            AS
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;

                -- Insert statements for procedure here
	            SELECT N.[StudentId] id,[RecordBook] nm, COUNT(P.SkipId) kol FROM [dbo].[Student] N
		            JOIN [dbo].[Skip] P ON N.StudentId=P.StudentId
			    GROUP BY N.[StudentId],[RecordBook]
            END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE Otchet");
        }
    }
}
