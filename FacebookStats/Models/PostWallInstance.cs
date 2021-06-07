using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookStats.Models
{
    public class PostWallInstance
    {
        public int Id { get; set; }
        public string AdName { get; set; }
        public string HeaderMessage { get; set; }
        public string WebSiteDisplayURL { get; set; }
        public string WebSiteURL { get; set; }
        public string MaxBudget { get; set; }
        public string ImageFiles { get; set; }
        public string ImageHash { get; set; }
        public string Locations { get; set; }
        public string PageId { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string AdId { get; set; }
        public bool IsValid()
        {
            return
                !string.IsNullOrEmpty(AdName) &&
                !string.IsNullOrEmpty(HeaderMessage) &&
                !string.IsNullOrEmpty(WebSiteDisplayURL) &&
                !string.IsNullOrEmpty(WebSiteURL) &&
                !string.IsNullOrEmpty(MaxBudget) &&
                !string.IsNullOrEmpty(ImageFiles) &&
                !string.IsNullOrEmpty(Locations) &&
                !string.IsNullOrEmpty(DateStart) &&
                !string.IsNullOrEmpty(DateEnd) &&
                !string.IsNullOrEmpty(ImageHash) &&
                !string.IsNullOrEmpty(PageId);
        }
    }
}