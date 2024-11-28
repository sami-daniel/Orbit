CREATE DATABASE orbitdatabase;
use orbitdatabase;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4 ENGINE = InnoDB;

START TRANSACTION;

ALTER DATABASE orbitdatabase CHARACTER SET utf8mb4;

CREATE TABLE `user` (
    `user_id` int unsigned NOT NULL AUTO_INCREMENT,
    `user_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `user_email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `user_password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `user_profile_name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
    `user_description` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
    `user_profile_image_byte_type` LONGBLOB NULL,
    `user_profile_banner_image_byte_type` LONGBLOB NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`user_id`)
) CHARACTER SET=utf8mb3 COLLATE=utf8mb3_general_ci ENGINE = InnoDB;

CREATE TABLE `follower` (
    `user_id` int unsigned NOT NULL,
    `follower_id` int unsigned NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`user_id`, `follower_id`),
    CONSTRAINT `follower_ibfk_1` FOREIGN KEY (`follower_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE,
    CONSTRAINT `follower_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb3 COLLATE=utf8mb3_general_ci ENGINE = InnoDB;

CREATE TABLE `post` (
    `post_id` int unsigned NOT NULL AUTO_INCREMENT,
    `post_content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `post_date` datetime NOT NULL DEFAULT (NOW()),
    `post_image_byte_type` LONGBLOB NULL,
    `post_video_byte_type` LONGBLOB NULL,
    `post_likes` int unsigned NOT NULL,
    `user_id` int unsigned NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`post_id`),
    CONSTRAINT `post_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ENGINE = InnoDB;

CREATE TABLE `user_preference` (
    `preference_id` int unsigned NOT NULL AUTO_INCREMENT,
    `preference_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `UserId` int unsigned NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`preference_id`),
    CONSTRAINT `user_preference_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`user_id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ENGINE = InnoDB;

CREATE TABLE `like` (
    `like_id` int unsigned NOT NULL AUTO_INCREMENT,
    `user_id` int unsigned NULL,
    `post_id` int unsigned NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`like_id`),
    CONSTRAINT `like_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE SET NULL,
    CONSTRAINT `like_ibfk_2` FOREIGN KEY (`post_id`) REFERENCES `post` (`post_id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ENGINE = InnoDB;

CREATE TABLE `post_preference` (
    `preference_id` int unsigned NOT NULL AUTO_INCREMENT,
    `preference_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `PostId` int unsigned NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`preference_id`),
    CONSTRAINT `post_preference_ibfk_1` FOREIGN KEY (`PostId`) REFERENCES `post` (`post_id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ENGINE = InnoDB;

CREATE INDEX `follower_id` ON `follower` (`follower_id`);

CREATE INDEX `like_ibfk_1` ON `like` (`user_id`);

CREATE INDEX `like_ibfk_2` ON `like` (`post_id`);

CREATE INDEX `post_ibfk_1` ON `post` (`user_id`);

CREATE INDEX `post_preference_ibfk_1` ON `post_preference` (`PostId`);

CREATE UNIQUE INDEX `user_email_UNIQUE` ON `user` (`user_email`);

CREATE UNIQUE INDEX `user_name_UNIQUE` ON `user` (`user_name`);

CREATE INDEX `user_preference_ibfk_1` ON `user_preference` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241128022707_InitialCreate', '8.0.6');

COMMIT;

