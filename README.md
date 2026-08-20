# Evolucional - Testes Práticos

Repositório contendo as soluções dos testes práticos de desenvolvimento Back-end e Front-end realizados para o processo seletivo da Evolucional.

## Projetos

### Back-end

API REST para gerenciamento de alunos, turmas e matrículas, desenvolvida em ASP.NET Web API com .NET Framework 4.8, SQL Server e Dapper.

O projeto inclui, entre outros recursos:

- CRUD de alunos e turmas
- Matrícula e desmatrícula de alunos
- Regras de negócio para matrículas
- Relatórios
- Paginação e filtros
- Testes
- Cache de consultas

A documentação completa do projeto está disponível em [`backend/README.md`](backend/README.md).

### Front-end

Painel de gerenciamento de produtos desenvolvido com React e TypeScript.

O projeto contempla:

- Listagem de produtos
- Busca por nome
- Filtro por categoria
- Paginação via API
- Visualização de detalhes
- Cadastro e edição
- Validação de formulário
- Exclusão com confirmação
- Tratamento dos estados de carregamento, erro e resultados vazios

A documentação específica do Front-end será mantida em [`frontend/README.md`](frontend/README.md).

## Estrutura

```text
evolucional-crm/
├── backend/       # Teste Back-end
├── frontend/      # Teste Front-end
├── database/      # Scripts e arquivos de banco de dados
├── postman/       # Coleções para testes da API
└── README.md      # Visão geral do repositório
