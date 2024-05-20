using Godot;
using System;


namespace ai4u;

public partial class CBMoveActuator : Actuator
{
    //forces applied on the x, y and z axes.    
    private float move, turn, jump, jumpForward;

    [ExportCategory("Movement")]
    [Export]
    private float moveAmount = 1;
    [Export]
    private float turnAmount = 1;
    [Export]
    private float jumpPower = 1;
    [Export]
    private float jumpForwardPower = 1;
    [Export]
    private float collisionShapeHalfHeight = 1.0f;
    [Export]
    private float precision = 0.001f;
    [Export]
    private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private float lerpFactor = 0.4f;


    private BasicAgent agent;
    
    private CharacterBody3D body;
    private PhysicsDirectSpaceState3D spaceState;

    private CollisionShape3D collisionShape;

    public CBMoveActuator()
    {

    }

    public override void OnSetup(Agent agent)
    {
        shape = new int[1]{4};
        isContinuous = true;
        rangeMin = new float[]{0, -1, 0, 0};
        rangeMax = new float[]{1, 1, 1, 1};
        this.agent = (BasicAgent) agent;
        agent.AddResetListener(this);
        body = this.agent.GetAvatarBody() as CharacterBody3D;
        this.spaceState = body.GetWorld3D().DirectSpaceState;
    }

    public bool OnGround
    {
        get
        {
            return body.IsOnFloor();
        }
    }

    public override void Act()
    {
        double delta = this.agent.GetPhysicsProcessDeltaTime();
        Vector3 velocity = body.Velocity;
        if (agent != null && !agent.Done)
        {
            float[] action = agent.GetActionArgAsFloatArray();
            move = action[0];
            turn = action[1];
            jump = action[2];
            jumpForward = action[3];
            if (Mathf.Abs(turn) < precision)
            {
                turn = 0;
            }
						
            if (Mathf.Abs(jump) < precision)
            {
                jump = 0;
            }
						
            if (Mathf.Abs(jumpForward) < precision)
            {
                jumpForward = 0;
            }
            
            // Add the gravity.
            if (!body.IsOnFloor())
            {
			    velocity.Y -= gravity * (float)delta;
            }
            else
            {
                if (jump > 0)
                {
                    velocity.Y = jump * jumpPower;
                }

                
                if ( Math.Abs(turn) > 0)
                {
                    var newdirection = Quaternion.FromEuler(new Vector3(0, turn * turnAmount, 0)) * body.Basis.Z;
                
                    var targetposition = body.Position + newdirection;

                    body.LookAt(targetposition);
                }
                
                
                // Get the input direction and handle the movement/deceleration.
                Vector3 direction = (body.Transform.Basis.Z * move).Normalized();

                
                if (direction != Vector3.Zero)
                {
                    velocity = direction * moveAmount;
                }
                else
                {
                    velocity.X = Mathf.Lerp(body.Velocity.X, 0, lerpFactor);
                    velocity.Z = Mathf.Lerp(body.Velocity.Z, 0, lerpFactor);
                }
            }
            body.Velocity = velocity;
            body.MoveAndSlide();
        }
        move = 0;
        turn = 0;
        jump = 0;
        jumpForward = 0;
    }

    public override void OnReset(Agent agent)
    {
        turn = 0;
        move = 0;
        jump = 0;
        jumpForward = 0;
    }
}
