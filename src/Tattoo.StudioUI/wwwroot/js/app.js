//document.addEventListener('DOMContentLoaded', function () {
//    const sections = document.querySelectorAll('.section');
//    const menuItems = document.querySelectorAll('.menu ul li');

//    function highlightMenu() {
//        let scrollPosition = document.documentElement.scrollTop || document.body.scrollTop;

//        sections.forEach((section, index) => {
//            const sectionTop = section.offsetTop - window.innerHeight / 2;
//            const sectionBottom = sectionTop + section.offsetHeight;

//            if (scrollPosition >= sectionTop && scrollPosition < sectionBottom) {
//                menuItems.forEach(item => item.classList.remove('active'));
//                menuItems[index].classList.add('active');
//            }
//        });
//    }

//    window.addEventListener('scroll', highlightMenu);
//    highlightMenu(); // Activate the current section on page load
//});

window.scrollToSection = function (sectionId) {
    var element = document.getElementById(sectionId);
    element.scrollIntoView({ behavior: 'smooth' });
};


window.showCookiePolicy = () => {
    document.getElementById('cookiePolicyModal').style.display = 'block';
};

window.hideCookiePolicy = () => {
    document.getElementById('cookiePolicyModal').style.display = 'none';
};


//window.onscroll = function () {
//    highlightMenu();
//};

//function highlightMenu() {
//    //var sections = document.querySelectorAll('.section');
//    //var menuItems = document.querySelectorAll('.menu nav ul li');

//    //sections.forEach((section, index) => {
//    //    var rect = section.getBoundingClientRect();
//    //    if (rect.top <= window.innerHeight / 2 && rect.bottom >= window.innerHeight / 2) {
//    //        menuItems.forEach(item => item.classList.remove('active'));
//    //        menuItems[index].classList.add('active');
//    //    }
//    //});
//    const sections = document.querySelectorAll('.section');
//    const navLinks = document.querySelectorAll('.menu ul li');

//    let currentIndex = sections.length - 1;

//    sections.forEach((section, index) => {
//        if (window.scrollY < section.offsetTop + section.offsetHeight / 2) {
//            currentIndex = index;
//        }
//    });

//    navLinks.forEach((link, index) => {
//        if (index === currentIndex) {
//            link.classList.add('active');
//        } else {
//            link.classList.remove('active');
//        }
//    });
//}


window.zoomImage = {
    setScale: function (element, scale) {
        element.style.transform = `scale(${scale})`;
    }
};

function focusElementByClass(className) {
    var elements = document.getElementsByClassName(className);
    if (elements.length > 0) {
        var element = elements[0]; // Obtén el primer elemento encontrado
        element.focus(); // Enfoca el elemento

        // Opciones para el método scrollIntoView
        var scrollIntoViewOptions = {
            behavior: 'smooth', // Animación suave
            block: 'nearest', // Posiciona el elemento lo más cercano posible al borde de la ventana
            inline: 'nearest' // Centra el elemento en la ventana
        };

        // Desplaza el elemento al área visible
        element.scrollIntoView(scrollIntoViewOptions);
    }
}

window.addEventListener("beforeunload", function (event) {
    event.preventDefault();
    event.returnValue = 'Esta app esta desarrollada con Blazor Wasm Single Page no es necesario recargar.'; // Un mensaje aquí no se muestra en la mayoría de los navegadores modernos, pero es necesario para algunos.
});

//window.scrollToSection = (sectionId) => {
//    document.getElementById(sectionId).scrollIntoView({ behavior: 'smooth' });
//};

//window.highlightMenuOnScroll = () => {
//    const sections = document.querySelectorAll('.section');
//    const navLinks = document.querySelectorAll('.menu ul li');

//    let currentIndex = sections.length - 1;

//    sections.forEach((section, index) => {
//        if (window.scrollY < section.offsetTop + section.offsetHeight / 2) {
//            currentIndex = index;
//        }
//    });

//    navLinks.forEach((link, index) => {
//        if (index === currentIndex) {
//            link.classList.add('active');
//        } else {
//            link.classList.remove('active');
//        }
//    });
//};

//window.addEventListener('scroll', window.highlightMenuOnScroll);


function resetScroll(selector) {
    var element = document.querySelector(selector);
    if (element) {
        element.scrollTop = 0; // Restablece la posición de desplazamiento vertical
    }
}

