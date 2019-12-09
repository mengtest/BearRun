using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BriberyClickController : Controller
{
    public override void Execute(object data)
    {
        CoinArgs e = data as CoinArgs;
        UIDead dead = GetView<UIDead>();
        GameModel gm = GetModel<GameModel>();
        //花钱
        //if（花钱成功）{
        //dead.briber ++;
        //}
        if (gm.GetMoney(e.coin))
        {
            dead.Hide();
            dead.BriberyTime ++;
            UIResume resume = GetView<UIResume>();
            resume.StartCount();
        }
    }
}