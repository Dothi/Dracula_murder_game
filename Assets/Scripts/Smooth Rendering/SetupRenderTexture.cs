using UnityEngine;
using System.Collections;

/// <summary>
/// Sets up a render texture and instructs the main camera to use it.
/// </summary>
public class SetupRenderTexture : MonoBehaviour
{
    [SerializeField]
    private int antialias;

    private void OnEnable()
    {
        // create render texture double the width/height of the screen
        RenderTexture renderTexture = new RenderTexture(Screen.width * 2,
                                                        Screen.height * 2,
                                                        0, 
                                                        RenderTextureFormat.ARGB32);
        renderTexture.antiAliasing = antialias;
        renderTexture.Create();

        // instruct main camera to render to texture
        Camera.main.targetTexture = renderTexture;

        // activate camera that blits render texture to screen
        GetComponent<Camera>().enabled = true;
    }
}
