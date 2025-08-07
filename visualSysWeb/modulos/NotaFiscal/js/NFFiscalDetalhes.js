
function atualizaPIS(textbox) {
    var valorCST = textbox.value || '';

    //Encontra a TR (linha do Gridview) onde o TextBox está
    var linha = textbox.closest("tr");

    //Pegar os valores para efeito de base de cálculo
    var txVlrItem = linha.cells[6].innerText.trim();
    var txVlrICMS = linha.querySelector('input[name*="txtVlrICMs"]');

    var vlrItem = parseFloat(txVlrItem.replace(",", "."));
    var vlrICMS = parseFloat(txVlrICMS.value.replace(",", "."));


    var txBC = linha.querySelector('input[name*="txtBCPisCofins"]');
    var txAliqPIS = linha.querySelector('input[name*="txtAliqPIS"]');
    var txVlrPis = linha.querySelector('input[name*="txtVlrPIS"]');
    var txAliqCofins = linha.querySelector('input[name*="txt1AliqCofins"]');
    var txVlrCofins = linha.querySelector('input[name*="txtVlrcofins"]');

    if (valorCST == "50") {
        txBC.value = (vlrItem - vlrICMS).toFixed(2).replace(".",",");
        txAliqPIS.value = (1.65).toFixed(2).replace(".", ",");
        txVlrPis.value = ((vlrItem - vlrICMS) * 0.0165).toFixed(2).replace(".", ",");
        txAliqCofins.value = (7.60).toFixed(2).replace(".", ",");;
        txVlrCofins.value = ((vlrItem - vlrICMS) * 0.0760).toFixed(2).replace(".", ",");
    }
    else {
        txBC.value = "0,00";
        txAliqPIS.value = "0,00";
        txVlrPis.value = "0,00";
        txAliqCofins.value = "0,00";
        txVlrCofins.value = "0,00";
    }

}