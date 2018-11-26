using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShopWithAuthen.Service.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();
        Order GetByID(int id);
        void ChangeStatus(int id, int status);
        void Commit();
        void Dispose();
        bool CreateOrder(Order order);
        void BeginTransaction();
        bool EndTransaction();
        void RollBack();
    }


    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<OrderDetail> _orderDetailRepository;
        private readonly IBaseRepository<CartDetail> _cartDetailRepository;
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderDetail> orderDetailRepository,
            IBaseRepository<CartDetail> cartDetailRepository,
            IBaseRepository<Book> bookRepository,
        IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _cartDetailRepository = cartDetailRepository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }

        public void ChangeStatus(int id, int status)
        {
            var order = GetByID(id);
            order.Status = status;
            Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public bool CreateOrder(Order order)
        {
            try
            {
                BeginTransaction();
                // create order
                _orderRepository.Add(order);
                Commit();
                // create order details
                IEnumerable<OrderDetail> listOrderDetails = from cartDetail in _cartDetailRepository.GetMany(cd => cd.UserID.Equals(order.UserId))
                                                            join book in _bookRepository.GetAll() on cartDetail.BookID equals book.ID
                                                            where cartDetail.Quantity > 0
                                                            select new OrderDetail()
                                                            {
                                                                BookID = book.ID,
                                                                OrderID = order.Id,
                                                                BookName = book.Name,
                                                                Price = book.Price,
                                                                Image = book.Image,
                                                                Quantity = cartDetail.Quantity
                                                            };

                foreach (var item in listOrderDetails)
                {
                    _orderDetailRepository.Add(item);
                    Commit();
                    int wareHouseQuantity = item.Book.Quantity;
                    item.Book.Quantity = wareHouseQuantity - item.Quantity;
                    Commit();
                }
                EndTransaction();
                return true;
            }
            catch (Exception ex)
            {
                RollBack();
                return false;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public bool EndTransaction()
        {
            return _unitOfWork.EndTransaction();
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetByID(int id)
        {
            return _orderRepository.GetById(id);
        }

        public void RollBack()
        {
            _unitOfWork.RollBack();
        }
    }
}
