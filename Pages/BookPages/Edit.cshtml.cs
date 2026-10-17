using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public EditModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            IncarcaListe();
            return NotFound();
        }

        var book = await _context.Book.FirstOrDefaultAsync(m => m.ID == id); //aduc carte din DB
        if (book is null)
        {
            return NotFound();
        }
        Book = book;
        IncarcaListe(); //e transmis numa idul care e aratat ca nume prin SelectList, deci trebuie sa incarcam lista de autori si editam cartea cu idul respectiv
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
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
