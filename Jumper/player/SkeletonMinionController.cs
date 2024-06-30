using Godot;
using System;
using ai4u;

public partial class SkeletonMinionController : Node3D, IAgentResetListener
{
	[Export]
	private RLAgent agent;

	private const int MOVE = 0;
	private const int TURN = 1;
	private const int JUMP = 2;
	private const int JUMP_FORWARD = 3;

	private float[] pActions = new float[4];

	private CharacterBody3D characterBody3D;

	private AnimationTree animationTree;

	private Vector3 animVelocity;

	private bool onFloor = false;
	private float jumpForce = 0;

	private float turn = 0;

	private float move = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		if (agent != null)
		{
			characterBody3D = agent.GetAvatarBody() as CharacterBody3D;
			agent.OnStepEnd += PlayAnimation;
			agent.AddResetListener(this);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public  void PlayAnimation(RLAgent agent)
	{
		if (agent.Alive())
		{
			float[] actions = agent.GetActionArgAsFloatArray();
			jumpForce = actions[JUMP] + actions[JUMP_FORWARD]; 
			animVelocity = characterBody3D.GetRealVelocity(); 
			turn = actions[TURN];
			move = actions[MOVE];
			pActions = actions;
			onFloor = characterBody3D.IsOnFloor();
		}
		//GD.Print(animVelocity);
	}

	public void OnReset(Agent agent)
	{
		animVelocity = Vector3.Zero;
		onFloor = false;
		jumpForce = 0;
		turn = 0;
		move = 0;
	}
}
