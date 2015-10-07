using System.Linq;
using NUnit.Framework;
using System.Net;
using Newtonsoft.Json.Linq;

namespace JoulsePowerTests
{
    [TestFixture]
    public class AuthControllerTest : TestBase
    {
        

        public AuthControllerTest()
        {
            this.AutoLogin = false;
        }

        [Test]
        public void StatusCodeEquals200WhenILoginMeWithGoodCredentials()
        {
            var result = base.Login(false, "fafnir", "attractive");

            base.AssertStatusCode(HttpStatusCode.OK, result);
        }

        [Test]
        public void GettingTokenWhenILoginMeWithGoodCredentials()
        {
            var result = base.Login(false, "fafnir", "attractive");

            JToken token = JObject.Parse(result.Content.ReadAsStringAsync().Result);
            var tokenValue = (string) token.SelectToken("token");
            var username = (string) token.SelectToken("user").SelectToken("name");
            var roles = JArray.Parse(token.SelectToken("user").SelectToken("roles").ToString());

            Assert.NotNull(tokenValue);
            Assert.NotNull(username);
            Assert.AreEqual("fafnir", username);
            Assert.AreEqual("user", roles.ElementAt(0).ToString());
            Assert.AreEqual("admin", roles.ElementAt(1).ToString());
        }

        [Test]
        public void StatusCodeEquals403WhenILoginMeWithFakeCredentials()
        {
            var result = base.Login(false, "fafnir", "fakkeeeee");

            base.AssertStatusCode(HttpStatusCode.Forbidden, result);
        }
    }
}