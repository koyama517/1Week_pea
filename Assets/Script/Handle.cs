using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Handle : MonoBehaviour
{
    [Header("��{�ݒ�")]
    public Transform player;
    public float maxX = 3f;
    public float maxY = 2f;
    public float angleLerpSpeed = 10f;
    public float minXOffset = 1f;

    [Header("���ˏ���")]
    public float horizontalSpeedThreshold = 5f;  // ���������x臒l�i�E�����ɓ������甭�ˁj
    public float reloadDistanceThreshold = 1f;   // �v���C���[�ɋ߂Â����烊���[�h����

    [Header("�U���֘A")]
    public float attackDuration = 0.5f;

    [Header("�e����")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform[] handleSegments;

    [Header("�F�ύX")]
    public Color normalColor = Color.white;
    public Color attackColor = Color.red;
    public Color readyToFireColor = Color.yellow;

    public bool isAttacking { get; private set; }
    private bool canFire = true;
    private bool isInAttackCoroutine = false;
    private SpriteRenderer sr;
    private Vector3 prevMouseWorldPos;
    private float currentAngle = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        prevMouseWorldPos = GetMouseWorldPosition();
    }

    void Update()
    {
        if (Camera.main == null || player == null) return;

        // �����[�h����F�n���h�����v���C���[�ɋ߂���΃����[�h����
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (!canFire && distToPlayer <= reloadDistanceThreshold)
        {
            canFire = true;
            Debug.Log("�����[�h����");
        }

        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 mouseDelta = mouseWorldPos - prevMouseWorldPos;
        float horizontalSpeed = Mathf.Abs(mouseDelta.x) / Time.deltaTime;

        // ���ˏ����͉��������x��臒l�ȏォ�E�����ɓ����Ă��āA�����[�h������ԂȂ甭��
        if (!isAttacking && canFire && horizontalSpeed >= horizontalSpeedThreshold && mouseDelta.x > 0f && !isInAttackCoroutine)
        {
            StartCoroutine(TriggerAttack());
        }

        prevMouseWorldPos = mouseWorldPos;

        // --- �n���h���ʒu ---
        Vector3 direction = mouseWorldPos - player.position;
        direction.x = Mathf.Clamp(direction.x, minXOffset, maxX);
        direction.y = Mathf.Clamp(direction.y, -maxY, maxY);
        transform.position = player.position + direction;

        // --- ��]���� ---
        float targetAngle = 0f;
        if (Mathf.Approximately(direction.x, minXOffset))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
        else
        {
            Vector3 dir = transform.position - player.position;
            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float currentZ = transform.eulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;
            currentAngle = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * angleLerpSpeed);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        // --- �F�ύX ---
        if (sr != null)
        {
            if (!isAttacking)
                sr.color = normalColor;
            else if (canFire)
                sr.color = readyToFireColor;
            else
                sr.color = attackColor;
        }
    }

    IEnumerator TriggerAttack()
    {
        isInAttackCoroutine = true;
        isAttacking = true;
        canFire = false; // ���ˌ�̓����[�h�҂�
        Debug.Log("�U���J�n");

        if (bulletPrefab != null && bulletSpawnPoint != null && handleSegments.Length > 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            BulletAlongCurve bulletScript = bullet.GetComponent<BulletAlongCurve>();
            if (bulletScript != null)
            {
                List<Vector3> path = new List<Vector3>();
                Vector3 basePos = handleSegments[0].position;
                float stretchFactor = 3f;

                path.Add(basePos);
                for (int i = 1; i < handleSegments.Length; i++)
                {
                    Vector3 dir = handleSegments[i].position - handleSegments[i - 1].position;
                    Vector3 stretchedPoint = path[i - 1] + dir.normalized * dir.magnitude * stretchFactor;
                    path.Add(stretchedPoint);
                }

                bulletScript.SetPath(path);
            }
            else
            {
                Debug.LogWarning("BulletAlongCurve �X�N���v�g��������܂���");
            }
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        isInAttackCoroutine = false;
        Debug.Log("�U���I��");
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z - player.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        return worldPos;
    }
}
