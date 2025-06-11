using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;
using System.Text.Json;

namespace OneBeyondApi.DataAccess
{
    public class CatalogueRepository : ICatalogueRepository
    {
        public CatalogueRepository()
        {
        }
        public List<BookStock> GetCatalogue()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo)
                    .ToList();
                return list;
            }
        }

        public List<BookStock> SearchCatalogue(CatalogueSearch search)
        {
            using (var context = new LibraryContext())
            {
                var list = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo)
                    .AsQueryable();

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.Author)) {
                        list = list.Where(x => x.Book.Author.Name.Contains(search.Author));
                    }
                    if (!string.IsNullOrEmpty(search.BookName)) {
                        list = list.Where(x => x.Book.Name.Contains(search.BookName));
                    }
                }
                    
                return list.ToList();
            }
        }

        public BookStock ReturnBookStock(BookStock returnedBookStock)
        {
            using (var context = new LibraryContext())
            {
                try
                {
                    var bookStock = context.Catalogue.First(x => x.Book == returnedBookStock.Book);
                    Console.WriteLine("BOOK STOCK TO RETURN: {0}", JsonSerializer.Serialize(bookStock));
                    if (bookStock is not null)
                    {
                        bookStock.Book = returnedBookStock.Book;
                        bookStock.LoanEndDate = null;
                        bookStock.OnLoanTo = null;
                        var affected = context.SaveChanges();
                        Console.WriteLine($"SaveChanges affected {affected} rows");

                        return bookStock;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving changes: {ex.Message}");
                }
            }

            return returnedBookStock;
        }
    }
}
