using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JoulsePowerTests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IDictionary<string, string> Credentials;

        protected HttpClient Client;

        protected string Token { get; private set; }

        protected bool AutoLogin { get; set; }

        private readonly string _uri = "http://localhost:52242/api/token";

        private readonly string _bodyPattern = "username={0}&password={1}";

        protected TestBase()
        {
            this.Token = null;
            this.AutoLogin = true;

            this.Client = new HttpClient();
            this.Credentials = new Dictionary<string, string> {{"fafnir", "attractive"}};
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            this.OnSetupBefore();

            if (this.AutoLogin) {
                this.Login();
            }

            this.OnSetupAfter();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            this.OnTearDownAfter();
        }

        protected HttpResponseMessage Post(string uri, string content, string contentType = "application/json")
        {
            return this.Client.PostAsync(uri, new StringContent(content, Encoding.UTF8, contentType)).Result;
        }

        protected HttpResponseMessage Get(string uri, bool useToken = true)
        {
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(uri),
                Method = HttpMethod.Get,
            };

            if (useToken && this.Token != null) {
                request.Headers.Add("X-AUTH", this.Token);
            }

            return this.Client.SendAsync(request).Result;
        }

        protected void AssertStatusCode(HttpStatusCode expected, HttpResponseMessage requestResult)
        {
            Assert.AreEqual(expected, requestResult.StatusCode);
        }

        protected virtual void OnSetupBefore() { }

        protected virtual void OnSetupAfter() { }

        protected virtual void OnTearDownAfter()
        {
            this.Client.Dispose();
        }

        protected HttpResponseMessage Login(bool autoAssert = true, string username = null, string password = null)
        {
            var body = string.Format(
                this._bodyPattern,
                username ?? this.Credentials.First().Key, 
                password ?? this.Credentials.First().Value
            );

            var result = this.Post(this._uri, body, "application/x-www-form-urlencoded");

            JToken token = JObject.Parse(result.Content.ReadAsStringAsync().Result);
            this.Token = (string)token.SelectToken("token");

            if (autoAssert)
            {
                Assert.NotNull(this.Token, "Login failed!");
            }

            return result;
        }
    }
}
