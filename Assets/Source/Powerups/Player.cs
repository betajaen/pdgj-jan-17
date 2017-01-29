using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Player : PowerUp
{

  public static Vector2 Position;

  void Start()
  {
    hide = true;
  }

  void Update()
  {
      thing.InputLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
      thing.InputRight = Input.GetKey(KeyCode.D)  || Input.GetKey(KeyCode.RightArrow);
      thing.InputJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) ||  Input.GetKey(KeyCode.KeypadEnter);
      thing.InputPickup = Input.GetKeyUp(KeyCode.W)  || Input.GetKeyUp(KeyCode.UpArrow);
      thing.InputDrop = Input.GetKeyUp(KeyCode.S)  || Input.GetKeyUp(KeyCode.DownArrow);
      thing.InputNext = Input.GetKeyUp(KeyCode.Q)  || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);

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
