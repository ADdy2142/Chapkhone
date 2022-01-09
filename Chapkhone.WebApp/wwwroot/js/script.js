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
        loop: true,
        autoplay: {
            delay: 7500,
            disableOnInteraction: false,
        },
        breakpoints: {
            0: {
                slidesPerView: 1,
                spaceBetween: 20,
                centeredSlides: true,
            },
            325: {
                slidesPerView: 2,
                spaceBetween: 20,
                centeredSlides: true,
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
        loop: true,
        autoplay: {
            delay: 7500,
            disableOnInteraction: false,
        },
        breakpoints: {
            0: {
                slidesPerView: 3,
                spaceBetween: 0,
                centeredSlides: true,
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
        spaceBetween: 10,
        centeredSlides: true,
        loop: true,
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

    var prevSortItem = $(".sort-content .sort-list-items .sort-list-item .button.active");

    // Get Products by Sorting
    $(".sort-list-item .button").click(function (e) {
        e.preventDefault();
        debugger;
        var currentSortItem = $(this);
        if ($(prevSortItem).html() === $(currentSortItem).html()) {
            return;
        } else {
            $(prevSortItem).removeClass("active");
            prevSortItem = currentSortItem;
            $(prevSortItem).addClass("active");
        }

        var sortingBy;
        switch ($(this).html()) {
            case "جدیدترین":
                sortingBy = 0;
                break;

            case "قدیمی ترین":
                sortingBy = 1;
                break;

            case "گرانترین":
                sortingBy = 2;
                break;

            case "ارزانترین":
                sortingBy = 3;
                break;

            default:
                break;
        }

        var productsContainer = $("#products-container");
        $.get("ajax/test.html", function (data) {
            $(productsContainer).empty();
            $(productsContainer).html(data);
        });
    });

    // Submit Form When User Image Changed
    $(".change-image-input").change(function (e) {
        $("#change-image-form").submit();
    });
});