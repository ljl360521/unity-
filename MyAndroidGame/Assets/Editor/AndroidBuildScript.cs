using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

/// <summary>
/// Android 命令行构建脚本
/// 使用方法: Unity -batchmode -quit -projectPath <项目路径> -executeMethod AndroidBuildScript.BuildAndroid -logFile build.log
/// </summary>
public class AndroidBuildScript
{
    // 构建配置
    private const string COMPANY_NAME = "MyCompany";
    private const string PRODUCT_NAME = "MyAndroidGame";
    private const string GAME_VERSION = "1.0.0";
    
    // 默认场景名称（必须存在于 Assets/Scenes/ 目录）
    private const string SCENE_NAME = "MainScene";

    /// <summary>
    /// 主构建入口 - 从命令行调用此方法
    /// </summary>
    public static void BuildAndroid()
    {
        Debug.Log("=== 开始 Android 构建 ===");

        try
        {
            // 1. 配置玩家设置
            ConfigurePlayerSettings();

            // 2. 获取或创建场景路径
            string scenePath = EnsureSceneExists();

            // 3. 配置构建设置
            string buildFolder = Path.Combine(Directory.GetCurrentParentDirectory(), "Build");
            string apkName = $"{PRODUCT_NAME}_{GAME_VERSION}.apk";
            string buildPath = Path.Combine(buildFolder, apkName);

            // 确保输出目录存在
            if (!Directory.Exists(buildFolder))
                Directory.CreateDirectory(buildFolder);

            // 4. 构建选项
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = new[] { scenePath },
                locationPathName = buildPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            // 5. 执行构建
            Debug.Log($"正在构建 APK: {buildPath}");
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

            // 6. 输出结果
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("=== 构建成功 ===");
                Debug.Log($"APK 路径: {buildPath}");
                Debug.log($"总大小: {report.summary.totalSize / (1024 * 1024):F2} MB");
                Debug.Log($"总时间: {report.summary.totalTime.TotalSeconds:F1} 秒");
            }
            else
            {
                Debug.LogError("=== 构建失败 ===");
                EditorApplication.Exit(1);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"构建异常: {e.Message}\n{e.StackTrace}");
            EditorApplication.Exit(1);
        }

        // 退出Unity
        EditorApplication.Exit(0);
    }

    /// <summary>
    /// 配置玩家设置（包名、版本等）
    /// </summary>
    static void ConfigurePlayerSettings()
    {
        Debug.Log("配置 Player Settings...");

        // 公司和产品名
        PlayerSettings.companyName = COMPANY_NAME;
        PlayerSettings.productName = PRODUCT_NAME;

        // 包名（Android applicationId）
        PlayerSettings.SetApplicationIdentifier(
            BuildTargetGroup.Android,
            $"com.{COMPANY_NAME.ToLower()}.{PRODUCT_NAME.ToLower()}"
        );

        // 版本号
        PlayerSettings.bundleVersion = GAME_VERSION;
        PlayerSettings.Android.bundleVersionCode = 1;

        // API 兼容性
        PlayerSettings.SetApiCompatibilityLevel(
            BuildTargetGroup.Android,
            ApiCompatibilityLevel.NET_4_6
        );

        // 图形API
        PlayerSettings.SetGraphicsAPIs(
            BuildTarget.Android,
            new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 }
        );

        // 分辨率对话框模式
        PlayerSettings.Android.resizableWindow = false;
        PlayerSettings.defaultScreenWidth = 1920;
        PlayerSettings.defaultScreenHeight = 1080;

        // 设置默认图标（使用Unity内置图标）
        // 注意：实际项目中应该使用自定义图标

        Debug.Log("Player Settings 配置完成");
    }

    /// <summary>
    /// 确保场景存在，返回场景路径
    /// </summary>
    static string EnsureSceneExists()
    {
        string scenePath = $"Assets/Scenes/{SCENE_NAME}.unity";

        if (!File.Exists(scenePath))
        {
            Debug.Log($"创建新场景: {scenePath}");
            
            // 创建新场景并保存
            var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
                UnityEditor.SceneManagement.NewSceneMode.Single
            );
            
            // 添加基本游戏对象到场景
            SetupBasicGameObjects(scene);
            
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, scenePath);
        }
        
        return scenePath;
    }

    /// <summary>
    /// 在场景中设置基本游戏对象
    /// </summary>
    static void SetupBasicGameObjects(UnityEngine.SceneManagement.Scene scene)
    {
        Debug.Log("设置场景内容...");

        // 1. 创建主光源
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = Color.white;
        light.intensity = 1f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // 2. 创建相机
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        camera.backgroundColor = new Color(0.3f, 0.5f, 0.8f); // 天蓝色背景
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.transform.position = new Vector3(0f, 5f, -10f);
        camera.transform.rotation = Quaternion.Euler(20f, 0f, 0f);

        // 3. 创建地面
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(3f, 1f, 3f); // 30x30地面

        // 4. 创建玩家立方体
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.5f, 0f);
        player.AddComponent<Rigidbody>();
        player.GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        player.AddComponent<PlayerController>();

        // 给玩家添加特殊颜色
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
            playerRenderer.material.color = Color.green;

        // 5. 创建游戏管理器
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.AddComponent<GameManager>();

        // 6. 创建Canvas UI
        CreateUI();

        Debug.Log("场景设置完成");
    }

    /// <summary>
    /// 创建UI界面
    /// </summary>
    static void CreateUI()
    {
        // 创建Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // 分数文本
        CreateText(canvasObj.transform, "ScoreText", "分数: 0", 
            new Vector2(100f, 30f), new Vector2(50f, 50f), 24);

        // 时间文本
        CreateText(canvasObj.transform, "TimeText", "时间: 60s", 
            new Vector2(120f, 30f), new Vector2(-170f, 50f), 24);

        // 游戏结束面板
        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.7f); // 半透明黑色
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelObj.SetActive(false); // 初始隐藏

        // 最终分数文本
        Text finalScore = CreateText(panelObj.transform, "FinalScoreText", "最终分数: 0",
            new Vector2(300f, 50f), new Vector2(0f, 40f), 36);
        finalScore.alignment = TextAnchor.MiddleCenter;

        // 重新开始按钮文字
        CreateText(panelObj.transform, "RestartText", "点击屏幕重新开始",
            new Vector2(300f, 30f), new Vector2(0f, -30f), 20).alignment = TextAnchor.MiddleCenter;
    }

    /// <summary>
    /// 辅助方法：创建文本对象
    /// </summary>
    static Text CreateText(Transform parent, string name, string text, Vector2 size, Vector2 position, int fontSize)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = position;

        Text textComponent = textObj.AddComponent<Text>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.alignment = TextAnchor.MiddleLeft;
        textComponent.color = Color.white;

        // 需要一个字体，这里使用Unity默认字体（如果可用）
        // 实际项目中应该指定具体字体文件
        Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (defaultFont != null)
            textComponent.font = defaultFont;

        return textComponent;
    }
}

/// <summary>
/// 辅助类：获取当前目录的父目录
/// </summary>
public static class DirectoryHelper
{
    public static string GetCurrentParentDirectory()
    {
        return Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? Directory.GetCurrentDirectory();
    }
}
