using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MyApplication.Dto;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

[Binding]
public class UserLoginSteps
{

    private readonly TestServer _server;
    private readonly HttpClient _client;
    private HttpResponseMessage _response;

    public UserLoginSteps()
    {
        _server = new TestServer(new WebHostBuilder().UseStartup<Program>());
        _client = _server.CreateClient();
    }

    [Given(@"I have entered a valid email and password")]
    public async Task GivenIHaveEnteredAValidEmailAndPassword()
    {
        var loginDto = new LoginDto { email = "test@example.com", password = "password" };
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

        _response = await _client.PostAsync("/api/user/login", content);
    }

    [When(@"I press login")]
    public void WhenIPressLogin()
    {
        // This step is implemented in the Given step
    }

    [Then(@"I should be logged in")]
    public void ThenIShouldBeLoggedIn()
    {
        _response.EnsureSuccessStatusCode();
    }
}