using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly LibraryContext _context;

        public BorrowerRepository(LibraryContext context)
        {
            _context = context;
        }
        public List<Borrower> GetBorrowers()
        {
            var list = _context.Borrowers
                .ToList();
            return list;
        }

        public Guid AddBorrower(Borrower borrower)
        {
            _context.Borrowers.Add(borrower);
            _context.SaveChanges();
            return borrower.Id;
        }
    }
}
