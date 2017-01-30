using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Glyph : MonoBehaviour
{
  SpriteRenderer sr;
  Transform tr;

  public Sprite Sprite
  {
    get { return sr.sprite; }
    set { sr.sprite = value; }
  }

  public bool Visible
  {
    get { return gameObject.activeSelf; }
    set { gameObject.SetActive(value); }
  }

  public float X
  {
    get { return tr.position.x; }
    set { Vector3 v = tr.position; v.x = value; tr.position = v; }
  }
  
  public float Y
  {
    get { return tr.position.y; }
    set { Vector3 v = tr.position; v.y = value; tr.position = v; }
  }

  public Vector2 Position
  {
    get { return tr.position; }
    set { tr.position = value; }
  }

  void Awake()
  {
    tr = transform;
    sr = GetComponent<SpriteRenderer>();
  }

  public static Glyph Create(int X, int Y, Sprite sprite)
  {
    GameObject go = new GameObject();
    go.layer = LayerMask.NameToLayer("HUD");
    var tr = go.transform;
    
    tr.localPosition = new Vector2(X, Y);
    var sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = sprite;
    
    return go.AddComponent<Glyph>();
  }
}


public class PowerUpGlyph : Glyph
{
  public PowerUp PowerUp;

  public static PowerUpGlyph CreatePowerUpGlyph(float X, float Y, Sprite sprite)
  {
    GameObject go = new GameObject();
    go.layer = LayerMask.NameToLayer("HUD");
    var tr = go.transform;
    
    tr.localPosition = new Vector2(X, Y);
    var sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = sprite;

    return go.AddComponent<PowerUpGlyph>();
  }
}
