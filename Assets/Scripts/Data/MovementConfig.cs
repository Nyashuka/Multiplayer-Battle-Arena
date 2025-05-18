using System;

namespace Data
{
	[System.Serializable]
    public struct MovementConfig : IEquatable<MovementConfig>
    {
	    public float MoveSpeed;
	    public float JumpImpulse;
	    public float UpGravity;
	    public float DownGravity;
	    public float GroundAcceleration;
	    public float GroundDeceleration;
	    public float AirAcceleration;
	    public float AirDeceleration;

		public MovementConfig(float moveSpeed, float jumpImpulse, float upGravity, float downGravity, float groundAcceleration, float groundDeceleration, float airAcceleration, float airDeceleration)
	    {
		    MoveSpeed = moveSpeed;
		    JumpImpulse = jumpImpulse;
		    UpGravity = upGravity;
		    DownGravity = downGravity;
		    GroundAcceleration = groundAcceleration;
		    GroundDeceleration = groundDeceleration;
		    AirAcceleration = airAcceleration;
		    AirDeceleration = airDeceleration;
	    }
		
	    public static MovementConfig GetDefault()
	    {
		    return new MovementConfig
		    {
	    		MoveSpeed = 10.0f,
        		JumpImpulse = 10.0f,
        		UpGravity = -25.0f,
        		DownGravity = -40.0f,
        		GroundAcceleration = 55.0f,
        		GroundDeceleration = 25.0f,
        		AirAcceleration = 25.0f,
        		AirDeceleration = 1.3f
		    };
	    }

	    public bool Equals(MovementConfig other)
	    {
		    return MoveSpeed.Equals(other.MoveSpeed) && JumpImpulse.Equals(other.JumpImpulse) && UpGravity.Equals(other.UpGravity) && DownGravity.Equals(other.DownGravity) && GroundAcceleration.Equals(other.GroundAcceleration) && GroundDeceleration.Equals(other.GroundDeceleration) && AirAcceleration.Equals(other.AirAcceleration) && AirDeceleration.Equals(other.AirDeceleration);
	    }

	    public override bool Equals(object obj)
	    {
		    return obj is MovementConfig other && Equals(other);
	    }

	    public override int GetHashCode()
	    {
		    return HashCode.Combine(MoveSpeed, JumpImpulse, UpGravity, DownGravity, GroundAcceleration, GroundDeceleration, AirAcceleration, AirDeceleration);
	    }
    }
}