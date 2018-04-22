using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp_API_TestFramework;
using RestSharp_API_TestFramework.Model;

namespace Products_API_Test.Test_Scripts
{
    [TestClass]
    public class GetRequestTests
    {
        static string _baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
        StudentDbContext _studentDbContext = new StudentDbContext();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string endPoint = "";


        [TestCategory("GET")]
        [TestMethod]
        public void Can_Retrieve_All_Students()
        {
            endPoint = _baseUrl;
            headers.Add("content-type", "application/json");
            var result = API_Helper.GetRequest(endPoint, headers);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var fromAPI = JsonConvert.DeserializeObject<List<Student>>(result.Content).ToList();
            var fromDB = _studentDbContext.Students.ToList();

            Assert.IsTrue(API_Helper.Check3Spots(fromAPI, fromDB));
        }

        [TestCategory("GET")]
        [TestMethod]
        public void Can_Retrieve_Specific_Student()
        {
            int studentPicked;
            var ids = _studentDbContext.Students.Select(s => s.StudentId).ToArray();
            int randomID = ids.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (ids.Count() >= 1)
                studentPicked = randomID;
            else
                throw new Exception("There is no data to test in DB");

            endPoint = _baseUrl + "/" + studentPicked;
            headers.Add("content-type", "application/json");
            var result = API_Helper.GetRequest(endPoint, headers);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var fromAPI = JsonConvert.DeserializeObject<Student>(result.Content);
            var fromDB = _studentDbContext.Students.Where(s => s.StudentId == randomID).ToList().FirstOrDefault();

            Assert.IsTrue(fromAPI.stEquals(fromDB));
        }

        [TestCleanup]
        public void TestClean()
        {
            headers.Clear();
            endPoint = string.Empty;
            //clean up the test data
            _studentDbContext.Students.RemoveRange(_studentDbContext.Students.Where(s => s.Email.Contains("@test.com")));
            _studentDbContext.SaveChanges();
        }
    }
}
