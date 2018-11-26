using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BookShopWithAuthen.Service.Services
{
    public interface ICartService
    {
        IEnumerable<CartDetail> GetByUserID(string userID);
        int GetTotalMoney(string userID);
        void CreateCartDetail(CartDetail cartDetail);
        void RemoveCartDetail(string userID, int bookID);
        void UpdateCartDetail(CartDetail cartDetail);
        void AddItemToCart(string userID, int bookID, int quantity = 1);
        int GetQuanity(string userID, int bookID);

    }


    public class CartService : ICartService
    {
        private readonly IBaseRepository<CartDetail> _cartDetailRepository;
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IBaseRepository<CartDetail> cartDetailRepository, IBaseRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            _cartDetailRepository = cartDetailRepository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddItemToCart(string userID, int bookID, int quantity = 1)
        {
            try
            {
                CartDetail current = _cartDetailRepository.Get(cd => cd.BookID == bookID && cd.UserID == userID);
                if (current == null)
                {
                    CartDetail cartDetail = new CartDetail()
                    {
                        UserID = userID,
                        BookID = bookID,
                        Quantity = quantity
                    };
                    _cartDetailRepository.Add(cartDetail);
                    Commit();
                }
                else
                {
                    current.Quantity = current.Quantity + quantity;
                    Commit();
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
           
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void CreateCartDetail(CartDetail cartDetail)
        {
            _cartDetailRepository.Add(cartDetail);
            Commit();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IEnumerable<CartDetail> GetByUserID(string userID)
        {
            return _cartDetailRepository.GetMany(cd => cd.UserID.Equals(userID));
        }

        public int GetQuanity(string userID, int bookID)
        {
            CartDetail cartDetail = _cartDetailRepository.Get(c => c.UserID.Equals(userID) && c.BookID.Equals(bookID));
            if (cartDetail == null)
            {
                return 0;
            }
            return _cartDetailRepository.Get(c => c.UserID.Equals(userID) && c.BookID.Equals(bookID)).Quantity;
        }

        public int GetTotalMoney(string userID)
        {
            var totalMoney = from cd in GetByUserID(userID)
                             join book in _bookRepository.GetAll() on cd.BookID equals book.ID
                             select cd.Quantity * book.Price;
            return totalMoney.Sum();
                             
        }

        public void RemoveCartDetail(string userID, int bookID)
        {
            _cartDetailRepository.Delete(cd => cd.UserID.Equals(userID) && cd.BookID == bookID);
            Commit();
        }

        public void UpdateCartDetail(CartDetail cartDetail)
        {
            _cartDetailRepository.Update(cartDetail);
            Commit();
        }
    }
}
