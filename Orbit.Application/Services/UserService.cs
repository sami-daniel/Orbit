using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Helpers;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Orbit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> AddUserAsync(UserAddRequest userAddRequest)
        {
            ArgumentNullException.ThrowIfNull(nameof(userAddRequest));

            if (!ValidationHelper.IsValid(userAddRequest))
            {
                throw new ArgumentException("Dados invalidos para o usuario!");
            }

            IEnumerable<Domain.Entities.User> users = await _unitOfWork.User.FindAsync(user => user.UserEmail == userAddRequest.UserEmail);
            IEnumerable<Domain.Entities.User> usernames = await _unitOfWork.User.FindAsync(user => user.UserName == userAddRequest.UserName);

            if (users.Any())
            {
                throw new ArgumentException("E-mail já cadastrado anteriormente!");
            }
            if (usernames.Any())
            {
                throw new ArgumentException("Username já cadastrado anteriormente!");
            }
            Domain.Entities.User user = userAddRequest.ToUser();

            await _unitOfWork.User.AddAsync(user);

            _ = _unitOfWork.Complete();

            return user.ToUserResponse();
        }

        public async Task<IEnumerable<UserResponse>> FindUsersAsync(Expression<Func<UserResponse, bool>> predicate)
        {
            Expression<Func<User, bool>> userPredicate = ConvertPredicate(predicate);

            IEnumerable<User> users = await _unitOfWork.User.FindAsync(userPredicate);

            return users.Select(u => u.ToUserResponse());
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            IEnumerable<Domain.Entities.User> users = await _unitOfWork.User.GetAllAsync();
            List<UserResponse> usersResponses = [];
            foreach (Domain.Entities.User user in users)
            {
                usersResponses.Add(user.ToUserResponse());
            }

            return usersResponses;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(params string[] navProperties)
        {
            IEnumerable<User> users = await _unitOfWork.User.GetAllAsync(navProperties);
            List<UserResponse> userResponses = [];
            foreach (User user in users)
            {
                userResponses.Add(user.ToUserResponse());
            }

            return userResponses;
        }

        private static Expression<Func<User, bool>> ConvertPredicate(Expression<Func<UserResponse, bool>> predicate)
        {
            ParameterExpression? paramUserResponse = predicate.Parameters.FirstOrDefault();
            ParameterExpression paramUser = Expression.Parameter(typeof(User), paramUserResponse!.Name);
            Expression body = new ParameterReplacer(paramUserResponse, paramUser).Visit(predicate.Body);
            Expression<Func<User, bool>> lambda = Expression.Lambda<Func<User, bool>>(body, paramUser);

            return lambda;
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}