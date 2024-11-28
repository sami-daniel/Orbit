using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orbit.Data.Migrations;

public partial class UpdateUserPreferenceLogic : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Criar Trigger
        migrationBuilder.Sql("""
            CREATE TRIGGER `like_to_preference_staging_area` 
            AFTER INSERT ON `like`
            FOR EACH ROW
            BEGIN
                -- Atualiza a contagem de likes no post
                UPDATE post
                SET post_likes = post_likes + 1
                WHERE post_id = NEW.post_id;

                -- Remove preferências antigas da área de staging
                CALL remove_outdated_preference_from_staging_area(NEW.user_id);

                -- Insere preferências do post na staging area
                CALL add_preference_to_staging_area(NEW.user_id, NEW.post_id);

                -- Atualiza as preferências do usuário com base na staging area
                CALL update_user_preference_according_to_staging_area(NEW.user_id);
            END;
        """);

        // Criar Procedure para remover preferências antigas da staging area
        migrationBuilder.Sql("""
            CREATE PROCEDURE `remove_outdated_preference_from_staging_area`(IN user_id INT)
            BEGIN
                DELETE FROM preference_staging_area
                WHERE user_id = user_id AND DATEDIFF(NOW(), created_at) > 3;
            END;
        """);

        // Criar Procedure para adicionar preferências à staging area
        migrationBuilder.Sql("""
            CREATE PROCEDURE `add_preference_to_staging_area`(IN user_id INT, IN post_id INT)
            BEGIN
                DECLARE VAR_preference_id INT DEFAULT 0;
                DECLARE VAR_preference_name VARCHAR(255);
                DECLARE VAR_done BOOL DEFAULT FALSE;

                DECLARE CUR CURSOR FOR
                    SELECT preference_id, preference_name
                    FROM post_preference
                    WHERE PostId = post_id;

                DECLARE CONTINUE HANDLER FOR NOT FOUND SET VAR_done = TRUE;

                OPEN CUR;

                -- Insere preferências do post na staging area
                FETCH CUR INTO VAR_preference_id, VAR_preference_name;
                WHILE NOT VAR_done DO
                    INSERT INTO preference_staging_area (user_id, preference_id, created_at)
                    VALUES (user_id, VAR_preference_id, NOW());
                    FETCH CUR INTO VAR_preference_id, VAR_preference_name;
                END WHILE;

                CLOSE CUR;
            END;
        """);

        // Criar Procedure para atualizar preferências do usuário
        migrationBuilder.Sql("""
            CREATE PROCEDURE `update_user_preference_according_to_staging_area`(IN user_id INT)
            BEGIN
                DECLARE VAR_preference_name VARCHAR(255);
                DECLARE VAR_preference_id INT DEFAULT 0;
                DECLARE VAR_count INT DEFAULT 0;
                DECLARE VAR_done BOOL DEFAULT FALSE;
                DECLARE CUR CURSOR FOR
                    SELECT preference_id
                    FROM preference_staging_area
                    WHERE user_id = user_id;

                DECLARE CONTINUE HANDLER FOR NOT FOUND SET VAR_done = TRUE;

                OPEN CUR;

                -- Insere as preferências na tabela user_preference
                REPEAT
                    FETCH CUR INTO VAR_preference_id;
                    IF NOT VAR_done THEN
                        SELECT preference_name INTO VAR_preference_name
                        FROM post_preference
                        WHERE preference_id = VAR_preference_id;
                        
                        -- Conta quantas preferências do mesmo tipo existem
                        SELECT COUNT(*) INTO VAR_count
                        FROM preference_staging_area
                        WHERE user_id = user_id AND preference_id = VAR_preference_id;

                        IF VAR_count > 3 THEN
                            -- Insere na user_preference
                            INSERT INTO user_preference (user_id, preference_name)
                            VALUES (user_id, VAR_preference_name);

                            -- Remove da staging area
                            DELETE FROM preference_staging_area
                            WHERE user_id = user_id AND preference_id = VAR_preference_id;
                        END IF;
                    END IF;
                UNTIL VAR_done END REPEAT;

                CLOSE CUR;
            END;
        """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Remover a trigger e as procedures criadas
        migrationBuilder.Sql("DROP TRIGGER IF EXISTS `like_to_preference_staging_area`;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS `update_user_preference_according_to_staging_area`;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS `add_preference_to_staging_area`;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS `remove_outdated_preference_from_staging_area`;");
    }
}
