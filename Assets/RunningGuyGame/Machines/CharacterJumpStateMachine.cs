using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;


public class CharacterJumpStateMachine : CharacterJumpStateMachineBase {
    
    public CharacterJumpStateMachine(ViewModel vm, string propertyName) : 
            base(vm, propertyName) {
    }
}
