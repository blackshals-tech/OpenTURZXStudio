# OpenTURZXStudio v1.0.0 - Build & Distribution Guide

## 🏗️ Como Compilar o Executável

### Windows

**Opção 1: Script Automático (Recomendado)**
```bash
build.bat
```

**Opção 2: Manual via Terminal**
```bash
cd src/OpenTURZXStudio.Desktop
dotnet publish -c Release -o ../../dist/OpenTURZXStudio-v1.0.0 `
    -p:PublishSingleFile=true `
    -p:SelfContained=true `
    -p:RuntimeIdentifier=win-x64
```

**Opção 3: Visual Studio**
```
Build > Publish OpenTURZXStudio.Desktop
Select "Folder" profile
Configure to publish as single file
Publish
```

### Linux/macOS

**Via Script:**
```bash
chmod +x build.sh
./build.sh
```

**Manual:**
```bash
cd src/OpenTURZXStudio.Desktop
dotnet publish -c Release -o ../../dist/OpenTURZXStudio-v1.0.0 \
    -p:PublishSingleFile=true \
    -p:SelfContained=true \
    -p:RuntimeIdentifier=osx-x64
```

---

## 📦 Resultado da Compilação

### Estrutura do Executável

```
dist/OpenTURZXStudio-v1.0.0/
├── OpenTURZXStudio.Desktop.exe    (Principal - Execute este!)
├── OpenTURZXStudio.Core.dll
├── MaterialDesignThemes.Wpf.dll
├── CommunityToolkit.Mvvm.dll
└── [outras DLLs do .NET]
```

### Tamanho
- **Aplicação**: ~50-80 MB (self-contained com .NET runtime)
- **Com compactação (ZIP)**: ~20-30 MB

---

## 🚀 Executar o .EXE

### Opção 1: Duplo clique
```
Abra: dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe
```

### Opção 2: Linha de comando
```bash
.\dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe
```

### Opção 3: PowerShell
```powershell
& "dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe"
```

---

## 📤 Distribuir o Aplicativo

### Método 1: Compactar para Distribuição

```bash
# Windows (PowerShell)
Compress-Archive -Path dist\OpenTURZXStudio-v1.0.0 `
                 -DestinationPath OpenTURZXStudio-v1.0.0-win-x64.zip

# Linux/macOS
cd dist
zip -r ../OpenTURZXStudio-v1.0.0-portable.zip OpenTURZXStudio-v1.0.0/
cd ..
```

### Método 2: Criar Arquivo Portable

```bash
# Copie a pasta dist/OpenTURZXStudio-v1.0.0
# Comprima como ZIP
# Renomeie para: OpenTURZXStudio-v1.0.0-portable.zip
# Distribua via GitHub Releases, Google Drive, etc.
```

### Método 3: GitHub Releases

1. Vá para: https://github.com/blackshals-tech/OpenTURZXStudio/releases
2. Draft a new release
3. Tag version: `v1.0.0`
4. Faça upload do ZIP
5. Publique!

---

## ✅ Verificação de Integridade

Antes de distribuir, teste o executável:

```bash
# Verificar se o arquivo existe
Test-Path "dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe"

# Executar e verificar se a aplicação inicia
.\dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe

# Verificar informações do arquivo
Get-Item "dist\OpenTURZXStudio-v1.0.0\OpenTURZXStudio.Desktop.exe" | Select-Object Name, Length, LastWriteTime
```

---

## 🔧 Opções de Build Avançadas

### Sem Runtime Incorporado (Menor, requer .NET instalado)
```bash
dotnet publish -c Release -o dist/OpenTURZXStudio-v1.0.0 `
    -p:PublishSingleFile=true `
    -p:SelfContained=false `
    -p:RuntimeIdentifier=win-x64
```

### Com Trimming (Remover código não utilizado)
```bash
dotnet publish -c Release -o dist/OpenTURZXStudio-v1.0.0 `
    -p:PublishSingleFile=true `
    -p:SelfContained=true `
    -p:PublishTrimmed=true `
    -p:RuntimeIdentifier=win-x64
```

### Com ReadyToRun (Pré-compilado, mais rápido)
```bash
dotnet publish -c Release -o dist/OpenTURZXStudio-v1.0.0 `
    -p:PublishSingleFile=true `
    -p:SelfContained=true `
    -p:PublishReadyToRun=true `
    -p:RuntimeIdentifier=win-x64
```

---

## 🐛 Troubleshooting

### Erro: "dotnet not found"
- Instale .NET 6.0: https://dotnet.microsoft.com/download
- Reinicie o terminal depois

### Erro: "Unable to find RuntimeIdentifier"
- Use: `dotnet rid` para ver os RIDs disponíveis
- Para Windows: `win-x64` (64-bit) ou `win-x86` (32-bit)

### Aplicação não inicia
- Verificar logs em `logs/app.log`
- Executar via PowerShell com `-ErrorAction Continue` para mais detalhes

### Arquivo muito grande
- Use `PublishTrimmed=true` para reduzir tamanho
- Use compressão ZIP (reduz ~60-70%)

---

## 📊 Comparação de Opções

| Opção | Tamanho | Requer .NET | Inicialização | Uso |
|-------|---------|-------------|---------------|---------|
| SelfContained=true | 50-80 MB | Não | Normal | Distribuição |
| SelfContained=false | 10-20 MB | Sim | Mais rápido | Uso local |
| Com Trimming | 30-50 MB | Não | Normal | Produção |
| Com ReadyToRun | 60-90 MB | Não | Muito rápido | Performance |

---

## 📝 Checklist para Distribuição

- [ ] Compilação bem-sucedida
- [ ] Executável testado e funcionando
- [ ] Arquivo .exe presente em `dist/`
- [ ] Todas as DLLs necessárias presentes
- [ ] Sem erros ao iniciar aplicação
- [ ] Arquivo compactado (ZIP ou 7Z)
- [ ] Versão documentada (v1.0.0)
- [ ] Release notes preparadas
- [ ] README com instruções de uso
- [ ] Checksum (SHA256) calculado

---

## 🚀 Próximos Passos

1. Executar `build.bat` para gerar o .exe
2. Testar o executável
3. Comprimir para distribuição
4. Criar GitHub Release
5. Compartilhar com o mundo! 🌍

---

**OpenTURZXStudio v1.0.0 - Pronto para Distribuição!** 🎉
