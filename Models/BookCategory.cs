namespace Tamas_Daria_Lab2.Models
{
    public class BookCategory //normalizarea a 3-a a unei relatii many-to-many
    // se aduc PKs din tabele ca sa elimine datele redundante
    {
        public int ID { get; set; }
        public int BookID { get; set; } //FK
        public Book Book { get; set; } //navigation property
        public int CategoryID { get; set; } //FK
        public Category Category { get; set; } //navigation property



    }
}
