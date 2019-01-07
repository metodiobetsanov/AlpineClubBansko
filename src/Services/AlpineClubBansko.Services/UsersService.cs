using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.UserViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> usersRepository;

        public UsersService(IRepository<User> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IEnumerable<UserProfileViewModel> GetAllUsersAsViewModels()
        {
            return this.usersRepository.All().To<UserProfileViewModel>();
        }

        public User GetUser(string id)
        {
            return this.usersRepository.GetById(id);
        }

        public UserProfileViewModel GetUserById(string id)
        {
            return this.GetAllUsersAsViewModels().FirstOrDefault(a => a.Id == id);
        }
    }
}