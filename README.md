# Evolucional - Teste Prático de Desenvolvimento

## Backend - API de Controle de Matrículas

API REST para gerenciamento de alunos, turmas e matrículas escolares,
desenvolvida para o teste prático da Evolucional.

### Tecnologias

-   .NET Framework 4.8
-   ASP.NET Web API
-   SQL Server
-   Dapper
-   SQL escrito manualmente
-   MSTest
-   Moq

## Arquitetura

O backend foi organizado em camadas com responsabilidades separadas:

``` text
Controller
    ↓
Service
    ↓
Repository
    ↓
Dapper / SQL Server
```

-   **Controllers:** contrato HTTP, requisições e status HTTP.
-   **Services:** regras de negócio e coordenação das operações.
-   **Repositories:** acesso ao SQL Server com Dapper e SQL manual.
-   **DTOs:** contratos de entrada e saída da API.

## Estrutura do backend

``` text
backend/
└── Evolucional.Matriculas/
    ├── Evolucional.Matriculas.Api/
    │   ├── Controllers/
    │   ├── DTOs/
    │   ├── Exceptions/
    │   ├── Infrastructure/
    │   ├── Models/
    │   ├── Repositories/
    │   │   └── Interfaces/
    │   └── Services/
    ├── Evolucional.Matriculas.Tests/
    └── Evolucional.Matriculas.sln
```

## Banco de dados

O projeto utiliza SQL Server. O arquivo `script-banco.sql` contém a
criação das tabelas e os dados iniciais.

Tabelas:

-   `Aluno`
-   `Turma`
-   `Matricula`

Execute o script em uma instância do SQL Server antes de iniciar a API.

## Configuração da conexão

Configure a connection string no `Web.config` de acordo com sua
instância do SQL Server.

``` xml
	<connectionStrings>
		<add name="TesteEscola" 
			 connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TesteEscola;Integrated Security=True"
			 providerName="System.Data.SqlClient" />
	</connectionStrings>
```

## Como executar

### Pré-requisitos

-   Visual Studio com suporte ao .NET Framework 4.8
-   .NET Framework 4.8
-   SQL Server
-   SQL Server Management Studio ou equivalente

### Passos

1.  Clone o repositório.
2.  Execute o `script-banco.sql` no SQL Server.
3.  Configure a connection string no `Web.config`.
4.  Abra `Evolucional.Matriculas.sln`.
5.  Restaure os pacotes NuGet.
6.  Defina `Evolucional.Matriculas.Api` como projeto de inicialização.
7.  Compile a solution.
8.  Execute pelo Visual Studio/IIS Express.

## Endpoints

### Alunos

``` http
GET /api/alunos
GET /api/alunos/{id}
POST /api/alunos
PUT /api/alunos/{id}
DELETE /api/alunos/{id}
```

A listagem suporta paginação, filtro opcional por nome e informa o total
de registros.

Exemplo:

``` http
GET /api/alunos?page=1&pageSize=10&nome=Felipe
```

O `DELETE` realiza exclusão lógica através do campo `Ativo`.

### Turmas

``` http
GET /api/turmas
```

Retorna as turmas com a quantidade atual de vagas disponíveis.

### Matrículas

``` http
POST /api/matriculas
```

Payload:

``` json
{
  "AlunoId": 1,
  "TurmaId": 1
}
```

Regras:

-   aluno deve existir;
-   turma deve existir;
-   aluno deve estar ativo;
-   turma deve possuir vaga;
-   aluno não pode estar matriculado duas vezes na mesma turma.

A criação da matrícula e o decremento de `VagasDisponiveis` são
executados na mesma transação.

O decremento também protege a última vaga no momento da escrita:

``` sql
UPDATE Turma
SET VagasDisponiveis = VagasDisponiveis - 1
WHERE Id = @TurmaId
  AND VagasDisponiveis > 0;
```

Se nenhuma linha for afetada, a matrícula não é concluída.

### Relatório

``` http
GET /api/relatorios/alunos-por-turma
```

Retorna por turma:

-   nome da turma;
-   quantidade de alunos matriculados;
-   vagas restantes.

A consulta é realizada diretamente no SQL com `JOIN` e `GROUP BY`. Foi
utilizado `LEFT JOIN` para incluir turmas sem matrículas.

## Status HTTP

  Status   Utilização
  -------- -----------------------------------------------
  200      Consulta ou atualização realizada com sucesso
  201      Registro criado com sucesso
  400      Requisição ou dados inválidos
  404      Registro não encontrado
  409      Operação impedida por regra de negócio

## Testes unitários

Os testes das regras de matrícula utilizam:

-   MSTest
-   Moq

Os repositories são substituídos por mocks, permitindo testar o
`MatriculaService` isoladamente e sem SQL Server.

Cenários cobertos:

-   aluno inativo;
-   turma sem vagas;
-   matrícula duplicada;
-   matrícula válida;
-   vaga consumida entre a validação e a efetivação da matrícula.

### Executar os testes

No Visual Studio:

``` text
Teste → Gerenciador de Testes → Executar Todos
```

## Decisões técnicas

### Dapper e SQL manual

O acesso a dados utiliza Dapper e SQL escrito manualmente, conforme
solicitado.

### Exclusão lógica

A exclusão de aluno altera o campo `Ativo`, preservando o registro no
banco.

### DTOs

DTOs específicos são utilizados nos contratos HTTP para evitar
acoplamento direto com os modelos persistidos.

### Transação de matrícula

O decremento da vaga e o `INSERT` da matrícula pertencem à mesma
transação. Em caso de falha, é realizado rollback.

### Concorrência de vagas

Além da validação no Service, o `UPDATE` exige `VagasDisponiveis > 0`.
Isso impede que duas requisições concorrentes levem a quantidade de
vagas para um valor negativo.

## Itens bônus

Implementado:

-   testes unitários das regras de matrícula com MSTest e Moq.

## Autor

Ricardo Vieira
