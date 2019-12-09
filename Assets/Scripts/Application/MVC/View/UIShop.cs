using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : View
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public int selectIndex = 0;
    public MeshRenderer ball;
    public ItemState state = ItemState.UnBuy;

    public SkinnedMeshRenderer playerSkm;

    //图片资源
    public Sprite spBuy;
    public Sprite spEquip;
    public Sprite gizmoBuy;
    public Sprite gizmoUnBuy;
    public Sprite gizmoEquip;
    public List<Sprite> head; // momo sali suger

    //持有按钮图片
    public Image imgBuyFootBall;
    public Image imgBuySkin;
    public Image imgBuyCloth;
    public Text txtGrade;

    //football gizmo
    public List<Image> footBallGizmo;
    public List<Image> skinGizmo;
    public List<Image> clothGizmo;

    //更新UI
    public Text txtMoney;
    public Text txtName;
    public Image headShow;

    GameModel gm;
    

    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_Shop;
        }
    }


    #endregion

    #region 方法

    public void UpdateUI()
    {
        txtMoney.text = gm.Coin.ToString();
        txtName.text = gm.Username;
        headShow.overrideSprite = head[gm.TakeOnCloth.SkinID];
    }

    #region 足球
    public void NormalFootBall()
    {
        selectIndex = 0;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        ball.material =  Game.Instance.staticData.GetFootballInfo(selectIndex).Material;
        UpdateFootBallBuyButton(selectIndex);
    }

    public void FireFootBall()
    {
        selectIndex = 1;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        ball.material = Game.Instance.staticData.GetFootballInfo(selectIndex).Material;
        UpdateFootBallBuyButton(selectIndex);
    }
    public void ColorFootBall()
    {
        selectIndex = 2;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        ball.material = Game.Instance.staticData.GetFootballInfo(selectIndex).Material;
        UpdateFootBallBuyButton(selectIndex);
    }

    //更新按钮显示
    public void UpdateFootBallBuyButton(int i)
    {
        state = gm.CheckFootBallState(i);
        switch (state)
        {
            case ItemState.UnBuy:
                imgBuyFootBall.transform.gameObject.SetActive(true);
                imgBuyFootBall.overrideSprite = spBuy;
                break;
            case ItemState.Buy:
                imgBuyFootBall.transform.gameObject.SetActive(true);
                imgBuyFootBall.overrideSprite = spEquip;
                break;
            case ItemState.Equip:
                imgBuyFootBall.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    //购买按钮点击
    public void OnBuyFootBallCilck()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");

        state = gm.CheckFootBallState(selectIndex);
        switch (state)
        {
            case ItemState.UnBuy:
                //发起购买
               int money =  Game.Instance.staticData.GetFootballInfo(selectIndex).Coin;
                BuyFootBallArgs e = new BuyFootBallArgs
                {
                    coin = money,
                    noeIndex = selectIndex
                };
                //购买足球
                SendEvent(Consts.E_BuyFootBall, e);
                break;
            case ItemState.Buy:
                //装备
                int moneys = Game.Instance.staticData.GetFootballInfo(selectIndex).Coin;
                BuyFootBallArgs ee = new BuyFootBallArgs
                {
                    coin = moneys,
                    noeIndex = selectIndex
                };
                //购买足球
                SendEvent(Consts.E_EquipFootBall, ee);
                break;
            case ItemState.Equip:
                //noting
                break;
            default:
                break;
        }
    }

    //更新football Gizmo
    public void UpdateFootBallGizmo()
    {
        for(int i  = 0;i< 3; i++)
        {
            state = gm.CheckFootBallState(i);
            switch (state)
            {
                case ItemState.UnBuy:
                    footBallGizmo[i].overrideSprite = gizmoUnBuy;
                    break;
                case ItemState.Buy:
                    footBallGizmo[i].overrideSprite = gizmoBuy;
                    break;
                case ItemState.Equip:
                    footBallGizmo[i].overrideSprite = gizmoEquip;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion


    #region 皮肤

    public void OnMoMoClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        selectIndex = 0;
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(selectIndex, 0).Material;
        state = gm.CheckSkinState(selectIndex);
        UpdateBuySkinButton();

    }

    public void OnSaliClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        selectIndex = 1;
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(selectIndex, 0).Material;
        state = gm.CheckSkinState(selectIndex);
        UpdateBuySkinButton();
    }

    public void OnSugerClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        selectIndex = 2;
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(selectIndex, 0).Material;
        state = gm.CheckSkinState(selectIndex);
        UpdateBuySkinButton();
    }

    //皮肤代码
    public void UpdateBuySkinButton()
    {
        state = gm.CheckSkinState(selectIndex);
        switch (state)
        {
            case ItemState.UnBuy:
                imgBuySkin.transform.gameObject.SetActive(true);
                imgBuySkin.overrideSprite = spBuy;
                break;
            case ItemState.Buy:
                imgBuySkin.transform.gameObject.SetActive(true);
                imgBuySkin.overrideSprite = spEquip;
                break;
            case ItemState.Equip:
                imgBuySkin.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    //买皮肤
    public void OnBuySkinClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");

        state = gm.CheckSkinState(selectIndex);
        switch (state)
        {
            case ItemState.UnBuy:
                BuyID Id = new BuyID { SkinID = selectIndex, ClothID = 0 };
                int money = Game.Instance.staticData.GetPlayerInfo(selectIndex, 0).Coin;
                BuyClothArgs e = new BuyClothArgs
                {
                     coin = money,
                     id = Id
                };
                SendEvent(Consts.E_BuyCloth, e);
                break;
            case ItemState.Buy:
                //装备
                BuyID Ids = new BuyID { SkinID = selectIndex, ClothID = 0 };
                int moneys = Game.Instance.staticData.GetPlayerInfo(selectIndex, 0).Coin;
                BuyClothArgs es = new BuyClothArgs
                {
                    coin = moneys,
                    id = Ids
                };
                //装备
                SendEvent(Consts.E_EquipCloth, es);
                break;
            case ItemState.Equip:
                //noting
                break;
            default:
                break;
        }
        //跟新按钮
        //gizmo
        UpdateBuySkinButton();
        UpdateSkinGizmo();
    }

    //更新skin Gizmo
    public void UpdateSkinGizmo()
    {
        for (int i = 0; i < 3; i++)
        {
            state = gm.CheckSkinState(i);
            switch (state)
            {
                case ItemState.UnBuy:
                    skinGizmo[i].overrideSprite = gizmoUnBuy;
                    break;
                case ItemState.Buy:
                    skinGizmo[i].overrideSprite = gizmoBuy;
                    break;
                case ItemState.Equip:
                    skinGizmo[i].overrideSprite = gizmoEquip;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion


    #region 衣服
    public void OnNormalClick()
    {
        selectIndex = 0;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, selectIndex).Material;
        UpdateBuyClothButton();
    }

    public void OnBrizalClick()
    {
        selectIndex =1;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, selectIndex).Material;
        UpdateBuyClothButton();
    }

    public void OnGerManClick()
    {
        selectIndex = 2;
        Game.Instance.sound.PlayEffect("Se_UI_Dress");
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, selectIndex).Material;
        UpdateBuyClothButton();
    }

    //cloth代码
    public void UpdateBuyClothButton()
    {
        BuyID id = new BuyID
        {
             ClothID = selectIndex,
                SkinID = gm.TakeOnCloth.SkinID
        };
        state = gm.CheckClothState(id);
        switch (state)
        {
            case ItemState.UnBuy:
                imgBuyCloth.transform.gameObject.SetActive(true);
                imgBuyCloth.overrideSprite = spBuy;
                break;
            case ItemState.Buy:
                imgBuyCloth.transform.gameObject.SetActive(true);
                imgBuyCloth.overrideSprite = spEquip;
                break;
            case ItemState.Equip:
                imgBuyCloth.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }


    //买cloth
    public void OnBuyClothClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");

        BuyID ID = new BuyID
        {
            ClothID = selectIndex,
            SkinID = gm.TakeOnCloth.SkinID
        };
        state = gm.CheckClothState(ID);
        switch (state)
        {
            case ItemState.UnBuy:
                //BuyID Id = new BuyID { SkinID = selectIndex, ClothID = selectIndex };
                int money = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, selectIndex).Coin;
                BuyClothArgs e = new BuyClothArgs
                {
                    coin = money,
                    id = ID
                };
                SendEvent(Consts.E_BuyCloth, e);
                break;
            case ItemState.Buy:
                //装备
               
                int moneys = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, selectIndex).Coin;
                BuyClothArgs es = new BuyClothArgs
                {
                    coin = moneys,
                    id = ID
                };
                //装备
                SendEvent(Consts.E_EquipCloth, es);
                break;
            case ItemState.Equip:
                //noting
                break;
            default:
                break;
        }
        //跟新按钮
        UpdateBuyClothButton();
        //gizmo
        UpdateClothGizmo();
    }

    //更新cloth Gizmo
    public void UpdateClothGizmo()
    {
        for (int i = 0; i < 3; i++)
        {
            BuyID ID = new BuyID
            {
                ClothID = i,
                SkinID = gm.TakeOnCloth.SkinID
            };
            state = gm.CheckClothState(ID);
            switch (state)
            {
                case ItemState.UnBuy:
                    clothGizmo[i].overrideSprite = gizmoUnBuy;
                    break;
                case ItemState.Buy:
                    clothGizmo[i].overrideSprite = gizmoBuy;
                    break;
                case ItemState.Equip:
                    clothGizmo[i].overrideSprite = gizmoEquip;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

   
    public void OnGroupSkin()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        ball.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
        UpdateSkinGizmo();
        Hide();
    }

    public void OnGroupCloth()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        UpdateClothGizmo();
        ball.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
        Hide();
    }
    public void OnGroupFootBall()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        UpdateFootBallGizmo();
        Hide();

    }


    void Hide()
    {
        imgBuySkin.transform.gameObject.SetActive(false);
        imgBuyCloth.transform.gameObject.SetActive(false);
        imgBuyFootBall.transform.gameObject.SetActive(false);
    }


    public void OnPlayGame()
    {
        Game.Instance.LoadLevel(3);
        Game.Instance.sound.PlayEffect("Se_UI_Button");
    }

    public void OnHomeGame()
    {
        Game.Instance.LoadLevel(1);
        Game.Instance.sound.PlayEffect("Se_UI_Button");
    }

    public void OnReturnClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        if (gm.lastIndex == 4)
            gm.lastIndex = 1;
        Game.Instance.LoadLevel(gm.lastIndex);
    }

    #endregion

    #region Unity回调


    private void Awake()
    {
        gm = GetModel<GameModel>();
        gm = GetModel<GameModel>();
        playerSkm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        ball.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
        UpdateUI();

        UpdateSkinGizmo();

        txtGrade.text = gm.Grade.ToString();
    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string name, object data)
    {
    }

    #endregion

    #region 帮助方法
    #endregion

}
