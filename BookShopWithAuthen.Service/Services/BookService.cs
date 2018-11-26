using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
namespace BookShopWithAuthen.Service.Services

{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetBooksBySomeCondition(string searchValue, int priceFrom, int priceTo,
            int authorID, int categoryID);
        IEnumerable<Book> GetBooksSameCategory(int limit, int categoryID);
        Book GetByID(int ID);
        void UpdateQuantityBook(int bookID, int newQuantity);
        IEnumerable<Book> SearchBookBySearchValue(IEnumerable<Book> oldList, string searchValue);
        IEnumerable<Book> GetBestSellerBooks(IEnumerable<Book> oldList,DateTime fromDate, DateTime toDate);
        IEnumerable<Book> GetBestSellerBooksOfACategory(int categoryID, DateTime fromDate, DateTime toDate, int? limit);

    }


    public class BookService : IBookService
    {
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<OrderDetail> _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBaseRepository<Book> bookRepository, IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderDetail> orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public void Commit()
        {
            _unitOfWork.Commit();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookRepository.GetAll();
        }

        public IEnumerable<Book> GetBestSellerBooks(IEnumerable<Book> oldList, DateTime fromDate, DateTime toDate)
        {
            var allBooks = oldList?? _bookRepository.GetAll();
            var allOrders = _orderRepository.GetAll();
            var allOrderDetails = _orderDetailRepository.GetAll();
            // get all order ID from date  to date and have status finisehed
            var orderIDs = from order in allOrders
                           where order.OrderDate >= fromDate
                           && order.OrderDate <= toDate
                           && order.Status.Equals((int)StatusOrder.Finished)
                           select order.Id;
            // get all bookIDs and sum quantity in orderDetails

            var bookIDsAndQuantity = from orderDetail in allOrderDetails
                                     where orderIDs.Contains(orderDetail.OrderID)
                                     group orderDetail by orderDetail.BookID into g
                                     select new { bookID = g.Key, sumQuantity = g.Sum(od => od.Quantity) };
            var allBestSellerBooks = from tmp in (from book in allBooks
                                 join b2 in bookIDsAndQuantity on book.ID equals b2.bookID into bMix
                                 from b3 in bMix.DefaultIfEmpty()
                                 select new { book = book, sumQuantity = b3?.sumQuantity ?? 0 })
                    orderby tmp.sumQuantity descending
                    select tmp.book;
                    
            return allBestSellerBooks;

        }

        public IEnumerable<Book> GetBestSellerBooksOfACategory(int categoryID, DateTime fromDate, DateTime toDate, int? limit)
        {
            if (limit == null)
            {
                return GetBestSellerBooks(null, DateTime.MinValue, DateTime.Today).Where(b => b.CategoryID == categoryID);
            }
            else
            {
                return GetBestSellerBooks(null, DateTime.MinValue, DateTime.Today).Where(b => b.CategoryID == categoryID).Take((int)limit);

            }
        }

        public IEnumerable<Book> GetBooksBySomeCondition(string searchValue, int priceFrom, int priceTo, int authorID, int categoryID)
        {
            var allBooks = from b in GetAll()
                           where b.Price >= priceFrom && b.Price <= priceTo
                           select b;
            if (!string.IsNullOrEmpty(searchValue))
            {

                allBooks = SearchBookBySearchValue(allBooks, searchValue);
            }
            if (categoryID != -1)
            {
                allBooks = allBooks.Where(b => b.CategoryID == categoryID);
            }
            if (authorID != -1)
            {
                allBooks = allBooks.Where(b => b.Authors.FirstOrDefault(a => a.ID == authorID) != null);
            };
            return allBooks.ToList();
        }

        public IEnumerable<Book> GetBooksSameCategory(int limit, int categoryID)
        {
            return _bookRepository.GetMany(b => b.CategoryID == categoryID).Take(limit);
        }

        public Book GetByID(int ID)
        {
            return _bookRepository.GetById(ID);
        }

        public IEnumerable<Book> SearchBookBySearchValue(IEnumerable<Book> oldList, string searchValue)
        {
           var searchList = _bookRepository.GetMany(b => b.Name.Contains(searchValue));
            return from b1 in oldList
                   join b2 in searchList on b1.ID equals b2.ID
                   select b1;
        }

        public void UpdateQuantityBook(int bookID, int newQuantity)
        {
            var tmpBook = GetByID(bookID);
            tmpBook.Quantity = newQuantity;
            Commit();
        }

        #region BookService member



        #endregion
    }
}
