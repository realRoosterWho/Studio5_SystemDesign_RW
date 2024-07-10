using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(UDPSender))]
public class UDPSenderLogic : OdinEditor
{
    private string customMessage = "";

    public override void OnInspectorGUI()
    {
        // 使用 Odin Inspector 的方法绘制默认的 Inspector
        base.OnInspectorGUI();

        // 使用 Unity 原生的方法绘制自定义的部分
        UDPSender udpSender = (UDPSender)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom Message");
        customMessage = EditorGUILayout.TextField(customMessage);

        if (GUILayout.Button("Send Message"))
        {
            udpSender.SendMessage(customMessage);
        }

        if (GUILayout.Button("Send Default Image"))
        {
            udpSender.SendDefaultImage();
        }

        if (GUILayout.Button("Send Dictionary"))
        {
            udpSender.SendDefaultDictionary();
        }
    }
}