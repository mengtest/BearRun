using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using XLua;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
[RequireComponent(typeof(Client))]
[RequireComponent(typeof(LoadAB))]
[RequireComponent(typeof(HotFix))]
[Hotfix]
public class Game : MonoSingleton<Game>
{
    //全局访问
    [HideInInspector]
    public ObjectPool objectPool;
    [HideInInspector]
    public Sound sound;
    [HideInInspector]
    public StaticData staticData;
    [HideInInspector]
    public Client client;
    [HideInInspector]
    public LoadAB loadAB;
    [HideInInspector]
    public HotFix hotFix;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Start()
    {
        //别删
        DontDestroyOnLoad(gameObject);

        objectPool = ObjectPool.Instance;
        sound = Sound.Instance;
        staticData = StaticData.Instance;
        client = Client.Instance;
        loadAB = LoadAB.Instance;
        hotFix = HotFix.Instance;

        //初始化注册startUpcontroller
        RegisterController(Consts.E_StartUp, typeof(StartUpController));

        //游戏启动
        SendEvent(Consts.E_StartUp);

        // 加载启动资源
        Invoke("StartUp", 1);
    }

    private void StartUp()
    {
        Game.Instance.LoadLevel(1);
        Game.Instance.loadAB.StartLoadAB();
        Game.Instance.hotFix.StartHotFix();
        Game.Instance.client.StartClient();
    }

    public void LoadLevel(int level) {

        //发送退出场景事件
        ScenesArgs e = new ScenesArgs()
        {
            //获取当前场景索引
            scenesIndex = SceneManager.GetActiveScene().buildIndex
        };

        SendEvent(Consts.E_ExitScenes,e);

        //发送加载新的场景事件
        SceneManager.LoadScene(level,LoadSceneMode.Single);
    }

    //进入新场景
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("进入新场景：" + scene.buildIndex);
        //发送进入场景事件
        ScenesArgs e = new ScenesArgs()
        {//获取当前场景索引
            scenesIndex = scene.buildIndex
        };

        SendEvent(Consts.E_EnterScenes, e);
    }

    //发送事件
    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    //注册controler
    void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

    // 当程序退出时关闭socket
    private void OnDestroy()
    {
        Game.Instance.client.CloceRequest();
        Game.Instance.client.Close();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}