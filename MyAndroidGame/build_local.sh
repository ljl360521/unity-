#!/bin/bash

# ============================================================
#   Unity Android Game - 本地快速构建脚本
#   适用于: Windows / macOS / Linux
# ============================================================

set -e

echo "=============================================="
echo "   🎮 Unity Android Game - 本地构建工具"
echo "=============================================="
echo ""

# 检测操作系统
OS="$(uname -s)"
case "$OS" in
    Darwin*)     PLATFORM="macos";;
    Linux*)      PLATFORM="linux";;
    CYGWIN*|MINGW*|MSYS*) PLATFORM="windows";;
    *)          PLATFORM="unknown";;
esac

echo "[INFO] 检测到系统: $PLATFORM"

# ============================================================
# 步骤 1: 检查/安装 Unity Hub
# ============================================================
echo ""
echo "[步骤 1/5] 检查 Unity Hub..."

if command -v unityhub &> /dev/null; then
    echo "✅ Unity Hub 已安装"
else
    echo "⚠️  未检测到 Unity Hub"
    echo ""
    echo "请手动安装 Unity Hub:"
    if [ "$PLATFORM" = "windows" ]; then
        echo "   下载: https://unity.com/download#download-windows"
        echo "   运行: UnityHubSetup.exe"
    elif [ "$PLATFORM" = "macos" ]; then
        echo "   下载: https://unity.com/download#download-mac"
        echo "   运行: UnityHubSetup.dmg"
    else
        echo "   下载: https://unity.com/download#download-linux"
        echo "   运行: UnityHub.AppImage 或 .deb 包"
    fi
    echo ""
    read -p "安装完成后按回车继续..." 
fi

# ============================================================
# 步骤 2: 检查 Unity Editor
# ============================================================
echo ""
echo "[步骤 2/5] 检查 Unity Editor..."

UNITY_EDITOR=""
if [ "$PLATFORM" = "windows" ]; then
    # Windows 默认路径
    for path in \
        "C:/Program Files/Unity/Hub/Editor/2022.3.22f1/Editor/Unity.exe" \
        "$LOCALAPPDATA/Unity/Hub/Editor/2022.3.22f1/Editor/Unity.exe"; do
        if [ -f "$path" ]; then
            UNITY_EDITOR="$path"
            break
        fi
    done
elif [ "$PLATFORM" = "macos" ]; then
    # macOS 默认路径
    UNITY_EDITOR="/Applications/Unity/Hub/Editor/2022.3.22f1/Unity.app/Contents/MacOS/Unity"
else
    # Linux 默认路径
    UNITY_EDITOR="$HOME/Unity/Hub/Editor/2022.3.22f1/Editor/Unity"
fi

if [ -n "$UNITY_EDITOR" ] && [ -f "$UNITY_EDITOR" ]; then
    echo "✅ Unity Editor 已安装: $UNITY_EDITOR"
else
    echo "⚠️  未检测到 Unity Editor 2022.3.22f1"
    echo ""
    echo "请通过 Unity Hub 安装:"
    echo "   1. 打开 Unity Hub"
    echo "   2. 进入 'Installs' 标签页"
    echo "   3. 点击 'Install Editor'"
    echo "   4. 选择版本: **2022.3.22f1 (LTS)**"
    echo "   5. 勾选模块: **Android Build Support**"
    echo "   6. 点击 Install"
    echo ""
    read -p "安装完成后按回车继续..."
    
    # 重新检测
    if [ "$PLATFORM" = "linux" ] && [ -f "$UNITY_EDITOR" ]; then
        echo "✅ 检测到 Unity Editor"
    fi
fi

# ============================================================
# 步骤 3: 登录/激活许可证
# ============================================================
echo ""
echo "[步骤 3/5] Unity 许可证状态..."

# 尝试检查许可证（需要运行一次Unity）
echo "提示: 首次使用需要登录 Unity 账号"
echo "       如果尚未登录，Unity 会弹出登录窗口"
echo ""
read -p "是否已登录 Unity 账号？(y/n): " -n 1 -r
echo ""

