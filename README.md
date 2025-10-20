# 📈 Portfolio Financeiro API

Uma **API REST em .NET 8** para gerenciamento completo de portfólios de investimentos com análises financeiras avançadas.

## � Quick Start

### Pré-requisitos
- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))

### Executar
```bash
dotnet restore
dotnet run
```

### Acessar
- **Swagger UI**: `http://localhost:5000`
- A aplicação abre diretamente na documentação interativa

## 🎯 Funcionalidades

### ✅ **Gestão de Ativos**
- CRUD completo de ativos financeiros
- 15 ativos reais da bolsa brasileira pré-carregados
- Busca por símbolo (ex: PETR4, VALE3)
- Atualização de preços

### ✅ **Gestão de Portfólios**
- Criação e gerenciamento de portfólios
- Adição/remoção/atualização de posições
- 3 portfólios modelo pré-configurados
- Cálculos de valor atual e rentabilidade

### ✅ **Análises Financeiras**
- **Performance**: Retorno total e percentual por posição
- **Análise de Risco**: Diversificação por setor e concentração
- **Rebalanceamento**: Sugestões automáticas baseadas na alocação target

## 🏗️ Tecnologias

- **.NET 8** + **ASP.NET Core Web API**
- **Entity Framework Core** com **In-Memory Database**
- **Repository Pattern** + **Service Layer**
- **Swagger/OpenAPI** para documentação
- **DTOs** com validação automática

## 📊 Dados de Demonstração

A aplicação vem com dados reais pré-carregados:
- **15 ativos** da bolsa brasileira (PETR4, VALE3, ITUB4, etc.)
- **3 portfólios** modelo (Conservador, Crescimento, Dividendos)
- **30 dias** de histórico de preços para análises

## 🧪 Exemplos de Uso

### Listar ativos
```http
GET /api/assets
```

### Buscar por símbolo
```http
GET /api/assets/search?symbol=PETR4
```

### Detalhes do portfólio
```http
GET /api/portfolios/1
```

### Análise de performance
```http
GET /api/portfolios/1/performance
```

### Sugestões de rebalanceamento
```http
GET /api/portfolios/1/rebalancing
```

## 📈 Resposta de Exemplo

```json
{
  "portfolioId": 1,
  "portfolioName": "Portfólio Conservador",
  "totalInvestment": 100000.00,
  "currentValue": 105750.00,
  "totalReturn": 5750.00,
  "returnPercentage": 5.75,
  "positions": [
    {
      "asset": "PETR4",
      "quantity": 500,
      "currentPrice": 35.50,
      "returnPercentage": 18.33
    }
  ]
}
```

## 🎯 Arquitetura

```
Controllers/ → Services/ → Repositories/ → Models/
     ↓              ↓            ↓           ↓
  Endpoints    Lógica de    Acesso a    Entidades
   da API      Negócio       Dados       & DTOs
```

## 🔍 Funcionalidades Principais

✅ **CRUD completo** de Assets e Portfolios  
✅ **Carregamento automático** do SeedData.json  
✅ **Cálculo de valor atual** do portfólio  
✅ **Cálculo de retorno** total e percentual  
✅ **Algoritmo básico** de rebalanceamento  
✅ **Endpoints funcionais** com validação  
✅ **Documentação Swagger** completa  

---

**Portfolio Financeiro API** - Desenvolvido com .NET 8

---

## 🏗️ Estrutura Técnica Esperada

### 1. Arquitetura em Camadas
```
├── Controllers/          # API Controllers
├── Services/            # Lógica de negócio
├── Repositories/        # Acesso a dados
├── Models/             # Entidades e DTOs
├── Infrastructure/     # Configurações, DbContext
└── Tests/             # Testes unitários
```

### 2. Entidades e Relacionamentos

Você deve modelar as entidades necessárias para representar:
- **Ativos financeiros** com informações como símbolo (PETR4), nome, tipo, setor e preço
- **Portfólios** pertencentes a usuários específicos
- **Posições** que representam a quantidade de cada ativo no portfólio
- **Histórico de preços** para cálculos de performance
- **Transações** de compra/venda para rastreabilidade

*Dica: Pense nos relacionamentos entre as entidades e como isso impacta os cálculos financeiros.*

### 3. Endpoints Esperados

#### Assets Controller
- `GET /api/assets` - Listar todos os ativos
- `GET /api/assets/{id}` - Obter ativo específico
- `GET /api/assets/search?symbol={symbol}` - Buscar por símbolo
- `POST /api/assets` - Criar novo ativo
- `PUT /api/assets/{id}/price` - Atualizar preço

#### Portfolios Controller
- `GET /api/portfolios` - Listar portfólios do usuário
- `POST /api/portfolios` - Criar novo portfólio
- `GET /api/portfolios/{id}` - Detalhes do portfólio
- `POST /api/portfolios/{id}/positions` - Adicionar posição
- `PUT /api/portfolios/{id}/positions/{positionId}` - Atualizar posição
- `DELETE /api/portfolios/{id}/positions/{positionId}` - Remover posição

#### Analytics Controller
- `GET /api/portfolios/{id}/performance` - Performance do portfólio
- `GET /api/portfolios/{id}/risk-analysis` - Análise de risco
- `GET /api/portfolios/{id}/rebalancing` - Sugestão de rebalanceamento

---

## 📋 Regras de Negócio

### 1. Cálculos de Performance
*Métricas para avaliar como o investimento está performando ao longo do tempo.*

**Requisitos:**
- **Retorno Total**: Percentual de ganho/perda desde o investimento inicial. Ex: investiu R$ 1000, hoje vale R$ 1200 = 20% de retorno
- **Retorno Anualizado**: Retorno convertido para base anual, considerando o tempo de investimento. Permite comparar investimentos de períodos diferentes
- **Volatilidade**: Mede o quanto o preço do ativo varia (risco). Alto desvio padrão = mais volátil = mais arriscado
- Todos os cálculos devem tratar casos extremos (divisão por zero, dados insuficientes)

