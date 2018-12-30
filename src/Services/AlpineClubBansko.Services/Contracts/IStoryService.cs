using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IStoryService
    {
        IEnumerable<StoryViewModel> GetAllStories();

        StoryViewModel GetStoryById(string id);

        Story GetStory(string id);

        Task<string> CreateAsync(string title, User user);

        Task<bool> UpdateAsync(StoryViewModel model);

        Task<bool> DeleteAsync(string id);

        Task<bool> AddViewed(string id);

        Task<bool> Favorite(string id, User user);

        Task<bool> CreateComment(string id, string content, User user);

        Task<bool> DeleteComment(string id);
    }
}