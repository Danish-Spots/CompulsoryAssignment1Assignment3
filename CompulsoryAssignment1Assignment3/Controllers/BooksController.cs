using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompulsoryAssignment1Assignment1;
using CompulsoryAssignment1Assignment3.StaticLists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.SignalR.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompulsoryAssignment1Assignment3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: <BooksController>
        [HttpGet]
        public List<Book> Get()
        {
            return BooksList.books;
        }

        // GET <BooksController>/0123456789abc
        [HttpGet("{isbn13}")]
        public IActionResult Get(string isbn13)
        {
            Book b = getBook(isbn13);
            if (b == null)
            {
                return NotFound(new { message = "No Book Found Corresponding To The Provided ISBN13" });
            }

            return Ok(b);
        }

        // POST <BooksController>
        [HttpPost]
        public IActionResult Post([FromBody] Book value)
        {
            Book b = getBook(value.ISBN13);
            if (b != null)
            {
                return Conflict(new {message = "ISBN13 Already In Use"});
            }
            BooksList.books.Add(value);
            return CreatedAtAction("Get", new {isbn13 = value.ISBN13}, value);
        }

        // PUT <BooksController>/1234567890abc
        [HttpPut("{isbn13}")]
        public IActionResult Put(string isbn13, [FromBody] Book value)
        {
            if (isbn13 != value.ISBN13)
                return BadRequest(new {message = "Provided ISBN13 Mismatch"});

            Book b = getBook(isbn13);

            if (b != null)
            {
                b.Authour = value.Authour;
                b.PageNumber = value.PageNumber;
                b.Title = value.Title;
            }
            else
                return NotFound(new {message = "No Book Found Corresponding To The Provided ISBN13"});

            return NoContent();
        }

        // DELETE <BooksController>/1234567890abc
        [HttpDelete("{isbn13}")]
        public IActionResult Delete(string isbn13)
        {
            Book b = getBook(isbn13);

            if (b != null)
                BooksList.books.Remove(b);
            else
                return NotFound(new { message = "No Book Found Corresponding To The Provided ISBN13" });
            return Ok(b);
        }

        private Book getBook(string isbn13)
        {
            Book book = BooksList.books.FirstOrDefault(b => b.ISBN13 == isbn13);
            return book;
        }
    }
}
