// Based on the pen "Grid items view with Flip Plugin" by GreenSock:
// https://codepen.io/GreenSock/pen/JjXqMZK

const items = gsap.utils.toArray(".item"),
      details = document.querySelector('.detail'),
      detailContent = document.querySelector('.content'),
      detailImage = document.querySelector('.detail img'),
      detailTitle = document.querySelector('.detail .title'),
      detailTitleSpa = document.querySelector('.detail .titlespa'),
      detailSecondary = document.querySelector('.detail .secondary'),
      detailDescription = document.querySelector('.detail .description');

let activeItem; 

gsap.set(detailContent, { yPercent: -100 }); 

function showDetails(item) {
	
  if (activeItem) {
		return hideDetails();
	}
  
	let onLoad = () => {

		Flip.fit(details, item, {scale: true, fitChild: detailImage});
		const state = Flip.getState(details);
		gsap.set(details, {clearProps: true}); 
		gsap.set(details, {xPercent: -50, top: "50%", yPercent: -50, visibility: "visible", overflow: "hidden"});

		Flip.from(state, {
			duration: 0.5,
			ease: "power2.inOut",
			scale: true,
			onComplete: () => gsap.set(details, {overflow: "auto"}) 
		}).to(detailContent, {yPercent: 0}, 0.2);

		detailImage.removeEventListener("load", onLoad);
		document.addEventListener('click', hideDetails);
	};

	const data = item.dataset;
	detailImage.addEventListener("load", onLoad);
	detailImage.src = item.querySelector('img').src;
	
	detailTitle.innerText = data.title;
	detailTitleSpa.innerText = data.titlespa;
	detailSecondary.innerText = data.secondary;
	detailDescription.innerText = data.text;

	gsap.to(items, {opacity: 0.3, stagger: { amount: 0.7, from: items.indexOf(item), grid: "auto"}}).kill(item);
	gsap.to(".app", {backgroundColor: "#888", duration: 1, delay: 0.3}); 
	activeItem = item;
	 
	$("#CurrentImageUrl").prop("href", item.querySelector('img').src);
}

function hideDetails() {
	document.removeEventListener('click', hideDetails);
	gsap.set(details, {overflow: "hidden"});

	const state = Flip.getState(details);

	Flip.fit(details, activeItem, {scale: true, fitChild: detailImage});

	const tl = gsap.timeline();
	tl.set(details, {overflow: "hidden"})
    .to(detailContent, {yPercent: -100})
	  .to(items, {opacity: 1, stagger: {amount: 0.7, from: items.indexOf(activeItem), grid: "auto"}})
	  .to(".app", {backgroundColor: "#fff"}, "<");

	Flip.from(state, {
		scale: true,
		duration: 0.5,
		delay: 0.25, 
		onInterrupt: () => tl.kill()
	}).set(details, {visibility: "hidden"});

	activeItem = null;
}

gsap.utils.toArray('.item').forEach(item => item.addEventListener('click', () => showDetails(item)));

window.addEventListener('load', () => {
	gsap.to('.app', { autoAlpha: 1, duration: 0.2 });
	gsap.from('.item', {autoAlpha: 0, yPercent: 30, stagger: 0.04});
});