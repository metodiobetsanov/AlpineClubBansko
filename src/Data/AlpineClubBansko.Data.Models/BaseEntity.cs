using System;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Data.Models
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}