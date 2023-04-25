using System;
using Microsoft.Xna.Framework;

namespace Monolith.math;

public static class MPhysicsHelper
{
	public static void FindContactPoints(MGeometryObject bodyA, MGeometryObject bodyB, out Vector2 contact1, out Vector2 contact2, out int contactCount)
	{
		contact1 = Vector2.Zero;
		contact2 = Vector2.Zero;
		contactCount = 0;

		if (bodyA is MPolygon polygonA && bodyB is MPolygon polygonB)
		{
			FindPolygonsContactPoints(polygonA.Vertices, polygonB.Vertices, out contact1, out contact2, out contactCount);
		}
		else if (bodyA is MPolygon polygon && bodyB is MCircle circle)
		{
			FindCirclePolygonContactPoint(circle.Center, circle.Radius, polygon.CenterOfMass, polygon.Vertices, out contact1);
			contactCount = 1;
		}
		else if (bodyA is MCircle circleA && bodyB is MPolygon polygonC)
		{
			FindCirclePolygonContactPoint(circleA.Center, circleA.Radius, polygonC.CenterOfMass, polygonC.Vertices, out contact1);
			contactCount = 1;
		}
		else if (bodyA is MCircle circleB && bodyB is MCircle circleC)
		{
			FindCirclesContactPoint(circleB.Center, circleB.Radius, circleC.Center, out contact1);
			contactCount = 1;
		}
	}
	
	private static void FindPolygonsContactPoints(Vector2[] verticesA, Vector2[] verticesB, out Vector2 contact1, out Vector2 contact2, out int contactCount)
    {
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0;

        float minDistSq = float.MaxValue;

        foreach (var p in verticesA)
        {
	        for(int j = 0; j < verticesB.Length; j++)
	        {
		        Vector2 va = verticesB[j];
		        Vector2 vb = verticesB[(j + 1) % verticesB.Length];

		        PointSegmentDistance(p, va, vb, out float distSq, out Vector2 cp);

		        if (MMathHelper.NearlyEqual(distSq, minDistSq))
		        {
			        if (!MMathHelper.NearlyEqual(cp, contact1))
			        {
				        contact2 = cp;
				        contactCount = 2;
			        }
		        }
		        else if (distSq < minDistSq)
		        {
			        minDistSq = distSq;
			        contactCount = 1;
			        contact1 = cp;
		        }
	        }
        }

        foreach (var p in verticesB)
        {
	        for (int j = 0; j < verticesA.Length; j++)
	        {
		        Vector2 va = verticesA[j];
		        Vector2 vb = verticesA[(j + 1) % verticesA.Length];

		        PointSegmentDistance(p, va, vb, out float distSq, out Vector2 cp);

		        if (MMathHelper.NearlyEqual(distSq, minDistSq))
		        {
			        if (!MMathHelper.NearlyEqual(cp, contact1))
			        {
				        contact2 = cp;
				        contactCount = 2;
			        }
		        }
		        else if (distSq < minDistSq)
		        {
			        minDistSq = distSq;
			        contactCount = 1;
			        contact1 = cp;
		        }
	        }
        }
    }
	
	private static void FindCirclePolygonContactPoint(Vector2 circleCenter, float circleRadius, Vector2 polygonCenter, Vector2[] polygonVertices, out Vector2 cp)
	{
		cp = Vector2.Zero;

		float minDistSq = float.MaxValue;

		for(int i = 0; i < polygonVertices.Length; i++)
		{
			Vector2 va = polygonVertices[i];
			Vector2 vb = polygonVertices[(i + 1) % polygonVertices.Length];

			PointSegmentDistance(circleCenter, va, vb, out float distSq, out Vector2 contact);

			if(distSq < minDistSq)
			{
				minDistSq = distSq;
				cp = contact;
			}
		}
	}
	
	private static void FindCirclesContactPoint(Vector2 centerA, float radiusA, Vector2 centerB, out Vector2 cp)
	{
		Vector2 dir = centerB - centerA;
		dir.Normalize();
		cp = centerA + dir * radiusA;
	}
	
