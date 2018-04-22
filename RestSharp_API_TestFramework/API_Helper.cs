using RestSharp;
using RestSharp_API_TestFramework.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_API_TestFramework
{
    public class API_Helper
    {
        
         public static string _baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
             
        
        private static IRestResponse CallingAPI(Method httpMethod, Dictionary<string, string> headers, string endPoint, string jsonData="")
        {
            var client = new RestClient(endPoint);
            var request = new RestRequest(httpMethod);
            if(headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            if (httpMethod == Method.PUT || httpMethod == Method.POST)
            {
                request.AddParameter(headers["content-type"], jsonData, ParameterType.RequestBody);
            }
            IRestResponse response = client.Execute(request);
            return response;
        }

        
        // GET Request
        public static IRestResponse GetRequest(string endPoint, Dictionary<string, string> headers)
        {
            return CallingAPI(Method.GET, headers, endPoint);
        }


        //POST Request
        public static IRestResponse PostRequest(string endPoint, Dictionary<string, string> headers, string jsonData)
        {
            return CallingAPI(Method.POST, headers, endPoint, jsonData);
        }

        // PUT Request
        public static IRestResponse PutRequest(string endPoint, Dictionary<string, string> headers, string jsonData)
        {
            return CallingAPI(Method.PUT, headers, endPoint, jsonData);
        }

        // DELETE Request
        public static IRestResponse DeleteRequest(string endPoint, Dictionary<string, string> headers)
        {
            return CallingAPI(Method.DELETE, headers, endPoint);
        }



        public static bool Check3Spots(List<Student> first, List<Student> second)
        {
            if (first.Count() != second.Count())
            {
                return false;
            }
            return (first.First().stEquals(second.First()) &&
                    (first[first.Count / 2].stEquals(second[second.Count / 2])) &&
                    (first.Last().stEquals(second.Last())));
        }
    }
}
