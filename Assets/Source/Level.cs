﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class Level : MonoBehaviour
{
  public GameObject Bullet;
  public GameObject Snail;
  public GameObject Player;
  public GameObject PowerUp_Jump;
  public GameObject PowerUp_Gun;

  public static Level  Ptr;

  public int           SpriteWidth, SpriteHeight;
  public TextAsset     File;
  public Texture2D[]   Art;
  public Dictionary<string, Sprite>      Sprites;
  public Dictionary<int,   Sprite>       SpritesByTmx;
  public Sprite[]      SpritesAll;
  public bool          Dirty;
  public List<Tile>    Tiles;

  public void Awake()
  {
    Ptr = this;
  }

  public void Start()
  {
    Dirty = true;

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
    foreach(var tile in Tiles)
    {
      GameObject.Destroy(tile.gameObject);
    }
  }
  
  void SpawnObject(int x, int y, GameObject t)
  {
    GameObject go = GameObject.Instantiate(t);
    go.transform.position = new Vector3(x, y);
  }

  void Add(int X, int Y, Sprite sprite)
  {
    if (sprite.name == "snailWalk1")
    {
      SpawnObject(X, Y, Snail);
      return;
    }
    
    if (sprite.name == "p2_front")
    {
      SpawnObject(X, Y, Player);
      return;
    }
    
    if (sprite.name == "spring")
    {
      SpawnObject(X, Y, PowerUp_Jump);
      return;
    }
    
    if (sprite.name == "PewPewPew")
    {
      SpawnObject(X, Y, PowerUp_Gun);
      return;
    }

    GameObject go = new GameObject();
    go.layer = LayerMask.NameToLayer("Platform");
    var tr = go.transform;
    tr.parent = transform;

    var tile = go.AddComponent<Tile>();

    tr.localPosition = new Vector2(X , Y);
    var sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = sprite;

    go.AddComponent<BoxCollider2D>();
    

  }

  void Draw()
  {
    Clear();

    XmlReader xmlReader = XmlReader.Create(new StringReader(File.text));
 
    int _width = 0, _height = 0;

    while (xmlReader.Read()) {
            //scan map size
            if (xmlReader.IsStartElement("map")) {
                _width = int.Parse (xmlReader.GetAttribute("width"));
                _height = int.Parse (xmlReader.GetAttribute("height"));
            }
            //scan object layer
            if (xmlReader.IsStartElement("object")) {
                int x = int.Parse (xmlReader.GetAttribute("x"));
                int y = int.Parse (xmlReader.GetAttribute("y"));
                int gid = int.Parse (xmlReader.GetAttribute("gid"));
                string name = xmlReader.GetAttribute ("name");
                //CreateTile(x, y, gid, name);
            }
            //scan object layer
            if (xmlReader.IsStartElement("tile")) {
                int id = int.Parse (xmlReader.GetAttribute("id"));
                xmlReader.ReadToDescendant("image");

                string path = xmlReader.GetAttribute("source");
                
        
                string name = path.Substring(path.LastIndexOf('/')) + 1;
                name = name.Substring(1, name.Length - 6);
                
        if (Sprites.ContainsKey(name))
        { 
                SpritesByTmx.Add(id, Sprites[name]);
        }
            }
            //scan tile data layer
            if (xmlReader.IsStartElement("data")) {
                string data = xmlReader.ReadInnerXml();
                string[] lines = data.Split ('\n');
                int height = lines.Length - 2; //removes additional empty line
                for (int j=1; j<height+1; j++) {
                    string line = lines[j];
                    string[] cols = line.Split (',');
                    int width = cols.Length - 1;
                    for (int i=0; i<width+1; i++) {
                        int tile = 0;
                        if (int.TryParse (cols[i], out tile)) {

                            if (tile == 0)
                              continue;
                            

                            if (SpritesByTmx.ContainsKey(tile) == true)
                            {
                              Add(i, _height - j, SpritesByTmx[tile - 1]);
                            }
                            else
                            {
                              Debug.LogWarningFormat("{0} {1} is wrong {2}", i, j, tile);
                            }

                            //CreateTile(i, _height - j, tile, "");
                        }
                    }
                }
            }
        }
    }
  

}