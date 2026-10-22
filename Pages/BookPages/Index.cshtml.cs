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

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        BookD = new BookData(); 

        BookD.Books = await _context.Book
            .Include(a => a.Autor) //expresie lamba pt b carti alege b carti titlu, ca sa poti accesa atributele obiectului
            .Include(p => p.Publisher) // Include the Publisher navigation property
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (id != null) 
        {
            BookID = id.Value;
            Book book = BookD.Books
                .Where(i => i.ID == id.Value).Single();
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }
    }
}
