using Godot;
using System;
using ai4u;

public partial class MovingBlock : StaticBody3D
{


	public DynamicFallReward dynamicFallReward;


	[Export]
	private float horizontalBlockDistance = 7;


	[Export]
	private float verticalBlockDistance = 3;

	public PackedScene movingBlockScene;


	private bool playerOver = false;

	private MovingBlock movingBlock = null;


	public MovingBlock movingBlockParent {get; set;} = null;

	public Node3D agentTarget {get; set;} = null;

	private RLAgent agent;

	private bool receivedReward = false;

	public MovingBlock Child
	{
		get
		{
			return movingBlock;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		agent = (RLAgent)GetTree().GetFirstNodeInGroup("player").GetNode("Agent");
		if (movingBlockParent == null)
		{
			BuildNeighborhood();
		}
	}

	public void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("player"))
		{
			playerOver = true;
			if (!receivedReward)
			{
				agent.Rewards.Add(10);
				receivedReward = true;
			}
			if (movingBlock == null)
			{
				BuildNeighborhood();				
			}
			if (movingBlockParent != null)
			{
				movingBlockParent.Visible = false;
			}
		}
	}

	private void BuildNeighborhood()
	{
		movingBlock = movingBlockScene.Instantiate<MovingBlock>();
		movingBlock.movingBlockScene = movingBlockScene;
		movingBlock.movingBlockParent = this;

		int relativeHorizontalPosition = GD.RandRange(0, 3);
		int relativeVerticalPosition = GD.RandRange(0, 2);

		float dx = 0, dy = 0, dz = 0;

		if (relativeHorizontalPosition == 0) //FRONT
		{
			dz = horizontalBlockDistance;				
		}
		else if (relativeHorizontalPosition == 1) //BACK
		{
			dz = -horizontalBlockDistance;
		}
		else if (relativeHorizontalPosition == 3) //RIGHT
		{
			dx = horizontalBlockDistance;
		}
		else //LEFT
		{
			dx = -horizontalBlockDistance;
		}

		if (relativeVerticalPosition == 1) //UP
		{
			dy = verticalBlockDistance;
		}
		else if (relativeVerticalPosition == 2) //DOWN
		{
			dy = -verticalBlockDistance;
		}
		GetParent().AddChild(movingBlock);

		if (Transform.Origin.Y < movingBlock.Transform.Origin.Y)
		{
			dynamicFallReward.dynamicReference = this;
		}
		else
		{
			dynamicFallReward.dynamicReference = movingBlock;
		}
		movingBlock.dynamicFallReward = dynamicFallReward;
		movingBlock.agentTarget = this.agentTarget;
		movingBlock.Position = Position + new Vector3(dx, dy, dz);
		agentTarget.Transform = movingBlock.Transform;
	}


	public void OnBodyExited(Node3D body)
	{
	}
}
