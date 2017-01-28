using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : PowerUp
{

  public float WalkSpeed = 10.0f;

  void Update()
  {
    Vector3 m = new Vector3(0, 0);
    
    if (thing.InputLeft)
      m.x -= WalkSpeed;
    else if (thing.InputRight)
      m.x += WalkSpeed;

    thing.Velocity += m;

  }
}
