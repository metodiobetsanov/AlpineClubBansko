using System;
using System.ComponentModel.DataAnnotations;

namespace AlpineClubBansko.Data.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string AuthorId { get; set; }

        [Required]
        public virtual User Author { get; set; }
    }
}