	public static void PointSegmentDistance(Vector2 p, Vector2 a, Vector2 b, out float distanceSquared, out Vector2 cp)
	{
		Vector2 ab = b - a;
		Vector2 ap = p - a;

		float proj = Vector2.Dot(ap, ab);
		float abLenSq = MMathHelper.LengthSquared(ab);
		float d = proj / abLenSq;

		if (d <= 0f)
			cp = a;
		else if(d >= 1f)
			cp = b;
		else
			cp = a + ab * d;

		distanceSquared = MMathHelper.DistanceSquared(p, cp);
	}

	public static bool Contains(MGeometryObject obj, Point point)
	{
		switch (obj)
		{
			case MPolygon polygon:
				return ContainsPolygonPoint(polygon, point);
			case MCircle circle:
				return ContainsCirclePoint(circle, point);
		}

		return false;
	}
	
	public static bool ContainsPolygonPoint(MPolygon polygon, Point point)
	{
		bool inside = false;
		for (int i = 0, j = polygon.Vertices.Length - 1; i < polygon.Vertices.Length; j = i++)
		{
			if (polygon.Vertices[i].Y > point.Y != polygon.Vertices[j].Y > point.Y &&
			    point.X < (polygon.Vertices[j].X - polygon.Vertices[i].X) * (point.Y - polygon.Vertices[i].Y) / (polygon.Vertices[j].Y - polygon.Vertices[i].Y) + polygon.Vertices[i].X)
			{
				inside = !inside;
			}
		}
		
		return inside;
	}

	private static bool ContainsCirclePoint(MCircle circle, Point point)
	{
		return MMathHelper.Distance(circle.Center, point.ToVector2()) <= circle.Radius;
	}

	public static bool Intersect(MGeometryObject a, MGeometryObject b, out Vector2 normal, out float depth)
	{
		normal = Vector2.Zero;
		depth = 0f;

		return a switch
		{
			MPolygon polygon when b is MPolygon mPolygon => IntersectPolygons(polygon, mPolygon, out normal, out depth),
			MPolygon polygon when b is MCircle circle => IntersectPolygonCircle(polygon, circle, out normal, out depth),
			MCircle circle when b is MPolygon polygon => IntersectPolygonCircle(polygon, circle, out normal, out depth),
			MCircle circle when b is MCircle mCircle => IntersectCircles(circle, mCircle, out normal, out depth),
			_ => false
		};
	}


	public static bool IntersectCircles(MCircle a, MCircle b, out Vector2 normal, out float depth)
	{
		normal = Vector2.Zero;
		depth = 0f;

		float distance = MMathHelper.Distance(a.Center, b.Center);
		float radii = a.Radius + b.Radius;

		if (distance >= radii)
			return false;

		normal = b.Center - a.Center;
		normal.Normalize();
		depth = radii - distance;

		return true;
	}

	public static bool IntersectPolygons(MPolygon a, MPolygon b, out Vector2 normal, out float depth)
	{
		var verticesA = a.Vertices;
		var verticesB = b.Vertices;
		var centerA = a.CenterOfMass;
		var centerB = b.CenterOfMass;
		
	    normal = Vector2.Zero;
        depth = float.MaxValue;

        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 va = verticesA[i];
            Vector2 vb = verticesA[(i + 1) % verticesA.Length];

            Vector2 edge = vb - va;
            Vector2 axis = new Vector2(-edge.Y, edge.X);
            axis.Normalize();

            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 va = verticesB[i];
            Vector2 vb = verticesB[(i + 1) % verticesB.Length];

            Vector2 edge = vb - va;
            Vector2 axis = new Vector2(-edge.Y, edge.X);
            axis.Normalize();

            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        Vector2 direction = centerB - centerA;

        if (Vector2.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
	}
	
	private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
	{
		min = float.MaxValue;
		max = float.MinValue;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector2 v = vertices[i];
			float proj = Vector2.Dot(v, axis);

			if(proj < min) { min = proj; }
			if(proj > max) { max = proj; }
		}
	}

	private static bool OverlapOnAxis(Vector2[] p1, Vector2[] p2, Vector2 axis, out float depth)
	{
	    float min1, max1, min2, max2;

	    ProjectPolygon(p1, axis, out min1, out max1);
	    ProjectPolygon(p2, axis, out min2, out max2);

	    if (max1 < min2 || max2 < min1)
	    {
	        depth = 0;
	        return false;
	    }

	    float overlap1 = max1 - min2;
	    float overlap2 = max2 - min1;

	    depth = MathF.Min(overlap1, overlap2);
	    return true;
	}

