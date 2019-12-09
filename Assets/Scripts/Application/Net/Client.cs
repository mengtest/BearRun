using System;
using System.Net.Sockets;
using UnityEngine;
using Sproto;
using SprotoType;

// 用来跟服务器做通信
public class Client : MonoSingleton<Client>
{
    private const string ip = "47.108.131.45";
    private const int port = 8888;
    private static Socket clientSocket;

    public void StartClient()
    {
        Debug.Log("client start");
        // 初始化socket
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            // 连接服务器
            clientSocket.Connect(ip, port);
            Debug.Log("server connected");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // 关闭socket连接
    public void Close()
    {
        clientSocket.Close();
    }

    // 接收服务器消息
    public byte[] ReceiveMessage()
    {
        Debug.Log("start receive");
        byte[] data = new byte[1024];
        clientSocket.Receive(data);
        return data;
    }

    // 发送客户端消息
    public void SendMessage(byte[] data)
    {
        if (clientSocket.Connected == false)
        {
            StartClient();
        }
        clientSocket.Send(data);
    }

    public void CloceRequest()
    {
        SprotoRpc client = new SprotoRpc();
        SprotoRpc.RpcRequest clientRequest = client.Attach(Protocol.Instance);
        // request
        close.request obj = new close.request();
        byte[] req = clientRequest.Invoke<Protocol.close>(obj, 6);
        Game.Instance.client.SendMessage(req);
    }
}