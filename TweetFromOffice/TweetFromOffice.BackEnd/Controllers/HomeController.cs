using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TweetFromOffice.BackEnd.Helpers;
using TweetFromOffice.BackEnd.Models;

namespace TweetFromOffice.BackEnd.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }


        public async Task<ActionResult> PostTweet(string tweetText = "")
        {
            MvcAuthorizer auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            // do OAuth if the token is null
            if (auth.CredentialStore.OAuthToken == null)
            {
                return RedirectToAction("BeginAsync", "OAuth", new { returnUrl = Request.Url });
            }

            Status postedTweet;
            ulong postedTweetStatusId = 0;
            if (!string.IsNullOrEmpty(tweetText))
            {
                var twitterCtx = new TwitterContext(auth);

                postedTweet = await twitterCtx.TweetAsync(tweetText);

                postedTweetStatusId = postedTweet.StatusID;
            }
            else
            {
                postedTweet = null;
            }

            var viewModel = new PostTweetViewModel()
            {
                TweetText = tweetText,
                PostedTweet = postedTweet,
                PostedTweetStatusId = postedTweetStatusId
            };

            return View(viewModel);
        }

        public async Task<ActionResult> SearchTweets(string query = "")
        {
            MvcAuthorizer auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            // do OAuth if the token is null
            if (auth.CredentialStore.OAuthToken == null)
            {
                return RedirectToAction("BeginAsync", "OAuth", new { returnUrl = Request.Url });
            }

            List<Status> statuses;
            if (!string.IsNullOrEmpty(query))
            {
                var twitterCtx = new TwitterContext(auth);

                var searchResponse =
                    await
                    (from search in twitterCtx.Search
                     where search.Type == SearchType.Search &&
                           search.Query == "\"" +query + "\""
                     select search)
                    .SingleOrDefaultAsync();

                statuses = searchResponse.Statuses.ToList();
            }
            else
            {
                statuses = new List<Status>();
            }
            
            var viewModel = new SearchTweetsViewModel()
            {
                Query = query,
                Statuses = statuses
            };

            return View(viewModel);
        }

    }
}