### 2. Sistema de Rebalanceamento
*Processo de ajustar o portfólio para manter a estratégia de investimento planejada.*

**Requisitos:**
- **Alocação Ideal**: Estratégia definida pelo investidor (ex: 30% em bancos, 20% em mineração)
- **Peso Atual**: Percentual real de cada ativo no portfólio hoje (pode ter mudado com oscilações de preço)
- **Transações Sugeridas**: Compras/vendas para voltar à alocação desejada
- Minimizar o **número de transações** (menos custos e complexidade)
- Considerar **custos de transação** de 0.3% por operação
- Não sugerir transações menores que R$ 100,00 (não compensa os custos)

### 3. Análise de Risco e Diversificação
*Métricas para avaliar o nível de risco do portfólio e sua diversificação.*

**Requisitos:**
- **Sharpe Ratio**: Mede retorno ajustado ao risco. Quanto maior, melhor (mais retorno por unidade de risco)
- **Taxa Selic**: Taxa básica de juros do Brasil, usada como referência de investimento "sem risco"
- **Concentração por Setor**: Evita ter muito dinheiro em um setor só (ex: só bancos = risco se setor financeiro quebrar)
- **Risco de Concentração**: Percentual do maior ativo individual (evita "colocar todos os ovos numa cesta")
- **Correlação entre Ativos**: Ativos do mesmo setor tendem a subir/descer juntos, reduzindo diversificação

---

## 📊 Guia de Utilização dos Dados

### 📁 Arquivo SeedData.json
O arquivo `SeedData.json` é sua **fonte única de dados** para o teste. Ele contém:

#### 🏢 **Assets (15 ativos)**
```json
{
  "symbol": "PETR4",
  "name": "Petrobras PN", 
  "type": "Stock",
  "sector": "Energy",
  "currentPrice": 35.50
}
```
- **15 ativos** reais da bolsa brasileira
- **10 setores** diversificados (Energy, Financial, Mining, etc.)
- Preços atualizados para outubro/2024

#### 💼 **Portfolios (3 perfis)**
```json
{
  "name": "Portfólio Conservador",
  "userId": "user-001",
  "totalInvestment": 100000.00,
  "positions": [...]
}
```
- **Conservador**: Foco em dividendos e baixo risco
- **Crescimento**: Ações de tecnologia e varejo
- **Dividendos**: Empresas maduras com boa distribuição

#### 📈 **Price History (30 dias)**
- Histórico completo para **5 ativos principais**
- Dados diários de setembro-outubro/2024
- Base para cálculos de volatilidade e retorno

#### 🏛️ **Market Data**
- Taxa Selic: 12% a.a.
- Performance do Ibovespa
- Métricas por setor

---

### 🔧 Como Implementar

#### 1. **Relacionamentos Importantes**
- Um Portfolio tem múltiplas Positions
- Uma Position referencia um Asset (por Symbol)
- PriceHistory vinculado ao Asset
- Calcule valores atuais usando CurrentPrice

---

### 🚨 Pontos de Atenção

#### **Não Hardcode Dados**
- Carregue todos os dados do `SeedData.json` na inicialização
- Use variáveis e constantes em vez de valores fixos

#### **Mantenha Consistência**
- Use `Symbol` como chave para relacionar Position ↔ Asset
- `CurrentPrice` do Asset vs `AveragePrice` da Position
- Datas no formato ISO 8601 (yyyy-MM-dd)

#### **Trate Edge Cases**
- E se não houver histórico de preços?
- E se a alocação target não somar 100%?
- E se o preço atual for zero?

---

## 🚀 Como Entregar

### 1. Submissão do Código
- Disponibilize o código em um repositório Git(Github, Gitlab...).
- Envie o link do repositório para avaliação.

### 2. Estrutura Mínima Esperada
- Controllers com todos os endpoints especificados
- Services com lógica de negócio e cálculos financeiros
- Models/Entities modeladas adequadamente
- DbContext configurado (In-Memory DB)
- Startup/Program.cs com DI configurada
- **Seed automático** do `SeedData.json` na inicialização

### 3. Documentação
- README com instruções de execução
- Comentários no código explicando algoritmos financeiros
- Documentação dos endpoints

### 4. Testes Obrigatórios
- Testes unitários dos cálculos financeiros críticos
- Validação usando cenários do `SeedData.json`

---

### ✅ **Entregas Obrigatórias**
- [ ] CRUD completo de Assets e Portfolios
- [ ] Carregamento automático do SeedData.json
- [ ] Cálculo de valor atual do portfólio
- [ ] Cálculo de retorno total e percentual
- [ ] Algoritmo básico de rebalanceamento
- [ ] 3 testes unitários dos cálculos críticos
- [ ] Endpoints funcionais com validação básica

### 🚀 **Diferencial**
- [ ] Cálculo de volatilidade com histórico
- [ ] Sharpe ratio completo
- [ ] Análise de concentração por setor
- [ ] Sistema de alertas
- [ ] Documentação Swagger completa
- [ ] Testes de integração

---

## 💡 Dicas

1. **Comece pelo básico**: CRUD primeiro, otimizações depois
2. **Use InMemory Database**: Mais rápido para desenvolvimento  
3. **Priorize algoritmos**: Cálculos valem mais pontos que endpoints
4. **Valide o essencial**: Preços negativos, quantidades inválidas
5. **Documente decisões**: Explique fórmulas financeiras usadas
6. **Teste frequentemente**: Valide cada cálculo implementado
7. **Atenção**: Funcional > perfeito#
