# MyAndroidGame - Unity 3D 收集游戏 (Android)

## 📱 项目简介

这是一个使用 **Unity 2022.3.22f1** 开发的简单 **3D 收集游戏**，专为 **Android 平台** 设计。

### 游戏特性

- ✅ **3D 图形渲染** - 使用 URP (Universal Render Pipeline)
- ✅ **触屏控制支持** - 专为移动设备优化
- ✅ **计分系统** - 实时分数统计
- ✅ **倒计时模式** - 60秒限时挑战
- ✅ **UI 界面** - 分数、时间、游戏结束画面
- ✅ **物理系统** - 刚体碰撞检测

---

## 🎮 游戏玩法

1. **控制绿色立方体（玩家）**
   - PC端：WASD / 方向键移动，Q/E旋转
   - 移动端：触摸拖动控制

2. **收集黄色胶囊物体**
   - 每个收集物 +10 分
   - 物体会自转和浮动动画

3. **60秒内获取最高分**
   - 时间结束显示最终成绩
   - 可点击重新开始

---

## 🛠️ 开发环境要求

| 组件 | 版本/要求 |
|------|----------|
| Unity Editor | 2022.3.22f1 (LTS) |
| Unity Module | Android Build Support |
| JDK | OpenJDK 11 (随Unity安装) |
| Android SDK | Platform 31 & 32 |
| Android NDK | r23b |
| 操作系统 | Windows / macOS / Linux |

---

## 📦 安装步骤

### 方法一：使用命令行工具（推荐）

```bash
# 1. 安装 Unity CLI
curl -fsSL https://public-cdn.cloud.unity3d.com/hub/prod/cli/install.sh | bash

# 2. 安装 npm 工具包
npm install -g @rage-against-the-pixel/unity-cli

# 3. 安装 Unity Hub + Editor + Android模块
unity-cli hub-install
unity-cli setup-unity -u 2022.3.22f1 -m android
```

### 方法二：手动安装

1. 从 [Unity Download](https://unity.com/download) 下载 Unity Hub
2. 打开 Unity Hub → Installs → Install Editor → 选择 **2022.3.22f1 LTS**
3. 在安装时勾选 **Android Build Support** 模块
4. 安装完成后添加 Android SDK 和 NDK

---

## 🚀 构建指南

### 快速构建（一键脚本）

```bash
# 1. 进入项目目录
cd MyAndroidGame

# 2. 添加执行权限
chmod +x build_android.sh

# 3. 运行构建（首次需要激活许可证）
./build_android.sh

# 如果有 Unity 账号，设置环境变量后运行：
export UNITY_EMAIL="your-email@example.com"
export UNITY_PASSWORD="your-password"
./build_android.sh
```

### 手动构建

```bash
# 使用 Unity 命令行构建
/path/to/Unity/Hub/Editor/2022.3.22f1/Editor/Unity \
  -batchmode \
  -quit \
  -projectPath /path/to/MyAndroidGame \
  -executeMethod AndroidBuildScript.BuildAndroid \
  -logFile build.log \
  -nographics
```

### 在 Unity Editor 中构建

1. 打开 Unity Hub
2. 点击 **Add** 添加项目路径
3. 点击 **Open** 打开项目
4. 菜单栏 → **File → Build Settings...**
5. 选择平台 **Android**
6. 点击 **Switch Platform**（如果尚未切换）
7. 确保 **Scenes/SampleScene** 已添加到构建列表
8. 点击 **Build And Run** 或 **Build**

---

## 📂 项目结构

```
MyAndroidGame/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity          # 主场景
│   ├── Scripts/
│   │   ├── PlayerController.cs      # 玩家控制器
│   │   ├── Collectible.cs           # 收集物逻辑
│   │   └── GameManager.cs           # 游戏管理器
│   └── Editor/
│       └── AndroidBuildScript.cs    # Android构建脚本
├── ProjectSettings/                  # Unity项目设置
├── build_android.sh                 # 一键构建脚本
├── README.md                        # 本文档
└── Build/                           # 构建输出目录（生成后）
    └── MyAndroidGame_*.apk          # 最终APK文件
```

---

## 🔧 核心代码说明

### PlayerController.cs
玩家控制器，处理移动输入和物理运动：
- 支持 WASD/方向键 和 触摸输入
- Q/E 键旋转角色
- Rigidbody 物理驱动

### Collectible.cs
收集物组件：
- 自动旋转 + 浮动动画
- 碰撞触发器检测
- 分数增加 + 缩放消失特效

### GameManager.cs
游戏管理器：
- 计分系统
- 60秒倒计时
- UI 更新
- 游戏结束逻辑
- 场景初始化（自动生成20个收集物）

### AndroidBuildScript.cs
编辑器构建脚本：
- 命令行调用入口 (`BuildAndroid()`)
- 自动配置 Player Settings
- 动态创建场景和游戏对象
- 生成标准 APK 文件

---

## ⚙️ 自定义配置

### 修改游戏参数

编辑 `Assets/Scripts/GameManager.cs`：

```csharp
public int gameDuration = 60;  // 游戏时长（秒）
// 改为 120 表示2分钟模式
```

编辑 `Assets/Scripts/PlayerController.cs`：

```csharp
public float moveSpeed = 10f;     // 移动速度
public float rotateSpeed = 100f;  // 旋转速度
```

### 修改 Android 设置

编辑 `Assets/Editor/AndroidBuildScript.cs`：

```csharp
private const string COMPANY_NAME = "MyCompany";  // 公司名称
private const string PRODUCT_NAME = "MyAndroidGame"; // 产品名称
private const string GAME_VERSION = "1.0.0";       // 版本号
```

### 修改包名

默认包名：`com.mycompany.myandroidgame`

可在 `ConfigurePlayerSettings()` 方法中修改 `SetApplicationIdentifier` 参数。

---

## 🐛 常见问题

### Q1: 提示 "No valid Unity license found"
**A:** 需要激活 Unity 许可证：
- Personal版（免费）：[Unity Personal](https://unity.com/download)
- 在脚本中设置 `UNITY_EMAIL` 和 `UNITY_PASSWORD` 环境变量

### Q2: Android SDK not found
**A:** 确保安装了 Android Build Support 模块：
```bash
unity-cli setup-unity -u 2022.3.22f1 -m android
```

### Q3: 构建失败 "Failed to compile scripts"
**A:** 检查 .NET API 兼容性等级：
- Edit → Project Settings → Player → Other Settings
- Api Compatibility Level → .NET Framework

### Q4: APK 安装到手机提示 "应用未安装"
**A:** 
- 确保手机开启了"允许安装未知来源应用"
- 签名问题：需要在 Build Settings 中配置 Keystore

---

## 📄 许可证

本项目使用 MIT License。

---

## 🙏 致谢

- [Unity Technologies](https://unity.com/) - 游戏引擎
- [@rage-against-the-pixel/unity-cli](https://www.npmjs.com/package/@rage-against-the-pixel/unity-cli) - CLI 工具

---

**祝游戏开发愉快！🎮✨**

如有问题，欢迎提交 Issue 或 Pull Request。
