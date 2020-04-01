using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NinjaDudsDesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public async void TestWebApi()
        {
            var response = new Response() { Message = "YippeKayYay" };
            var person = new Person()
            {
                Email = "alan.purugganan@gmail.com",
                FirstName = "Alan",
                LastName = "Purugganan"
            };

            var options = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
        };

            string json = JsonSerializer.Serialize<Response>(response, options);

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://xwvhb77s80.execute-api.us-east-1.amazonaws.com");

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/Prod/write-examples", content);

            string message = "Error";

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                json = await result.Content.ReadAsStringAsync();
                var responseBody = JsonSerializer.Deserialize<Response>(json, options);
                message = responseBody.Message;
            }

            Console.WriteLine(message);

            //now attempt a read
            result = await client.GetAsync("/Prod/read-examples");

            message = "Error";

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                json = await result.Content.ReadAsStringAsync();
                var messageItems = JsonSerializer.Deserialize<ResponseB>(json, options);
                int x = 9;    
            }

            


        }

        public MainWindow()
        {
            InitializeComponent();

            TestWebApi();           
        }
    }

    public class Person
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
    }

    public class ResponseB
    {
        public string S3Result { get; set; }

        public MessageItem[] DbResult { get; set; }
    
    }

        


    public class MessageItem
    {
        public string Pk { get; set; }
        public string Sk { get; set; }

        public string Message { get; set; }

    }


}
