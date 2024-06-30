using ai4u;
using Godot;
using System;

public partial class ShowSteps : Label
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	private NodePath agentPath;
	private RLAgent agent;



	public override void _Ready()
	{
		agent = GetNode<RLAgent>(agentPath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = "Steps " + agent.NSteps;
	}
}
