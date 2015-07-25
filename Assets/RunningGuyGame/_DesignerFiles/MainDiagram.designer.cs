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
    
    private IDisposable _ShouldJumpDisposable;
    
    private IDisposable _IsNotOnTheGroundDisposable;
    
    public P<MovementIntention> _MovementIntentionProperty;
    
    public CharacterMovementStateMachine _MovementStateProperty;
    
    public P<JumpIntention> _JumpIntentionProperty;
    
    public CharacterJumpStateMachine _JumpStateProperty;
    
    public P<Boolean> _IsOnTheGroundProperty;
    
    public P<Boolean> _JumpLockedProperty;
    
    public P<Int32> _JumpsPerformedProperty;
    
    public P<Int32> _CoinsCollectedProperty;
    
    public P<Boolean> _ShouldMoveLeftProperty;
    
    public P<Boolean> _ShouldMoveRightProperty;
    
    public P<Boolean> _ShouldStopProperty;
    
    public P<Boolean> _ShouldJumpProperty;
    
    public P<Boolean> _IsNotOnTheGroundProperty;
    
    protected CommandWithSender<CharacterViewModel> _PickUpCoin;
    
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
        _JumpIntentionProperty = new P<JumpIntention>(this, "JumpIntention");
        _JumpStateProperty = new CharacterJumpStateMachine(this, "JumpState");
        _IsOnTheGroundProperty = new P<Boolean>(this, "IsOnTheGround");
        _JumpLockedProperty = new P<Boolean>(this, "JumpLocked");
        _JumpsPerformedProperty = new P<Int32>(this, "JumpsPerformed");
        _CoinsCollectedProperty = new P<Int32>(this, "CoinsCollected");
        _ShouldMoveLeftProperty = new P<Boolean>(this, "ShouldMoveLeft");
        _ShouldMoveRightProperty = new P<Boolean>(this, "ShouldMoveRight");
        _ShouldStopProperty = new P<Boolean>(this, "ShouldStop");
        _ShouldJumpProperty = new P<Boolean>(this, "ShouldJump");
        _IsNotOnTheGroundProperty = new P<Boolean>(this, "IsNotOnTheGround");
        this.ResetShouldMoveLeft();
        this.ResetShouldMoveRight();
        this.ResetShouldStop();
        this.ResetShouldJump();
        this.ResetIsNotOnTheGround();
        this._MovementStateProperty.GoLeft.AddComputer(_ShouldMoveLeftProperty);
        this._MovementStateProperty.GoRight.AddComputer(_ShouldMoveRightProperty);
        this._MovementStateProperty.Stop.AddComputer(_ShouldStopProperty);
        this._JumpStateProperty.JumpExpected.AddComputer(_ShouldJumpProperty);
        this._JumpStateProperty.LeftGround.AddComputer(_IsNotOnTheGroundProperty);
        this._IsOnTheGroundProperty.Subscribe(_JumpStateProperty.Landed);
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
    
    public virtual void ResetShouldJump() {
        if (_ShouldJumpDisposable != null) _ShouldJumpDisposable.Dispose();
        _ShouldJumpDisposable = _ShouldJumpProperty.ToComputed( ComputeShouldJump, this.GetShouldJumpDependents().ToArray() ).DisposeWith(this);
    }
    
    public virtual void ResetIsNotOnTheGround() {
        if (_IsNotOnTheGroundDisposable != null) _IsNotOnTheGroundDisposable.Dispose();
        _IsNotOnTheGroundDisposable = _IsNotOnTheGroundProperty.ToComputed( ComputeIsNotOnTheGround, this.GetIsNotOnTheGroundDependents().ToArray() ).DisposeWith(this);
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
    
    public virtual Boolean ComputeShouldJump() {
        return default(Boolean);
    }
    
    public virtual IEnumerable<IObservableProperty> GetShouldJumpDependents() {
        yield return _JumpIntentionProperty;
        yield break;
    }
    
    public virtual Boolean ComputeIsNotOnTheGround() {
        return default(Boolean);
    }
    
    public virtual IEnumerable<IObservableProperty> GetIsNotOnTheGroundDependents() {
        yield return _IsOnTheGroundProperty;
        yield break;
    }
}

public partial class CharacterViewModel : CharacterViewModelBase {
    
    private LevelRootViewModel _ParentLevelRoot;
    
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
    
    public virtual P<JumpIntention> JumpIntentionProperty {
        get {
            return this._JumpIntentionProperty;
        }
    }
    
    public virtual JumpIntention JumpIntention {
        get {
            return _JumpIntentionProperty.Value;
        }
        set {
            _JumpIntentionProperty.Value = value;
        }
    }
    
