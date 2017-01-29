using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Jump : PowerUp
{
  
  public float WalkSpeed = 100.0f;

  void Start()
  {
    sprite = G.g.Prefab_PowerUp_Jump.GetComponent<SpriteRenderer>().sprite;
  }

  void Update()
  {
    Vector3 m = new Vector3(0, 0);
    
    if (thing.InputJump && thing.OnGround())
    { 
      thing.Velocity.y += thing.MaxVelocity * 2.0f;
    }

  }

}
