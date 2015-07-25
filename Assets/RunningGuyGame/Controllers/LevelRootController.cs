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

        levelRoot.PlayerProperty
            .Where(player => player != null)
            .Subscribe(player => NewPlayer(levelRoot, player));
    }

    private void NewPlayer(LevelRootViewModel levelRoot, CharacterViewModel player)
    {
        player.IsAliveProperty
            .Where(isAlive => isAlive == false)
            .Subscribe(_ => this.ExecuteCommand(levelRoot.LoseGame))
            .DisposeWhenChanged(levelRoot.PlayerProperty);

        player.FinishReached.Subscribe(_ => this.ExecuteCommand(levelRoot.WinGame));
    }

    private void CoinAdded(LevelRootViewModel levelRoot, CoinViewModel coin) 
    {
        coin.PickUp.Subscribe(_ =>
        {
            levelRoot.Coins.Remove(coin);
        });
    }

    public override void Restart(LevelRootViewModel levelRoot)
    {
        base.Restart(levelRoot);
        GameManager.TransitionLevel<LevelSceneManager>(scenemanager => { }, "Assets");
    }
}
