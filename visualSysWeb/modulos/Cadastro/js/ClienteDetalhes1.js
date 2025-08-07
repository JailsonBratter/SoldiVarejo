
function changeFile(f) {
    $fileName = document.getElementById('file-name');
    $fileName.textContent = f.files[0].name;

    $txtNomeArquivo = document.getElementById('MainContent_txtNomeImagem');
    $txtNomeArquivo.value = f.files[0].name;

};

function chkTodos() {

    var ch = true;
    if (getClienteId("TabContainer1_TabGerarSenhaAcesso_gridModulos_chkTodos").checked) {
        ch = true;
    }
    else {
        ch = false;
    }


    inputs = getClienteId("TabContainer1_TabGerarSenhaAcesso_gridModulos").getElementsByTagName('input');


    for (i = 0; i < inputs.length; i++) {
        elm = inputs[i]
        if (elm.type == 'checkbox') {
            elm.checked = ch;
        }
     
    }
}