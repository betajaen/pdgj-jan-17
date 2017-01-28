using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class G : MonoBehaviour
{
  public static G g;
  public static Bounds LevelBounds = new Bounds();
  public static float Gravity = -25.0f;

  public GameObject Prefab_Bullet;
  public GameObject Prefab_Snail;
  public GameObject Prefab_Player;
  public GameObject Prefab_PowerUp_Jump;
  public GameObject Prefab_PowerUp_Gun;
  
  public TextAsset     MapFile;
  public Texture2D[]   Art;
  public static Dictionary<string, Sprite>      Sprites;
  public static Dictionary<int,   Sprite>       SpritesByTmx;
  public static Sprite[]      SpritesAll;

  void Awake()
  {
    g = this;
    
    Sprites = new Dictionary<string, Sprite>(Art.Length);
    SpritesByTmx = new Dictionary<int, Sprite>(Art.Length);
    SpritesAll = new Sprite[Art.Length];

    for(int i=0;i < Art.Length;i++)
    {
      Texture2D texture = Art[i];
      Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 70);
      sprite.name = texture.name;
      Sprites.Add(texture.name, sprite);
      SpritesAll[i] = sprite;
    }

  }

}
