using BaiTest2023.Models;

namespace BaiTest2023.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Tile { get; set; }
        public string Description { get; set; }
        public int Authorld { get; set; }
        public DateTime PulblishDate { get; set; }
        public Double Price { get; set; }
    }
}
