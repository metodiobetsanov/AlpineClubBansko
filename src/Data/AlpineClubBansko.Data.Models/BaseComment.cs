using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public abstract class BaseComment : BaseEntity
    {
        public string Comment { get; set; }
    }
}
