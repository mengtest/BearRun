using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class EnterScenesController : Controller
{
    public override void Execute(object data)
    {
        ScenesArgs e = data as ScenesArgs;
        switch (e.scenesIndex)
        {
            case 1:
                RegisterView(GameObject.Find("UIMainMenu").GetComponent<UIMainMenu>());
                Game.Instance.sound.PlayBG("Bgm_JieMian");
                break;
            case 2:
                RegisterView(GameObject.Find("UIShop").GetComponent<UIShop>());
                Game.Instance.sound.PlayBG("Bgm_JieMian");
                break;
            case 3:
                RegisterView(GameObject.Find("Canvas").transform.Find("UIBuyTools").GetComponent<UIBuyTools>());
                Game.Instance.sound.PlayBG("Bgm_JieMian");
                break;
            case 4:
                Game.Instance.sound.PlayBG("Bgm_ZhanDou");
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerMove>());
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerAnim>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIPause").GetComponent<UIPause>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIResume").GetComponent<UIResume>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIDead").GetComponent<UIDead>()); 
                RegisterView(GameObject.Find("Canvas").transform.Find("UIFinalScore").GetComponent<UIFinalScore>());
                GameModel gm = GetModel<GameModel>();
                gm.IsPause = false;
                gm.IsPlay = true;
                break;
            case 5:
                RegisterView(GameObject.Find("UILoginRegister").GetComponent<UILoginRegister>());
                Game.Instance.sound.PlayBG("Bgm_JieMian");
                break;
        }
    }
}