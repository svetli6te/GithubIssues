using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace GithubIssuesTests
{
    [TestClass]
    public class APITests
    {
        private const string POST_ISSUE_URL = "https://api.github.com/repos/svetli6te/GithubIssues/issues";
        private const string POST_ISSUE_COMMENT_URL = "https://api.github.com/repos/svetli6te/GithubIssues/issues/{issueNumber}/comments";
        private const string GET_ISSUE_URL = "https://api.github.com/repos/svetli6te/GithubIssues/issues/{issueNumber}";
        private const string GET_ISSUE_COMMENT_URL = "https://api.github.com/repos/svetli6te/GithubIssues/issues/comments/{commentId}";
        private const string TOKEN = "Bearer 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";

        [TestMethod]
        public void CreateIssueWithStringTitle()
        {
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_URL
            };
           
            string issueTitle = Lorem.Sentence();
            string body = "{\"title\": " +  "\"" + issueTitle + "\"" + "}";

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");
            postRequest.AddHeader("Authorization", TOKEN);
            postRequest.AddJsonBody(body);

            IRestResponse response =  client.Post(postRequest);

            Assert.AreEqual(201, (int)response.StatusCode);
            int issueNumber = int.Parse(JObject.Parse(response.Content).GetValue("number").ToString());

            IRestRequest getRequest = new RestRequest()
            {
                Resource = GET_ISSUE_URL.Replace("{issueNumber}", issueNumber.ToString())
            };

            getRequest.AddHeader("Accept", "application/vnd.github.v3+json");

            response = client.Get(getRequest);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void CreateIssueWithIntegerTitle()
        {
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_URL
            };

            int issueTitle = RandomNumber.Next(10000);
            object body = new 
            {
                title = issueTitle
            };

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");
            postRequest.AddHeader("Authorization", TOKEN);
            postRequest.AddJsonBody(body);

            IRestResponse response = client.Post(postRequest);

            Assert.AreEqual(201, (int)response.StatusCode);
            int issueNumber = int.Parse(JObject.Parse(response.Content).GetValue("number").ToString());

            IRestRequest getRequest = new RestRequest()
            {
                Resource = GET_ISSUE_URL.Replace("{issueNumber}", issueNumber.ToString())
            };

            getRequest.AddHeader("Accept", "application/vnd.github.v3+json");

            response = client.Get(getRequest);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void CreateIssueComment()
        {
            int issueNumber = RandomNumber.Next(1, 5);
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_COMMENT_URL.Replace("{issueNumber}", issueNumber.ToString())
            };

            string issueComment = Lorem.Sentence();
            string body = "{\"body\": " + "\"" + issueComment + "\"" + "}";

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");
            postRequest.AddHeader("Authorization", TOKEN);
            postRequest.AddJsonBody(body);

            IRestResponse response = client.Post(postRequest);

            Assert.AreEqual(201, (int)response.StatusCode);
            int commentId = int.Parse(JObject.Parse(response.Content).GetValue("id").ToString());

            IRestRequest getRequest = new RestRequest()
            {
                Resource = GET_ISSUE_COMMENT_URL.Replace("{commentId}", commentId.ToString())
            };

            getRequest.AddHeader("Accept", "application/vnd.github.v3+json");

            response = client.Get(getRequest);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void Code404WhenCreatingIssueWithoutToken()
        {
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_URL
            };

            string issueTitle = Lorem.Sentence();
            string body = "{\"title\": " + "\"" + issueTitle + "\"" + "}";

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");
            postRequest.AddJsonBody(body);

            IRestResponse response = client.Post(postRequest);

            Assert.AreEqual(404, (int)response.StatusCode);
        }

        [TestMethod]
        public void Code404WhenCreatingIssueWithoutTokenAndBody()
        {
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_URL
            };

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Post(postRequest);

            Assert.AreEqual(404, (int)response.StatusCode);
        }

        [TestMethod]
        public void Code404WhenCreatingIssueWithoutBody()
        {
            IRestClient client = new RestClient();
            IRestRequest postRequest = new RestRequest()
            {
                Resource = POST_ISSUE_URL
            };          

            postRequest.AddHeader("Accept", "application/vnd.github.v3+json");
            postRequest.AddHeader("Content-Type", "application/json");
            postRequest.AddHeader("Authorization", TOKEN);

            IRestResponse response = client.Post(postRequest);

            Assert.AreEqual(422, (int)response.StatusCode);
        }
    }
}
