using UnityEngine;

public class DisableCollidersInChildren : MonoBehaviour
{
    void Start()
    {
        // 해당 오브젝트의 모든 Collider를 비활성화
        DisableCollidersRecursively(gameObject);
    }

    void DisableCollidersRecursively(GameObject obj)
    {
        // 현재 오브젝트의 Collider를 비활성화
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // 하위 오브젝트에 대해 재귀적으로 호출
        foreach (Transform child in obj.transform)
        {
            DisableCollidersRecursively(child.gameObject);
        }
    }
}
