using Godot;


public class MagicCirclePNG : Node2D
{
    public string circle = "res://Assets/Magical Circles/Circle6.png";

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        Rotation += .005f;
    }

    public void Activate()
    {
        Visible = true;

    }

    public void SetCircle(int circleId)
    {
        circle = "res://Assets/Magical Circles/Circle" + circleId + ".png";

    }

    public void SetColor(Color newColor)
    {
        GetNode<Sprite>("PNGSprite").Modulate = newColor;
    }
}