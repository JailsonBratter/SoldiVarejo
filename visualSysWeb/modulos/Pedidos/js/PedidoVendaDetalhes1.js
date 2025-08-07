function calculaTotal() {
    let vPlu = getClienteId("txtPLU").value
    let cod_cliente = getClienteId("txtCliente_Fornec").value;
    var vQtde = getClienteId("txtQtde").value.replace(',', '.');
    var vprecoEditado = getClienteId("txtUnitario").value.replace(',', '.');
    //alert("Aqui inicia o cálculo.");
    
    PageMethods.getValorUnitarioPlu(vPlu, vQtde, cod_cliente, vprecoEditado, (valorAtacado) => {
        console.log(valorAtacado);
        let vPreco = 0;
        if (valorAtacado > 0) {
            getClienteId("txtUnitario").value = Number(valorAtacado).toFixed(2).replace('.', ',')
            getClienteId("")
            vPreco = valorAtacado;
        }
        else {
            vPreco = getClienteId("txtUnitario").value.replace(',', '.');
        }
        var vEmb = getClienteId("txtEmbalagem").value.replace(',', '.');

        var vDescPorc = getClienteId("txtDescontoItem").value.replace(',', '.');
        var vDesc = ((Number(vPreco) * Number(vDescPorc)) / 100);
        var vPrecFinal = (Number(vPreco) - Number(vDesc));
        var vtotal = ((Number(vQtde) * Number(vEmb)) * Number(vPrecFinal));
        //alert("Total Item" + vtotal.toString());
        getClienteId("TxtTotalItem").value = Number(vtotal).toFixed(2).replace('.', ',');
    });
}

function calculaTotalEditado() {
    var vQtde = getClienteId("txtQtde").value.replace(',', '.');
    var vPreco = getClienteId("txtUnitario").value.replace(',', '.');
    var vDescPorc = getClienteId("txtDescontoItem").value.replace(',', '.');
    var vDesc = ((Number(vPreco) * Number(vDescPorc)) / 100);
    var vPrecFinal = (Number(vPreco) - Number(vDesc));
    var vEmb = getClienteId("txtEmbalagem").value.replace(',', '.');


    var vtotal = ((Number(vQtde) * Number(vEmb)) * Number(vPrecFinal));

    getClienteId("TxtTotalItem").value = Number(vtotal).toFixed(2).replace('.', ',');
}

function desabilitar(out) {
    //OnClientClick = "document.getElementById('nextBtn').style.visibility='hidden';"
    out.style.visibility = 'hidden';
}

function limite_textarea(valor) {
    quant = 255;
    total = valor.length;
    resto = quant - total;
    document.getElementById('cont').innerHTML = resto;
    if(total <= quant) {
        document.getElementById('cont').innerHTML = resto;
    } else {
        document.getElementById('cont').innerHTML = "<spam style=\"color:red;\">"+resto+"</style>";
    }
}