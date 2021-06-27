using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;

namespace CoreHelpers
{
    public class SpamFilterAttribute : ActionFilterAttribute
    {
        public SpamFilterResultTypes ResultType = SpamFilterResultTypes.Json;
        public int DelayRequestInSeconds = 10; // in seconds
        public string ErrorMessageKey = "";
        public string SubmitedFormInfoField = "";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var cache = filterContext.HttpContext.Cache;
            //Grab the IP Address from the originating Request
            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? (request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress);
            originationInfo += request.UserAgent;
            //Now we just need the target URL Information
            var targetInfo = request.RawUrl + request.QueryString;
            
            // hash:
            var hashValue = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));
            //Checks if the hashed value is contained in the Cache (indicating a repeat request)
            if (cache[hashValue] != null)
            {
                var error_msg = (filterContext.HttpContext.GetGlobalResourceObject("Messages", ErrorMessageKey) ?? filterContext.HttpContext.GetGlobalResourceObject("Messages", "SpamGenralMessage")).ToString();
                var action_info = "";
                //Adds the Error Message to the Model and Redirect
                if (ResultType == SpamFilterResultTypes.Json)
                {
                    // info field to return as json:
                    action_info =
                        (!String.IsNullOrEmpty(this.SubmitedFormInfoField) &&
                        filterContext.ActionParameters.Where(p => p.Value is FormCollection).Any() &&
                        ((FormCollection)filterContext.ActionParameters.Where(p => p.Value is FormCollection).First().Value)[SubmitedFormInfoField] != null)
                        ?
                        ((FormCollection)filterContext.ActionParameters.Where(p => p.Value is FormCollection).First().Value)[SubmitedFormInfoField].ToString() 
                        : 
                        "";
                    filterContext.Result = new JsonResult
                    {
                        Data = new { Done = false, Errors = new string[] { error_msg }, Info = action_info }
                    };
                }
                else
                {
                    throw new Exception(error_msg);
                }
            }
            else
            {
                cache.Add(hashValue, "", null, DateTime.Now.AddSeconds(DelayRequestInSeconds), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}