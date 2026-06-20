using UnityEngine;

/// <summary>
/// 收集物 - 玩家碰撞时增加分数并消失
/// </summary>
public class Collectible : MonoBehaviour
{
    public int points = 10;
    public Color color = Color.yellow;
    public float rotationSpeed = 50f;

    private GameManager _gameManager;
    private Renderer _renderer;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _renderer = GetComponent<Renderer>();

        // 随机颜色
        if (_renderer != null)
        {
            _renderer.material.color = new Color(
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f)
            );
        }
    }

    void Update()
    {
        // 自转动画
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        // 上下浮动
        float yOffset = Mathf.Sin(Time.time * 2f) * 0.5f;
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            1f + yOffset,
            transform.localPosition.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 增加分数
            if (_gameManager != null)
                _gameManager.AddScore(points);

            // 播放特效（简单缩放后销毁）
            StartCoroutine(CollectEffect());
        }
    }

    System.Collections.IEnumerator CollectEffect()
    {
        // 放大效果
        for (float i = 0; i < 1f; i += Time.deltaTime * 3)
        {
            transform.localScale = Vector3.one * (1 + i);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
