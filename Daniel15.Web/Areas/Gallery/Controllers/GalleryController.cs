﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Daniel15.Configuration;
using Daniel15.Web.Areas.Gallery.Models;
using Daniel15.Web.Areas.Gallery.ViewModels;
using System.Linq;
using Daniel15.Shared.Extensions;
using Daniel15.Web.Mvc;

namespace Daniel15.Web.Areas.Gallery.Controllers
{
	/// <summary>
	/// Handles browsing and serving pictures in a gallery.
	/// </summary>
	public partial class GalleryController : Controller
	{
		/// <summary>
		/// Maximum size (width or height) for thumbnail images
		/// </summary>
		private const int MAX_THUMBNAIL_SIZE = 200;
		/// <summary>
		/// Name of the thumbnail directory
		/// </summary>
		private const string THUMBNAIL_DIR = "thumb";

		/// <summary>
		/// The gallery configuration
		/// </summary>
		private readonly IGalleryConfiguration _galleryConfig;

		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryController" /> class.
		/// </summary>
		/// <param name="galleryConfig">The gallery configuration.</param>
		public GalleryController(IGalleryConfiguration galleryConfig)
		{
			_galleryConfig = galleryConfig;
		}

		/// <summary>
		/// Displays a listing of gallery images or serves a gallery file, depending on whether the
		/// request was for a file or directory.
		/// </summary>
		/// <param name="galleryName">Name of the gallery</param>
		/// <param name="path">Path to display</param>
		/// <returns>Gallery listing or file download</returns>
		public virtual ActionResult Index(string galleryName, string path = "")
		{
			var gallery = GetGallery(galleryName);
			var fullPath = GetAndValidateFullPath(gallery, path);

			// Don't allow access to thumbnail directory
			if (path == THUMBNAIL_DIR)
			{
				return HttpNotFound("Tried to directly access thumbnail folder");
			}

			// Directory? Return a directory listing.
			if (System.IO.Directory.Exists(fullPath))
			{
				return Directory(gallery, path, fullPath);
			}
			// File? Serve the file
			else if (System.IO.File.Exists(fullPath))
			{
				return Download(fullPath);
			}
			else
			{
				// Neither a directory or file - Bail out!
				return HttpNotFound(string.Format("Gallery file '{0}' not found in gallery {1}.", path, gallery.Name));
			}
		}

		/// <summary>
		/// Displays a listing of gallery images in the specified path
		/// </summary>
		/// <param name="gallery">Gallery to display images for</param>
		/// <param name="path">Relative path to display</param>
		/// <param name="fullPath">Full file system path</param>
		/// <returns>Directory listing</returns>
		private ActionResult Directory(IGallery gallery, string path, string fullPath)
		{
			var dirBlacklist = new HashSet<string> { THUMBNAIL_DIR, "cgi-bin" };

			var directories = System.IO.Directory.EnumerateDirectories(fullPath)
				// Ignore thumbnail directory
				.Where(x => !dirBlacklist.Contains(Path.GetFileName(x)))
				.Select(x => BuildGalleryModel(gallery, x, GalleryFileModel.FileType.Directory));

			var files = System.IO.Directory.EnumerateFiles(fullPath)
				// Ignore dotfiles (hidden)
				.Where(x => !Path.GetFileName(x).StartsWith("."))
				.Select(x => BuildGalleryModel(gallery, x, GalleryFileModel.FileType.File));

			return View(Views.Index, new IndexViewModel
			{
				Gallery = gallery,
				Path = path,
				Files = directories.Concat(files)
			});
		}

		/// <summary>
		/// Returns the specified file. This is just a fallback - The directory should be configured
		/// in Nginx or IIS and served directly.
		/// </summary>
		/// <param name="fullPath">Full path of the file to serve</param>
		/// <returns>File contents</returns>
		private ActionResult Download(string fullPath)
		{
			return this.AccelRedirectFile(fullPath, "image/png");
		}

