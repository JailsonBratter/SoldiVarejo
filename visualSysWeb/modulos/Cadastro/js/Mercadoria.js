
function selecionar(elemento) {
    var Inputs = gridMercadorias.getElementsByTagName("input");
    for (var i = 0; i < Inputs.length; ++i) {
        if (Inputs[i].type == 'checkbox') {
            Inputs[i].checked = elemento.checked;

        }
    }
}