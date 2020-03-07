using Library.Models.Models;
using Library.Services.Contracts;
using Library.Web.Mapper;
using Library.Web.Models.BookManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Controllers
{
    public class SearchBooksController : Controller
    {
        private readonly IBookManager _bookManager;
        private readonly IAccountManager _accountManager;
        private readonly ILibrarySystem _system;

        public SearchBooksController(IBookManager bookManager, IAccountManager accountManagager, ILibrarySystem system)
        {
            _bookManager = bookManager;
            _accountManager = accountManagager;
            _system = system;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder)
        {
            var allBooks = await _bookManager.GetAllBooksAsync();

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.YearSortParm = string.IsNullOrEmpty(sortOrder) ? "year_desc" : "";
            ViewBag.RatingSortParm = string.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";


            switch (sortOrder)
            {
                case "name_desc":
                    allBooks = allBooks.OrderByDescending(b => b.Author.Name).ToList();
                    break;
                case "title_desc":
                    allBooks = allBooks.OrderByDescending(b => b.Title).ToList();
                    break;
                case "year_desc":
                    allBooks = allBooks.OrderByDescending(b => b.Year).ToList();
                    break;
                case "rating_desc":
                    allBooks = allBooks.OrderByDescending(b => b.Rating).ToList();
                    break;
                default:
                    allBooks = allBooks.OrderBy(b => b.Author.Name).ToList();
                    break;
            }

            var allBooksVM = allBooks.Select(x => x.MapToViewModel());

            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            var searchVM = new SearchViewModel()
            {
                AllBooks = new List<BookViewModel>(),
                BookSearchResults = new List<BookViewModel>()
            };

            await SearchViewModelMapper(searchVM, allBooksVM, user);

            return View(searchVM);
        }

        [HttpPost]
        public async Task<IActionResult> SearchResults(SearchViewModel viewModel/*, string sortOrder*/)
        {
            var bookResult = await _bookManager
                .SearchAsync(viewModel.SearchName.ToLower(), viewModel.ByTitle, viewModel.ByAuthor, viewModel.ByPublisher, viewModel.ByGenre);

            //ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            //ViewBag.YearSortParm = string.IsNullOrEmpty(sortOrder) ? "year_desc" : "";
            //ViewBag.RatingSortParm = string.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";


            //switch (viewModel.SortOrder)
            //{
            //    case "name_desc":
            //        bookResult = bookResult.OrderByDescending(b => b.Author.Name).ToList();
            //        break;
            //    case "title_desc":
            //        bookResult = bookResult.OrderByDescending(b => b.Title).ToList();
            //        break;
            //    case "year_desc":
            //        bookResult = bookResult.OrderByDescending(b => b.Year).ToList();
            //        break;
            //    case "rating_desc":
            //        bookResult = bookResult.OrderByDescending(b => b.Rating).ToList();
            //        break;
            //    default:
            //        bookResult = bookResult.OrderBy(b => b.Author.Name).ToList();
            //        break;
            //}

            var booksMapped = bookResult
                .Select(x => x.MapToViewModel())
                .ToList();
            var user = await _accountManager.GetUserByUsernameAsync(User.Identity.Name);

            await this.SearchResultViewModelMapper(viewModel, booksMapped, user);

            return View(viewModel);
        }

        private async Task SearchViewModelMapper(SearchViewModel searchVM, IEnumerable<BookViewModel> allBookCopies, User user)
        {
            foreach (var book in allBookCopies)
            {
                if (!searchVM.AllBooks.Any(x => x.ISBN == book.ISBN))
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        book.IsBookCheckedout = _system.IsBookCheckedout(user, book.ISBN);
                        book.IsChBooksMaxQuota = _system.IsMaxCheckedoutQuota(user);
                        book.AreAllCopiesChecked = await _system.AreAllCopiesCheckedAsync(book.ISBN);
                        book.UserStatus = user.Status.ToString();
                    }

                    book.BookCopies = await _bookManager.BookCopiesCountAsync(book.ISBN);
                    searchVM.AllBooks.Add(book);
                }
            }
        }

        private async Task SearchResultViewModelMapper(SearchViewModel searchVM, IEnumerable<BookViewModel> searchResult, User user)
        {
            foreach (var book in searchResult)
            {
                if (!searchVM.BookSearchResults.Any(x => x.ISBN == book.ISBN))
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        book.IsBookCheckedout = _system.IsBookCheckedout(user, book.ISBN);
                        book.IsChBooksMaxQuota = _system.IsMaxCheckedoutQuota(user);
                        book.AreAllCopiesChecked = await _system.AreAllCopiesCheckedAsync(book.ISBN);
                        book.UserStatus = user.Status.ToString();
                    }

                    book.BookCopies = await _bookManager.BookCopiesCountAsync(book.ISBN);
                    searchVM.BookSearchResults.Add(book);
                }
            }
        }
    }
}