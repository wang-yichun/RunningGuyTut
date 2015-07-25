using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;


public class GameFlowStateMachine : GameFlowStateMachineBase {
    
    public GameFlowStateMachine(ViewModel vm, string propertyName) : 
            base(vm, propertyName) {
    }
}
