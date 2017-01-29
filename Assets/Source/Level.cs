using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

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

  void Add(int X, int Y, Sprite sprite)
  {
    if (sprite.name == "snailWalk1")
    {
      SpawnObject(X, Y, G.g.Prefab_Snail);
      return;
    }

    if (sprite.name == "p2_front")
    {
      SpawnObject(X, Y, G.g.Prefab_Player);
      return;
    }

    if (sprite.name == "spring")
    {
      SpawnObject(X, Y, G.g.Prefab_PowerUp_Jump);
      return;
    }

    if (sprite.name == "raygun")
    {
      SpawnObject(X, Y, G.g.Prefab_PowerUp_Gun);
      return;
    }

    GameObject go = new GameObject();
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
    while (xmlReader.Read())
    {
      //scan map size
      if (xmlReader.IsStartElement("map"))
      {
        _width = int.Parse(xmlReader.GetAttribute("width"));
        _height = int.Parse(xmlReader.GetAttribute("height"));
      }
      //scan object layer
      if (xmlReader.IsStartElement("object"))
      {
        float x = float.Parse(xmlReader.GetAttribute("x")) / 70.0f;
        float y = _height - (float.Parse(xmlReader.GetAttribute("y")) / 70.0f);

        string name = xmlReader.GetAttribute("name");
        
        Debug.Log(name);

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

        continue;
      }
      //scan object layer
      if (xmlReader.IsStartElement("tile"))
      {
        int id = int.Parse(xmlReader.GetAttribute("id"));
        xmlReader.ReadToDescendant("image");

        string path = xmlReader.GetAttribute("source");


        string name = path.Substring(path.LastIndexOf('/')) + 1;
        name = name.Substring(1, name.Length - 6);

        if (G.Sprites.ContainsKey(name))
        {
          G.SpritesByTmx.Add(id, G.Sprites[name]);
        }
      }
      //scan tile data layer
      if (xmlReader.IsStartElement("data"))
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
                Add(i, _height - j, G.SpritesByTmx[tile - 1]);
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
