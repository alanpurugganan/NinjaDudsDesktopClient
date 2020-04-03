using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

        protected string PngToBase64(string path)
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

        protected BitmapSource Base64ToPng(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);

            BitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);

            BitmapSource bitmapSource = decoder.Frames[0];

            return bitmapSource;
        }


        public async void TestWebApi()
        {
            NinjaDudsAwsApi.Init(true);
            NinjaDudsAwsApi api = NinjaDudsAwsApi.Current;

            
            var s3UploadRequest = new S3UploadRequest()
            {
                Key = "test599.png",
                Content = PngToBase64(@"C:\Users\blank\Pictures\image.png"),
                IsBase64Encoded = true,
                ContentType = "image/png"
            };

            var s3DownloadRequest = new S3DownloadRequest()
            {
                Key = "test59.png",
            };

            try
            {

                var s3UploadResponse = await api.S3UploadAsync(s3UploadRequest);
                var s3DownloadResponse = await api.S3DownloadAsync(s3DownloadRequest);
                ImageViewer.Source = Base64ToPng(s3DownloadResponse.Content);

            }
            catch (Exception ex)
            {
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

        public MainWindow()
        {
            InitializeComponent();

            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;

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

    //Added a comment

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
        public string Message { get; set; }
        public string ContentType { get; set; }
        public bool IsBase64Encoded { get; set; }
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

        


    public class MessageItem
    {
        public string Pk { get; set; }
        public string Sk { get; set; }

        public string Message { get; set; }

    }


}
