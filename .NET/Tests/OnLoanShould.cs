using Microsoft.EntityFrameworkCore;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace Tests
{
    public class OnLoanShould
    {
        public LibraryContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<LibraryContext>();
            builder.UseInMemoryDatabase(databaseName: "LibraryDbInMemory");

            var dbContextOptions = builder.Options;
            var context = new LibraryContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public void ReturnBook_ShouldFineUser_WhenLate()
        {
            // Arrange
            var context = CreateContext();
            var onLoanRepository = new OnLoanRepository(context);

            var borrowerId = Guid.NewGuid();
            var borrower = new Borrower { Id = borrowerId, Name = "Test Borrower", EmailAddress = "test@gmail.com", Fine = 0 };
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Name = "Sample Book", ISBN = "123", Format = BookFormat.Paperback };

            var catalogue = new BookStock
            {
                Book = book,
                OnLoanTo = borrower,
                LoanEndDate = DateTime.UtcNow.AddDays(-2)
            };

            context.Borrowers.Add(borrower);
            context.Books.Add(book);
            context.Catalogue.Add(catalogue);

            context.SaveChanges();

            // Act
            var result = onLoanRepository.ReturnBook(book.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(500, context.Borrowers.Find(borrowerId)?.Fine);
        }
    }
}