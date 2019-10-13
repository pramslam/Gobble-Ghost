using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public StatBlock StartingStats;
    public float MiddleSizeThreshold; // player only. Sorry this code is kinda scrambled and bad

    public ProximityHandler ProxHandler = null;

    [HideInInspector] public bool IsPlayer = false;
    [HideInInspector] public float Size = 1.0f;
    [HideInInspector] public float Agility = 1.0f;
    [HideInInspector] public float Speed = 1.0f;
    [HideInInspector] public float Luminosity = 1.0f;
    [HideInInspector] public List<Ability> Abilities;

    [HideInInspector] public float DashDuration = 0.5f;
    [HideInInspector] public float DashCooldown = 2f;
    [HideInInspector] public float DashSpeedBonusMultiplier = 0.0f;

    public delegate void StatAddedDelegate();
    public StatAddedDelegate OnStatAdded;

    public delegate void OnConsumeDelegate(EnemyType type);
    public OnConsumeDelegate OnConsume;

    float Timer = 0.0f;
    [HideInInspector] public Vector2 GoalPosition;
    [HideInInspector] public bool fleeing = false;

    void Start()
    {
        if (StartingStats == null) Debug.LogError(name + " has no Statblock! Is it assigned in the inspector?");
        IsPlayer = StartingStats.IsPlayer;
        Size = StartingStats.Size;
        Agility = StartingStats.Agility;
        Speed = StartingStats.Speed;
        Luminosity = StartingStats.Luminosity;
        Abilities = StartingStats.Abilities;
        DashDuration = StartingStats.DashDuration;
        DashCooldown = StartingStats.DashCooldown;
        DashSpeedBonusMultiplier = StartingStats.DashSpeedBonusMultiplier;

        if (!IsPlayer)
        {
            Size = SetRandomSize();
        }

        if(StartingStats.Type == EnemyType.SLIME) transform.parent.localScale = new Vector3(Size, Size, 1f);
        else transform.localScale = new Vector3(Size, Size, 1f);
    }

    private void OnDestroy()
    {
        StartingStats.OnConsumed();
    }

    //--------------------------------------

    void LateUpdate()
    {
        if (!IsPlayer && Speed > 0) MoveEntity();
    }
    //--------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherEntity = other.GetComponent<Entity>();
        if (otherEntity == null || other.tag == "Proximity") return; // we don't care

        if (otherEntity.IsPlayer)
        {
            if (otherEntity.Size - Size > otherEntity.MiddleSizeThreshold)
            {
                if (otherEntity.ProxHandler != null) otherEntity.ProxHandler.SetJustAteFace();
                Debug.Log(name + " getting eaten. \nSize: " + Size + " Player Size: " + otherEntity.Size);
                foreach (var effect in StartingStats.ConsumeEffects)
                {
                    ApplyEffect(effect, otherEntity, other.gameObject);
                    otherEntity.OnStatAdded();
                }
                AudioManager.instance.PlayEating(otherEntity.GetComponent<AudioSource>());
                otherEntity.OnConsume(StartingStats.Type);
                if (!IsPlayer) Destroy(transform.parent.gameObject);
                else Destroy(gameObject);
            }
            else if (otherEntity.Size - Size <= otherEntity.MiddleSizeThreshold && otherEntity.Size - Size >= -otherEntity.MiddleSizeThreshold)
            {
                if (otherEntity.ProxHandler != null) otherEntity.ProxHandler.SetJojoFace();
            }
            else
            {
                if (StartingStats.Type != EnemyType.CANDY) // candy no do harm
                {
                    SceneChanger.EnterScene("StartMenu");
                    Debug.Log("Player was consumed by " + name +". \nPlayer size: " + otherEntity.Size + " Other size: " + Size);
                }
            }
        }
    }

    private void ApplyEffect(StatBlock.Reward effect, Entity otherEntity, GameObject other)
    {
        if (effect.Type == StatBlock.RewardType.ABILITY && !otherEntity.Abilities.Contains(effect.AbilityToBeRewarded))
        {
            otherEntity.Abilities.Add(effect.AbilityToBeRewarded);
        }
        else if (effect.Type == StatBlock.RewardType.STAT_MODIFIER)
        {
            switch (effect.StatToBeModified)
            {
                case (Stat.AGILITY):
                    otherEntity.Agility += effect.Modifier;
                    break;
                case (Stat.LIGHT):
                    otherEntity.Luminosity += effect.Modifier;
                    break;
                case (Stat.SPEED):
                    otherEntity.Speed += effect.Modifier;
                    break;
                case (Stat.SIZE):
                    otherEntity.Size += effect.Modifier;
                    if (!otherEntity.IsPlayer) other.transform.parent.localScale += new Vector3(effect.Modifier, effect.Modifier, 0f);
                    else other.transform.localScale += new Vector3(effect.Modifier, effect.Modifier, 0f);

                    if (otherEntity.IsPlayer && other.transform.localScale.x >= 10f)
                    {
                        SceneChanger.EnterScene("GameEnd");
                        Debug.Log("************************* You win!!!! **************************");
                    }

                    break;
                default:
                    Debug.LogError("Entity.cs: stat to be modified was NONE!");
                    break;
            }
        }
    }

    // ------------------------

    float SetRandomSize()
    {
        var val = 0.0f;
        var chance = UnityEngine.Random.value;
        if (chance < 0.6f) val = UnityEngine.Random.Range(0.1f, 1f);
        else val = UnityEngine.Random.Range(1.1f, 5f);
       // Debug.Log("Setting random size: " + val);
        return val;
    }

    // ------------------------

    void MoveEntity()
    {
        Timer = Timer - Time.deltaTime;
        if (Timer < 1f)
        {
            Timer = 6f;
            GoalPosition = new Vector2(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
            Debug.Log("new goal pos set");
        }
        if (!IsPlayer)
        {
            if (transform.parent == null) Debug.LogError("thing has no parent: " + name);
            transform.parent.position = Vector2.MoveTowards(transform.position, GoalPosition, Speed * Time.deltaTime);
        }
        else transform.position = Vector2.MoveTowards(transform.position, GoalPosition, Speed * Time.deltaTime);
    }
}
