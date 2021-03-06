// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Daniel15.Web.Areas.Gallery.Controllers
{
    public partial class GalleryController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected GalleryController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Thumbnail()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Thumbnail);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public GalleryController Actions { get { return MVC.Gallery.Gallery; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Gallery";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Gallery";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Gallery";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Thumbnail = "Thumbnail";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Thumbnail = "Thumbnail";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string galleryName = "galleryName";
            public readonly string path = "path";
        }
        static readonly ActionParamsClass_Thumbnail s_params_Thumbnail = new ActionParamsClass_Thumbnail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Thumbnail ThumbnailParams { get { return s_params_Thumbnail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Thumbnail
        {
            public readonly string galleryName = "galleryName";
            public readonly string path = "path";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Areas/Gallery/Views/Gallery/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_GalleryController : Daniel15.Web.Areas.Gallery.Controllers.GalleryController
    {
        public T4MVC_GalleryController() : base(Dummy.Instance) { }

        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string galleryName, string path);

        public override System.Web.Mvc.ActionResult Index(string galleryName, string path)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "galleryName", galleryName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "path", path);
            IndexOverride(callInfo, galleryName, path);
            return callInfo;
        }

        partial void ThumbnailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string galleryName, string path);

        public override System.Web.Mvc.ActionResult Thumbnail(string galleryName, string path)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Thumbnail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "galleryName", galleryName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "path", path);
            ThumbnailOverride(callInfo, galleryName, path);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
