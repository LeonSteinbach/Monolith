using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics;

public class MRadioButton : MNode
{
    private readonly string hoverSound, clickSound;

    private bool isHovering, wasHovering;
    private bool isPressed;
    private bool isChecked;

    public event Action<MRadioButton> OnSelected, OnDeselected, OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed;

    public MRadioButton(MStaticSprite uncheckedSprite, MStaticSprite hoverUncheckedSprite, MStaticSprite checkedSprite,
        MStaticSprite hoverCheckedSprite, string value = null, MText text = null, SpriteFont font = null, Color color = default,
        bool isChecked = false, string hoverSound = null, string clickSound = null)
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
        
        this.isChecked = isChecked;
        this.hoverSound = hoverSound;
        this.clickSound = clickSound;

        Value = value;
    }

    private MSprite Sprite => MouseHover() ? 
        (isChecked ? GetNode<MStaticSprite>("hoverChecked") : GetNode<MStaticSprite>("hoverUnchecked")) :
        (isChecked ? GetNode<MStaticSprite>("checked") : GetNode<MStaticSprite>("unchecked"));

    public string Value { get; set; }
    
    public MText Text => GetNode<MText>("text");

    public bool MouseHover() => isHovering;

    public bool MouseEntered() => isHovering && !wasHovering;

    public bool MouseLeft() => !isHovering && wasHovering;

    public bool MousePressed() => isPressed;

    public bool IsSelected() => isChecked;
    
    public override Rectangle Hitbox => Sprite.Hitbox;

    public void Select()
    {
        if (!isChecked)
        {
            isChecked = true;
            OnSelected?.Invoke(this);
        }
    }

    public void Deselect()
    {
        isChecked = false;
        OnDeselected?.Invoke(this);
    }

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
            Select();
            OnMousePressed?.Invoke(this);
        }
    }

    public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
    {
        Sprite.Render(graphics, spriteBatch, gameTime);
        Text?.Render(graphics, spriteBatch, gameTime);
    }

    public override void OnAddToNode(MNode parent) { }

    public override void OnRemoveFromNode(MNode parent) { }
}