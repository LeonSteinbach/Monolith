using Microsoft.Xna.Framework;
using Monolith.scene;

namespace Monolith.graphics
{
    public abstract class MSprite : MNode
    {
        public abstract bool Centered { get; set; }
        public abstract Color Color { get; set; }
        public abstract float Layer { get; set; }
        public abstract Vector2 Origin { get; }
        public abstract Rectangle SourceOffset { get; set; }
    }
}