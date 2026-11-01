using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;
using Tamas_Daria_Lab2.Models.ViewModels;

namespace Tamas_Daria_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public IndexModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public PublisherIndexData CategoryData { get; set; }
    public int CategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        CategoryData = new PublisherIndexData();
        CategoryData.Categories = await _context.Category
            .Include(i => i.BookCategories).ThenInclude(b => b.Book).ThenInclude(a => a.Autor)
            .ToListAsync();

        if (id != null)
        {
            CategoryID = id.Value;
            Category category = CategoryData.Categories
                .Where(i => i.ID == id.Value).Single();
            CategoryData.Books = category.BookCategories.Select(s => s.Book);
        }

    }
}
