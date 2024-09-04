using System.Drawing;
using System.Linq.Expressions;
using Orbit.Application.Exceptions;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.UnitOfWork.Interfaces;

namespace Orbit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddUserAsync(User user)
        {

        }
        public Task<User> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "") => throw new NotImplementedException();
        public Task<User?> GetUserByIdentifierAsync(string userIdentifier) => throw new NotImplementedException();
        public Task UpdateUserAsync(string userIdentifier, User user) => throw new NotImplementedException();
    }
}
