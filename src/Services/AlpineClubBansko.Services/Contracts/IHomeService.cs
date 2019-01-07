using AlpineClubBansko.Services.Models.HomeViewModels;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IHomeService
    {
        HomeViewModel GetHomeViewModel();
    }
}