using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyTools : View
{
    GameModel gm;

    public Text txtGizmoMangent;
    public Text txtGizmoMultiply;
    public Text txtGizmoInvincible;
    public Text txtMoney;
    public SkinnedMeshRenderer skm;
    public MeshRenderer render;
  

    public override string Name
    {
        get
        {
            return Consts.V_BuyTools;
        }
    }

    public override void HandleEvent(string name, object data)
    {
    }

    private void Awake()
    {
        gm = GetModel<GameModel>();
        InitUI();
    }

    public void InitUI()
    {
        txtMoney.text = gm.Coin.ToString();
        ShowOrHide(gm.Magnet, txtGizmoMangent);
        ShowOrHide(gm.Multiply, txtGizmoMultiply);
        ShowOrHide(gm.Invincible, txtGizmoInvincible);

        gm = GetModel<GameModel>();
        skm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        render.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
    }

    void ShowOrHide(int i, Text txt)
    {
        if (i > 0)
        {           
            txt.transform.parent.gameObject.SetActive(true);
            txt.text = i.ToString();
        }
        else
        {
            txt.transform.parent.gameObject.SetActive(false);
            //txt.text = i.ToString();
        }

    }

    public void OnReturnClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        if (gm.lastIndex == 4)
            gm.lastIndex = 1;
        Game.Instance.LoadLevel(gm.lastIndex);
    }


    public void OnMagnetClick(int i= 100)
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        BuyToolsArgs e = new BuyToolsArgs
        {
            coin = i,
            itemKind = ItemKind.ItemMagnet
        };
        SendEvent(Consts.E_BuyTools, e);
    }

    public void OnInvincibleClick(int i = 200)
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        BuyToolsArgs e = new BuyToolsArgs
        {
            coin = i,
            itemKind = ItemKind.ItemInvincible
        };
        SendEvent(Consts.E_BuyTools, e);
    }
    public void OnMulityCilck(int i = 200)
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        BuyToolsArgs e = new BuyToolsArgs
        {
            coin = i,
            itemKind = ItemKind.ItemMultiply
        };
        SendEvent(Consts.E_BuyTools, e);
    }

    public void OnRandomClick(int i = 300)
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        int t = Random.Range(0, 3);
        switch (t)
        {
            case 0:
                OnMagnetClick(i);
                break;
            case 1:
                OnInvincibleClick(i);
                break;
            case 2:
                OnMulityCilck(i);
                break;
        }

    }

    public void OnPlayClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(4);
    }

}
