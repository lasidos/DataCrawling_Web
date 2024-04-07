﻿using System.IO;
using System.Web.Mvc;

namespace DataCrawling_Web.Service.Util
{
    /// <summary>
    /// Reference: http://www.west-wind.com/weblog/posts/2012/May/30/Rendering-ASPNET-MVC-Views-to-String
    /// Class that renders MVC views to a string using the
    /// standard MVC View Engine to render the view. 
    /// 
    /// Note: This class can only be used within MVC 
    /// applications that have an active ControllerContext.
    /// </summary>
    public class ViewRendererHelper
    {
        /// <summary>
        /// Required Controller Context
        /// </summary>
        protected ControllerContext Context { get; set; }


        public ViewRendererHelper(ControllerContext controllerContext)
        {
            Context = controllerContext;
        }

        /// <summary>
        /// Renders a full MVC view to a string. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout        
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to render the view with</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, false);
        }


        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderPartialView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, true);
        }

        public static string RenderView(string viewPath, object model,
                                        ControllerContext controllerContext)
        {
            ViewRendererHelper renderer = new ViewRendererHelper(controllerContext);
            return renderer.RenderView(viewPath, model);
        }

        public static string RenderPartialView(string viewPath, object model,
                                               ControllerContext controllerContext)
        {
            ViewRendererHelper renderer = new ViewRendererHelper(controllerContext);
            return renderer.RenderPartialView(viewPath, model);
        }

        protected string RenderViewToStringInternal(string viewPath, object model,
                                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
            {
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            }
            else
            {
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);
            }

            if (viewEngineResult == null || viewEngineResult.View == null)
            {
                throw new FileNotFoundException("View not found");
            }

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(Context, view,
                                            Context.Controller.ViewData,
                                            Context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}