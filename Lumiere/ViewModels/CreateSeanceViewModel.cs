using System;

namespace Lumiere.ViewModels
{
    public class CreateSeanceViewModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Price { get; set; }
        public int RoomNumber { get; set; }
        public Guid FilmId { get; set; }
    }
}
