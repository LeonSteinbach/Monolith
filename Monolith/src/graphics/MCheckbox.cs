using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;

namespace Monolith.graphics
{
    public class MCheckbox
    {
        private readonly MStaticSprite defaultSprite, hoverUncheckedSprite, checkedSprite, hoverCheckedSprite;
        private readonly string hoverSound, clickSound;
        
        private bool isHovering, wasHovering;
        private bool isPressed;
        private bool isChecked;

        public MCheckbox(MStaticSprite defaultSprite, MStaticSprite hoverUncheckedSprite, MStaticSprite checkedSprite,
            MStaticSprite hoverCheckedSprite, bool isChecked, string hoverSound = null, string clickSound = null)
        {
            this.defaultSprite = defaultSprite ?? throw new ArgumentNullException(nameof(defaultSprite));
            this.hoverUncheckedSprite = hoverUncheckedSprite ?? throw new ArgumentNullException(nameof(hoverUncheckedSprite));
            this.checkedSprite = checkedSprite ?? throw new ArgumentNullException(nameof(checkedSprite));
            this.hoverCheckedSprite = hoverCheckedSprite ?? throw new ArgumentNullException(nameof(hoverCheckedSprite));
            this.hoverSound = hoverSound;
            this.clickSound = clickSound;
            this.isChecked = isChecked;
        }

        public Rectangle Rectangle => Hover() ? (isChecked ? hoverCheckedSprite.Hitbox : hoverUncheckedSprite.Hitbox) :
                                               (isChecked ? checkedSprite.Hitbox : defaultSprite.Hitbox);

        public bool Hover() => isHovering;

        public bool Entered() => isHovering && !wasHovering;

        public bool Left() => !isHovering && wasHovering;

        public bool Pressed() => isPressed;

        public bool IsChecked() => isChecked;

        public void Update()
        {
            wasHovering = isHovering;
            isHovering = Rectangle.Contains(MInput.MousePosition());

            isPressed = isHovering && MInput.IsLeftPressed();

            if (Entered() && hoverSound != null)
            {
                MAudioManager.PlaySound(hoverSound);
            }

            if (Pressed())
            {
                if (clickSound != null)
                    MAudioManager.PlaySound(clickSound);
                isChecked = !isChecked;
            }
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MStaticSprite sprite = Hover() ? (isChecked ? hoverCheckedSprite : hoverUncheckedSprite) :
                                             (isChecked ? checkedSprite : defaultSprite);

            sprite.Render(gameTime, spriteBatch);
        }
    }
}
