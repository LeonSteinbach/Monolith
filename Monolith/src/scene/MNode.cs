using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.scene;
public abstract class MNode
{
    private Vector2 position = Vector2.Zero;
    private Vector2 scale = Vector2.One;
    private float rotation;

    public string Name { get; set; }
    public bool IsVisible { get; set; } = true;
    public MScene Parent { get; internal set; }
    public MLayer Layer { get; set; }

    public MNode()
    {
        
    }

    public MNode(Vector2 position, Vector2 scale, float rotation, string name = null)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
        Name = name;
    }

    public Vector2 Position
    {
        get => position;
        set
        {
            position = value;
            OnTransformChanged();
        }
    }

    public float Rotation
    {
        get => rotation;
        set
        {
            rotation = value;
            OnTransformChanged();
        }
    }

    public Vector2 Scale
    {
        get => scale;
        set
        {
            scale = value;
            OnTransformChanged();
        }
    }

    public Vector2 GlobalPosition
    {
        get
        {
            var result = Position;
            var parent = Parent;
            while (parent != null)
            {
                result += parent.Position;
                parent = parent.Parent;
            }
            return result;
        }
    }

    public float GlobalRotation
    {
        get
        {
            var result = Rotation;
            var parent = Parent;
            while (parent != null)
            {
                result += parent.Rotation;
                parent = parent.Parent;
            }
            return result;
        }
    }

    public Vector2 GlobalScale
    {
        get
        {
            var result = Scale;
            var parent = Parent;
            while (parent != null)
            {
                result *= parent.Scale;
                parent = parent.Parent;
            }
            return result;
        }
    }

    public void AddToScene(MScene scene)
    {
        scene.AddNode(this);
    }

    public void RemoveFromScene()
    {
        Parent?.RemoveNode(this);
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime);

    public abstract void OnAddToScene(MScene scene);

    public abstract void OnRemoveFromScene(MScene scene);

    protected abstract void OnTransformChanged();
}