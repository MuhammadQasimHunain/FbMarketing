using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookStats.Models.FacebookModels
{
    public class AccountInfo
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AdAccountInfo
    {
        public AdAccounts adaccounts { get; set; }
        public string id { get; set; }
    }
    public class Preview
    {
        public List<PreviewData> data { get; set; }
    }
    public class PreviewData
    {
        public string body { get; set; }
    }
    public class AdAccounts
    {
        public AdAccount[] data { get; set; }
        public Paging paging { get; set; }
    }
    public class AdAccount
    {
        public string account_id { get; set; }
        public string id { get; set; }
    }
    public class Paging
    {
        public Cursors cursors { get; set; }
    }
    public class Cursors {
        public string before { get; set; }
        public string after { get; set; }
    }
}