using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tamas_Daria_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Display(Name = "Book Title")]
        public string Title { get; set; }

        public int AutorID { get; set; }
        public Authors? Autor { get; set; } //trb sa fie nullable pt ca in create nu dai un intreg obiect de autor

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }

        public int? PublisherID { get; set; }

        public Publisher? Publisher { get; set; } //navigation property, ? ii pt ca poate fi null, la obiect poate lipsi, nu trb incarcat neaparat
        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}
