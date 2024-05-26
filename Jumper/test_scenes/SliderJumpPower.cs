using Godot;
using System;
using ai4u;

public partial class SliderJumpPower : HSlider
{

	[Export]
	private ArrowPhysicsMoveController controller;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (controller != null)
		{
			controller.jumpPower = (float)Value;
		}
	}
}
