using Microsoft.EntityFrameworkCore;

public class Tamas_Daria_Lab2Context(DbContextOptions<Tamas_Daria_Lab2Context> options) : DbContext(options)
{
    public DbSet<Tamas_Daria_Lab2.Models.Book> Book { get; set; } = default!;
}
