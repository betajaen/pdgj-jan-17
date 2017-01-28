using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Gun : PowerUp
{
  public float maxTime = 0.25f;
  float timer = 0.0f;

  public void Update()
  {
    thing.CanShoot = true;
    
    timer -= UnityEngine.Time.deltaTime;

    if (thing.Shoot && timer <= 0.0f)
    { 
      Bullet.Shoot(thing.ShootDir, thing.ShootPlayer, transform.position);
      timer = maxTime;
    }
  }
}
