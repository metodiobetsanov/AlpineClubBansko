using AlpineClubBansko.Services.Models.WebDataViewModels;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IAdminService
    {
        WebDataViewModel GetWebData();
    }
}