# Orbit - Rede Social para Profissionais de Tecnologia
![version](https://img.shields.io/badge/version-1.2.0-black.svg) <br>
![Orbit](https://img.shields.io/badge/Orbit-black.svg)

**Orbit 1.2.0**

Bem-vindo ao Orbit, uma rede social inovadora projetada para conectar profissionais de tecnologia, incluindo programadores, designers, engenheiros de software, especialistas em TI e outros profissionais do setor. Nosso objetivo é proporcionar um espaço dinâmico e colaborativo onde esses profissionais possam interagir, compartilhar conhecimentos, trabalhar em projetos conjuntos e explorar novas oportunidades de carreira.

Esta é a versão 1.2.0 do projeto. Atualmente contamos com um sistema de login e cadastro, redirecionamento para a página de perfil e pesquisa de usuarios com base no nome

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina:

- [ASP.NET Core SDK](https://dotnet.microsoft.com/download) (versão 8.0 ou superior)
- [Git](https://git-scm.com/downloads)
- [MySql](https://dev.mysql.com/downloads/workbench/)

## Instruções para Rodar o Projeto 

### Passo 1: Clonar o Repositório

Abra o terminal e execute o seguinte comando para clonar o repositório do projeto:

``` sh
git clone -b bedrock https://github.com/sami-daniel/orbit.git
cd Orbit
```

### Passo 2: Abrir o Projeto

No terminal abra o projeto em um editor:
# Visual Studio Code
``` sh
code .
```
OU

# Visual Studio
Dois cliques no arquivo .sln

### Passo 3: Restaurar Dependências 
No terminal restaure as dependências do projeto:
``` sh
dotnet restore
```

### Passo 4: Compilar o Projeto
No terminal compile o projeto:
``` sh
dotnet build
```

### Passo 5: Script do banco de dados
No cmd do MySql ou no MySql Workbench rode o script abaixo:
``` MySqlCmd
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

CREATE SCHEMA IF NOT EXISTS `orbitdatabase` DEFAULT CHARACTER SET utf8 ;

CREATE TABLE IF NOT EXISTS `orbitdatabase`.`user` (
  `user_id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_name` VARCHAR(100) NOT NULL,
  `user_email` VARCHAR(200) NOT NULL,
  `user_date_of_birth` DATE NOT NULL,
  `user_password` VARCHAR(200) NOT NULL,
  `user_description` MEDIUMTEXT NULL DEFAULT NULL,
  `user_image_byte_type` LONGBLOB NULL DEFAULT NULL,
  `user_profile_name` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE INDEX `user_name_UNIQUE` (`user_name` ASC) VISIBLE,
  UNIQUE INDEX `user_email_UNIQUE` (`user_email` ASC) VISIBLE,
  UNIQUE INDEX `user_id_UNIQUE` (`user_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `orbitdatabase`.`follower` (
  `follower_id` INT(10) UNSIGNED NOT NULL,
  `user_id` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`follower_id`, `user_id`),
  INDEX `fk_follower_user1_idx` (`user_id` ASC) VISIBLE,
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
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
```

### Passo 6: Salvar string de conexão
Salvar a 'Environment Variable' com a string de conexão do banco de dados. 
Talvez seja nescessário reiniciar o computador para a variável entrar em vigor: 
``` PowerSheel
setx ConnectionStrings__OrbitConnection "server=localhost;database=orbitdatabase;uid={seuusuariodomysql};pwd={suasenhadomysql}"
```

### Passo 7: Rodar o Projeto
``` sh
cd Orbit
dotnet run
```
### Passo 8: Abrir o projeto
No CMD saídas semelhantes a essa abaixo aparecerão. Copie o link web, semelhante a
esse e cole no navegador:
http://localhost:5000/ 
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
