// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;


public class CharacterMovementStateMachineBase : Invert.StateMachine.StateMachine {
    
    private StateMachineTrigger _GoLeft;
    
    private StateMachineTrigger _GoRight;
    
    private StateMachineTrigger _Stop;
    
    private Idle _Idle;
    
    private MoveLeft _MoveLeft;
    
    private MoveRight _MoveRight;
    
    public CharacterMovementStateMachineBase(ViewModel vm, string propertyName) : 
            base(vm, propertyName) {
    }
    
    public virtual StateMachineTrigger GoLeft {
        get {
            if ((this._GoLeft == null)) {
                this._GoLeft = new StateMachineTrigger(this, "GoLeft");
            }
            return this._GoLeft;
        }
    }
    
    public virtual StateMachineTrigger GoRight {
        get {
            if ((this._GoRight == null)) {
                this._GoRight = new StateMachineTrigger(this, "GoRight");
            }
            return this._GoRight;
        }
    }
    
    public virtual StateMachineTrigger Stop {
        get {
            if ((this._Stop == null)) {
                this._Stop = new StateMachineTrigger(this, "Stop");
            }
            return this._Stop;
        }
    }
    
    public override Invert.StateMachine.State StartState {
        get {
            return this.Idle;
        }
    }
    
    public virtual Idle Idle {
        get {
            if ((this._Idle == null)) {
                this._Idle = new Idle();
            }
            return this._Idle;
        }
    }
    
    public virtual MoveLeft MoveLeft {
        get {
            if ((this._MoveLeft == null)) {
                this._MoveLeft = new MoveLeft();
            }
            return this._MoveLeft;
        }
    }
    
    public virtual MoveRight MoveRight {
        get {
            if ((this._MoveRight == null)) {
                this._MoveRight = new MoveRight();
            }
            return this._MoveRight;
        }
    }
    
    public override void Compose(List<State> states) {
        base.Compose(states);
        this.Idle.StateMachine = this;
        Idle.GoLeft = new StateTransition("GoLeft", Idle,MoveLeft);
        Idle.GoRight = new StateTransition("GoRight", Idle,MoveRight);
        Idle.AddTrigger(GoLeft, Idle.GoLeft);
        Idle.AddTrigger(GoRight, Idle.GoRight);
        states.Add(Idle);
        this.MoveLeft.StateMachine = this;
        MoveLeft.Stop = new StateTransition("Stop", MoveLeft,Idle);
        MoveLeft.GoRight = new StateTransition("GoRight", MoveLeft,MoveRight);
        MoveLeft.AddTrigger(Stop, MoveLeft.Stop);
        MoveLeft.AddTrigger(GoRight, MoveLeft.GoRight);
        states.Add(MoveLeft);
        this.MoveRight.StateMachine = this;
        MoveRight.GoLeft = new StateTransition("GoLeft", MoveRight,MoveLeft);
        MoveRight.Stop = new StateTransition("Stop", MoveRight,Idle);
        MoveRight.AddTrigger(GoLeft, MoveRight.GoLeft);
        MoveRight.AddTrigger(Stop, MoveRight.Stop);
        states.Add(MoveRight);
    }
}

public class Idle : Invert.StateMachine.State {
    
    private StateTransition _GoLeft;
    
    private StateTransition _GoRight;
    
    public virtual StateTransition GoLeft {
        get {
            return this._GoLeft;
        }
        set {
            _GoLeft = value;
        }
    }
    
    public virtual StateTransition GoRight {
        get {
            return this._GoRight;
        }
        set {
            _GoRight = value;
        }
    }
    
    public override string Name {
        get {
            return "Idle";
        }
    }
    
    private void GoLeftTransition() {
        this.Transition(this.GoLeft);
    }
    
    private void GoRightTransition() {
        this.Transition(this.GoRight);
    }
}

public class MoveLeft : Invert.StateMachine.State {
    
    private StateTransition _Stop;
    
    private StateTransition _GoRight;
    
    public virtual StateTransition Stop {
        get {
            return this._Stop;
        }
        set {
            _Stop = value;
        }
    }
    
    public virtual StateTransition GoRight {
        get {
            return this._GoRight;
        }
        set {
            _GoRight = value;
        }
    }
    
    public override string Name {
        get {
            return "MoveLeft";
        }
    }
    
    private void StopTransition() {
        this.Transition(this.Stop);
    }
    
    private void GoRightTransition() {
        this.Transition(this.GoRight);
    }
}

public class MoveRight : Invert.StateMachine.State {
    
    private StateTransition _GoLeft;
    
    private StateTransition _Stop;
    
    public virtual StateTransition GoLeft {
        get {
            return this._GoLeft;
        }
        set {
            _GoLeft = value;
        }
    }
    
    public virtual StateTransition Stop {
        get {
            return this._Stop;
        }
        set {
            _Stop = value;
        }
    }
    
    public override string Name {
        get {
            return "MoveRight";
        }
    }
    
    private void GoLeftTransition() {
        this.Transition(this.GoLeft);
    }
    
    private void StopTransition() {
        this.Transition(this.Stop);
    }
}
