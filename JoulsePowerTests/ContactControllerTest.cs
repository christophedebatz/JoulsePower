using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JoulsePowerTests
{
    [TestFixture]
    public class ContactControllerTest : TestBase
    {
        private readonly string _uri = "http://localhost:52242/api/contacts";

        [Test]
        public void GettingStatusCode200WhenIRequestContactsList()
        {
            var result = base.Get(this._uri);

            base.AssertStatusCode(HttpStatusCode.OK, result);
        }

        [Test]
        public void GettingAListWhenIRequestContactsList()
        {
            var result = base.Get(this._uri);

            base.AssertStatusCode(HttpStatusCode.OK, result);

            var jarray = JArray.Parse(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(jarray.Count(), 4);
        }
    }
}
