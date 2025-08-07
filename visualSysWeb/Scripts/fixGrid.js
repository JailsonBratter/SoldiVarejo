// fixGrid : Projeto para fixar
// colunas e linhas de uma gridview usando JQuery
//
// Por : Dennes Torres
// http://twitter.com/Dennes
// dennes@bufaloinfo.com.br
// http://www.bufaloinfo.com.br
//
// Data : 11/04/2011
//
// Estas linhas não devem ser removidas

jQuery.fn.fixarGrid = function (colunasFixas) {
    var nomeGrid;
    nomeGrid = $(this).attr("id");
    fixartudo("#" + nomeGrid, colunasFixas);
}

//Rotina principal, faz as duas fixações juntas
function fixartudo(tabela, qtdColunas) {
    var nomeContainer;
    var tamanhos = new Array();
    var qtd = 0;
    var i;
    var linhas;
    var linhascopia;
    var nomeCopia = "divcopia";

    nomeContainer = $(tabela).parent().parent().attr("id");
    nomeContainer = "#" + nomeContainer;

    //guardando a largura original das colunas
    $(nomeContainer + " " + tabela)
                .children("tbody")
                .children("tr").first().children("td").each(
                function () {
                    tamanhos[qtd] = $(this).attr("clientWidth");
                    qtd = qtd + 1;
                });

    fixarcoluna(tabela, nomeContainer, qtdColunas);
    fixartitulo(tabela, nomeContainer, nomeCopia)

    nomeCopia = "#" + nomeCopia;

    //Remove o titulo original
    $(nomeContainer + " " + tabela).children("thead").remove();

    //redefinindo a largura das colunas para o valor original
    i = qtdColunas;
    $(nomeContainer + " " + tabela)
                .children("tbody")
                .children("tr").first().children("td").each(
                function () {
                    $(this).css("width", tamanhos[i]);
                    i = i + 1;
                })

    //O mesmo para divcopia
    i = qtdColunas;
    $(nomeCopia + " " + tabela)
                .children("thead")
                .children("tr").first().children("th").each(
                function () {
                    $(this).css("width", tamanhos[i]);
                    i = i + 1;
                })

    $(nomeContainer + " " + tabela).css("width",
                    $(nomeContainer + " " + tabela).attr("clientWidth"));

    // acertar a altura total

    $("#divcopia2").css("height",
                    $(nomeContainer).attr("clientHeight"));



    clientWidth = $("#divcopia2 " + tabela).attr("clientWidth");
    $("#divcopia2").css("width",
                      clientWidth)
                     .css("float", "left");

    $(nomeContainer).css("width",
                       $(nomeContainer).attr("clientWidth") -
                       clientWidth);

    //                $(nomeContainer).css("width",
    //                       $(nomeContainer).attr("clientWidth"));

    $(nomeCopia + " " + tabela).css("width",
                 $(nomeContainer + " " + tabela).attr("clientWidth"));

    $(nomeCopia).css("width", $(nomeContainer).attr("clientWidth"));

    // acertar a altura da linha
    linhas = $("tr", nomeContainer + " table");
    linhascopia = $("tr", "#divcopia2 table");

    i = 0;
    for (i = 0; i < linhas.length; i++) {
        clientHeight = linhas[i].clientHeight;
        $(linhascopia[i + 1]).css("height", clientHeight);
    }

    // inicio da 2a fase
    var containerOriginal = nomeContainer;
    nomeContainer = "#divcopia2";
    nomeCopia = "copiaextra";
    qtd = 0;
    //guardando a largura original das colunas
    tamanhos = new Array();
    $(nomeContainer + " " + tabela)
                .children("tbody")
                .children("tr").first().children("td").each(
                function () {
                    tamanhos[qtd] = $(this).attr("clientWidth");
                    qtd = qtd + 1;
                });


    fixartitulo(tabela, "#divcopia2", nomeCopia);

    nomeCopia = "#" + nomeCopia;

    //Remove o titulo original
    $(nomeContainer + " " + tabela).children("thead").remove();

    //redefinindo a largura das colunas para o valor original
    i = 0;
    $(nomeContainer + " " + tabela)
                .children("tbody")
                .children("tr").first().children("td").each(
                function () {
                    $(this).css("width", tamanhos[i]);
                    i = i + 1;
                })

    //O mesmo para divcopia
    i = 0;
    $(nomeCopia + " " + tabela)
                .children("thead")
                .children("tr").first().children("th").each(
                function () {
                    $(this).css("width", tamanhos[i]);
                    i = i + 1;
                })

    //retirar o float

    $(nomeCopia).css("float", "");


    $(nomeCopia).css("width", $(nomeContainer).attr("clientWidth"));

    $(nomeCopia + " " + tabela).css("width",
                         $(nomeContainer).css("width"));

    $(nomeContainer + " " + tabela).css("width",
                        $(nomeContainer).css("width"));

    //Ajuste do posicionamento
    $("#divcopia").insertBefore("#divcopia2");
    $("#copiaextra").css("float", "left");
    $(nomeContainer).css("height",
                    $(containerOriginal).attr("clientHeight"));

}



//Fixar as colunas da gridView
function fixarcoluna(tabela, nomeContainer, qtdColunas) {
    //   var tamanhos = new Array();  altura das linhas
    var qtd = 0; // quantidade de colunas
    var i = 0; //contadores
    var icopia = 0 // contador 2
    var colunas // colunas da divcopia2
    var clientWidth //clientWidth da divcopia2 gridview1
    var linhas //linhas da divtabela
    var linhascopia //linhas da divcopia
    var clientHeight // altura da linha


    $(nomeContainer).clone().insertBefore(nomeContainer)
                    .attr("id", "divcopia2")
                    .css("overflow", "hidden")

    linhascopia = $("tr", "#divcopia2 table");
    for (icopia = 0; icopia < linhascopia.length; icopia++) {
        colunas = $(linhascopia[icopia]).children("td,th");

        if (qtd == 0)
            qtd = colunas.length;

        for (i = qtdColunas; i < qtd; i++)
            $(colunas[i]).remove();
    }

    i = 0;
    linhas = $("tr", nomeContainer + " table");
    for (i = 0; i < linhas.length; i++) {
        colunas = $(linhas[i]).children("td,th");
        for (icopia = 0; icopia < qtdColunas; icopia++) {
            $(colunas[icopia]).remove();
        }
    }

    $(nomeContainer).scroll(function () {
        $("#divcopia2").scrollLeft($(this).scrollLeft())
                          .scrollTop($(this).scrollTop());

        $("#divcopia").scrollLeft($(this).scrollLeft())
                          .scrollTop($(this).scrollTop());
    })
}



//fixar o título da gridview
function fixartitulo(tabela, nomeContainer, nomeCopia) {
    var tamanhos = new Array();
    var qtd = 0;
    var i = 0;

    $(nomeContainer).clone().insertBefore(nomeContainer)
                    .attr("id", nomeCopia)
                    .css("overflow", "hidden")
                    .css("height", "")
                     .children("div")
                     .children("table")
                     .css("width", $(tabela)
                        .attr("clientWidth"))

    nomeCopia = "#" + nomeCopia;

    $(nomeCopia + " table")
                    .children("tbody").remove();

    $(nomeCopia + " table")
                .children("thead")
                .children("tr").first().children("th").each(
                function () {
                    $(this).css("width", tamanhos[i]);
                    i = i + 1;
                })
            }

