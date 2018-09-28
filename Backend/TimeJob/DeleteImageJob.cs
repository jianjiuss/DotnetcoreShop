using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.TimeJob
{
    public class DeleteImageJob
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DeleteImageJob(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void Start(IServiceProvider serviceProvider)
        {
            Task.Factory.StartNew(() => {
                while(true)
                {
                    var client = _httpClientFactory.CreateClient();
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                        List<string> stayImageName = new List<string>();
                        var products = context.Products.ToList();

                        foreach (var item in products)
                        {
                            string name = Path.GetFileName(item.IconImageUrl);
                            stayImageName.Add(name);
                        }

                        var images = context.Images.ToList();
                        foreach(var item in images)
                        {
                            string name = Path.GetFileName(item.ImageUrl);
                            stayImageName.Add(name);
                        }

                        StringBuilder sb = new StringBuilder(string.Empty);
                        foreach(var name in stayImageName)
                        {
                            sb.Append(name + ";");
                        }
                        string fileNames = sb.ToString().TrimEnd(';');

                        client.DefaultRequestHeaders.Add("token", Util.GlobalVariable.ImageServerToken);
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("filenames", fileNames));
                        var result = client.PostAsync(Util.GlobalVariable.ImageServer + "/file/deleteFile", new FormUrlEncodedContent(keyValuePairs));
                        result.Wait();
                        var response = result.Result;
                    }
                    Thread.Sleep(TimeSpan.FromHours(1));
                }
                
            });
        }
    }
}
