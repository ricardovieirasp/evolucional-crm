using Evolucional.Matriculas.Api.DTOs.Matriculas;
using Evolucional.Matriculas.Api.Exceptions;
using Evolucional.Matriculas.Api.Models;
using Evolucional.Matriculas.Api.Repositories.Interfaces;
using Evolucional.Matriculas.Api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Evolucional.Matriculas.Tests
{
    [TestClass]
    public class MatriculaServiceTests
    {
        [TestMethod]
        public async Task CreateAsync_AlunoInativo_DeveLancarAlunoInativoException()
        {
            // Arrange

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();

            alunoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Aluno
                {
                    Id = 1,
                    Nome = "Aluno Teste",
                    Ativo = false
                });

            turmaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Turma
                {
                    Id = 1,
                    Nome = "Turma Teste",
                    VagasDisponiveis = 10
                });

            var service = new MatriculaService(
                matriculaRepositoryMock.Object, 
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object);

            var dto = new CriarMatriculaDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<AlunoInativoException>(
                () => service.CreateAsync(dto));

        }

        [TestMethod]
        public async Task CreateAsync_TurmaSemVaga_DeveLancarTurmaSemVagaException()
        {
            // Arrange
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();

            alunoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Aluno
                {
                    Id = 1,
                    Nome = "Aluno Ativo",
                    Ativo = true
                });

            turmaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Turma
                {
                    Id = 1,
                    Nome = "Turma de Teste Lotada",
                    VagasDisponiveis = 0
                });

            var service = new MatriculaService(
                matriculaRepositoryMock.Object,
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object);

            var dto = new CriarMatriculaDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<TurmaSemVagaException>(
                () => service.CreateAsync(dto));
        }

        [TestMethod]
        public async Task CreateAsync_MatriculaDuplicada_DeveLancarMatriculaDuplicadaException()
        {
            // Arrange
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();

            alunoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Aluno
                {
                    Id = 1,
                    Nome = "Aluno Ativo",
                    Ativo = true
                });

            turmaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(
                new Turma
                {
                    Id = 1,
                    Nome = "Turma de Teste",
                    VagasDisponiveis = 10
                });

            matriculaRepositoryMock
                .Setup(repository => repository.ExistsAsync(1, 1))
                .ReturnsAsync(true);

            var service = new MatriculaService(
                matriculaRepositoryMock.Object,
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object);

            var dto = new CriarMatriculaDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<MatriculaDuplicadaException>(
                () => service.CreateAsync(dto));

            // Verify

            matriculaRepositoryMock.Verify(
                repository => repository.ExistsAsync(1, 1),
                Times.Once);

            matriculaRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Never);

        }

        [TestMethod]
        public async Task CreateAsync_DadosValidos_DeveCriarMatricula()
        {
            // Arrange
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();

            alunoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Aluno
                {
                    Id = 1,
                    Nome = "Aluno Ativo",
                    Ativo = true
                });

            turmaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Turma
                {
                    Id = 1,
                    Nome = "Turma com vaga",
                    VagasDisponiveis = 10
                });

            matriculaRepositoryMock
                .Setup(repo => repo.ExistsAsync(1, 1))
                .ReturnsAsync(false);

            matriculaRepositoryMock
                .Setup(repo => repo.CreateAsync(1, 1))
                .ReturnsAsync(new Matricula
                {
                    Id = 100,
                    AlunoId = 1,
                    TurmaId = 1,
                    DataMatricula = DateTime.UtcNow
                });

            var service = new MatriculaService(
                matriculaRepositoryMock.Object,
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object);

            var dto = new CriarMatriculaDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            // Act
            var resultado = await service.CreateAsync(dto);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(100, resultado.Id);
            Assert.AreEqual(1, resultado.AlunoId);
            Assert.AreEqual(1, resultado.TurmaId);

            // Verify
            matriculaRepositoryMock.Verify(
                repo => repo.ExistsAsync(1, 1),
                Times.Once);

            matriculaRepositoryMock.Verify(
                repo => repo.CreateAsync(1, 1),
                Times.Once);
        }

        [TestMethod]
        public async Task CreateAsync_VagaConsumidaDuranteOperacao_DeveLancarTurmaSemVagaException()
        {
            // Arrange
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();

            alunoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Aluno
                {
                    Id = 1,
                    Nome = "Aluno Ativo",
                    Ativo = true
                });

            turmaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Turma
                {
                    Id = 1,
                    Nome = "Última vaga",
                    VagasDisponiveis = 1
                });

            matriculaRepositoryMock
                .Setup(repo => repo.ExistsAsync(1, 1))
                .ReturnsAsync(false);

            matriculaRepositoryMock
                .Setup(repo => repo.CreateAsync(1, 1))
                .ReturnsAsync((Matricula)null);

            var service = new MatriculaService(
                matriculaRepositoryMock.Object,
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object);

            var dto = new CriarMatriculaDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<TurmaSemVagaException>(
                () => service.CreateAsync(dto));

            // Verify
            matriculaRepositoryMock.Verify(
                repo => repo.ExistsAsync(1, 1),
                Times.Once);

            matriculaRepositoryMock.Verify(
                repo => repo.CreateAsync(1, 1),
                Times.Once);
        }
    }
}
