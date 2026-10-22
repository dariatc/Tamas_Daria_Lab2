using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tamas_Daria_Lab2.Models;

namespace Tamas_Daria_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Tamas_Daria_Lab2Context _context;

    public CreateModel(Tamas_Daria_Lab2Context context)
    {
        _context = context; 
    }

    public IActionResult OnGet()
    {
        IncarcaListe(); //daca nu e valid, reincarca listele inainte sa returneze pag ca sa aiba dropdow mneu optiuni

        var book = new Book();
        book.BookCategories = new List<BookCategory>();

        PopulateAssignedCategoryData(_context, book);
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(string[] selectedCategories) //programare asincrona
    {
        
        var newBook = new Book();
        if( selectedCategories != null)
        {
            newBook.BookCategories = new List<BookCategory>();
            foreach (var cat in selectedCategories)
            {
                var catToAdd = new BookCategory
                {
                    CategoryID = int.Parse(cat) //face din string in int ca parametrul e string[]
                };
                newBook.BookCategories.Add(catToAdd);
            }
        }

        Book.BookCategories = newBook.BookCategories; //muti din variabila ajutatoare in aia oficiala
        _context.Book.Add(Book);
        await _context.SaveChangesAsync(); //sa schimbe si in context/DB
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
