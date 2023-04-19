using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.scene;

namespace Monolith.graphics;

public class MAnimatedSprite : MSprite
{
    private readonly Texture2D texture;
    private readonly int columns;
    private readonly int frameWidth, frameHeight;
    private int currentFrame, frames;
    private int delay;
    private double elapsed;

    public bool Playing { get; private set; }
    public bool Looping { get; set; }
    public bool Flipped { get; set; }
    public override bool Centered { get; set; } = true;
    public override Color Color { get; set; } = Color.White;
    public override float Layer { get; set; }

    public MAnimatedSprite(Texture2D texture, int columns, int rows, int frames, int delay, bool looping = true, bool flipped = false)
    {
        this.texture = texture;
        this.columns = columns;
        this.frames = frames;
        this.delay = delay;

        frameWidth = texture.Width / columns;
        frameHeight = texture.Height / rows;

        currentFrame = 0;
        elapsed = 0d;

        Looping = looping;
        Flipped = flipped;
    }

    public override Rectangle Hitbox
    {
        get
        {
            int width = (int)(frameWidth * Scale.X);
            int height = (int)(frameHeight * Scale.Y);
            int x = (int)(Position.X - Origin.X * Scale.X);
            int y = (int)(Position.Y - Origin.Y * Scale.Y);

            return new Rectangle(x, y, width, height);
        }
    }

    public override Vector2 Origin => Centered ? new Vector2(frameWidth / 2f, frameHeight / 2f) : Vector2.Zero;

    public void Start()
    {
        currentFrame = 0;
        Playing = true;
    }

    public void Resume()
    {
        Playing = true;
    }

    public void Pause()
    {
        Playing = false;
    }

    public void Stop()
    {
        currentFrame = 0;
        Playing = false;
    }

    private Rectangle SourceRectangle => new (
        currentFrame % columns * frameWidth + SourceOffset.X,
        currentFrame / columns + SourceOffset.Y,
        frameWidth + SourceOffset.Width,
        frameHeight + SourceOffset.Height);

    public override Rectangle SourceOffset { get; set; }

    public override void Update(GameTime gameTime)
    {
        elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (elapsed >= delay)
        {
            if (currentFrame >= frames - 1)
            {
                if (Looping)
                    currentFrame = 0;
                else
                    Stop();
            }
            else
            {
                currentFrame++;
            }

            elapsed = 0d;
        }
    }

    public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (!Playing || !IsVisible) return;

        SpriteEffects effect = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        var rectangle = new Rectangle(
            SourceRectangle.X + SourceOffset.X,
            SourceRectangle.Y + SourceOffset.Y,
            SourceRectangle.Width + SourceOffset.Width,
            SourceRectangle.Height + SourceOffset.Height);
        
        spriteBatch.Draw(texture, Position, rectangle == Rectangle.Empty ? null : rectangle, Color, Rotation, Origin, Scale, effect, Layer);
    }

    public override void OnAddToNode(MNode parent) { }

    public override void OnRemoveFromNode(MNode parent) { }

    public override void OnTransformPosition() { }

    public override void OnTransformRotation() { }

    public override void OnTransformScale() { }
}