function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=700,width=1200');

    var hostURl = window.location.href;

    hostURl = hostURl.substring(0, hostURl.indexOf('/modulos'));

    mywindow.document.write('<html><head><title>Relatorio Impressão</title>');
    mywindow.document.write(' <link href="' + hostURl + '/Styles/site1.css" rel="stylesheet" type="text/css">');



    mywindow.document.write('</head><body >');

    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/


    mywindow.setTimeout(function () {
        mywindow.print()
    }, 1);


    //mywindow.close();

    return true;
}

function MostrarChave(Valor) {
    var mywindow = window.open('', 'CHAVE', 'height=100,width=500');

    mywindow.document.write('<html><head><title>Chave</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h3>'+Valor+'</h3>');
    mywindow.document.write('</body></html>');

    return true;
}


function abrirExcel(e) {

    var table_html = $("#MainContent_pnRelatorio").html();
    table_html = table_html.replace(/<img[^>]*>/gi, "");
    table_html = table_html.replace(/<a[^>]*>|<\/a>/g, "");
    table_html = table_html.replace("MainContent_pnRelatorioVisualizar", "2pnRelatorioVisualizar");

    $("#somehiddentable").html(table_html); 
    $("#somehiddentable").btechco_excelexport({
        containerid: "2pnRelatorioVisualizar"
        , datatype: $datatype.Table
        , filename: 'Reports'

    });
    
}

function onCalendarShown() {



    var cal = $find("calendar1");

    //Setting the default mode to month

    cal._switchMode("years", false);

    //cal._switchMode("month", true);



    //Iterate every month Item and attach click event to it

    if (cal._monthsBody) {

        for (var i = 0; i < cal._monthsBody.rows.length; i++) {

            var row = cal._monthsBody.rows[i];

            for (var j = 0; j < row.cells.length; j++) {

                Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);

            }

        }

    }

}



function onCalendarHidden() {

    var cal = $find("calendar1");

    //Iterate every month Item and remove click event from it

    if (cal._monthsBody) {

        for (var i = 0; i < cal._monthsBody.rows.length; i++) {

            var row = cal._monthsBody.rows[i];

            for (var j = 0; j < row.cells.length; j++) {

                Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);

            }

        }

    }



}

function call(eventElement) {

    var target = eventElement.target;
    var cal = $find("calendar1");
    switch (target.mode) {

        case "month":



            cal._visibleDate = target.date;

            cal.set_selectedDate(target.date);

            cal._switchMonth(target.date);

            cal._blur.post(true);

            cal.raiseDateSelectionChanged();

            break;

        case "years":


            cal._visibleDate = target.years;

            cal.set_selectedDate(target.years);

            cal._switchMonth(target.years);

            cal._blur.post(true);

            cal.raiseDateSelectionChanged();

            break;

    }

}


