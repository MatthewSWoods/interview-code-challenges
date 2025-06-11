using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public interface ILoanService
    {
        public List<BorrowerLoans> GetOnLoanDetails();
        public Task ReturnBook(string bookName, string borrowerName);
        public Task LevyFine(BookStock returnedBookStock);
    }
}
