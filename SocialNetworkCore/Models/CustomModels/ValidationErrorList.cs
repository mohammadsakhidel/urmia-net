using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public class ValidationErrorList : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string val)
        {
            this.Add(new KeyValuePair<string, string>(key, val));
        }
    }
}