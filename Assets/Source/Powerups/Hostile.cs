using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Hostile : PowerUp
{
  public Vector2 d;
  public Vector2 p;
  public float   maxDistance = 7.0f;
  public float   shootDistance = 3.0f;

  public float   distance = 0.0f;
  public float   maxShootTimer = 0.25f;

  public bool left, right;

  public float shootTimer = 0.0f;

  void Start()
  {
    hide = true;
    Roll();
  }

  void Roll()
  {
    if (UnityEngine.Random.Range(0, 2) == 0)
    {
      left = true;
      right = false;
    }
    else
    {
      left = false;
      right = true;
    }
  }

  void Update()
  {
    thing.Shoot = false;
    shootTimer -= Time.deltaTime;

    Vector2 m = transform.position;
    p = Player.Position;
    d = (Player.Position - m);
    distance = d.magnitude;

    if (distance < maxDistance)
    { 
      thing.InputLeft  = false;
      thing.InputRight = false;

      if (d.x > 0.0f)
        thing.InputRight = true;
      else if (d.x < 0.0f)
        thing.InputLeft = true;

      left = thing.InputLeft;
      right = thing.InputRight;

      if (distance < shootDistance)
      {
        thing.Shoot = true;
        thing.ShootDir = (left);
        thing.ShootPlayer = true;
      }
      

    }
    else
    {
      if (UnityEngine.Random.Range(1, 100) == 1)
        Roll();

      thing.InputLeft = left;
      thing.InputRight = right;
    }
  }

}
