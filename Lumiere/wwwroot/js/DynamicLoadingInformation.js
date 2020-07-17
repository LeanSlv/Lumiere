$(function () {
  loadDates($('#film'));
});

// ������������ ���������� ��� � ����������� �� ������.
function loadDates(select_film) {
  var filmId = select_film.val();

  $.ajax({
    type: 'GET',
    url: '/Booking/DatesList?filmId=' + filmId,
    success: function (data) {
      $('#date').replaceWith(data);
      loadTimes($('#date'));
    }
  });
}

// ������������ ���������� ������� ������� � ����������� �� ����.
function loadTimes(select_date) {
  var filmId = $('#film').val();
  var date = select_date.val();

  $.ajax({
    type: 'GET',
    url: '/Booking/TimesList?filmId=' + filmId + '&date=' + date,
    success: function (data) {
      $('#time').replaceWith(data);
      loadRoom($('#time'));
    }
  });
}

// ������������ ���������� ������ ���� � ����������� �� ������� ������. 
function loadRoom(select_time) {
  var filmId = $('#film').val();
  var date = $('#date').val();
  var time = select_time.val();

  $.ajax({
    type: 'GET',
    url: '/Booking/RoomNumbersList?filmId=' + filmId + '&date=' + date + '&time=' + time,
    success: function (data) {
      $('#roomNumber').replaceWith(data);
      loadPrice($('#roomNumber'));
    }
  });
}

// ������������ ���������� ���� ������ � ����������� �� ������ ����. 
function loadPrice(select_room) {
  var filmId = $('#film').val();
  var date = $('#date').val();
  var time = $('#time').val();
  var room = select_room.val();

  var seance = {
    FilmId: filmId,
    Date: date,
    Time: time,
    RoomNumber: room 
  }

  $.ajax({
    type: 'POST',
    url: '/Booking/LoadPrice',
    data: seance,
    dataType: "json",
    success: function (data) {
      $('#price').text(data);
        price = data;
    }
  });
}