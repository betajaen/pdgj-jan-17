﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Collect : PowerUp
{
  public PowerUp Pickup;
  
  void Awake()
  {
    gameObject.tag = "Pickup";
  }

}
