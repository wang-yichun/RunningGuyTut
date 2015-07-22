using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class CharacterAvatarView {
    
    /// Subscribes to the state machine property and executes a method for each state.
    public override void MovementStateChanged(Invert.StateMachine.State value) {
        base.MovementStateChanged(value);
    }
    
    public override void OnIdle() {
        base.OnIdle();
    }
    
    public override void OnMoveLeft() {
        base.OnMoveLeft();
    }
    
    public override void OnMoveRight() {
        base.OnMoveRight();
    }
}
