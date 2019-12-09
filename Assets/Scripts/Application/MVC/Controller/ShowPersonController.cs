using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SprotoType;

class ShowPersonController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        UILoginRegister loginRegister = GetView<UILoginRegister>();
        getUser.response obj = gm.GetUserRequest();
        if (obj.status == true)
        {
            loginRegister.ShowPerson(gm.Username, gm.Coin, gm.Score, gm.Exp, gm.Rank);
        }
        else
        {
            loginRegister.OnReturnClicK();
        }
    }
}