using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessenger : MonoBehaviour
{
    [SerializeField] Text BatBox = null;
    [SerializeField] Text CandyBox = null;
    [SerializeField] Text SlimeBox = null;
    [SerializeField] Text WispBox = null;
    [SerializeField] Text SpiderBox = null;

    private int batsCollected = 0;
    private int candyCollected = 0;
    private int slimesCollected = 0;
    private int wispsCollected = 0;
    private int spidersCollected = 0;

    Entity Player = null;
    public FogController fogController;

    void Start()
    {
        Player = GetComponent<Entity>();
        if (Player == null) Debug.LogError("Entity component would not be found. Is it added to the gameObject?");

        BatBox.text = batsCollected.ToString();
        CandyBox.text = candyCollected.ToString();
        SlimeBox.text = slimesCollected.ToString();
        WispBox.text = wispsCollected.ToString();
        SpiderBox.text = spidersCollected.ToString();

        Player.OnConsume = new Entity.OnConsumeDelegate(FilterEnemiesIntoBoxes);
    }

    // extremely bad hack fix for ui update bug 
    private void Update()
    {
        BatBox.text = batsCollected.ToString();
        CandyBox.text = candyCollected.ToString();
        SlimeBox.text = slimesCollected.ToString();
        WispBox.text = wispsCollected.ToString();
        SpiderBox.text = spidersCollected.ToString();
    }

    void FilterEnemiesIntoBoxes(EnemyType type)
    {
        if (type == EnemyType.BAT)
        {
            batsCollected++;
            UpdateBoxText(BatBox, batsCollected);
        }
        if (type == EnemyType.CANDY)
        {
            candyCollected++;
            UpdateBoxText(CandyBox, batsCollected);
        }
        if (type == EnemyType.SLIME)
        {
            slimesCollected++;
            UpdateBoxText(SlimeBox, batsCollected);
        }
        if (type == EnemyType.WISP)
        {
            wispsCollected++;
            UpdateBoxText(WispBox, wispsCollected);

            fogController.FogDecrease();
        }
        if (type == EnemyType.SPIDER)
        {
            spidersCollected++;
            UpdateBoxText(SpiderBox, spidersCollected);
        }
    }

    void UpdateBoxText(Text text, int collected)
    {
        text.text = collected.ToString();
      //  Canvas.ForceUpdateCanvases(); // this didn't work..
    }
}
