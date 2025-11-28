function abrePopupCofigColetor() {
    var url = window.location.href;
    url = url.substr(0, url.indexOf("/modulos/"));
    window.open( url + "/code/ConfiguracaoColetor.aspx", 'Configurar Coletor', 'height=200,width=600');
    
}
window.atualizarDiferenca = function (txt) {
    var row = txt.closest("tr");
    var cells = row.getElementsByTagName("td");

    var saldoAtual = parseFloat((cells[6].innerText || "0").replace(",", "."));
    var contado = parseFloat((txt.value || "0").replace(",", "."));
    var diferenca = contado - saldoAtual;

    cells[9].innerText = diferenca.toFixed(3);
};