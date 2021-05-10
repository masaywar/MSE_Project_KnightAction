using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : ScriptObject
{
    public int speed;

    public bool isDestroyable = true;
    public bool isUp = false;

    public Rigidbody2D despawnPlace;
    public Transform despawnContainer;

    private InGameController inGameController;

    private bool isDead = false;

    private void Start()
    {
        inGameController = InGameController.Instance;
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
        if(!isDead)
            rectTransform.anchoredPosition += Vector2.left * speed * GameManager.Instance.deltaTime;
    }

    private bool CheckValidPos()
    {
        return despawnPlace.position.x < rectTransform.position.x * rectTransform.localScale.x;
    }

    public void OnEnemySpawn(bool onGround)
    {
        isUp = onGround ? false : true;
    }

    public void Notify()
    {
        Destroy();
    }

    public void Destroy()
    {
        Destroy(false, false);
    }

    public void DestroyForced()
    {
        Destroy(false, true);
    }

    public void DestroyWithAnim(bool force)
    {
        Destroy(true, force);
    }

    private void Destroy(bool isAnim, bool force)
    {

        inGameController.Unsubscribe(this);

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

            rectTransform.anchoredPosition += randomVector * GameManager.Instance.deltaTime * speed;
            transform.Rotate(new Vector3(0, 0, 1) * GameManager.Instance.deltaTime * speed);
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
