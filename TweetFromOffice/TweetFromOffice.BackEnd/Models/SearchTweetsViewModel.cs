using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetFromOffice.BackEnd.Models
{
    public class SearchTweetsViewModel
    {
        public string Query { get; set; }
        public List<Status> Statuses { get; set; }
    }
}
