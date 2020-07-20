using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    public class BookingController : Controller
    {
        private readonly IReservedSeatRepository _reservedSeatRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISeanceRepository _seanceRepository;
        private readonly int seatsCountInRow;

        public BookingController(IReservedSeatRepository reservedSeatRepository, IFilmRepository filmRepository, IUserRepository userRepository, ISeanceRepository seanceRepository)
        {
            _reservedSeatRepository = reservedSeatRepository;
            _filmRepository = filmRepository;
            _userRepository = userRepository;
            _seanceRepository = seanceRepository;
            seatsCountInRow = 5;
        }

        [Authorize]
        public IActionResult Index(Guid filmId = default)
        {
            List<Film> films = _filmRepository.GetAll().ToList();

            List<BookingFilmViewModel> bookingFilms = new List<BookingFilmViewModel>();
            foreach(Film film in films)
            {
                bookingFilms.Add(new BookingFilmViewModel {
                    FilmId = film.Id,
                    FilmName = film.Name
                });
            }

            if (filmId != default)
                ViewBag.SelectedFilm = filmId;

            return View(bookingFilms);
        }

        [HttpGet]
        public IActionResult BookingConfirm()
        {
            return View();
        }

        [HttpPost]
        public async Task<int[]> GetReservedSeats(FilmSeance filmSeance)
        {
            if (!ModelState.IsValid)
                return new int[] { };


            Guid seanceId = await _seanceRepository.GetIdBySeance(filmSeance);
            FilmSeance seance = await _seanceRepository.GetByIdAsync(seanceId);
            if (seance == null)
                return new int[] { };

            List<int> seatsNumber = new List<int>();
            foreach(ReservedSeat reservedSeat in seance.ReservedSeats)
            {
                int seatNumber = (reservedSeat.SeatsNumber) + ((reservedSeat.RowNumber - 1) * seatsCountInRow);
                seatsNumber.Add(seatNumber);
            }

            return seatsNumber.ToArray();
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> ReservedSeats(ReservedSeatViewModel reservedSeats)
        {
            if (!ModelState.IsValid)
                return false;

            string userId = await _userRepository.GetCurrentUserId(User);
            if (string.IsNullOrEmpty(userId))
                return false;

            FilmSeance seance = new FilmSeance
            {
                Date = reservedSeats.Date,
                Time = reservedSeats.Time,
                Price = reservedSeats.Price,
                RoomNumber = reservedSeats.RoomNumber,
                FilmId = reservedSeats.FilmId
            };
            Guid seanceId = await _seanceRepository.GetIdBySeance(seance);
            if (seanceId == default)
                return false;

            for (int i = 0; i < reservedSeats.SeatNumbers.Length; i++)
            {
                double row = ((double) reservedSeats.SeatNumbers[i] + 1) / ((double) seatsCountInRow);
                int rowNumber = Convert.ToInt32(Math.Ceiling(row));
                int seatNumber = (reservedSeats.SeatNumbers[i] + 1) - ((rowNumber - 1) * seatsCountInRow);
                ReservedSeat reservedSeat = new ReservedSeat
                {
                    RowNumber = rowNumber,
                    SeatsNumber = seatNumber,
                    UserId = userId,
                    SeanceId = seanceId
                };

                await _reservedSeatRepository.CreateAsync(reservedSeat);
            }

            return true;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(Guid seatId)
        {
            if (seatId == default)
                return NotFound();

            ReservedSeat reservedSeat = await _reservedSeatRepository.GetByIdAsync(seatId);
            if (reservedSeat == null)
                return NotFound();

            await _reservedSeatRepository.DeleteAsync(reservedSeat);

            string userId = await _userRepository.GetCurrentUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Profile", "User", new { id = userId });
        }

        [HttpGet]
        public IActionResult DatesList(Guid filmId)
        {
            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();

            List<DateTime> dates = new List<DateTime>();
            foreach (FilmSeance seance in seances)
            {
                if (dates.Contains(seance.Date))
                    continue;

                dates.Add(seance.Date);
            }

            return PartialView(dates);
        }

        [HttpGet]
        public IActionResult TimesList(Guid filmId, string date)
        {
            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();

            List<TimeSpan> times = new List<TimeSpan>();
            foreach (FilmSeance seance in seances)
            {
                if (seance.Date == DateTime.Parse(date))
                    times.Add(seance.Time);
            }

            return PartialView(times);
        }

        [HttpGet]
        public IActionResult RoomNumbersList(Guid filmId, string date, string time)
        {
            if (filmId == default)
                return PartialView(new List<int>());

            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();
            if (seances == null)
                return PartialView(new List<int>());

            if (!DateTime.TryParse(date, out DateTime seanceDate))
                return PartialView(new List<int>());

            if (!TimeSpan.TryParse(time, out TimeSpan seanceTime))
                return PartialView(new List<int>());

            List<int> roomNumbers = new List<int>();
            foreach (FilmSeance seance in seances)
            {
                if (seance.Date == seanceDate && seance.Time == seanceTime)
                    roomNumbers.Add(seance.RoomNumber);
            }

            return PartialView(roomNumbers);
        }

        [HttpPost]
        public int LoadPrice(FilmSeance filmSeance)
        {
            if (filmSeance.FilmId == default)
                return 0;

            if (filmSeance.Date == default)
                return 0;

            if (filmSeance.Time == default)
                return 0;

            if (filmSeance.RoomNumber < 1 || filmSeance.RoomNumber > 2)
                return 0;

            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmSeance.FilmId).ToList();
            if (seances == null)
                return 0;

            foreach(FilmSeance seance in seances)
            {
                if(seance.Date == filmSeance.Date &&
                    seance.Time == filmSeance.Time &&
                    seance.RoomNumber == filmSeance.RoomNumber &&
                    seance.FilmId == filmSeance.FilmId)
                {
                    return seance.Price;
                }

            }

            return 0;
        }

        [HttpPost]
        public FilmSeance LoadFormat(FilmSeance filmSeance)
        {
            if (!ModelState.IsValid)
                return new FilmSeance();

            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmSeance.FilmId).ToList();
            if (seances == null)
                return new FilmSeance();

            foreach (FilmSeance seance in seances)
            {
                if (seance.Date == filmSeance.Date &&
                    seance.Time == filmSeance.Time &&
                    seance.RoomNumber == filmSeance.RoomNumber &&
                    seance.FilmId == filmSeance.FilmId &&
                    seance.Price == filmSeance.Price)
                {
                    return seance;
                }
            }

            return new FilmSeance();
        } 
    }
}