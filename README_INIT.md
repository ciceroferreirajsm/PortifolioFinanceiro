# 📈 Portfolio Financeiro API

Uma **API REST** robusta desenvolvida em **.NET 8** para gerenciamento completo de portfólios de investimentos, oferecendo funcionalidades desde o CRUD básico de ativos até análises avançadas de performance e algoritmos de rebalanceamento.

## 🎯 Visão Geral

Este sistema permite que investidores gerenciem seus portfólios de forma inteligente, com:
- **Gestão de Ativos**: Controle completo de ações, fundos e outros instrumentos financeiros
- **Análise de Performance**: Métricas detalhadas de retorno, volatilidade e risco
- **Rebalanceamento Inteligente**: Algoritmos que sugerem ajustes para manter a estratégia de investimento
- **Histórico de Preços**: Acompanhamento da evolução dos ativos ao longo do tempo

## 🚀 Tecnologias Utilizadas

### Framework & Runtime
- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - Para criação da API REST
- **C# 12** - Linguagem de programação

### Banco de Dados & ORM
- **Entity Framework Core** - ORM para acesso a dados
- **In-Memory Database** - Banco em memória para desenvolvimento e testes
- **Microsoft.EntityFrameworkCore.InMemory** - Provider para banco em memória

### Documentação & Testes
- **Swagger/OpenAPI** - Documentação interativa da API
- **Swashbuckle.AspNetCore** - Geração automática do Swagger

### Arquitetura & Padrões
- **Repository Pattern** - Abstração da camada de dados
- **Service Layer** - Lógica de negócio centralizada
- **DTOs (Data Transfer Objects)** - Contratos de API bem definidos
- **Dependency Injection** - Inversão de controle nativa do .NET
- **Async/Await** - Programação assíncrona para melhor performance

## 🏗️ Arquitetura do Sistema

```
📁 PortifolioFinanceiro/
├── 📁 Controllers/           # Endpoints da API
│   ├── AssetsController.cs      # CRUD de ativos
│   ├── PortfoliosController.cs  # Gestão de portfólios
│   └── AnalyticsController.cs   # Análises e relatórios
├── 📁 Services/             # Lógica de negócio
│   ├── IAssetService.cs         # Interface do serviço de ativos
│   ├── AssetService.cs          # Implementação do serviço de ativos
│   ├── IPortfolioService.cs     # Interface do serviço de portfólios
│   ├── PortfolioService.cs      # Implementação do serviço de portfólios
│   ├── SeedDataService.cs       # Carregamento de dados iniciais
│   └── MappingService.cs        # Mapeamento entre entidades e DTOs
├── 📁 Repositories/         # Camada de acesso a dados
│   ├── IAssetRepository.cs      # Interface do repositório de ativos
│   ├── AssetRepository.cs       # Implementação do repositório de ativos
│   ├── IPortfolioRepository.cs  # Interface do repositório de portfólios
│   ├── PortfolioRepository.cs   # Implementação do repositório de portfólios
│   ├── IPositionRepository.cs   # Interface do repositório de posições
│   └── PositionRepository.cs    # Implementação do repositório de posições
├── 📁 Models/               # Entidades e DTOs
│   ├── Asset.cs                 # Entidade de ativo financeiro
│   ├── Portfolio.cs             # Entidade de portfólio
│   ├── Position.cs              # Entidade de posição em portfólio
│   ├── User.cs                  # Entidade de usuário
│   ├── Transaction.cs           # Entidade de transação
│   ├── PriceHistory.cs          # Entidade de histórico de preços
│   └── 📁 DTOs/                # Data Transfer Objects
│       ├── AssetDto.cs          # DTOs para ativos
│       ├── PortfolioDto.cs      # DTOs para portfólios
│       └── SeedDataDto.cs       # DTOs para dados de seed
├── 📁 Infrastructure/       # Configuração de banco e contexto
│   └── AppDbContext.cs          # Contexto do Entity Framework
├── Program.cs               # Configuração da aplicação
└── SeedData.json           # Dados iniciais (15 ativos, 3 portfólios)
```

## 📊 Funcionalidades Implementadas

### 🔸 **Gestão de Ativos**
- ✅ **GET** `/api/assets` - Lista todos os ativos
- ✅ **GET** `/api/assets/{id}` - Busca ativo por ID
- ✅ **GET** `/api/assets/search?symbol={symbol}` - Busca por símbolo (ex: PETR4)
- ✅ **POST** `/api/assets` - Cria novo ativo
- ✅ **PUT** `/api/assets/{id}/price` - Atualiza preço do ativo

### 🔸 **Gestão de Portfólios**
- ✅ **GET** `/api/portfolios` - Lista portfólios (com filtro opcional por usuário)
- ✅ **POST** `/api/portfolios` - Cria novo portfólio
- ✅ **GET** `/api/portfolios/{id}` - Detalhes completos do portfólio
- ✅ **POST** `/api/portfolios/{id}/positions` - Adiciona posição ao portfólio
- ✅ **PUT** `/api/portfolios/{id}/positions/{positionId}` - Atualiza posição
- ✅ **DELETE** `/api/portfolios/{id}/positions/{positionId}` - Remove posição

### 🔸 **Análises Financeiras**
- ✅ **GET** `/api/portfolios/{id}/performance` - Análise de performance
- ✅ **GET** `/api/portfolios/{id}/risk-analysis` - Análise de risco e diversificação
- ✅ **GET** `/api/portfolios/{id}/rebalancing` - Sugestões de rebalanceamento

