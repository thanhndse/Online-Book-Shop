using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.Services;
using BookShopWithAuthen.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Linq.SqlClient;

namespace BookShopWithAuthen.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private IAuthorService authorService;
        private ICategoryService categoryService;
        private IBookService bookService;
        public CategoriesController(IAuthorService authorService, ICategoryService categoryService,
            IBookService bookService)
        {
            this.authorService = authorService;
            this.categoryService = categoryService;
            this.bookService = bookService;
        }
        // GET: Categories
        public ActionResult Index(SearchCategoryModel searchCategoryModel)
        {
            int pageSize = 10;
            //get select list of Author ID
            List<SelectListItem> selectListItemsAuthor = new List<SelectListItem>();
            foreach (Author item in authorService.GetAll())
            {
                selectListItemsAuthor.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.ID) });
            }
            ViewBag.authorIDList = new SelectList(selectListItemsAuthor, "Value", "Text", -1);
            
            // get list of category
            ViewBag.listCategory = categoryService.GetAll();
            
            // get selected list of category
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Category cate in categoryService.GetAll())
            {
                selectListItems.Add(new SelectListItem { Text = cate.Name, Value = Convert.ToString(cate.ID) });
            }
            ViewBag.categoryID = new SelectList(selectListItems, "Value", "Text", -1);

            // get selected list of selectBy
            Dictionary<string, int> dicSortType = new Dictionary<string, int>
            {
                { "Thứ tự theo giá: Cao đến thấp", (int)sortType.orderByPriceHigh },
                { "Thứ tự theo giá: Thấp đến cao", (int)sortType.orderByPriceLow },
                { "Thứ tự theo sản phẩm mới", (int)sortType.orderByNew },
                { "Thứ tự theo mua nhiều", (int)sortType.orderBySell },
                //{ "Thứ tự theo đánh giá", (int)sortType.orderByRate }
            };
            List<SelectListItem> selectListItemsOrderBy = new List<SelectListItem>();
            foreach (KeyValuePair<string, int> entry in dicSortType)
            {
                selectListItemsOrderBy.Add(new SelectListItem { Text = entry.Key, Value = Convert.ToString(entry.Value) });
            }
            ViewBag.listSortType = new SelectList(selectListItemsOrderBy, "Value", "Text");
            
            // get all books
            // ID = -1 get books of all category
            var allWarehouseBooks = bookService.GetBooksBySomeCondition(searchCategoryModel.SearchValue,
                searchCategoryModel.PriceFrom,
                searchCategoryModel.PriceTo,
                searchCategoryModel.AuthorID,
                searchCategoryModel.ID);
            switch (searchCategoryModel.sortBy)
            {
                case (int)sortType.orderByPriceHigh:
                    allWarehouseBooks = allWarehouseBooks.OrderByDescending(b => b.Price);
                    break;
                case (int)sortType.orderByPriceLow:
                    allWarehouseBooks = allWarehouseBooks.OrderBy(b => b.Price);
                    break;
                case (int)sortType.orderBySell:
                    allWarehouseBooks = bookService.GetBestSellerBooks(allWarehouseBooks, DateTime.MinValue, DateTime.Today);
                    break;

            }
            ViewBag.pageCount = Math.Ceiling(allWarehouseBooks.Count() / (pageSize*1.0));
            int startIndex = pageSize * (searchCategoryModel.Page - 1);

            // phan trang
            ViewBag.allBooks = allWarehouseBooks.Skip(startIndex).Take(pageSize).ToList();
            return View(searchCategoryModel);
        }
    }
}
