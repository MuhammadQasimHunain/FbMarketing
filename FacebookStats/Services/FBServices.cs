using Facebook;
using FacebookStats.Models;
using FacebookStats.Models.FacebookModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FacebookStats.Services
{

    public class FBServices
    {
        public static AccountInfo GetAccountInfo()
        {
            FacebookClient app = new FacebookClient(FacebookSettings.AccessToken);
            dynamic parameters = new ExpandoObject();
            app.AppId = FacebookSettings.AppId;
            JsonObject result = (JsonObject)app.Get("me");
            if (result == null)
            {
                return null;
            }

            return new AccountInfo
            {
                Id = ((JsonObject)result)["id"] + "",
                FirstName = ((JsonObject)result)["first_name"] + "",
                LastName = ((JsonObject)result)["last_name"] + ""
            };
        }

        public static CampaignandAd CreateCampaignAndAd(CampaignWallInstance campaignWallInstance)
        {
            CampaignFBModel campaignFBModel = campaignWallInstance.Campaign;
            CampaignandAd campaignandAd = new CampaignandAd();
            // get adAccountID
            var adAccount = GetAdAccounts();
            if (adAccount == null)
            {
                campaignandAd.Message = "Dont Have an Ad Account ID.";
                return campaignandAd;
            }
            string adAccountID = adAccount.adaccounts.data.FirstOrDefault()?.id;
            campaignandAd.AdAccountID = adAccountID;
            dynamic jsonString = JsonConvert.SerializeObject(campaignFBModel);
            // campaign
            var campaignid = CreateCampaignData(adAccountID, jsonString);
            if (string.IsNullOrEmpty(campaignid))
            {
                campaignandAd.Message = @"Can't create campaign.";
            }
            campaignandAd.CampaignID = campaignid;

            // adrequest
            AdSetRequest adSetRequest = campaignWallInstance.AdSet;
            adSetRequest.campaign_id = campaignid;
            var adsets = CreateAdsetData(adAccountID, adSetRequest);
            if (adsets == null)
            {
                campaignandAd.Message = @"Can't create Adset.";
            }
            else
            {
                string adsetid = adsets.data.FirstOrDefault()?.id;
                campaignandAd.AdsetID = adsetid;
            }

            // ad creative
            AdCreativeRequest adCreativeRequest = campaignWallInstance.AdCreative;
            CampaignId adcreativeresponse = CreateAdcreatives(adAccountID, adCreativeRequest);
            if (adcreativeresponse == null)
            {
                campaignandAd.Message = @"Can't create creative";
            }
            campaignandAd.CreativeID = new List<string>();
            campaignandAd.CreativeID.Add(adcreativeresponse.id);

            //ad
            Ad ad = campaignWallInstance.ad;
            ad.adset_id = campaignandAd.AdsetID;
            ad.creative = campaignandAd.CreativeID.FirstOrDefault();
            var adresponse = CreateAd(adAccountID, ad);
            if (adresponse == null)
            {
                campaignandAd.Message = @"Can't create Adset.";
            }
            else
            {
                string adid = adresponse.id;
                campaignandAd.AdID = adid;
            }

            campaignandAd.Message = "Ad is created Successfully.";
            return campaignandAd;
        }


        public static string GetPreview(string adID)
        {
            string result = FBClient.Get(adID+ "/previews", @"ad_format=DESKTOP_FEED_STANDARD");
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                Preview peview = JsonConvert.DeserializeObject<Preview>(result);
                return peview.data?.FirstOrDefault()?.body;
            }
        }
        public static AdAccountInfo GetAdAccounts()
        {
            string result = FBClient.Get(FacebookSettings.UserId, @"fields=adaccounts");
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                AdAccountInfo AdAccount = JsonConvert.DeserializeObject<AdAccountInfo>(result);
                return AdAccount;
            }
        }

        public static string CreateCampaignData(string accountID, dynamic data)
        {
            string result = FBClient.Post(accountID + "/campaigns", string.Empty, data);
            if (!string.IsNullOrEmpty(result))
            {
                CampaignResponseModel AdAccount = JsonConvert.DeserializeObject<CampaignResponseModel>(result);
                return AdAccount.data.FirstOrDefault().id;
            }
            else
            {
                return result;
            }
        }


        public static AdsetResponseModel CreateAdsetData(string accountID, dynamic data)
        {
            string fields = @"fields=name,lifetime_budget,id";
            string result = FBClient.Post(accountID + "/adsets", fields, data);
            if (!string.IsNullOrEmpty(result))
            {
                AdsetResponseModel adset = JsonConvert.DeserializeObject<AdsetResponseModel>(result);
                return adset;
            }
            else
            {
                return null;
            }
        }

        public static CampaignId CreateAdcreatives(string accountID, dynamic data)
        {
            string result = FBClient.Post(accountID + "/adcreatives", string.Empty, data);
            if (!string.IsNullOrEmpty(result))
            {
                CampaignResponseModel adset = JsonConvert.DeserializeObject<CampaignResponseModel>(result);
                return adset.data.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }


        public static CampaignId CreateAd(string accountID, dynamic data)
        {
            string result = FBClient.Post(accountID + "/ads", string.Empty, data);
            if (!string.IsNullOrEmpty(result))
            {
                CampaignResponseModel adset = JsonConvert.DeserializeObject<CampaignResponseModel>(result);
                return adset.data.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static string CreateAdImage(string fileName)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("filename", fileName);
            HttpWebResponse result = FBClient.MultipartFormDataPost(keyValuePairs);
            if (result != null)
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(result.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
            else
            {
                return string.Empty;
            }

        }

        public static dynamic Post(string message, string attribution, string address)
        {
            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            app.AccessToken = FacebookSettings.AccessToken;
            app.AppId = FacebookSettings.AppId;
            parameters.message = message;
            parameters.attribution = attribution;

            dynamic result = app.Post(address, parameters);

            return result;
        }
    }
}