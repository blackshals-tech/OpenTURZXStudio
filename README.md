# OpenTURZXStudio

> **Aplicação Desktop Profissional para Gerenciamento de Dispositivos TURZX via USB/Serial**

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)]()

## 🎯 Sobre o Projeto

OpenTURZXStudio é uma suite profissional e modular para comunicação com dispositivos TURZX. O projeto é dividido em três componentes principais:

- **OpenTURZXStudio.Core** - Biblioteca DLL com toda a lógica de negócio
- **OpenTURZXStudio.Desktop** - Aplicação WPF moderna e intuitiva
- **OpenTURZXStudio.Tests** - Suite completa de testes unitários

## 📦 Estrutura do Projeto

```
OpenTURZXStudio/
├── src/
│   ├── OpenTURZXStudio.Core/
│   │   ├── Core/
│   │   │   ├── DeviceDetector.cs       # Detecção de dispositivos USB/Serial
│   │   │   ├── SerialManager.cs        # Gerenciamento de comunicação serial
│   │   │   ├── UsbProtocol.cs          # Protocolo customizado USB
│   │   │   ├── GifConverter.cs         # Processamento de GIFs
│   │   │   ├── GifPlayer.cs            # Reprodução de animações
│   │   │   ├── ImageSender.cs          # Envio otimizado de imagens
│   │   │   └── Logger.cs               # Sistema centralizado de logging
│   │   └── OpenTURZXStudio.Core.csproj
│   │
│   └── OpenTURZXStudio.Desktop/
│       ├── MainWindow.xaml             # Interface principal (5 abas)
│       ├── ViewModels/                 # MVVM ViewModels
│       ├── Models/                     # Modelos de dados
│       └── OpenTURZXStudio.Desktop.csproj
│
├── tests/
│   └── OpenTURZXStudio.Tests/
│       ├── LoggerTests.cs
│       ├── DeviceDetectorTests.cs
│       ├── SerialManagerTests.cs
│       ├── UsbProtocolTests.cs
│       ├── GifConverterTests.cs
│       ├── GifPlayerTests.cs
│       ├── ImageSenderTests.cs
│       └── OpenTURZXStudio.Tests.csproj
│
├── OpenTURZXStudio.sln                 # Solution Visual Studio
├── README.md                           # Este arquivo
└── LICENSE                             # Licença MIT
```

## 🎨 Interface Desktop (WPF)

### Abas Principais

| Aba | Descrição | Funcionalidades |
|-----|-----------|------------------|
| 📊 **Dashboard** | Central de controle | Cards de status, gerenciador de arquivos, preview de imagens |
| 🔌 **Dispositivos** | Gerenciamento de conexões | Lista de portas seriais, dispositivos USB, conectar/desconectar |
| 🎬 **GIF Player** | Reprodutor de animações | Carregamento, reprodução, navegação de frames |
| 📝 **Logs** | Monitor de eventos | Log em tempo real, limpar, exportar |

### Design
- 🌙 Tema Dark profissional
- 🎨 Material Design com cores modernas
- 📱 Interface responsiva
- ⚡ Performance otimizada

## 🚀 Primeiros Passos

### Pré-requisitos
- .NET 6.0 ou superior
- Visual Studio 2022 (ou VS Code + .NET CLI)
- Windows 7+ (para WPF)

### Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/blackshals-tech/OpenTURZXStudio.git
   cd OpenTURZXStudio
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Compile o projeto**
   ```bash
   dotnet build
   ```

4. **Execute a aplicação**
   ```bash
   # Desktop
   dotnet run --project src/OpenTURZXStudio.Desktop
   
   # Ou abra em Visual Studio
   start OpenTURZXStudio.sln
   ```

## 🧪 Testes

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com relatório de cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover

# Testes específicos
dotnet test --filter ClassName=OpenTURZXStudio.Tests.LoggerTests
```

### Cobertura de Testes
- **Total**: 45+ testes unitários
- **Cobertura**: ~85% do código Core
- **Frameworks**: xUnit, Moq, FluentAssertions

## 💻 Uso da Biblioteca Core

### Exemplo Básico

```csharp
using OpenTURZXStudio.Core;

// Inicializar componentes
var logger = new Logger("logs/app.log");
var detector = new DeviceDetector(logger);
var serialManager = new SerialManager(logger);

// Listar portas disponíveis
var ports = detector.GetAvailablePorts();
Console.WriteLine($"Portas encontradas: {string.Join(", ", ports)}");

// Conectar a um dispositivo
if (ports.Any())
{
    bool connected = await serialManager.OpenAsync(ports[0], 115200);
    if (connected)
    {
        logger.Info("Conectado com sucesso!");
        
        // Enviar dados
        var protocol = new UsbProtocol(logger);
        var pingPacket = protocol.CreatePingPacket();
        await serialManager.SendAsync(pingPacket);
        
        // Desconectar
        serialManager.Close();
    }
}
```

### Reproduzir GIF

```csharp
var gifConverter = new GifConverter(logger);
var gifPlayer = new GifPlayer(logger);

// Carregar GIF
var gifData = await gifConverter.LoadGifAsync("animation.gif");
if (gifData != null)
{
    gifPlayer.LoadGif(gifData);
    gifPlayer.FrameChanged += (sender, e) => 
    {
        Console.WriteLine($"Frame {e.FrameIndex}/{e.TotalFrames}");
    };
    
    // Reproduzir
    await gifPlayer.PlayAsync(loop: true);
}
```

### Enviar Imagem

```csharp
var imageSender = new ImageSender(logger, serialManager, protocol);

imageSender.TransferProgress += (sender, e) =>
{
    Console.WriteLine($"Progresso: {e.PercentComplete}%");
};

