using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp_API_TestFramework;
using RestSharp_API_TestFramework.Model;

namespace Products_API_Test.Test_Scripts
{
    [TestClass]
    public class PutRequestTests
    {
        static string _baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
        StudentDbContext _studentDbContext = new StudentDbContext();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string endPoint = "";

        [TestCategory("PUT")]
        [TestMethod]
        public void Can_Update_Existing_Student_Info()
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

            Student updatedData = new Student()
            {
                FirstName = "xxxFirstName",
                LastName = "xxxLastName",
                Email = "xxx@test.com",
                Phone = "0000000000",
                isActive = false
            };

            updatedData.StudentId = postData.StudentId;
            endPoint = _baseUrl + "/" + updatedData.StudentId;

            var jsonData = JsonConvert.SerializeObject(updatedData);
            headers.Add("content-type", "application/json");

            // Update the test data
            var result = API_Helper.PutRequest(endPoint, headers, jsonData);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            // Get the updated data from DB
            var fromDB = _studentDbContext.Students.Where(s => s.StudentId == updatedData.StudentId).FirstOrDefault();

            // Refresh DbContext so that it has the most updated data.
            (((IObjectContextAdapter)_studentDbContext).ObjectContext).Refresh(RefreshMode.StoreWins, fromDB);

            Assert.IsTrue(updatedData.stEquals(fromDB));
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
