
function selecionarPedidos(elemento) {
    var Inputs = MainContent_gridPedidosImportar.getElementsByTagName("input");
    for (var i = 0; i < Inputs.length; ++i) {
        if (Inputs[i].type == 'checkbox') {
            Inputs[i].checked = elemento.checked;

        }
    }
    vlrTotalSelecionado();
}

function vlrTotalSelecionado()
{
    var Inputs = MainContent_gridPedidosImportar.getElementsByTagName("input");
    
    var vlrTotal = 0;
    for (var i = 1; i < Inputs.length; ++i) {
        if (Inputs[i].type == 'checkbox') {
            if(Inputs[i].checked)
            {
                var rows = MainContent_gridPedidosImportar.getElementsByTagName("tr");
                var row = rows[i];
                var tds  = row.getElementsByTagName("td");
                var vValorCel = tds[5].innerText;

                vlrTotal += Number(vValorCel.replace(",", "."));

            }

        }
    }
    MainContent_lblValorSelecionado.innerText = Number(vlrTotal).toFixed(2).replace('.', ',');
}