using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PixelPerfectCamera : MonoBehaviour
{
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

}
