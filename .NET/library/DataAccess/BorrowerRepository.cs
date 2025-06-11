using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class BorrowerRepository : IBorrowerRepository
    {
        public BorrowerRepository()
        {
        }
        public List<Borrower> GetBorrowers()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Borrowers
                    .ToList();
                return list;
            }
        }

        public Guid AddBorrower(Borrower borrower)
        {
            using (var context = new LibraryContext())
            {
                context.Borrowers.Add(borrower);
                context.SaveChanges();
                return borrower.Id;
            }
        }

        public decimal FineBorrower(Borrower borrower, decimal amount)
        {
            using (var context = new LibraryContext())
            {
                var borrowerToFine = context.Borrowers.Where(b => b.Id == borrower.Id).First();
                if (borrowerToFine is not null)
                {
                    borrowerToFine.OutstandingFines = borrowerToFine.OutstandingFines + amount;
                    context.SaveChanges();

                    return borrowerToFine.OutstandingFines;
                }
            }

            return 0;
        }
    }
}
