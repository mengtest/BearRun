using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class PauseGameController : Controller
{
    public override void Execute(object data)
    {
        PauseArgs e = data as PauseArgs;
        GameModel gm = GetModel<GameModel>();
        gm.IsPause = true;
        UIPause pause = GetView<UIPause>();
        pause.Show();
        pause.txtCoin.text = e.coin.ToString();
        pause.txtDis.text = e.distance.ToString();
        pause.txtScore.text = e.score.ToString();
    }
}