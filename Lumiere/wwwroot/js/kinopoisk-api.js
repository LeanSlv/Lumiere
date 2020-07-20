document.addEventListener("DOMContentLoaded", () => {
    var kinopoisk_id = $('#kinopoisk_id').val();
    if (kinopoisk_id !== null || kinopoisk_id !== "") {
        getRaiting(kinopoisk_id);
    }
});


function getRaiting(id) {
    var request = new XMLHttpRequest();

    request.open("GET", "https://kinopoiskapiunofficial.tech/api/v2.1/films/" + id + "?append_to_response=RATING", false);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.setRequestHeader("X-API-KEY", "e961fbe1-05d8-4830-81b6-8f6eed118564");


    request.send();

    if (request.status == 200) {
        data = JSON.parse(request.responseText);
        let rating = data.rating.rating;
        document.getElementById("rating").innerHTML = rating;

        $('span[name ="star"]').each(function (index) {
            if (index < Number(rating)) {
                $(this).addClass("active");
            }
        });
    }
}