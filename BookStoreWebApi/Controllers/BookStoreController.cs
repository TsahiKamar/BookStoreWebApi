using System;
using System.Collections.Generic;
using System.Diagnostics;
using BookStoreWebApi.Models;
using BookStoreWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;

namespace BookStoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookStoreController : ControllerBase 
    {
        private IUserService _userService;
        private IRepositoryService _repositoryService;

        //static List<BookModel> booksList = new List<BookModel>();
        //static List<PurchaseModel> salesList = new List<PurchaseModel>();

         private readonly ILogger<BookStoreController> _logger;

        public BookStoreController(IUserService userService, ILogger<BookStoreController> logger, IRepositoryService repositoryService)
        {
            _userService = userService;
            _logger = logger;
            _repositoryService = repositoryService;
        }

        [HttpGet]
        public IList<BookModel> Get()
        {
              return this._repositoryService.GetBooks();
        }

         [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest request)
        {
            AuthenticateResponse response = null;
            try
            {
                response = _userService.Authenticate(request);

                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });//return 400
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Authenticate error : ", ex.ToString(), EventLogEntryType.Error);
            }
            return Ok(response);
        }

        [HttpGet("BookSearch")]
        public ActionResult<BookModel> BookSearch(string bookName)
        {
            BookModel book = null; 
            
            try
            {
                var repositoryService = new RepositoryService();
                book = repositoryService.BookSearch(bookName);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("BookSearch error : ", ex.ToString(), EventLogEntryType.Error);
            }
            return Ok(book);
        }


        [Authorize]
        [HttpPost("AddNewBook")]
        public ActionResult<Error> AddNewBook(BookModel request)
        {
            Error error = null;
            List<BookModel> books;
            try
            {
                error = this._repositoryService.AddNewBook(request);
                if (error != null)
                {
                    if (error.code != 0) return error;
                }
                books = this._repositoryService.GetBooks();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("AddNewBook error : ", ex.ToString(), EventLogEntryType.Error);
                return new Error() { code = -1, desc = ex.ToString() };
            }

            return Ok(books);
        }

        [Authorize]
        [HttpPost("BookPurchase")]
        public ActionResult<List<PurchaseModel>> BookPurchase(PurchaseModel request)
        {
            try
            {
                 this._repositoryService.BookPurchase(request);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("BookPurchase error : ", ex.ToString(), EventLogEntryType.Error);
            }

            return Ok(request);
        }

        [Authorize]
        [HttpGet("GetMyLastPurchase")]
        public ActionResult<PurchaseModel> GetMyLastPurchase(string userName)
        {
            PurchaseModel response = null;
            try
            {
                response = this._repositoryService.GetBookLastPurchased(userName);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("GetLastPurchaseBooks error : ", ex.ToString(), EventLogEntryType.Error);
            }

            return response;
        }

        [Authorize]
        [HttpPost("BookUpdate")]
        public ActionResult<Error> BookUpdate(BookModel request)
        {
            Error error = null;
            List<BookModel> books;
            try
            {
                error = this._repositoryService.BookUpdate(request);
                if (error != null)
                {
                    if (error.code != 0) return error ;
                }
                books = this._repositoryService.GetBooks();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("BookUpdate error : ", ex.ToString(), EventLogEntryType.Error);
                return new Error() { code = -1, desc = ex.ToString() };
            }
            return Ok(books);
        }

        [Authorize]
        [HttpDelete("BookDelete")]
        public ActionResult<Error> BookDelete(int id)
        {
            Error error = null;
            List<BookModel> books;

            try
            {
                error = this._repositoryService.BookDelete(id);

                if (error != null)
                {
                    if (error.code != 0) 
                        return error;
                }
                books = this._repositoryService.GetBooks();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("GetLastPurchaseBooks error : ", ex.ToString(), EventLogEntryType.Error);
                return new Error() { code = -1, desc = ex.ToString() };
            }
            return Ok(books);
        }

    }
}

