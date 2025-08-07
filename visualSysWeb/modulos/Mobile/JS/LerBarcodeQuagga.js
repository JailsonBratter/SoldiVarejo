function startScanner() {
    localStorage.removeItem('barraLida');
    localStorage.setItem('barraLida', 'inicio');
    open('lerBarra.html');
    return localStorage.getItem('barraLida');
}

//    Quagga.init({
//        inputStream: {
//            name: "Live",
//            type: "LiveStream",
//            target: document.querySelector('#camera')    // Or '#yourElement' (optional)
//        },
//        decoder: {
//            readers: ["code_128_reader"]
//        }
//    }, function (err) {
//        if (err) {
//            console.log(err);
//            return
//        }
//        console.log("Initialization finished. Ready to start");
//        Quagga.start();
//    });

//    Quagga.onDetected(function (data) {
//        console.log(data);
//        document.querySelector('#resultado').innerHTML = data.codeResult.code;
//        alert(document.querySelector('#resultado').textContent);
//        window.close();
//    });
//}