## 📈 Dados de Demonstração

A aplicação vem pré-carregada com dados reais da Bolsa Brasileira:

### **15 Ativos Reais** 
- **PETR4** (Petrobras), **VALE3** (Vale), **ITUB4** (Itaú), **BBDC4** (Bradesco)
- **MGLU3** (Magazine Luiza), **WEGE3** (Weg), **RENT3** (Localiza)
- **B3SA3** (B3), **TOTS3** (Totvs), **VIVT3** (Vivo), **ABEV3** (Ambev)
- **SUZB3** (Suzano), **JBSS3** (JBS), **GGBR4** (Gerdau), **CCRO3** (CCR)

### **3 Portfólios Modelo**
1. **Portfólio Conservador** - Foco em dividendos e baixo risco
2. **Portfólio Crescimento** - Ações de tecnologia e varejo
3. **Portfólio Dividendos** - Empresas maduras com boa distribuição

### **Histórico de Preços**
- **30 dias** de dados históricos para os 5 principais ativos
- Dados de **setembro-outubro 2024** para cálculos de volatilidade

## 🛠️ Como Executar a Aplicação

### **Pré-requisitos**
- **.NET 8 SDK** instalado ([Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0))
- **IDE/Editor** de sua preferência (Visual Studio, VS Code, Rider)

### **Passos para Execução**

1. **Clone ou baixe o projeto**
   ```bash
   cd PortifolioFinanceiro
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Execute a aplicação**
   ```bash
   dotnet run
   ```

4. **Acesse a documentação interativa**
   - **Swagger UI**: `http://localhost:5000` ou `https://localhost:7000`
   - A aplicação abre diretamente no Swagger para facilitar os testes

### **Verificação da Execução**

Ao iniciar, você verá no console:
```
info: PortifolioFinanceiro.Services.SeedDataService[0]
      Starting database seeding...
info: PortifolioFinanceiro.Services.SeedDataService[0]
      Seeded 3 users
info: PortifolioFinanceiro.Services.SeedDataService[0]
      Seeded 15 assets
info: PortifolioFinanceiro.Services.SeedDataService[0]
      Database seeding completed successfully.
```

## 🧪 Testando a API

### **Via Swagger UI (Recomendado)**
1. Acesse `http://localhost:5000`
2. Explore os endpoints disponíveis
3. Execute testes diretamente na interface

### **Exemplos de Requisições**

**Listar todos os ativos:**
```http
GET /api/assets
```

**Buscar ativo por símbolo:**
```http
GET /api/assets/search?symbol=PETR4
```

**Detalhes de um portfólio:**
```http
GET /api/portfolios/1
```

**Análise de performance:**
```http
GET /api/portfolios/1/performance
```

### **Resposta de Exemplo - Performance**
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
      "averagePrice": 30.00,
      "currentPrice": 35.50,
      "investedValue": 15000.00,
      "currentValue": 17750.00,
      "positionReturn": 2750.00,
      "returnPercentage": 18.33
    }
  ]
}
```

## 📋 Principais Recursos Técnicos

### **Cálculos Financeiros Implementados**
- ✅ **Valor Atual do Portfólio**: Soma das posições atuais
- ✅ **Retorno Total**: Diferença entre valor atual e investido
- ✅ **Retorno Percentual**: (Valor Atual - Investido) / Investido × 100
- ✅ **Alocação Atual**: Percentual de cada ativo no portfólio
- ✅ **Desvio da Meta**: Diferença entre alocação atual e target

### **Validações e Tratamento de Erros**
- ✅ **Validação de DTOs** com Data Annotations
- ✅ **Tratamento de exceções** em todos os endpoints
- ✅ **Logs estruturados** para debugging
- ✅ **Códigos HTTP apropriados** (200, 201, 400, 404, 500)

### **Banco de Dados**
- ✅ **Relacionamentos bem definidos** entre entidades
- ✅ **Configurações de precisão** para campos monetários
- ✅ **Seed automático** na inicialização
- ✅ **Índices únicos** para símbolos de ativos

## 🔮 Possíveis Melhorias Futuras

### **Algoritmos Financeiros Avançados**
- [ ] Cálculo de **Sharpe Ratio** com taxa Selic
- [ ] **Volatilidade** baseada em histórico de preços
- [ ] **Correlação entre ativos** para diversificação
- [ ] **Value at Risk (VaR)** para análise de risco

### **Features Adicionais**
- [ ] **Autenticação e autorização** de usuários
- [ ] **Integração com APIs** de cotações em tempo real
- [ ] **Alertas automáticos** para rebalanceamento
- [ ] **Relatórios em PDF** ou Excel
- [ ] **Dashboard web** para visualização

### **Infraestrutura**
- [ ] **Banco de dados real** (SQL Server, PostgreSQL)
- [ ] **Cache Redis** para consultas frequentes
- [ ] **Testes unitários** e de integração
- [ ] **CI/CD pipeline**
- [ ] **Containerização** com Docker

## 🤝 Contribuição

Este projeto foi desenvolvido como um **desafio técnico** demonstrando:
- ✅ **Conhecimentos em .NET 8** e arquitetura de APIs
- ✅ **Implementação de padrões** Repository e Service Layer
- ✅ **Cálculos financeiros** e lógica de negócio complexa
- ✅ **Boas práticas** de desenvolvimento e documentação

## 📄 Licença

Este projeto é apenas para **fins educacionais e demonstração técnica**.

---

**🚀 Desenvolvido com .NET 8 | Portfolio Financeiro API**
