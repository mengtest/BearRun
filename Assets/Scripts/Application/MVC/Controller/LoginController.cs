using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SprotoType;

class LoginController : Controller
{
    public override void Execute(object data)
    {
        LoginArgs e = data as LoginArgs;
        GameModel gm = GetModel<GameModel>();
        login.response obj = gm.LoginRequest(e);
        UILoginRegister loginRegister = GetView<UILoginRegister>();
        loginRegister.ShowError(obj.error);
    }
}