using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monolith.math;

namespace Monolith.camera;

public class MCamera
{
    private Point windowSize;

    private readonly Dictionary<string, Matrix> matricesCache;
    private const int matricesCacheCapacity = 100;

    private float rotation;
    public float Rotation
    {
	    get => rotation;
	    set => rotation = value;
    }

    private float delay = 100.0f;
    public float Delay
    {
	    get => delay;
	    set => delay = value;
    }

    private float zoomDelay = 1.0f;
    public float ZoomDelay
    {
	    get => zoomDelay;
	    set => zoomDelay = value;
    }

    private float zoomLevel = 1.0f;
    public float ZoomLevel
    {
	    get => zoomLevel;
	    set => zoomLevel = value;
    }

    private float targetZoom = 1.0f;
    public float TargetZoom
    {
	    get => targetZoom;
	    set => targetZoom = value;
    }

    private float minZoom;
    public float MinZoom
    {
	    get => minZoom;
	    set => minZoom = value;
    }

    private float maxZoom = float.PositiveInfinity;
    public float MaxZoom
    {
	    get => maxZoom;
	    set => maxZoom = value;
    }

    private float zoomSpeed = 1.0f;
    public float ZoomSpeed
    {
	    get => zoomSpeed;
	    set => zoomSpeed = value;
    }
    
	private bool shaking;
	private float shakingForce;
	private int shakingDuration;
	private DateTime shakingStarted;
	private Vector2 position;
	private Vector2 targetPosition;

	public MCamera(Point windowSize)
	{
		this.windowSize = windowSize;
		
		position = windowSize.ToVector2() / 2f;
		targetPosition = position;

		matricesCache = new Dictionary<string, Matrix>();
	}

	public Matrix TransformationMatrix()
	{
		string key = position + " | " + $"{rotation:N8}" + " | " + $"{rotation:N8}" + " | " + windowSize;
		
		if (!matricesCache.ContainsKey(key))
		{
			Matrix matrix = 
				Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				Matrix.CreateScale(ZoomLevel, ZoomLevel, 1.0f) *
				Matrix.CreateTranslation(new Vector3(windowSize.X / 2f, windowSize.Y / 2f, 0));
			
			matricesCache.Add(key, matrix);
			return matrix;
		}

		if (matricesCache.Count > matricesCacheCapacity)
		{
			for (int i = 0; i < matricesCache.Count; i++)
			{
				string cachedKey = matricesCache.ElementAt(i).Key;
				if (cachedKey != key)
				{
					matricesCache.Remove(cachedKey);
					break;
				}
			}
		}

		return matricesCache[key];
	}

	public Vector2 ScreenToWorld(Vector2 vector)
	{
		return Vector2.Transform(vector, Matrix.Invert(TransformationMatrix()));
	}

	public Point ScreenToWorld(Point point)
	{
		return Vector2.Transform(point.ToVector2(), Matrix.Invert(TransformationMatrix())).ToPoint();
	}

	public Vector2 WorldToScreen(Vector2 vector)
	{
		return Vector2.Transform(vector, TransformationMatrix());
	}

	public Point WorldToScreen(Point point)
	{
		return Vector2.Transform(point.ToVector2(), TransformationMatrix()).ToPoint();
	}

	public Rectangle VisibleArea()
	{
		var inverseViewMatrix = Matrix.Invert(TransformationMatrix());

		var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
		var tr = Vector2.Transform(new Vector2(windowSize.X, 0), inverseViewMatrix);
		var bl = Vector2.Transform(new Vector2(0, windowSize.Y), inverseViewMatrix);
		var br = Vector2.Transform(windowSize.ToVector2(), inverseViewMatrix);

		var min = new Vector2(
			Math.Min(tl.X, Math.Min(tr.X, Math.Min(bl.X, br.X))),
			Math.Min(tl.Y, Math.Min(tr.Y, Math.Min(bl.Y, br.Y))));
		var max = new Vector2(
			Math.Max(tl.X, Math.Max(tr.X, Math.Max(bl.X, br.X))),
			Math.Max(tl.Y, Math.Max(tr.Y, Math.Max(bl.Y, br.Y))));

		return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
	}

	public void Zoom(int direction)
	{
		TargetZoom += Math.Sign(direction) * ZoomSpeed;

		if (TargetZoom >= MaxZoom)
		{
			TargetZoom = MaxZoom;
		}

		if (TargetZoom <= MinZoom)
		{
			TargetZoom = MinZoom;
		}
	}

	public void Shake(float force, int duration)
	{
		shaking = true;
		shakingForce = force;
		shakingDuration = duration;
		shakingStarted = DateTime.Now;
	}

	public void Follow(Vector2 target)
	{
		targetPosition = target;
	}

	public void Update(GameTime gameTime)
	{
		if (shaking)
		{
			targetPosition = position + new Vector2(
				MMathHelper.RandFloat() * 2 - 1, MMathHelper.RandFloat() * 2 - 1) * shakingForce;

			if ((DateTime.Now - shakingStarted).TotalMilliseconds >= shakingDuration)
			{
				shaking = false;
			}
		}

		position += (targetPosition - position) / delay * gameTime.ElapsedGameTime.Milliseconds;
		ZoomLevel += (targetZoom - ZoomLevel) / zoomDelay * gameTime.ElapsedGameTime.Milliseconds;
	}
}