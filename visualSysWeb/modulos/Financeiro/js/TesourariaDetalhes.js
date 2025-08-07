
function atualizaGrid() {

    var rows = getClienteId("TabContainer1_tabCadastro_gridItens").rows.length - 2;

    var vtotalSistema = 0;
    var vtotalDigitado = 0;
    var mTotal = true;
    for (var i = 0; i < rows; i++) {

        try {

            var vSistema = moedaParaNumero(gridLinhaTxt(i, "txtRegistrado").value);
            gridLinhaTxt(i, "txtRegistrado").value = numeroParaMoeda(Number(vSistema));
            vtotalSistema = Number(vtotalSistema) + Number(vSistema);
        } catch (e) {
        mTotal = false;
        }


        var vDigitado = moedaParaNumero(gridLinhaTxt(i, "txtValor").value);
        gridLinhaTxt(i, "txtValor").value = numeroParaMoeda(Number(vDigitado));
        vtotalDigitado = Number(vtotalDigitado) + Number(vDigitado);

        try {


            var vDiferenca = Number(vDigitado) - Number(vSistema);
            gridLinhaTxt(i, "lblDiferenca").innerHTML = numeroParaMoeda(Number(vDiferenca));
            if (Number(vDiferenca) > 0) {
                gridLinhaTxt(i, "lblDiferenca").style.color = "Blue";
            }
            else if (Number(vDiferenca) < 0) {
                gridLinhaTxt(i, "lblDiferenca").style.color = "Red";
            }
            else {
                gridLinhaTxt(i, "lblDiferenca").style.color = "Black";
            }

        } catch (e) {

        }
    }


    if (mTotal) {
        getClienteId("TabContainer1_tabCadastro_gridItens_lblValorTotal").innerHTML = numeroParaMoeda(Number(vtotalDigitado));
        getClienteId("TabContainer1_tabCadastro_gridItens_lblRegistradoTotal").innerHTML = numeroParaMoeda(Number(vtotalSistema));
        var totalDiferenca = Number(vtotalDigitado) - Number(vtotalSistema);
        getClienteId("TabContainer1_tabCadastro_gridItens_lblDiferencaTotal").innerHTML = numeroParaMoeda(Number(totalDiferenca));
    }



}



function gridLinhaTxt(linha, idObj) {
    //MainContent_gridItens_txtPreco_compra_1

    return getClienteId("TabContainer1_tabCadastro_gridItens_" + idObj + "_" + linha);
}

function gridLista(linha, idObj) {
    //MainContent_gridItens_txtPreco_compra_1

    return getClienteId("GridLista_" + idObj + "_" + linha);
}


function onError(desc) {
    alert(desc);
}
var linha;
function onSuccess(result) {
   comboCartao = gridLista(linha, "ddlCartao");
   limparCombo(comboCartao);
   
    if (result.length > 0) {
             for (i = 0; i < result.length; i++) {
                 var opt0 = document.createElement("option");
                 opt0.value = result[i];
                 opt0.text = result[i];
                 comboCartao.add(opt0, comboCartao.options[i]);

             }
         }
    
}

function detalhesCartao(comboFinalizadora) {

    var strInicio = (Number(comboFinalizadora.id.length) - 2);
    linha = comboFinalizadora.id.substring(strInicio, comboFinalizadora.id.length);
     if(linha.substring(0,1) == "_")
     {
        linha = linha.substring(1,2);
     }
         var strCartao = comboFinalizadora.options[comboFinalizadora.selectedIndex].text;
         PageMethods.GetCartoes(strCartao,onSuccess);

}


function limparCombo(combo) {
    while (combo.length) {
        combo.remove(0);
    }
}