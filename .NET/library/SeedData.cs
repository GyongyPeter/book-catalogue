using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi
{
    public class SeedData
    {
        public static void SetInitialData(LibraryContext context)
        {
            var ernestMonkjack = new Author
            {
                Name = "Ernest Monkjack"
            };
            var sarahKennedy = new Author
            {
                Name = "Sarah Kennedy"
            };
            var margaretJones = new Author
            {
                Name = "Margaret Jones"
            };
            var brandonSanderson = new Author
            {
                Name = "Brandon Sanderson"
            };

            var clayBook = new Book
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58874"),
                Name = "The Importance of Clay",
                Format = BookFormat.Paperback,
                Author = ernestMonkjack,
                ISBN = "1305718181"
            };

            var agileBook = new Book
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58875"),
                Name = "Agile Project Management - A Primer",
                Format = BookFormat.Hardback,
                Author = sarahKennedy,
                ISBN = "1293910102"
            };

            var rustBook = new Book
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58876"),
                Name = "Rust Development Cookbook",
                Format = BookFormat.Paperback,
                Author = margaretJones,
                ISBN = "3134324111"
            };

            var stormLightBook = new Book
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58877"),
                Name = "The Way of Kings",
                Format = BookFormat.Paperback,
                Author = brandonSanderson,
                ISBN = "9780765326355"
            };

            var daveSmith = new Borrower
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58878"),
                Name = "Dave Smith",
                EmailAddress = "dave@smithy.com"
            };

            var lianaJames = new Borrower
            {
                Id = new Guid("954c9369-545d-4ee8-8769-5e1668f58879"),
                Name = "Liana James",
                EmailAddress = "liana@gmail.com"
            };

            var bookOnLoanUntilToday = new BookStock {
                Book = clayBook,
                OnLoanTo = daveSmith,
                LoanEndDate = DateTime.Now.Date
            };

            var bookNotOnLoan = new BookStock
            {
                Book = clayBook,
                OnLoanTo = null,
                LoanEndDate = null
            };

            var bookOnLoanUntilNextWeek = new BookStock
            {
                Book = agileBook,
                OnLoanTo = lianaJames,
                LoanEndDate = DateTime.Now.Date.AddDays(7)
            };

            var bookOnLoanBeforeThisWeek = new BookStock
            {
                Book = stormLightBook,
                OnLoanTo = lianaJames,
                LoanEndDate = DateTime.Now.Date.AddDays(-1)
            };

            var rustBookStock = new BookStock
            {
                Book = rustBook,
                OnLoanTo = null,
                LoanEndDate = null
            };

            context.Authors.Add(ernestMonkjack);
            context.Authors.Add(sarahKennedy);
            context.Authors.Add(margaretJones);


            context.Books.Add(clayBook);
            context.Books.Add(agileBook);
            context.Books.Add(rustBook);

            context.Borrowers.Add(daveSmith);
            context.Borrowers.Add(lianaJames);

            context.Catalogue.Add(bookOnLoanUntilToday);
            context.Catalogue.Add(bookNotOnLoan);
            context.Catalogue.Add(bookOnLoanUntilNextWeek);
            context.Catalogue.Add(bookOnLoanBeforeThisWeek);
            context.Catalogue.Add(rustBookStock);

            context.SaveChanges();
        }
    }
}
