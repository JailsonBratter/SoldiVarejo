function chkTodos() {




    var ch = true;
    if (getClienteId("chkTodos").checked) {
        ch = true;
    }
    else {
        ch = false;
    }

    inputs = getClienteId("pnBlocos").getElementsByTagName('input');

    for (i = 0; i < inputs.length; i++) {
        elm = inputs[i]
        if (elm.type == 'checkbox') {
            elm.checked = ch;
        }
    }
}

function blocosfilhosSelecionados() {
    inputs = getClienteId("pnBlocos").getElementsByTagName('input');
    for (i = 0; i < inputs.length; i++) {
        elm = inputs[i]
        if (elm.type == 'checkbox') {
            if (!elm.checked) {
                getClienteId("chkTodos").checked = false;
            }
            for (c = 0; c < inputs.length; c++) {
                {

                    chfilho = inputs[c];
                    var strid = elm.id.substring(0, elm.id.length - 2);
                    if (chfilho.id.indexOf(strid) >= 0) {
                        if (chfilho.type == 'checkbox') {
                            if (chfilho.checked) {
                                elm.checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}