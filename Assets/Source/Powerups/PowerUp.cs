using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
  public Thing  thing;
  public bool   hide;
  public Sprite sprite;
  
  public void Awake()
  {
    thing = GetComponent<Thing>();
    hide = false;
  }

  public static void CreateDropable(float x, float y, Type type)
  {
    if (type == typeof(Gun))
    {
      Instantiate(G.g.Prefab_PowerUp_Gun, new Vector3(x, y, 0), Quaternion.identity);
    }
    else if (type == typeof(Jump))
    {
      Instantiate(G.g.Prefab_PowerUp_Jump, new Vector3(x, y, 0), Quaternion.identity);
    }
    else
    {
      Debug.LogFormat("Unknown Type! {0}", type);
    }
  }

}
