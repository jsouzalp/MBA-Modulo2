
# Feedback 2a Entrega - Avaliação Geral e Recomendações

## Front End

### Navegação

- Navegação clara, fácil e objetiva.

### Design

- Ótimo design, bom uso do Angular Material e componentes
- O dashboard ficou muito bom com o uso dos graficos

### Funcionalidade

- Funcionalidades muito bem implementadas

- No popup de orçamento geral "Porcentual limite de despesas sobre a receita lançada"
- Não ficou muito claro sobre a intenção
- Não consegui inserir valores aceitos (o botão salvar não habilita)
- Não precisa proibir o lançamento passando do limite, pois isso ocorre sempre no passado, ou seja o gasto foi realizado e precisa ser contabilizado

## Back End

### Arquitetura

- A arquitetura está limpa, bem distribuida, na medida ideal, nada a pontuar de negativo.

### Funcionalidade

- Boa implementação de validações de negócios via notificações
- Boa implementação de geração de relatórios
- Destaques e diferenciais positivos:
    - ExceptionFilter
    - Versionamento de API
    - Validação de ambiente para uso do SQLite

### Modelagem

- Modelagem ajustada ao propósito do projeto, as entidades estão anêmicas mas isto não é um problema, era esperado.
- Os serviços de negócios estão orquestrando o fluxo de forma adequada e esperada.
- Nos serviços poderia ter menos manipulação de dados, isso poderia ficar a cargo do repositório entregar, divisão negócios e dados.

## Projeto

### Organização

- Bem organizado em pastas, bom uso da pasta Data para o SQLite

### Documentação

- Bem documentado, tanto no projeto, API quanto no repositório no GH.

### Instalação

- Instalação ideal, bastou rodar os comandos especificados no repositório.

### Sugestões

- Transformar o projeto num case open source, implementar mais funcionalidades, deixar rodando em um server para acesso publico, futuramente implementar um meio de pagamento para transformar em um SaaS (não precisa ter objetivos financeiros, apenas case mesmo).

