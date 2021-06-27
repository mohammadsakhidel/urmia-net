using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class PixelAdvertisement
    {
        public string GetImageLink(string place)
        {
            var pos = GetPosition(place);
            var lnk =
                "<a title=\"" + this.Title + "\" href=\"" + MyHelper.ToAbsolutePath("~/go") + "?src=pixeladv&dest=" + HttpContext.Current.Server.UrlEncode(this.Target) + "&info=" + this.Id + "\" target=\"_blank\" class=\"pxl_adv_lnk\" style=\"top: " + (Digits.PixelAdvBlockSides * pos.TopIndex) + "px; left:" + (Digits.PixelAdvBlockSides * pos.LeftIndex) + "px;\">" +
                    "<img src=\"" + MyHelper.ToAbsolutePath(Urls.PixelAdvertisements + this.ImageFileName) + "\" width=\"" + (Digits.PixelAdvBlockSides * this.WidthBlocks) + "px\" height=\"" + (Digits.PixelAdvBlockSides * this.HeightBlocks) + "px\" />" +
                "</a>";
            return lnk;
        }
        public List<string> Places
        {
            get
            {
                return MyHelper.SplitString(this.Position, ";").Select(p => new PixelAdvertisementPosition(p)).Where(p => p.IsVisible).Select(pos => pos.Place).ToList();
            }
        }
        public static PixelAdvertisement GetModelFromCollection(FormCollection form)
        {
            var model = new PixelAdvertisement();
            //Title
            model.Title = form["Title"];
            //Target
            model.Target = form["Target"];
            //WidthBlocks
            if (MyHelper.IsByte(form["WidthBlocks"]))
                model.WidthBlocks = Convert.ToByte(form["WidthBlocks"]);
            //HeightBlocks
            if (MyHelper.IsByte(form["HeightBlocks"]))
                model.HeightBlocks = Convert.ToByte(form["HeightBlocks"]);
            //BeginDate
            if (!String.IsNullOrEmpty(form["BeginDateDay"]) && !String.IsNullOrEmpty(form["BeginDateMonth"]) && !String.IsNullOrEmpty(form["BeginDateYear"]))
                model.BeginDate = DateHelper.GetMiladyDateFromInfo(form["BeginDateDay"], form["BeginDateMonth"], form["BeginDateYear"]);
            //EndDate
            if (!String.IsNullOrEmpty(form["EndDateDay"]) && !String.IsNullOrEmpty(form["EndDateMonth"]) && !String.IsNullOrEmpty(form["EndDateYear"]))
                model.EndDate = DateHelper.GetMiladyDateFromInfo(form["EndDateDay"], form["EndDateMonth"], form["EndDateYear"]);
            //Type
            if (MyHelper.IsByte(form["Type"]))
                model.Type = Convert.ToByte(form["Type"]);
            //Cost
            if (MyHelper.IsInt32(form["Cost"]))
                model.Cost = Convert.ToInt32(form["Cost"]);
            //PaymentStatus
            if (MyHelper.IsByte(form["PaymentStatus"]))
                model.PaymentStatus = Convert.ToByte(form["PaymentStatus"]);
            //Position:
            var positions = new List<string>();
            var positions_str = "";
            if (form["HomeVis"] != null && form["HomeVis"].ToLower() == "on")
            {
                var top = MyHelper.IsInt32(form["HomeTopIndex"]) ? Convert.ToInt32(form["HomeTopIndex"]) : 0;
                var left = MyHelper.IsInt32(form["HomeLeftIndex"]) ? Convert.ToInt32(form["HomeLeftIndex"]) : 0;
                positions.Add("home:True," + top + "," + left);
            }
            else
            {
                positions.Add("home:False,0,0");
            }
            if (form["HomePageVis"] != null && form["HomePageVis"].ToLower() == "on")
            {
                var top = MyHelper.IsInt32(form["HomePageTopIndex"]) ? Convert.ToInt32(form["HomePageTopIndex"]) : 0;
                var left = MyHelper.IsInt32(form["HomePageLeftIndex"]) ? Convert.ToInt32(form["HomePageLeftIndex"]) : 0;
                positions.Add("homepage:True," + top + "," + left);
            }
            else
            {
                positions.Add("homepage:False,0,0");
            }
            if (form["ProfileVis"] != null && form["ProfileVis"].ToLower() == "on")
            {
                var top = MyHelper.IsInt32(form["ProfileTopIndex"]) ? Convert.ToInt32(form["ProfileTopIndex"]) : 0;
                var left = MyHelper.IsInt32(form["ProfileLeftIndex"]) ? Convert.ToInt32(form["ProfileLeftIndex"]) : 0;
                positions.Add("profile:True," + top + "," + left);
            }
            else
            {
                positions.Add("profile:False,0,0");
            }
            for (var i = 0; i < positions.Count; i++)
            {
                positions_str += positions[i] + (i < positions.Count - 1 ? ";" : "");
            }
            model.Position = positions_str;
            //Considerations
            if (!String.IsNullOrEmpty(form["Considerations"]))
                model.Considerations = form["Considerations"];
            return model;
        }
        public PixelAdvertisementPosition GetPosition(string place)
        {
            var positions = MyHelper.SplitString(this.Position, ";").Select(p => new PixelAdvertisementPosition(p));
            try
            {
                return positions.Single(p => p.Place == place);
            }
            catch
            {
                return null;
            }
        }
        #region Validation:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        public ValidationErrorList ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                _validationErrors = value;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!_isValidated)
                    Validate();
                return ValidationErrors.Count() == 0;
            }
        }
        public void Validate()
        {
            //Title:
            if (String.IsNullOrEmpty(this.Title))
                ValidationErrors.Add("Title", Resources.Messages.RequiredTitle);
            //Target:
            if (String.IsNullOrEmpty(this.Target))
                ValidationErrors.Add("Target", Resources.Messages.RequiredTarget);
            //WidthBlocks:
            if (!(this.WidthBlocks.HasValue && this.WidthBlocks.Value > 0))
                ValidationErrors.Add("WidthBlocks", Resources.Messages.RequiredWidthBlocks);
            //HeightBlocks:
            if (!(this.HeightBlocks.HasValue && this.HeightBlocks.Value > 0))
                ValidationErrors.Add("HeightBlocks", Resources.Messages.RequiredHeightBlocks);
            //ImageFileName:
            if (String.IsNullOrEmpty(this.ImageFileName))
                ValidationErrors.Add("ImageFileName", Resources.Messages.RequiredImageFile);
            //BeginDate:
            if (!this.BeginDate.HasValue)
                ValidationErrors.Add("BeginDate", Resources.Messages.RequiredBeginDate);
            //Type:
            if (!this.Type.HasValue)
                ValidationErrors.Add("Type", Resources.Messages.RequiredAdvType);

            _isValidated = true;
        }
        #endregion
    }

    public class PixelAdvertisementPosition
    {
        public PixelAdvertisementPosition(string txt)
        {
            Place = MyHelper.SplitString(txt, ":")[0];
            var info = MyHelper.SplitString(MyHelper.SplitString(txt, ":")[1], ",");
            IsVisible = Convert.ToBoolean(info[0]);
            TopIndex = Convert.ToInt32(info[1]);
            LeftIndex = Convert.ToInt32(info[2]);
        }
        public string Place { get; set; }
        public bool IsVisible { get; set; }
        public int TopIndex { get; set; }
        public int LeftIndex { get; set; }
    }
}