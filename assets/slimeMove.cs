using System.Collections;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    private HealthBarHUDTester healthBarHUDTester; // 인스턴스 변수로 변경
    public Animator slimeAnimator;
    public float speed = 1.0f;
    public float attackInterval = 3.0f;
    public float waitAfterArrival = 1.0f;

    public Sprite targetSprite;     // 추가: 목표에 도달한 후 대기시간을 기다릴 때 표시할 스프라이트
    public Sprite newTargetSprite;  // 추가: 새로운 목표로 이동할 때 표시할 스프라이트

    private float targetZ;
    private bool isMoving = true;
    private bool isAttacking = false;
    private bool spriteShown = false;
    private GameObject targetObject;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        healthBarHUDTester = FindObjectOfType<HealthBarHUDTester>();
        SetNewTargetZ();
        CreateTargetSprite();
    }

    void Update()
    {
        float distanceToTarget = Mathf.Abs(transform.position.z - targetZ);

        if (distanceToTarget > 0.1f && !isAttacking)
        {
            MoveTowardsTarget();
        }
        else if (distanceToTarget <= 0.1f && !isAttacking)
        {
            if (!spriteShown)
            {
                ShowTargetSprite();
            }
            StartCoroutine(AttackRoutine());
        }
        else
        {
            // 추가: 목표에 도달하고 공격 중이 아닌 경우에만 MoveTowardsTarget 호출
            if (!isAttacking)
            {
                MoveTowardsTarget();
            }
        }
    }

    void MoveTowardsTarget()
{
    if (!isAttacking) // Check if not attacking
    {
        isMoving = true;
        Vector3 direction = new Vector3(0, 0, targetZ) - new Vector3(0, 0, transform.position.z);
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // 목표 지점에 거의 도달했을 때 정확한 위치로 이동
        if (Mathf.Abs(transform.position.z - targetZ) < 0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, targetZ);
            StartCoroutine(WaitAfterArrival()); // 도착 후 대기 시작
        }

        // Play "walk" animation only when not attacking and not arrived
        if (!isAttacking && Mathf.Abs(transform.position.z - targetZ) > 0.1f)
        {
            slimeAnimator.SetTrigger("Walk");

        }
    }
}


    IEnumerator AttackRoutine()
    {
        isMoving = false;
        isAttacking = true;
        spriteShown = false; // 공격 시작 시 스프라이트 상태 초기화

        transform.LookAt(Camera.main.transform.position);
        yield return new WaitForSeconds(attackInterval); // 공격 간격만큼 대기
        slimeAnimator.SetTrigger("Attack");
        healthBarHUDTester.Hurt(1); // 인스턴스를 통해 메서드 호출
        isAttacking = false;
    }

IEnumerator WaitAfterArrival()
{
    // 공격 1초 전에 스프라이트를 변경
    yield return new WaitForSeconds(waitAfterArrival - 1.0f);
    ChangeSpriteToNew();

    // 대기 시간이 끝난 후 실행할 코드 추가 가능
    yield return new WaitForSeconds(1.0f);

    // Walk 애니메이션을 중복으로 발동하지 않도록 체크
    if (!isAttacking)
    {
        slimeAnimator.SetTrigger("Walk");
    }
}


    void CreateTargetSprite()
    {
        if (targetObject == null)
        {
            targetObject = new GameObject("TargetObject");
            targetObject.transform.parent = transform;
            targetObject.transform.localPosition = new Vector3(0, 0.5f, -0.1f);
            targetObject.transform.localRotation = Quaternion.identity;

            Vector3 localPosition = targetObject.transform.localPosition;
            localPosition.z += 1.0f;
            targetObject.transform.localPosition = localPosition;

            spriteRenderer = targetObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = targetSprite;
            spriteRenderer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteRenderer.enabled = false;
        }
    }

    void ShowTargetSprite()
    {
        if (spriteShown) return;

        CreateTargetSprite();
        spriteShown = true;
        spriteRenderer.enabled = true;
    }

    void ChangeSpriteToNew()
    {
        if (!spriteShown)
        {
            ChangeSprite(newTargetSprite);
            spriteShown = true;
        }
    }

    void ChangeSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    void SetNewTargetZ()
    {
        targetZ = Random.Range(5f, 7f);
    }
}
