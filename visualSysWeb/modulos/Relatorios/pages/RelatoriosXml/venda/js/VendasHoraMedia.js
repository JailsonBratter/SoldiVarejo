function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=700,width=1200');
    var hostURl = window.location.href;
    hostURl = hostURl.substring(0, hostURl.indexOf('/modulos'));
    mywindow.document.write('<html><head><title>Relatorio Vendas Medias</title>');

    mywindow.document.write(' <link href="' + hostURl + '/Scripts/chartist/chartist.min.css" rel="stylesheet" type="text/css">');
    mywindow.document.write(' <link href="../css/VendasHoraMedia.css" rel="stylesheet" />');


    mywindow.document.write(' <script type="text/javascript" src="' + hostURl + '/Scripts/chartist/chartist.min.js">\x3C/script>');



    mywindow.document.write('</head><body>');

    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    //mywindow.close();
    mywindow.document.close(); // necessary for IE >= 10
    

    return true;
}


function carregarGraficos(nomeGrafico, lbl, valores) {
    var data = {
        labels: lbl,
        series: valores

    };



    var options = {

        height: 300
    };
    new Chartist.Line(nomeGrafico, data, options);



};