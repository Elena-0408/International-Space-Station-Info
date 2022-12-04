using ISS_Info.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;


namespace ISS_Info.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
                      
        public string GetData(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            return response;
        }

        public RootCoordinates GetCoordinates()
        {
            
            string response = GetData("http://api.open-notify.org/iss-now.json");
             var result = JsonConvert.DeserializeObject<RootCoordinates>(response);
          
            return result;
        }

        public DateTime ConvertToUTC()
        {
            int timestamp = GetCoordinates().timestamp;
            DateTime date = new DateTime(1970, 1, 1).AddSeconds(timestamp);
            DateTime utcTime = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            return utcTime;
        }

        public IEnumerable<Person> GetPeopleInSpace()
        {
            string response = GetData("http://api.open-notify.org/astros.json");
            var resultJson = JsonConvert.DeserializeObject<RootAtronauts>(response);
            var result = resultJson.people.FindAll(p => p.craft == "ISS");
            return result;
        }
    }
}
    




