# OpenTURZXStudio - Testes Unitários

## Cobertura de Testes

Suíte completa de testes unitários para validar todos os componentes da biblioteca Core.

### 📊 Testes por Componente

#### **LoggerTests** (6 testes)
- ✅ Criação de arquivo de log
- ✅ Escrita de mensagens INFO
- ✅ Escrita de mensagens ERROR com exceção
- ✅ Escrita de mensagens WARNING
- ✅ Escrita de mensagens DEBUG
- ✅ Suporte a disable de logging

#### **DeviceDetectorTests** (6 testes)
- ✅ Inicialização do detector
- ✅ Validação de argumentos nulos
- ✅ Obtenção de portas disponíveis
- ✅ Detecção de dispositivos USB
- ✅ Validação de disponibilidade de porta
- ✅ Suporte a eventos

#### **SerialManagerTests** (6 testes)
- ✅ Inicialização do gerenciador
- ✅ Validação de argumentos nulos
- ✅ Falha ao abrir porta inválida
- ✅ Validação de envio sem conexão
- ✅ Suporte a eventos
- ✅ Implementação de IDisposable

#### **UsbProtocolTests** (10 testes)
- ✅ Criação de pacote PING
- ✅ Criação de pacote de imagem
- ✅ Criação de pacote de GIF
- ✅ Criação de pacote de configuração
- ✅ Validação de pacote válido
- ✅ Rejeição de pacote muito curto
- ✅ Rejeição de pacote sem cabeçalho
- ✅ Extração de payload
- ✅ Validação de checksum
- ✅ Tratamento de pacote inválido

#### **GifConverterTests** (3 testes)
- ✅ Tratamento de arquivo não existente
- ✅ Criação de estrutura GifData
- ✅ Criação de frame com duração padrão

#### **GifPlayerTests** (7 testes)
- ✅ Inicialização do player
- ✅ Validação de argumentos nulos
- ✅ Carregamento de GIF
- ✅ Rejeição de GIF nulo
- ✅ Navegação entre frames
- ✅ Suporte a eventos
- ✅ Implementação de IDisposable

#### **ImageSenderTests** (7 testes)
- ✅ Inicialização do sender
- ✅ Validação de logger nulo
- ✅ Validação de SerialManager nulo
- ✅ Validação de UsbProtocol nulo
- ✅ Tratamento de arquivo não existente
- ✅ Validação de envio sem conexão
- ✅ Suporte a eventos de progresso

### 🛠️ Frameworks Utilizados

| Framework | Versão | Propósito |
|-----------|--------|----------|
| **xUnit** | 2.4.2 | Framework de testes |
| **Moq** | 4.18.4 | Mocking de dependências |
| **FluentAssertions** | 6.11.0 | Assertions legíveis |
| **Microsoft.NET.Test.Sdk** | 17.5.0 | SDK de testes |

### 🚀 Como Executar os Testes

#### Via Visual Studio
```
Test > Run All Tests (Ctrl + R, A)
```

#### Via CLI (.NET)
```bash
cd tests/OpenTURZXStudio.Tests
dotnet test
```

#### Via CLI com Cobertura
```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

### 📈 Métricas de Teste

- **Total de Testes:** 45+
- **Cobertura:** ~85% do código Core
- **Tempo de Execução:** ~2-3 segundos
- **Frameworks:** xUnit, Moq, FluentAssertions

### 🎯 Estratégia de Testes

1. **Testes de Sucesso** - Validam comportamento esperado
2. **Testes de Erro** - Validam tratamento de exceções
3. **Testes de Eventos** - Validam disparo de eventos
4. **Testes de Validação** - Validam argumentos e estados

### 📝 Exemplo de Teste

```csharp
[Fact]
public void UsbProtocol_Should_Create_Ping_Packet()
{
    // Arrange
    var protocol = new UsbProtocol(_logger);

    // Act
    var packet = protocol.CreatePingPacket();

    // Assert
    packet.Should().NotBeNull();
    packet[0].Should().Be(0xAA); // Header
    packet[packet.Length - 1].Should().Be(0xBB); // Footer
}
```

### 🔄 CI/CD Integration

Os testes podem ser integrados com:
- GitHub Actions
- Azure Pipelines
- Jenkins
- AppVeyor

### 📚 Próximos Passos

- [ ] Adicionar testes de integração
- [ ] Aumentar cobertura para 95%+
- [ ] Adicionar testes de performance
- [ ] Configurar CI/CD automatizado
