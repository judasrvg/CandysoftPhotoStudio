

window.scrollToSection = function (sectionId) {
    const element = document.getElementById(sectionId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
    }
}

document.addEventListener('DOMContentLoaded', (event) => {
    window.initializeCarousel = () => {
        var nextBtn = document.querySelector('.next');
        var prevBtn = document.querySelector('.prev');
        var carousel = document.querySelector('.carousel');
        var list = document.querySelector('.list');
        var items = document.querySelectorAll('.item');
        var runningTimeElement = document.querySelector('.carousel .timeRunning'); // Asegurarse de que el elemento existe

        if (!runningTimeElement) {
            return;
        }

        let timeRunning = 7000; // Duración de la animación en milisegundos
        let timeAutoNext = 7000;

        let runTimeOut;
        let runNextAuto;

        // Función que mueve el carrusel hacia adelante o hacia atrás
        function showSlider(type) {
            let sliderItemsDom = document.querySelectorAll('.carousel .list .item');
            if (type === 'next') {
                list.appendChild(sliderItemsDom[0]);
                carousel.classList.add('next');
            } else {
                list.prepend(sliderItemsDom[sliderItemsDom.length - 1]);
                carousel.classList.add('prev');
            }

            // Remover la clase después de la animación
            clearTimeout(runTimeOut);
            runTimeOut = setTimeout(() => {
                carousel.classList.remove('next');
                carousel.classList.remove('prev');
            }, timeRunning);

            // Resetear la animación de tiempo
            runningTimeElement.style.animation = 'none';
            runningTimeElement.offsetHeight;  // Trigger reflow
            runningTimeElement.style.animation = null;
            runningTimeElement.style.animation = 'runningTime 7s linear 1 forwards';

            // Programar el siguiente cambio automático
            clearTimeout(runNextAuto);
            runNextAuto = setTimeout(() => {
                nextBtn.click();
            }, timeAutoNext);
        }

        // Asignar eventos a los botones
        nextBtn.addEventListener('click', () => showSlider('next'));
        prevBtn.addEventListener('click', () => showSlider('prev'));

        // Iniciar la primera animación
        runningTimeElement.style.animation = 'none';
        runningTimeElement.offsetHeight;  // Trigger reflow
        runningTimeElement.style.animation = null;
        runningTimeElement.style.animation = 'runningTime 7s linear 1 forwards';

        // Iniciar el desplazamiento automático
        runNextAuto = setTimeout(() => {
            nextBtn.click();
        }, timeAutoNext);
    };

    // Inicializa el carrusel
    window.initializeCarousel();
});




