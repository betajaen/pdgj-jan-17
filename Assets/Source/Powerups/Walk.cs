using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : PowerUp
{
  
  public float Acceleration = 0.0f;

  void Update()
  {
    Vector3 m = new Vector3(0, 0);
    
    if (thing.InputLeft)
    {
      thing.Acceleration.x -= thing.MaxVelocity * 4.0f;
    }
    else if (thing.InputRight)
    {
      thing.Acceleration.x += thing.MaxVelocity * 4.0f;
    }

  }
}
