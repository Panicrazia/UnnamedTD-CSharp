using Godot;


public class WaveTimer : Timer
{
    public int ticks;

    public override void _Ready()
    {
        Connect("timeout", this, "Countdown");

    }

    public void Countdown()
    {
        ticks -= 1;
        if (ticks <= 0)
        {
            GD.Print("committing seppuku");
            QueueFree();
        }
    }
}