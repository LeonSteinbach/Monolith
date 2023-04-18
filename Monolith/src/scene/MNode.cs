using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.scene;

public abstract class MNode
{
	private Vector2 position = Vector2.Zero;
	private Vector2 scale = Vector2.One;
	private float rotation;
	
	private readonly List<MNode> nodes = new();
	
	public string Name { get; set; }

	public bool IsVisible { get; set; } = true;
	public MNode Parent { get; private set; }
	
	public abstract Rectangle Hitbox { get; }

	public Vector2 Position
	{
		get => position;
		set
		{
			position = value;
			OnTransformPosition();
		}
	}

	public float Rotation
	{
		get => rotation;
		set
		{
			rotation = value;
			OnTransformRotation();
		}
	}

	public Vector2 Scale
	{
		get => scale;
		set
		{
			scale = value;
			OnTransformScale();
		}
	}
	
	public void AddNode(MNode node)
	{
		node.Parent = this;
		nodes.Add(node);
		node.OnAddToNode(this);
	}

	public void RemoveNode(MNode node)
	{
		node.Parent = null;
		nodes.Remove(node);
		node.OnRemoveFromNode(this);
	}

	public abstract void Update(GameTime gameTime);

	public void UpdateChildren(GameTime gameTime)
	{
		foreach (var node in nodes)
			node.Update(gameTime);
	}

	public abstract void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime);

	public void RenderChildren(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		foreach (var node in nodes)
			node.Render(graphics, spriteBatch, gameTime);
	}
	
	public T GetNode<T>(string name) where T : MNode
	{
		if (this is T && Name == name)
			return (T)this;

		foreach (var node in nodes)
		{
			var result = node.GetNode<T>(name);
			if (result != null)
				return result;
		}

		return null;
	}

	public List<T> GetAllNodes<T>() where T : MNode
	{
		var result = new List<T>();

		foreach (var node in nodes)
		{
			if (node is T tNode)
				result.Add(tNode);
			result.AddRange(node.GetAllNodes<T>());
		}

		return result;
	}


	public abstract void OnAddToNode(MNode parent);
	
	public abstract void OnRemoveFromNode(MNode parent);

	public virtual void OnTransformPosition()
	{
		foreach (var sprite in GetAllNodes<MNode>())
			sprite.Position = Position;
	}

	public virtual void OnTransformRotation()
	{
		foreach (var sprite in GetAllNodes<MNode>())
			sprite.Rotation = Rotation;
	}

	public virtual void OnTransformScale()
	{
		foreach (var sprite in GetAllNodes<MNode>())
			sprite.Scale = Scale;
	}
}