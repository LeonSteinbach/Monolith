using System;
using Microsoft.Xna.Framework.Content;

namespace Monolith.settings;

public static class MAppSettings
{
	public static ContentManager Content { get; set; }
	
	private static string contentRoot = "Content";
	public static string ContentRoot
	{
		get => contentRoot;
		set
		{
			contentRoot = value;
			if (Content is null)
				throw new NullReferenceException(
					"The content manager must be initialized before setting the content root directory!");
			Content.RootDirectory = contentRoot;
		}
	}
}