using DataCrawling_Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Service.Api
{
    public class ApiService
    {
        public ApiService() { } 

        public ApiService(string apiKey) { }


        public List<DataApiModel> GenerateSampleData(int count)
        {
            var sampleData = new List<DataApiModel>();

            for (int i = 1; i <= count; i++)
            {
                var data = new DataApiModel
                {
                    No = i,
                    Type = i % 2 == 0 ? "공공" : "마이플랫폼",
                    Name = $"Sample API {i}",
                    Description = $"Sample Description {i}",
                    IsFree = i % 2 == 0,
                    담당자 = $"Owner {i}",
                    프로토콜 = "HTTP",
                    HTTP메서드 = "GET",
                    요청URL = $"https://api.example.com/{i}",
                    제공타입 = new DataFormat[] { DataFormat.JSON, DataFormat.XML },
                };

                sampleData.Add(data);
            }

            return sampleData;
        }
    }
}