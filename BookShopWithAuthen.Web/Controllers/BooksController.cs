using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.OtherServices;
using BookShopWithAuthen.Service.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace BookShopWithAuthen.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        // GET: Books
        public ActionResult Index(int id)
        {

            int limitSameCategory = 6;
            // get book

            Book book = bookService.GetByID(id);
            if (book == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            // get sach cung the loai
            ViewBag.sameCateBooks = bookService.GetBooksSameCategory(limitSameCategory, book.CategoryID);
            return View(book);
        }

    }
}