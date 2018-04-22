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
    public class PostRequestTests
    {
        static string _baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
        StudentDbContext _studentDbContext = new StudentDbContext();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string endPoint = "";

        [TestCategory("POST")]
        [TestMethod]
        public void Can_Create_A_New_Student()
        {
            endPoint = _baseUrl;
            Student postData = new Student()
            {
                FirstName = "testFirstName",
                LastName = "testLastName",
                Email = "test@test.com",
                Phone = "3939992222",
                isActive = true
            };

            var jsonData = JsonConvert.SerializeObject(postData);
            headers.Add("content-type", "application/json");
            var result = API_Helper.PostRequest(endPoint, headers, jsonData);

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var fromDB = _studentDbContext.Students.OrderByDescending(s => s.StudentId).FirstOrDefault();
            postData.StudentId = fromDB.StudentId;
            Assert.IsTrue(postData.stEquals(fromDB));

            //clean up the test data
            _studentDbContext.Students.ToList().RemoveAll(s => s.Email == "test@test.com");
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
