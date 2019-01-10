using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.UserViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IUsersService
    {
        IQueryable<User> GetAllUsers();

        IEnumerable<UserProfileViewModel> GetAllUsersAsViewModels();

        User GetUserById(string id);

        UserProfileViewModel GetUserByIdAsViewModel(string id);
    }
}