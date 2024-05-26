using Godot;
using System;
using ai4u;

public partial class GameManager : Node, IAgentResetListener
{

	[Export]
	private BasicAgent agent;

	[Export]
	private PackedScene movingBlockScene;

	[Export]
	private DynamicFallReward fallReward;

	[Export]
	private Node3D agentTarget;

	private MovingBlock movingBlock;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		agent.AddResetListener(this);

	}

	public void OnReset(Agent agent)
	{
		CharacterBody3D body = this.agent.GetAvatarBody() as CharacterBody3D;
		body.Velocity = Vector3.Zero;
		body.MoveAndSlide();
		while (movingBlock != null)
		{
			var c = movingBlock.Child;
			movingBlock.Visible = false;
			GetTree().QueueDelete(movingBlock);
			movingBlock = c;
		}
		movingBlock = movingBlockScene.Instantiate<MovingBlock>();
		movingBlock.agentTarget = agentTarget;
		movingBlock.dynamicFallReward = fallReward;
		fallReward.dynamicReference = movingBlock;
		movingBlock.movingBlockScene = movingBlockScene;
		movingBlock.Position = Vector3.Zero;
		AddChild(movingBlock);
		body.Position = movingBlock.Position + Vector3.Up*3;
	}
}
