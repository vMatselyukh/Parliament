using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParliamentController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetConfig()
        {            
            return Request.CreateResponse(HttpStatusCode.OK, ConfigHelper.GetConfig(), "application/json");
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetFile(string url)
        {
            var pathElements = url.Split(Path.DirectorySeparatorChar).ToList();
            pathElements.Remove("content");

            var filePath = Path.Combine(ConfigHelper.ContentPath, Path.Combine(pathElements.ToArray()));
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }
    }
}