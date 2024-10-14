

window.scrollToSection = function (sectionId) {
    const element = document.getElementById(sectionId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
    }
}

window.initializeSplide = () => {
    const mainSlider = document.getElementById('main-slider');
    const thumbnailSlider = document.getElementById('thumbnail-slider');

    if (mainSlider && thumbnailSlider) {
        console.log("Inicializando Splide...");

        var main = new Splide(mainSlider, {
            type: 'fade',
            heightRatio: 0.5,
            pagination: false,
            arrows: false,
            cover: true,
        });

        var thumbnails = new Splide(thumbnailSlider, {
            rewind: true,
            fixedWidth: 104,
            fixedHeight: 58,
            isNavigation: true,
            gap: 10,
            focus: 'center',
            pagination: false,
            cover: true,
            dragMinThreshold: {
                mouse: 4,
                touch: 10,
            },
            breakpoints: {
                640: {
                    fixedWidth: 66,
                    fixedHeight: 38,
                },
            },
        });

        main.sync(thumbnails);
        main.mount();
        thumbnails.mount();

        console.log("Splide inicializado");
    }
};


