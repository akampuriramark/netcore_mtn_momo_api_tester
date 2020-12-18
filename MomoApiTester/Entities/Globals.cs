using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoApiTester.Entities
{
    public class Globals
    {
        // Am using different user and api keys for Disbursement and Collections. Why> you ask?
        // Personal preferance, decoupling of the two subscriptions and their implementation

        // Disbursement
        public const string DISBURSEMENT_API_USER = "b2e70972-a9ee-42ae-9034-22b6fa4d44d8";
        public const string DISBURSEMENT_API_KEY = "d59e4b6a6d254703a667458dba2f4307";
        public const string DISBURSEMENT_SUBSCRIPTION_KEY = "392da4360c57471383b2c9cf404177b2";

        // Collections
        public const string COLLECTION_API_USER = "754c1684-e502-4321-a333-d6d69da17e53";
        public const string COLLECTION_API_KEY = "d5a7e737cada4376ba6c53703f3fa950";
        public const string COLLECTION_PARTYID_TYPE = "MSISDN";
        public const string COLLECTION_SUBSCRIPTION_KEY = "becab31530e44d38a7bcbd60bcefc616";
        public static string COLLECTION_GET_TRAN_STATUS_URL = $"https://ericssonbasicapi2.azure-api.net/collection/v1_0/requesttopay/";
        public const string COLLECTION_GENERATE_TOKEN_URL = "https://ericssonbasicapi2.azure-api.net/collection/token/";
        public const string COLLECTION_HTTP_GET_URL = "https://ericssonbasicapi2.azure-api.net/collection/v1_0/requesttopay/{referenceId}?";
        public const string COLLECTION_REQUEST_2_PAY_URL = "https://ericssonbasicapi2.azure-api.net/collection/v1_0/requesttopay";
    }
}
