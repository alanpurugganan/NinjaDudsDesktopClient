using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
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
        public List<Task> Tasks = new List<Task>();

       


        public async void TestWebApi()
        {
            NinjaDudsAwsApi.Init(true);
            NinjaDudsAwsApi api = NinjaDudsAwsApi.Current;

            try
            {
                dynamic obj = JsObj();
                obj.Key = "key2.png";
                obj.Content = Utility.PngToBase64(@"C:\Users\blank\Pictures\lumpiakitchen.png");
                obj.IsBase64Encoded = true;
                obj.ContentType = "image/png";

                

                //await api.S3UploadAsync(obj);

                obj = JsObj();
                obj.Key = "key2.png";

                //var s3DownloadResponse = await api.S3DownloadAsync(obj);
                //ImageViewer.Source = Utility.Base64ToPng(s3DownloadResponse.content);

                obj = JsObj();
                obj.TableName = "NinjaDudsMainTablexxLocalxx";
                obj.Item = JsObj();
                obj.Item.PK = "alan.purugganan@gmail.com";
                obj.Item.SK = "jjjjjjjjj";
                obj.Item.CustomMessage = "cool";

                //await api.DbPutAsync(obj);

                obj = JsObj();
                obj.TableName = "NinjaDudsMainTablexxLocalxx";
                obj.Key = JsObj();
                obj.Key.PK = "alan.purugganan@gmail.com";
                obj.Key.SK = "jjjjjjjjj";

                obj = await api.DbGetAsync(obj);


                obj = JsObj();
                obj.TableName = "NinjaDudsMainTablexxLocalxx";
                obj.Key = JsObj();
                obj.Key.PK = "alan.purugganan@gmail.com";
                obj.Key.SK = "jjjjjjjjj";
                obj.UpdateExpression = "set CustomMessage = :r";


                
                obj.ExpressionAttributeValues = JsObj();
                IDictionary<string, object> dict = obj.ExpressionAttributeValues;
                dict[":r"] = "Cool part 2";


                await api.DbUpdateAsync(obj);


                //        TableName: table,
                //Key:
                //            {
                //                "year": year,
                //    "title": title
                //},
                //UpdateExpression: "set info.rating = :r, info.plot=:p, info.actors=:a",
                //ExpressionAttributeValues:
                //            {
                //                ":r":5.5,
                //    ":p":"Everything happens all at once.",
                //    ":a":["Larry", "Moe", "Curly"]
                //},
                //ReturnValues:"UPDATED_NEW"



                //THIS EXAMPLE IS TO DOWNLOAD MULTIPLE IMAGES
                //List<Tuple<string, System.Windows.Controls.Image>> keys = new List<Tuple<string, System.Windows.Controls.Image>>();
                //keys.Add(new Tuple<string, System.Windows.Controls.Image>("key1.png", this.ImageViewer));
                //keys.Add(new Tuple<string, System.Windows.Controls.Image>("key2.png", this.ImageViewer2));

                //foreach (var key in keys)
                //{

                //    Task t = Task.Factory.StartNew(async (object obj) =>
                //    {
                //        Tuple<string, System.Windows.Controls.Image> data = obj as Tuple<string, System.Windows.Controls.Image>;

                //        Debug.WriteLine(data.Item1);

                //        S3DownloadRequest downloadRequest = new S3DownloadRequest()
                //        {
                //            Key = data.Item1
                //        };


                //        var s3DownloadResponse = await api.S3DownloadAsync(downloadRequest);
                //        data.Item2.Dispatcher.Invoke(() =>
                //        {
                //            data.Item2.Source = Base64ToPng(s3DownloadResponse.Content);
                //        });


                //    },
                //    key);

                //    Tasks.Add(t);

                //}






            }
            catch (Exception ex)
            {
                dynamic err = Utility.ToJsObj(ex.Message);
                int x = 9;
            }

            

        //    var person = new Person()
        //    {
        //        Email = "alan.purugganan@gmail.com",
        //        FirstName = "Alan",
        //        LastName = "Purugganan"
        //    };

        //    var options = new JsonSerializerOptions()
        //    {
        //        IgnoreNullValues = true,
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //        PropertyNameCaseInsensitive = true
        //};

            //string json = JsonSerializer.Serialize<S3UploadRequest>(s3UploadRequest, options);

            //using var client = new HttpClient();

            
            //client.BaseAddress = (local)
            //    ? new Uri("http://127.0.0.1:3000")
            //    : new Uri("https://xwvhb77s80.execute-api.us-east-1.amazonaws.com");
                        
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //string pathFolder = (local)
            //    ? ""
            //    : "/Prod";

            //string path = pathFolder + "/s3-upload";
            
            //var result = await client.PostAsync(path, content);
            
            
            //string message = "Error";

            //if(result.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    json = await result.Content.ReadAsStringAsync();
            //    var responseBody = JsonSerializer.Deserialize<Response>(json, options);
            //    message = responseBody.Message;
            //}

            //Console.WriteLine(message);

            ////now attempt a read
            //result = await client.GetAsync("/Prod/read-examples");

            //message = "Error";

            //if (result.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    json = await result.Content.ReadAsStringAsync();
            //    var messageItems = JsonSerializer.Deserialize<ResponseB>(json, options);
            //    int x = 9;    
            //}

            


        }

        public MjpegDecoder _mjpeg;
        public dynamic JsObj()
        {
            dynamic expando = new ExpandoObject();
            return expando;
        }

        public List<object> JsArray()
        {
            return new List<object>();
        }
                

        public MainWindow()
        {
            InitializeComponent();

            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;


            //test json with dictionary

            dynamic o = JsObj();
            o.Hello = "sdf";
            o.Item = JsObj();
            o.Item.PK = "alan.purugganan@gmail.com";
            o.Item.SK = "123456";
            o.Item.List = JsArray();
            o.Item.List.Add(JsObj());
            o.Item.List[0].Hello = "sdf";

            string xy = Utility.ToJson(o);

            dynamic p = Utility.ToJsObj(xy);

            string h = p.Item.List[0].Hello;
          

            

            
            

            

            //_mjpeg.ParseStream(new Uri("http://192.168.1.14:8081"));
            TestWebApi();           
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            var rotatedImage = new TransformedBitmap(e.BitmapImage, new RotateTransform(90));

            
            ImageViewer.Source = rotatedImage;
        }

        void _mjpeg_Error(object sender, MjpegProcessor.ErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mjpeg.FrameReady -= mjpeg_FrameReady;

            BitmapSource bitmapImage = ImageViewer.Source as BitmapSource;

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);

            ImageViewer.Source = null;

            //now convert back
            imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);

            BitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);

            BitmapSource bitmapSource = decoder.Frames[0];

          

            ImageViewer.Source = bitmapSource;



        }

        private bool UploadFile(string PostURL)
        {
            try
            {
               // int contentLength = fileUpload.PostedFile.ContentLength;
                //byte[] data = new byte[contentLength];
                //fileUpload.PostedFile.InputStream.Read(data, 0, contentLength);

                // Prepare web request...
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                //webRequest.Method = "POST";
                //webRequest.ContentType = &quot; multipart / form - data & quot; ;
                //webRequest.ContentLength = data.Length;
                //using (Stream postStream = webRequest.GetRequestStream())
                //{
                //    // Send the data.
                //    postStream.Write(data, 0, data.Length);
                //    postStream.Close();
                //}
                return true;
            }
            catch (Exception ex)
            {
                //Log exception here...
                return false;
            }
        }
    }

    public static class Utility
    {
        public static string PngToBase64(string path)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(path));
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }

        public static BitmapSource Base64ToPng(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);

            BitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);

            BitmapSource bitmapSource = decoder.Frames[0];

            return bitmapSource;
        }

        public static ExpandoObject ParseObject(JsonElement jsonElement)
        {
            dynamic obj = new ExpandoObject();
            IDictionary<string, object> dict = obj;

            foreach (var o in jsonElement.EnumerateObject())
            {
                if (o.Value.ValueKind == JsonValueKind.Number)
                    dict[o.Name] = o.Value.GetDouble();
                else if (o.Value.ValueKind == JsonValueKind.String)
                    dict[o.Name] = o.Value.GetString();
                else if (o.Value.ValueKind == JsonValueKind.Object)
                    dict[o.Name] = ParseObject(o.Value);
                else if (o.Value.ValueKind == JsonValueKind.True || o.Value.ValueKind == JsonValueKind.False)
                    dict[o.Name] = o.Value.GetBoolean();
                else if (o.Value.ValueKind == JsonValueKind.Array)
                    dict[o.Name] = ParseArray(o.Value);
            }

            return obj;
        }

        public static List<object> ParseArray(JsonElement jsonElement)
        {
            var list = new List<object>();

            foreach (var o in jsonElement.EnumerateArray())
            {
                if (o.ValueKind == JsonValueKind.Number)
                    list.Add(o.GetDouble());
                else if (o.ValueKind == JsonValueKind.String)
                    list.Add(o.GetString());
                else if (o.ValueKind == JsonValueKind.Object)
                    list.Add(ParseObject(o));
                else if (o.ValueKind == JsonValueKind.True || o.ValueKind == JsonValueKind.False)
                    list.Add(o.GetBoolean());

            }

            return list;
        }

        public static object ToJsObj(string s)
        {
            var doc = JsonDocument.Parse(s);

            var jsonElement = doc.RootElement;
            if (jsonElement.ValueKind == JsonValueKind.Object)
                return ParseObject(jsonElement);
            else
                return ParseArray(jsonElement);
        }

        public static string ToJson(dynamic o)
        {
            return JsonSerializer.Serialize(o);
        }

        public static string ToJson(List<object> o)
        {
            return JsonSerializer.Serialize(o);
        }





    }


    //Added a comment

    public class Person
    {
        public string PK { get; set; }

        public string SK { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
    }

    public class DbPutRequest
    {
        public string TableName { get; set; }
        public object Item { get; set; }
    }

    public class DbPutResponse
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class DbGetRequest
    {
        public string Key { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool IsBase64Encoded { get; set; }
    }

    public class DbGetResponse
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class S3UploadRequest
    {
        public string Key { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool IsBase64Encoded { get; set; }
    }

    public class S3UploadResponse
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class S3DownloadRequest
    {
        public string Key { get; set; }
    }

    public class S3DownloadResponse
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }

    public class ReadExampleResponse
    {
        public string S3Result { get; set; }

        public MessageItem[] DbResult { get; set; }
    
    }

    public class DownloadImageData
    {
        public System.Windows.Controls.Image Image { get; set; }

        public string Key { get; set; }
    }

        


    public class MessageItem
    {
        public string Pk { get; set; }
        public string Sk { get; set; }

        public string Message { get; set; }

    }

    


}
