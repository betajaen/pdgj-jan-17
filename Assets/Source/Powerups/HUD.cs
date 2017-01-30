using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : PowerUp
{
  public int PowerUpIndex = 0;
  
  public List<PowerUpGlyph> PowerUpsGlyphs = new List<PowerUpGlyph>(4);
  public List<PowerUpGlyph> PowerUpPool    = new List<PowerUpGlyph>(4);
  public bool ReDrawNeeded = false;

  Glyph selectedGlyph;
  public PowerUp selected;

  void Start()
  {
    hide = true;
    thing.OnPowerUp += ThingOnOnPowerUp;
    thing.OnPowerDown += ThingOnOnPowerDown;
    ReDrawNeeded = true;
    selectedGlyph = Glyph.Create(0,0, G.g.HudSelectedSprite);
  }

  private void ThingOnOnPowerUp(PowerUp powerUp)
  {
    ReDrawNeeded = true;

    selected = powerUp;
  }
  
  private void ThingOnOnPowerDown(PowerUp powerUp)
  {
    ReDrawNeeded = true;

    if (selected == powerUp)
    {
      selected = null;
    }
  }

  public void Update()
  {

    if (thing.InputNext)
    {
      int s = -1;
      foreach (var kv in PowerUpsGlyphs)
      {
        if (s == 1)
        {
          selected = kv.PowerUp;
          s = 0;
          break;
        }

        if (selected == kv.PowerUp)
          s = 1;
      }

      if (s == 1 && thing.PowerUps.Count > 0)
      {
        selected = PowerUpsGlyphs[0].PowerUp;
      }
      
      ReDrawNeeded = true;
    }
    
    if (thing.InputDrop && selected)
    {
      Type type = selected.GetType();
      thing.RemovePowerUp(selected.GetType());
      
      Vector2 feet = thing.CC.middleFeet;

      PowerUp.CreateDropable(feet.x, feet.y, type);
      ReDrawNeeded = true;
    }

    if (ReDrawNeeded)
    {
      ReDrawNeeded = false;
      Redraw();
    }
  }

  public void Redraw()
  {
    foreach (var g in PowerUpsGlyphs)
    {
      g.Visible = false;
      g.PowerUp = null;
      PowerUpPool.Add(g);
    }

    PowerUpsGlyphs.Clear();

    int index = 0;
    selectedGlyph.Visible = false;

    foreach (var kv in thing.PowerUps)
    {
      var powerup = kv.Value;
      if (powerup.hide)
        continue;

      PowerUpGlyph glyph = GetPowerUpGlyph(index, 14.75f, powerup.GetType());
      glyph.PowerUp = kv.Value;
      
      if (selected == null)
      {
        selected = powerup;
      }

      if (selected == powerup)
      {
        selectedGlyph.Position = new Vector2(index, 14.75f);
        selectedGlyph.Visible = true;
      }
      
      PowerUpsGlyphs.Add(glyph);
      index++;

    }
  }

  PowerUpGlyph GetPowerUpGlyph(float x, float y, Type type)
  {

    Sprite theSprite = G.PowerUpTypeToSprite(type);


    if (PowerUpPool.Count > 0)
    {
      PowerUpGlyph g = PowerUpPool[PowerUpPool.Count - 1];
      PowerUpPool.RemoveAt(PowerUpPool.Count - 1);
      g.Position = new Vector2(x, y);
      g.Sprite = theSprite;
      g.Visible = true;
      return g;
    }

    return PowerUpGlyph.CreatePowerUpGlyph(x, y, theSprite);
  }

}
