using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;
using UnityEngine;
using UniRx;


public partial class CharacterSounds
{

    public AudioClip JumpSound;
    public AudioClip FootstepSound;
    public AudioClip LandedSound;
    public AudioClip CoinSound;
    private AudioSource _audioSource;

    public override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Bind(ViewBase view)
    {
        base.Bind(view);
        view.BindProperty(Character.JumpStateProperty, JumpStateChanged);
        view.BindProperty(Character.MovementStateProperty, MovementStateChanged);
        view.BindCommandExecuted(Character.PickUpCoin, CoinPickedUp);
        Character.IsOnTheGroundProperty
            .DistinctUntilChanged()
            .Where(value => value)
            .Subscribe(value => Landed())
            .DisposeWith(view);
    }

    private void Landed()
    {
        _audioSource.PlayOneShot(LandedSound);
    }

    private void CoinPickedUp()
    {
        _audioSource.PlayOneShot(CoinSound);
    }

    private void MovementStateChanged(State state)
    {
        if (state is MoveLeft || state is MoveRight)
        {
            Observable.Interval(TimeSpan.FromMilliseconds(240))
                .Subscribe(_ =>
                {
                    if (!(Character.JumpState is InTheAir))
                    {
                        _audioSource.PlayOneShot(FootstepSound, 0.4f);
                    }
                }).DisposeWhenChanged(Character.MovementStateProperty);
        }
    }

    private void JumpStateChanged(State state)
    {
        if (state is DoJump && Character.IsOnTheGround == true)
        {
            _audioSource.PlayOneShot(JumpSound);
        }
    }
}
