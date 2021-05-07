using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : Singleton<InGameController>
{
    private System.Random random = new System.Random();

    private ObjectManager cachedObjectManager;
    private bool isSpawning = false;

    private float waitTime = 0.2f;
    private WaitForSeconds wait;

    public Transform spawnPlace;
    public Transform activeContainer;

    public Player player;
    public List<EnemyObject> enemyList = new List<EnemyObject>();


    //Pattern inner class will be removed

    [System.Serializable]
    private class Pattern
    {
        /// <summary>
        /// Pattern :
        ///      U : up-side
        ///      D : down-side
        ///      B : Destroyable / Breakable
        ///      H : Undestroyable / Hard  
        ///      Length of Pattern must be 16.
        /// </summary>

        [SerializeField]
        public List<string> unparsedPattern = new List<string>();

        private string[] symbols = new string[] { "UB", "UH", "DB", "DH" };

        public Pattern()
        {

            MakePattern(16).ForEach<string>(s =>
            {
                AddPattern(s);
                print(s);
            });

        }

        public string GetPattern(int index)
        {
            if (index < unparsedPattern.Count)
                return unparsedPattern[index];

            Debug.Log("Index Error, Set index parameter less than unparsedPattern's length");
            return null;
        }

        private void AddPattern(string pattern)
        {
            unparsedPattern.Add(pattern);
        }

        private IEnumerable<string> MakePattern(int count)
        {
            int tempCount = 0;

            while (tempCount < count)
            {
                string pattern = "";
                for (int i = 0; i < 8; i++)
                {
                    System.Random rand = new System.Random(150);

                    int idx = rand.Next(0, 4);
                    pattern += symbols[idx];
                }

                tempCount += 1;
                yield return pattern;
            }
        }
    }

    [SerializeField]
    private Pattern pattern = new Pattern();

    private void Start()
    {
        cachedObjectManager = ObjectManager.Instance;

        activeContainer = spawnPlace.GetChild(2);
        wait = new WaitForSeconds(waitTime);
    }

    private void Update()
    {
        SpawnEnemy(0);
    }

    private List<bool[]> ParsePattern(string unparsedPattern)
    {
        bool isGround = true;
        bool isDestroyable = true;

        List<bool[]> stateList = new List<bool[]>();

        for (int i = 1; i < unparsedPattern.Length; i += 2)
        {
            isGround = unparsedPattern[i - 1] == 'D' ? true : false;
            isDestroyable = unparsedPattern[i] == 'B' ? true : false;

            stateList.Add(new bool[] { isGround, isDestroyable });
        }

        return stateList;
    }

    public void SpawnEnemy(int index)
    {
        if (isSpawning)
            return;

        isSpawning = true;

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        int idx = random.Next(pattern.unparsedPattern.Count);


        string unparsedPattern = pattern.GetPattern(idx);

        if (unparsedPattern.Length != 16)
        {
            Debug.Log("Not permitted pattern, please modify that");
        }

        List<bool[]> stateList = ParsePattern(unparsedPattern);

        foreach (var state in stateList)
        {
            ///
            /// state[0] : isGround
            /// state[1] : isDestroyable
            ///

            bool isGround = state[0];
            string tag = state[1] ? "DestroyableEnemy" : "UnDestroyableEnemy";

            int spawnIdx = isGround ? 0 : 1;
            var spawned = cachedObjectManager.Spawn<EnemyObject>(tag, spawnPlace.GetChild(spawnIdx).position, spawnPlace.GetChild(2));

            spawned.OnEnemySpawn(isGround);
            enemyList.Add(spawned);

            yield return wait;
        }

        yield return new WaitForSeconds(1.0f);

        isSpawning = false;
    }

    public void OnDestroyEnemy(GameObject enemy, bool force = false)
    {
        EnemyObject tempEnemy = null;
        int findIdx = 0;

        tempEnemy = enemyList[findIdx = enemyList.FindIndex(e => enemy.Equals(e.gameObject))];

        if (findIdx < 0 || findIdx > enemyList.Count)
        {
            return;
        }

        enemyList.RemoveAt(findIdx);
        tempEnemy.DestroyWithAnim(force);
    }

    public void Unsubscribe(EnemyObject enemy)
    {
        enemyList.Remove(enemy);
    }
}
