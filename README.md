
# ⚙️ Mottu - Projeto .NET (Easy Finder)

### 👥 Integrantes do Projeto

- **Gustavo de Aguiar Lima Silva** - RM: 557707  
- **Julio Cesar Conceição Rodrigues** - RM: 557298  
- **Matheus de Freitas Silva** - RM: 552602  

---

### 💡 Descrição da Solução

Este projeto em **.NET 9** utiliza **Minimal APIs**, **Entity Framework Core** (Oracle) e interface gráfica via **Scalar UI** para um sistema de controle de pátios de motos. Permite gerenciamento completo de galpões, andares, pátios, blocos, vagas e motos, incluindo operações de CRUD, consultas paginadas/filtradas, autenticação JWT e predição automatizada de manutenção via ML.

---

### 🛠️ Como Executar o Projeto Localmente

#### ✅ Pré-requisitos

Certifique-se de ter instalado:

- [.NET SDK 9.0+](https://dotnet.microsoft.com/en-us/download)
- Acesso a um banco Oracle (ex: `oracle.fiap.com.br`)
- Uma IDE como **Rider**, **Visual Studio 2022+** ou **VS Code** com extensões C#

---

#### 🚀 Executando o Projeto

1. Clone ou extraia o repositório:

```bash
git clone https://github.com/Challenge-MottuGJM/dotnet.git
cd .Net
```

2. Configure a connection string em `appsettings.json`:

```json
"ConnectionStrings": {
  "FiapOracleDb": "Data Source=//oracle.fiap.com.br:1521/orcl;User Id=seu_usuario;Password=sua_senha;"
}
```

3. Execute o projeto:

```bash
dotnet run --project EasyFinder.csproj
```

### 🔒 Autenticação e Acesso Protegido

1. Faça login para obter o **JWT**:

   - Use Scalar UI, Postman, ou curl:
   - Endpoint: `POST /login`
   - Exemplo de corpo:

     ```
     { "usuario": "admin", "senha": "senha123" }
     ```

   - O login retorna um campo `"token"`. 
   - **Copie o token retornado**.

2. Para acessar endpoints protegidos:

   - Vá até Scalar UI (web)
   - Clique no ícone de cadeado (🔒) ou campo "Security" (Bearer)
   - **Cole o token JWT** no prompt e autorize.
   - Todos endpoints `/api/v1/...` exigem o header:

     ```
     Authorization: Bearer <token>
     ```

   - Sem token, o acesso retorna erro **401 Unauthorized**.

---

### 📦 Tecnologias Utilizadas

- .NET 9
- Entity Framework Core + Oracle
- Minimal APIs
- Scalar.AspNetCore (interface gráfica)
- OpenAPI
- JWT para autenticação
- ML.NET para predição de probabilidade
- Helpcheck para análise de saúde da API
- C#

---

### 📬 Como Usar a API Localmente

Você pode interagir com os endpoints da API usando **Scalar UI**, **Postman**, **curl** ou navegando pelas URLs diretamente.

---

## 📋 Tabela de Endpoints da API

| Entidade  | Método HTTP | Rota                                | Descrição                                     |
|-----------|-------------|-------------------------------------|-----------------------------------------------|
| Login     | POST        | /api/v1/login                              | Retorna o token JWT para acesso        |
| Galpões   | GET         | /api/v1/galpoes                            | Retorna todos os galpões               |
| Galpões   | GET         | /api/v1/galpoes/{id}                       | Retorna um galpão por ID               |
| Galpões   | POST        | /api/v1/galpoes/inserir                    | Insere um novo galpão                  |
| Galpões   | PUT         | /api/v1/galpoes/atualizar/{id}             | Atualiza um galpão                     |
| Galpões   | DELETE      | /api/v1/galpoes/deletar/{id}               | Remove um galpão pelo ID               |
| Andares   | GET         | /api/v1/andares                            | Retorna todos os andares               |
| Andares   | GET         | /api/v1/andares/{id}                       | Retorna um andar por ID                |
| Andares   | POST        | /api/v1/andares/inserir                    | Insere um novo andar                   |
| Andares   | PUT         | /api/v1/andares/atualizar/{id}             | Atualiza um andar                      |
| Andares   | DELETE      | /api/v1/andares/deletar/{id}               | Remove um andar pelo ID                |
| Patios    | GET         | /api/v1/patios                             | Retorna todos os pátios                |
| Patios    | GET         | /api/v1/patios/{id}                        | Retorna um pátio por ID                |
| Patios    | POST        | /api/v1/patios/inserir                     | Insere um novo pátio                   |
| Patios    | PUT         | /api/v1/patios/atualizar/{id}              | Atualiza um pátio                      |
| Patios    | DELETE      | /api/v1/patios/deletar/{id}                | Remove um pátio pelo ID                |
| Blocos    | GET         | /api/v1/blocos                             | Retorna todos os blocos                |
| Blocos    | GET         | /api/v1/blocos/{id}                        | Retorna um bloco por ID                |
| Blocos    | POST        | /api/v1/blocos/inserir                     | Insere um novo bloco                   |
| Blocos    | PUT         | /api/v1/blocos/atualizar/{id}              | Atualiza um bloco                      |
| Blocos    | DELETE      | /api/v1/blocos/deletar/{id}                | Remove um bloco pelo ID                |
| Vagas     | GET         | /api/v1/vagas                              | Retorna todas as vagas                 |
| Vagas     | GET         | /api/v1/vagas/{id}                         | Retorna uma vaga por ID                |
| Vagas     | POST        | /api/v1/vagas/inserir                      | Insere uma nova vaga                   |
| Vagas     | PUT         | /api/v1/vagas/atualizar/{id}               | Atualiza uma vaga                      |
| Vagas     | DELETE      | /api/v1/vagas/deletar/{id}                 | Remove uma vaga pelo ID                |
| Motos     | GET         | /api/v1/motos                              | Retorna todas as motos                 |
| Motos     | GET         | /api/v1/motos/{id}                         | Retorna uma moto por ID                |
| Motos     | GET         | /api/v1/motos/status/{status}              | Filtra motos por status                |
| Motos     | GET         | /api/v1/motos/modelo/{modelo}              | Filtra motos por modelo                |
| Motos     | GET         | /api/v1/motos/placa/{placa}                | Filtra motos por placa                 |
| Motos     | GET         | /api/v1/motos/paginadas                    | Retorna motos paginadas (sem filtro)   |
| Motos     | GET         | /api/v1/motos/search                       | Retorna motos paginadas por modelo     |
| Motos     | POST        | /api/v1/motos/inserir                      | Insere uma nova moto                   |
| Motos     | PUT         | /api/v1/motos/atualizar/placa/{placa}      | Atualiza uma moto por placa            |
| Motos     | PUT         | /api/v1/motos/atualizar/chassi/{chassi}    | Atualiza uma moto por chassi           |
| Motos     | PUT         | /api/v1/motos/atualizar/{id}               | Atualiza uma moto por id               |
| Motos     | DELETE      | /api/v1/motos/deletar/{id}                 | Remove uma moto pelo ID                |
| ML Manutenção   | POST        | /api/v1/ml/motos/{chassi}/prob-manutencao  | Faz a análise de probabilidade de manutenção da moto         |

---

### 🧪 Executando os Testes

1. Pré-requisitos:
   - API configurada conforme instruções acima.
   - Banco disponível e pré-populado conforme necessidade dos testes.
   - Projeto EasyFinder.Tests referenciando a API.

2. No terminal:
   
dotnet test
