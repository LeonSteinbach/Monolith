using Microsoft.Xna.Framework;
using Monolith.math;
using Monolith.physics;

namespace Sandbox;

public class Platform : MBody
{
	public override MGeometryObject Hitbox { get; }

	public Platform(Vector2[] vertices)
	{
		Hitbox = new MPolygon(vertices);
		IsStatic = true;
	}
}