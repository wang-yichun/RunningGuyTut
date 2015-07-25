using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


public class CharacterController : CharacterControllerBase {
    
    public override void InitializeCharacter(CharacterViewModel character) {
        character.JumpStateProperty.Subscribe(state => JumpStateChanged(character, state));
    }

    private void JumpStateChanged(CharacterViewModel character, Invert.StateMachine.State state)
    {
        if (state is NoJump)
        {
            character.JumpsPerformed = 0;
        }
        else if (state is DoJump)
        {
            character.JumpsPerformed++;
            character.JumpLocked = true;

            Observable
                .Timer(TimeSpan.FromMilliseconds(100))
                .Subscribe(l =>
                {
                    character.JumpLocked = false;
                });

        }
    }
    public override void PickUpCoin(CharacterViewModel character)
    {
        base.PickUpCoin(character);
        character.CoinsCollected++;
    }
    public override void Hit(CharacterViewModel character)
    {
        if (character.IsInvulnarable) return;
        character.Lives--;

        if (character.Lives <= 0)
        {
            character.IsAlive = false;
        }
        else
        {
            character.IsInvulnarable = true;
            Observable.Timer(TimeSpan.FromMilliseconds(1500))
                .Subscribe(l =>
                {
                    character.IsInvulnarable = false;
                });
        }
    }
}
