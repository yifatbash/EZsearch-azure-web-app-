using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EZsearch3.Models;
using EZsearch3.BL;
using System.Web.Http.Filters;
using System.Web.Http.Results;
namespace EZsearch3.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        private readonly object request;

        /// <summary>
        /// The function saves the search details into tbl_history on ezsearchDB, using BL layer.
        /// </summary>
        /// <param name="search">A search entity. Contains the value and the date will be enter to the DB.</param>
        /// <returns>True- if added successfully, else- False.</returns>
        [HttpPost]
        [Route("saveToHistory")]
        public bool AddSearchToDB(SearchEnt search)
        {
            try
            {
                if (search == null)
                    return false;
                else
                {
                    return BL_IMP.AddSearchEntity(search);
                }
            }
            catch (ApplicationException e)
            {

                throw e;
            }
        }



        /// <summary>
        /// The function returns list of all the previous searches in JSON string format, order by date.
        /// </summary>
        [HttpGet]
        [Route("getHistoryList")]
        public string GetHistoryList()
        {
            try
            {
                var history = BL_IMP.GetHistoryList();
                var jhistory = Newtonsoft.Json.JsonConvert.SerializeObject(history);

                return jhistory;
            }
            catch (ApplicationException e)
            {
                throw e;
            }

        }

        /// <summary>
        /// The function makes a request to the Bing Web Search API and returns data as a JSON string.
        /// </summary>
        /// <param name="searchQuery">value </param>
        /// <returns>List of results in JSON string format</returns>
        [HttpGet]
        [Route("bing")]
        public string BingWebSearchApi(string searchQuery)
        {
            try
            {
                //My subscription key.
                const string accessKey = "bcbadc4bbb194542962610f48099d491";
                const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
                // Construct the search request URI.
                var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

                // Perform request and get a response.
                WebRequest request = HttpWebRequest.Create(uriQuery);
                request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
                return json;
            }
            catch (ApplicationException e)
            {

                throw e;
            }

        }
    }

}

