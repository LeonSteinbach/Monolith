namespace Monolith.scene;

public class MLayer
{
	public int Index { get; }
	public float Depth { get; }

	public MLayer(int index, float depth)
	{
		Index = index;
		Depth = depth;
	}
}