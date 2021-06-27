using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public class SearchMemberInfo
    {
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public byte? EducationLevel { get; set; }
        public byte? MaritalStatus { get; set; }
        public byte? AgeFrom { get; set; }
        public byte? AgeTo { get; set; }
        public bool JustOnlines { get; set; }
        public bool JustWithPP { get; set; }
        public byte? LivingRegion { get; set; }
        public string LivingCity { get; set; }
    }
}