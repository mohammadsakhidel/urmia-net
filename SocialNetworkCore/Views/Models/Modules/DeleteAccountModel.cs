using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class DeleteAccountModel
    {
        #region Properties:
        // nothing
        #endregion

        #region Methods:
        public static DeleteAccountModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new DeleteAccountModel();
            return model;
        }
        #endregion
    }
}