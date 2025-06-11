using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

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
    }
}
