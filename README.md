
# ⚙️ Mottu - Projeto .NET (Easy Finder)

### 👥 Integrantes do Projeto

- **Gustavo de Aguiar Lima Silva** - RM: 557707  
- **Julio Cesar Conceição Rodrigues** - RM: 557298  
- **Matheus de Freitas Silva** - RM: 552602  

---

### 💡 Descrição da Solução

Este projeto em **.NET 9** utiliza a abordagem de **Minimal APIs** com integração ao **Entity Framework Core** e banco de dados **Oracle**, além de interface gráfica via **Scalar UI**.

A aplicação simula um sistema para controle de pátios de motos, permitindo o gerenciamento de galpões, andares, pátios, blocos, vagas e motos — com operações de CRUD e consultas paginadas e filtradas.

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
cd Mottu
```

2. Configure a connection string em `appsettings.json`:

```json
"ConnectionStrings": {
  "FiapOracleDb": "Data Source=//oracle.fiap.com.br:1521/orcl;User Id=seu_usuario;Password=sua_senha;"
}
```

3. Execute o projeto:

```bash
dotnet run --project Mottu.csproj
```

4. Acesse a API e a interface web:

- API: `http://localhost:5148`
- Scalar UI: `http://localhost:5148/scalar`

---

### 📦 Tecnologias Utilizadas

- .NET 9
- Entity Framework Core + Oracle
- Minimal APIs
- Scalar.AspNetCore (interface gráfica)
- OpenAPI
- C#

---

### 📬 Como Usar a API Localmente

Você pode interagir com os endpoints da API usando **Scalar UI**, **Postman**, **curl** ou navegando pelas URLs diretamente.

---

## 📋 Tabela de Endpoints da API

| Entidade  | Método HTTP | Rota                                | Descrição                              |
|-----------|-------------|-------------------------------------|----------------------------------------|
| Galpões   | GET         | /galpoes                            | Retorna todos os galpões               |
| Galpões   | GET         | /galpoes/{id}                       | Retorna um galpão por ID               |
| Galpões   | POST        | /galpoes/inserir                    | Insere um novo galpão                  |
| Galpões   | PUT         | /galpoes/atualizar/{id}             | Atualiza um galpão                     |
| Galpões   | DELETE      | /galpoes/deletar/{id}               | Remove um galpão pelo ID               |
| Andares   | GET         | /andares                            | Retorna todos os andares               |
| Andares   | GET         | /andares/{id}                       | Retorna um andar por ID                |
| Andares   | POST        | /andares/inserir                    | Insere um novo andar                   |
| Andares   | PUT         | /andares/atualizar/{id}             | Atualiza um andar                      |
| Andares   | DELETE      | /andares/deletar/{id}               | Remove um andar pelo ID                |
| Patios    | GET         | /patios                             | Retorna todos os pátios                |
| Patios    | GET         | /patios/{id}                        | Retorna um pátio por ID                |
| Patios    | POST        | /patios/inserir                     | Insere um novo pátio                   |
| Patios    | PUT         | /patios/atualizar/{id}              | Atualiza um pátio                      |
| Patios    | DELETE      | /patios/deletar/{id}                | Remove um pátio pelo ID                |
| Blocos    | GET         | /blocos                             | Retorna todos os blocos                |
| Blocos    | GET         | /blocos/{id}                        | Retorna um bloco por ID                |
| Blocos    | POST        | /blocos/inserir                     | Insere um novo bloco                   |
| Blocos    | PUT         | /blocos/atualizar/{id}              | Atualiza um bloco                      |
| Blocos    | DELETE      | /blocos/deletar/{id}                | Remove um bloco pelo ID                |
| Vagas     | GET         | /vagas                              | Retorna todas as vagas                 |
| Vagas     | GET         | /vagas/{id}                         | Retorna uma vaga por ID                |
| Vagas     | POST        | /vagas/inserir                      | Insere uma nova vaga                   |
| Vagas     | PUT         | /vagas/atualizar/{id}               | Atualiza uma vaga                      |
| Vagas     | DELETE      | /vagas/deletar/{id}                 | Remove uma vaga pelo ID                |
| Motos     | GET         | /motos                              | Retorna todas as motos                 |
| Motos     | GET         | /motos/{id}                         | Retorna uma moto por ID                |
| Motos     | GET         | /motos/status/{status}              | Filtra motos por status                |
| Motos     | GET         | /motos/modelo/{modelo}              | Filtra motos por modelo                |
| Motos     | GET         | /motos/placa/{placa}                | Filtra motos por placa                 |
| Motos     | GET         | /motos/paginadas                    | Retorna motos paginadas (sem filtro)   |
| Motos     | GET         | /motos/search                       | Retorna motos paginadas por modelo     |
| Motos     | POST        | /motos/inserir                      | Insere uma nova moto                   |
| Motos     | PUT         | /motos/atualizar/{id}               | Atualiza uma moto                      |
| Motos     | DELETE      | /motos/deletar/{id}                 | Remove uma moto pelo ID                |

---
