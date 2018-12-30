using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.StoryViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StoryService> logger;

        public StoryService(IRepository<Story> storyRepository,
            IRepository<StoryComment> storyCommentRepository,
            ILogger<StoryService> logger)
        {
            this.logger = logger;
            this.storyCommentRepository = storyCommentRepository;
            this.storyRepository = storyRepository;
        }

        public IEnumerable<StoryViewModel> GetAllStories()
        {
            return this.storyRepository.All().To<StoryViewModel>();
        }

        public Story GetStory(string id)
        {
            return this.storyRepository.GetById(id);
        }

        public StoryViewModel GetStoryById(string id)
        {
            return this.GetAllStories().FirstOrDefault(s => s.Id == id);
        }

        public async Task<string> CreateAsync(string title, User user)
        {
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
            Story story = this.storyRepository.GetById(model.Id);
            story.Title = model.Title;
            story.Content = model.Content;
            story.ModifiedOn = DateTime.UtcNow;

            this.storyRepository.Update(story);
            var result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Story story = this.storyRepository.All().FirstOrDefault(s => s.Id == id);

            this.storyRepository.Delete(story);

            var result = await this.storyRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> CreateComment(string id, string content, User user)
        {

            StoryComment comment = new StoryComment
                {
                    Author = user,
                    StoryId = id,
                    Comment = content
                };

            await this.storyCommentRepository.AddAsync(comment);
            var result = await this.storyCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteComment(string id)
        {
            StoryComment comment = this.storyCommentRepository.GetById(id);

            this.storyCommentRepository.Delete(comment);
            var result = await this.storyCommentRepository.SaveChangesAsync();
            return result != 0;
        }



        public async Task<bool> AddViewed(string id)
        {
            Story story = this.storyRepository.GetById(id);

            story.Views += 1;

            this.storyRepository.Update(story);
            var changed = await this.storyRepository.SaveChangesAsync();

            return changed != 0;
        }

        public async Task<bool> Favorite(string id, User user)
        {
            Story story = this.storyRepository.GetById(id);
            if (story.Favorite.Any(f => f.UserId == user.Id))
            {
                var item = story.Favorite.FirstOrDefault(f => f.UserId == user.Id);
                story.Favorite.Remove(item);

            }
            else
            {
                story.Favorite.Add(new LikedStories
                {
                    User = user,
                    Story = story
                });

            }

            this.storyRepository.Update(story);
            var changed = await this.storyRepository.SaveChangesAsync();

            return changed != 0;
        }
    }
}