

function valorArredondar(object) {
    PageMethods.getValorArredondar(102.32, 2, 1, (resp) => {
        
    });
}

function calculaValor(object,grid) {
    PageMethods.getTabPrecoMarg((tabMargem) => {
        const index = object.id.substring(object.id.length - 2).replace("_", "");
        const row = object.parentElement.parentElement;
        console.log(row.cells[3].innerText);   
        const ddl = getClienteId("TabContainer1_"+grid+"_ddlTipoArredondamento_" + index);
     
        if (ddl != null) {
            if (tabMargem) {
                let txtMargemItem = getClienteId("TabContainer1_" + grid +"_txtMargemItem_" + index);
                let txtPrecoPromocaoItem = getClienteId("TabContainer1_"+grid+"_txtPrecoPromocaoItem_"+index);

                let vlrMargem = Number(txtMargemItem.value.replace(',', '.'));
                let vlrCusto = Number(row.cells[4].innerText.replace(',', '.'));
                PageMethods.getPrecoMargem(vlrCusto, vlrMargem, (preco) => {
                    PageMethods.getValorArredondar(preco, 1, ddl.value, (precoArredondado) => {
                        txtPrecoPromocaoItem.value = Number(precoArredondado).toFixed(2).replace('.', ',')
                        //calcularPrecoMargem(row);
                   
                    })
                    
                });
                
            }
            else {
                let chk = getClienteId("TabContainer1_" + grid +"_chkAcrescimo_" + index);
                let txtPrecoPromocaoItem = getClienteId("TabContainer1_" + grid +"_txtPrecoPromocaoItem_" + index);
                let txtDesc = getClienteId("TabContainer1_" + grid +"_txtDescontoItem_" + index);

                let tipo = ddl.value;
                let preco = Number(row.cells[3].innerText.replace(',', '.'));

                let desconto = Number(txtDesc.value.replace(",","."));
                let acrescimo = (chk != null ? chk.checked : false);
                PageMethods.getCalculoPrecoPromocao(preco, desconto, acrescimo, (valor) => {
                    PageMethods.getValorArredondar(valor, 1, tipo, (precoArredondado) => {
                        let nValorDesc = (preco - precoArredondado);
                        if (acrescimo) {
                            nValorDesc = nValorDesc * -1;
                        }
                        txtPrecoPromocaoItem.value = Number(precoArredondado).toFixed(2).replace('.', ',')
                        if (nValorDesc != 0) {
                            nValorDesc = (nValorDesc / preco) * 100;
                            txtDesc.value = Number(nValorDesc).toFixed(2).replace('.', ',')
                        }
                    })

                })
            }

        }
    });
}

function calculaDesconto(object,grid)
{
    const index = object.id.substring(object.id.length - 2).replace("_", "");
    const row = object.parentElement.parentElement;
    const ddl = getClienteId("TabContainer1_" + grid + "_ddlTipoArredondamento_" + index);

    let chk = getClienteId("TabContainer1_" + grid + "_chkAcrescimo_" + index);
    let txtPrecoPromocaoItem = getClienteId("TabContainer1_" + grid + "_txtPrecoPromocaoItem_" + index);
    let txtDesc = getClienteId("TabContainer1_" + grid + "_txtDescontoItem_" + index);

    let tipo = ddl.value;
    let preco = Number(row.cells[3].innerText.replace(',', '.'));

   
    let valor = Number(txtPrecoPromocaoItem.value.replace(',', '.'));
    PageMethods.getValorArredondar(valor, 2, tipo, (precoArredondado) => {
        let nValorDesc = (preco - precoArredondado);


        txtPrecoPromocaoItem.value = Number(precoArredondado).toFixed(2).replace('.', ',');
        if (txtDesc != null && nValorDesc != 0) {
            nValorDesc = (nValorDesc / preco) * 100;
            if (nValorDesc < 0) {
                nValorDesc = nValorDesc * -1;
                chk.checked = true;
            }
            else {
                chk.checked = false;
            }
            txtDesc.value = Number(nValorDesc).toFixed(2).replace('.', ',');

        }
    });
   
}

function calculaMargemPreco(object, grid)
{
    const index = object.id.substring(object.id.length - 2).replace("_", "");
    const row = object.parentElement.parentElement;
    let txtMarg = getClienteId("TabContainer1_" + grid + "_txtMargemItem_" + index);
    let txtPrecoPromo = getClienteId("TabContainer1_" + grid + "_txtPrecoPromocaoItem_" + index);
    const ddl = getClienteId("TabContainer1_" + grid + "_ddlTipoArredondamento_" + index);
    
    let preco = Number(row.cells[4].innerText.replace(',', '.'));

    let marg = Number(txtMarg.value.replace(",", "."));
    PageMethods.getPrecoMargem(preco, marg, (precoFinal) => {
        PageMethods.getValorArredondar(precoFinal, 1, ddl.value, (precoArredondado) => {
            txtPrecoPromo.value = Number(precoArredondado).toFixed(2).replace('.',',');
            calcularPrecoMargem(object,grid);
        })
    })
 
}

function calcularPrecoMargem(object, grid)
{
    const index = object.id.substring(object.id.length - 2).replace("_", "");
    const row = object.parentElement.parentElement;

    let txtMarg = getClienteId("TabContainer1_" + grid + "_txtMargemItem_" + index);
    let txtPrecoPromo = getClienteId("TabContainer1_" + grid + "_txtPrecoPromocaoItem_" + index);

    let preco = Number(row.cells[4].innerText.replace(',', '.'));
    
    let precoPromocao = Number(txtPrecoPromo.value.replace(",", "."));
    txtPrecoPromo.value = Number(precoPromocao).toFixed(2).replace('.', ',');

    PageMethods.getValorMargem(preco, precoPromocao, (margFinal) => {
        txtMarg.value = Number(margFinal).toFixed(2).replace('.', ',');
    });
}
