using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SimplySecureApi.Common.Exception
{
    public class ApiException : System.Exception
    {
        public HttpResponseMessage Response { get; set; }

        public ApiException(HttpResponseMessage response)
        {
            Response = response;
        }

        public HttpStatusCode StatusCode => Response.StatusCode;

        public IEnumerable<string> Errors => Data.Values.Cast<string>().ToList();

        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            try
            {
                var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                var exception = new ApiException(response);

                if (deserializedErrorObject.ModelState != null)
                {
                    var errors
                        = deserializedErrorObject.ModelState.Select
                            (keyValuePair => string.Join(". ", keyValuePair.Value));

                    for (var i = 0; i < errors.Count(); i++)
                    {
                        exception.Data.Add(i, errors.ElementAt(i));
                    }
                }
                else
                {
                    var error
                        = JsonConvert.DeserializeObject<Dictionary<string, string>>
                            (httpErrorObject);

                    foreach (var keyValuePair in error)
                    {
                        exception.Data.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                return exception;
            }
            catch (System.Exception ex)
            {
                return (ApiException)ex;
            }
        }
    }
}