if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo ""
    echo "请在 Unity Hub 中登录:"
    echo "   1. 点击右上角 'Sign In'"
    echo "   2. 输入你的账号: 2901685818@qq.com"
    echo "   3. 输入密码"
    echo "   4. 选择许可类型: **Personal** (免费)"
    echo ""
    read -p "登录完成后按回车继续..."
fi

# ============================================================
# 步骤 4: 打开项目
# ============================================================
echo ""
echo "[步骤 4/5] 准备游戏项目..."

PROJECT_PATH="$(cd "$(dirname "$0")" && pwd)"
echo "项目路径: $PROJECT_PATH"

if [ ! -d "$PROJECT_PATH/Assets" ]; then
    echo "❌ 错误: 未找到 Unity 项目文件"
    exit 1
fi

echo "✅ 项目文件完整"

# ============================================================
# 步骤 5: 构建 APK
# ============================================================
echo ""
echo "[步骤 5/5] 开始构建 Android APK..."
echo ""

BUILD_FOLDER="$PROJECT_PATH/Build"
mkdir -p "$BUILD_FOLDER"

APK_NAME="MyAndroidGame_$(date +%Y%m%d_%H%M%S).apk"
BUILD_PATH="$BUILD_FOLDER/$APK_NAME"

echo "输出文件: $BUILD_PATH"
echo ""
echo "正在调用 Unity 构建系统..."
echo "（这可能需要 2-5 分钟，请耐心等待）"
echo ""

# 执行构建
if [ -n "$UNITY_EDITOR" ] && [ -f "$UNITY_EDITOR" ]; then
    "$UNITY_EDITOR" \
        -batchmode \
        -quit \
        -projectPath "$PROJECT_PATH" \
        -executeMethod AndroidBuildScript.BuildAndroid \
        -logFile "$PROJECT_PATH/build.log" \
        -nographics
    
    BUILD_EXIT_CODE=$?
else
    echo "⚠️  未找到 Unity Editor，请手动构建:"
    echo ""
    echo "   方法 A - 使用 Unity Editor GUI:"
    echo "     1. 打开 Unity Hub"
    echo "     2. 点击 'Add' 添加项目路径:"
    echo "        $PROJECT_PATH"
    echo "     3. 点击 'Open' 打开项目"
    echo "     4. 菜单栏 → File → Build Settings..."
    echo "     5. 选择平台: Android"
    echo "     6. 点击 'Switch Platform' (如需要)"
    echo "     7. 确保 MainScene 在构建列表中"
    echo "     8. 点击 'Build And Run' 或 'Build'"
    echo ""
    echo "   方法 B - 使用命令行 (需要先找到Unity路径):"
    echo "     /path/to/Unity \\"
    echo "       -batchmode -quit \\"
    echo "       -projectPath $PROJECT_PATH \\"
    echo "       -executeMethod AndroidBuildScript.BuildAndroid \\"
    echo "       -logFile build.log"
    
    read -p "按回键退出..."
    exit 0
fi

# 检查构建结果
echo ""
echo "=============================================="
if [ $BUILD_EXIT_CODE -eq 0 ]; then
    echo "   ✅ 构建成功!"
    echo "=============================================="
    echo ""
    
    # 查找生成的APK
    if ls "$BUILD_FOLDER"/*.apk 1> /dev/null 2>&1; then
        echo "📦 生成的 APK 文件:"
        ls -lh "$BUILD_FOLDER"/*.apk
        echo ""
        echo "APK 位置: $BUILD_FOLDER/"
        
        # 尝试打开文件夹
        if [ "$PLATFORM" = "macos" ]; then
            open "$BUILD_FOLDER"
        elif [ "$PLATFORM" = "linux" ]; then
            xdg-open "$BUILD_FOLDER" 2>/dev/null || true
        fi
    else
        echo "⚠️  未找到 APK 文件，请查看日志:"
        echo "   $PROJECT_PATH/build.log"
    fi
else
    echo "   ❌ 构建失败 (退出码: $BUILD_EXIT_CODE)"
    echo "=============================================="
    echo ""
    echo "请查看日志了解详情:"
    echo "   $PROJECT_PATH/build.log"
    echo ""
    echo "--- 最后 50 行日志 ---"
    tail -50 "$PROJECT_PATH/build.log" 2>/dev/null || echo "日志文件不存在"
fi

echo ""
read -p "按回键退出..."
