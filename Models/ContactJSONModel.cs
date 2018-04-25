using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;


namespace Cloudant.Models
{
    
    public class ContactJSONHeader
    {
        public int Total_rows { get; set; }
        public int Offset { get; set; }
        public List<ContactJSONData> Rows { get; set; }
    }
 
    
    public class ContactJSONData
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public Value Value { get; set; }
        [JsonProperty("Doc")]
        public DocumentJSON Doc { get; set; }
    }
 
    public class Value
    {
        public string Rev { get; set; }
    }
 
    
    public class Contact
    {  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }


   
    [JsonObject("Doc")]
    public class DocumentJSON : Contact
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("_rev")]
        public string Rev { get; set; }
    }


    
}