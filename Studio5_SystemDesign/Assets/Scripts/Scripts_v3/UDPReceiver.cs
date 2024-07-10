using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    public int listenPort = 5005;
    private UdpClient udpClient;
    private Thread listenerThread;

    void Start()
    {
        udpClient = new UdpClient(listenPort);
        listenerThread = new Thread(new ThreadStart(ListenForMessages));
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    private void ListenForMessages()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, listenPort);
        while (true)
        {
            byte[] data = udpClient.Receive(ref endPoint);
            string message = Encoding.UTF8.GetString(data);
            Debug.Log("Received message: " + message);
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
        listenerThread.Abort();
    }
}
