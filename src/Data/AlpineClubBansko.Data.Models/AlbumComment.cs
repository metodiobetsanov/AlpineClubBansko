using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class AlbumComment : BaseComment
    {
        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}
