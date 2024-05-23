using Godot;
using System;
using ai4u;

public partial class SkeletonMinionController : Node3D
{
	[Export]
	private BasicAgent agent;

	private const int MOVE = 0;
	private const int TURN = 1;
	private const int JUMP = 2;
	private const int JUMP_FORWARD = 3;

	private float[] pActions = new float[4];

	private CharacterBody3D characterBody3D;

	private AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (agent != null)
		{
			characterBody3D = agent.GetAvatarBody() as CharacterBody3D;
			agent.beginOfStepEvent += PlayAnimation;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public  void PlayAnimation(BasicAgent agent)
	{
		float[] actions = agent.GetActionArgAsFloatArray();
		if (characterBody3D != null)
		{
			if (characterBody3D.IsOnFloor())
			{

				if (actions[JUMP] > 0 && actions[JUMP] <= 0.2f)
				{
					animationPlayer.Play("Jump_Start");
				}
				else if (actions[JUMP] > 0.2f)
				{
					animationPlayer.Play("Jump_Full_Long", customBlend:0.9);
				}
				else if (characterBody3D.Velocity.Length() > 0.05f && characterBody3D.Velocity.Length() < 5)
				{
					animationPlayer.Play("Walking_A", customBlend:0.9);
				}
				else if (characterBody3D.Velocity.Length() >= 5)
				{
					animationPlayer.Play("Running_A", customBlend:0.9);
				}
				else if ( Math.Abs(actions[TURN]) > 0.05f)
				{
					animationPlayer.Play("Walking_A", customBlend:0.9, customSpeed: Math.Abs(actions[TURN]));
				}
				else
				{
					animationPlayer.Play("Idle", customBlend:0.9);
				}
			}
			else
			{
				if (characterBody3D.Velocity.Y > 0)
				{
					animationPlayer.Play("Jump_Full_Long");
				}
				else if (characterBody3D.Velocity.Y < 0)
				{
					animationPlayer.Play("Jump_Idle");
				}
			}
		}
		else
		{
			animationPlayer.Play("Idle");
		}

		pActions = actions;
	}
}
