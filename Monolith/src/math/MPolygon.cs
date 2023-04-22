using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;
using Monolith.input;

namespace Monolith.math;

public class MPolygon
{
	public List<Vector2> Points { get; set; }
	
	public MPolygon(List<Vector2> points)
	{
		Points = points;
	}
	
	public Vector2 CenterOfMass
	{
		get
		{
			float area = 0;
			Vector2 center = Vector2.Zero;

			for (int i = 0; i < Points.Count; i++)
			{
				int j = (i + 1) % Points.Count;
				float cross = MMathHelper.Cross(Points[i], Points[j]);
				area += cross;
				center += cross * (Points[i] + Points[j]);
			}

			if (area == 0)
				return Points[0];

			return center / (3 * area);
		}
	}

	public Rectangle BoundingBox
	{
		get
		{
			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;

			foreach (Vector2 point in Points)
			{
				minX = Math.Min(minX, point.X);
				minY = Math.Min(minY, point.Y);
				maxX = Math.Max(maxX, point.X);
				maxY = Math.Max(maxY, point.Y);
			}

			return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
		}
	}

	public void Rotate(float angle)
	{
		Vector2 center = CenterOfMass;
		for (int i = 0; i < Points.Count; i++)
			Points[i] = MMathHelper.RotatePoint(Points[i], center, angle);
	}

	public bool Intersect(Vector2 point)
	{
		bool inside = false;
		for (int i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
		{
			if (Points[i].Y > point.Y != Points[j].Y > point.Y &&
			    point.X < (Points[j].X - Points[i].X) * (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X)
			{
				inside = !inside;
			}
		}
		
		return inside;
	}

	public bool Intersect(MPolygon other)
	{
		List<Vector2> edges = new List<Vector2>();
		
		for (int i = 0; i < Points.Count; i++)
		{
			int j = (i + 1) % Points.Count;
			edges.Add(Points[j] - Points[i]);
		}
		
		for (int i = 0; i < other.Points.Count; i++)
		{
			int j = (i + 1) % other.Points.Count;
			edges.Add(other.Points[j] - other.Points[i]);
		}

		for (int i = 0; i < edges.Count; i++)
		{
			Vector2 axis = new Vector2(-edges[i].Y, edges[i].X);
			float minA = float.MaxValue, maxA = float.MinValue;
			float minB = float.MaxValue, maxB = float.MinValue;

			foreach (Vector2 vertex in Points)
			{
				float projection = Vector2.Dot(vertex, axis);
				minA = Math.Min(minA, projection);
				maxA = Math.Max(maxA, projection);
			}
			
			foreach (Vector2 vertex in other.Points)
			{
				float projection = Vector2.Dot(vertex, axis);
				minB = Math.Min(minB, projection);
				maxB = Math.Max(maxB, projection);
			}

			if (maxA < minB || maxB < minA)
				return false;
		}

		return true;
	}

	public void Render(SpriteBatch spriteBatch, Color color, float thickness)
	{
		for (int i = 0; i < Points.Count; i++)
		{
			int j = (i + 1) % Points.Count;
			MShapes.DrawLine(spriteBatch, Points[i], Points[j], color, thickness);
		}
	}

	public void RenderCenterOfMass(SpriteBatch spriteBatch, Color color, float radius, int sides, float thickness)
	{
		MShapes.DrawCircle(spriteBatch, CenterOfMass, radius, sides, color, thickness);
	}
	
	public void RenderBoundingBox(SpriteBatch spriteBatch, Color color, float thickness)
	{
		MShapes.DrawRectangle(spriteBatch, BoundingBox, color, thickness);
	}

}