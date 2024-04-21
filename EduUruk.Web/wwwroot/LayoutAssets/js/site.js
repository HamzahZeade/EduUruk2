document.addEventListener("DOMContentLoaded", function(){
    window.addEventListener('scroll', function() {
        if (window.scrollY > 100) {
            document.getElementById('navbar_top').classList.add('fixed-top');
            document.querySelector('.speakers-logo').src = "/LayoutAssets/img/logo.svg";
            document.querySelector('.navbar-toggler svg').setAttribute('fill', '#000');
            document.querySelector('.speakers-logo').classList.remove('my-3');
            document.querySelector('#navbar_top_container').classList.remove('border-bottom');
            document.querySelectorAll('.nav-link').forEach((element) => element.classList.add('nav-link2'));
        } else {
             document.getElementById('navbar_top').classList.remove('fixed-top');
            document.querySelector('.speakers-logo').src = "/LayoutAssets/img/logo-white.svg";
             document.querySelector('.navbar-toggler svg').setAttribute('fill', '#fff');
             document.querySelector('.speakers-logo').classList.add('my-3');
             document.querySelector('#navbar_top_container').classList.add('border-bottom');
             document.querySelectorAll('.nav-link').forEach((element) => element.classList.remove('nav-link2'));
        } 
    });
}); 