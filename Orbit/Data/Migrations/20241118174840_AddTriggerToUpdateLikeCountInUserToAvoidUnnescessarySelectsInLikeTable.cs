using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerToUpdateLikeCountInUserToAvoidUnnescessarySelectsInLikeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            CREATE TRIGGER `post_like_count_update` AFTER INSERT ON `likes`
            FOR EACH ROW
            BEGIN
                UPDATE post
                SET post_likes = post_likes + 1
                WHERE user_id = NEW.user_id;
            END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER `post_like_count_update`");   
        }
    }
}