    public virtual CharacterJumpStateMachine JumpStateProperty {
        get {
            return this._JumpStateProperty;
        }
    }
    
    public virtual Invert.StateMachine.State JumpState {
        get {
            return _JumpStateProperty.Value;
        }
        set {
            _JumpStateProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> IsOnTheGroundProperty {
        get {
            return this._IsOnTheGroundProperty;
        }
    }
    
    public virtual Boolean IsOnTheGround {
        get {
            return _IsOnTheGroundProperty.Value;
        }
        set {
            _IsOnTheGroundProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> JumpLockedProperty {
        get {
            return this._JumpLockedProperty;
        }
    }
    
    public virtual Boolean JumpLocked {
        get {
            return _JumpLockedProperty.Value;
        }
        set {
            _JumpLockedProperty.Value = value;
        }
    }
    
    public virtual P<Int32> JumpsPerformedProperty {
        get {
            return this._JumpsPerformedProperty;
        }
    }
    
    public virtual Int32 JumpsPerformed {
        get {
            return _JumpsPerformedProperty.Value;
        }
        set {
            _JumpsPerformedProperty.Value = value;
        }
    }
    
    public virtual P<Int32> CoinsCollectedProperty {
        get {
            return this._CoinsCollectedProperty;
        }
    }
    
    public virtual Int32 CoinsCollected {
        get {
            return _CoinsCollectedProperty.Value;
        }
        set {
            _CoinsCollectedProperty.Value = value;
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
    
    public virtual P<Boolean> ShouldJumpProperty {
        get {
            return this._ShouldJumpProperty;
        }
    }
    
    public virtual Boolean ShouldJump {
        get {
            return _ShouldJumpProperty.Value;
        }
        set {
            _ShouldJumpProperty.Value = value;
        }
    }
    
    public virtual P<Boolean> IsNotOnTheGroundProperty {
        get {
            return this._IsNotOnTheGroundProperty;
        }
    }
    
    public virtual Boolean IsNotOnTheGround {
        get {
            return _IsNotOnTheGroundProperty.Value;
        }
        set {
            _IsNotOnTheGroundProperty.Value = value;
        }
    }
    
    public virtual CommandWithSender<CharacterViewModel> PickUpCoin {
        get {
            return _PickUpCoin;
        }
        set {
            _PickUpCoin = value;
        }
    }
    
    public virtual LevelRootViewModel ParentLevelRoot {
        get {
            return this._ParentLevelRoot;
        }
        set {
            _ParentLevelRoot = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var character = controller as CharacterControllerBase;
        this.PickUpCoin = new CommandWithSender<CharacterViewModel>(this, character.PickUpCoin);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("MovementIntention", (int)this.MovementIntention);
        stream.SerializeString("MovementState", this.MovementState.Name);;
		stream.SerializeInt("JumpIntention", (int)this.JumpIntention);
        stream.SerializeString("JumpState", this.JumpState.Name);;
        stream.SerializeBool("IsOnTheGround", this.IsOnTheGround);
        stream.SerializeBool("JumpLocked", this.JumpLocked);
        stream.SerializeInt("JumpsPerformed", this.JumpsPerformed);
        stream.SerializeInt("CoinsCollected", this.CoinsCollected);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.MovementIntention = (MovementIntention)stream.DeserializeInt("MovementIntention");
        this._MovementStateProperty.SetState(stream.DeserializeString("MovementState"));
		this.JumpIntention = (JumpIntention)stream.DeserializeInt("JumpIntention");
        this._JumpStateProperty.SetState(stream.DeserializeString("JumpState"));
        		this.IsOnTheGround = stream.DeserializeBool("IsOnTheGround");;
        		this.JumpLocked = stream.DeserializeBool("JumpLocked");;
        		this.JumpsPerformed = stream.DeserializeInt("JumpsPerformed");;
        		this.CoinsCollected = stream.DeserializeInt("CoinsCollected");;
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_MovementIntentionProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_MovementStateProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_JumpIntentionProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_JumpStateProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_IsOnTheGroundProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_JumpLockedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_JumpsPerformedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_CoinsCollectedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_ShouldMoveLeftProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ShouldMoveRightProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ShouldStopProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ShouldJumpProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_IsNotOnTheGroundProperty, false, false, false, true));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("PickUpCoin", PickUpCoin) { ParameterType = typeof(void) });
    }
}

[DiagramInfoAttribute("RunningGuyGame")]
public class CoinViewModelBase : ViewModel {
    
    protected CommandWithSender<CoinViewModel> _PickUp;
    
    public CoinViewModelBase(CoinControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public CoinViewModelBase() : 
            base() {
    }
    
    public override void Bind() {
        base.Bind();
    }
}

public partial class CoinViewModel : CoinViewModelBase {
    
    private LevelRootViewModel _ParentLevelRoot;
    
    public CoinViewModel(CoinControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public CoinViewModel() : 
            base() {
    }
    
    public virtual CommandWithSender<CoinViewModel> PickUp {
        get {
            return _PickUp;
        }
        set {
            _PickUp = value;
        }
    }
    
    public virtual LevelRootViewModel ParentLevelRoot {
        get {
            return this._ParentLevelRoot;
        }
        set {
            _ParentLevelRoot = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var coin = controller as CoinControllerBase;
        this.PickUp = new CommandWithSender<CoinViewModel>(this, coin.PickUp);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("PickUp", PickUp) { ParameterType = typeof(void) });
    }
}

[DiagramInfoAttribute("RunningGuyGame")]
public class LevelRootViewModelBase : ViewModel {
    
    private IDisposable _ScoreDisposable;
    
    public P<CharacterViewModel> _PlayerProperty;
    
    public P<Int32> _ScoreProperty;
    
    public ModelCollection<CoinViewModel> _CoinsProperty;
    
    public LevelRootViewModelBase(LevelRootControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public LevelRootViewModelBase() : 
            base() {
    }
    
    public override void Bind() {
        base.Bind();
        _PlayerProperty = new P<CharacterViewModel>(this, "Player");
        _ScoreProperty = new P<Int32>(this, "Score");
        _CoinsProperty = new ModelCollection<CoinViewModel>(this, "Coins");
        _CoinsProperty.CollectionChanged += CoinsCollectionChanged;
        this.ResetScore();
        this.BindProperty(_PlayerProperty, p=> ResetScore());
    }
    
    public virtual void ResetScore() {
        if (_ScoreDisposable != null) _ScoreDisposable.Dispose();
        _ScoreDisposable = _ScoreProperty.ToComputed( ComputeScore, this.GetScoreDependents().ToArray() ).DisposeWith(this);
    }
    
    protected virtual void CoinsCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
    }
    
    public virtual Int32 ComputeScore() {
        return default(Int32);
    }
    
    public virtual IEnumerable<IObservableProperty> GetScoreDependents() {
        if (_PlayerProperty.Value != null) {
            yield return _PlayerProperty.Value._CoinsCollectedProperty;
        }
        yield break;
    }
}

public partial class LevelRootViewModel : LevelRootViewModelBase {
    
    public LevelRootViewModel(LevelRootControllerBase controller, bool initialize = true) : 
            base(controller, initialize) {
    }
    
    public LevelRootViewModel() : 
            base() {
    }
    
    public virtual P<CharacterViewModel> PlayerProperty {
        get {
            return this._PlayerProperty;
        }
    }
    
    public virtual CharacterViewModel Player {
        get {
            return _PlayerProperty.Value;
        }
        set {
            _PlayerProperty.Value = value;
            if (value != null) value.ParentLevelRoot = this;
        }
    }
    
    public virtual P<Int32> ScoreProperty {
        get {
            return this._ScoreProperty;
        }
    }
    
    public virtual Int32 Score {
        get {
            return _ScoreProperty.Value;
        }
        set {
            _ScoreProperty.Value = value;
        }
    }
    
    public virtual ModelCollection<CoinViewModel> Coins {
        get {
            return this._CoinsProperty;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var levelRoot = controller as LevelRootControllerBase;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		if (stream.DeepSerialize) stream.SerializeObject("Player", this.Player);
        if (stream.DeepSerialize) stream.SerializeArray("Coins", this.Coins);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		if (stream.DeepSerialize) this.Player = stream.DeserializeObject<CharacterViewModel>("Player");
if (stream.DeepSerialize) {
        this.Coins.Clear();
        this.Coins.AddRange(stream.DeserializeObjectArray<CoinViewModel>("Coins"));
}
    }
    
    public override void Unbind() {
        base.Unbind();
        _CoinsProperty.CollectionChanged -= CoinsCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_PlayerProperty, true, false, false));
        list.Add(new ViewModelPropertyInfo(_ScoreProperty, false, false, false, true));
        list.Add(new ViewModelPropertyInfo(_CoinsProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
    
    protected override void CoinsCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        foreach (var item in args.NewItems.OfType<CoinViewModel>()) item.ParentLevelRoot = this;;
    }
}

public enum MovementIntention {
    
    Left,
    
    Right,
    
    Stop,
}

public enum JumpIntention {
    
    Jump,
    
    Idle,
}
