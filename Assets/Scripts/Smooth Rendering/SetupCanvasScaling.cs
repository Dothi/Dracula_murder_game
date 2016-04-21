using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// Modifies all CanvasScalers in the scene to double their scale factors.
/// </summary>
public class SetupCanvasScaling : MonoBehaviour
{
    private void OnEnable()
    {
        CanvasScaler[] canvasScalers = FindObjectsOfType<CanvasScaler>();
        for (int i = 0; i < canvasScalers.Length; i++)
        {
            CanvasScaler canvasScaler = canvasScalers[i];
            switch (canvasScaler.uiScaleMode)
            {
                case CanvasScaler.ScaleMode.ConstantPixelSize:
                    canvasScaler.scaleFactor *= 2f;
                    break;
                case CanvasScaler.ScaleMode.ConstantPhysicalSize:
                    Debug.LogError("cannot use " + typeof(SetupCanvasScaling).Name +
                        " script with a Canvas in 'constant physical size' mode");
                    break;
                case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                    canvasScaler.referenceResolution /= 2f;
                    break;
            }
        }

        Camera camera = GetComponent<Camera>();
        MyGraphicRaycaster[] graphicRaycasters = FindObjectsOfType<MyGraphicRaycaster>();
        for (int i = 0; i < graphicRaycasters.Length; i++)
        {
            graphicRaycasters[i].raycastCamera = camera;
        }
    }
}

/// <summary>
/// A graphic raycaster that works like the original GraphicRaycaster,
/// but uses a different camera.
/// </summary>
public class MyGraphicRaycaster : GraphicRaycaster
{
    [NonSerialized]
    public Camera raycastCamera;

    public override Camera eventCamera
    {
        get
        {
            return raycastCamera;
        }
    }
}