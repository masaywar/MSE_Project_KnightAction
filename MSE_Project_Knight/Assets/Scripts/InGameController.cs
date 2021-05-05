using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour, Observer
{
    private System.Random random = new System.Random();

    private ObjectManager cachedObjectManager;
    private bool isSpawning = false;

    private float waitTime = 0.2f;
    private WaitForSeconds wait;

    public Transform spawnPlace;
    public Transform activeContainer;

    public Player player;
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

        public string[] unparsedPattern;

        public Pattern()
        {
            unparsedPattern = new string[] {
                "UBDBUBDBUBDBUBDB"
            };
        }

        public string  GetPattern(int index)
        {
            if (index < unparsedPattern.Length)
                return unparsedPattern[index];

            Debug.Log("Index Error, Set index parameter less than unparsedPattern's length");
            return null;
        }

    }

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
            isGround = unparsedPattern[i-1] == 'D' ? true : false;
            isDestroyable = unparsedPattern[i] == 'B' ? true : false;

            stateList.Add(new bool[] {isGround, isDestroyable});
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
        int idx = random.Next(pattern.unparsedPattern.Length);
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
            string tag = state[1] ? "DestroyableEnemy" : "UndestroyableEnemy";

            int spawnIdx = isGround ? 0 : 1;

            var spawned = cachedObjectManager.Spawn<EnemyObject>(tag, spawnPlace.GetChild(spawnIdx).position, spawnPlace.GetChild(2));

            spawned.OnEnemySpawn(isGround);

            yield return wait;
        }

        yield return new WaitForSeconds(1.0f);

        isSpawning = false;
    }
}
