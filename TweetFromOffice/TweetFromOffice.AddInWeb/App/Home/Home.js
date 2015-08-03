/// <reference path="../App.js" />

(function () {
    "use strict";

    // These are my personal Twitter keys so you should store your own keys here, you can create an application at http://apps.twitter.com. However this is just a sample so if you cannot be bothered to create your own, use these and expect that the may change without notice
    var twitterConsumerKey = "arWUup59V9t3oA7JVtoaP42Ws";
    var twitterConsumerSecret = "hMrWNIqDLy6Ee3cOLMvt2gVZpYOT1wWv7ptO4tfEg23Li3tq1H";
    var twitterAccessToken = "";
    var twitterAccessTokenSecret = "";

    // The initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            app.initialize();

            $('#SendTweet').click(SendTweet);

        });
    };

    function SendTweet() {
        var tweet = $("#Tweet")[0].value;
        app.showNotification('You will tweet', tweet);
    }

})();