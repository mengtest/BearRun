using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FinalShowUIController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        UIBoard board = GetView<UIBoard>();
        board.Hide();
        UIFinalScore final = GetView<UIFinalScore>();
        final.Show();
        //2.更新总exp
        gm.Exp += board.Coin + board.Distance * (board.GoalCount + 1);
        //3.更新分数
        int boardScore = (board.Coin + board.Distance * (board.GoalCount + 1)) * 5;
        //1.更新UI
        final.UpdateUI(board.Distance, board.Coin, boardScore, board.GoalCount, gm.Exp, gm.Grade);
        
        UIDead dead = GetView<UIDead>();
        dead.Hide();

        // 让model层向服务器提交玩家的本局信息
        if (gm.IsLogin == true)
        {
            gm.PostUserRequest(board.Coin, boardScore, gm.Exp);
        }
        else
        {
            gm.Coin += board.Coin;
            gm.Score += boardScore;
        }
    }
}