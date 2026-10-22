using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class DetailsModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;
    public DetailsModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(a => a.Autor) // pt ca trb sa accesam si datele autorului
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .AsNoTracking() //e doar pt citire
            .FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        else
        {
            Book = book;
        }

        return Page();
    }
}
