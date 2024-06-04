using PayPal.Api;
using System;
using System.Collections.Generic;

namespace NetFramwork_WildNature.Db
{
    public static class PaypalConfiguration
    {
        // Variables for storing the clientID and clientSecret key  
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Constructor  
        static PaypalConfiguration()
        {
            var config = GetConfig();
            if (config.ContainsKey("clientId") && config.ContainsKey("clientSecret"))
            {
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }
            else
            {
                throw new Exception("ClientId or ClientSecret is missing in the configuration.");
            }
        }

        // Getting properties from the web.config  
        public static Dictionary<string, string> GetConfig()
        {
            try
            {
                return PayPal.Api.ConfigManager.Instance.GetProperties();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving PayPal configuration: " + ex.Message);
            }
        }

        private static string GetAccessToken()
        {
            // Getting access token from PayPal  
            try
            {
                string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
                return accessToken;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting access token: " + ex.Message);
            }
        }

        public static APIContext GetAPIContext()
        {
            // Return APIContext object by invoking it with the access token  
            try
            {
                APIContext apiContext = new APIContext(GetAccessToken());
                apiContext.Config = GetConfig();
                return apiContext;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating APIContext: " + ex.Message);
            }
        }
    }
}
