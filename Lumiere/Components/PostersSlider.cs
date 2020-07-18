using Lumiere.Models;
using Lumiere.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Components
{
    public class PostersSlider : ViewComponent
    {
        private readonly ISeanceRepository _seanceRepository;
        private readonly IFilmRepository _filmRepository;

        public PostersSlider(ISeanceRepository seanceRepository, IFilmRepository filmRepository)
        {
            _seanceRepository = seanceRepository;
            _filmRepository = filmRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<Guid, string> filmPosters = new Dictionary<Guid, string>();

            List<FilmSeance> seances = _seanceRepository.GetAll().ToList();
            foreach(FilmSeance seance in seances)
            {
                if(seance.Date == DateTime.Today)
                {
                    Film film = await _filmRepository.GetByIdAsync(seance.FilmId);
                    if (film == null)
                        continue;

                    if (filmPosters.ContainsKey(film.Id))
                        continue;

                    if(film.Posters.Count >= 2)
                        filmPosters.Add(film.Id, film.Posters[1].Url);
                }
            }

            return View("PostersSlider", filmPosters);
        }
    }
}
