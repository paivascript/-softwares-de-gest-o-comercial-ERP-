# Mercado SeuZe

Aplicação WinForms (.NET 10, C#) para gestão de clientes de um mercado, com banco MySQL.

## Stack
- **.NET 10** (Windows Forms)
- **C#** com `Nullable enable`
- **MySQL** (`MySql.Data` 9.7.0)
- Arquitetura em camadas: `Modelo` → `DAL` → `BLL` → `PL`

## Estrutura
```
MercadoSeuZe/
├── Program.cs              # Entry point
├── App.config
├── MercadoSeuZe.csproj
├── seuzeSql.sql            # Script de criação do banco
├── Modelo/                 # Entidades (Cliente, Usuario)
├── DAL/                    # Acesso a dados (Conexao, ClienteDao, UsuarioDao)
├── BLL/                    # Regras de negócio (ClienteBLL)
└── PL/                     # Forms (Login, Principal, Cliente, PesquisaCliente)
```

## Pré-requisitos
- **.NET SDK 10** (https://dotnet.microsoft.com/download)
- **MySQL Server** instalado e rodando em `localhost`
- Usuário `root` com senha `1234` (ou ajuste em `DAL/conection.cs`)

## Como rodar

### 1. Criar o banco de dados
Abra o MySQL Workbench (ou `mysql` no terminal) e execute o script:
```bash
mysql -u root -p < seuzeSql.sql
```
Isso cria o banco `mercadoseuze`, todas as tabelas e o usuário admin padrão.

**Login padrão**: `admin` / senha `1234`

### 2. Restaurar dependências e compilar
No diretório do projeto (`MercadoSeuZe/`):
```bash
dotnet restore
dotnet build
```

### 3. Executar
```bash
dotnet run
```

A janela de login abrirá. Use `admin` / `1234` para entrar.

## Funcionalidades
- **Login** validado contra a tabela `usuario` (sessão de usuário ativa pra registrar vendas)
- **Cadastro / Listagem / Exclusão de Clientes** com busca em tempo real
- **Produtos**: cadastro, edição, exclusão e listagem com pesquisa
- **Vendas**: tela de PDV com seleção de cliente, carrinho de produtos, cálculo automático do total, baixa de estoque automática e registro transacional (insere venda + itens + atualiza estoque numa única transação com rollback em caso de falha)

## Configuração do banco
String de conexão em `DAL/conection.cs`:
```
server=localhost;database=mercadoseuze;uid=root;pwd=1234
```
Ajuste conforme seu ambiente.
# mercadoseuZe
# mercadoseuZe
