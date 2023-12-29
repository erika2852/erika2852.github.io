using UnityEngine;
using System.Collections;
public class Destroy : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 검사
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast를 사용하여 마우스 클릭 지점에 무엇이 있는지 확인
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast가 명중되었는지 확인
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                // 명중한 오브젝트의 태그가 "Monster"인지 확인
                if (hit.collider.CompareTag("monster"))
                {
                    // 몬스터의 애니메이션 컴포넌트 가져오기
                    Animator monsterAnimator = hit.collider.gameObject.GetComponent<Animator>();

                    // 몬스터를 Damage 애니메이션으로 전환
                    monsterAnimator.SetTrigger("Damage");

                    // 코루틴을 사용하여 딜레이 후 몬스터 파괴
                    StartCoroutine(DestroyAfterAnimation(hit.collider.gameObject));
                }
            }
        }
    }

    IEnumerator DestroyAfterAnimation(GameObject monster)
    {
        // 애니메이션의 길이에 따라 딜레이를 조정
        yield return new WaitForSeconds(monster.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        Debug.Log("Destroying Monster: " + monster.name);
        Destroy(monster);
         RandomSpawn randomSpawn = FindObjectOfType<RandomSpawn>();

                    // RandomSpawn 스크립트의 OnDestroy 함수 호출
                    if (randomSpawn != null)
                    {
                        randomSpawn.OnDestroy2();
                    }
    }
}
