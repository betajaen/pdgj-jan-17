using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Jump : PowerUp
{
  
  public float WalkSpeed = 100.0f;

  void Update()
  {
    Vector3 m = new Vector3(0, 0);
    
    if (thing.InputUp && thing.OnGround())
    { 
      thing.Velocity.y += thing.MaxVelocity * 2.0f;
    }

  }

}
