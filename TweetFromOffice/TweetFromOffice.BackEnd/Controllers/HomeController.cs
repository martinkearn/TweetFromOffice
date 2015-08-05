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

        [HttpGet]
        public ActionResult PostTweet()
        {
            return View();
        }

        [HttpPost]
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

            var twitterCtx = new TwitterContext(auth);
            
            var tweet = await twitterCtx.TweetAsync(tweetText);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> SearchTweets(string query = "")
        {
            List<Status> statuses;
            if (!string.IsNullOrEmpty(query))
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