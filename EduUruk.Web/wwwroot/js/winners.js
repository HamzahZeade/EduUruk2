//// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//var btnS1 = document.querySelector('#btnSound1');
//var btnS2 = document.querySelector('#btnSound2');

//var bg_audio = document.getElementById("bg_audio");
//var bg_sound = 0;

//btnS1.addEventListener('click', function (e) {
//    e.preventDefault();
//    playbgaudio();
//});

//btnS2.addEventListener('click', function (e) {
//    e.preventDefault();
//    playbgaudio();
//});

//function playbgaudio() {
//    if (bg_sound == 0) {
//        btnS1.querySelector('#btnSound1Icon').src = '/icon/sound.gif';
//        btnS2.querySelector('#btnSound2Icon').src = '/icon/sound.gif';
//        bg_audio.volume = 0.2;
//        bg_sound = 1;
//        bg_audio.play();
//    } else if (bg_sound == 1) {
//        btnS1.querySelector('#btnSound1Icon').src = '/icon/mute.png';
//        btnS2.querySelector('#btnSound2Icon').src = '/icon/mute.png';
//        bg_audio.volume = 0.2;
//        bg_sound = 0;
//        bg_audio.pause();
//    }
//}



var y2019 = document.querySelector('.y2019');
var y2020 = document.querySelector('.y2020');
var y2022 = document.querySelector('.y2022');

y2019.addEventListener('click', function (e) {
    e.preventDefault();
    y2019.classList.add('btnYearCurrent');
    y2020.classList.remove('btnYearCurrent');
    y2022.classList.remove('btnYearCurrent');

    btnWinnersY2019.classList.remove('d-none');
    btnWinnersY2020.classList.add('d-none');
    btnWinnersY2022.classList.add('d-none');
});
y2020.addEventListener('click', function (e) {
    e.preventDefault();
    y2019.classList.remove('btnYearCurrent');
    y2020.classList.add('btnYearCurrent');
    y2022.classList.remove('btnYearCurrent');

    btnWinnersY2019.classList.add('d-none');
    btnWinnersY2020.classList.remove('d-none');
    btnWinnersY2022.classList.add('d-none');
});
y2022.addEventListener('click', function (e) {
    e.preventDefault();
    y2019.classList.remove('btnYearCurrent');
    y2020.classList.remove('btnYearCurrent');
    y2022.classList.add('btnYearCurrent');

    btnWinnersY2019.classList.add('d-none');
    btnWinnersY2020.classList.add('d-none');
    btnWinnersY2022.classList.remove('d-none');
});

var btnsWinner19 = document.querySelector('#btnWinnersY2019').querySelectorAll('button.btnWinner');
var btnsWinner20 = document.querySelector('#btnWinnersY2020').querySelectorAll('button.btnWinner');
var btnsWinner22 = document.querySelector('#btnWinnersY2022').querySelectorAll('button.btnWinner');

btnsWinner19.forEach(btn => {
    btn.addEventListener('click', function () {
        btnsWinner19.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner20.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner22.forEach(b => { b.classList.remove('btnWinnerCurrent'); });

        btn.classList.add('btnWinnerCurrent');

        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dName, dTitle, dType, dInfo);
    });
});

btnsWinner20.forEach(btn => {
    btn.addEventListener('click', function () {
        btnsWinner19.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner20.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner22.forEach(b => { b.classList.remove('btnWinnerCurrent'); });

        btn.classList.add('btnWinnerCurrent');

        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dName, dTitle, dType, dInfo);
    });
});

btnsWinner22.forEach(btn => {
    btn.addEventListener('click', function () {
        btnsWinner19.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner20.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner22.forEach(b => { b.classList.remove('btnWinnerCurrent'); });

        btn.classList.add('btnWinnerCurrent');

        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dName, dTitle, dType, dInfo);
    });
});

var vid_s = document.createElement('source');

function viewData(dName, dTitle, dType, dInfo) {
    var wViewer = document.querySelector('.winner-view');

    $(wViewer).fadeOut(300, function () {
        document.querySelector('#wVidvS').setAttribute('src', "");
        document.querySelector('#wVidv').load();

        document.querySelector('#wImgv').src = "";
        document.querySelector('#wLinkv').href = "";

        var wn = document.querySelector('#winner-name');
        var wt = document.querySelector('#winner-title');
        var wV = document.querySelector('.wVid');
        var wI = document.querySelector('.wImg');
        var wC = document.querySelector('.wIcon');

        wV.classList.add('d-none');
        wI.classList.add('d-none');
        wC.classList.add('d-none');

        wn.innerHTML = dName;
        wt.innerHTML = dTitle;

        if (dType == 'vid') {
            wV.classList.remove('d-none');
            document.querySelector('#wVidvS').setAttribute('src', dInfo);
            document.querySelector('#wVidv').load();
        } else if (dType == 'img') {
            wI.classList.remove('d-none');
            document.querySelector('#wImgv').src = dInfo;
        } else if (dType == 'link') {
            wC.classList.remove('d-none');
            document.querySelector('#wLinkv').href = dInfo;
        }
        $(wViewer).fadeIn(300);
    });
}

document.querySelector('[data-code="222"]').click();