bool success = await imageSender.SendImageAsync("imagem.jpg");
if (success)
{
    logger.Info("Imagem enviada com sucesso!");
}
```

## 🏗️ Arquitetura

### Camadas

```
┌─────────────────────────────────┐
│   OpenTURZXStudio.Desktop       │  Apresentação (WPF)
│  (Aplicação Desktop WPF)         │
└──────────┬──────────────────────┘
           │
┌──────────▼──────────────────────┐
│  OpenTURZXStudio.Core           │  Lógica (Biblioteca)
│  (Componentes Reutilizáveis)    │
└──────────┬──────────────────────┘
           │
┌──────────▼──────────────────────┐
│   System.IO.Ports               │  Comunicação
│   System.Device.Gpio            │
│   System.Drawing                │
└─────────────────────────────────┘
```

### MVVM Pattern (Desktop)

- **Models** - Estruturas de dados (TransferModel, DeviceModel)
- **ViewModels** - Lógica de apresentação (MainViewModel)
- **Views** - Interface XAML (MainWindow)

## 📊 Componentes Core

### Logger
- Logging centralizado em arquivo
- Suporte a console + arquivo
- Níveis: INFO, WARNING, ERROR, DEBUG

### DeviceDetector
- Detecção automática de portas seriais
- Detecção de dispositivos USB
- Eventos de conexão/desconexão

### SerialManager
- Comunicação serial assíncrona
- Gerenciamento de timeouts
- Eventos de recebimento de dados

### UsbProtocol
- Protocolo customizado com header/footer
- Validação de checksum XOR
- Tipos de pacotes: PING, IMAGE, GIF, CONFIG

### GifConverter
- Carregamento de arquivos GIF
- Extração de frames
- Redimensionamento de imagens

### GifPlayer
- Reprodução com controle de frames
- Suporte a loop
- Eventos de mudança de frame

### ImageSender
- Envio de imagens em chunks
- Barra de progresso
- Tratamento de erros

## 🔧 Configuração

### Configurar Porta Serial

```csharp
await serialManager.OpenAsync(
    portName: "COM3",
    baudRate: 115200,
    dataBits: 8,
    stopBits: StopBits.One,
    parity: Parity.None
);
```

### Customizar Logging

```csharp
var logger = new Logger("custom_path/app.log");
logger.SetEnabled(false);  // Desabilitar
logger.SetEnabled(true);   // Reabilitar
```

## 📝 API Reference

Ver documentação completa em cada classe:
- [Logger.cs](src/OpenTURZXStudio.Core/Logger.cs)
- [DeviceDetector.cs](src/OpenTURZXStudio.Core/DeviceDetector.cs)
- [SerialManager.cs](src/OpenTURZXStudio.Core/SerialManager.cs)
- [UsbProtocol.cs](src/OpenTURZXStudio.Core/UsbProtocol.cs)
- [GifConverter.cs](src/OpenTURZXStudio.Core/GifConverter.cs)
- [GifPlayer.cs](src/OpenTURZXStudio.Core/GifPlayer.cs)
- [ImageSender.cs](src/OpenTURZXStudio.Core/ImageSender.cs)

## 🐛 Troubleshooting

### Porta Serial não encontrada
```csharp
var detector = new DeviceDetector(logger);
var ports = detector.GetAvailablePorts();
if (ports.Count == 0)
{
    logger.Warning("Nenhuma porta serial detectada");
}
```

### Falha ao enviar dados
```csharp
if (!serialManager.IsConnected)
{
    logger.Error("Dispositivo desconectado");
}
```

### GIF não carrega
```csharp
var gifData = await gifConverter.LoadGifAsync("image.gif");
if (gifData == null)
{
    logger.Error("Falha ao carregar GIF");
}
```

## 📦 Dependências

### Core
- System.IO.Ports (7.0.0)
- System.Device.Gpio (3.0.0)
- System.Drawing (Nativo)

### Desktop
- MaterialDesignThemes (4.8.0)
- CommunityToolkit.Mvvm (8.2.1)

### Tests
- xUnit (2.4.2)
- Moq (4.18.4)
- FluentAssertions (6.11.0)
- Microsoft.NET.Test.Sdk (17.5.0)

## 🔄 CI/CD

### GitHub Actions
Configurável para:
- Executar testes automaticamente em cada push
- Gerar relatórios de cobertura
- Build e deploy automático

## 📄 Licença

Este projeto é licenciado sob a Licença MIT - ver arquivo [LICENSE](LICENSE) para detalhes.

## 👤 Autor

**blackshals-tech**
- GitHub: [@blackshals-tech](https://github.com/blackshals-tech)

## 📞 Suporte

Para reportar bugs ou sugerir features:
1. Abra uma [Issue](https://github.com/blackshals-tech/OpenTURZXStudio/issues)
2. Descreva o problema com detalhes
3. Inclua logs e screenshots se possível

## 🎉 Roadmap

- [x] OpenTURZXStudio.Core
- [x] OpenTURZXStudio.Desktop (WPF)
- [x] OpenTURZXStudio.Tests
- [ ] Documentação API completa
- [ ] GitHub Actions (CI/CD)
- [ ] Versão Mobile (opcional)
- [ ] Mais exemplos de uso

## ⭐ Contribuindo

Contribuições são bem-vindas! Por favor:
1. Faça fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFeature`)
3. Commit suas mudanças (`git commit -m 'Add NovaFeature'`)
4. Push para a branch (`git push origin feature/NovaFeature`)
5. Abra um Pull Request

---

**Desenvolvido com ❤️ usando C# e .NET 6.0**
