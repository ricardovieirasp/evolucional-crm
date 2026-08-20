$(function () {

    let paginaAtual = 1;
    const tamanhoPagina = 5;
    let totalRegistros = 0;

    function carregarAlunos() {

        const nome = $("#filtroNome").val().trim();

        $("#loading").removeClass("hidden");
        $("#error").addClass("hidden");
        $("#empty").addClass("hidden");
        $("#tabelaAlunos").addClass("hidden");

        $.ajax({
            url: "/api/alunos",
            method: "GET",

            data: {
                page: paginaAtual,
                pageSize: tamanhoPagina,
                nome: nome
            },

            success: function (response) {

                const alunos = response.Items || [];

                totalRegistros = response.Total || 0;
                paginaAtual = response.Page || 1;

                preencherTabela(alunos);

                $("#totalAlunos").text(totalRegistros);

                $("#paginaAtual").text(
                    "Página " + paginaAtual
                );

                atualizarPaginacao(
                    response.PageSize || tamanhoPagina
                );

                if (alunos.length === 0) {
                    $("#empty").removeClass("hidden");
                    $("#tabelaAlunos").addClass("hidden");
                } else {
                    $("#tabelaAlunos").removeClass("hidden");
                }
            },

            error: function () {
                $("#error").removeClass("hidden");
                $("#tabelaAlunos").addClass("hidden");
            },

            complete: function () {
                $("#loading").addClass("hidden");
            }
        });
    }

    function preencherTabela(alunos) {

        const tbody = $("#alunosBody");

        tbody.empty();

        alunos.forEach(function (aluno) {

            const nascimento = aluno.DataNascimento
                ? formatarData(aluno.DataNascimento)
                : "-";

            const status = aluno.Ativo
                ? '<span class="status active">Ativo</span>'
                : '<span class="status inactive">Inativo</span>';

            const linha = `
                <tr>
                    <td>${aluno.Id}</td>
                    <td>${aluno.Nome}</td>
                    <td>${aluno.Email || "-"}</td>
                    <td>${nascimento}</td>
                    <td>${status}</td>
                </tr>
            `;

            tbody.append(linha);
        });
    }

    function formatarData(data) {
        return new Date(data)
            .toLocaleDateString("pt-BR");
    }

    function atualizarPaginacao(pageSize) {

        const totalPaginas =
            Math.ceil(totalRegistros / pageSize);

        $("#paginaAnterior")
            .prop("disabled", paginaAtual <= 1);

        $("#proximaPagina")
            .prop(
                "disabled",
                paginaAtual >= totalPaginas
            );
    }

    $("#btnBuscar").on("click", function () {
        paginaAtual = 1;
        carregarAlunos();
    });

    $("#btnLimpar").on("click", function () {
        $("#filtroNome").val("");
        paginaAtual = 1;
        carregarAlunos();
    });

    $("#filtroNome").on("keypress", function (event) {

        if (event.which === 13) {
            paginaAtual = 1;
            carregarAlunos();
        }
    });

    $("#paginaAnterior").on("click", function () {

        if (paginaAtual > 1) {
            paginaAtual--;
            carregarAlunos();
        }
    });

    $("#proximaPagina").on("click", function () {

        const totalPaginas =
            Math.ceil(totalRegistros / tamanhoPagina);

        if (paginaAtual < totalPaginas) {
            paginaAtual++;
            carregarAlunos();
        }
    });

    carregarAlunos();
});