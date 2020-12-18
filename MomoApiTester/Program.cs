using MomoApiTester.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MomoApiTester
{
    class Program
    {
        //when you subscribe for a product, you get 2 keys...a primary and a secondary
        //put the primary key here (i hear either can work)
        private const string SubscriptionKey = Globals.COLLECTION_SUBSCRIPTION_KEY;

        //once you have an API user, you can then use the create API key endpoint
        //at to create an API key...put it here
        private const string ApiKey = Globals.COLLECTION_API_KEY;

        //before using the API, you must register an API user by firing a json request
        //to the create API user endpoint. I used postman to fire. Details here
        //
        private const string ApiUser = Globals.COLLECTION_API_USER;

        //to create an API key you must supply a call back host e.g localhost or example.com etc
        //so your call back url must be a sub url in that call back host otherwise it wont work
        private const string CallBackURL = "http://localhost/";

        static void Main(string[] args)
        {
            //before you can use the API methods you need a session token.
            //u use the token API to create one.
            string token = GenerateToken(SubscriptionKey);

            Console.WriteLine($"Token: {token}");

            //for the MTN API..the actual transaction ID is a UUID generated for
            //each and every request. Its what MTN requires to be unique for each request
            string transactionID = GenerateUUID();

            Console.WriteLine($"TransactionID: {transactionID}");

            //once you have the token you can now send a request to pay.
            //this API returns an empty response with http status code 202 (Accepted)
            Response payResponse = SendRequestToPay(transactionID, token, SubscriptionKey, CallBackURL);

            Console.WriteLine($"RequestToPayResponse (Empty is good): {payResponse.StatusCode} : {payResponse.ReasonPhrase}");

            //you need this last API to get the final status of a previously submitted
            //transaction. Remember the actual transaction ID is the UUID generated
            Response tranStatus = GetTransactionStatus(transactionID, token, SubscriptionKey);

            Console.WriteLine($"Transaction Status: {tranStatus.StatusCode} : {tranStatus.Content}");

            //leave
            Console.ReadLine();
        }

        private static Response GetTransactionStatus(string uuid, string token, string subscriptionKey)
        {
            string url = Globals.COLLECTION_GET_TRAN_STATUS_URL + uuid;//d18ab8ea-a1d8-454b-8fcb-1580441515f4";


            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {token}" },
                {"Ocp-Apim-Subscription-Key", subscriptionKey },
                {"X-Target-Environment", "sandbox" },
            };

            Response statusResponse = Task.Run(() => SendHttpGetRequest(url, headers)).Result;
            return statusResponse;
        }

        private static Response SendRequestToPay(string uuid, string token, string subscriptionKey, string callbackURL)
        {
            string url = Globals.COLLECTION_REQUEST_2_PAY_URL;
            string body = "{" +
                              "\"amount\": \"1000\"," +
                              "\"currency\": \"EUR\"," +
                              "\"externalId\": \"125677889945656675659678\"," +
                              "\"payer\": {" +
                                            "\"partyIdType\": \""+ Globals.COLLECTION_PARTYID_TYPE + "\"," +
                                            "\"partyId\": \"256784292383\"" +
                              "}," +
                              "\"payerMessage\": \"Test Payment 5\"," +
                              "\"payeeNote\": \"Test Payment 6\"" +
                            "}";

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"X-Reference-Id", uuid },
                {"Authorization", $"Bearer {token}" },
                {"Ocp-Apim-Subscription-Key", subscriptionKey },
                //{"X-Callback-Url", callbackURL }, // Removed callback URL till production
                {"X-Target-Environment", "sandbox" },
            };

            Response payResponse = Task.Run(() => SendHttpPostRequest(url, body, headers)).Result;
            return payResponse;
        }

        private static string GenerateToken(string subscriptionKey)
        {
            string url = Globals.COLLECTION_GENERATE_TOKEN_URL;
            string body = "";

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Basic " + GetBase64String($"{ApiUser}:{ApiKey}") },
                { "Ocp-Apim-Subscription-Key", subscriptionKey }
            };

            Response tokenJson = Task.Run(() => SendHttpPostRequest(url, body, headers)).Result;
            dynamic token = JsonConvert.DeserializeObject(tokenJson.Content);
            return token.access_token.ToString();
        }

        private static string GetBase64String(string plainstring)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainstring));
        }

        private static async Task<Response> SendHttpGetRequest(string url, Dictionary<string, string> headers)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Authorization", $"{headers["Authorization"]}");
            client.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

            var uri = Globals.COLLECTION_HTTP_GET_URL + queryString;

            var response = await client.GetAsync(url);

            return new Response()
            {
                StatusCode = response?.StatusCode.ToString(),
                Content = await (response?.Content?.ReadAsStringAsync()),
                ReasonPhrase = response?.ReasonPhrase.ToString(),
                Request = response?.RequestMessage?.ToString(),
                IsSuccessStatusCode = response?.IsSuccessStatusCode
            };
            //return await response.Content.ReadAsStringAsync();
        }

        private static async Task<Response> SendHttpPostRequest(string url, string body, Dictionary<string, string> headers)
        {
            var client = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Authorization", headers["Authorization"]);
            headers.Remove("Authorization");

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                foreach (var key in headers.Keys)
                {
                    content.Headers.Add(key, headers[key]);
                }
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(url, content);
            }
            return new Response()
            {
                StatusCode = response?.StatusCode.ToString(),
                Content = await (response?.Content?.ReadAsStringAsync()),
                ReasonPhrase = response?.ReasonPhrase.ToString(),
                Request = response?.RequestMessage?.ToString(),
                IsSuccessStatusCode = response?.IsSuccessStatusCode
            };
            //return await response?.Content?.ReadAsStringAsync();

        }

        private static string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
