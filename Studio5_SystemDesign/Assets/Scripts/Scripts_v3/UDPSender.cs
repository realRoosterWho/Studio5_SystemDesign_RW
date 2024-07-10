using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

using System.Collections;
using System.Linq;

public class UDPSender : SerializedMonoBehaviour
{
    public Dictionary<string, string> m_Dictionary;

    public string serverIP = "127.0.0.1";
    public int serverPort = 5005;
    private UdpClient udpClient;
    public RenderTexture m_SendTexture;
    void Start()
    {
        udpClient = new UdpClient();
        SendMessage("Hello from Unity!");
        SendRenderTexture(m_SendTexture);
    }

    public void SendRenderTexture(RenderTexture renderTexture)
    {
        Texture2D texture = RenderTextureToTexture2D(renderTexture);
        SendImage(texture);
    }
    
    public void SendDefaultImage()
    {
        SendRenderTexture(m_SendTexture);
    }

    public void SendImage(Texture2D texture)
    {
        byte[] imageData = texture.EncodeToPNG();
        Debug.Log("Image size: " + imageData.Length);

        // Add header to indicate image type
        byte[] header = Encoding.UTF8.GetBytes("IMAG");
        byte[] dataToSend = new byte[header.Length + imageData.Length];
        Buffer.BlockCopy(header, 0, dataToSend, 0, header.Length);
        Buffer.BlockCopy(imageData, 0, dataToSend, header.Length, imageData.Length);

        // Split data to chunks and send
        const int chunkSize = 8192;
        int totalChunks = Mathf.CeilToInt((float)dataToSend.Length / chunkSize);
        for (int i = 0; i < totalChunks; i++)
        {
            int offset = i * chunkSize;
            int size = Mathf.Min(chunkSize, dataToSend.Length - offset);
            byte[] chunk = new byte[size];
            Buffer.BlockCopy(dataToSend, offset, chunk, 0, size);

            udpClient.Send(chunk, chunk.Length, serverIP, serverPort);
        }

        Debug.Log("Image sent.");
    }
    
    //发送字典，接受一个字典参数
    public void SendDictionary(Dictionary<string, string> dict)
    {
        string json = DictionaryToJsonString(dict);
        SendJson(json);
    }
    public void SendDefaultDictionary()
    {
        SendDictionary(m_Dictionary);
    }

    public void SendMessage(string message)
    {
        byte[] messageData = Encoding.UTF8.GetBytes(message);

        // Add header to indicate message type
        byte[] header = Encoding.UTF8.GetBytes("TEXT");
        byte[] dataToSend = new byte[header.Length + messageData.Length];
        Buffer.BlockCopy(header, 0, dataToSend, 0, header.Length);
        Buffer.BlockCopy(messageData, 0, dataToSend, header.Length, messageData.Length);

        udpClient.Send(dataToSend, dataToSend.Length, serverIP, serverPort);

        Debug.Log("Message sent: " + message);
    }

    public void SendJson(string json)
    {
        byte[] jsonData = Encoding.UTF8.GetBytes(json);

        // Add header to indicate JSON type
        byte[] header = Encoding.UTF8.GetBytes("JSON");
        byte[] dataToSend = new byte[header.Length + jsonData.Length];
        Buffer.BlockCopy(header, 0, dataToSend, 0, header.Length);
        Buffer.BlockCopy(jsonData, 0, dataToSend, header.Length, jsonData.Length);

        udpClient.Send(dataToSend, dataToSend.Length, serverIP, serverPort);

        Debug.Log("JSON sent: " + json);
    }

    private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
    {
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = currentRT;
        return texture;
    }
    
    //将字典转换为json字符串
    public string DictionaryToJsonString<T1, T2>(Dictionary<T1, T2> dict)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        foreach (var kvp in dict)
        {
            sb.Append("\"");
            sb.Append(kvp.Key);
            sb.Append("\":");
            sb.Append(kvp.Value);
            sb.Append(",");
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append("}");
        return sb.ToString();
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}