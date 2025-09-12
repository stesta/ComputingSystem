using Godot;
using System;

public partial class Screen : Sprite2D
{
    private Image _image;
    private ImageTexture _texture;

	[Export]
	public int Width { get; set; } = 512;

	[Export]
	public int Height { get; set; } = 256;

	[Export]
	public Color Background { get; set; } = new Color(0, 0, 0);

	[Export]
	public Color Foreground { get; set; } = new Color(1, 1, 1);

	public override void _Ready()
	{
		// Create the image
		_image = Image.CreateEmpty(Width, Height, false, Image.Format.Rgb8); 
		_image.Fill(Background);

		// Create the texture from the image
		_texture = ImageTexture.CreateFromImage(_image);
		Texture = _texture;

		// Scale up the sprite so pixels are visible
		Scale = new Vector2(2, 2);
	}

	// responds to Hardware Clock signal
    public void _on_timer_timeout(int clock)
    {
        // turn on a few pixels
		// eventually we'll read from the screen block of memory and update pixels. 
        SetPixel(10, 10, clock == 1);
        SetPixel(11, 10, clock == 1);
        SetPixel(12, 10, clock == 1);
    }

    public void SetPixel(int x, int y, bool on)
	{
		if (x < 0 || x >= Width || y < 0 || y >= Height)
			return;

		_image.SetPixel(x, y, on ? Foreground : Background);
		_texture.Update(_image);
	}
}
