using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
  public Thing thing;

  public void Awake()
  {
    thing = GetComponent<Thing>();
  }
}
