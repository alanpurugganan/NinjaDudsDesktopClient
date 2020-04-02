using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NinjaDudsDesktopClient
{
    class NinjaDudsAwsApi
    {
        public static NinjaDudsAwsApi Current { get; protected set; }

        public static NinjaDudsAwsApi Init(bool local = false)
        {
            if (Current == null)
            {
                Current = new NinjaDudsAwsApi();
                Current.InternalInit(local);
            }

            return Current;
        }

        public void InternalInit(bool local = false)
        {
            this.HttpClient = new HttpClient();

            this.HttpClient.BaseAddress = (local)
                ? new Uri("http://127.0.0.1:3000")
                : new Uri("https://xwvhb77s80.execute-api.us-east-1.amazonaws.com");

            this.JsonSerializerOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            this.Local = local;
        }

        protected bool Local { get; set; }

        protected string ParentFolder => (Local) ? string.Empty : "/Prod";
        
        protected HttpClient HttpClient { get; set; }

        protected JsonSerializerOptions JsonSerializerOptions {get;set;}

        protected Task<HttpResponseMessage> HttpPost<T>(T request, string resource)
        {
            string json = JsonSerializer.Serialize<T>(request, JsonSerializerOptions);
            string path = ParentFolder + "/" + resource;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return HttpClient.PostAsync(path, content);
        }

        protected Task<HttpResponseMessage> HttpGet<T>(T request, string resource)
        {
            string json = JsonSerializer.Serialize<T>(request, JsonSerializerOptions);
            string path = ParentFolder + "/" + resource;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return HttpClient.GetAsync(path);
        }

        public async Task<S3UploadResponse> S3UploadAsync(S3UploadRequest request)
        {
            var result = await HttpPost(request, "s3-upload");
            
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(result.ReasonPhrase);

            string json = await result.Content.ReadAsStringAsync();
            var responseBody = JsonSerializer.Deserialize<S3UploadResponse>(json, JsonSerializerOptions);
            return responseBody;
        }

    }
}
