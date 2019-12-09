using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public class UIFinalScore : View
{
    public Text txtxDis;
    public Text txtCoin;
    public Text txtGoal;
    public Text txtScore;
    public Slider sliExpslidr;
    public Text txtExp;
    public Text txtGrade;

    public override string Name
    {
        get
        {
            return Consts.V_FinalScore;
        }
    }

    public void Start()
    {
    }

    public void Update()
    {
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
        gameObject.SetActive(true);
    }

    //更新UI
    public void UpdateUI(int dis,int coin, int score, int goal,int exp,int garde)
    {
        //1.距离
        txtxDis.text = dis.ToString();
        //2.金币
        txtCoin.text = coin.ToString();
        //3.得分
        txtScore.text = score.ToString();
        //4.进球
        txtGoal.text = goal.ToString();
        //5.slider文字
        txtExp.text = exp.ToString() + "/" + (500 + garde * 100).ToString();// 
        //6.slider value
        sliExpslidr.value = (float)exp / (500 + garde * 100);
        //6.garde
        txtGrade.text = garde.ToString() + "级";
    }

    public void OnReplayClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(4);
    }

    public void OnShopClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(2);
    }

    public void OnMainClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(1);
    }
}