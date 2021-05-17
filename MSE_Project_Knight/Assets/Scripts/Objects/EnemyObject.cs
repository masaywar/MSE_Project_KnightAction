using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : ScriptObject
{
    public bool isDestroyable = true;
    public bool isUp = false;

    public Rigidbody2D despawnPlace;


    private IngameController inGameController;
    private bool isDead = false;

    private void Start()
    {

        inGameController = IngameController.Instance;
        inGameController.OnHitEnemy += Destroy;
    }

    private void FixedUpdate()
    {
        if (CheckValidPos())
            Move();
        else
            DestroyForced();
    }

    public virtual void Move()
    {
        if (!isDead)
            rectTransform.anchoredPosition += Vector2.left * speed * GameManager.Instance.deltaTime;
    }

    private bool CheckValidPos()
    {
        return despawnPlace.position.x < rectTransform.position.x;
    }

    public void Destroy(GameObject obj)
    {
        Destroy(obj, false, false);
    }

    public void DestroyForced()
    {
        Destroy(this.gameObject, false, true);
    }

    public void DestroyWithAnim(GameObject obj, bool force)
    {
        Destroy(obj, true, force);
    }

    private void Destroy(GameObject obj, bool isAnim, bool force)
    {
        if (obj != this.gameObject) return;

        if (isDestroyable || force)
        {
            isDead = true;
            var randomVector = new Vector2(1, Random.Range(.2f, 1f));

            if (isAnim)
                StartCoroutine(OnDestroyAnimate(randomVector));
            else
                gameObject.SetActive(false);
        }
    }

    private IEnumerator OnDestroyAnimate(Vector2 randomVector)
    {
        float anim = 0;
        while (anim < 5)
        {
            anim += GameManager.Instance.deltaTime;

            rectTransform.anchoredPosition += randomVector * GameManager.Instance.deltaTime * 10;
            transform.Rotate(new Vector3(0, 0, 1) * GameManager.Instance.deltaTime * 800);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Initialize();
        isDead = false;
        ObjectManager.Instance.Despawn<ScriptObject>(this);
    }

}
