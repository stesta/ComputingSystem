using Godot;
using ComputingSystem.Core;
using System.Collections.Generic;

namespace ComputingSystem.Peripherals;

public partial class Screen : Sprite2D
{
    private Image _image;
    private ImageTexture _texture;

	// screen variables
	private int _rows = 256;
    private int _columns = 512;
    private int _wordsPerRow = 32;
    private int _bitsPerWord = 16;

    [Export]
	public Color Background { get; set; } = new Color(0, 0, 0);

	[Export]
	public Color Foreground { get; set; } = new Color(1, 1, 1);

	public override void _Ready()
	{
		// Create the image
		_image = Image.CreateEmpty(_columns, _rows, false, Image.Format.Rgb8); 
		_image.Fill(Background);

		// Create the texture from the image
		_texture = ImageTexture.CreateFromImage(_image);
		Texture = _texture;

		// Scale up the sprite so pixels are visible
		Scale = new Vector2(2, 2);
	}

    public void SetPixels(List<PixelChange> pixels)
	{
        foreach(var pixel in pixels)
        {
            _image.SetPixel(pixel.X, pixel.Y, pixel.Value == 1 ? Foreground : Background);
        }

        _texture.Update(_image);
    }
}
