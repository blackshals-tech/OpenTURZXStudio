# OpenTURZXStudio v1.0.0 - Quick Start

## 🚀 Começar em 5 Minutos

### 1. Clone o Repositório
```bash
git clone https://github.com/blackshals-tech/OpenTURZXStudio.git
cd OpenTURZXStudio
```

### 2. Restaure e Compile
```bash
dotnet restore
dotnet build
```

### 3. Execute a Aplicação
```bash
cd src/OpenTURZXStudio.Desktop
dotnet run
```

### 4. Execute os Testes
```bash
cd tests/OpenTURZXStudio.Tests
dotnet test
```

## 📖 Documentação Rápida

### Usar a Biblioteca Core

```csharp
using OpenTURZXStudio.Core;

// Inicializar
var logger = new Logger("app.log");
var detector = new DeviceDetector(logger);
var serialManager = new SerialManager(logger);

// Detectar dispositivos
var ports = detector.GetAvailablePorts();
logger.Info($"Portas encontradas: {string.Join(", ", ports)}");

// Conectar
if (ports.Any())
{
    await serialManager.OpenAsync(ports[0], 115200);
    logger.Info("Conectado!");
}
```

### Reproduzir GIF

```csharp
var converter = new GifConverter(logger);
var player = new GifPlayer(logger);

var gif = await converter.LoadGifAsync("animation.gif");
player.LoadGif(gif);
await player.PlayAsync(loop: true);
```

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true
```

## 📚 Referência Completa

- [README.md](../README.md) - Guia completo
- [CHANGELOG.md](../CHANGELOG.md) - Histórico
- [CONTRIBUTING.md](../CONTRIBUTING.md) - Como contribuir

## 🆘 Suporte

- [Issues](https://github.com/blackshals-tech/OpenTURZXStudio/issues)
- [Discussions](https://github.com/blackshals-tech/OpenTURZXStudio/discussions)

---

**Pronto para começar?** Clone agora! 🚀
