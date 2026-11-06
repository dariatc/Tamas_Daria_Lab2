using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public EditModel(Tamas_Daria_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
        if (borrowing is null)
        {
            return NotFound();
        }
        Borrowing = borrowing;

        IncarcaListe();
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

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
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

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }

    private async Task IncarcaListe()
    {
        ViewData["MemberID"] = new SelectList( _context.Set<Member>(), "ID", "FullName");
        //trb variabila ca in situatia cu books pt ca selectlist poate primi un singur camp
        var carti = _context.Book
            .Include(a => a.Autor)
            .Select(s => new
            {
                ID = s.ID,
                Descriere = s.Title + " - " + s.Autor.FirstName + " " + s.Autor.LastName + " - " + s.PublishingDate.ToString("yyyy")
            });
        ViewData["BookID"] = new SelectList( carti , "ID", "Descriere");
    }
}
