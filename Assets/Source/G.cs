using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
  public GameObject Prefab_Exit;
  public GameObject BackgroundObject;
  public GameObject HUD;
  
  public Sprite HudSelectedSprite;

  public TextAsset     MapFile;
  public Texture2D[]   Art;
  public static Dictionary<string, Sprite>      Sprites;
  public static Dictionary<int,   Sprite>       SpritesByTmx;
  public static Sprite[]      SpritesAll;
  public int Level;

  void Awake()
  {
    if (g != null)
    {
      Destroy(this);
      return;
    }

    GameObject.DontDestroyOnLoad(gameObject);
    g = this;
    
    Sprites = new Dictionary<string, Sprite>(Art.Length);
    SpritesByTmx = new Dictionary<int, Sprite>(Art.Length);
    SpritesAll = new Sprite[Art.Length];

    for(int i=0;i < Art.Length;i++)
    {
      Texture2D texture = Art[i];
      Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 70);
      sprite.name = texture.name;
      if (Sprites.ContainsKey(texture.name) == false)
      { 
        Sprites.Add(texture.name, sprite);
      }
      SpritesAll[i] = sprite;
    }

    LoadTiles();
    DrawHud();
  }

  void LoadTiles()
  {
    
    XmlReader xmlReader = XmlReader.Create(new StringReader(G.g.MapFile.text));
    
    int firstgid = 1;

    while (xmlReader.Read())
    {
      //scan object layer
      
      if (xmlReader.IsStartElement("tileset"))
      {
        firstgid = int.Parse(xmlReader.GetAttribute("firstgid"));
      }
      if (xmlReader.IsStartElement("tile"))
      {
        int id = int.Parse(xmlReader.GetAttribute("id"));
        xmlReader.ReadToDescendant("image");

        string path = xmlReader.GetAttribute("source");


        string name = path.Substring(path.LastIndexOf('/')) + 1;
        name = name.Substring(1, name.Length - 6);

        if (G.Sprites.ContainsKey(name))
        {
          G.SpritesByTmx.Add(id + firstgid, G.Sprites[name]);
        }
      }
    }

  }

  void DrawHud()
  {
    var hud = Instantiate(HUD);
    var tr = hud.transform;
    tr.position = new Vector3(10, 15f, 0);
    tr.localScale = new Vector3(30, 2.0f, 1.5f);
    DontDestroyOnLoad(hud);
  }

  public static Sprite PowerUpTypeToSprite(Type type)
  {
    if (type == typeof(Jump))
      return g.Prefab_PowerUp_Jump.GetComponent<SpriteRenderer>().sprite;
    else if (type == typeof(Gun))
      return g.Prefab_PowerUp_Gun.GetComponent<SpriteRenderer>().sprite;
    return SpritesAll[0];
  }
}
