using LibraryManagement.Data.Interfaces;
using LibraryManagement.Data.Model;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        [Route("Book")]
        public IActionResult List(int? authorId, int? borrowerId)
        {
            var book = _bookRepository.GetAllWithAuthor().ToList();

            IEnumerable<Book> books;

            ViewBag.AuthorId = authorId;

            if (borrowerId != null)
            {
                books = _bookRepository.FindWithAuthorAndBorrower(x => x.BorrowerId == borrowerId);
                return CheckBooksCount(books);
            }

            if (authorId == null)
            {
                books = _bookRepository.GetAllWithAuthor();
                return CheckBooksCount(books);
            }
            else
            {
                var author = _authorRepository.GetWithBooks((int)authorId);

                if (author.Books == null || author.Books.Count == 0)
                    return View("EmptyAuthor", author);
            }

            books = _bookRepository.FindWithAuthor(a => a.Author.AuthorId == authorId);

            return CheckBooksCount(books);
        }

        private IActionResult CheckBooksCount(IEnumerable<Book> books)
        {
            if (books == null || books.ToList().Count == 0)
            {
                return View("Empty");
            }
            else
            {
                return View(books);
            }
        }

        public IActionResult Update(int id)
        {
            Book book = _bookRepository.FindWithAuthor(a => a.BookId == id).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            var bookVM = new BookEditViewModel
            {
                Book = book,
                Authors = _authorRepository.GetAll()
            };

            return View(bookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BookEditViewModel bookVM)
        {
            if (!ModelState.IsValid)
            {
                bookVM.Authors = _authorRepository.GetAll();
                return View(bookVM);
            }

            _bookRepository.Update(bookVM.Book);

            return RedirectToAction("List");
        }

        public IActionResult Create(int? authorId)
        {
            Book book = new Book();

            if (authorId != null)
            {
                book.AuthorId = (int)authorId;
            }

            var bookVM = new BookEditViewModel
            {
                Authors = _authorRepository.GetAll(),
                Book = book
            };

            return View(bookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookEditViewModel bookVM)
        {
            if (!ModelState.IsValid)
            {
                bookVM.Authors = _authorRepository.GetAll();
                return View(bookVM);
            }

            _bookRepository.Create(bookVM.Book);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id, int? authorId)
        {
            var book = _bookRepository.GetById(id);

            _bookRepository.Delete(book);

            return RedirectToAction("List", new { authorId = authorId });
        }
    }
}
