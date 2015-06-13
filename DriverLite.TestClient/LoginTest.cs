using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DriverLite.TestClient
{
    public class LoginTest : TestBase
    {
        public override void Run(string[] args)
        {
            var s = Status();
            s.Wait();
            Debug.WriteLine(s.Result);

            var x = Login();
            x.Wait();
            Debug.WriteLine(x.Result);
        }

        private async Task<string> Status()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11720/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetStringAsync("api/Login/Status");

                return response;
            }
        }

        private async Task<bool> Login()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11720/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using(var response = await client.PostAsJsonAsync("api/Login", new { email = "email", vehicle ="vehicle", password = "password" }))
                {
                    var x = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}
