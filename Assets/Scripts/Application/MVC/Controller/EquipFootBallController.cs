using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EquipFootBallController : Controller
{
    public override void Execute(object data)
    {
        UIShop shop = GetView<UIShop>();
        BuyFootBallArgs e = data as BuyFootBallArgs;
        GameModel gm = GetModel<GameModel>();
        gm.TakeOnFootball = e.noeIndex;

        shop.UpdateFootBallBuyButton(e.noeIndex);
        //更新Gizmo
        shop.UpdateFootBallGizmo();
    }
}