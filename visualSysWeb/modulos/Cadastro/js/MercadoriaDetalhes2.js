
function calculaPrecoDesconto() {
    var vPreco = getClienteId("txtTbPreco").value.replace(',', '.');
    var vtxtTbPreco = getClienteId("txtTbPrecoPromocao").value.replace(',', '.');
    var vDif = Number(vPreco) - Number(vtxtTbPreco);
    var vtxtTbPrecoDesconto = ((Number(vDif) / Number(vPreco)) * 100);
    getClienteId("txtTbPrecoDesconto").value = Number(vtxtTbPrecoDesconto).toFixed(2).replace('.', ',');

}

function calculaDescontoPreco() {
    var vPreco = getClienteId("txtTbPreco").value.replace(',', '.');
    var vtxtTbPrecoDesconto = getClienteId("txtTbPrecoDesconto").value.replace(',', '.');
    var vtxtTbPreco = (Number(vPreco) - ((Number(vPreco) * Number(vtxtTbPrecoDesconto)) / 100));
    getClienteId("txtTbPrecoPromocao").value = Number(vtxtTbPreco).toFixed(2).replace('.', ',');

}


function pratoDoDia(chk) {
    
    var Inputs = divPratoDia.getElementsByTagName("input");
        for (var i = 0; i < Inputs.length; ++i) {
            if (Inputs[i].type === 'checkbox') {
                Inputs[i].disabled = !chk.checked;
                if (!chk.checked)
                    Inputs[i].checked = false;
            }
        }
   
}