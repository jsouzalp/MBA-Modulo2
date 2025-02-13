# 🎓 **Plataforma de Controle Financeiro Pessoal - Aplicação com SPA e API RESTful**



## **1. Apresentação** 

Bem-vindo ao repositório do projeto **FinPlanner360**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **MÓDULO 2 - Desenvolvimento Full-Stack Avançado com ASP.NET Core**.
O objetivo deste projeto é oferecer uma solução de controle financeiro de receitas e despesas para um melhor controle da vida financeira do usuário.
Solução foi desenvolvida em Angular 18 e .Net 8 (api RESTful).

### **Autor(es)**
- **Hugo Domynique Ribeiro Nunes**
- **Jairo Azevedo de Souza**
- **Jason Santos do Amaral**
- **Marco Aurelio Roque Pinto**
- **Pedro Otávio Gutierres**

## **2. Proposta do Projeto**

O projeto consiste em:

- **FrontEnd Angular:** Interface web para interação do usuário.
- **API RESTful:** Exposição dos recursos do controle financeiro para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação e Autorização:** Implementação de controle de acesso, diferenciando administradores e usuários comuns.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - Angular
  - ASP.NET Web API
  - Entity Framework Core
- **Banco de Dados:** 
  - SQLite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Angular 18
  - HTML/CSS para estilização básica
- **Documentação da API:** 
  - Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

```
  src/
    ├── API/
        ├── FinPlanner360.Api/       - API RESTfull
        ├── FinPlanner360.Business/  - Models, Services, Extensions
        ├── Blog.Repositories/       - Repositories, Migrations, config EF Core
    ├── FRONT/
        ├── node_modules/   - bibliotecas do projeto
        ├── src/                     
            ├── app/          - pasta principal, contém código da aplicação 
            ├── assets/       - Armazena arquivos extras, como imagens
            ├── environments/ - Contém arquivos relacionados ao ambiente
  README.md               - Arquivo de Documentação do Projeto
  FEEDBACK.md             - Arquivo para Consolidação dos Feedbacks
  .gitignore              - Arquivo de Ignoração do Git
```

## **5. Funcionalidades**

- **CRUD para Categorias e Transações:** Permite criar, editar, visualizar e excluir categorias e transações.
- **Autenticação e Autorização:** Diferenciação entre usuários comuns e administradores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- Angular (instalar o Node.js e o Angular CLI)
- SQLite
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   
   ```bash
   git clone -b develop https://github.com/jsouzalp/MBA-Modulo2.git
   cd MBA-Modulo2
   ```
   
2. **Configuração do Banco de Dados:**
   
   - No arquivo `appsettings.json`, configure a string de conexão do SQLite.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a API:**
   
   ```bash
   cd src/FinPlanner360.Api/
   dotnet run
   ```
   
   - Acesse a documentação da API em: http://localhost:5001/swagger
   
4. **Executar a Aplicação Angular:**
   
   ```bash
   cd src/FRONT/
   
   npm install
   
   ng serve
   ```
   
   - Acesse a aplicação em: http://localhost:4200
   - Para acessar com os usuários previamente cadastrados, utilizar a senha: Password@2024


## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.

