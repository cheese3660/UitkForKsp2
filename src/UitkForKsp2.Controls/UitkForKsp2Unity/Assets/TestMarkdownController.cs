using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UitkForKsp2.Controls;
using UitkForKsp2.Controls.MarkdownRenderer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

public class TestMarkdownController : MonoBehaviour
{

    private IEnumerator ImageHandlerCoroutine(string link, Image image)
    {
        var trueLink = $"https://{link}";
        var request = UnityWebRequestTexture.GetTexture(trueLink);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            image.image = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
    
    private void UnityWebRequestImageHandler(string link, Image image)
    {
        // var trueLink = $"https://{link}";
        // SerialLogger.Log($"True link {trueLink}");
        // UnityWebRequest request = UnityWebRequestTexture.GetTexture(trueLink);
        // var handle = request.SendWebRequest();
        // long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 5000;
        // while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < milliseconds && !handle.isDone)
        // {
        //     // Spin
        // }
        //
        // if (handle.isDone && request.result == UnityWebRequest.Result.Success)
        // {
        //     return ((DownloadHandlerTexture)request.downloadHandler).texture;
        // }
        // else
        // {
        //     return null;
        // }
        StartCoroutine(ImageHandlerCoroutine(link, image));
    }
    
    public UIDocument Document;
    // Start is called before the first frame update
    void Start()
    {
        MarkdownApi.RegisterMarkdownImageHandler("https",UnityWebRequestImageHandler);
        var md = Document.rootVisualElement.Q<MarkdownElement>();
        Document.rootVisualElement.Q<DropdownField>()
            .RegisterValueChangedCallback(evt => md.Markdown = File.ReadAllText($"Assets/{evt.newValue}.md"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
