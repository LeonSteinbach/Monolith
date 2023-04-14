using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monolith.math;

public class MathHelper
{
	private static readonly Random random = new Random();
        
	public static float RandFloat()
	{
		return (float) random.NextDouble();
	}

	public static int RandInt(int min, int max)
	{
		if (min > max)
			throw new ArgumentException("Min value must be smaller than max value!");

		if (min < 0)
			return random.Next(0, max + 1 - min) + min;
		return random.Next(min, max + 1);
	}

	public static T Choice<T>(List<T> items)
	{
		return items[RandInt(0, items.Count - 1)];
	}
	
	public static float Interpolate(float x0, float x1, float alpha)
	{
		return x0 * (1 - alpha) + alpha * x1;
	}

	public static float Distance(Vector2 a, Vector2 b)
	{
		return (float) Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
	}

	public static float Distance(Point a, Point b)
	{
		return Distance(a.ToVector2(), b.ToVector2());
	}

	public static float AngleRadians(Vector2 a, Vector2 b)
	{
		return (float) Math.Atan2(b.Y - a.Y, b.X - a.X);
	}

	public static float AngleRadians(Point a, Point b)
	{
		return AngleRadians(a.ToVector2(), b.ToVector2());
	}

	public static float RadiansToDegrees(float angle)
	{
		return angle * 180f / (float) Math.PI;
	}

	public static float DegreesToRadians(float angle)
	{
		return angle * (float) Math.PI / 180f;
	}
        
	public static float MapRange(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
        
	public static Vector2 IsoToOrtho(Vector2 position)
	{
		return new Vector2((2 * position.Y + position.X) / 2, (2 * position.Y - position.X) / 2);
	}
        
	public static Vector2 OrthoToIso(Vector2 position)
	{
		return new Vector2(position.X - position.Y, (position.X + position.Y) / 2);
	}
	
	private static float[,] GenerateSmoothNoise(float[,] baseNoise, int width, int height, int octave)
        {
            float[,] smoothNoise = new float[width, height];

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                int sampleI0 = (i / samplePeriod) * samplePeriod;
                int sampleI1 = (sampleI0 + samplePeriod) % width;
                float horizontalBlend = (i - sampleI0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    int sampleJ0 = (j / samplePeriod) * samplePeriod;
                    int sampleJ1 = (sampleJ0 + samplePeriod) % height;
                    float verticalBlend = (j - sampleJ0) * sampleFrequency;

                    float top = MathHelper.Interpolate(baseNoise[sampleI0, sampleJ0], baseNoise[sampleI1, sampleJ0], horizontalBlend);
                    float bottom = MathHelper.Interpolate(baseNoise[sampleI0, sampleJ1], baseNoise[sampleI1, sampleJ1], horizontalBlend);

                    smoothNoise[i, j] = MathHelper.Interpolate(top, bottom, verticalBlend);
                }
            }

            return smoothNoise;
        }
        
        public static float[,] PerlinNoise(int width, int height, int octaveCount)
        {
            float[,] baseNoise = new float[width, height];
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    baseNoise[x, y] = RandFloat();
                }
            }

            float[][,] smoothNoise = new float[octaveCount][,];

            float persistence = 0.5f;

            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, width, height, i);
            }

            float[,] perlinNoise = new float[width, height];
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistence;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
            }

            // Normalize
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    perlinNoise[i, j] /= totalAmplitude;

            return perlinNoise;
        }
}