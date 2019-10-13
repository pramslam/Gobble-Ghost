using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[RequireComponent(typeof(AbilityManager))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] List<AbilityUIController> AbilitiesUI = new List<AbilityUIController>();
    [SerializeField] float lastDashPress = 0.0f;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject mainCamera;

    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] float spawnDistance = 8.5f;
    [SerializeField] float secondsBetweenSpawn = 2;

    [SerializeField] float elapsedTime = 0.0f;

    private AbilityManager AbilityManagerRef;               // todo maybe too many things are depending on each other here
    private Entity Player;

    public GameObject particleFX;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();     // Get SpriteRenderer
        enemySpawner = FindObjectOfType<EnemySpawner>();     // Find SpawnEnemy

        Player = GetComponent<Entity>();
        if (Player == null) Debug.LogError("Entity component would not be found. Is it added to the gameObject?");
        AbilityManagerRef = GetComponent<AbilityManager>();
        if (AbilityManagerRef == null) Debug.LogError("Ability Manager reference is null. Is it missing the component?");

        if (AbilitiesUI[0] != null && !Player.Abilities.Contains(Ability.DASH))
        {
            Debug.Log("Hiding dash ability");
            AbilitiesUI[0].SetVisible(false);
        }

        Player.OnStatAdded = new Entity.StatAddedDelegate(ShowAbilityUI);
    }

    private void FixedUpdate()
    {
        ForwardMovement();
    }

    void LateUpdate()
    {
        // Camera follows the player
        // hack fix to handle the camera depdency here... on scene destroy it loses the reference
        var cam = FindObjectOfType<Camera>();
        if (mainCamera == null) mainCamera = cam.gameObject;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        particleFX.transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (AbilitiesUI.Count == 0) Debug.LogError("PlayerController: Abilities.Count is 0. Is it set in the inspector?");
        if (AbilitiesUI[0] != null && Player.Abilities.Contains(Ability.DASH)
            && Input.GetButtonDown("Ability1") && Time.time > lastDashPress)
        {
            Debug.Log("Dashing!");
            lastDashPress = Time.time + Player.DashCooldown;
            AudioManager.instance.PlayDash(GetComponent<AudioSource>());
            AbilityManagerRef.Dash();
            AbilitiesUI[0].BeginCooldownAnimation(Player.DashCooldown);
        }

        elapsedTime += Time.deltaTime;

        // Spawn enemy
        if (elapsedTime > secondsBetweenSpawn)
        {
            elapsedTime = 0;
            enemySpawner.SpawnEnemy(transform.position + -transform.right * spawnDistance);
        }
    }

    // Rotate player sprite toward mouse
    void ForwardMovement()
    {
        //Get the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

        //Rotate the sprite to the mouse point
        Vector3 diff = mousePos - transform.position;
        diff.Normalize();

        // Flip sprite
        if (diff.x > 0.0f)
            spriteRenderer.flipY = true;
        else if (diff.x < 0.0f)
            spriteRenderer.flipY = false;

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);

        //Move the sprite towards the mouse
        if (!Vectors.HasNans(-transform.right * Player.Speed * Time.deltaTime))
        {
            transform.position += -transform.right * Player.Speed * Time.deltaTime;
        }
        else Debug.LogWarning("Position had NaNs");
    }

    // ---------------------------

    void ShowAbilityUI()
    {
        if(Player.Abilities.Contains(Ability.DASH)) AbilitiesUI[0].SetVisible(true);
    }
}
