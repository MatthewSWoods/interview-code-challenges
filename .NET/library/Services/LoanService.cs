using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneBeyondApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILogger<LoanService> _logger;
        private readonly ICatalogueRepository _catalogueRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        public LoanService(
            ILogger<LoanService> logger,
            ICatalogueRepository catalogueRepository,
            IBorrowerRepository borrowerRepository)
        {
            _logger = logger;
            _catalogueRepository = catalogueRepository;
            _borrowerRepository = borrowerRepository;
        }

        public List<BorrowerLoans> GetOnLoanDetails()
        {
            var onLoan = new List<BorrowerLoans>();
            var bookStock = _catalogueRepository.GetCatalogue();
            
            if (bookStock is null)
            {
                _logger.LogWarning("No books in catalogue");
                return onLoan;
            }

            var booksOnLoan = bookStock.Where(x => x.OnLoanTo is not null);

            if (booksOnLoan.Count() == 0)
            {
                _logger.LogInformation("No books are on loan");
                return onLoan;
            }

            onLoan = booksOnLoan
                .GroupBy(x => x.OnLoanTo!.Id)
                .Select(g =>
                {
                    var borrower = g.First().OnLoanTo!;
                    return new BorrowerLoans
                    {
                        Id = borrower.Id,
                        Name = borrower.Name,
                        EmailAddress = borrower.EmailAddress,
                        BookNames = g.Select(b => b.Book?.Name!)
                                     .Where(name => !string.IsNullOrEmpty(name))
                                     .ToList()
                    };
                })
                .ToList();

            return onLoan;
        }

        public Task ReturnBook(string returnedBookName, string borrowerName)
        {
            var bookSearch = new CatalogueSearch() { BookName = returnedBookName };
            var bookStock = _catalogueRepository.SearchCatalogue(bookSearch);

            if (bookStock.Count() == 0)
            { 
                _logger.LogInformation("Could not find book in catalogue: {0}", returnedBookName);
                return Task.CompletedTask;
            }

            var onLoanDetails = GetOnLoanDetails();
            var returnedBookStock = bookStock
                .Where(x => x.Book.Name == returnedBookName)
                .Where(b => b.OnLoanTo?.Name == borrowerName)
                .FirstOrDefault();

            if (returnedBookStock is null)
            {
                _logger.LogWarning("Unable to match return details to books on Loan");
            }

            _logger.LogInformation("Returning Book: {0}", JsonSerializer.Serialize(returnedBookStock));
            var updatedBookStock = _catalogueRepository.ReturnBookStock(returnedBookStock!);
            _logger.LogInformation("Returned Book: {0}", JsonSerializer.Serialize(updatedBookStock));

            return Task.CompletedTask;

        }
    }
}
