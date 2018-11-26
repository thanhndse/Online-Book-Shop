using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;

namespace BookShopWithAuthen.Service.Services
{
    public interface IOrderDetailService
    {
        void CreateOrderDetail(OrderDetail orderDetail);
    }


    public class OrderDetailService : IOrderDetailService
    {
        private readonly IBaseRepository<OrderDetail> _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IBaseRepository<OrderDetail> orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }

     
        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailRepository.Add(orderDetail);
            Commit();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    
    }
}
