using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class AlbumPhotosModel
    {
        public Album Album { get; set; }
        public bool IsPostBack { get; set; }
        public bool PostBackDone { get; set; }
        public string PostBackMessage { get; set; }
        //***** modules:
        public List<Tuple<AlbumPhoto, PhotoEditorModel>> AlbumPhotoRecords { get; set; }
        public SimpleUploaderModel SimpleUploaderModel { get; set; }
        //*****
        public static AlbumPhotosModel Create(Album album, System.Web.Mvc.TempDataDictionary tempData, System.Data.Objects.ObjectContext context)
        {
            var model = new AlbumPhotosModel();
            model.Album = album;
            model.IsPostBack = tempData["done"] != null && tempData["message"] != null;
            model.PostBackDone = model.IsPostBack && (bool)tempData["done"];
            model.PostBackMessage = model.IsPostBack ? tempData["message"].ToString() : "";
            model.SimpleUploaderModel = SimpleUploaderModel.Create(context);
            //--- photo records:
            var photos = album.Photos.OrderByDescending(p => p.DateOfAdd);
            var phRecords = new List<Tuple<AlbumPhoto, PhotoEditorModel>>();
            foreach (var ph in photos)
            {
                var photoEditorModel = PhotoEditorModel.Create(ph, context);
                phRecords.Add(new Tuple<AlbumPhoto, PhotoEditorModel>(ph, photoEditorModel));
            }
            //---
            model.AlbumPhotoRecords = phRecords;
            return model;
        }
    }
}