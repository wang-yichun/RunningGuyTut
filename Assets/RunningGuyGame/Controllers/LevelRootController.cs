using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniRx;
using UnityEngine;


public class LevelRootController : LevelRootControllerBase {
    
    public override void InitializeLevelRoot(LevelRootViewModel levelRoot)
    {
        levelRoot._CoinsProperty
            .Where(c => c.Action == NotifyCollectionChangedAction.Add)
            .Select(c => c.NewItems[0] as CoinViewModel)
            .Subscribe(coin => CoinAdded(levelRoot, coin));
    }

    private void CoinAdded(LevelRootViewModel levelRoot, CoinViewModel coin) 
    {
        coin.PickUp.Subscribe(_ =>
        {
            levelRoot.Coins.Remove(coin);
        });
    }

}
