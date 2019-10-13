using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player's proximity handler
[RequireComponent(typeof(CircleCollider2D))]
public class ProximityHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer ImageComponent = null;
    [SerializeField] Sprite NormalSprite = null;
    [SerializeField] Sprite JojoSprite = null;
    [SerializeField] Sprite JustAteSprite = null;
    [SerializeField] Sprite GoingToEat = null;
    [SerializeField] float tempFaceDuration = 1f;
    [SerializeField] Entity This;

    public NaniFX naniFX;
    public SweatFX sweatFX;
    public VeinFX veinFX;

    private float LastDashTime; // todo generify the abilityManager
    private State state = State.NORMAL;

    enum State
    {
        NORMAL,
        GOING_TO_EAT,
        JUST_ATE,
        JOJO
    }

    private void Start()
    {
        if(This == null) This = GetComponentInParent<Entity>();
        LastDashTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == State.GOING_TO_EAT || state == State.JOJO)
        {
            ImageComponent.sprite = NormalSprite;
            state = State.NORMAL;
        }

        if (This.IsPlayer)
        {
            // Disable all effect
            naniFX.Hide();
            sweatFX.Hide();
            veinFX.Hide();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (state == State.NORMAL || state == State.JOJO)
        {
            var otherEntity = other.gameObject.GetComponent<Entity>();
            if (otherEntity == null) return;

            // can eat it
            if (This.IsPlayer && This.Size - otherEntity.Size > This.MiddleSizeThreshold)
            {
                ImageComponent.sprite = GoingToEat;
                state = State.GOING_TO_EAT;
            }
            // gonna be et
            else if (otherEntity.IsPlayer && This.Size - otherEntity.Size < -This.MiddleSizeThreshold)
            {
                Debug.Log("*********** Hit");
                ImageComponent.sprite = GoingToEat; // this is "going to be eaten" for non-players
                state = State.GOING_TO_EAT;
            }
            // gonna be et (for player)
            else if (This.IsPlayer && This.Size - otherEntity.Size < -This.MiddleSizeThreshold && otherEntity.StartingStats.Type != EnemyType.CANDY)
            {
                Debug.Log("Player close to being eaten");
                sweatFX.Show();
                veinFX.Show();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherEntity = other.gameObject.GetComponent<Entity>();
        if (otherEntity == null) return;
        if (This.StartingStats.Type == EnemyType.BAT && otherEntity.IsPlayer)
        {
            // too small, run away!
            if (This.Size - otherEntity.Size < -This.MiddleSizeThreshold)
            {
                if (LastDashTime + This.DashCooldown < Time.time)
                {
                    LastDashTime = Time.time;
                    StartCoroutine(RunAway(other, Time.time));
                }
            }
        }
    }
  
    IEnumerator RunAway(Collider2D other, float initialTime)
    {
        Debug.Log("Run away");
        // Gets a vector that points from the player's position to the target's.
        var heading = other.transform.position - transform.parent.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        This.GoalPosition = -(direction * 6); // arbitrary number

        var temp = This.Speed;
        var increaseSpeed = This.Speed * This.DashSpeedBonusMultiplier;
        while (Time.time < initialTime + This.DashDuration)
        {
            This.Speed = increaseSpeed;
            yield return null;
        }

        This.Speed = temp;
        //This.transform.position = -Vector2.MoveTowards(transform.position, other.gameObject.transform.position, 0.01f);
    }

    public void SetJustAteFace()
    {
        ImageComponent.sprite = JustAteSprite;
        state = State.JUST_ATE;
        StartCoroutine(SetJustAteFace(Time.time));
    }

    public void SetJojoFace()
    {
        ImageComponent.sprite = JojoSprite;
        state = State.JOJO;
        StartCoroutine(SetJojoFace(Time.time));
        naniFX.Show();
    }

    public IEnumerator SetJustAteFace(float initialTime)
    {
        while (Time.time < initialTime + tempFaceDuration)
        {
            yield return null;
        }

        ImageComponent.sprite = NormalSprite;
        state = State.NORMAL;
    }

    // so much bad practice >.< // todo clean
    public IEnumerator SetJojoFace(float initialTime)
    {
        while (Time.time < initialTime + tempFaceDuration)
        {
            yield return null;
        }

        ImageComponent.sprite = NormalSprite;
        state = State.NORMAL;
    }
}
