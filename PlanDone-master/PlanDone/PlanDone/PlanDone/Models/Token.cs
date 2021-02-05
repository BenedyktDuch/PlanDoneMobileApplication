using Newtonsoft.Json;

namespace PlanDone.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = ".issued")]
        public string Issued{ get; set; }

        [JsonProperty(PropertyName = ".expires")]
        public string Expired { get; set; }

        public override string ToString()
        {
            return  "\n TOKEN INFORMATION: "+
                    "\n Token: " + AccessToken + 
                    "\n Type: " +Type+
                    "\n ExpiersIn: " + ExpiresIn+
                    "\n Username: " + Username +
                    "\n Issued: " +Issued+
                    "\n Expired: " + Expired;
        }

    }
}
