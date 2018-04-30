using System.Net;
using System.Net.Http;

namespace Cloudant.Models
{
    public class ResponseModel : IResponseModel
    {
        private HttpStatusCode statusCode;
        public HttpStatusCode StatusCode { get => statusCode; set => statusCode = value; }

        private HttpRequestMessage requestMessage;
        public HttpRequestMessage RequestMessage { get => requestMessage; set => requestMessage = value; }

        private bool isError;
        public bool IsError { get => isError; set => isError = value; }
    }
}