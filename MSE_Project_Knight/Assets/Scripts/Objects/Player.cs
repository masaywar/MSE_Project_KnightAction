using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ScriptObject
{
    public ICompanion character;

    public IngameController playerController;
    
    //Variables for raycasting
    private RaycastHit2D[] hits;
    public float rayDist;

    // Check player is on fever. It will be norified by ingame controller that player is on fever or not.
    [SerializeField]
    private bool isFever;

    // Cached Transform for raycasting and moving player object.
    private RectTransform upTransform;
    private RectTransform downTransform;
    private RectTransform feverTransform;

    //Observer
    public void PlayerInitialize()
    {
        character = GetComponent<ICompanion>();
        character.SetPrefab(GetComponent<SPUM_Prefabs>());
        var rect = GameObject.FindGameObjectWithTag("PlayerMove");

        upTransform = rect.transform.GetChild(0).GetComponent<RectTransform>();
        downTransform = rect.transform.GetChild(1).GetComponent<RectTransform>();
        feverTransform = rect.transform.GetChild(2).GetComponent<RectTransform>();
        character.RunMotion();
    }

    // Reference of IngameController's Attack event. 
    // Ingame Controller have reference of this method to make it loose coupled between 
    // enemies and player.
    
    // In attackMode 0 : Attack on ground 
    //               1 : Attack on sky
    //               2 : Attack in player fever mode
    private RaycastHit2D[] OnPlayerAttack(int attackMode)
    {
        if (isFever)
            attackMode = 2;

        SetPosition(attackMode);
        character.AttackMotion();


        return Attack(attackMode);
    }

    // Set player position to Uptransform or downTransform.
    // if attackMode == 0, then player is placed to downTransform.
    // if attackMode == 1, then player is placed to upTransform.
    // the last one means when player is in fever mode, then placed in feverTransform
    // which placed in between upTransform and downTransform.
    private void SetPosition(int attackMode)
    {
        switch (attackMode)
        {
            case 0: // ground
                rectTransform.position = downTransform.position;
                break;

            case 1: //up
                rectTransform.position = upTransform.position;
                break;

            case 2: //fever
                rectTransform.position = feverTransform.position;
                break;
        }
    }

    // Player attack with raycasting.
    // if hits are found, return hits and then raise event to ingameController
    // if not, return null and also raise same event to ingameController
    private RaycastHit2D[] Attack(int attackMode)
    {
        if (TryRaycastHit(attackMode, out hits))
        {
            return hits;
        }

        return null;
    }

    // Checking raycasthits
    private bool TryRaycastHit(int attackMode, out RaycastHit2D[] hits)
    {
        switch (attackMode)
        {
            case 0: //ground
                hits = Physics2D.BoxCastAll(
                    downTransform.position,
                    downTransform.sizeDelta.normalized,
                    0,
                    Vector2.right * rayDist,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(downTransform.position, Vector2.right * rayDist, Color.red);
                break;

            case 1: //up
                hits = Physics2D.BoxCastAll(
                    upTransform.position,
                    upTransform.sizeDelta.normalized,
                    0,
                    Vector2.right * rayDist,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(upTransform.position, Vector2.right * rayDist, Color.red, 100f);
                break;

            case 2: //fever
                var tempList = new List<RaycastHit2D>();

                tempList.AddRange(
                    Physics2D.BoxCastAll(
                    downTransform.position,
                    downTransform.sizeDelta.normalized,
                    0,
                    Vector2.right * rayDist ,
                    rayDist, LayerMask.GetMask("InPlayGame")));

                tempList.AddRange(
                    Physics2D.BoxCastAll(
                    upTransform.position,
                    upTransform.sizeDelta.normalized,
                    0,
                    Vector2.right * rayDist,
                    rayDist, LayerMask.GetMask("InPlayGame")));

                hits = tempList.ToArray();
                break;

            default:
                hits = null;
                break;
        }

        return hits.Length > 0;
    }

    // Player action in fever mode.
    private void OnPlayerFever(bool isFever)
    {
        this.isFever = isFever;

        if (isFever)
            SetPosition(2);
        else
            SetPosition(0);
    }

    private void OnPlayerDead()
    {
        Unsubscribe();
        StartCoroutine(DieAnimation());
    }

    private void OnPlayerMiss()
    {
        StartCoroutine(StunAnimation());
    }

    // Destroy All enemies.
    // It also using raycast.
    // ray from upTransform and downTransform like feverMode/
    private RaycastHit2D[] OnPlayerUlt()
    {
        var tempList = new List<RaycastHit2D>();

        character.UltMotion();

        tempList.AddRange(
            Physics2D.BoxCastAll(
            downTransform.position,
            downTransform.sizeDelta,
            0,
            Vector2.right,
            rayDist, LayerMask.GetMask("InPlayGame")));

        tempList.AddRange(
            Physics2D.BoxCastAll(
            upTransform.position,
            upTransform.sizeDelta,
            0,
            Vector2.right * 10,
            rayDist, LayerMask.GetMask("InPlayGame")));

        return tempList.ToArray();
    }


    // Add event to ingame controller
    public void Subscribe()
    {
        playerController.OnPlayerDead += OnPlayerDead;
        playerController.OnPlayerAttack += OnPlayerAttack;
        playerController.OnPlayerJumpAttack += OnPlayerAttack;
        playerController.OnPlayerFever += OnPlayerFever;
        playerController.OnPlayerMiss += OnPlayerMiss;
        playerController.OnPlayerUlt += OnPlayerUlt;
    }

    // Delete event from ingame controller
    public void Unsubscribe()
    {
        playerController.OnPlayerDead -= OnPlayerDead;
        playerController.OnPlayerAttack -= OnPlayerAttack;
        playerController.OnPlayerJumpAttack -= OnPlayerAttack;
        playerController.OnPlayerFever -= OnPlayerFever;
        playerController.OnPlayerMiss -= OnPlayerMiss;
        playerController.OnPlayerUlt -= OnPlayerUlt;
    }

    // Animations

    private IEnumerator DieAnimation()
    {
        character.DeadMotion();
        yield return new WaitForSeconds(.5f);
    }

    private IEnumerator StunAnimation()
    {
        character.StunMotion();
        yield return new WaitForSeconds(0.5f);
        character.RunMotion();
    }

    private void OnDisable()
    {
        if (playerController != null)
            Unsubscribe();
    }

    private void OnEnable()
    {
        if (playerController != null)
            Subscribe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable") || collision.CompareTag("Undestroyable"))
        {
            OnPlayerMiss();
            playerController.hp -= playerController.damage;
        }
    }
}

