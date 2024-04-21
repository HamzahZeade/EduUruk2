/* Created by Tivotal */

var swiper = new Swiper(".slider-content", {
  slidesPerView: 3,
    spaceBetween: 25,
    centeredSlides: true,
  loop: "true",
  centerSlide: "true",
  fade: "true",
  grabCursor: "true",
  pagination: {
    el: ".swiper-pagination",
    clickable: "true",
    dynamicBullets: "true",
  },
  navigation: {
      nextEl: ".swiper-button-prev",
      prevEl: ".swiper-button-next",
  },

  breakpoints: {
    0: {
      slidesPerView: 1,
    },
    520: {
      slidesPerView: 2,
    },
    950: {
      slidesPerView: 3,
    },
  },
});
swiper.on('slideChange', function () {
  myCallbackfunction(this);
 
});

swiper.slideNext();
function myCallbackfunction(data){
  //debugger
  let previuosCard = document.getElementsByClassName('card swiper-slide')[data.previousIndex > data.activeIndex ? data.activeIndex + 1 : data.activeIndex - 1]
  previuosCard.style.zoom = 'normal';
  previuosCard.style.zIndex = '2';
  let activeCard = document.getElementsByClassName('card swiper-slide')[data.activeIndex];
  activeCard.style.zoom = '1.2';
  activeCard.style.zIndex = '1';
  console.log(data.realIndex);
}
