function abrePopupCofigColetor() {
    var url = window.location.href;
    url = url.substr(0, url.indexOf("/modulos/"));

    window.open( url + "/code/ConfiguracaoColetor.aspx", 'Configurar Coletor', 'height=200,width=600');
    
}