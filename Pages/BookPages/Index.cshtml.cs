using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public IndexModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }
    public IList<Book> Book { get; set; } = default!;
    public BookData BookD { get; set; }
    public int BookID { get; set; }
    public int CategoryID { get; set; }

    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }

    public string CurrentFilter { get; set; }

    public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
    {
        BookD = new BookData();

        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";

        CurrentFilter = searchString;

        BookD.Books = await _context.Book
            .Include(a => a.Autor) //expresie lamba pt b carti alege b carti titlu, ca sa poti accesa atributele obiectului
            .Include(p => p.Publisher) // Include the Publisher navigation property
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (!String.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(s => s.Autor.FirstName.Contains(searchString)
            || s.Autor.LastName.Contains(searchString)
            || s.Title.Contains(searchString));
        }

        if (id != null) 
        {
            BookID = id.Value;
            Book book = BookD.Books
                .Where(i => i.ID == id.Value).Single();
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Title);
                break;
            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Autor.FullName);
                break;
            case "author":
                BookD.Books = BookD.Books.OrderBy(s => s.Autor.FullName);
                break;
            default:
                BookD.Books = BookD.Books.OrderBy(s => s.Title);
                break;
        }
    }
}
