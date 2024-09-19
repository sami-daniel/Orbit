-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
SHOW WARNINGS;
-- -----------------------------------------------------
-- Schema orbitdatabase
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `orbitdatabase` ;

-- -----------------------------------------------------
-- Schema orbitdatabase
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `orbitdatabase` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
SHOW WARNINGS;
USE `orbitdatabase` ;

-- -----------------------------------------------------
-- Table `orbitdatabase`.`user`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `orbitdatabase`.`user` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `orbitdatabase`.`user` (
  `user_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_name` VARCHAR(255) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
  `user_email` VARCHAR(255) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
  `user_password` VARCHAR(255) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
  `user_profile_name` VARCHAR(255) NOT NULL,
  `user_description` TEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  `user_profile_image_byte_type` LONGBLOB NULL DEFAULT NULL,
  `user_profile_banner_image_byte_type` LONGBLOB NULL DEFAULT NULL,
  `is_private_profile` BIT(1) NOT NULL,
  PRIMARY KEY (`user_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;
CREATE UNIQUE INDEX `user_name_UNIQUE` ON `orbitdatabase`.`user` (`user_name` ASC) VISIBLE;

SHOW WARNINGS;
CREATE UNIQUE INDEX `user_email_UNIQUE` ON `orbitdatabase`.`user` (`user_email` ASC) VISIBLE;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `orbitdatabase`.`follower`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `orbitdatabase`.`follower` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `orbitdatabase`.`follower` (
  `user_id` INT UNSIGNED NOT NULL,
  `follower_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`user_id`, `follower_id`),
  CONSTRAINT `follower_ibfk_1`
    FOREIGN KEY (`follower_id`)
    REFERENCES `orbitdatabase`.`user` (`user_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `follower_ibfk_2`
    FOREIGN KEY (`user_id`)
    REFERENCES `orbitdatabase`.`user` (`user_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;
CREATE INDEX `follower_id` ON `orbitdatabase`.`follower` (`follower_id` ASC) VISIBLE;

SHOW WARNINGS;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
