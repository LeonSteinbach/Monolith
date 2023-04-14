using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monolith.settings;

namespace Monolith.assets;

public static class MAssetManager
{
    private static ContentManager content;
    
	private static Dictionary<string, Texture2D> textures;
    private static Dictionary<string, SpriteFont> fonts;
    private static Dictionary<string, Effect> shaders;
    private static Dictionary<string, SoundEffect> sounds;
    private static Dictionary<string, SoundEffect> music;

    internal static void Initialize()
    {
        content = MAppSettings.Content;
        if (content is null)
            throw new NullReferenceException(
                "The content manager must be initialized before initializing the asset manager!");

        textures = new Dictionary<string, Texture2D>();
        fonts = new Dictionary<string, SpriteFont>();
        shaders = new Dictionary<string, Effect>();
        sounds = new Dictionary<string, SoundEffect>();
        music = new Dictionary<string, SoundEffect>();
    }

    public static void Dispose()
    {
        foreach (KeyValuePair<string, Texture2D> pair in textures)
            pair.Value.Dispose();
        
        foreach (KeyValuePair<string, Effect> pair in shaders)
            pair.Value.Dispose();
        
        foreach (KeyValuePair<string, SoundEffect> pair in sounds)
            pair.Value.Dispose();
        
        foreach (KeyValuePair<string, SoundEffect> pair in music)
            pair.Value.Dispose();
        
        textures.Clear();
        fonts.Clear();
        shaders.Clear();
        sounds.Clear();
        music.Clear();
    }

    public static void LoadTexture(string key, string path)
    {
        textures.Add(key, content.Load<Texture2D>(path));
    }
    
    public static void LoadTexture(string key, string path, float scale, GraphicsDevice graphicsDevice)
    {
        textures.Add(key, MTextureHelper.ScaleTexture(content.Load<Texture2D>(path), scale, graphicsDevice));
    }
    
    public static void LoadTexture(string key, string path, Vector2 scale, GraphicsDevice graphicsDevice)
    {
        textures.Add(key, MTextureHelper.ScaleTexture(content.Load<Texture2D>(path), scale, graphicsDevice));
    }

    public static void LoadFont(string key, string path)
    {
        fonts.Add(key, content.Load<SpriteFont>(path));
    }
    
    public static void LoadFont(string key, string path, float spacing, int lineSpacing)
    {
        fonts.Add(key, MFontHelper.SpacingFont(content.Load<SpriteFont>(path), spacing, lineSpacing));
    }
    
    public static void LoadShader(string key, string path)
    {
        shaders.Add(key, content.Load<Effect>(path));
    }
    
    public static void LoadSound(string key, string path)
    {
        sounds.Add(key, content.Load<SoundEffect>(path));
    }
    
    public static void LoadMusic(string key, string path)
    {
        music.Add(key, content.Load<SoundEffect>(path));
    }

    public static Texture2D GetTexture(string key)
    {
        return textures[key];
    }
    
    public static SpriteFont GetFont(string key)
    {
        return fonts[key];
    }
    
    public static Effect GetShader(string key)
    {
        return shaders[key];
    }
    
    public static SoundEffect GetSound(string key)
    {
        return sounds[key];
    }
    
    public static SoundEffect GetMusic(string key)
    {
        return music[key];
    }
}