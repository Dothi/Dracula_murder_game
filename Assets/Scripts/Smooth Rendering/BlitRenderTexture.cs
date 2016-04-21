using UnityEngine;
using System.Collections;

/// <summary>
/// Blits the main camera's render texture directly to the screen.
/// </summary>
public class BlitRenderTexture : MonoBehaviour
{
    private Camera mainCamera;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        // blit render texture directly to screen
        Graphics.Blit(mainCamera.targetTexture, (RenderTexture)null);
    }
}
