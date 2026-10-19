using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tamas_Daria_Lab2.Models
{
    public class BookCategoriesPageModel: PageModel //construieste o lista cu toate categoriile si pt fiecare retina daca cartea curenra e bifata sau nu, pt many-to-many 
    {
        public List<AssignedCategoryData> AssignedCategoryDataList; 

        public void PopulateAssignedCategoryData(Tamas_Daria_Lab2Context context, Book book) //citeste entitatile Category si populeaza lista
        {
            var allCategories = context.Category; //ia din DB
            var bookCategories = new HashSet<int>(book.BookCategories.Select(c => c.CategoryID));
            //hashset = o colectie cu cautare foarte rapida
            //book.BookCategories = book e cartea primit in paranmetru + tabela de legatura cu ce categorii are cartea
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData //clasa ajutatoare pt atribute
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = bookCategories.Contains(cat.ID)
                });
            }
        }

        public void UpdateBookCategories(Tamas_Daria_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            if(selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>
                (bookToUpdate.BookCategories.Select(c => c.CategoryID));
            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if(!bookCategories.Contains(cat.ID))
                    {
                        bookToUpdate.BookCategories.Add(
                            new BookCategory
                            {
                                BookID = bookToUpdate.ID,
                                CategoryID = cat.ID
                            })
                    }
                }
                else
                {
                    if (bookCategories.Contains(cat.ID))
                    {
                        BookCategory bookToRemove
                            = bookToUpdate
                               .BookCategories
                               .SingleOrDefault(I => I.CategoryID == cat.ID);
                        context.Remove(bookToRemove)
                    }
                }
            }
        }
    }
}
