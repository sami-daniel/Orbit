# Orbit - Rede Social para Profissionais de Tecnologia

**Versão Base (Bedrock)**

Bem-vindo ao Orbit, uma rede social inovadora projetada para conectar profissionais de tecnologia, incluindo programadores, designers, engenheiros de software, especialistas em TI e outros profissionais do setor. Nosso objetivo é proporcionar um espaço dinâmico e colaborativo onde esses profissionais possam interagir, compartilhar conhecimentos, trabalhar em projetos conjuntos e explorar novas oportunidades de carreira.

Esta é a versão base do Orbit, completamente configurada e funcional. Esta versão inclui todas as configurações iniciais necessárias para rodar o projeto.

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina:

- [ASP.NET Core SDK](https://dotnet.microsoft.com/download) (versão 8.0 ou superior)
- [Git](https://git-scm.com/downloads)

## Instruções para Rodar e Mexer no Projeto

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
``` sh
dotnet build
```

### Passo 5: Rodar o Projeto
``` sh
cd Orbit
dotnet run
```
