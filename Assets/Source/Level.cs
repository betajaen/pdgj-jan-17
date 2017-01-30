using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Level : MonoBehaviour
{
  public int SpriteWidth, SpriteHeight;
  public bool Dirty;
  public List<Tile> Tiles;

  public void Start()
  {
    Dirty = true;
  }

  public void Update()
  {
    if (Dirty)
    {
      Dirty = false;
      Draw();
    }
  }

  void Clear()
  {
    foreach (var tile in Tiles)
    {
      GameObject.Destroy(tile.gameObject);
    }
  }

  void SpawnObject(float x, float y, GameObject t)
  {
    GameObject go = GameObject.Instantiate(t);
    go.transform.position = new Vector3(x, y);
  }
  
  void SpawnBackground(float x, float y, float w, float h, int gid, int layer)
  {

    GameObject go = Instantiate(G.g.BackgroundObject);
    go.name = gid.ToString();
    var sr = go.GetComponent<SpriteRenderer>();
    sr.sprite = G.SpritesByTmx[gid];

    float ww = sr.sprite.rect.width;
    float hh = sr.sprite.rect.height;
    
    go.transform.position = new Vector3(-0.5f + x + ww / 70.0f / 2.0f, -0.5f + y + hh / 70.0f / 2.0f, 1 + layer);
    
    sr.color = Color.white;
    
    Debug.LogFormat("{0} {1} {2} {3}", w, h, ww, hh);
    go.transform.localScale = new Vector3(w / ww, h / hh, 1.0f);
  }

  void Add(int X, int Y, Sprite sprite)
  {
    GameObject go = new GameObject();
    go.name = sprite.name;
    go.layer = LayerMask.NameToLayer("Platform");
    var tr = go.transform;
    tr.parent = transform;

    var tile = go.AddComponent<Tile>();

    tr.localPosition = new Vector2(X, Y);
    var sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = sprite;

    var col = go.AddComponent<BoxCollider2D>();

    G.LevelBounds.Encapsulate(col.bounds);
  }

  void Draw()
  {
    Clear();



    XmlReader xmlReader = XmlReader.Create(new StringReader(G.g.MapFile.text));

    int _width = 0, _height = 0;

    G.LevelBounds = new Bounds();
    G.LevelBounds.Encapsulate(new Vector3(0, 15.5f, 0));
    G.LevelBounds.Encapsulate(new Vector3(19, 0.0f, 0));

    bool read = false;
    bool background = false;
    int layer = 0;
    string namePrefix = G.g.Level.ToString() + "_";
    int firstgid = 1;

    while (xmlReader.Read())
    {
      //scan map size
      if (xmlReader.IsStartElement("objectgroup"))
      {
        string n = xmlReader.GetAttribute("name");
        read = n.StartsWith(namePrefix);
        background = n.Contains("Layer");
        if (background)
        {
          layer = int.Parse(n.Substring(n.LastIndexOf("r") + 1));
          Debug.Log(layer);
        }
      }

      if (xmlReader.IsStartElement("layer"))
      {
        background = false;
        read = xmlReader.GetAttribute("name").StartsWith(namePrefix);
      }

      if (xmlReader.IsStartElement("map"))
      {
        _width = int.Parse(xmlReader.GetAttribute("width"));
        _height = int.Parse(xmlReader.GetAttribute("height"));
      }
      //scan object layer
      if (xmlReader.IsStartElement("object") && read)
      {
        float mx = float.Parse(xmlReader.GetAttribute("x")) / 70.0f;
        float my = float.Parse(xmlReader.GetAttribute("y")) / 70.0f;

        float x = mx;
        float y = (_height - my);
        
        string name = xmlReader.GetAttribute("name");
        
        if (background)
        {
          int gid = int.Parse(xmlReader.GetAttribute("gid"));
          float w = float.Parse(xmlReader.GetAttribute("width"));
          float h = float.Parse(xmlReader.GetAttribute("height"));

          SpawnBackground(x, y, w, h, gid, layer);
        }
        else
        {
          if (name == "Gun")
          {
            SpawnObject(x, y, G.g.Prefab_PowerUp_Gun);
          }
          else if (name == "Jump")
          {
            SpawnObject(x, y, G.g.Prefab_PowerUp_Jump);
          }
          else if (name == "Player")
          {
            SpawnObject(x, y, G.g.Prefab_Player);
          }
          else if (name == "Snail")
          {
            SpawnObject(x, y, G.g.Prefab_Snail);
          }
          else if (name == "Exit")
          {
            SpawnObject(x, y, G.g.Prefab_Exit);
          }
        }
        
      }
//      if (xmlReader.IsStartElement("tileset"))
//      {
//        firstgid = int.Parse(xmlReader.GetAttribute("firstgid"));
//      }
//      //scan object layer
//      if (xmlReader.IsStartElement("tile"))
//      {
//        int id = int.Parse(xmlReader.GetAttribute("id"));
//        xmlReader.ReadToDescendant("image");
//
//        string path = xmlReader.GetAttribute("source");
//
//
//        string name = path.Substring(path.LastIndexOf('/')) + 1;
//        name = name.Substring(1, name.Length - 6);
//
//        if (G.Sprites.ContainsKey(name))
//        {
//          G.SpritesByTmx.Add(id + firstgid, G.Sprites[name]);
//        }
//      }
      //scan tile data layer
      if (xmlReader.IsStartElement("data") && read)
      {
        string data = xmlReader.ReadInnerXml();
        string[] lines = data.Split('\n');
        int height = lines.Length - 2; //removes additional empty line
        for (int j = 1; j < height + 1; j++)
        {
          string line = lines[j];
          string[] cols = line.Split(',');
          int width = cols.Length - 1;
          for (int i = 0; i < width + 1; i++)
          {
            int tile = 0;
            if (int.TryParse(cols[i], out tile))
            {
              if (tile == 0)
                continue;
              
              if (G.SpritesByTmx.ContainsKey(tile) == true)
              {
                Add(i, _height - j, G.SpritesByTmx[tile]);
              }
              else
              {
                Debug.LogWarningFormat("{0} {1} is wrong {2}", i, j, tile - 1);
              }
              
            }
          }
        }
      }
    }
  }


}
