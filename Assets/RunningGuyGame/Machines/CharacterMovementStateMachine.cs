using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;


public class CharacterMovementStateMachine : CharacterMovementStateMachineBase {
    
    public CharacterMovementStateMachine(ViewModel vm, string propertyName) : 
            base(vm, propertyName) {
    }
}
