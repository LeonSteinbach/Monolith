using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;

namespace Monolith.math;

public class MPolygon : MGeometryObject
{
	public Vector2[] Vertices { get; set; }

	public new static MPolygon Empty => new (Array.Empty<Vector2>());

	public override Vector2 CenterOfMass
	{
		get
		{
			float area = 0;
			Vector2 center = Vector2.Zero;

			for (int i = 0; i < Vertices.Length; i++)
			{
				int j = (i + 1) % Vertices.Length;
				float cross = MMathHelper.Cross(Vertices[i], Vertices[j]);
				area += cross;
				center += cross * (Vertices[i] + Vertices[j]);
			}

			if (area == 0)
				return Vertices[0];

			return center / (3 * area);
		}
	}

	public override float Area
	{
		get
		{
			float area = 0;

			for (int i = 0; i < Vertices.Length; i++)
			{
				int j = (i + 1) % Vertices.Length;
				area += 0.5f * MMathHelper.Cross(Vertices[i], Vertices[j]);
			}

			return Math.Abs(area);
		}
	}

	public override float Circumference
	{
		get
		{
			float circumference = 0;

			for (int i = 0; i < Vertices.Length; i++)
			{
				int j = (i + 1) % Vertices.Length;
				float edgeLength = Vector2.Distance(Vertices[i], Vertices[j]);
				circumference += edgeLength;
			}

			return circumference;
		}
	}


	private Rectangle boundingBox;
	public override Rectangle BoundingBox => boundingBox;

	public override float Angle { get; protected set; }

	public MPolygon(Vector2[] vertices)
	{
		Vertices = vertices;

		Update();
	}

	private void Update()
	{
		float minX = float.MaxValue, minY = float.MaxValue;
		float maxX = float.MinValue, maxY = float.MinValue;

		foreach (Vector2 point in Vertices)
		{
			minX = Math.Min(minX, point.X);
			minY = Math.Min(minY, point.Y);
			maxX = Math.Max(maxX, point.X);
			maxY = Math.Max(maxY, point.Y);
		}

		boundingBox = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
	}

	public (bool Intersected, List<Vector2> IntersectionPoints, Vector2 MTV) Intersect(MPolygon other)
	{
		List<Vector2> edges = new List<Vector2>();
		List<Vector2> intersectionPoints = new List<Vector2>();
		Vector2 mtv = Vector2.Zero;
		float minOverlap = float.MaxValue;

		for (int i = 0; i < Vertices.Length; i++)
		{
			int j = (i + 1) % Vertices.Length;
			edges.Add(Vertices[j] - Vertices[i]);
		}

		for (int i = 0; i < other.Vertices.Length; i++)
		{
			int j = (i + 1) % other.Vertices.Length;
			edges.Add(other.Vertices[j] - other.Vertices[i]);
		}

		bool hasIntersection = true;

		for (int i = 0; i < edges.Count; i++)
		{
			Vector2 axis = new Vector2(-edges[i].Y, edges[i].X);
			axis.Normalize();
			float minA = float.MaxValue, maxA = float.MinValue;
			float minB = float.MaxValue, maxB = float.MinValue;

			foreach (Vector2 vertex in Vertices)
			{
				float projection = Vector2.Dot(vertex, axis);
				minA = Math.Min(minA, projection);
				maxA = Math.Max(maxA, projection);
			}

			foreach (Vector2 vertex in other.Vertices)
			{
				float projection = Vector2.Dot(vertex, axis);
				minB = Math.Min(minB, projection);
				maxB = Math.Max(maxB, projection);
			}

			if (maxA < minB || maxB < minA)
			{
				hasIntersection = false;
				break;
			}
			else
			{
				// Calculate the overlap on this axis
				float overlap = Math.Min(maxA, maxB) - Math.Max(minA, minB);

				// Check if this overlap is smaller than the current minimum
				if (overlap < minOverlap)
				{
					minOverlap = overlap;
					mtv = axis;
				}

				// Find the points of intersection by checking each edge
				for (int j = 0; j < Vertices.Length; j++)
				{
					int k = (j + 1) % Vertices.Length;
					Vector2 edge = Vertices[k] - Vertices[j];
					Vector2 edgeNormal = new Vector2(-edge.Y, edge.X);

					if (Vector2.Dot(edgeNormal, axis) < 0)
						edgeNormal = -edgeNormal;

					// Check if the edge's normal is parallel to the current axis
					if (Math.Abs(Vector2.Dot(edgeNormal, axis) - Vector2.Dot(edge, axis)) > 1e-6f)
					{
						for (int l = 0; l < other.Vertices.Length; l++)
						{
							int m = (l + 1) % other.Vertices.Length;
							if (MMathHelper.LineSegmentIntersection(Vertices[j], Vertices[k], other.Vertices[l],
								    other.Vertices[m], out var intersectionPoint))
								intersectionPoints.Add(intersectionPoint);
						}
					}
				}
			}
		}

		if (hasIntersection)
	    {
	        // Normalize the MTV and multiply by the minimum overlap
	        mtv.Normalize();
	        mtv *= minOverlap;
	    }

	    return (hasIntersection, intersectionPoints, mtv);
	}

	public override void Move(Vector2 vector)
	{
		for (int i = 0; i < Vertices.Length; i++)
			Vertices[i] += vector;
			
		Update();
	}

	public override void Rotate(float angle)
	{
		Angle += angle;
		
		Vector2 center = CenterOfMass;
		for (int i = 0; i < Vertices.Length; i++)
			Vertices[i] = MMathHelper.RotatePoint(Vertices[i], center, angle);
			
		Update();
	}

	public override void Render(SpriteBatch spriteBatch, Color color, bool renderCenterOfMass = false, bool renderBoundingBox = false, bool renderAngle = false)
	{
		for (int i = 0; i < Vertices.Length; i++)
		{
			int j = (i + 1) % Vertices.Length;
			MShapes.DrawLine(spriteBatch, Vertices[i], Vertices[j], color, 1);
		}
		
		if (renderCenterOfMass)
			MShapes.DrawCircle(spriteBatch, CenterOfMass, 5, 10, Color.Red, 5);
		
		if (renderBoundingBox)
			MShapes.DrawRectangle(spriteBatch, BoundingBox, Color.White, 1);

		if (renderAngle)
		{
			Vector2 direction = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle));
			direction.Normalize();
			direction *= 20f;
			
			MShapes.DrawLine(spriteBatch, CenterOfMass, CenterOfMass + direction, Color.Red, 1f);
		}
	}
}