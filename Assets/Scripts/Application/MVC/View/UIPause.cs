using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : View
{
    public Text txtDis;
    public Text txtCoin;
    public Text txtScore;
    public SkinnedMeshRenderer skm;
    public MeshRenderer render;
    GameModel gm;

    public override string Name
    {
        get
        {
            return Consts.V_Pause;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateUI();
    }

    public override void HandleEvent(string name, object data)
    {
    }

    public void OnResumeClick()
    {
        Hide();
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        SendEvent(Consts.E_ResumeGame);
    }

    public void OnHomeClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(1);
    }

    private void Awake()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        gm = GetModel<GameModel>();
        skm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        render.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
    }
}
