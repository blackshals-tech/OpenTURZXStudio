# OpenTURZXStudio

## Descrição

OpenTURZXStudio é uma suite profissional de ferramentas para comunicação com dispositivos TURZX via USB/Serial. O projeto é dividido em componentes modulares:

### **OpenTURZXStudio.Core** (Em desenvolvimento)

Biblioteca DLL com toda a lógica de negócio:

- **DeviceDetector** - Detecção de dispositivos USB e portas seriais
- **SerialManager** - Gerenciamento de comunicação serial assíncrona
- **UsbProtocol** - Protocolo customizado de comunicação USB
- **GifConverter** - Carregamento e processamento de arquivos GIF
- **GifPlayer** - Reprodução de animações GIF com controle de frames
- **ImageSender** - Envio otimizado de imagens e GIFs para dispositivos
- **Logger** - Sistema centralizado de logging

## Arquitetura

```
OpenTURZXStudio.sln
├── OpenTURZXStudio.Core (Class Library)
│   └── Core components (DeviceDetector, SerialManager, etc.)
├── OpenTURZXStudio.Desktop (WPF App) [Próximo]
└── OpenTURZXStudio.Tests (Unit Tests) [Próximo]
```

## Tecnologias

- **.NET 6.0** - Framework base
- **C# 10** - Linguagem
- **System.IO.Ports** - Comunicação serial
- **System.Device.Gpio** - Gerenciamento de dispositivos
- **System.Drawing** - Processamento de imagens

## Roadmap

- [x] OpenTURZXStudio.Core
- [ ] OpenTURZXStudio.Desktop (WPF)
- [ ] OpenTURZXStudio.Tests
- [ ] Documentação API
- [ ] Exemplos de uso

## Como usar

```csharp
// Inicializar componentes
var logger = new Logger("logs/app.log");
var detector = new DeviceDetector(logger);
var serialManager = new SerialManager(logger);

// Listar portas disponíveis
var ports = detector.GetAvailablePorts();

// Conectar a um dispositivo
await serialManager.OpenAsync("COM3", 115200);

// Enviar dados
var usbProtocol = new UsbProtocol(logger);
var pingPacket = usbProtocol.CreatePingPacket();
await serialManager.SendAsync(pingPacket);
```

## Licença

MIT
