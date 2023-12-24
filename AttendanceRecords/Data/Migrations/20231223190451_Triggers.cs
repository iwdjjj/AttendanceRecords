using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceRecords.Data.Migrations
{
    /// <inheritdoc />
    public partial class Triggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[AddDelCount]
            ON  [dbo].[Student]
            AFTER INSERT, DELETE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;


        	    DECLARE @GroupId int;
                DECLARE @Count int;       

                SELECT TOP 1 @GroupId=[GroupId] FROM INSERTED;
                if (@GroupId IS NULL) BEGIN
                    SELECT TOP 1 @GroupId=[GroupId] FROM DELETED;
                END
                
                SET @Count = (SELECT COUNT(*) FROM [dbo].[Student] WHERE [GroupId]=@GroupId)
                UPDATE [dbo].[Group] SET [Student_Count]=@Count WHERE [GroupId]=@GroupId
               
            END");


            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[UpdCount]
            ON  [dbo].[Student]
            AFTER UPDATE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;


        	    DECLARE @GroupId int;
                DECLARE @GroupId2 int;
                DECLARE @Count int;       

                SELECT TOP 1 @GroupId=[GroupId] FROM INSERTED;
                SELECT TOP 1 @GroupId2=[GroupId] FROM DELETED;
                
                SET @Count = (SELECT COUNT(*) FROM [dbo].[Student] WHERE [GroupId]=@GroupId)
                UPDATE [dbo].[Group] SET [Student_Count]=@Count WHERE [GroupId]=@GroupId

                SET @Count = (SELECT COUNT(*) FROM [dbo].[Student] WHERE [GroupId]=@GroupId2)
                UPDATE [dbo].[Group] SET [Student_Count]=@Count WHERE [GroupId]=@GroupId2 
               
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS [dbo].[AddDelCount]");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS [dbo].[UpdCount]");
        }
    }
}
