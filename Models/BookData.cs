namespace Tamas_Daria_Lab2.Models
{
    public class BookData //clasa ajutatoare container pt a accesa toate atributele carti + categorii
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<BookCategory> BookCategories { get; set; }
    }
}
