using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SprotoType;

class RegisterController : Controller
{
    public override void Execute(object data)
    {
        RegisterArgs e = data as RegisterArgs;
        GameModel gm = GetModel<GameModel>();
        register.response obj = gm.RegisterRequest(e);
        UILoginRegister loginRegister = GetView<UILoginRegister>();
        loginRegister.ShowError(obj.error);
    }
}