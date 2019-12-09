using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SprotoType;

class ShowRankController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        UIMainMenu uiMainMenu = GetView<UIMainMenu>();
        List<info> rank = gm.GetRankRequest();
        if (gm.Index > rank.Count - 1)
        {
            gm.Times++;
            if ((gm.Times & 1) != 0)
            {
                gm.Index -= 5;
            }
            else
            {
                gm.Index = -1;
            }
        }
        uiMainMenu.ShowRank(rank, gm.Index);
    }
}