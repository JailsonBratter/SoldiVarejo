function nomeCliente() {
    var cod = getClienteId("txtCodCliente").value;
    var strNome = PagePadrao.GetNomeCliente(cod).value;
    

    getClienteId("txtCliente").value = strNome;
}


function nomeChange() {
    var nome = getClienteId("txtCliente").value;
    if (nome.indexOf("|") >= 0) {
       getClienteId("txtCodCliente").value= nome.substr(0, nome.indexOf("|"));
       getClienteId("txtCliente").value = nome.substr(nome.indexOf("|")+1);
    }
}

function mesmoFuncionario() {
    getClienteId("txtFuncionarioEntrega").value = getClienteId("txtFuncionarioRetira").value;
}