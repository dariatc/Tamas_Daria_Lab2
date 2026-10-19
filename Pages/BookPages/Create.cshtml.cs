using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public CreateModel(Tamas_Daria_Lab2Context context)
    {
        _context = context; 
    }

    public IActionResult OnGet()
    {
        IncarcaListe(); //daca nu e valid, reincarca listele inainte sa returneze pag ca sa aiba dropdow mneu optiuni
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync() //programare asincrona
    {
        if (!ModelState.IsValid)
        {
            IncarcaListe();
            return Page();
        }

        _context.Book.Add(Book); // ADAUGA TABELUL BOOK IN CONTEXT
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
    
    private void IncarcaListe()
    {
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        var autori = _context.Authors //tabelul 
            .Select(a => new //select din sql, pt fiecare a faci atributu asta
            {
                ID = a.ID, //copiaza id ul din a in campul id
                Nume = a.FirstName + " " + a.LastName
            });
        ViewData["AutorID"] = new SelectList(autori, "ID", "Nume");
    }
}
