using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Bullet : PowerUp
{
  public bool left;
  public bool hitPlayer;
  float timer = 0.0f;

  public float maxTimer = 1.0f;
  
  void Update()
  {
    timer += Time.deltaTime;
    if (timer >= maxTimer)
    {
      GameObject.Destroy(gameObject);
    }

    if (left)
      thing.InputLeft = true;
    else
      thing.InputRight = true;
  }

  
  public static void Shoot(bool shootLeft, bool shootPlayer, Vector3 position)
  {
    GameObject bullet = GameObject.Instantiate(G.g.Prefab_Bullet, position, Quaternion.identity);
    Bullet b = bullet.gameObject.GetComponent<Bullet>();
    b.hitPlayer = shootPlayer;
    b.left = shootLeft;
  }


}
