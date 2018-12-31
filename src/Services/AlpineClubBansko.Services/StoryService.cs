using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Common;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class StoryService : IStoryService
    {
        private readonly IRepository<Story> storyRepository;
        private readonly IRepository<StoryComment> storyCommentRepository;
        private readonly IRepository<LikedStories> likedStoriesRepository;

        public StoryService(IRepository<Story> storyRepository,
            IRepository<StoryComment> storyCommentRepository,
            IRepository<LikedStories> likedStoriesRepository)
        {
            this.likedStoriesRepository = likedStoriesRepository;
            this.storyCommentRepository = storyCommentRepository;
            this.storyRepository = storyRepository;
        }

        public IQueryable<Story> GetAllStories()
        {
            return this.storyRepository.All();
        }

        public IEnumerable<StoryViewModel> GetAllStoriesAsViewModels()
        {
            return this.storyRepository.All().To<StoryViewModel>();
        }

        public Story GetStoryById(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            return this.storyRepository.GetById(storyId);
        }

        public StoryViewModel GetStoryByIdAsViewModel(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            return this.GetAllStoriesAsViewModels().FirstOrDefault(s => s.Id == storyId);
        }

        public async Task<string> CreateAsync(string title, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(title, nameof(title));

            ArgumentValidator.ThrowIfNull(user, nameof(user));

            Story story = new Story
            {
                Title = title,
                Author = user,
                CreatedOn = DateTime.UtcNow
            };

            await this.storyRepository.AddAsync(story);
            await this.storyRepository.SaveChangesAsync();

            return story.Id;
        }

        public async Task<bool> UpdateAsync(StoryViewModel model)
        {
            ArgumentValidator.ThrowIfNull(model, nameof(model));

            Story story = this.storyRepository.GetById(model.Id);
            story.Title = model.Title;
            story.Content = model.Content;
            story.ModifiedOn = DateTime.UtcNow;

            this.storyRepository.Update(story);
            var result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            Story story = this.storyRepository.All().FirstOrDefault(s => s.Id == storyId);

            this.storyRepository.Delete(story);

            var result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> CreateCommentAsync(string storyId, string content, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));
            ArgumentValidator.ThrowIfNullOrEmpty(content, nameof(content));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            var comment = new StoryComment
            {
                AuthorId = user.Id,
                StoryId = storyId,
                Comment = content,
                CreatedOn = DateTime.UtcNow
            };

            await this.storyCommentRepository.AddAsync(comment);
            var result = await this.storyCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(commentId, nameof(commentId));

            var item = this.storyCommentRepository.GetById(commentId);

            this.storyCommentRepository.Delete(item);
            var result = await this.storyCommentRepository.SaveChangesAsync();
            return result != 0;
        }



        public async Task<bool> AddViewedAsync(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            Story story = this.storyRepository.GetById(storyId);

            story.Views += 1;

            this.storyRepository.Update(story);
            var changed = await this.storyRepository.SaveChangesAsync();

            return changed != 0;
        }

        public async Task<bool> FavoriteAsync(string storyId, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));
            ArgumentValidator.ThrowIfNull(user, nameof(user));
            
            if (this.likedStoriesRepository.All()
                .Any(f => f.UserId == user.Id && f.StoryId == storyId))
            {
                var item = this.likedStoriesRepository.All()
                    .FirstOrDefault(f => f.UserId == user.Id && f.StoryId == storyId);

                this.likedStoriesRepository.Delete(item);

            }
            else
            {
                await this.likedStoriesRepository.AddAsync(new LikedStories
                {
                    UserId = user.Id,
                    StoryId = storyId,
                });

            }
            var changed = await this.likedStoriesRepository.SaveChangesAsync();

            return changed != 0;
        }
    }
}