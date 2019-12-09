using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SprotoType;
using Sproto;

public class UILoginRegister : View
{
    public SkinnedMeshRenderer skm;
    public MeshRenderer render;
    public Text txtMoney;
    GameModel gm;

    // 登录注册
    public GameObject uiLogin;
    public GameObject uiRegister;
    public GameObject uiPerson;

    public InputField loginName;
    public InputField loginPass;
    public InputField registerName;
    public InputField registerPass;
    public InputField registerPass2;
    public GameObject uiError;
    public Text errorInfo;

    public Text username;
    public Text coin;
    public Text score;
    public Text exp;
    public Text rank;

    public override string Name
    {
        get
        {
            return Consts.V_LoginSign;
        }
    }

    public override void HandleEvent(string name, object data)
    {
    }

    private void Start()
    {
        gm = GetModel<GameModel>();
        InitUI();
    }

    public void InitUI()
    {
        txtMoney.text = gm.Coin.ToString();
        gm = GetModel<GameModel>();
        skm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        render.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
        if (gm.IsLogin == true)
        {
            SendEvent(Consts.E_ShowPerson);
        }
    }

    public void OnLoginClick()
    {
        if (loginName.text == "" || loginPass.text == "")
        {
            ShowError("用户名或密码不能为空");
        }
        else
        {
            LoginArgs e = new LoginArgs()
            {
                username = loginName.text,
                password = loginPass.text,
            };
            SendEvent(Consts.E_Login, e);
        }
    }

    public void OnRegisterClick()
    {
        if (registerName.text == "" || registerPass.text == "" || registerPass2.text == "")
        {
            ShowError("用户名或密码不能为空");
        }
        else if (registerPass.text != registerPass2.text)
        {
            ShowError("两次输出的密码不一致");
        }
        else
        {
            RegisterArgs e = new RegisterArgs
            {
                username = registerName.text,
                password = registerPass.text,
            };
            SendEvent(Consts.E_Register, e);
        }
    }

    public void ShowRegister()
    {
        uiLogin.SetActive(false);
        uiRegister.SetActive(true);
    }

    public void ShowLogin()
    {
        uiLogin.SetActive(true);
        uiRegister.SetActive(false);
    }

    public void ShowError(string errorInfo)
    {
        this.errorInfo.text = errorInfo;
        uiError.SetActive(true);
        if (errorInfo == "登录成功") 
        {
            Invoke("LoginSuccess", 1);
        }
        if (errorInfo == "注册成功")
        {
            Invoke("RegisterSuccess", 1);
        }
    }

    public void LoginSuccess()
    {
        CloseError();
        SendEvent(Consts.E_ShowPerson);
    }

    public void RegisterSuccess()
    {
        CloseError();
        ShowLogin();
    }

    public void CloseError()
    {
        uiError.SetActive(false);
    }

    public void OnReturnClicK()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(1);
    }

    public void OnPlayClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(3);
    }

    public void ShowPerson(string username, int coin, int score, int exp, int rank)
    {
        this.username.text = username;
        txtMoney.text = coin.ToString();
        this.coin.text = coin.ToString() + "个";
        this.score.text = score.ToString() + "分";
        this.exp.text = exp.ToString() + "点";
        if (rank == 0)
        {
            this.rank.text = "无";
        }
        else
        {
            this.rank.text = rank.ToString()+"名";
        }
        uiLogin.SetActive(false);
        uiRegister.SetActive(false);
        uiError.SetActive(false);
        uiPerson.SetActive(true);
    }
}