	private static void ProjectPolygon(Vector2[] vertices, Vector2 axis, out float min, out float max)
	{
	    min = Vector2.Dot(vertices[0], axis);
	    max = min;

	    for (int i = 1; i < vertices.Length; i++)
	    {
	        float projection = Vector2.Dot(vertices[i], axis);

	        if (projection < min)
	        {
	            min = projection;
	        }
	        else if (projection > max)
	        {
	            max = projection;
	        }
	    }
	}


	public static bool PolyLine(Vector2[] vertices, float x1, float y1, float x2, float y2)
	{
		for (int current = 0; current < vertices.Length; current++)
		{
			var next = current + 1;
			if (next == vertices.Length) next = 0;

			float x3 = vertices[current].X;
			float y3 = vertices[current].Y;
			float x4 = vertices[next].X;
			float y4 = vertices[next].Y;

			bool hit = LineLine(x1, y1, x2, y2, x3, y3, x4, y4);
			if (hit)
			{
				return true;
			}
		}
		return false;
	}
	
	public static bool LineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
	{
		float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
		float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

		if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
		{
			return true;
		}
		return false;
	}
	
	public static bool PolyPoint(Vector2[] vertices, float px, float py)
	{
		bool collision = false;
		for (int current = 0; current < vertices.Length; current++)
		{
			var next = current + 1;
			if (next == vertices.Length) next = 0;

			Vector2 vc = vertices[current];
			Vector2 vn = vertices[next];

			if (((vc.Y > py && vn.Y < py) || (vc.Y < py && vn.Y > py)) &&
			    (px < (vn.X - vc.X) * (py - vc.Y) / (vn.Y - vc.Y) + vc.X))
			{
				collision = !collision;
			}
		}
		return collision;
	}
	
	public static bool IntersectPolygonCircle(MPolygon polygon, MCircle circle, out Vector2 normal, out float depth)
	{
		normal = Vector2.Zero;
		depth = 0f;
		bool collision = false;

		int next;
		float closestDistance = float.MaxValue;
		Vector2 closestPoint = Vector2.Zero;

		for (int current = 0; current < polygon.Vertices.Length; current++)
		{
			next = current + 1;
			if (next == polygon.Vertices.Length) next = 0;

			Vector2 vc = polygon.Vertices[current];
			Vector2 vn = polygon.Vertices[next];

			if (LineCircle(vc, vn, circle.Center, circle.Radius))
			{
				collision = true;

				Vector2 edge = vn - vc;
				Vector2 axis = new Vector2(-edge.Y, edge.X);
				axis.Normalize();

				float t = Vector2.Dot(circle.Center - vc, axis);
				Vector2 projection = vc + t * axis;

				float distance = Vector2.Distance(circle.Center, projection);

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestPoint = projection;
				}
			}
		}

		if (!collision) return false;
		
		normal = circle.Center - closestPoint;
		depth = circle.Radius - closestDistance;
		normal.Normalize();

		return true;
	}

	private static bool LineCircle(Vector2 v1, Vector2 v2, Vector2 circleCenter, float radius)
	{
		bool inside1 = PointCircle(v1, circleCenter, radius);
		bool inside2 = PointCircle(v2, circleCenter, radius);

		if (inside1 || inside2) return true;

		float len = Vector2.Distance(v1, v2);
		float dot = Vector2.Dot(circleCenter - v1, v2 - v1) / (len * len);

		Vector2 closest = v1 + dot * (v2 - v1);

		bool onSegment = LinePoint(v1, v2, closest);
		if (!onSegment) return false;

		float distance = Vector2.Distance(closest, circleCenter);

		return distance <= radius;
	}

	private static bool PointCircle(Vector2 point, Vector2 circleCenter, float radius)
	{
		float distance = Vector2.Distance(point, circleCenter);
		return distance <= radius;
	}

	private static bool LinePoint(Vector2 v1, Vector2 v2, Vector2 point)
	{
		float d1 = Vector2.Distance(point, v1);
		float d2 = Vector2.Distance(point, v2);
		float lineLen = Vector2.Distance(v1, v2);

		float buffer = 0.1f;

		return (d1 + d2 >= lineLen - buffer) && (d1 + d2 <= lineLen + buffer);
	}
}