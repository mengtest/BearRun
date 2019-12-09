using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDead : View
{
    //贿赂次数
    int m_BriberyTime = 1;
    public Text txtBribery;
    public override string Name
    {
        get
        {
            return Consts.V_Dead;
        }
    }

    public int BriberyTime
    {
        get
        {
            return m_BriberyTime;
        }

        set
        {
            m_BriberyTime = value;
        }
    }

    public override void HandleEvent(string name, object data)
    {
    }

    public void Hide()
    {
        gameObject.SetActive(false);

    }

    public void Show()
    {
        txtBribery.text = (500 * (BriberyTime )).ToString();
        gameObject.SetActive(true);
    }

    //鼠标点击拒绝
    public void OnCancleClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        SendEvent(Consts.E_FinalShowUI);
    }

    //贿赂
    public void OnBriberyClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        CoinArgs e = new CoinArgs
        {
            coin = BriberyTime * 500
        };
        SendEvent(Consts.E_BriberyClick,e );
    }

    private void Awake()
    {
        m_BriberyTime = 1;
    }

    
}
