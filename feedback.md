
# Feedback 1a Entrega - Avaliação Geral e Recomendações

## Front End

### Navegação

- Navegação clara, fácil e objetiva.

### Design

- Ótimo design, bom uso do Angular Material e componentes
- O dashboard ficou muito bom com o uso dos graficos

### Funcionalidade

- Não consegui utilizar o sistema com meu usuário, provavelmente está faltando associar alguma claim ou role na criação, ao utilizar um usuário previamente registrado consegui navegar.
- O relatório poderia fazer um total de entradas e saídas e demonstrar o resultado do período (positivo ou negativo)
- A mensagem das notificações poderia ser um pouco mais detalhada (ex: ao atingir 80% do budget)
- As demais funcionalidades estão atendendo ao proprósito do escopo

## Back End

### Arquitetura

- A arquitetura está limpa, bem distribuida, na medida ideal
- A camada “Repositories” faria mais sentido se fosse “Data”

### Funcionalidade

- Boa implementação de validações de negócios via notificações
- Boa implementação de geração de relatórios
- A distribuição das extensões agrupadoras de configuração da program.cs ficou um pouco confusa para mim, mas não que esteja errada, mas dá para reorganizar de forma mais coerente.
- Nomenclatura de tabelas e colunas segue um padrão antiguado “TB_” assim como as PKs ex: “BUDGET_ID”, não tem necessidade de CAPS e também de repetir o nome da entidade na PK.

### Modelagem

- Modelagem ajustada ao propósito do projeto, as entidades estão anêmicas mas isto não é um problema, era esperado.
- Os serviços de negócios estão orquestrando o fluxo de forma adequada e esperada.

## Projeto

### Organização

- Bem organizado em pastas, bom uso da pasta Data para o SQLite

### Documentação

- Bem documentado, tanto no projeto, API quanto no repositório no GH.

### Instalação

- Instalação ideal, bastou rodar os comandos especificados no repositório.

