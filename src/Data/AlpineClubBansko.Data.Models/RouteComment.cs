﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Data.Models
{
    public class RouteComment : BaseComment
    {
        public string RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}
