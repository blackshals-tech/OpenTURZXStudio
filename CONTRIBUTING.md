# Guia de Contribuição

Obrigado por se interessar em contribuir para o OpenTURZXStudio! 🎉

## Como Contribuir

### Reportando Bugs

1. **Verifique se o bug já foi reportado** no [Issues](https://github.com/blackshals-tech/OpenTURZXStudio/issues)
2. **Descreva o problema** com clareza:
   - Qual é o comportamento esperado?
   - Qual é o comportamento atual?
   - Como reproduzir o problema?

3. **Forneça exemplos** para demonstrar as etapas
4. **Inclua screenshots** ou logs se relevante

### Sugerindo Melhorias

1. Abra uma [Issue](https://github.com/blackshals-tech/OpenTURZXStudio/issues)
2. Use um título descritivo
3. Forneça uma descrição clara da melhoria
4. Liste exemplos de como a melhoria funcionaria

### Pull Requests

1. **Fork o repositório**
   ```bash
   git clone https://github.com/seu-usuario/OpenTURZXStudio.git
   ```

2. **Crie uma branch** para sua feature
   ```bash
   git checkout -b feature/NovaFeature
   ```

3. **Faça suas mudanças** e commit
   ```bash
   git commit -m 'Add NovaFeature'
   ```

4. **Push para a branch**
   ```bash
   git push origin feature/NovaFeature
   ```

5. **Abra um Pull Request** descrevendo suas mudanças

## Padrões de Código

### C# Style Guide

```csharp
// Nomes com PascalCase para classes e métodos públicos
public class MyClass
{
    // Nomes com camelCase para variáveis locais
    private string _privateField;
    
    // Usar explicit typing
    public void MyMethod()
    {
        var result = SomeMethod();
    }
    
    // Adicionar XML documentation
    /// <summary>
    /// Faz algo importante.
    /// </summary>
    /// <param name="param1">Primeiro parâmetro</param>
    /// <returns>O resultado</returns>
    public string DoSomething(string param1)
    {
        return param1;
    }
}
```

### Nomeação

- **Classes**: `PascalCase` (ex: `DeviceDetector`)
- **Métodos**: `PascalCase` (ex: `GetAvailablePorts`)
- **Variáveis locais**: `camelCase` (ex: `deviceName`)
- **Constantes**: `UPPER_SNAKE_CASE` (ex: `MAX_CHUNK_SIZE`)
- **Interfaces**: Prefixo `I` (ex: `IDisposable`)

### Commits

- Use mensagens claras e descritivas
- Comece com um verbo no imperativo
- Exemplos:
  - ✅ "Add device detection feature"
  - ✅ "Fix serial port timeout issue"
  - ✅ "Refactor ImageSender class"
  - ❌ "fixed bug"
  - ❌ "update"

## Testes

- Adicione testes para novas funcionalidades
- Mantenha a cobertura acima de 85%
- Use xUnit + Moq + FluentAssertions

```csharp
[Fact]
public void MyFeature_Should_DoSomething()
{
    // Arrange
    var obj = new MyClass();
    
    // Act
    var result = obj.DoSomething();
    
    // Assert
    result.Should().Be(expected);
}
```

## Documentação

- Adicione XML documentation para públicos
- Mantenha o README atualizado
- Documente mudanças no CHANGELOG.md

## Processo de Review

1. Um maintainer irá revisar seu PR
2. Pode haver sugestões de mudanças
3. Após aprovação, o PR será mergeado
4. Sua contribuição será creditada no CHANGELOG

## Código de Conduta

Seja respeitoso com outros contribuidores. Discriminação, assédio ou abuso não serão tolerados.

## Dúvidas?

Abra uma [Discussion](https://github.com/blackshals-tech/OpenTURZXStudio/discussions) ou uma [Issue](https://github.com/blackshals-tech/OpenTURZXStudio/issues).

---

Obrigado por contribuir! 🚀
