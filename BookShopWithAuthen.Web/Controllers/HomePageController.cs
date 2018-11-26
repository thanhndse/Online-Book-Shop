using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopWithAuthen.Web.Controllers
{
    public class HomePageController : Controller
    {
        private IBookService bookService;
        private ICategoryService categoryService;
        public HomePageController(IBookService bookService, ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }

        // GET: HomePage
        public ActionResult Index()
        {
            int limit = 6;
            Dictionary<string, IEnumerable<Book>> model = new Dictionary<string, IEnumerable<Book>>();
            IEnumerable<Category> getAllCategories = categoryService.GetAll();
            foreach(var item in getAllCategories)
            {
                IEnumerable<Book> bestSellerOfCategory = bookService.GetBestSellerBooksOfACategory(item.ID, DateTime.MinValue, DateTime.Today, limit);
                model.Add(item.Name.ToUpper(), bestSellerOfCategory);
            }
            return View(model);
        }
    }
}