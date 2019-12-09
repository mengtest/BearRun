using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Execute(object data)
    {
        //注册所有的controller    
        RegisterController(Consts.E_EnterScenes, typeof(EnterScenesController));
        RegisterController(Consts.E_ExitScenes, typeof(ExitScenesController));
        RegisterController(Consts.E_EndGame, typeof(EndGameController));
        RegisterController(Consts.E_PauseGame, typeof(PauseGameController));
        RegisterController(Consts.E_ResumeGame, typeof(ResumeGameController));
        RegisterController(Consts.E_HitItem, typeof(HitItemController));
        RegisterController(Consts.E_FinalShowUI, typeof(FinalShowUIController));
        RegisterController(Consts.E_BriberyClick, typeof(BriberyClickController));
        RegisterController(Consts.E_ContinueGame, typeof(ContinueGameController));
        RegisterController(Consts.E_BuyTools, typeof(BuyToolsController));
        RegisterController(Consts.E_Login, typeof(LoginController));
        RegisterController(Consts.E_Register, typeof(RegisterController));
        RegisterController(Consts.E_ShowPerson, typeof(ShowPersonController));
        RegisterController(Consts.E_ShowRank, typeof(ShowRankController));
        //装备足球
        RegisterController(Consts.E_EquipFootBall, typeof(EquipFootBallController));
        //买足球
        RegisterController(Consts.E_BuyFootBall, typeof(BuyFootBallController));
        //买衣服
        RegisterController(Consts.E_BuyCloth, typeof(BuyClothController));
        //装备衣服
        RegisterController(Consts.E_EquipCloth, typeof(EquipClothController));

        //注册model
        RegisterModel(new GameModel());

        //初始化
        GameModel gm = GetModel<GameModel>();
        gm.Init();
    }
}