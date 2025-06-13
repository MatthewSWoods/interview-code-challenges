﻿using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IBorrowerRepository
    {
        public List<Borrower> GetBorrowers();

        public Guid AddBorrower(Borrower borrower);

        public decimal FineBorrower(Borrower borrower, decimal fineAmount);
    }
}
