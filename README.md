# 🎮 My 3D Game - High Quality Android Game

## 游戏简介
一款使用 **Three.js** 开发的高画质3D安卓收集游戏，采用WebGL渲染技术，支持PC和移动端双端游玩。

![Platform](https://img.shields.io/badge/Platform-Android-green)
![Graphics](https://img.shields.io/badge/Engine-Three.js%20WebGL-blue)
![Quality](https://img.shields.io/badge/Graphics-HQ%20PBR-orange)

## ✨ 游戏特性

### 高画质图形 (AAA级别)
- **PBR材质系统** - 物理基于渲染，金属度/粗糙度控制
- **实时阴影** - PCF软阴影，2048x2048高分辨率
- **全局光照** - 多光源系统（方向光 + 蓝色/橙色点光源）
- **后处理效果** - ACES电影级色调映射 + sRGB色彩空间
- **体积雾效** - 指数雾增加场景深度感
- **抗锯齿** - 多重采样抗锯齿(MSAA)
- **自发光材质** - 收集物和玩家动态发光效果
- **粒子系统** - 收集时的爆炸粒子特效

### 游戏玩法
- ⏱️ **60秒限时挑战** - 在限定时间内获取最高分
- 💎 **三种稀有度收集物**:
  - 🔵 **普通(蓝色)**: 10分
  - 🟡 **稀有(金色)**: 25分  
  - 🔴 **史诗(红色)**: 50分
- 🎯 **物理碰撞检测** - 精确的碰撞系统
- 📊 **实时计分UI** - 动态分数和时间显示

### 双端控制支持
- **PC端**: WASD / 方向键移动
- **手机端**: 触屏滑动控制

## 📱 安装方式

### 方法1: 直接下载APK
1. 下载 `My3DGame_Final.apk` 文件
2. 传输到安卓设备
3. 允许"安装未知来源应用"
4. 打开APK文件安装

### 方法2: 扫码安装
```
[二维码占位]
```

## 🎮 操作指南

| 平台 | 操作 |
|------|------|
| 手机 | 滑动屏幕移动角色 |
| PC | WASD 或 方向键 |

**目标**: 在60秒内收集尽可能多的发光球体！

## 🛠️ 技术栈

| 组件 | 技术 | 版本 |
|------|------|------|
| 3D引擎 | Three.js (WebGL) | r128 |
| Android容器 | WebView (硬件加速) | API 33 |
| 构建工具 | Gradle | 8.14.4 |
| Java版本 | OpenJDK | 17 |
| 目标架构 | arm64-v8a | - |

## 📂 项目结构

```
My3DGame_Android/
├── app/src/main/
│   ├── AndroidManifest.xml      # 应用清单
│   ├── java/.../MainActivity.java  # WebView加载器
│   └── assets/www/
│       └── index.html           # Three.js 3D游戏核心(15KB)
└── build.gradle                 # Gradle构建配置
```

## 🚀 性能优化

- WebGL硬件加速渲染
- PBR材质批量渲染
- 阴影贴图优化 (2048x2048)
- 粒子系统对象池复用
- 响应式设计适配各种屏幕

## 📄 许可证

MIT License - 可自由使用、修改和分发

## 👨‍💻 开发者

AI Assistant (Powered by Godot Engine + Three.js)

---

**享受游戏！如有问题欢迎反馈！** 🎉
