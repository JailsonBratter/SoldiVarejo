function abrePopupCofigColetor() {
    var url = window.location.href;
    url = url.substr(0, url.indexOf("/modulos/"));

    window.open(url + "/code/ConfiguracaoColetor.aspx?nf=true", 'Configurar Coletor', 'height=220,width=600');

}