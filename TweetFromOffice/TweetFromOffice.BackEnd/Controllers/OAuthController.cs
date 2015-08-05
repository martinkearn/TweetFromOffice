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
    public class OAuthController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("BeginAsync");
        }

        public async Task<ActionResult> BeginAsync(string returnUrl = "/")
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"]
                }
            };

            //check if we already have auth in a cookie
            if (Cookies.Read(Request, Server, "TwitterOAuthToken") != null)
            {
                //load credentials from cookie. if oauthToken is not null, assume all of them are not null
                try
                {
                    auth.CredentialStore.OAuthToken = Cookies.Read(Request, Server, "TwitterOAuthToken");
                    auth.CredentialStore.OAuthTokenSecret = Cookies.Read(Request, Server, "TwitterOAuthTokenSecret");
                    auth.CredentialStore.ScreenName = Cookies.Read(Request, Server, "TwitterScreenName");
                    auth.CredentialStore.UserID = Convert.ToUInt64(Cookies.Read(Request, Server, "TwitterUserID"));
                }
                catch
                {
                    //if an exception occurs, do OAuth consent
                    string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete") + "?returnUrl=" + returnUrl;
                    return await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
                }

                //return to calling url
                return Redirect(returnUrl);
            }
            else
            {
                string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete") +"?returnUrl=" +returnUrl;
                return await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
            }
        }

        public async Task<ActionResult> CompleteAsync(string returnUrl)
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            await auth.CompleteAuthorizeAsync(Request.Url);

            //save credentials in cookie
            var credentials = auth.CredentialStore;
            Cookies.Write(Response, "TwitterOAuthToken", credentials.OAuthToken);
            Cookies.Write(Response, "TwitterOAuthTokenSecret", credentials.OAuthTokenSecret);
            Cookies.Write(Response, "TwitterScreenName", credentials.ScreenName);
            Cookies.Write(Response, "TwitterUserID", credentials.UserID.ToString());

            return Redirect(returnUrl);
        }

        public PartialViewResult AuthStatus()
        {
            var viewModel = new TwitterLoginViewModel()
            {
                ScreenName = Cookies.Read(Request, Server, "TwitterScreenName")
            };

            return PartialView(viewModel);
        }

        public ActionResult LogOut()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = null
            };
            Cookies.Write(Response, "TwitterOAuthToken", string.Empty);
            Cookies.Write(Response, "TwitterOAuthTokenSecret", string.Empty);
            Cookies.Write(Response, "TwitterScreenName", string.Empty);
            Cookies.Write(Response, "TwitterUserID", string.Empty);
            return RedirectToAction("AuthStatus");
        }
    }
}