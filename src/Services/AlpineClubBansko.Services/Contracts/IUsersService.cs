using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.UserViewModels;
using System.Collections.Generic;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IUsersService
    {
        IEnumerable<UserProfileViewModel> GetAllUsers();

        User GetUser(string id);

        UserProfileViewModel GetUserById(string id);
    }
}