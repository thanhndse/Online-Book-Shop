using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;

namespace BookShopWithAuthen.Service.Services
{
    public interface IPublisherService
    {
    }


    public class PublisherService : IPublisherService
    {
        private readonly IBaseRepository<Publisher> _publisherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IBaseRepository<Publisher> publisherRepository, IUnitOfWork unitOfWork)
        {
            _publisherRepository = publisherRepository;
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
    }
}
