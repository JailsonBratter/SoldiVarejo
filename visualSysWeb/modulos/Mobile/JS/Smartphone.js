function calculaQtde() {
    alert("vou calcular");
    var vQtde = getClienteId("txtQuantidade").value.replace(',', '.');
    var vQtdeEmbalagem = getClienteId("txtEmbalagem").value.replace(',', '.');
    var vQtdeTotal = 0;

    if (vQtde > 0 && vQtdeEmbalagem > 0) {
        vQtdeTotal = (Number(vQtde) * Number(vQtdeEmbalagem));
    }
    else {
        vQtdeTotal = 0;
    }

    getClienteId("txtQuantidadeTotal").value = Number(vQtdeTotal).toFixed(0).replace(".", ",");
    
}
