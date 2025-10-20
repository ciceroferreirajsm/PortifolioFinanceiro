# 🛠️ Guia Rápido para Desenvolvedores

## ⚡ Execução Rápida
```bash
# 1. Clonar/baixar o projeto
cd PortifolioFinanceiro

# 2. Restaurar dependências
dotnet restore

# 3. Executar
dotnet run

# 4. Acessar
# http://localhost:5000 (Swagger UI)
```

## 📋 Checklist de Desenvolvimento

### ✅ **Implementado**
- [x] CRUD completo de Assets
- [x] CRUD completo de Portfolios e Positions
- [x] Carregamento automático do SeedData.json
- [x] Cálculo de valor atual do portfólio
- [x] Cálculo de retorno total e percentual
- [x] Algoritmo básico de rebalanceamento
- [x] Endpoints funcionais com validação
- [x] Documentação Swagger completa
- [x] Repository Pattern + Service Layer
- [x] DTOs com validação
- [x] Tratamento de erros
- [x] Logs estruturados

### 🚀 **Próximas Implementações**
- [ ] Cálculo de volatilidade com histórico
- [ ] Sharpe ratio completo
- [ ] Análise de concentração por setor
- [ ] Sistema de alertas
- [ ] Testes unitários
- [ ] Testes de integração

## 🔗 Endpoints Principais

### Assets
- `GET /api/assets` - Lista todos
- `GET /api/assets/{id}` - Por ID
- `GET /api/assets/search?symbol=PETR4` - Por símbolo
- `POST /api/assets` - Criar
- `PUT /api/assets/{id}/price` - Atualizar preço

### Portfolios
- `GET /api/portfolios` - Lista todos
- `GET /api/portfolios?userId=user-001` - Por usuário
- `GET /api/portfolios/{id}` - Detalhes completos
- `POST /api/portfolios` - Criar
- `POST /api/portfolios/{id}/positions` - Adicionar posição
- `PUT /api/portfolios/{id}/positions/{positionId}` - Atualizar posição
- `DELETE /api/portfolios/{id}/positions/{positionId}` - Remover posição

### Analytics
- `GET /api/portfolios/{id}/performance` - Performance
- `GET /api/portfolios/{id}/risk-analysis` - Análise de risco
- `GET /api/portfolios/{id}/rebalancing` - Sugestões de rebalanceamento

## 🗄️ Estrutura de Dados

### Entidades Principais
```csharp
Asset (Id, Symbol, Name, Type, Sector, CurrentPrice, LastUpdated)
Portfolio (Id, Name, UserId, TotalInvestment, CreatedAt)
Position (Id, AssetId, PortfolioId, Quantity, AveragePrice, TargetAllocation)
User (Id, Name)
PriceHistory (Id, AssetId, Date, Price)
Transaction (Id, PortfolioId, AssetId, Date, Quantity, Price, Type)
```

### Relacionamentos
```
User (1) ←→ (N) Portfolio
Portfolio (1) ←→ (N) Position
Portfolio (1) ←→ (N) Transaction
Asset (1) ←→ (N) Position
Asset (1) ←→ (N) PriceHistory
Asset (1) ←→ (N) Transaction
```

## 🧮 Fórmulas Implementadas

### Performance
```csharp
CurrentValue = Sum(Position.Quantity * Asset.CurrentPrice)
TotalReturn = CurrentValue - TotalInvestment
ReturnPercentage = (TotalReturn / TotalInvestment) * 100
```

### Alocação
```csharp
CurrentAllocation = (PositionCurrentValue / PortfolioCurrentValue)
Deviation = |CurrentAllocation - TargetAllocation|
```

### Rebalanceamento
```csharp
TargetValue = PortfolioCurrentValue * TargetAllocation
AdjustmentValue = TargetValue - PositionCurrentValue
Action = AdjustmentValue > 0 ? "BUY" : "SELL"
SuggestedQuantity = |AdjustmentValue| / CurrentPrice
```

## 🗂️ Arquivos Importantes

```
📁 PortifolioFinanceiro/
├── 📄 SeedData.json          # Dados iniciais (NÃO MODIFICAR)
├── 📄 Program.cs             # Configuração da aplicação
├── 📄 README.md              # Documentação principal
├── 📄 RELATORIO_CORRECOES.md # Análise das correções feitas
└── 📁 Models/DTOs/           # DTOs para comunicação API
    ├── AssetDto.cs           # DTOs de ativos
    ├── PortfolioDto.cs       # DTOs de portfólios
    └── SeedDataDto.cs        # DTOs para deserialização
```

## 🔧 Configurações de Desenvolvimento

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Banco de Dados
- **Tipo**: In-Memory Database
- **Nome**: "PortifolioFinanceiroDb"
- **Seed**: Automático na inicialização
- **Dados**: 15 ativos, 3 portfólios, 3 usuários

## 🐛 Debugging

### Logs Importantes
```
[INF] Starting database seeding...
[INF] Seeded 3 users
[INF] Seeded 15 assets
[INF] Seeded price history for 5 assets
[INF] Seeded 3 portfolios
[INF] Database seeding completed successfully.
```

### Possíveis Problemas
1. **SeedData.json não encontrado**: Verificar se está na raiz do projeto
2. **Erro de deserialização**: Verificar formato JSON
3. **Seed duplicado**: Logs indicam "Database already contains data"

## 📊 Dados de Teste

### Usuários
- `user-001`: Investidor Conservador
- `user-002`: Investidor de Crescimento  
- `user-003`: Investidor de Dividendos

### Ativos Principais
- `PETR4`: Petrobras PN (R$ 35,50)
- `VALE3`: Vale ON (R$ 65,20)
- `ITUB4`: Itaú PN (R$ 32,10)
- `BBDC4`: Bradesco PN (R$ 15,80)
- `MGLU3`: Magazine Luiza ON (R$ 8,75)

## 💡 Dicas de Implementação

1. **Sempre validar DTOs** antes de processar
2. **Usar try-catch** em todos os métodos de service
3. **Logs informativos** para debugging
4. **Retornar códigos HTTP apropriados**
5. **Documentar métodos públicos** para Swagger
6. **Os testes unitarios foram desenvolvidos usando o Copilot**

---

**🚀 Portfolio Financeiro API - Desenvolvido com .NET 8 - **



