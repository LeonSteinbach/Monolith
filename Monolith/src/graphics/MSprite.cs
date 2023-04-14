using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.graphics
{
    public interface IMSprite
    {
        public Vector2 Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Layer { get; set; }
        public void Render(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public class MAnimatedSprite : IMSprite
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
        public Vector2 Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public float Layer { get; set; }

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
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, frameWidth, frameHeight);
        }

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

        private Rectangle SourceRectangle => new Rectangle(
            currentFrame % columns * frameWidth,
            currentFrame / columns,
            frameWidth,
            frameHeight);

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Playing)
                return;

            SpriteEffects effect = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, effect, Layer);

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

            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, frameWidth, frameHeight);
        }
    }

    public class MStaticSprite : IMSprite
    {
        private readonly Texture2D texture;
        public Vector2 Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public float Layer { get; set; }

        public MStaticSprite(Texture2D texture)
        {
            this.texture = texture;
            Position = Vector2.Zero;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }
    }
}
