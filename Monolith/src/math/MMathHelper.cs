using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monolith.math;

public static class MMathHelper
{
	private static readonly Random random = new ();
        
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
	
	public static float Cross(Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}
	
	public static Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
	{
		float cos = (float)Math.Cos(angle);
		float sin = (float)Math.Sin(angle);
		float x = cos * (point.X - pivot.X) - sin * (point.Y - pivot.Y) + pivot.X;
		float y = sin * (point.X - pivot.X) + cos * (point.Y - pivot.Y) + pivot.Y;
		return new Vector2(x, y);
	}

	public static bool NearlyEqual(float a, float b)
	{
		return Math.Abs(a - b) <= 1e-6;
	}
	
	public static bool NearlyEqual(Vector2 a, Vector2 b)
	{
		return Math.Abs(a.X - b.X) <= 1e-6 && Math.Abs(a.Y - b.Y) <= 1e-6;
	}
	
	public static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersectionPoint)
	{
		intersectionPoint = Vector2.Zero;

		float s1_x, s1_y, s2_x, s2_y;
		s1_x = p2.X - p1.X;
		s1_y = p2.Y - p1.Y;
		s2_x = p4.X - p3.X;
		s2_y = p4.Y - p3.Y;

		float s, t;
		s = (-s1_y * (p1.X - p3.X) + s1_x * (p1.Y - p3.Y)) / (-s2_x * s1_y + s1_x * s2_y);
		t = (s2_x * (p1.Y - p3.Y) - s2_y * (p1.X - p3.X)) / (-s2_x * s1_y + s1_x * s2_y);

		if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
		{
			intersectionPoint.X = p1.X + (t * s1_x);
			intersectionPoint.Y = p1.Y + (t * s1_y);
			return true;
		}

		return false;
	}
	
	public static float Interpolate(float x0, float x1, float alpha)
	{
		return x0 * (1 - alpha) + alpha * x1;
	}

	public static float LengthSquared(Vector2 v)
	{
		return v.X * v.X + v.Y * v.Y;
	}

	public static float Length(Vector2 v)
	{
		return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
	}

	public static float DistanceSquared(Vector2 a, Vector2 b)
	{
		float dx = a.X - b.X;
		float dy = a.Y - b.Y;
		return dx * dx + dy * dy;
	}

	public static float Distance(Vector2 a, Vector2 b)
	{
		float dx = a.X - b.X;
		float dy = a.Y - b.Y;
		return MathF.Sqrt(dx * dx + dy * dy);
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

                    float top = Interpolate(baseNoise[sampleI0, sampleJ0], baseNoise[sampleI1, sampleJ0], horizontalBlend);
                    float bottom = Interpolate(baseNoise[sampleI0, sampleJ1], baseNoise[sampleI1, sampleJ1], horizontalBlend);

                    smoothNoise[i, j] = Interpolate(top, bottom, verticalBlend);
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

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    perlinNoise[i, j] /= totalAmplitude;

            return perlinNoise;
        }
}