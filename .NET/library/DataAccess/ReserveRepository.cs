using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class ReserveRepository : IReserveRepository
    {
        private readonly LibraryContext _context;

        public ReserveRepository(LibraryContext context)
        {
            _context = context;
        }

        public DateTime AvailableFrom(Guid bookId)
        {

            var bookStock = _context.Catalogue
                .Include(x => x.Book)
                .FirstOrDefault(x => x.Book.Id == bookId);

            return bookStock.LoanEndDate.Value.AddDays(1); 
        }

        public ReservationResult ReserveBook(Guid bookId, Guid borrowerId)
        {

            var bookStock = _context.Catalogue
                .Include(x => x.Book)
                .Include(x => x.OnLoanTo)
                .Where(x => x.Book.Id == bookId)
                .FirstOrDefault();

            if (bookStock == null)
            {
                return ReservationResult.BookNotFound;
            }

            if (bookStock.Reserved != null)
            {
                return ReservationResult.AlreadyReserved;
            }

            var borrower = _context.Borrowers
                .FirstOrDefault(x => x.Id == borrowerId);

            if (borrower == null)
            {
                return ReservationResult.BorrowerNotFound;
            }

            bookStock.Reserved = borrower;

            _context.SaveChanges();


            return ReservationResult.Success;
        }
    }
}
