using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class CharacterAvatarView { 

    /// Subscribes to the state machine property and executes a method for each state.
    public override void JumpStateChanged(Invert.StateMachine.State value) {
        base.JumpStateChanged(value);
    }
    
    public override void OnNoJump() {
        base.OnNoJump();
    }
    
    public override void OnDoJump() {
        base.OnDoJump();
        var rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        rigidbody2d.AddForce(transform.up * 8, ForceMode2D.Impulse);
    }
    
    public override void OnInTheAir() {
        base.OnInTheAir();
    }

    /// Subscribes to the property and is notified anytime the value changes.
    public override void IsNotOnTheGroundChanged(Boolean value) {
        base.IsNotOnTheGroundChanged(value);
        animator.SetBool("IsInTheAir", value);
    }


    Vector3 speed = new Vector3(3, 0, 0);
    protected Animator animator;
    protected Transform art;

    public override void Awake()
    {
        base.Awake();
        art = transform.FindChild("CharacterArt");
        animator = art.GetComponent<Animator>();
    }

    /// Subscribes to the state machine property and executes a method for each state.
    public override void MovementStateChanged(Invert.StateMachine.State value) {
        base.MovementStateChanged(value);
    }
    
    public override void OnIdle() {
        base.OnIdle();

        animator.SetInteger("State", 0);
    }
    
    public override void OnMoveLeft() {
        Observable.EveryFixedUpdate()
            .Subscribe(l => transform.Translate(-speed * Time.deltaTime))
            .DisposeWhenChanged(Character.MovementStateProperty);

        animator.SetInteger("State", 1);
        art.localEulerAngles = new Vector3(0, 180, 0);
    }
    
    public override void OnMoveRight() {
        Observable.EveryFixedUpdate()
            .Subscribe(l => transform.Translate(speed * Time.deltaTime))
            .DisposeWhenChanged(Character.MovementStateProperty);

        animator.SetInteger("State", 1);
        art.localEulerAngles = new Vector3(0, 0, 0);
    }

    protected override MovementIntention CalculateMovementIntention()
    {
        if (Input.GetKey(KeyCode.A)) return MovementIntention.Left;
        if (Input.GetKey(KeyCode.D)) return MovementIntention.Right;
        return MovementIntention.Stop;
    }

    protected override JumpIntention CalculateJumpIntention()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return JumpIntention.Jump;
        }
        return JumpIntention.Idle;
    }

    protected override bool CalculateIsOnTheGround()
    {
        return Physics2D.Raycast(transform.position, -transform.up.normalized, 0.05f, LayerMask.GetMask(new[] { "Floor" }));
    }
}
