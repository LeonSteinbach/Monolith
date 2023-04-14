using System;
using Microsoft.Xna.Framework.Content;

namespace Monolith;

public static class AppSettings
{
    public static ContentManager Content { get; set; }
    
    private static string contentRoot = "content";
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