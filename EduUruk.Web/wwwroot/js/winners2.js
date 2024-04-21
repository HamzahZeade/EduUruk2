
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

        var dLogo = btn.getAttribute('data-logo');
        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dLogo, dName, dTitle, dType, dInfo);
    });
});

btnsWinner20.forEach(btn => {
    btn.addEventListener('click', function () {
        btnsWinner19.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner20.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner22.forEach(b => { b.classList.remove('btnWinnerCurrent'); });

        btn.classList.add('btnWinnerCurrent');

        
        var dLogo = btn.getAttribute('data-logo');
        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dLogo, dName, dTitle, dType, dInfo);
    });
});

btnsWinner22.forEach(btn => {
    btn.addEventListener('click', function () {
        btnsWinner19.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner20.forEach(b => { b.classList.remove('btnWinnerCurrent'); });
        btnsWinner22.forEach(b => { b.classList.remove('btnWinnerCurrent'); });

        btn.classList.add('btnWinnerCurrent');

        var dLogo = btn.getAttribute('data-logo');
        var dName = btn.getAttribute('data-name');
        var dTitle = btn.getAttribute('data-title');
        var dType = btn.getAttribute('data-type');
        var dInfo = btn.getAttribute('data-info');
        viewData(dLogo, dName, dTitle, dType, dInfo);
    });
});

var vid_s = document.createElement('source');

function viewData(dLogo, dName, dTitle, dType, dInfo) {
    var wViewer = document.querySelector('.winner-view');

    $(wViewer).fadeOut(300, function () {
        document.querySelector('#wVidvS').setAttribute('src', "");
        document.querySelector('#wVidv').load();

        document.querySelector('#wImgv').src = "";
        document.querySelector('#wLinkv').href = "";

        var wl = document.querySelector('#winner-logo')
        var wn = document.querySelector('#winner-name');
        var wt = document.querySelector('#winner-title');
        var wV = document.querySelector('.wVid');
        var wI = document.querySelector('.wImg');
        var wC = document.querySelector('.wIcon');

        wV.classList.add('d-none');
        wI.classList.add('d-none');
        wC.classList.add('d-none');

        if (dLogo != null) {
            wl.src = dLogo;
            wl.classList.remove('d-none');
        } else {
            wl.classList.add('d-none');
        }
        
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




