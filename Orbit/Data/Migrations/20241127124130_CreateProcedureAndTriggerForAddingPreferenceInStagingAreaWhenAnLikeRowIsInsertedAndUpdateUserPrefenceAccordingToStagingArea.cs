using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateProcedureAndTriggerForAddingPreferenceInStagingAreaWhenAnLikeRowIsInsertedAndUpdateUserPrefenceAccordingToStagingArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET @executed = NULL;");
            migrationBuilder.Sql("""
            CREATE PROCEDURE `remove_outdated_preference_from_staging_area`(IN user_id INT)
            BEGIN
                DECLARE VAR_datetime DATETIME;

                DECLARE VAR_done BOOL DEFAULT FALSE;
                DECLARE CUR CURSOR FOR SELECT created_at FROM preference_staging_area 
                WHERE user_id = user_id;
                DECLARE CONTINUE HANDLER FOR NOT FOUND SET VAR_done = TRUE;

                OPEN CUR;
                FETCH CUR INTO VAR_datetime;
                IF VAR_done THEN
                    IF (DAY(NOW()) - DAY(VAR_datetime) > 3 ) THEN
                        DELETE FROM preference_staging_area WHERE user_id = user_id;
                    END IF;
                END IF;
                CLOSE CUR;
            END;
            """
            );
            migrationBuilder.Sql("""
            CREATE PROCEDURE `add_preference_to_staging_area`(IN user_id INT, IN post_id INT)
            BEGIN
                DECLARE VAR_preference_id INT DEFAULT 0;
                DECLARE VAR_preference_name VARCHAR(255);

                DECLARE VAR_done BOOL DEFAULT FALSE;
                DECLARE CUR CURSOR FOR SELECT preference_id, preference_name FROM post_preference WHERE PostId = post_id;
                DECLARE CONTINUE HANDLER FOR NOT FOUND SET VAR_done = TRUE;

                OPEN CUR;
                FETCH CUR INTO VAR_preference_id, VAR_preference_name;
                IF VAR_done THEN
                    INSERT INTO preference_staging_area
                    VALUES (user_id, VAR_preference_id, DEFAULT);
                END IF;
                CLOSE CUR;
            END;
            """
            );

            migrationBuilder.Sql("""
            CREATE PROCEDURE `update_user_preference_according_to_staging_area`(IN user_id INT)
            BEGIN
                DECLARE VAR_preference_name VARCHAR(255);
                DECLARE VAR_preference_id INT DEFAULT 0;
                DECLARE VAR_count INT DEFAULT 0;
                DECLARE VAR_done BOOL DEFAULT FALSE;
                DECLARE CUR CURSOR FOR SELECT preference_id FROM preference_staging_area WHERE user_id = user_id;
                DECLARE CONTINUE HANDLER FOR NOT FOUND SET VAR_done = TRUE;

                OPEN CUR;
                REPEAT
                    FETCH CUR INTO VAR_preference_id;
                    IF NOT VAR_done THEN
                        SELECT preference_name INTO VAR_preference_name FROM post_preference WHERE preference_id = VAR_preference_id;
                        SELECT COUNT(*) INTO VAR_count FROM preference_staging_area WHERE user_id = user_id AND preference_id = VAR_preference_id;
                        IF VAR_count > 3
                        THEN
                            DELETE FROM preference_staging_area WHERE user_id = user_id AND preference_id = VAR_preference_id;
                            INSERT INTO user_preference (user_id, preference_name) VALUES (user_id, VAR_preference_name);
                        END IF;
                    END IF;
                UNTIL VAR_done END REPEAT;
                CLOSE CUR;
            END;
            """);

            migrationBuilder.Sql("""
            CREATE TRIGGER `like_to_preference_staging_area` 
            AFTER INSERT ON `likes`
            FOR EACH ROW
            BEGIN
                CALL remove_outdated_preference_from_staging_area(NEW.user_id);
                IF @executed IS NULL THEN
                    SET @executed = 1;
                    CALL add_preference_to_staging_area(NEW.user_id, NEW.post_id);
                END IF;
                CALL update_user_preference_according_to_staging_area(NEW.user_id);
            END;
            """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER `like_to_preference_staging_area`");
            migrationBuilder.Sql("DROP PROCEDURE `update_user_preference_according_to_staging_area`");
            migrationBuilder.Sql("DROP PROCEDURE `add_preference_to_staging_area`");
            migrationBuilder.Sql("DROP PROCEDURE `remove_outdated_preference_from_staging_area`");
        }
    }
}
