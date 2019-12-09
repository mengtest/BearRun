using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SprotoType;

public class UIMainMenu : View
{
    public SkinnedMeshRenderer skm;
    public MeshRenderer render;
    private GameModel gm;

    // ShowRank相关
    public GameObject content;
    public RectTransform contentRT;
    private GameObject rowPrefab;
    private GameObject go;
    private int rankPoolCount = 5;
    private List<GameObject> rankPool = new List<GameObject>();
    private float timer = 0;

    // 初始化排行榜对象池
    private void InitRankPool()
    {
        for (int i = 0; i < rankPoolCount; i++)
        {
            go = Instantiate(rowPrefab, content.transform);
            go.SetActive(false);
            rankPool.Add(go);
        }
    }

    // 回收对象并重置个别对象的颜色属性
    private void Recycle()
    {
        for (int i = 0; i < rankPoolCount; i++)
        {
            rankPool[i].SetActive(false);
            Text[] texts = rankPool[i].GetComponentsInChildren<Text>();
            if (texts[0].color == Color.green)
            {
                Color defaultColor;
                ColorUtility.TryParseHtmlString("#D4D432", out defaultColor);
                texts[0].color = defaultColor;
                texts[1].color = defaultColor;
                texts[2].color = defaultColor;
            }
        }
    }

    public override string Name
    {
        get
        {
            return Consts.V_MainMenu;
        }
    }

    public override void HandleEvent(string name, object data)
    {
    }

    // 显示排行榜函数
    public void ShowRank(List<info> rank, int index)
    {
        int rankCount = rank.Count;
        // 每次显示前都进行对象池回收，确保每个对象都是false。
        Recycle();
        Debug.Log("ShowRank Start");
        for (int i = 0; i < 5; i++)
        {
            index++;
            if (index < 0 || index > rankCount - 1)
            {
                continue;
            }
            // 设置对应行内容
            SetRowTexts(GetRow(), rank[index]);
        }
    }

    // 从对象池获取对象
    public GameObject GetRow()
    {
        foreach (GameObject go in rankPool)
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return go;
            }
        }
        return null;
    }

    //// 每个15帧更新一次
    //private void Update()
    //{
    //    if (timer > 15)
    //    {
    //        UpdateRank();
    //        timer = 0;
    //    }
    //    timer += Time.deltaTime;
    //}

    // 更新排行榜函数
    public void UpdateRank()
    {
        Debug.Log("Down");
        gm.Index += 5;
        SendEvent(Consts.E_ShowRank);
    }

    // 设置每一行的文本值
    private void SetRowTexts(GameObject row, info i)
    {
        Text[] texts = row.GetComponentsInChildren<Text>();
        texts[0].text = i.username;
        texts[1].text = i.score + "分";
        texts[2].text = "第" + i.scorerank + "名";
        if (i.username == gm.Username)
        {
            texts[0].color = Color.green;
            texts[1].color = Color.green;
            texts[2].color = Color.green;
        }
    }

    public void OnPlayClicK()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(3);
    }

    public void OnMeClicK()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(5);
    }

    public void OnShopClick()
    {
        Game.Instance.sound.PlayEffect("Se_UI_Button");
        Game.Instance.LoadLevel(2);
    }

    private void Start()
    {
        gm = GetModel<GameModel>();
        skm.material = Game.Instance.staticData.GetPlayerInfo(gm.TakeOnCloth.SkinID, gm.TakeOnCloth.ClothID).Material;
        render.material = Game.Instance.staticData.GetFootballInfo(gm.TakeOnFootball).Material;
        rowPrefab = Game.Instance.loadAB.rankDict["row"];
        InitRankPool();
        gm.InitIndex();
        SendEvent(Consts.E_ShowRank);
    }
}