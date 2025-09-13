using Godot;
using System;
using System.Diagnostics;

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

    // Signal Handler for Screen Updates
    public void SetPixels(int[] pixels)
	{
        for (int i = 0; i < 8192; i++)
        {
            ushort word = (ushort)pixels[i];
            int row = i / _wordsPerRow;
            int colStart = (i % _wordsPerRow) * _bitsPerWord;

            for (int bit = 0; bit < _bitsPerWord; bit++)
            {
                int x = colStart + bit;
                int y = row;
                bool isSet = (word & (1 << bit)) != 0;
                _image.SetPixel(x, y, isSet ? Foreground : Background);
            }
        }

        _texture.Update(_image);
    }
}
