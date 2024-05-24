
param(
    [string] $namespace,
    [string] $outputDir,
    [string] $location,
    [bool] $force,
    [string] $connectionString = $env:ConnectionStrings__DefaultConnection,
    [string] $provider = "Pomelo.EntityFrameworkCore.MySql"
)


if(-not $namespace) {
    Write-Host "Por favor, forneça um namespace usando o parâmetro -namespace." -ForegroundColor Red
    Exit
}

# Verifica se o diretório de saída foi fornecido
if(-not $outputDir) {
    Write-Host "Por favor, forneça um diretório de saída usando o parâmetro -outputDir." -ForegroundColor Red
    Exit
}

# Verifica se o local foi fornecido e se é um diretório válido
if(-not $location -or -not (Test-Path -Path $location -PathType Container)) {
    Write-Host "Por favor, forneça um local de projeto válido usando o parâmetro -location." -ForegroundColor Red
    Exit
}

Set-Location -Path $location

# Executa o scaffold
if($force) {
    dotnet ef dbcontext scaffold "$connectionString" $provider --namespace $namespace --output-dir $outputDir --force
} else {
    dotnet ef dbcontext scaffold "$connectionString" $provider --namespace $namespace --output-dir $outputDir
}

$contextFile = Get-ChildItem -Path $outputDir -Filter "*Context.cs"

# Remove o arquivo de contexto do banco de dados gerado pelo scaffold
if($contextFile) {
    Remove-Item $contextFile.FullName
    Write-Host "Context file removed: $contextFile"
}

Write-Host "Looks fine!"

<#
.SYNOPSIS
   Script para gerar um contexto de banco de dados usando Entity Framework Core.
.DESCRIPTION
   Este script gera um contexto de banco de dados usando Entity Framework Core para um banco de dados MySQL.
.PARAMETER namespace
   O namespace para o contexto do banco de dados e as entidades.
.PARAMETER outputDir
   O diretório de saída onde os arquivos do contexto e das entidades serão gerados.
.PARAMETER location
   O local do projeto onde o comando será executado.
.PARAMETER force
   Indica se o script deve substituir arquivos existentes ao gerar o contexto e as entidades.
.PARAMETER connectionString
   A string de conexão com o banco de dados. Se não fornecida, será usada a variável de ambiente ConnectionStrings__DefaultConnection.
.PARAMETER provider
   O provedor de banco de dados a ser usado. Por padrão, é usado o provedor para MySQL (Pomelo.EntityFrameworkCore.MySql).
.EXAMPLE
   .\Generate-DbContext.ps1 -namespace MyNamespace -outputDir C:\Projeto\Context -location C:\Projeto -force $true
   Gera um contexto de banco de dados no diretório C:\Projeto\Context, substituindo arquivos existentes, usando o namespace MyNamespace.
.NOTES
   Autor: [Seu Nome]
   Versão: 1.0
   Data: [Data de criação do script]
#>

param(
    [string] $namespace,
    [string] $outputDir,
    [string] $location,
    [bool] $force,
    [string] $connectionString = $env:ConnectionStrings__DefaultConnection,
    [string] $provider = "Pomelo.EntityFrameworkCore.MySql"
)

# Verifica se o namespace foi fornecido
if (-not $namespace) {
    Write-Host "Please provide a namespace using the -namespace parameter." -ForegroundColor Red
    Exit
}

# Verifica se o diretorio foi fornecido
if (-not $outputDir) {
    Write-Host "Please provide an output directory using the -outputDir parameter." -ForegroundColor Red
    Exit
}

# Verifica se o a localizacao é valida e foi fornecida
if (-not $location -or -not (Test-Path -Path $location -PathType Container)) {
    Write-Host "Please provide a valid project location using the -location parameter." -ForegroundColor Red
    Exit
}

Set-Location -Path $location

# Executa o scaffold
if ($force) {
    dotnet ef dbcontext scaffold "$connectionString" $provider --namespace $namespace --output-dir $outputDir --force
} else {
    dotnet ef dbcontext scaffold "$connectionString" $provider --namespace $namespace --output-dir $outputDir
}

$contextFile = Get-ChildItem -Path $outputDir -Filter "*Context.cs"

# Remove o contexto gerado pelo scaffold
if ($contextFile) {
    Remove-Item $contextFile.FullName
    Write-Host "Context file removed: $($contextFile.FullName)"
}

Write-Host "Looks fine!"
