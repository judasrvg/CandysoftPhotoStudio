//window.readFileAsArrayBuffer = (input) => {
//    return new Promise((resolve, reject) => {
//        const reader = new FileReader();
//        reader.onload = (event) => {
//            resolve(new Uint8Array(event.target.result));
//        };
//        reader.onerror = reject;
//        reader.readAsArrayBuffer(input.files[0]);
//    });
//};

//window.getFileInputByName = (fileName) => {
//    const inputs = document.querySelectorAll('input[type="file"]');
//    for (let input of inputs) {
//        if (input.files.length > 0 && input.files[0].name === fileName) {
//            return input;
//        }
//    }
//    return null;
//};



window.onscroll = function () {
    highlightMenu();
};

function scrollToSection(sectionId) {
    var element = document.getElementById(sectionId);
    element.scrollIntoView({ behavior: 'smooth' });
}

function highlightMenu() {
    var sections = document.querySelectorAll('.section');
    var menuItems = document.querySelectorAll('.menu nav ul li');

    sections.forEach((section, index) => {
        var rect = section.getBoundingClientRect();
        if (rect.top <= window.innerHeight / 2 && rect.bottom >= window.innerHeight / 2) {
            menuItems.forEach(item => item.classList.remove('active'));
            menuItems[index].classList.add('active');
        }
    });
}

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

//function scrollToTop() {
//    window.scrollTo({ top: 0, behavior: 'smooth' });
//}

//function scrollToElement(elementId) {
//    const element = document.getElementById(elementId);
//    if (element) {
//        element.scrollIntoView({ behavior: 'smooth' });
//    }
//}

// Función Throttle para reducir la frecuencia de invocaciones
//function throttle(func, limit) {
//    let inThrottle;
//    return function () {
//        const args = arguments;
//        const context = this;
//        if (!inThrottle) {
//            func.apply(context, args);
//            inThrottle = true;
//            setTimeout(() => inThrottle = false, limit);
//        }
//    };
//}

//window.detectScrollToEnd = (className, dotNetReference) => {
//    var elements = document.getElementsByClassName(className);
//    if (elements.length > 0) {
//        const throttledScroll = throttle(async (e) => {
//            if (elements[0].scrollHeight - elements[0].scrollTop <= elements[0].clientHeight + 50) { // Margen de 50px
//                await dotNetReference.invokeMethodAsync('LoadMoreData');
//            }
//        }, 500); // Ejecuta la carga cada 500ms como máximo

//        elements[0].onscroll = throttledScroll;
//    }
//};

function resetScroll(selector) {
    var element = document.querySelector(selector);
    if (element) {
        element.scrollTop = 0; // Restablece la posición de desplazamiento vertical
    }
}

