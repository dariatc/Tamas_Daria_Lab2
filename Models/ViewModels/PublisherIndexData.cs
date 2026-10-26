namespace Tamas_Daria_Lab2.Models.ViewModels
{
    public class PublisherIndexData //pt a adauga proprietati la viewmodel, si pt a putea face un view care sa contina ambele tabele
    {
        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
