using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;
using Tamas_Daria_Lab2.Models.ViewModels;

namespace Tamas_Daria_Lab2.Pages.PublisherPages;

public class IndexModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public IndexModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; } = default!;

    public PublisherIndexData PublisherData { get; set; }
    public int PublisherID { get; set; }
    public int BookID { get; set; }

    public async Task OnGetAsync(int? id, int? bookID)
    {
        PublisherData = new PublisherIndexData();
        PublisherData.Publishers = await _context.Publisher
            .Include(p => p.Books).ThenInclude(c => c.Autor) // Include the Books navigation property
            .OrderBy(i => i.PublisherName)
            .ToListAsync();

        if (id != null)
        {
            PublisherID = id.Value; //dai ce id primeste in url
            Publisher publisher = PublisherData.Publishers
                .Where(i => i.ID == id.Value).Single(); //where returneaza colectie si cu single filtrezi
            PublisherData.Books = publisher.Books;
        }
    }
}