		/// <summary>
		/// Generates a thumbnail for the specified image
		/// </summary>
		/// <param name="galleryName">Name of the gallery</param>
		/// <param name="path">Image to generate thumbnail for</param>
		/// <returns>Thumbnail</returns>
		public virtual ActionResult Thumbnail(string galleryName, string path)
		{
			var gallery = GetGallery(galleryName);

			var fullPath = GetAndValidateFullPath(gallery, path);
			var cachePath = Path.Combine(gallery.ImageDir, THUMBNAIL_DIR, path);

			// Cache the thumbnail if it doesn't already exist
			if (!System.IO.File.Exists(cachePath))
			{
				System.IO.Directory.CreateDirectory(Path.GetDirectoryName(cachePath));

				using (var sourceImg = new Bitmap(fullPath))
				using (var thumb = sourceImg.GenerateThumbnail(MAX_THUMBNAIL_SIZE))
				{
					thumb.Save(cachePath, ImageFormat.Png);
				}	
			}

			return this.AccelRedirectFile(cachePath, "image/png");
		}

		/// <summary>
		/// Converts the specified relative path to an absolute file system path, and ensure it's 
		/// not outside the gallery directory.
		/// </summary>
		/// <param name="gallery">Gallery to display images for</param>
		/// <param name="path">Relative path</param>
		/// <returns>Absolute path</returns>
		private string GetAndValidateFullPath(IGallery gallery, string path)
		{
			var root = gallery.ImageDir;
			var fullPath = Path.Combine(root, path);

			// The URI class handles normalising the path (eg. c:\Blah\..\foo\ --> c:\foo)
			// After normalisation, check that it's under the root
			var inRoot = new Uri(fullPath).LocalPath.StartsWith(root);
			if (!inRoot)
			{
				throw new Exception("Tried to access path outside root. '" + path + "'");
			}

			return fullPath;
		}

		/// <summary>
		/// Builds a gallery item model from the specified path
		/// </summary>
		/// <param name="gallery">Gallery to display images for</param>
		/// <param name="path">Path to the file or directory</param>
		/// <param name="type">Type of the entity</param>
		/// <returns>Gallery item model</returns>
		private GalleryFileModel BuildGalleryModel(IGallery gallery, string path, GalleryFileModel.FileType type)
		{
			var relativePath = path
				.Replace(gallery.ImageDir, string.Empty)
				.TrimStart(Path.DirectorySeparatorChar);

			var relativeUri = relativePath.Replace('\\', '/');
			return new GalleryFileModel
			{
				FileName = Path.GetFileName(path),
				RelativePath = relativePath,
				Url = type == GalleryFileModel.FileType.File ? ImageUrl(gallery, relativeUri) : Url.Action(MVC.Gallery.Gallery.Index(gallery.Name, relativeUri)),
				ThumbnailUrl = ThumbnailUrl(gallery, relativeUri),
				Type = type
			};
		}

		/// <summary>
		/// Gets the specified gallery from the configuration
		/// </summary>
		/// <param name="name">Name of the gallery to return</param>
		/// <returns>Details on the specified gallery</returns>
		private IGallery GetGallery(string name)
		{
			if (!_galleryConfig.Galleries.ContainsKey(name))
			{
				throw new HttpException((int)HttpStatusCode.NotFound, string.Format("Gallery '{0}' doesn't exist", name));
			}

			return _galleryConfig.Galleries[name];
		}

		/// <summary>
		/// Get the URL to the thumbnail for the specified image
		/// </summary>
		/// <param name="gallery">Gallery to display images for</param>
		/// <param name="path">Image path</param>
		/// <returns>Thumbnail URL</returns>
		private string ThumbnailUrl(IGallery gallery, string path)
		{
			return ImageUrl(gallery, THUMBNAIL_DIR + "/" + path);
		}

		/// <summary>
		/// Get the URL to the specified gallery image
		/// </summary>
		/// <param name="gallery">Gallery to display images for</param>
		/// <param name="path">Image path</param>
		/// <returns>Gallery URL</returns>
		private string ImageUrl(IGallery gallery, string path)
		{
			return gallery.ImageUrl + path.Replace('\\', '/');
		}
	}
}
