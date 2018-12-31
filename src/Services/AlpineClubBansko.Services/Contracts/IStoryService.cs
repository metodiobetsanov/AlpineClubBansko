using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IStoryService
    {
        IQueryable<Story> GetAllStories();

        IEnumerable<StoryViewModel> GetAllStoriesAsViewModels();

        Story GetStoryById(string storyId);

        StoryViewModel GetStoryByIdAsViewModel(string storyId);

        Task<string> CreateAsync(string title, User user);

        Task<bool> UpdateAsync(StoryViewModel model);

        Task<bool> DeleteAsync(string storyId);

        Task<bool> AddViewedAsync(string storyId);

        Task<bool> FavoriteAsync(string storyId, User user);

        Task<bool> CreateCommentAsync(string storyId, string content, User user);

        Task<bool> DeleteCommentAsync(string commentId);
    }
}