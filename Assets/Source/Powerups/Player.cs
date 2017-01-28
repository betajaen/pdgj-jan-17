using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Player : PowerUp
{

  public static Vector2 Position;

  void Update()
  {
      thing.InputLeft = Input.GetKey(KeyCode.A);
      thing.InputRight = Input.GetKey(KeyCode.D);
      thing.InputUp = Input.GetKey(KeyCode.Space);
      thing.InputDown = Input.GetKey(KeyCode.S);

      if (thing.CanShoot && Input.GetKey(KeyCode.E))
      {
        thing.Shoot = true;
        thing.ShootDir = thing.DirectionLeft;
        thing.ShootPlayer = false;
      }
      else
      {
        thing.Shoot = false;
      }

  }
}
