using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookStats.Models.FacebookModels
{
    public class CampaignWallInstance
    {
        public CampaignFBModel Campaign { get; set; }
        public AdSetRequest AdSet { get; set; }
        public AdCreativeRequest AdCreative { get; set; }
        public Ad ad { get; set; }

        public CampaignWallInstance(PostWallInstance postWallInstance)
        {
            Campaign = new CampaignFBModel();
            AdSet = new AdSetRequest();
            AdCreative = new AdCreativeRequest();
            ad = new Ad();
            AdSet.targeting.geo_locations.countries = new List<string>();

            AdSet.lifetime_budget = 0;
            AdSet.daily_budget = 0;
            AdSet.start_time = DateTime.Now;
            AdSet.end_time = DateTime.Now;
            foreach (var item in postWallInstance.Locations.Split(';'))
            {
                AdSet.targeting.geo_locations.countries.Add(item);
            }

            AdCreative.object_story_spec.page_id = postWallInstance.PageId;
            AdCreative.object_story_spec.link_data.image_hash = postWallInstance.ImageFiles;
            AdCreative.object_story_spec.link_data.message = postWallInstance.HeaderMessage;


            ad.name = postWallInstance.AdName;
        }

        public CampaignWallInstance()
        {
            Campaign = new CampaignFBModel();
            AdSet = new AdSetRequest();
            AdCreative = new AdCreativeRequest();
            ad = new Ad();
        }

    }


    public class CampaignandAd
    {
        public string CampaignID { get; set; }
        public List<string> CreativeID { get; set; }
        public string AdsetID { get; set; }
        public string AdID { get; set; }
        public string AdAccountID { get; set; }
        public string Message { get; set; }
    }

    public class CampaignFBModel
    {
        public string name { get; set; } = "Campaign "  + DateTime.Now.ToString();
        public int cost_per_click { get; set; } = 10;
        public int cost_per_visit { get; set; } = 10;
        public string special_ad_categories { get; set; } = "NONE";
        public string objective { get; set; } = "LINK_CLICKS";
    }


    public class CampaignResponseModel
    {
        public List<CampaignId> data { get; set; }
        public Paging paging { get; set; }
    }
    public class CampaignId
    {
        public string id { get; set; }
    }
    public class AdsetResponseModel
    {
        public List<AdsetRep> data { get; set; }
        public Paging paging { get; set; }
    }
    public class AdsetRep
    {
        public string name { get; set; }
        public string lifetime_budget { get; set; }
        public string id { get; set; }
    }


    public class AdSetRequest
    {
        public string name { get; set; } = @"Adset " + DateTime.Now;
        public decimal lifetime_budget { get; set; }
        public decimal daily_budget { get; set; }
        public DateTime start_time { get; set; }
        public string optimization_goal { get; set; } = @"POST_ENGAGEMENT";
        public string billing_event { get; set; } = @"IMPRESSIONS";
        public string campaign_id { get; set; }
        public DateTime end_time { get; set; } 
        public string status { get; set; } = @"PAUSED";
        public Targeting targeting { get; set; }
        public AdSetRequest()
        {
            targeting = new Targeting();
        }
    }
    public class Targeting
    {
        public GeoLocations geo_locations { get; set; }
        public Targeting()
        {
            geo_locations = new GeoLocations();
        }
    }
    public class GeoLocations
    {
        public List<string> countries { get; set; }
    }

    public class AdCreativeRequest
    {
        public string name { get; set; } = @"Creative "+ DateTime.Now.ToString();
        public StorySpec object_story_spec { get; set; }

        public AdCreativeRequest()
        {
            object_story_spec = new StorySpec();
        }
    }
    public class StorySpec
    {
        public string page_id { get; set; }
        public LinkData link_data { get; set; }

        public StorySpec()
        {
            link_data = new LinkData();
        }
    }
    public class LinkData
    {
        public string image_hash { get; set; }
        public string link { get; set; }
        public string message { get; set; }
    }

    public class Ad
    {
        public string name { get; set; }
        public string status { get; set; } = @"PAUSED";
        public string adset_id { get; set; }
        public string creative { get; set; }
    }
}
