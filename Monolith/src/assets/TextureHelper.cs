using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.assets;

public static class TextureHelper
{
	public static Color[,] TextureToArray(Texture2D texture)
	{
		Color[] colors = new Color[texture.Width * texture.Height];
		texture.GetData(colors);

		Color[,] array = new Color[texture.Width, texture.Height];
		for (int y = 0; y < texture.Height; y++)
		{
			for (int x = 0; x < texture.Width; x++)
			{
				array[x, y] = colors[x + y * texture.Width];
			}
		}
		return array;
	}

	public static Texture2D ArrayToTexture(GraphicsDevice graphicsDevice, Color[,] array, int width, int height)
	{
		Color[] colors = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colors[x + width * y] = array[x, y];
			}
		}

		Texture2D texture = new Texture2D(graphicsDevice, width, height);
		texture.SetData(colors);

		return texture;
	}

	public static Texture2D ScaleTexture(Texture2D texture, Vector2 scale, GraphicsDevice graphicsDevice)
	{
		return _ScaleTexture(texture, scale, graphicsDevice);
	}

	public static Texture2D ScaleTexture(Texture2D texture, float scale, GraphicsDevice graphicsDevice)
	{
		Texture2D scaledTexture = _ScaleTexture(texture, new Vector2(scale, scale), graphicsDevice);
		scaledTexture.Name = texture.Name;
		return scaledTexture;
	}

	private static Texture2D _ScaleTexture(Texture2D texture, Vector2 scale, GraphicsDevice graphicsDevice)
	{
		if (Math.Abs(scale.X - 1f) < 0.001f && Math.Abs(scale.Y - 1f) < 0.001f)
			return texture;
		
		Color[,] array = TextureToArray(texture);

		int width = (int)(texture.Width * scale.X);
		int height = (int)(texture.Height * scale.Y);

		Color[,] scaled = new Color[width, height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				scaled[x, y] = array[(int)(x / scale.X), (int)(y / scale.Y)];
			}
		}

		Texture2D tScaled = ArrayToTexture(graphicsDevice, scaled, width, height);

		return tScaled;
	}

	public static Texture2D DrawBorder(Texture2D texture, int thickness, int alpha, Color color)
	{
		Color[,] original = TextureToArray(texture);
		Color[,] array = (Color[,])original.Clone();

		for (int i = 0; i < thickness; i++)
		{
			for (int y = 0; y < texture.Height; y++)
			{
				for (int x = 0; x < texture.Width; x++)
				{
					if (original[x, y].A <= alpha)
					{
						if (
							x > 0 && y > 0 && original[x - 1, y - 1].A > alpha ||
							y > 0 && original[x, y - 1].A > alpha ||
							x < texture.Width - 1 && y > 0 && original[x + 1, y - 1].A > alpha ||
							x > 0 && original[x - 1, y].A > alpha ||
							x < texture.Width - 1 && original[x + 1, y].A > alpha ||
							x > 0 && y < texture.Height - 1 && original[x - 1, y + 1].A > alpha ||
							y < texture.Height - 1 && original[x, y + 1].A > alpha ||
							x < texture.Width - 1 && y < texture.Height - 1 && original[x + 1, y + 1].A > alpha
						)
						{
							array[x, y] = color;
						}
					}
				}
			}
			original = (Color[,])array.Clone();
		}

		Texture2D bTexture = ArrayToTexture(texture.GraphicsDevice, array, texture.Width, texture.Height);
		return bTexture;
	}
}