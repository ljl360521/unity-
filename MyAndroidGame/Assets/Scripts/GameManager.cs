using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理器 - 管理分数、UI和游戏状态
/// </summary>
public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;
    public GameObject gameOverPanel;
    public Text finalScoreText;
    public int gameDuration = 60; // 游戏时长（秒）

    private int _score = 0;
    private float _timeRemaining;
    private bool _gameOver = false;

    // 单例模式
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _timeRemaining = gameDuration;
        UpdateUI();
        
        // 生成收集物
        SpawnCollectibles();
    }

    void Update()
    {
        if (_gameOver) return;

        _timeRemaining -= Time.deltaTime;
        UpdateUI();

        if (_timeRemaining <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 增加分数
    /// </summary>
    public void AddScore(int points)
    {
        _score += points;
        UpdateUI();
        Debug.Log($"Score: {_score}");
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"分数: {_score}";
        
        if (timeText != null)
            timeText.text = $"时间: {(int)_timeRemaining}s";
    }

    /// <summary>
    /// 生成收集物
    /// </summary>
    void SpawnCollectibles()
    {
        // 创建20个收集物，随机分布在场景中
        for (int i = 0; i < 20; i++)
        {
            GameObject collectible = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            collectible.name = $"Collectible_{i}";
            
            // 随机位置
            float x = Random.Range(-15f, 15f);
            float z = Random.Range(-15f, 15f);
            collectible.transform.position = new Vector3(x, 1f, z);
            
            // 随机大小
            float scale = Random.Range(0.5f, 1.5f);
            collectible.transform.localScale = new Vector3(scale, scale, scale);
            
            // 添加脚本
            collectible.AddComponent<Collectible>();
            
            // 添加碰撞体触发器
            Collider col = collectible.GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    void GameOver()
    {
        _gameOver = true;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        if (finalScoreText != null)
            finalScoreText.text = $"最终分数: {_score}";

        Debug.Log($"Game Over! Final Score: {_score}");
    }

    /// <summary>
    /// 重新开始游戏（用于按钮）
    /// </summary>
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
