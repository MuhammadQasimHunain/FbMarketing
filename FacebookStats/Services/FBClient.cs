using FacebookStats.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FacebookStats.Services
{
    public class FBClient
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static string Get(string url, string fields)
        {

            url = @"https://graph.facebook.com/v10.0/" + url + @"?access_token=EAADfq6jGkYoBALHeiCkpbAZCkWZARUYLnB4ZAFExsmB6zeVyeNXgIpZCRKbZBYVQGYgaQZCtLVwyfUXnSRBxLBfmwawhWVjwXBm23X1cB1zZA9v8tZAZC34UBMe6Xf04KAllirdY5E9hsZBNDmkHW0ZBaBim24gb5FkiCnyedC7S4KlCKj5807fVexpCHAolLKKMacZD&debug=all&" + fields + @"&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors";

            var request = WebRequest.Create(url);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();
            webResponse.Close();
            webStream.Close();
            return data;
        }


        public static string Post(string url, string fields, dynamic data)
        {
            using (var client = new HttpClient())
            {
                url = @"https://graph.facebook.com/v10.0/" + url + @"?access_token=EAADfq6jGkYoBALHeiCkpbAZCkWZARUYLnB4ZAFExsmB6zeVyeNXgIpZCRKbZBYVQGYgaQZCtLVwyfUXnSRBxLBfmwawhWVjwXBm23X1cB1zZA9v8tZAZC34UBMe6Xf04KAllirdY5E9hsZBNDmkHW0ZBaBim24gb5FkiCnyedC7S4KlCKj5807fVexpCHAolLKKMacZD&debug=all&" + fields + @"&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors";

                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

                var response = client.PostAsync(url, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            return string.Empty;
        }

        public static HttpWebResponse MultipartFormDataPost(Dictionary<string, object> postParameters)
        {
           string postUrl = @"https://graph.facebook.com/v10.0/act_997931414279422/adimages?access_token=EAADfq6jGkYoBAHvujz0pbUaZBLgO8LvLYrLmeFqAifV8decc47YzG7n7SJYIpEvnZAMkZBVzqxRZB4T77w2dCPThBBgoqvRH7zVa0XbqwxNy6ZC5K9ZAcHDJFdx7fmrMpIp2npANgJUHZC5nTcFoM59KjbXhjkpUaRgaJO4UlRKhTn5dXxFLlwN5YuBz26i0X0ZD";

            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, string.Empty, contentType, formData);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }


    }

    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
}