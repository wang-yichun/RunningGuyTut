using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[DiagramInfoAttribute("RunningGuyGame")]
public class CharacterViewModelBase : ViewModel {
    
    private IDisposable _ShouldMoveLeftDisposable;
    
    private IDisposable _ShouldMoveRightDisposable;
    
    private IDisposable _ShouldStopDisposable;
    
    public P<MovementIntention> _MovementIntentionProperty;
    
    public CharacterMovementStateMachine _MovementStateProperty;
    
    public P<Boolean> _ShouldMoveLeftProperty;
    
    public P<Boolean> _ShouldMoveRightProperty;
    
    public P<Boolean> _ShouldStopProperty;
    
    public CharacterViewModelBase(CharacterControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public CharacterViewModelBase() : 
            base() {
    }
    
    public override void Bind() {
        base.Bind();
        _MovementIntentionProperty = new P<MovementIntention>(this, "MovementIntention");
        _MovementStateProperty = new CharacterMovementStateMachine(this, "MovementState");
        _ShouldMoveLeftProperty = new P<Boolean>(this, "ShouldMoveLeft");
        _ShouldMoveRightProperty = new P<Boolean>(this, "ShouldMoveRight");
        _ShouldStopProperty = new P<Boolean>(this, "ShouldStop");
        this.ResetShouldMoveLeft();
        this.ResetShouldMoveRight();
        this.ResetShouldStop();
        this._MovementStateProperty.GoLeft.AddComputer(_ShouldMoveLeftProperty);
        this._MovementStateProperty.GoRight.AddComputer(_ShouldMoveRightProperty);
        this._MovementStateProperty.Stop.AddComputer(_ShouldStopProperty);
    }
    
    public virtual void ResetShouldMoveLeft() {
        if (_ShouldMoveLeftDisposable != null) _ShouldMoveLeftDisposable.Dispose();
        _ShouldMoveLeftDisposable = _ShouldMoveLeftProperty.ToComputed( ComputeShouldMoveLeft, this.GetShouldMoveLeftDependents().ToArray() ).DisposeWith(this);
    }
    
    public virtual void ResetShouldMoveRight() {
        if (_ShouldMoveRightDisposable != null) _ShouldMoveRightDisposable.Dispose();
        _ShouldMoveRightDisposable = _ShouldMoveRightProperty.ToComputed( ComputeShouldMoveRight, this.GetShouldMoveRightDependents().ToArray() ).DisposeWith(this);
    }
    
    public virtual void ResetShouldStop() {
        if (_ShouldStopDisposable != null) _ShouldStopDisposable.Dispose();
        _ShouldStopDisposable = _ShouldStopProperty.ToComputed( ComputeShouldStop, this.GetShouldStopDependents().ToArray() ).DisposeWith(this);
    }
    
    public virtual Boolean ComputeShouldMoveLeft() {
        return default(Boolean);
    }
    
    public virtual IEnumerable<IObservableProperty> GetShouldMoveLeftDependents() {
        yield return _MovementIntentionProperty;
        yield break;
    }
    
    public virtual Boolean ComputeShouldMoveRight() {
        return default(Boolean);
    }
    
    public virtual IEnumerable<IObservableProperty> GetShouldMoveRightDependents() {
        yield return _MovementIntentionProperty;
        yield break;
    }
    
    public virtual Boolean ComputeShouldStop() {
        return default(Boolean);
    }
    
    public virtual IEnumerable<IObservableProperty> GetShouldStopDependents() {
        yield return _MovementIntentionProperty;
        yield break;
    }
}

public partial class CharacterViewModel : CharacterViewModelBase {
    
    public CharacterViewModel(CharacterControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public CharacterViewModel() : 
            base() {
    }
    
    public virtual P<MovementIntention> MovementIntentionProperty {
        get {
            return this._MovementIntentionProperty;
        }
    }
    
    public virtual MovementIntention MovementIntention {
        get {
            return _MovementIntentionProperty.Value;
        }
        set {
            _MovementIntentionProperty.Value = value;
        }
    }
    
    public virtual CharacterMovementStateMachine MovementStateProperty {
        get {
            return this._MovementStateProperty;
        }
    }
    
    public virtual Invert.StateMachine.State MovementState {
        get {
            return _MovementStateProperty.Value;
        }
        set {
            _MovementStateProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> ShouldMoveLeftProperty {
        get {
            return this._ShouldMoveLeftProperty;
        }
    }
    
    public virtual Boolean ShouldMoveLeft {
        get {
            return _ShouldMoveLeftProperty.Value;
        }
        set {
            _ShouldMoveLeftProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> ShouldMoveRightProperty {
        get {
            return this._ShouldMoveRightProperty;
        }
    }
    
    public virtual Boolean ShouldMoveRight {
        get {
            return _ShouldMoveRightProperty.Value;
        }
        set {
            _ShouldMoveRightProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> ShouldStopProperty {
        get {
            return this._ShouldStopProperty;
        }
    }
    
    public virtual Boolean ShouldStop {
        get {
            return _ShouldStopProperty.Value;
        }
        set {
            _ShouldStopProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var character = controller as CharacterControllerBase;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("MovementIntention", (int)this.MovementIntention);
        stream.SerializeString("MovementState", this.MovementState.Name);;
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.MovementIntention = (MovementIntention)stream.DeserializeInt("MovementIntention");
        this._MovementStateProperty.SetState(stream.DeserializeString("MovementState"));
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_MovementIntentionProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_MovementStateProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_ShouldMoveLeftProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ShouldMoveRightProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ShouldStopProperty, false, false, false, true));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
}

public enum MovementIntention {
    
    Left,
    
    Right,
    
    Stop,
}
