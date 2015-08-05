using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetFromOffice.BackEnd.Models
{
    public class PostTweetViewModel
    {
        public string TweetText { get; set; }
        public Status PostedTweet { get; set; }
        public ulong PostedTweetStatusId { get; set; }
    }
}
