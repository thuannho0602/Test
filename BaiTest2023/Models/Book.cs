using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTest2023.Models
{
    [Table("Book")]
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Tile { get; set; }
        public string Description { get; set; }
        public int Authorld { get; set; }
        // foreign key 
        public virtual Author Author { get; set; }
        public DateTime PulblishDate { get; set; }
        public Double Price { get; set; }

    }
}
