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

        public Task<Dictionary<Borrower, List<string>>> GetOnLoanDetails()
        {
            var onLoan = new Dictionary<Borrower, List<string>>();
            var bookStock = _catalogueRepository.GetCatalogue();
            
            if (bookStock is null)
            {
                _logger.LogWarning("No books in catalogue");
                return Task.FromResult(onLoan);
            }

            var booksOnLoan = bookStock.Where(x => x.OnLoanTo is not null);

            if (booksOnLoan.Count() == 0)
            {
                _logger.LogInformation("No books are on loan");
                return Task.FromResult(onLoan);
            }

            onLoan = bookStock
                .Where(x => x.OnLoanTo is not null)
                .GroupBy(x => x.OnLoanTo!)
                .ToDictionary(
                    g => g.Key!,
                    g => g.Select(bol => bol.Book?.Name!)
                          .Where(name => !string.IsNullOrEmpty(name))
                          .ToList()
                );

            return Task.FromResult(onLoan);
        }
    }
}
