using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BuyToolsController : Controller
{
    public override void Execute(object data)
    {
        BuyToolsArgs e = data as BuyToolsArgs;
        UIBuyTools tool = GetView<UIBuyTools>();
        GameModel gm = GetModel<GameModel>();
        switch (e.itemKind)
        {
            case ItemKind.ItemMagnet:
                if (gm.GetMoney(e.coin))
                {
                    gm.Magnet++;

                }
                break;
            case ItemKind.ItemMultiply:
                if (gm.GetMoney(e.coin))
                {
                    gm.Multiply++;

                }
                break;
            case ItemKind.ItemInvincible:
                if (gm.GetMoney(e.coin))
                {
                    gm.Invincible++;

                }
                break;
            default:
                break;
        }
        tool.InitUI();
    }
}