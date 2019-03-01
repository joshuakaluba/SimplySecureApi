using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Authentication
{
    public class Token
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("expiryDate")]
        public System.DateTime ExpiryDate { get; set; }

        [JsonProperty("dateUserCreated")]
        public System.DateTime DateUserCreated { get; set; }
    }
}