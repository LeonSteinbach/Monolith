using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;

namespace Monolith.graphics;

public class MRadioButton
{
    private readonly MStaticSprite defaultSprite, hoverSprite, checkedSprite, hoverCheckedSprite;
    private readonly string text;
    private readonly SpriteFont font;
    private readonly Color color;
    private readonly string hoverSound, clickSound;

    private bool isHovering, wasHovering;
    private bool isPressed, wasPressed;
    private bool isChecked;

    public event Action<MRadioButton> OnSelected;

    public MRadioButton(MStaticSprite defaultSprite, MStaticSprite hoverSprite, MStaticSprite checkedSprite,
        MStaticSprite hoverCheckedSprite, string text = null, SpriteFont font = null, Color color = default,
        bool isChecked = false, string hoverSound = null, string clickSound = null)
    {
        this.defaultSprite = defaultSprite ?? throw new ArgumentNullException(nameof(defaultSprite));
        this.hoverSprite = hoverSprite ?? throw new ArgumentNullException(nameof(hoverSprite));
        this.checkedSprite = checkedSprite ?? throw new ArgumentNullException(nameof(checkedSprite));
        this.hoverCheckedSprite = hoverCheckedSprite ?? throw new ArgumentNullException(nameof(hoverCheckedSprite));
        this.text = text;
        this.font = font;
        this.color = color;
        this.isChecked = isChecked;
        this.hoverSound = hoverSound;
        this.clickSound = clickSound;
    }

    public Rectangle Rectangle => Hover()
        ? (isChecked ? hoverCheckedSprite.Hitbox : hoverSprite.Hitbox)
        : (isChecked ? checkedSprite.Hitbox : defaultSprite.Hitbox);

    public string Text => text;

    public bool Hover() => isHovering;

    public bool Entered() => isHovering && !wasHovering;

    public bool Left() => !isHovering && wasHovering;

    public bool Pressed() => isPressed;

    public bool IsSelected() => isChecked;

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
    }

    public void Update()
    {
        wasHovering = isHovering;
        isHovering = Rectangle.Contains(MInput.MousePosition());

        wasPressed = isPressed;
        isPressed = isHovering && MInput.IsLeftPressed();

        if (Entered() && hoverSound != null)
        {
            MAudioManager.PlaySound(hoverSound);
        }

        if (Pressed())
        {
            if (clickSound != null)
                MAudioManager.PlaySound(clickSound);
            Select();
        }
    }

    public void Render(GameTime gameTime, SpriteBatch spriteBatch)
    {
        MStaticSprite sprite = Hover() ? 
            (isChecked ? hoverCheckedSprite : hoverSprite) : 
            (isChecked ? checkedSprite : defaultSprite);

        sprite.Render(gameTime, spriteBatch);
    }
}