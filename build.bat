@echo off
REM Build script para OpenTURZXStudio v1.0.0
REM Este script compila o projeto e gera um executavel standalone

echo =====================================
echo OpenTURZXStudio v1.0.0 - Build Script
echo =====================================
echo.

REM Verificar se .NET CLI esta disponivel
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: .NET 6.0 nao esta instalado!
    echo Baixe em: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [1/4] Limpando arquivos anteriores...
cd src\OpenTURZXStudio.Desktop
dotnet clean -c Release >nul 2>&1

echo [2/4] Restaurando dependencias...
dotnet restore

echo [3/4] Compilando em Release mode...
dotnet build -c Release

echo [4/4] Publicando como executavel standalone...
dotnet publish -c Release -o ..\..\dist\OpenTURZXStudio-v1.0.0 -p:PublishSingleFile=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64

echo.
echo =====================================
echo BUILD CONCLUIDO COM SUCESSO!
echo =====================================
echo.
echo Executavel criado em:
echo   dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe
echo.
echo Para distribuir, comprima a pasta:
echo   dist\OpenTURZXStudio-v1.0.0
echo.
pause
