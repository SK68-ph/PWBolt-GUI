
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PWBolt_GUI.Network
{
    class Account
    {
        public string id { get; set; }
        public string website { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    class AccID
    {
        public string id { get; set; }
    }
    class Login
    {
        public string username { get; set; }
        public string pwbolt { get; set; }
    }
    static class WebServer
    {
        private static CookieContainer cookieJar = new CookieContainer();
        //https://elucidative-designa.000webhostapp.com
        //http://localhost
        private static string webserver = @"http://localhost";
        private static bool _isLoggedIn = false;
        public static bool IsLoggedIn { get { return _isLoggedIn; } set { _isLoggedIn = value; } }


        private static async Task<string> SendPostRequestAsync(string rawJson, string URL)
        {
            var data = new StringContent(rawJson, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            try
            {
                var client = new HttpClient(handler);
                var response = await client.PostAsync(URL, data);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show("Failed Connecting to webserver.");
            }
            return "";
        }

        private static async Task<string> SendResponseAsync(string URL)
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            try
            {
                var client = new HttpClient(handler);
                var response = await client.GetAsync(URL);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show("Failed Connecting to webserver.");
            }
            return "";
        }

        public static async Task LoginPWBolt(string user, string password)
        {
            var acc = new Login { username = user, pwbolt = password };
            string json = JsonConvert.SerializeObject(acc);
            Task<string> taskResponse = SendPostRequestAsync(json, webserver + "/login.php");
            string response = await taskResponse;
            if (response.Contains("OK"))
            {
                IsLoggedIn = true;
            }
            else if(response.Contains("ERROR"))
            {
                MessageBox.Show(response);
                cookieJar = new CookieContainer();
                IsLoggedIn = false;
            }
        }

        public static async Task RegisterPWBoltAsync(string user, string password)
        {
            var acc = new Login { username = user, pwbolt = password };
            string json = JsonConvert.SerializeObject(acc);
            Task<string> taskResponse = SendPostRequestAsync(json, webserver + "/register.php");
            string response = await taskResponse;
            if (response.Contains("ERROR") || response.Contains("OK"))
            {
                MessageBox.Show(response);
            }
        }

        public static Task<string> bolt_DisplayAccountAsync()
        {
            Task<string> taskResponse = SendResponseAsync(webserver + "/bolt_DisplayAccount.php");
            return taskResponse;
        }

        public static async Task bolt_AddAccountAsync(string id,string web, string user, string pass)
        {
            var acc = new Account { id = id, website = web, username = user, password = pass };
            string json = JsonConvert.SerializeObject(acc);
            Task<string> taskResponse = SendPostRequestAsync(json, webserver + "/bolt_AddAccount.php");
            string response = await taskResponse;
            if (response.Contains("ERROR") || response.Contains("OK"))
            {
                MessageBox.Show(response);
            }
        }

        public static async Task bolt_EditAccountAsync(string id, string web, string user, string pass)
        {
            var acc = new Account { id= id, website = web, username = user, password = pass };
            string json = JsonConvert.SerializeObject(acc);
            Task<string> taskResponse = SendPostRequestAsync(json, webserver + "/bolt_EditAccount.php");
            string response = await taskResponse;
            if (response.Contains("ERROR") || response.Contains("OK"))
            {
                MessageBox.Show(response);
            }
        }
        public static async Task bolt_DeleteAccountAsync(string id)
        {
            var accID = new AccID { id = id };
            string json = JsonConvert.SerializeObject(accID);
            Task<string> taskResponse = SendPostRequestAsync(json, webserver + "/bolt_DeleteAccount.php");
            string response = await taskResponse;
            if (response.Contains("ERROR") || response.Contains("OK"))
            {
                MessageBox.Show(response);
            }
        }

        public static void Logout()
        {
            cookieJar = new CookieContainer();
            IsLoggedIn = false;
        }

    }
}
