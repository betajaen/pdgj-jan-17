using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PixelPerfectCamera : MonoBehaviour
{
  Texture2D texture;
  void Start()
  {
    texture = new Texture2D(1, 1);
    texture.SetPixel(0,0, Color.black);
    texture.Apply();

  }
  void DrawQuad(Rect position)
  {
    GUI.DrawTexture(position, texture);
  }

  void LateUpdate()
  {
    float screenRatio = (float)Screen.width / (float)Screen.height;
    float targetRatio = G.LevelBounds.size.x /G.LevelBounds.size.y;
 
    if (screenRatio >= targetRatio)
    {
      Camera.main.orthographicSize = G.LevelBounds.size.y / 2;
    }
    else
    {
      float differenceInSize = targetRatio / screenRatio;
      Camera.main.orthographicSize = G.LevelBounds.size.y / 2 * differenceInSize;
    }
 
    transform.position = new Vector3(G.LevelBounds.center.x, G.LevelBounds.center.y, -1f);
  }

  void OnGUI()
  {
    Vector2 l = G.LevelBounds.min;
    Vector3 screenPos = Camera.main.WorldToScreenPoint(l);

    Rect r = new Rect(0, 0, screenPos.x, Screen.height);
    DrawQuad(r);
    l = G.LevelBounds.max;
    screenPos = Camera.main.WorldToScreenPoint(l);
    r = new Rect(screenPos.x, 0, Screen.width - screenPos.x, Screen.height);
    DrawQuad(r);
  }

}
