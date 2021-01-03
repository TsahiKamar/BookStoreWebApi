using BookStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreWebApi.Services
{

    #region Interface IRepositoryService
    public interface IRepositoryService
    {
        BookModel BookSearch(string bookName);
        List<BookModel> GetBooks();
        void SetBooks(List<BookModel> booksLst);
        PurchaseModel GetBookLastPurchased(string userName);
        Error AddNewBook(BookModel request);
        void BookPurchase(PurchaseModel request);

        Error BookUpdate(BookModel request);
        Error BookDelete(int id);

    }
    #endregion

    public class RepositoryService : IRepositoryService
    {
        private static List<BookModel> booksList = new List<BookModel>();
        private static List<PurchaseModel> bookSales = new List<PurchaseModel>();

        private static List<PublisherModel> publishersList = new List<PublisherModel>();
        private static List<AuthorModel> authorsList = new List<AuthorModel>();

        public RepositoryService()
        {

            if (publishersList.Count == 0)
            {
                PublisherModel publisher1 = new PublisherModel() { id = 1, name = "רום הוצאת ספרים",  mobilePhone = "0523111333",email="rom@gmail.com" };
                PublisherModel publisher2 = new PublisherModel() { id = 2, name = "אור הוצאה לאור", mobilePhone = "0543111333",email= "oror@gmail.com" };

                publishersList.Add(publisher1);
                publishersList.Add(publisher2);

            }


            if (authorsList.Count == 0)
            {
                AuthorModel author1 = new AuthorModel() { id = 1, firstName = "אקיו",lastName="מוריטה", mobilePhone = "0523111333" };
                AuthorModel author2 = new AuthorModel() { id = 2, firstName = "מיכאל",lastName="בר-זוהר" , mobilePhone = "0543111333" };

                authorsList.Add(author1);
                authorsList.Add(author2);

            }


            if (bookSales.Count == 0)
            {
                PurchaseModel book1 = new PurchaseModel() { id = 1, name = "האיש מאחורי האגדה", desc = "בן-גוריון", price = 120, purchaseDate = DateTime.Now,userName="Saky" };
                PurchaseModel book2 = new PurchaseModel() { id = 2, name = "תוצרת יפן", desc = "סיפורה של חברת סוני", price = 59, purchaseDate = DateTime.Now.AddDays(-2), userName = "Saky" };

                bookSales.Add(book1);
                bookSales.Add(book2);

                bookSales.OrderBy(x => x.purchaseDate);
            }

            //Book list init
            if (booksList.Count == 0)
            {
                BookModel book1 = new BookModel() { id = 1, name = "האיש מאחורי האגדה", desc = "בן-גוריון", price = 120, author = authorsList[1],publisher = publishersList[0] };
                BookModel book2 = new BookModel() { id = 2, name = "תוצרת יפן", desc = "סיפורה של חברת סוני", price = 59, author = authorsList[0], publisher = publishersList[1] };
                BookModel book3 = new BookModel() { id = 3, name = "פיתוח סודות השוק", desc = "בורסה", price = 66, author = authorsList[1], publisher = publishersList[0] };

                booksList.Add(book1);
                booksList.Add(book2);
                booksList.Add(book3);
            }



        }

        #region Public

        public BookModel BookSearch(string bookName)
        {
            BookModel book= null;
            try
            {
                book = booksList.Find(x => x.name == bookName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return book;
        }

        public List<BookModel> GetBooks()
        {
            return booksList;
        }

        public void SetBooks(List<BookModel> booksLst)
        {
            booksList = booksLst;
        }

        public Error AddNewBook(BookModel request)
        {
            try
            {
                booksList.Add(request) ;
            }
            catch (Exception ex)
            {
                return new Error() { code = -1, desc = ex.ToString() };
            }
            return new Error() { code = 0, desc = "" };
        }
      
        public Error BookUpdate(BookModel request)
        {
            try
            {
                booksList[booksList.FindIndex(x => x.id == request.id)] = request;
            }
            catch (Exception ex)
            {
                return new Error() { code = -1,desc = ex.ToString()};
            }
            return new Error() {code = 0,desc="" };
        }

        public Error BookDelete(int id)
        {
            var deleteId = 0;
            try
            {
                var index = booksList.FindIndex(x => x.id == id);
                if (index == -1) return new Error() {code = -1,desc="Request id not found !" };
                booksList.RemoveAt(index);
                deleteId = id;
            }
            catch (Exception ex)
            {
                new Error() { code = -1, desc = ex.ToString() };
            }
            return new Error() { code = 0, desc = "Book id " + deleteId.ToString() + " successfuly deleted !" };
        }
       
        public PurchaseModel GetBookLastPurchased(string userName)
        {
           try 
            {
                bookSales.OrderBy(x => x.purchaseDate);
                return bookSales.FindAll(s => s.userName == userName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
         }

        public void BookPurchase(PurchaseModel request)
        {
            try
            {
                bookSales.Add(request);
                bookSales.OrderBy(x => x.purchaseDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
