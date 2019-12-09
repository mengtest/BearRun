using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sproto;
using SprotoType;
using XLua;

[Hotfix]
public class GameModel : Model
{

    #region 常量
    const int InitCoin = 1000;
    #endregion

    #region 字段
    bool m_IsPlay = true;
    bool m_IsPause = false;

    //特技时间
    int m_SkillTime = 5;
    int m_Magnet;
    int m_Multiply;
    int m_Invincible;

    // 玩家信息
    string m_Username;
    int m_Grade;
    int m_Exp;
    int m_Score;
    int m_Coin;
    int m_Rank;

    // 排序相关
    List<info> rank;
    int m_Index;
    int m_Times;

    // 是否登录
    bool m_IsLogin;

    //装备
    int m_TakeOnFootball = 0;
    //购买
    public List<int> BuyFootBall = new List<int>();

    //装备衣服
    BuyID m_TakeOnCloth = new BuyID() { SkinID =0, ClothID = 0};
    //购买的衣服
    public List<BuyID> BuyCloth = new List<BuyID>();
    public int lastIndex = 1;
    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.M_GameModel;
        }
    }

    public bool IsPlay
    {
        get
        {
            return m_IsPlay;
        }

        set
        {
            m_IsPlay = value;
        }
    }

    public bool IsPause
    {
        get
        {
            return m_IsPause;
        }

        set
        {
            m_IsPause = value;
        }
    }

    public int SkillTime
    {
        get
        {
            return m_SkillTime;
        }

        set
        {
            m_SkillTime = value;
        }
    }

    public int Magnet
    {
        get
        {
            return m_Magnet;
        }

        set
        {
            m_Magnet = value;
        }
    }

    public int Multiply
    {
        get
        {
            return m_Multiply;
        }

        set
        {
            m_Multiply = value;
        }
    }

    public int Invincible
    {
        get
        {
            return m_Invincible;
        }

        set
        {
            m_Invincible = value;
        }
    }

    public int Grade
    {
        get
        {
            return m_Grade;
        }

        set
        {
            m_Grade = value;
        }
    }

    public int Exp
    {
        get
        {
            return m_Exp;
        }

        set
        {
            while (value > 500 + Grade * 100)
            {
                value -= 500 + Grade * 100;
                Grade++;
            }
            m_Exp = value;
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
            Debug.Log("现在还剩" + value + "钱");
        }
    }

    public int TakeOnFootball
    {
        get
        {
            return m_TakeOnFootball;
        }

        set
        {
            m_TakeOnFootball = value;
        }
    }

    public BuyID TakeOnCloth
    {
        get
        {
            return m_TakeOnCloth;
        }

        set
        {
            m_TakeOnCloth = value;
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

    public string Username
    {
        get
        {
            return m_Username;
        }

        set
        {
            m_Username = value;
        }
    }

    public int Rank
    {
        get
        {
            return m_Rank;
        }

        set
        {
            m_Rank = value;
        }
    }

    public bool IsLogin
    {
        get
        {
            return m_IsLogin;
        }

        set
        {
            m_IsLogin = value;
        }
    }

    public int Index
    {
        get
        {
            return m_Index;
        }

        set
        {
            m_Index = value;
        }
    }

    public int Times
    {
        get
        {
            return m_Times;
        }

        set
        {
            m_Times = value;
        }
    }
    #endregion

    #region 方法
    //初始化
    public void Init()
    {
        m_Magnet = 0;
        m_Invincible = 0;
        m_Multiply = 0;
        m_SkillTime = 5;
        m_Username = "小熊";
        m_Exp = 0;
        m_Grade = 1;
        m_Score = 0;
        m_Coin = InitCoin;
        m_Rank = 1;
        m_Index = -1;
        m_IsLogin = false;
        m_Times = 0;
        InitSkin();
        InitIndex();
    }

    void InitSkin()
    {
        //足球信息
        BuyFootBall.Add(m_TakeOnFootball);
        //衣服
        BuyCloth.Add(TakeOnCloth);
    }

    // 排行榜的初始定位
    public void InitIndex()
    {
        if (rank == null)
        {
            return;
        }
        for (int i = 0; i < rank.Count; i++)
        {
            if (rank[i].username == m_Username)
            {
                Debug.Log(i);
                m_Index = i - 2;
            }
        }
    }

    // 登录请求
    public login.response LoginRequest(LoginArgs e)
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        login.request obj = new login.request();
        obj.username = e.username;
        obj.password = e.password;
        byte[] req = clientRequest.Invoke<Protocol.login>(obj, 1);
        Game.Instance.client.SendMessage(req);
        // dispatch
        byte[] res = Game.Instance.client.ReceiveMessage();
        SprotoRpc.RpcInfo info = client.Dispatch(res);
        login.response obj2 = info.responseObj as login.response;
        if (obj2.status == true)
        {
            this.m_IsLogin = true;
            this.m_Username = e.username;
        }
        return obj2;
    }

    // 注册请求
    public register.response RegisterRequest(RegisterArgs e)
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        register.request obj = new register.request();
        obj.username = e.username;
        obj.password = e.password;
        byte[] req = clientRequest.Invoke<Protocol.register>(obj, 2);
        Game.Instance.client.SendMessage(req);
        // dispatch
        byte[] res = Game.Instance.client.ReceiveMessage();
        SprotoRpc.RpcInfo info = client.Dispatch(res);
        register.response obj2 = info.responseObj as register.response;
        if (obj2.status == true)
        {
            this.m_Username = e.username;
        }
        return obj2;
    }

    // 获取用户信息请求
    public getUser.response GetUserRequest()
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        getUser.request obj = new getUser.request();
        obj.username = m_Username;
        byte[] req = clientRequest.Invoke<Protocol.getUser>(obj, 3);
        Game.Instance.client.SendMessage(req);
        // dispatch
        byte[] res = Game.Instance.client.ReceiveMessage();
        SprotoRpc.RpcInfo info = client.Dispatch(res);
        getUser.response obj2 = info.responseObj as getUser.response;
        this.m_Score = (int)obj2.score;
        this.m_Coin = (int)obj2.coin;
        this.m_Exp = (int)obj2.exp;
        this.m_Rank = (int)obj2.rank;
        return obj2;
    }

    // 上传用户信息请求
    public void PostUserRequest(int coin, int score, int exp)
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        postUser.request obj = new postUser.request();
        this.m_Coin += coin;
        this.m_Score += score;
        this.m_Exp = exp;
        obj.username = m_Username;
        obj.coin = m_Coin;
        obj.score = m_Score;
        obj.exp = m_Exp;
        obj.rank = m_Rank;
        byte[] req = clientRequest.Invoke<Protocol.postUser>(obj, 4);
        Game.Instance.client.SendMessage(req);
        // dispatch
        byte[] res = Game.Instance.client.ReceiveMessage();
        SprotoRpc.RpcInfo info = client.Dispatch(res);
        postUser.response obj2 = info.responseObj as postUser.response;
        if (obj2.status == true)
        {
            Debug.Log("user data post success");
        }
        else
        {
            Debug.Log("user data post failed");
        }
    }

    // 获取排行榜信息请求
    public List<info> GetRankRequest()
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        getRank.request obj = new getRank.request();
        byte[] req = clientRequest.Invoke<Protocol.getRank>(obj, 5);
        Game.Instance.client.SendMessage(req);
        // dispatch
        byte[] res = Game.Instance.client.ReceiveMessage();
        SprotoRpc.RpcInfo info = client.Dispatch(res);
        getRank.response obj2 = info.responseObj as getRank.response;
        if (obj2.status == true)
        {
            Debug.Log("rank got!");
            rank = obj2.rank;
        }
        else
        {
            Debug.Log("rank failed!");
            rank = new List<info>();
        }
        return rank;
    }

    //买东西
    public bool GetMoney(int coin)
    {
        if(coin <= Coin)
        {
            Coin -= coin;
            return true;
        }
        return false;
    }

    //检查足球状态
    public ItemState CheckFootBallState(int i)
    {
        if(i == TakeOnFootball)
        {
            return ItemState.Equip;
        }
        else
        {
            if (BuyFootBall.Contains(i))
            {
                return ItemState.Buy;
            }
            else
            {
                return ItemState.UnBuy;
            }
        }
    }
    //检查skin
    public ItemState CheckSkinState(int i)
    {
        if (i == TakeOnCloth.SkinID)
        {
            return ItemState.Equip;
        }
        else
        {
            foreach(var a in BuyCloth)
            {
                if(a.SkinID == i)
                {
                    return ItemState.Buy;
                }
            }
            return ItemState.UnBuy;
        }

    }

    //检查cloth
    public ItemState CheckClothState(BuyID  id)
    {
        if (id.SkinID == TakeOnCloth.SkinID && id.ClothID == TakeOnCloth.ClothID)
        {
            return ItemState.Equip;
        }
        else
        {
            foreach (var a in BuyCloth)
            {
                if (a.SkinID == id.SkinID && a.ClothID == id.ClothID)
                {
                    return ItemState.Buy;
                }
            }
            return ItemState.UnBuy;
        }
    }
    #endregion
}

public class BuyID
{
    public int SkinID;
    public int ClothID;
}
