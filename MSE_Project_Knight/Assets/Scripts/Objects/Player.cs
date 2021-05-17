using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ScriptObject
{
    public SPUM_Prefabs prefab;

    private IngameController playerController;
    
    private RaycastHit2D[] hits;
    public float rayDist;

    [SerializeField]
    private bool isFever;

    public RectTransform upTransform;
    public RectTransform downTransform;
    public RectTransform feverTransform;

    private AudioSource audioSource;

    //Observer
    private void Start()
    {
        playerController = IngameController.Instance;

        prefab = GetComponent<SPUM_Prefabs>();
        audioSource = GetComponent<AudioSource>();

        Subscribe();
        prefab.PlayAnimation(1);
    }

    private RaycastHit2D[] OnPlayerAttack(int attackMode)
    {
        if (isFever)
            attackMode = 2;

        SetPosition(attackMode);
        prefab.PlayAnimation(4);

        return Attack(attackMode);
    }

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

    private RaycastHit2D[] Attack(int attackMode)
    {
        if (TryRaycastHit(attackMode, out hits))
        {
            SoundManager.Instance.PlayOneShot("Jab", audioSource);
            return hits;
        }

        SoundManager.Instance.PlayOneShot("Swing", audioSource);
        return null;
    }

    private bool TryRaycastHit(int attackMode, out RaycastHit2D[] hits)
    {
        switch (attackMode)
        {
            case 0: //ground
                hits = Physics2D.BoxCastAll(
                    downTransform.position,
                    downTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(downTransform.position, Vector2.right * rayDist, Color.red);
                break;

            case 1: //up
                hits = Physics2D.BoxCastAll(
                    upTransform.position,
                    upTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(upTransform.position, Vector2.right * rayDist, Color.red);
                break;

            case 2: //fever
                var tempList = new List<RaycastHit2D>();

                tempList.AddRange(
                    Physics2D.BoxCastAll(
                    downTransform.position,
                    downTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame")));

                tempList.AddRange(
                    Physics2D.BoxCastAll(
                    upTransform.position,
                    upTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame")));

                hits = tempList.ToArray();
                break;

            default:
                hits = null;
                break;
        }

        return hits.Length > 0;
    }

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
        StartCoroutine(DieAnimation());
        Unsubscribe();
    }

    private void OnPlayerMiss()
    {
        StartCoroutine(StunAnimation());
    }

    private RaycastHit2D[] OnPlayerUlt()
    {
        return null;
    }

    private void Subscribe()
    {
        playerController.OnPlayerDead += OnPlayerDead;
        playerController.OnPlayerAttack += OnPlayerAttack;
        playerController.OnPlayerJumpAttack += OnPlayerAttack;
        playerController.OnPlayerFever += OnPlayerFever;
        playerController.OnPlayerMiss += OnPlayerMiss;
        playerController.OnPlayerUlt += OnPlayerUlt;
    }

    private void Unsubscribe()
    {
        playerController.OnPlayerDead += OnPlayerDead;
        playerController.OnPlayerAttack += OnPlayerAttack;
        playerController.OnPlayerJumpAttack += OnPlayerAttack;
        playerController.OnPlayerFever += OnPlayerFever;
        playerController.OnPlayerMiss += OnPlayerMiss;
        playerController.OnPlayerUlt += OnPlayerUlt;
    }

    private IEnumerator DieAnimation()
    {
        prefab.PlayAnimation(2);
        yield return new WaitForSeconds(.5f);
    }

    private IEnumerator StunAnimation()
    {
        prefab.PlayAnimation(3);
        yield return new WaitForSeconds(0.5f);
        prefab.PlayAnimation(1);
    }
}
