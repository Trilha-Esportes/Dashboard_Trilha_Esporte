# 📊 Dashboard Trilha Esportes

Este projeto é um dashboard interativo desenvolvido em C# com Blazor (usando o framework MudBlazor). Ele permite a visualização de dados de vendas da Trilha Esportes e empresas parceiras, incluindo gráficos, tabelas e exportações em Excel.

---

## ⚙️ Configuração de Conexão ao Banco de Dados

1. Na raiz do projeto, crie um arquivo `.env`.

2. Adicione suas credenciais de acesso ao banco PostgreSQL:

```env
DB_USER=seu_usuario
DB_PASSWORD=sua_senha
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nome_do_banco
```

> ℹ️ O projeto utiliza a biblioteca `DotNetEnv` para carregar essas variáveis de ambiente.

---

## 🧰 Tecnologias e Dependências

O projeto foi construído em **.NET C#** com o framework **MudBlazor**. As principais dependências são:

* `Blazored.LocalStorage` – Armazenamento local no navegador
* `ClosedXML` – Geração e exportação de arquivos Excel
* `DotNetEnv` – Carregamento de variáveis de ambiente
* `MudBlazor` – Componentes de UI modernos para Blazor
* `Npgsql` – Conector PostgreSQL para .NET

---

## ▶️ Como Executar o Projeto

1. Instale as dependências com o comando:

```bash
dotnet restore
```

2. Compile o projeto:

```bash
dotnet build
```

3. Execute com hot-reload (modo desenvolvimento):

```bash
dotnet watch run
```

> Recomendado usar o SDK .NET 7 ou superior.

---

## 🗂️ Estrutura do Projeto

```
├── Data/
│   ├── Entities/        # Representações das tabelas do banco
│   └── Repository/      # Acesso e manipulação de dados
│
├── Domain/
│   ├── DTOs/            # Objetos de transferência para a UI
│   └── Services/        # Lógica de negócio, filtragem, gráficos, exportação
│
├── Application/         # Camada de orquestração da aplicação
│
├── Enum/                # Definições de enumeradores usados no sistema
│
└── Components/          # Componentes visuais e páginas Blazor
```

---

## 🖥️ Telas do Sistema

* Dados dos Pedidos (skuMarketplace)
* Dados do Anymarkets 
* Resumo Financeiro
* Pedidos com Devolução
* Scraping de Produtos

---

## 👨‍💼 Autor

Guilbert Marques
