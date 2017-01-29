using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Thing : MonoBehaviour
{

  const int kState_Idle = 0;
  const int kState_Walk = 1;
  const int kState_Fall = 2;
  const int kState_Jump = 3;
  
  public enum Mode
  {
    Still,
    Walking,
  }

  public SpriteRenderer   SpriteRenderer;
  public Animator         Animator;
  public SimpleAnimator   SimpleAnimator;
  public CharacterController2D      CC;
  public bool             LeftFacing;
  public Mode             M;
  public bool             InputLeft, InputRight, InputJump, InputFire, InputPickup, InputDrop, InputNext;
  public float            MaxVelocity = 4.0f;
  public Vector3          Acceleration, Velocity, Drag;
  public bool             IsPlayer;
  public bool             CanShoot;
  public bool             NoGravity;
  public bool             Shoot;
  public bool             ShootDir;
  public bool             ShootPlayer;
  public bool             DirectionLeft;
  public float            AirTimer;
  public int              State;
  public String[]         Animations;
  public Dictionary<Type, PowerUp>    PowerUps = new Dictionary<Type, PowerUp>(4); 
  public PowerUp          HoverPowerUp;
  
	public event Action<PowerUp> OnPowerUp;
  public event Action<PowerUp> OnPowerDown;

  void Start()
  {
    SpriteRenderer = GetComponent<SpriteRenderer>();
    Animator   = GetComponent<Animator>();
    SimpleAnimator  = GetComponent<SimpleAnimator>();
    CC         = GetComponent<CharacterController2D>();
    LeftFacing = false;

    SetState(kState_Idle);
    
    CC.onTriggerEnterEvent += onTriggerEvent;
    CC.onTriggerStayEvent  += onTriggerStayEvent;

    foreach(var powerUp in GetComponents<PowerUp>())
    {
      PowerUps.Add(powerUp.GetType(), powerUp);
    }

  }

  bool SetState(int s)
  {
    if (s != State)
    { 
      if (Animator)
      { 
        Animator.SetInteger("state", s);
      }
      else if (SimpleAnimator)
      {
        SimpleAnimator.speed = Mathf.Max(1.0f, (Velocity.x / MaxVelocity) * 8.0f);
        SimpleAnimator.PlayByIndex(s);
      }
      State = s;
      return true;
    }
    return false;
  }

  int GetState()
  {
    return State;
  }
  
  public bool AddPowerUp(Type type)
  {
    if (PowerUps.ContainsKey(type))
      return false;

    Component t = gameObject.AddComponent(type);
    PowerUps.Add(type, t as PowerUp);
    if (OnPowerUp != null)
    { 
      OnPowerUp(t as PowerUp);
    }
    return true;
  }

  public void RemovePowerUp(Type type)
  {
    if (PowerUps.ContainsKey(type))
    {
      PowerUp p = PowerUps[type];

      if (OnPowerDown != null)
      {
        OnPowerDown(p);
      }
      
      PowerUps.Remove(type);
      Destroy(p);
    }
  }

  private void onTriggerEvent(Collider2D obj)
  {
    if (IsPlayer && obj.name.StartsWith("Pickup"))
    {
      Collect collect = obj.GetComponent<Collect>();
      HoverPowerUp = collect.Pickup;
    }

    if (IsPlayer && obj.name.StartsWith("Hostile"))
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

  private void onTriggerStayEvent(Collider2D obj)
  {
    if (IsPlayer && obj.name.StartsWith("Pickup"))
    {
      Collect collect = obj.GetComponent<Collect>();
      HoverPowerUp = collect.Pickup;
    }
  }

  private void onTriggerLeaveEvent(Collider2D obj)
  {
    if (IsPlayer && obj.name.StartsWith("Pickup"))
    {
      HoverPowerUp = null;
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
  
  public bool OnGround()
  {
    return CC.isGrounded;
  }

  void Update()
  {
    if (IsPlayer && InputPickup && HoverPowerUp != null)
    {
      if (AddPowerUp(HoverPowerUp.GetType()))
      {
        GameObject.Destroy(HoverPowerUp.gameObject);
        HoverPowerUp = null;
      }
    }

    if (CC)
    { 
      
      Vector2 delta = ResolveMotion();

      if (IsPlayer)
      {
        Player.Position = transform.position;
      }
      
      if (OnGround() == false)
      {
        if (delta.y < 0.0f)
        {
          SetState(kState_Fall);
        }
        else
        {
          SetState(kState_Jump);
        }
      }
      else
      {
        if (Mathf.Abs(delta.x) > 0.01f)
        {
          SetState(kState_Walk);

          if (delta.x < 0.0f)
          {
            DirectionLeft = true;
            SpriteRenderer.flipX = true;
          }
          else
          {
            DirectionLeft = false;
            SpriteRenderer.flipX = false;
          }

        }
        else
        {
          SetState(kState_Idle);
        }
      }

    }
  }

  static float GetVelocity(float v, float a, float d, float mv)
  {
    if (Mathf.Abs(a) > Mathf.Epsilon)
    {
      v += a * Time.deltaTime;
    }
    else if (d > 0.0f)
    {
      float dd = d * Time.deltaTime;
      if (v - dd > 0.0f)
        v -= dd;
      else if (v + dd < 0.0f)
        v += dd;
      else
        v = 0.0f;
    }

    if (v > mv)
      v = mv;
    else if (v < -mv)
      v = -mv;

    return v;
  }

  Vector2 ResolveMotion()
  {
    Vector2 delta, move;
    delta.x = (GetVelocity(Velocity.x, Acceleration.x, Drag.x, MaxVelocity) - Velocity.x) * 0.5f;
    Velocity.x += delta.x;
    move.x = Velocity.x * Time.deltaTime;
    Velocity.x += delta.x;

    if (CC.isGrounded == false && !NoGravity)
    {
      Acceleration.y += G.Gravity;
    }
    
    delta.y = (GetVelocity(Velocity.y, Acceleration.y, Drag.y, MaxVelocity) - Velocity.y) * 0.5f;
    Velocity.y += delta.y;
    move.y = Velocity.y * Time.deltaTime;
    Velocity.y += delta.y;

    Vector2 p = transform.position;

    CC.move(move);
      
    Vector2 p2 = transform.position;

    Acceleration.x = 0.0f;
    Acceleration.y = 0.0f;

    return p2 - p;
  }

}
