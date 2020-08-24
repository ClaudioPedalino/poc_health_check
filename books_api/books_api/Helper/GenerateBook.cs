using Bogus;
using books_api.Entities;
using System;

namespace books_api.Helper
{
    public static class GenerateBook
    {
        public static Faker<Book> GenerateNewBook(Random random, string[] Genres)
            => new Faker<Book>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Author, x => x.Name.FullName())
                .RuleFor(x => x.Title, x => x.Commerce.ProductName())
                .RuleFor(x => x.Publisher, x => x.Company.CompanyName())
                .RuleFor(x => x.Genre, Genres[random.Next(Genres.Length)]);

    }
}
