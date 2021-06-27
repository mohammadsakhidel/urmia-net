using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class AlbumEditorModel
    {
        public Album Album { get; set; }
        public bool HasAlbum { get; set; }
        public FormAction FormAction { get; set; }

        public static AlbumEditorModel Create(Album album, FormAction formAction, System.Data.Objects.ObjectContext context)
        {
            var model = new AlbumEditorModel();
            model.Album = album;
            model.HasAlbum = album != null;
            model.FormAction = formAction;
            return model;
        }
    }
}