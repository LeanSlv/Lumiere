using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Components
{
    public class FilmsSelect : ViewComponent
    {
        private readonly IFilmRepository _filmRepository;

        public FilmsSelect(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Film> films = _filmRepository.GetAll().ToList();

            List<FilmsSelectViewModel> filmsSelects = new List<FilmsSelectViewModel>();
            foreach (Film film in films)
            {
                filmsSelects.Add(new FilmsSelectViewModel
                {
                    FilmId = film.Id,
                    FilmName = film.Name
                });
            }

            return View("FilmsSelect", filmsSelects);
        }
    }
}
