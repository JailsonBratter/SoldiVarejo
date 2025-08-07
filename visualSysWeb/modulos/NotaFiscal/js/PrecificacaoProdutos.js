
function gridLinhaTxt(input, idObj) {
    //MainContent_TabContainer1_TabPanel1_gridItens_txtPreco_novo_0
    //"MainContent_TabContainer1_TabPanel1_gridItens";
    var linha = input.id.substr(input.id.length - 2, 2);
    if (linha.substr(0, 1) == '_') {
        linha = linha.substr(1, 1)
    }
    return getClienteId("TabContainer1_TabPanel1_gridItens_" + idObj + "_" + linha)
}

function calculaMargem(input) {

    var precoCusto = gridLinhaTxt(input, "txtCusto").value.replace(',', '.');
    var precoVenda = gridLinhaTxt(input, "txtPreco_novo").value.replace(',', '.');
    var novaMargem = verificamargem(Number(precoCusto), Number(precoVenda), 0, 0);
    gridLinhaTxt(input, "txtMargem").value = Number(novaMargem).toFixed(4).replace('.', ',');
    gridLinhaTxt(input, "txtPreco_novo").value = Number(precoVenda).toFixed(2).replace('.', ',');

}

function calculaPreco(input) {
    var precoCusto = gridLinhaTxt(input, "txtCusto").value.replace(',', '.');
    var margem = gridLinhaTxt(input, "txtMargem").value.replace(',', '.');
    var novoPreco = verificapreco(Number(margem), Number(precoCusto));
    gridLinhaTxt(input, "txtPreco_novo").value = Number(novoPreco).toFixed(2).replace('.', ',');
    gridLinhaTxt(input, "txtMargem").value = Number(margem).toFixed(4).replace('.', ',');
}


function numerosGrid(input, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 44)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return autoTabGrid(input, evt);
}


function autoTabGrid(input, e) {
    var ind = 0;
    var isNN = (navigator.appName.indexOf("Netscape") != -1);
    var keyCode = (isNN) ? e.which : e.keyCode;
    var nKeyCode = e.keyCode;
    if (keyCode == 13) {
        if (!isNN) { window.event.keyCode = 0; } // evitar o beep  
        ind = getIndex(input);
        if ((ind + 1) == (input.form.length - 1)) {
            ind = 15;
            //ind = getIndex(input);
        }

        if (input.form[ind].type == 'textarea') {
            return false;
        }
        ind++;


        var hb = input.form[ind].disabled;
        while (hb) {
            ind++;
            hb = input.form[ind].disabled;
        }


        input.form[ind].focus();
        if (input.form[ind].type == 'text') {

            input.form[ind].select();
        }
        return false;
    }
}

function getIndex(input) {
    var index = -1, i = 0, found = false;
    while (i < input.form.length && index == -1)
        if (input.form[i] == input) {
            index = i;
            if (i < (input.form.length - 1)) {
                if (input.form[i + 1].type == 'hidden') {
                    index++;
                }
                if (input.form[i + 1].type == 'button' && input.form[i + 1].id == 'tabstopfalse') {
                    index++;
                }
            }
        }
        else
            i++;
    return index;
}
