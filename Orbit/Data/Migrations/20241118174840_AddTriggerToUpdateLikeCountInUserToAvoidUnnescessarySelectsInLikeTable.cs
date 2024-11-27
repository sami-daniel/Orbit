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
            migrationBuilder.Sql(@"CREATE TRIGGER `post_like_count_update` AFTER INSERT ON `likes`
                                   FOR EACH ROW
                                   BEGIN
                                        UPDATE users
                                        SET user_like_count = user_like_count + 1
                                        WHERE user_id = NEW.user_id;
                                   END;
                                   ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
