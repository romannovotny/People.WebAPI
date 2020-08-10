using Newtonsoft.Json;
using People.API.Models;
using People.API.Models.DTOs;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace People.IntegrationTests
{
    public class IntegrationTests
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string webApiBaseUrl = "http://webapi/api";

        [Fact]
        public async Task Login_WithWrongData_Should_Return_Unauthorized()
        {
            var login = LoginModelNotOK();
            var endpoint = $"{webApiBaseUrl}/auth/login";
            var response = await Client.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithRightData_Should_Return_OkStatus()
        {
            var login = LoginModelOK();
            var endpoint = $"{webApiBaseUrl}/auth/login";
            var response = await Client.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jwt = JsonConvert.DeserializeObject<Jwt>(response.Content.ReadAsStringAsync().Result);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.token);
        }

        public static async Task<string> Login()
        {
            var login = LoginModelOK();
            var endpoint = $"{webApiBaseUrl}/auth/login";
            var response = await Client.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jwt = JsonConvert.DeserializeObject<Jwt>(response.Content.ReadAsStringAsync().Result);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.token);
            return jwt.token;
        }

        [Fact]
        public async Task PostPerson_Should_Return_CreatedStatus()
        {
            await Login();
            var response = await Client.PostAsync($"{webApiBaseUrl}/person", new StringContent(JsonConvert.SerializeObject(TestPersonDto()), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostPerson_Should_Return_OnePerson()
        {
            await Login();
            var response = await Client.PostAsync($"{webApiBaseUrl}/person", new StringContent(JsonConvert.SerializeObject(TestPersonDto()), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var person = JsonConvert.DeserializeObject<PersonDto>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(TestPersonDto().Firstname, person.Firstname);

            response = await Client.GetAsync($"{webApiBaseUrl}/person");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var persons = JsonConvert.DeserializeObject<List<PersonDto>>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(persons);
            Assert.Single(persons);
            Assert.Equal(TestPersonDto().Firstname, persons[0].Firstname);
        }

        public class Jwt
        {
            public string token { get; set; }
        }

        public static PersonPostDto TestPersonDto()
        {
            var person = new PersonPostDto
            {
                Firstname = "TestDto1",
                Lastname = "ZbeznyDto",
                Email = "bezny@mail.com",
                Phone = "32568954"
            };
            return person;
        }

        public static LoginModel LoginModelOK()
        {
            var login = new LoginModel
            {
                UserName = "test",
                Password = "test",
            };
            return login;
        }

        public static LoginModel LoginModelNotOK()
        {
            var login = new LoginModel
            {
                UserName = "admi",
                Password = "pas",
            };
            return login;
        }
    }
}