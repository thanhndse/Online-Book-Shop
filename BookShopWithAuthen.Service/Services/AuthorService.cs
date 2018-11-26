using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using System.Collections.Generic;

namespace BookShopWithAuthen.Service.Services
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAll();
    }


    public class AuthorService : IAuthorService
    {
        private readonly IBaseRepository<Author> _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IBaseRepository<Author> authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
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

        #region Author Service member
        public IEnumerable<Author> GetAll()
        {
            return _authorRepository.GetAll();

        }
        #endregion
    }
}
