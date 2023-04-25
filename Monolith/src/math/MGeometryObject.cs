using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.math;

public abstract class MGeometryObject
{
	public static MGeometryObject Empty { get; }
	public abstract float Area { get; }
	public abstract float Circumference { get; }
	public abstract Vector2 CenterOfMass { get; }
	public abstract Rectangle BoundingBox { get; }
	public abstract float Angle { get; protected set; }
	
	public abstract void Move(Vector2 vector);
	
	public abstract void Rotate(float angle);

	public abstract void Render(SpriteBatch spriteBatch, Color color, bool renderCenterOfMass = false, bool renderBoundingBox = false, bool renderAngle = false);
}