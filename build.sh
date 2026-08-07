#!/bin/bash
# Build script para OpenTURZXStudio v1.0.0 (Linux/macOS)
# Este script compila o projeto e gera um executavel

echo "====================================="
echo "OpenTURZXStudio v1.0.0 - Build Script"
echo "====================================="
echo ""

# Verificar se .NET CLI esta disponivel
if ! command -v dotnet &> /dev/null; then
    echo "ERRO: .NET 6.0 nao esta instalado!"
    echo "Baixe em: https://dotnet.microsoft.com/download"
    exit 1
fi

echo "[1/4] Limpando arquivos anteriores..."
cd src/OpenTURZXStudio.Desktop
dotnet clean -c Release > /dev/null 2>&1

echo "[2/4] Restaurando dependencias..."
dotnet restore

echo "[3/4] Compilando em Release mode..."
dotnet build -c Release

echo "[4/4] Publicando como executavel..."
dotnet publish -c Release -o ../../dist/OpenTURZXStudio-v1.0.0 -p:PublishSingleFile=true -p:SelfContained=true -p:RuntimeIdentifier=osx-x64

echo ""
echo "====================================="
echo "BUILD CONCLUIDO COM SUCESSO!"
echo "====================================="
echo ""
echo "Executavel criado em:"
echo "  dist/OpenTURZXStudio-v1.0.0/OpenTURZXStudio.Desktop"
echo ""
