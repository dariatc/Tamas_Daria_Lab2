using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.PublisherPages;

public class IndexModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public IndexModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Publisher = await _context.Publisher
            .Include(p => p.Books) // Include the Books navigation property
            .ToListAsync();
    }
}
