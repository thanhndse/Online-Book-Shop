using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using System.Collections.Generic;

namespace BookShopWithAuthen.Service.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
    }


    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IBaseRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
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

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }
    }
}
