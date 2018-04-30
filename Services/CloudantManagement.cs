using System.Threading.Tasks;
using Cloudant.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Cloudant
{

    public class CloudantManagement : IDatabaseProvider
    {
        private readonly string _dbName;

        private readonly UrlEncoder _urlEncoder;

        private readonly IConfiguration _applicationConfiguration;

        private readonly IResponseModel _responseModel;

        public CloudantManagement(UrlEncoder urlEncoder, IConfiguration applicationConfiguration, IResponseModel responseModel)
        { 
            _applicationConfiguration = applicationConfiguration;
            
            _urlEncoder = urlEncoder;

            _responseModel = responseModel;

            _dbName = _applicationConfiguration["Cloudant:databasename"];
        }


        private HttpClient CreateCloudantClient()
        {
            string dbUserName = _applicationConfiguration["Cloudant:username"];
            string dbUserPassword = _applicationConfiguration["Cloudant:password"];
            string dbHost = _applicationConfiguration["Cloudant:host"];

            var authenticate = Convert.ToBase64String(Encoding.ASCII.GetBytes(dbUserName + ":" + dbUserPassword));

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://" + dbHost);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticate);

            return httpClient;
        }

        public async Task<IResponseModel> CreateAsync(Contact model)
        {
            using(var cloudantClient = CreateCloudantClient())
            {      
                var response = await cloudantClient.PostAsync(_dbName, new StringContent(JsonConvert.SerializeObject(model, Formatting.Indented), Encoding.UTF8, "application/json"));

                if(response.IsSuccessStatusCode)
                {
                    var dataAsString = await response.Content.ReadAsStringAsync();

                    var responseJson = JsonConvert.DeserializeObject<Contact>(dataAsString);

                    _responseModel.IsError = false;
                }

                _responseModel.RequestMessage = response.RequestMessage;
                _responseModel.StatusCode = response.StatusCode;
                
                return _responseModel;
            }
        }

        public async Task<DocumentJSON> GetDocumentById(string id)
        {
            string _content = null;

            using(var cloudantClient = CreateCloudantClient())
            {
                var response = await cloudantClient.GetAsync(_dbName + "/" + id);

                if (response.IsSuccessStatusCode)
                {
                    _content = await response.Content.ReadAsStringAsync();
                    DocumentJSON contact = JsonConvert.DeserializeObject<DocumentJSON>(_content);
                    
                    return contact;
                }

                return null;
            }
        }

        public async Task<List<ContactViewModel>> ListAll()
        {
            List<ContactViewModel> allContacts = new List<ContactViewModel>();

            using(var cloudantClient = CreateCloudantClient())
            {
                var response = await cloudantClient.GetAsync(_dbName + "/_all_docs?include_docs=true");

                if (response.IsSuccessStatusCode)
                {
                    var _requestContent = await response.Content.ReadAsStringAsync();
                    ContactJSONHeader contactsHeaders = JsonConvert.DeserializeObject<ContactJSONHeader>(_requestContent);

                    foreach(ContactJSONData jsonData in contactsHeaders.Rows)
                    {
                        allContacts.Add(new ContactViewModel(){
                            Id = jsonData.Doc.Id,
                            Rev = jsonData.Doc.Rev,
                            FirstName = jsonData.Doc.FirstName,
                            LastName = jsonData.Doc.LastName,
                            Email = jsonData.Doc.Email
                        });
                    }
                }
            }

            return allContacts;
        }

        public async Task<IResponseModel> UpdateAsync(DocumentJSON model)
        {
            using(var cloudantClient = CreateCloudantClient())
            {
                var response = await cloudantClient.PutAsync(_dbName + "/" + _urlEncoder.Encode(model.Id) + "?rev=" + _urlEncoder.Encode(model.Rev), 
                                                                new StringContent(JsonConvert.SerializeObject(model, Formatting.Indented), Encoding.UTF8, "application/json"));

                if(response.IsSuccessStatusCode)
                {
                    _responseModel.IsError = false;
                }

                _responseModel.RequestMessage = response.RequestMessage;
                _responseModel.StatusCode = response.StatusCode;
     
                return _responseModel;
            }
        }


        public async Task<IResponseModel> DeleteAsync(string id, string rev)
        {
            using(var cloudantClient = CreateCloudantClient())
            {
                var response = await cloudantClient.DeleteAsync(_dbName + "/" + _urlEncoder.Encode(id) + "?rev=" + _urlEncoder.Encode(rev));
                {
                    if(response.IsSuccessStatusCode)
                    {
                        _responseModel.IsError = false;
                    }

                    _responseModel.RequestMessage = response.RequestMessage;
                    _responseModel.StatusCode = response.StatusCode;
     
                    return _responseModel;      
                }   
            }
        }
    }

}