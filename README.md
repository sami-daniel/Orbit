# Orbit - Social Network for Technology Professionals
![version](https://img.shields.io/badge/version-1_0_0-black.svg) <br>
![Orbit](https://img.shields.io/badge/Orbit-black.svg)

**Version: Orbit 1.0.0**

Welcome to Orbit, an innovative social network designed to connect technology professionals, including programmers, designers, software engineers, IT specialists, and other industry professionals. Our goal is to provide a dynamic and collaborative space where these professionals can interact, share knowledge, work on joint projects, and explore new career opportunities.

## Prerequisites

Before you begin, you will need to have installed on your machine:

- [ASP.NET Core SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- [Git](https://git-scm.com/downloads)
- [MySql](https://dev.mysql.com/downloads/mysql/)

## Instructions to Run the Project

### Step 1: Clone the Repository

Open the terminal and run the following command to clone the project repository:

``` sh
git clone https://github.com/sami-daniel/Orbit.git
cd Orbit
```

### Step 3: Restore Dependencies
In the terminal, restore the project dependencies:
``` sh
dotnet restore
```

### Step 4: Build the Project
In the terminal, build the project:
``` sh
dotnet build
```

### Step 5: Save Connection String
Save the 'Environment Variable' with the database connection string.
You may need to restart your computer for the variable to take effect.
``` PowerSheel
setx ConnectionStrings__OrbitConnection "server=localhost;database=orbitdatabase;uid={seuusuariodomysql};pwd={suasenhadomysql}" # Essa string pode ser sua string de conexão, desde que habilite conectar com o servidor e ter acesso ao banco
setx ConnectionStrings__Firebase "https://orbit-f5fea-default-rtdb.firebaseio.com/"
```

### Step 6: Execute the database code
Execute the following code in the MySql command line or in Workbench
``` MySqlCmd
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
```

### Step 7: Run the Project
``` sh
cd Orbit
dotnet run
```

### Step 8: Open the project
In CMD, similar outputs to the one below will appear. Copy the web link, similar to this, and paste it in the browser:
``` sh
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[0]
      User profile is available. Using 'C:\Users\YourUser\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 GET http://localhost:5000/
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'YourProject.Controllers.HomeController.Index (YourProject)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[3]
      Route matched with {action = "Index", controller = "Home"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Index() on controller YourProject.Controllers.HomeController (YourProject).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[4]
      Executed action YourProject.Controllers.HomeController.Index (YourProject) in 0.5894ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'YourProject.Controllers.HomeController.Index (YourProject)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished in 1.9748ms 200 text/html; charset=utf-8
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Path\To\Your\Project
```
