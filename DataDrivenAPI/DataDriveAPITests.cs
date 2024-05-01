
using Newtonsoft.Json;
using System.Net;
using static DataDrivenAPI.ZipCodeLocationResponse;

namespace DataDrivenAPI

{
    public class ZipCodeLocationResponse
    {
        public class Place
        {
            [JsonProperty("place name")]
            public string placename { get; set; }
            public string longitude { get; set; }
            public string state { get; set; }

            [JsonProperty("state abbreviation")]
            public string stateabbreviation { get; set; }
            public string latitude { get; set; }
        }

        public class Location
        {
            [JsonProperty("post code")]
            public string postcode { get; set; }
            public string country { get; set; }

            [JsonProperty("country abbreviation")]
            public string countryabbreviation { get; set; }
            public List<Place> places { get; set; }
        }
    }
    [TestClass]
    public class DDAPITests
    {
        private TestContext _testContextInstance;

        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }
        [TestMethod]
        [DataRow("US", "12537", "NY", "New York")]
        [DataRow("US", "79912", "TX", "Texas")]
        public async Task LocationTestAsync(string countryCode, string postCode, string expectedSA, string expectedState)
        {
            
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.GetAsync($"https://api.zippopotam.us/{countryCode}/{postCode}").Result;

            //_testContextInstance.WriteLine(response.);
            
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            // deserializing
            var locationResponse = JsonConvert.DeserializeObject<Location>(await response.Content.ReadAsStringAsync());

            //_testContextInstance.WriteLine(locationResponse.country);

            //assert country abbreviation
            Assert.AreEqual(locationResponse.countryabbreviation, countryCode);

            //assert state
            Assert.AreEqual(locationResponse.places.First().state, expectedState);

            //assert state abbreviation
            Assert.AreEqual(locationResponse.places.First().stateabbreviation, expectedSA);
        }
    }
}