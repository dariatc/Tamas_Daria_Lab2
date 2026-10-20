using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tamas_Daria_Lab2.Models;
using System.Reflection.Metadata.Ecma335;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel //am creat un pagemodel costum ca sa poata mosteni functiile noi
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
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id); //aduc carte din DB
        if (book is null)
        {
            return NotFound();
        }
        Book = book;

        //ca sa obtinem informatiile pt checkboxes
        PopulateAssignedCategoryData(_context, Book);
        IncarcaListe(); //e transmis numa idul care e aratat ca nume prin SelectList, deci trebuie sa incarcam lista de autori si editam cartea cu idul respectiv
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
    {
      
        if (id == null)
        {
            return NotFound();
        }

        var bookToUpdate = await _context.Book
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories).ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);
        if(bookToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title, i => i.Autor,
            i => i.Price, i => i.PublishingDate, i => i.PublisherID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        } // updatebookcategories pt a aplica info la checkboxuri

        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData( _context, bookToUpdate);
        return Page();
    }

    //private bool BookExists(int id)
    //{
    //    return _context.Book.Any(e => e.ID == id);
    //}
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
