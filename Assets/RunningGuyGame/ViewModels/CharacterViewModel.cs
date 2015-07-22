using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class CharacterViewModel {
    public override bool ComputeShouldMoveLeft()
    {
        return MovementIntention == MovementIntention.Left;
    }

    public override bool ComputeShouldMoveRight()
    {
        return MovementIntention == MovementIntention.Right;
    }

    public override bool ComputeShouldStop()
    {
        return MovementIntention == MovementIntention.Stop;
    }

    public override bool ComputeIsNotOnTheGround()
    {
        return !IsOnTheGround;
    }

    public override bool ComputeShouldJump()
    {
        return IsOnTheGround && JumpIntention == JumpIntention.Jump;
    }
}
