using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TweetFromOffice.BackEnd.Helpers;

namespace TweetFromOffice.BackEnd.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> GetTweets()
        {
            MvcAuthorizer auth =  new MvcAuthorizer
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
                       search.Query == "\"Windows 10\""
                 select search)
                .SingleOrDefaultAsync();

            var statuses = searchResponse.Statuses.ToList();

            return View(statuses);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}