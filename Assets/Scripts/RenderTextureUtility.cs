using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureUtility
{
    public static Vector3 GetMousePosInWorldSpace(RectTransform displayTransform, Camera cam)
    {
        return GetWorldPositionInRenderTexture(displayTransform, cam, Input.mousePosition);
    }

    public static Vector3 GetWorldPositionInRenderTexture(RectTransform displayTransform, Camera cam, Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(displayTransform, position, null, out Vector2 localPos);

        Vector2 uv = Rect.PointToNormalized(displayTransform.rect, localPos);

        float px = uv.x * cam.targetTexture.width;
        float py = uv.y * cam.targetTexture.height;

        return cam.ScreenToWorldPoint(new Vector3(px, py, 0));
    }

    public static Vector3 GetRectPositionInRenderTexture(RectTransform displayTransform, Camera cam, Vector3 worldPos)
    {
        Vector3 rtPixel = cam.WorldToScreenPoint(worldPos);

        float u = rtPixel.x / cam.targetTexture.width;
        float v = rtPixel.y / cam.targetTexture.height;

        Rect r = displayTransform.rect;
        float localX = Mathf.Lerp(r.xMin, r.xMax, u);
        float localY = Mathf.Lerp(r.yMin, r.yMax, v);

        return RectTransformUtility.WorldToScreenPoint(null, displayTransform.TransformPoint(new Vector2(localX, localY)));
    }
}
