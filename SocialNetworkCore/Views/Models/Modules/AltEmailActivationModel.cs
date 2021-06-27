using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class AltEmailActivationModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public bool ActivationSucceed { get; set; }
        public string ActivationErrorMessage { get; set; }

        public static AltEmailActivationModel Create(string email, string code, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            var rgxEmail = new System.Text.RegularExpressions.Regex(Patterns.Email);
            var rgxCode = new System.Text.RegularExpressions.Regex(Patterns.AlternativeEmailActivationCode);
            //////////////////////////
            var model = new AltEmailActivationModel();
            model.Email = email;
            model.Code = code;
            //*** are email and code valid?
            var isEmailValid = !String.IsNullOrEmpty(model.Email) && rgxEmail.IsMatch(model.Email);
            var isCodeValid = !String.IsNullOrEmpty(model.Code) && rgxCode.IsMatch(model.Code);
            if (!isEmailValid || !isCodeValid)
            {
                model.ActivationSucceed = false;
                model.ActivationErrorMessage = Resources.Messages.AltEmailActivationNotValidEmailOrCode;
                return model;
            }
            //*** member exists?
            var member = mr.Get(email);
            if (member == null)
                throw new HttpException(404, "Page Not Found.");
            //*** acc setting exists?
            var accSetting = member.AccountSetting;
            if (accSetting == null)
                throw new HttpException(404, "Page Not Found.");
            //*** is codeCorrect?
            if (!String.IsNullOrEmpty(accSetting.AlternativeEmail) &&
                !String.IsNullOrEmpty(accSetting.AlternativeEmailActivationCode) &&
                accSetting.AlternativeEmailActivationCode == model.Code)
            {
                accSetting.AlternativeEmailApproved = true;
                mr.Save();
                model.ActivationSucceed = true;
            }
            else
            {
                model.ActivationSucceed = false;
                model.ActivationErrorMessage = Resources.Messages.AltEmailActivationCodeIncorrect;
            }
            return model;
        }
    }
}