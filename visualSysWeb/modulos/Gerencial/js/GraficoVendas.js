
function carregarGraficos(nomeGrafico,lbl,valores) {
    var data = {
        labels: lbl,
        series: valores
        
    };

    

    var options = {
        
        height: 300
    };
    new Chartist.Line(nomeGrafico, data, options);
    
    

};