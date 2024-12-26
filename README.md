# **Plataforma de Controle Financeiro Pessoal - Aplicação de Controle Financeiro com SPA e API RESTful**

## **1. Apresentação**
!!!!!!!!!!!!!!!!!!! Revisar

Seja bem-vindo ao repositório do projeto **Controle Financeiro Pessoal** chamado **FinPlanner360**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **MÓDULO 2 - Desenvolvimento Full-Stack Avançado com ASP.NET Core**.
O objetivo deste projeto é oferecer uma solução de controle financeiro de receitas e despesas para um melhor controle da vida financeira do usuário
Inicialmente esta solução está desenvolvida em Angular 19 (front-end) e .Net 8 (api RESTful)

### **Autores**
- **André Cesconetto**
- **Hugo Domynique Ribeiro Nunes**
- **Jairo Azevedo de Souza**
- **Jason Santos do Amaral**
- **Marco Aurelio Roque Pinto**
- **Pedro Otávio Gutierres**

## **2. Proposta do Projeto**
!!!!!!!!!!!!!!!!!!! Revisar de acordo com a evolução
O projeto consiste em:

- **Projeto.Camada:** Descrição

## **3. Tecnologias Utilizadas**
!!!!!!!!!!!!!!!!!!! Revisar
- **Linguagem de Programação:** 
  - C# (.Net 8)
- **Frameworks:**
  - Angular
  - ASP.NET Core Web API
  - Entity Framework Core
  - AutoMapper
  - FluentValidations
- **Banco de Dados:** 
  - SQLite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Angular
  - HTML/CSS para estilização básica
- **Documentação da API:** 
  - Swagger (Apenas em ambiente de desenvolvimento)

## **4. Estrutura do Projeto**
!!!!!!!!!!!!!!!!!!! Revisar Estrutura de acordo com a evolução

A estrutura do projeto é organizada da seguinte forma:

- data/
- docs/
- scripts/
- src/
  - Pasta.Projeto/ 
- readme.md - Arquivo de Documentação do Projeto
- feedback.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de "ignores" do Git

## **5. Funcionalidades Implementadas**
!!!!!!!!!!!!!!!!!!! Revisar

- **CRUD para Entrada de Informações:** Permite criar, editar, visualizar e excluir informações de usuário, categorias de lançamentos e lançamentos propriamente dito
- **API RESTful:** Exposição de endpoints para operações via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**
!!!!!!!!!!!!!!!!!!! Revisar

Para criação da estrutura de dados, é necessário localizar o arquivo *DatabaseSettings.json* e configurar nele a string de conexão. Após configurado, deve ser executado o migrations para criação da estrutura de tabelas:
Como esta aplicação está usando o SQLite, um database será criado automaticamente quando em ambiente de desenvolvimento e executada a aplicação (API).
Deverá existir uma pasta ./data e caso não exista, a mesma será criada automaticamente

## **Extensions do Visual Studio**
- Para visualização dos dados na base de dados criada (verifique a pasta ".\data") pelo Visual Studio, é necessária a instalação da extension "SQLite/SQL Server Compact Toolbox"
- Para um "deep-clean" dos arquivos temporários da solução, recomendo a instalação da extension "Open Command Line". Esta extension facilitará o uso do pacote ClearBinObj.cmd (veja em .helpers)

### **Pré-requisitos**
!!!!!!!!!!!!!!!!!!! Revisar Pré-Reqs

- .NET SDK 8.0 ou superior
- Angular 
- SQLite
- Visual Studio 2022
- Git

### **Passos para Execução**
!!!!!!!!!!!!!!!!!!! Revisar o passo a passo
1. **Clone o Repositório:**
   - `git clone https://github.com/jsouzalp/MBA-Modulo2.git`

2. **Configuração do Banco de Dados:**
   - No arquivo `databaseSettings.json`, configure a string de conexão do SQLite para o database *ConnectionStringApplication*.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos na pasta .\data
   - Instalar o dotnet ef (caso não tenha feito ainda):
     - dotnet tool install --global dotnet-ef
   - Para executar o migration:
     - dotnet ef migrations add InitialMigration --project .\FinPlanner360.Repositories --startup-project .\FinPlanner360.Api --context FinPlanner360DbContext
   - Para remover o migration:
     - dotnet ef migrations remove --project .\FinPlanner360.Repositories --startup-project .\FinPlanner360.Api --context FinPlanner360DbContext
   - Para ver o script que será gerado:
     - dotnet ef migrations script --no-build
   - Para sincronizar com o BD:
     - dotnet ef database update --project .\FinPlanner360.Api --context FinPlanner360DbContext

2. **Configuração do Banco de Dados de Autenticação (Identity):**
   - No arquivo `DatabaseSettings.json`, configure a string de conexão do SQLite para o database *ConnectionStringIdentity*.
   - Instalar o dotnet ef (caso não tenha feito ainda):
     - dotnet tool install --global dotnet-ef
   - Navegar para a pasta do projeto .\FinPlanner360.Api
   - Para executar o migration do *Identity*:
     - dotnet ef migrations add InitialMigration --project .\FinPlanner360.Repositories --startup-project .\FinPlanner360.Api --context ApplicationDbContext
   - Para remover o migration:
     - dotnet ef migrations remove --project .\FinPlanner360.Repositories --startup-project .\FinPlanner360.Api --context ApplicationDbContext
   - Para ver o script que será gerado:
     - dotnet ef migrations script --no-build
   - Para sincronizar com o BD:
     - dotnet ef database update --project .\FinPlanner360.Api --context ApplicationDbContext


3. **Executar a Aplicação MVC:**
   - `cd src/Blog.Mvc/`
   - `dotnet run`
   - Acesse a aplicação em: http://localhost:5000

4. **Executar a API:**
   - `cd src/Blog.Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5001/swagger

5. **Primeira Execução**
   Quando se tratar de uma primeira execução em ambiente de desenvolvimento, serão executados os Migrations e em seguida serão criados os usuários "jsouza.lp@gmail.com" e "cath.lp@gmail.com" com a password inicial "123" de forma automática já com algumas postagens e comentários para uma melhor experiência com a utilização da solução.
   
## **7. Instruções de Configuração**
- **JWT para API:** As chaves de configuração do JWT estão no `jwtSettings.json`.

## **8. Documentação da API**
A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em: http://localhost:5001/swagger

## **9. Testes da API**
!!!!!!!!!!!!!!!!!!! Revisar

Na pasta .\docs existe o arquivo "?????????.json" que pode ser importado para o Postman e executados os endpoints de acordo com a sua necessidade.
Para os endpoints que necessitam de autorização, recomendo que seja feito primeiro um login ou novo registro (pasta Authentication).

## **10. Avaliação**
- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.