using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAB : MonoSingleton<LoadAB>
{
    private string versionUri = @"http://arflow.cn/test/version.txt";
    private string abUri = @"http://arflow.cn/test/AssetBundles/";
    private int localVersion = 0;
    private AssetBundleManifest manifest;
    public string path;
    private bool isSaveAB = false;

    // 跑道资源
    public Dictionary<string, GameObject> objectsDict = new Dictionary<string, GameObject>();
    // 声音资源
    public Dictionary<string, AudioClip> soundsDict = new Dictionary<string, AudioClip>();
    // 排行榜资源
    public Dictionary<string, GameObject> rankDict = new Dictionary<string, GameObject>();
    // 热更新资源
    public Dictionary<string, GameObject> hotfixDict = new Dictionary<string, GameObject>();

    public void StartLoadAB()
    {
        path  = Application.persistentDataPath + "/";
        StartCoroutine(isUpdateAB());
    }

    private void Update()
    {
        if (isSaveAB)
        {
            LoadAllAB();
            isSaveAB = false;
        }
    }

    IEnumerator isUpdateAB()
    {
        UnityWebRequest request = UnityWebRequest.Get(versionUri);
        yield return request.SendWebRequest();
        int remoteVersion = int.Parse(request.downloadHandler.text);
        if (remoteVersion == localVersion)
        {
            Debug.Log("No Updating");
        }
        else
        {
            Debug.Log("Update");
            StartCoroutine(UpdateAB());
            localVersion = remoteVersion;
        }
    }

    IEnumerator UpdateAB()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(abUri+ "AssetBundles");
        yield return request.SendWebRequest();
        AssetBundle manifestAB = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        foreach (string abName in manifest.GetAllAssetBundles())
        {
            StartCoroutine(DownloadAndSaveAB(abName));
        }
        isSaveAB = true;
    }

    IEnumerator DownloadAndSaveAB(string abName)
    {
        UnityWebRequest request = UnityWebRequest.Get(abUri + abName);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            byte[] result = request.downloadHandler.data;
            FileStream writeStream;
            FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + abName);
            if (!fi.Exists)
            {
                writeStream = fi.Create();
            }
            else
            {
                //如果此文件存在则打开
                writeStream = fi.OpenWrite();
            }
            writeStream.Write(result, 0, result.Length);
            //文件写入存储到硬盘
            writeStream.Flush();
            writeStream.Close();
        }
    }

    public void LoadAllAB()
    {
        foreach (string abName in manifest.GetAllAssetBundles())
        {
            if (abName == "objects")
            {
                StartCoroutine(LoadPrefab("objects", objectsDict));
            }
            else if (abName == "sounds")
            {
                StartCoroutine(LoadSoundPrefab("sounds", soundsDict));
            }
            else if (abName == "rank")
            {
                StartCoroutine(LoadPrefab("rank", rankDict));
            }
            else if (abName == "hotfix")
            {
                StartCoroutine(LoadPrefab("hotfix", hotfixDict));
            }
            else if (abName == "lua")
            {
                StartCoroutine(SaveLuaTxt());
            }
            else if (abName == "share")
            {
                AssetBundle share = AssetBundle.LoadFromFile(path + abName);
            }
        }
    }

    IEnumerator LoadPrefab(string abName, Dictionary<string, GameObject> prefabDict)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path + abName);
        yield return request;
        AssetBundle ab = request.assetBundle;
        AssetBundleRequest request2 = ab.LoadAllAssetsAsync();
        yield return request2;
        object[] objs = request2.allAssets;
        foreach (GameObject go in objs)
        {
            prefabDict.Add(go.name, go);
        }
    }

    IEnumerator LoadSoundPrefab(string abName, Dictionary<string, AudioClip> soundPrefabDict)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path + abName);
        yield return request;
        AssetBundle ab = request.assetBundle;
        AssetBundleRequest request2 = ab.LoadAllAssetsAsync();
        yield return request2;
        object[] objs = request2.allAssets;
        foreach (AudioClip clip in objs)
        {
            soundPrefabDict.Add(clip.name, clip);
        }
    }

    IEnumerator SaveLuaTxt()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path + "lua");
        yield return request;
        AssetBundle ab = request.assetBundle;
        AssetBundleRequest request2 = ab.LoadAllAssetsAsync();
        yield return request2;
        object[] objs = request2.allAssets;
        foreach (TextAsset txt in objs)
        {
            Debug.Log(txt.name);
            FileStream writeStream;
            FileInfo fi = new FileInfo(path + txt.name + ".txt");
            if (!fi.Exists)
            {
                writeStream = fi.Create();
            }
            else
            {
                //如果此文件存在则打开
                writeStream = fi.OpenWrite();
            }
            writeStream.Write(txt.bytes, 0, txt.bytes.Length);
            //文件写入存储到硬盘
            writeStream.Flush();
            writeStream.Close();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
