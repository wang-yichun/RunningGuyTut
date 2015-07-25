using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class LevelRootView
{

    public TextMesh scoreText;

    /// This binding will add or remove views based on an element/viewmodel collection.
    public override ViewBase CreateCoinsView(CoinViewModel item) {
        return base.CreateCoinsView(item);
    }
    
    /// This binding will add or remove views based on an element/viewmodel collection.
    public override void CoinsAdded(ViewBase item) {
        base.CoinsAdded(item);
    }
    
    /// This binding will add or remove views based on an element/viewmodel collection.
    public override void CoinsRemoved(ViewBase item) {
        base.CoinsRemoved(item);
        Destroy(item.gameObject);
    }

    /// Subscribes to the property and is notified anytime the value changes.
    public override void ScoreChanged(Int32 value)
    {
        base.ScoreChanged(value);
        scoreText.text = "Score " + value;
    }
 
}
