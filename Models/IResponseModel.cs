using System.Net;
using System.Net.Http;

namespace Cloudant.Models
{
    public interface IResponseModel
    {
         HttpStatusCode StatusCode {get; set;}

         HttpRequestMessage RequestMessage {get; set;}

         bool IsError {get; set;}
    }
}