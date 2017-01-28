using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Thing : MonoBehaviour
{

  public Texture2D WalkAnimation;
  
  public enum Mode
  {
    Still,
    Walking,
  }

  public SpriteRenderer   SpriteRenderer;
  public Animator         Animator;
  public CharacterController2D      CC;
  public bool             LeftFacing;
  public Mode             M;
  public bool             InputLeft, InputRight, InputUp, InputDown, InputFire;
  public Vector3          Velocity;
  public bool             IsPlayer;
  public bool             CanShoot;
  public bool             NoGravity;
  public bool             Shoot;
  public bool             ShootDir;
  public bool             ShootPlayer;
  public bool             DirectionLeft;
  public float            AirTimer;
  public bool             InAir;
  
	private Animator        _animator;
	private RaycastHit2D    _lastControllerColliderHit;
  
  void Start()
  {
    SpriteRenderer = GetComponent<SpriteRenderer>();
    Animator   = GetComponent<Animator>();
    CC         = GetComponent<CharacterController2D>();
    LeftFacing = false;

    if (Animator)
    { 
      Animator.StartPlayback();
    }
    
    CC.onTriggerEnterEvent += onTriggerEvent;
  }
  
  private void onTriggerEvent(Collider2D obj)
  {
    if (IsPlayer && obj.name.StartsWith("Pickup"))
    {
      Collect collect = obj.GetComponent<Collect>();
      Type pickupType = collect.Pickup.GetType();

      if (gameObject.GetComponent(pickupType) == null)
      {
        gameObject.AddComponent(pickupType);
        GameObject.Destroy(collect.gameObject);
      }
    }
    else if (IsPlayer && obj.name.StartsWith("Hostile"))
    {
      Death();
    }
    else if (obj.name.StartsWith("Bullet"))
    {
      Bullet bullet = obj.GetComponent<Bullet>();

      if (bullet.hitPlayer && IsPlayer)
        Death();
      else if (bullet.hitPlayer == false && !IsPlayer)
        Death();
    }
  }

  void Death()
  {
      GameObject.Destroy(gameObject);

    if (IsPlayer)
    {
         Scene scene = SceneManager.GetActiveScene(); 
          SceneManager.LoadScene(scene.name);
    }
  }

  void ChangeMode(Mode mode)
  {
    switch(mode)
    {
      case Mode.Still:
        break;
      case Mode.Walking:
        break;
    }
  }

  void Update()
  {
    if (CC)
    { 
      Vector3 move = new Vector3();

      if (InAir)
      {
        AirTimer += Time.deltaTime;
      }
      
      move.x = Velocity.x;
      move.y = Velocity.y;

      if (!InAir && Velocity.y > 0.0f)
      {
        // Jump
        InAir = true;
        AirTimer = 0.0f;
      }

      if (!NoGravity)
        move.y -= 200.0f * Time.deltaTime; // Gravity

      CC.move(move * Time.deltaTime);
    
      Velocity = Velocity * 0.25f;

      if (IsPlayer)
      {
        Player.Position = transform.position;
      }

      if (move.x < 0.0f)
      {
        DirectionLeft = true;
        SpriteRenderer.flipX = false;
      }
      else
      {
        DirectionLeft = false;
        SpriteRenderer.flipX = true;
      }

    }
  }
}
