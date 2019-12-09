using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class BuyClothController : Controller
{
    public override void Execute(object data)
    {
        BuyClothArgs e = data as BuyClothArgs;
        GameModel gm = GetModel<GameModel>();
        UIShop shop = GetView<UIShop>();
        if (gm.GetMoney(e.coin))
        {
            //1.更新数据
            gm.BuyCloth.Add(e.id);
            foreach(var a in gm.BuyCloth)
            {
                Debug.Log("买了" + a.SkinID + "****" + a.ClothID);
            }
            //更新信息
            shop.UpdateUI();
        }
    }
}