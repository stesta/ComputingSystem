using Godot;
using System;

public partial class Screen : Sprite2D
{
	private Image image;
	private ImageTexture texture;

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
		image = Image.CreateEmpty(Width, Height, false, Image.Format.Rgb8); 
		image.Fill(Background);

		// Create the texture from the image
		texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;

		// Scale up the sprite so pixels are visible
		Scale = new Vector2(2, 2);

		// turn on a few pixels
		SetPixel(10, 10, true);
		SetPixel(11, 10, true);
		SetPixel(12, 10, true);

	}

	public void SetPixel(int x, int y, bool on)
	{
		if (x < 0 || x >= Width || y < 0 || y >= Height)
			return;

		image.SetPixel(x, y, on ? Foreground : Background);
		texture.Update(image);
	}
}
