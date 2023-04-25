using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;

namespace Monolith.math;

public class MCircle : MGeometryObject
{
	public new static MCircle Empty => new (Vector2.Zero, 0);
	
	public Vector2 Center { get; private set; }
	
	public float Radius { get; }

	public override float Area => MathF.PI * Radius * Radius;

	public override float Circumference => 2 * MathF.PI * Radius;

	public override Vector2 CenterOfMass => Center;

	public override Rectangle BoundingBox { get; }
	
	public override float Angle { get; protected set; }

	public MCircle(Vector2 center, float radius)
	{
		Center = center;
		Radius = radius;

		BoundingBox = new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)Radius * 2, (int)Radius * 2);
	}
	
	public override void Move(Vector2 vector)
	{
		Center += vector;
	}

	public override void Rotate(float angle)
	{
		Angle += angle;
	}

	public override void Render(SpriteBatch spriteBatch, Color color, bool renderCenterOfMass = false, bool renderBoundingBox = false, bool renderAngle = false)
	{
		MShapes.DrawCircle(spriteBatch, Center, Radius, (int)Circumference, color, 1);
		
		if (renderCenterOfMass)
			MShapes.DrawCircle(spriteBatch, CenterOfMass, 5, 10, Color.Red, 5);
		
		if (renderBoundingBox)
			MShapes.DrawRectangle(spriteBatch, BoundingBox, Color.White, 1);
		
		if (renderAngle)
		{
			Vector2 direction = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle));
			direction.Normalize();
			direction *= Radius;
			
			MShapes.DrawLine(spriteBatch, CenterOfMass, CenterOfMass + direction, Color.Red, 1f);
		}
	}
}