using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class LevelRootViewModel
{
    public override int ComputeScore()
    {
        if (Player == null) return 0;
        return Player.CoinsCollected;
    }
}
