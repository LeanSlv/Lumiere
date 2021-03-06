﻿using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Controllers
{
    public class SeanceController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ISeanceRepository _seanceRepository;

        public SeanceController(IFilmRepository filmRepository, ISeanceRepository seanceRepository)
        {
            _filmRepository = filmRepository;
            _seanceRepository = seanceRepository;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateSeanceViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            Film film = await _filmRepository.GetByIdAsync(model.FilmId);
            if(film == null)
            {
                ModelState.AddModelError(string.Empty, "Фильм не найден.");
                return PartialView(model);
            }

            FilmSeance seance = new FilmSeance
            {
                Date = model.Date,
                Time = model.Time,
                Price = model.Price,
                FilmId = model.FilmId,
                RoomNumber = model.RoomNumber,
                Format = model.Format
            };

            await _seanceRepository.CreateAsync(seance);

            film.Seances.Add(seance);
            await _filmRepository.UpdateAsync(film);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            FilmSeance seance = await _seanceRepository.GetByIdAsync(id);
            if (seance == null)
                return NotFound();

            await _seanceRepository.DeleteAsync(seance);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task DeletingPastSeances()
        {
            List<FilmSeance> seancesToDelete = _seanceRepository.GetAll().ToList();
            if (seancesToDelete == null)
                return;

            foreach(FilmSeance seance in seancesToDelete)
            {
                if (seance.Date < DateTime.Today)
                {
                    await _seanceRepository.DeleteAsync(seance);
                }
                else if (seance.Date == DateTime.Today && seance.Time <= DateTime.Now.TimeOfDay)
                {
                    await _seanceRepository.DeleteAsync(seance);
                }
            }
        }
    }
}