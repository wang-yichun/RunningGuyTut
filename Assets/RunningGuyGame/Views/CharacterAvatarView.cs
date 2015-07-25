using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class CharacterAvatarView { 

    /// Subscribes to the property and is notified anytime the value changes.
    public override void IsAliveChanged(Boolean value)
    {
        var smoothFollow = Camera.main.GetComponent<SmoothFollow>();
        var collidder = GetComponent<Collider2D>();

        if (!value)
        {
            art.transform.eulerAngles = new Vector3(0, 0, 90);
            collidder.enabled = false;
            smoothFollow.target = null;
        }
        else
        {
            art.transform.eulerAngles = new Vector3(0, 0, 0);
            collidder.enabled = true;
            smoothFollow.target = transform.Find("CameraPoin");
        }
    }

    /// Subscribes to the property and is notified anytime the value changes.
    public override void IsInvulnarableChanged(Boolean value)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>().ToList();

        if (value)
        {
            Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Subscribe(l =>
                {
                    if (Character.IsInvulnarable)
                    {
                        sprites.ForEach(s => s.enabled = l%2 == 0);
                    }
                })
                .DisposeWhenChanged(Character.IsInvulnarableProperty);
        }
        else
        {
            sprites.ForEach(s => s.enabled = true);
        }
    }
 

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

    public override void Bind()
    {
        base.Bind();
        this.BindViewTrigger2DWith<CoinView>(CollisionEventType.Enter, coinview =>
        {
            coinview.ExecutePickUp();
            ExecutePickUpCoin();
        });

        this.BindComponentCollision2DWith<Spikes>(CollisionEventType.Enter, _=> ExecuteHit());
        this.BindComponentCollision2DWith<WinLocation>(CollisionEventType.Enter, _ => ExecuteFinishReached());
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
