using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Handle : MonoBehaviour
{
    [Header("基本設定")]
    public Transform player;
    public float maxX = 3f;
    public float maxY = 2f;
    public float angleLerpSpeed = 10f;
    public float minXOffset = 1f;

    [Header("攻撃関連")]
    public float attackSpeedThreshold = 10f;
    public float attackDuration = 0.5f;
    public float lockDistanceThreshold = 2f;

    [Header("突き判定")]
    public float horizontalSpeedThreshold = 3f;

    [Header("リロード")]
    public float reloadDistanceThreshold = 1f; // プレイヤーに近づけばリロード完了
    private bool canFire = true;                // 発射可能かどうか

    [Header("弾発射")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 5f;
    public Transform[] handleSegments;

    [Header("色変更")]
    public Color normalColor = Color.white;
    public Color attackColor = Color.red;
    public Color readyToFireColor = Color.yellow;

    public bool isAttacking { get; private set; }
    public bool ShouldLockAngle { get; private set; }

    private Vector3 prevMouseWorldPos;
    private float currentAngle = 0f;
    private bool isInAttackCoroutine = false;

    private SpriteRenderer sr;
    private bool cachedIsThrustMotion = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        prevMouseWorldPos = GetMouseWorldPosition();
    }

    void Update()
    {
        if (Camera.main == null || player == null) return;

        // リロード判定：ハンドルがプレイヤーに近ければリロード完了
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (!canFire && distToPlayer <= reloadDistanceThreshold)
        {
            canFire = true;
            Debug.Log("リロード完了");
        }

        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 mouseDelta = (mouseWorldPos - prevMouseWorldPos);
        float horizontalSpeed = Mathf.Abs(mouseDelta.x) / Time.deltaTime;

        cachedIsThrustMotion = (mouseDelta.x > 0f) && (horizontalSpeed >= horizontalSpeedThreshold);

        // 攻撃判定は canFire も条件に追加
        if (!isAttacking && canFire && horizontalSpeed >= attackSpeedThreshold && !isInAttackCoroutine)
        {
            StartCoroutine(TriggerAttack());
        }

        prevMouseWorldPos = mouseWorldPos;

        // --- ハンドル位置 ---
        Vector3 direction = mouseWorldPos - player.position;
        direction.x = Mathf.Clamp(direction.x, minXOffset, maxX);
        direction.y = Mathf.Clamp(direction.y, -maxY, maxY);
        transform.position = player.position + direction;

        // --- 回転処理 ---
        float targetAngle = 0f;
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

        if (Mathf.Approximately(direction.x, minXOffset))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
        else
        {
            Vector3 dir = transform.position - player.position;
            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // 突きのみ角度固定
            ShouldLockAngle = isAttacking && cachedIsThrustMotion && distanceFromPlayer >= lockDistanceThreshold;

            if (!ShouldLockAngle)
            {
                float currentZ = transform.eulerAngles.z;
                if (currentZ > 180f) currentZ -= 360f;
                currentAngle = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * angleLerpSpeed);
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
        }

        // --- 色変更 ---
        if (sr != null)
        {
            if (!isAttacking)
                sr.color = normalColor;
            else if (ShouldLockAngle)
                sr.color = readyToFireColor;
            else
                sr.color = attackColor;
        }
    }

    IEnumerator TriggerAttack()
    {
        isInAttackCoroutine = true;
        isAttacking = true;
        canFire = false; // 発射したのでリロード待ち
        Debug.Log("攻撃開始");

        // 発射判定はコルーチン開始時に一回だけ決定
        bool canShootNow = cachedIsThrustMotion && Vector3.Distance(transform.position, player.position) >= lockDistanceThreshold;
        ShouldLockAngle = canShootNow;

        Debug.Log($"TriggerAttack: canShootNow={canShootNow}, cachedIsThrustMotion={cachedIsThrustMotion}, distance={Vector3.Distance(transform.position, player.position)}");

        if (canShootNow && bulletPrefab != null && bulletSpawnPoint != null && handleSegments.Length > 1)
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
                Debug.LogWarning("BulletAlongCurve スクリプトが見つかりません");
            }
        }
        else
        {
            Debug.Log("弾発射条件不成立");
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        isInAttackCoroutine = false;
        ShouldLockAngle = false;
        Debug.Log("攻撃終了");
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
