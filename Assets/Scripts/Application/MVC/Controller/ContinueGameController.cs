using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ContinueGameController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();


        UIBoard board = GetView<UIBoard>();
        if(board.Times < 0.1f)
        {
            board.Times += 20;
        }

        gm.IsPause = false;
        gm.IsPlay = true;
    }
}