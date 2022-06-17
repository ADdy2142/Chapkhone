$(document).ready(function () {
    // Toggle Navbar Items
    var navbarToggleButton = $(".navbar-toggle-button");
    $(navbarToggleButton).click(function (e) {
        e.preventDefault();
        var navbarItems = $(".site-navbar-items");
        $(navbarItems).toggleClass("active");
    });

    // Enable Bootstrap Tooltip
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Swiper
    var mainSwiper = new Swiper(".main-swiper", {
        slidesPerView: 1,
        spaceBetween: 0,
        loop: true,
        effect: "fade",
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
            dynamicBullets: true,
        }
    });

    var newProductsSwiper = new Swiper(".new-products-swiper", {
        loop: false,
        autoplay: {
            delay: 7500,
            disableOnInteraction: false,
        },
        breakpoints: {
            0: {
                slidesPerView: 1,
                spaceBetween: 20,
                centeredSlides: false,
            },
            325: {
                slidesPerView: 2,
                spaceBetween: 20,
                centeredSlides: false,
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 25,
                centeredSlides: false,
            },
            992: {
                slidesPerView: 4,
                spaceBetween: 30,
                centeredSlides: false,
            }
        },
    });

    var ourCustomersSwiper = new Swiper(".our-customers-swiper", {
        loop: false,
        autoplay: {
            delay: 7500,
            disableOnInteraction: false,
        },
        breakpoints: {
            0: {
                slidesPerView: 3,
                spaceBetween: 0,
                centeredSlides: false,
            },
            768: {
                slidesPerView: 7,
                spaceBetween: 29,
                centeredSlides: false,
            },
            992: {
                slidesPerView: 7,
                spaceBetween: 34,
                centeredSlides: false,
            }
        },
    });

    var productDetailsImagesSwiper = new Swiper(".product-details-images-swiper", {
        slidesPerView: 1,
        spaceBetween: 20,
        centeredSlides: true,
        loop: false,
        autoplay: {
            delay: 3000,
            disableOnInteraction: false,
        }
    });

    // Load Customer Design Images
    $(".customer-design-file-input").change(function (e) {
        var files = e.target.files;
        var container = $(".customer-design-image-container");
        $(container).empty();
        if (files) {
            for (let i = 0; i < files.length; i++) {
                var element = files[i];
                var imageWrap = document.createElement("div");
                $(imageWrap).addClass("customer-design-image-wrap");
                var image = document.createElement("img");
                $(image).addClass("customer-design-image");
                image.src = URL.createObjectURL(element);
                $(imageWrap).append(image);
                $(container).append(imageWrap);
            }
        }
    });

    // Submit Form When User Image Changed
    $(".change-image-input").change(function (e) {
        $("#change-image-form").submit();
    });

    $(".custom-input-group .unit-price-input").change(function () {
        let unitPriceInputs = $(".custom-input-group .unit-price-input");
        let totalPrice = 0;
        let unitPriceType = $(this).data("unit-price-type");
        let unitPrice = $(this).data("unit-price");
        switch (unitPriceType) {
            case 0:
                let width = $(unitPriceInputs[0]).val() / 100;
                let height = $(unitPriceInputs[1]).val() / 100;
                let m2 = width * height;
                totalPrice = m2 * unitPrice;
                totalPrice = separate(parseInt(totalPrice, 10));
                $("#total-design-price").html(totalPrice + " تومان");
                break;

            case 1:
                let weight = $(unitPriceInputs[0]).val();
                weight = weight / 1000;
                totalPrice = weight * unitPrice;
                totalPrice = separate(parseInt(totalPrice, 10));
                $("#total-design-price").html(totalPrice + " تومان");
                break;

            case 2:
                let qty = $(unitPriceInputs[0]).val();
                totalPrice = qty * unitPrice;
                totalPrice = separate(parseInt(totalPrice, 10));
                $("#total-design-price").html(totalPrice + " تومان");
                break;
        }
    });

    function separate(Number) {
        debugger;
        Number += '';
        Number = Number.replace(',', '');
        x = Number.split('.');
        y = x[0];
        z = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(y))
            y = y.replace(rgx, '$1' + ',' + '$2');
        return y + z;
    }
});