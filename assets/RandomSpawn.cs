using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public int score = 0; // 스코어 변수 추가
    public GameObject respawnRange;
    public GameObject green;
    public float spawnTime = 10f; // 총 스폰 시간
    public int maxCount = 2; // 스폰당 최대 개수

    private BoxCollider rangeCollider;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    public float minimumDistance = 2.0f;

    private void Start()
    {
        rangeCollider = respawnRange.GetComponent<BoxCollider>();
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        float timeInterval = spawnTime / maxCount;

        while (true)
        {
            for (int i = 0; i < maxCount; i++)
            {
                Vector3 newPosition = Return_RandomPosition();

                GameObject instantSlime = Instantiate(green, newPosition, Quaternion.identity);
                spawnedPositions.Add(newPosition);

                yield return new WaitForSeconds(timeInterval);
            }

            yield return new WaitForSeconds(timeInterval); // 다음 스폰 사이클 전에 대기
        }
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 newPosition;
        int safetyNet = 100;

        do
        {
            Vector3 originPosition = respawnRange.transform.position;
            float range_X = rangeCollider.bounds.size.x;
            float range_Z = rangeCollider.bounds.size.z;

            range_X = Random.Range((range_X / 2) * -1, range_X / 2);
            range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

            newPosition = originPosition + new Vector3(range_X, 0f, range_Z);

            safetyNet--;
            if (safetyNet <= 0)
            {
                Debug.LogError("새로운 스폰 위치를 찾기 위한 시도가 너무 많았습니다.");
                return newPosition;
            }
        }
        while (IsPositionTooClose(newPosition));

        return newPosition;
    }

    bool IsPositionTooClose(Vector3 position)
    {
        foreach (var spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minimumDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void OnDestroy2()
    {
        // 슬라임이 파괴될 때 호출되는 함수
        // 다른 오브젝트에 있는 Destroy 함수에 의해 호출될 예정
        // 여기에서 스코어를 증가시킬 수 있습니다.
        score += 10;
        Debug.Log("Score: " + score);
    }


}
