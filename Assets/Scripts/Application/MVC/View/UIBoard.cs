using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoard : View
{

    #region 常量
    public const int startTime =50;
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_Coin = 0;
    int m_Distance = 0;
    int m_GoalCount = 0;
    int m_Score = 0;
    int m_SkillTime;
    float m_Time;
    GameModel gm;

    public Text txtCoin;
    public Text txtDis;
    public Text txtTimer;
    public Text txtGizmoMangent;
    public Text txtGizmoMultiply;
    public Text txtGizmoInvincible;

    public Slider sliTime;
    public Slider sliGoal; //射门

    public Button btnMagnet;
    public Button btnMultiply;
    public Button btnInvincible;
    public Button btnGoal;

    //双倍金币协程
    IEnumerator MultiplyCor;
    //吸铁石协程
    IEnumerator MagnetCor;
    //无敌状态协程
    IEnumerator InvinciblelCor;

    #endregion

    #region 属性

    public override string Name
    {
        get
        {
            return Consts.V_Board;
        }
    }

    public int Coin
    {
        get
        {
            return m_Coin;
        }

        set
        {
            m_Coin = value;
            txtCoin.text = value.ToString();
        }
    }

    public int Distance
    {
        get
        {
            return m_Distance;
        }

        set
        {
            m_Distance = value;
            txtDis.text = value.ToString() + "米";
        }
    }

    public float Times
    {
        get
        {
            return m_Time;
        }

        set
        {
            if (value < 0)
            {
                value = 0;
                //游戏结束
                SendEvent(Consts.E_EndGame);
            }
            if (value > startTime)
            {
                value = startTime;
            }
       
            m_Time = value;
            txtTimer.text = m_Time.ToString("f2") +"s";
            sliTime.value = value / startTime;
        }
    }

    public int GoalCount
    {
        get
        {
            return m_GoalCount;
        }

        set
        {
            m_GoalCount = value;
        }
    }

    public int Score
    {
        get
        {
            return m_Score;
        }

        set
        {
            m_Score = value;
        }
    }
    #endregion

    #region 方法
    //更新 按钮是否可用
    public void UpdateUI()
    {
        ShowOrHide(gm.Invincible, btnInvincible); 
        ShowOrHide(gm.Magnet, btnMagnet);
        ShowOrHide(gm.Multiply, btnMultiply);
    }

    void ShowOrHide(int i,Button btn)
    {
        if(i > 0)
        {
            btn.interactable = true;
            btn.transform.Find("Mask").gameObject.SetActive(false);
        }
        else
        {
            btn.interactable = false;
            btn.transform.Find("Mask").gameObject.SetActive(true);
        }

    }

    //暂停按钮点击
    public void OnPauseClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        PauseArgs e = new PauseArgs
        {
            coin = Coin,
            distance = Distance,
            score = Coin + Distance * (GoalCount + 1)
        };

        SendEvent(Consts.E_PauseGame ,e);
    }


    //UI更新

    //双倍金币
    public void HitMultiply()
    {

        if (MultiplyCor != null)
        {
            StopCoroutine(MultiplyCor);
        }
        MultiplyCor = MultiplyCoroutine();
        StartCoroutine(MultiplyCor);
    }

    IEnumerator MultiplyCoroutine()
    {
        float timer = m_SkillTime;
        txtGizmoMultiply.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoMultiply.text = GetTime(timer);
            }
            yield return 0;
        }
        txtGizmoMultiply.transform.parent.gameObject.SetActive(false);
    }

    //吸铁石
    public void HitMagnet()
    {
        if (MagnetCor != null)
        {
            StopCoroutine(MagnetCor);
        }
        MagnetCor = MagnetCoroutine();
        StartCoroutine(MagnetCor);

    }

    IEnumerator MagnetCoroutine()
    {
        float timer = m_SkillTime;
        txtGizmoMangent.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoMangent.text = GetTime(timer);
            }
            yield return 0;
        }
        txtGizmoMangent.transform.parent.gameObject.SetActive(false);
    }

    //无敌状态
    public void HitInvincible()
    {

        if (InvinciblelCor != null)
        {
            StopCoroutine(InvinciblelCor);
        }
        InvinciblelCor = InvincibleCoroutine();
        StartCoroutine(InvinciblelCor);
    }

    IEnumerator InvincibleCoroutine()
    {
        float timer = m_SkillTime;
        txtGizmoInvincible.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoInvincible.text = GetTime(timer);
            }
            yield return 0;
        }
        txtGizmoInvincible.transform.parent.gameObject.SetActive(false);
    }

    string GetTime(float time)
    {
        return ((int)time +1).ToString();
    }

    //magnet
    public void OnMagnetClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        ItemArgs e = new ItemArgs
        {
            hitCount = 1,
            kind = ItemKind.ItemMagnet
        };
        SendEvent(Consts.E_HitItem, e);
    }

    public void OnInvincibleClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        ItemArgs e = new ItemArgs
        {
            hitCount = 1,
            kind = ItemKind.ItemInvincible
        };
        SendEvent(Consts.E_HitItem, e);
    }
    public void OnMulityCilck()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        ItemArgs e = new ItemArgs
        {
            hitCount = 1,
            kind = ItemKind.ItemMultiply
        };
        SendEvent(Consts.E_HitItem, e);
    }

   void ShowGoalClick()
    {
        //1.slider可以显示
        //2.btn可以按下
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        btnGoal.interactable = true;
        sliGoal.value = 1;
        while(sliGoal.value > 0) //1.25随便写的
        {
            if ( !gm.IsPause && gm.IsPlay)
            {
                sliGoal.value -= 1.5f * Time.deltaTime;
            }
           
            yield return 0;
        }
        btnGoal.interactable = false;
        sliGoal.value = 0;
    }

    //按下射门键
    public void OnGoalBtnClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        SendEvent(Consts.E_ClickGoalButton);
        sliGoal.value = 0;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }



    #endregion

    #region Unity回调
    private void Awake()
    {
        Times = startTime;
        gm = GetModel<GameModel>();
        UpdateUI();
        m_SkillTime = gm.SkillTime;
    }

    private void Update()
    {
        if (!gm.IsPause && gm.IsPlay)
            Times -= Time.deltaTime;
    }

    #endregion

    #region 事件回调
    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(Consts.E_UpdataDis);
        AttentionList.Add(Consts.E_UpdateCoin);
        AttentionList.Add(Consts.E_HitAddTime);
        AttentionList.Add(Consts.E_HitGoalTrigger);
        AttentionList.Add(Consts.E_ShootGoal);
    }


    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case Consts.E_UpdataDis:
                DistanceArgs e1 = data as DistanceArgs;
                Distance = e1.distance;
                break;
            case Consts.E_UpdateCoin:
                CoinArgs e2 = data as CoinArgs;
                Coin += e2.coin;
                break;
            case Consts.E_HitAddTime:
                Times += 20;
                break;
            case Consts.E_HitGoalTrigger:
                ShowGoalClick();
                break;
            case Consts.E_ShootGoal:
                m_GoalCount += 1;
                print("进了"+m_GoalCount + "球");
                break;
        }
    }

   
    #endregion

    #region 帮助方法
    #endregion

}
