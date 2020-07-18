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

        public PostersSlider(ISeanceRepository seanceRepository)
        {
            _seanceRepository = seanceRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<Guid, string> filmPosters = new Dictionary<Guid, string>();

            List<FilmSeance> seances = _seanceRepository.GetAll().ToList();
            foreach(FilmSeance seance in seances)
            {
                if(seance.Date == DateTime.Today)
                {
                    if(seance.Film.Posters.Count >= 2)
                        filmPosters.Add(seance.Film.Id, seance.Film.Posters[1].Url);
                }
            }

            return View("PostersSlider", filmPosters);
        }
    }
}
