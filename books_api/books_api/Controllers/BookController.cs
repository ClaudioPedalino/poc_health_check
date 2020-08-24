using books_api.Entities;
using books_api.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace books_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        string[] Genres = { "Novel", "Fantasy", "Adventure", "Romance", "Contemporary", "Dystopian", "Mystery", "Horror", "Thriller" };
        Random random = new Random();
        const int BOOKS_QUANTITY = 30;

        // GET: api/<BookController>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            IList<Book> books = new List<Book>();

            for (int i = 0; i < BOOKS_QUANTITY; i++)
                books.Add(GenerateBook.GenerateNewBook(random, Genres));

            return books;
        }


        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public Book Get(int id)
            => GenerateBook.GenerateNewBook(random, Genres);

    }
}
