using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatBlock : ScriptableObject
{
    public EnemyType Type;

    [Header("Starting Stats")]
    public bool IsPlayer = false;
    public float Size = 1.0f;
    public float Agility = 1.0f;
    public float Speed = 1.0f;
    public float Luminosity = 1.0f;
    
    public List<Ability> Abilities = null;
    public List<Reward> ConsumeEffects;

    // -----------------

    // todo handle this nicely i.e. only show these if the entity starts with a dash ability
    [Header("Ability stats, where applicable")]
    public float DashDuration = 0.5f;
    public float DashCooldown = 0.5f;
    public float DashSpeedBonusMultiplier = 0.0f; // only bats and the player should have this above 0

    [Serializable]
    public struct Reward
    {
        public RewardType Type;
        public Ability AbilityToBeRewarded;

        public float Modifier;
        public Stat StatToBeModified;
    }

    public enum RewardType
    {
        NONE,
        ABILITY,
        STAT_MODIFIER,
        ABILITY_MODIFIER // I don't think we'll need this but it's here just in case anyway
    }

    // todo this might not be needed
    public void OnConsumed()
    {
        if (IsPlayer)
        {
            Debug.Log("Player died, game over!");
        }
    }
}
