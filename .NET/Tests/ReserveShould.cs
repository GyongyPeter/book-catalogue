using Microsoft.EntityFrameworkCore;
using OneBeyondApi;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace Tests
{
    public class ReserveShould
    {
        public LibraryContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<LibraryContext>();
            builder.UseInMemoryDatabase(databaseName: "LibraryDbInMemory");

            var dbContextOptions = builder.Options;
            var context = new LibraryContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            SeedData.SetInitialData(context);

            return context;
        }

        [Fact]
        public void ReserveBook_ShouldReturnSuccess_WhenBookIsLoanedAndAvailable()
        {
            // Arrange
            var reserveRepository = new ReserveRepository(CreateContext());
            var bookId = new Guid("954c9369-545d-4ee8-8769-5e1668f58874");
            var borrowerId = new Guid("954c9369-545d-4ee8-8769-5e1668f58878");

            // Act
            var result = reserveRepository.ReserveBook(bookId, borrowerId);

            // Assert
            Assert.Equal(ReservationResult.Success, result);
        }

        [Fact]
        public void ReserveBook_ShouldReturnBorrowerNotFound()
        {
            // Arrange
            var reserveRepository = new ReserveRepository(CreateContext());
            var bookId = new Guid("954c9369-545d-4ee8-8769-5e1668f58874");
            var borrowerId = new Guid("00000000-0000-0000-0000-000000000001");

            // Act
            var result = reserveRepository.ReserveBook(bookId, borrowerId);

            // Assert
            Assert.Equal(ReservationResult.BorrowerNotFound, result);
        }

        [Fact]
        public void ReserveBook_ShouldReturnBookNotFound()
        {
            // Arrange
            var reserveRepository = new ReserveRepository(CreateContext());
            var bookId = new Guid("00000000-0000-0000-0000-000000000001");
            var borrowerId = new Guid("954c9369-545d-4ee8-8769-5e1668f58878");

            // Act
            var result = reserveRepository.ReserveBook(bookId, borrowerId);

            // Assert
            Assert.Equal(ReservationResult.BookNotFound, result);
        }
    }
}