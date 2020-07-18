// Document ready.
$(function () {
    // Initialize Swiper.
    var swiper = new Swiper('.swiper-container', {
        loop: true,
        autoplay: {
            delay: 5000,
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
    });

    // Проверка и удаление сеансов каждую минуту.
    setInterval(deletingPastSeances, 60000);
});

function deletingPastSeances() {
    $.ajax({
        method: "POST",
        url: "Seance/DeletingPastSeances"
    });
}