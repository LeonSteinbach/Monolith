using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics
{
    public class MCheckbox : MNode
    {
        private readonly string hoverSound, clickSound;
        
        private bool isHovering, wasHovering;
        private bool isPressed;
        private bool isChecked;
        
        public event Action<MCheckbox> OnChecked, OnUnchecked, OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed;

        public MCheckbox(MStaticSprite uncheckedSprite, MStaticSprite hoverUncheckedSprite, MStaticSprite checkedSprite,
            MStaticSprite hoverCheckedSprite, bool isChecked, MText text = null, string hoverSound = null, string clickSound = null)
        {
            uncheckedSprite.Name = "unchecked";
            hoverUncheckedSprite.Name = "hoverUnchecked";
            checkedSprite.Name = "checked";
            hoverCheckedSprite.Name = "hoverChecked";
		
            AddNode(uncheckedSprite);
            AddNode(hoverUncheckedSprite);
            AddNode(checkedSprite);
            AddNode(hoverCheckedSprite);
            
            if (text != null)
            {
                text.Name = "text";
                AddNode(text);
            }
            
            this.hoverSound = hoverSound;
            this.clickSound = clickSound;
            this.isChecked = isChecked;
        }
        
        private MSprite Sprite => MouseHover() ? 
            (isChecked ? GetNode<MStaticSprite>("hoverChecked") : GetNode<MStaticSprite>("hoverUnchecked")) :
            (isChecked ? GetNode<MStaticSprite>("checked") : GetNode<MStaticSprite>("unchecked"));
        
        public MText Text => GetNode<MText>("text");

        public bool MouseHover() => isHovering;

        public bool MouseEntered() => isHovering && !wasHovering;

        public bool MouseLeft() => !isHovering && wasHovering;

        public bool MousePressed() => isPressed;

        public bool IsChecked() => isChecked;
        
        public override Rectangle Hitbox => Sprite.Hitbox;

        public override void Update(GameTime gameTime)
        {
            wasHovering = isHovering;
            isHovering = Hitbox.Contains(MInput.MousePosition());

            isPressed = isHovering && MInput.IsLeftPressed();

            if (MouseHover())
                OnMouseHover?.Invoke(this);
            
            if (MouseEntered())
            {
                if (hoverSound != null)
                    MAudioManager.PlaySound(hoverSound);
                OnMouseEntered?.Invoke(this);
            }
            
            if (MouseLeft())
                OnMouseLeft?.Invoke(this);

            if (MousePressed())
            {
                if (clickSound != null)
                    MAudioManager.PlaySound(clickSound);
                isChecked = !isChecked;
                
                OnMousePressed?.Invoke(this);
                
                if (isChecked)
                    OnChecked?.Invoke(this);
                else
                    OnUnchecked?.Invoke(this);
            }
        }

        public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsVisible) return;
		
            Sprite.Render(graphics, spriteBatch, gameTime);
            Text?.Render(graphics, spriteBatch, gameTime);
        }

        public override void OnAddToNode(MNode parent) { }

        public override void OnRemoveFromNode(MNode parent) { }
    }
}
