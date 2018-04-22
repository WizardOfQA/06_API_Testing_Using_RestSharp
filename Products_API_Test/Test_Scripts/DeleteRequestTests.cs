using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp_API_TestFramework;
using RestSharp_API_TestFramework.Model;

namespace Products_API_Test.Test_Scripts
{
    [TestClass]
    public class DeleteRequestTests
    {
        static string _baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
        StudentDbContext _studentDbContext = new StudentDbContext();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string endPoint = "";

        [TestCategory("DELETE")]
        [TestMethod]
        public void Can_Delete_A_Student()
        {
            Student postData = new Student()
            {
                FirstName = "testFirstName",
                LastName = "testLastName",
                Email = "test@test.com",
                Phone = "3939992222",
                isActive = true
            };

            // Add a test data to DB
            _studentDbContext.Students.Add(postData);
            _studentDbContext.SaveChanges();


            endPoint = _baseUrl + "/" + postData.StudentId;


            // Delete the test data
            var result = API_Helper.DeleteRequest(endPoint, headers);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var fromDB = _studentDbContext.Students.Where(s => s.StudentId == postData.StudentId).FirstOrDefault();

            Assert.IsNull(fromDB); //Doesn't exist in DB
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
