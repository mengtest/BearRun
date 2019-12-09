using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class BuyFootBallController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        UIShop shop = GetView<UIShop>();
        BuyFootBallArgs e = data as BuyFootBallArgs;
        if (gm.GetMoney(e.coin))
        {
            //把现在的id
            gm.BuyFootBall.Add(e.noeIndex);
            foreach (var a in gm.BuyFootBall)
            {
                Debug.Log("BuyFootBall买了" +a + "号");
            }
            //update
            shop.UpdateFootBallBuyButton(e.noeIndex);
            shop.UpdateFootBallGizmo();
            shop.UpdateUI();
        }
    }
}