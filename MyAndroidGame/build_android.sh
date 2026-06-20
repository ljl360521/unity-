#!/bin/bash

# ============================================================
# Unity Android Game - 一键构建脚本
# 使用方法: ./build_android.sh
# 前置条件: 已安装 Unity Editor + Android 模块
# ============================================================

set -e  # 遇到错误立即退出

echo "=========================================="
echo "   Unity Android Game Builder"
echo "=========================================="

# 配置变量
UNITY_EDITOR="${UNITY_EDITOR_PATH:-/root/Unity/Hub/Editor/2022.3.22f1/Editor/Unity}"
PROJECT_PATH="$(cd "$(dirname "$0")" && pwd)"
BUILD_PATH="$PROJECT_PATH/Build"
LOG_FILE="$PROJECT_PATH/build.log"
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
APK_NAME="MyAndroidGame_${TIMESTAMP}.apk"

echo "[INFO] 项目路径: $PROJECT_PATH"
echo "[INFO] Unity 编辑器: $UNITY_EDITOR"
echo "[INFO] APK 名称: $APK_NAME"
echo ""

# 检查 Unity 编辑器是否存在
if [ ! -f "$UNITY_EDITOR" ]; then
    echo "[ERROR] Unity 编辑器未找到: $UNITY_EDITOR"
    echo "请安装 Unity 或修改 UNITY_EDITOR_PATH 环境变量"
    exit 1
fi

# 激活许可证（如果需要）
echo "[STEP 1] 检查/激活 Unity 许可证..."
if [ -n "$UNITY_EMAIL" ] && [ -n "$UNITY_PASSWORD" ]; then
    echo "正在激活 Personal 许可证..."
    $UNITY_EDITOR \
        -batchmode \
        -quit \
        -username "$UNITY_EMAIL" \
        -password "$UNITY_PASSWORD" \
        -license personal \
        -logFile "$LOG_FILE" || true
fi

# 构建项目
echo ""
echo "[STEP 2] 构建 Android APK..."
echo "这可能需要几分钟时间，请耐心等待..."
echo ""

$UNITY_EDITOR \
    -batchmode \
    -quit \
    -projectPath "$PROJECT_PATH" \
    -executeMethod AndroidBuildScript.BuildAndroid \
    -logFile "$LOG_FILE" \
    -nographics

# 检查构建结果
if [ $? -eq 0 ]; then
    echo ""
    echo "=========================================="
    echo "   ✅ 构建成功!"
    echo "=========================================="
    
    # 查找生成的APK
    if ls "$BUILD_PATH"/*.apk 1> /dev/null 2>&1; then
        echo ""
        echo "📦 生成的 APK 文件:"
        ls -lh "$BUILD_PATH"/*.apk
        echo ""
        echo "APK 位置: $BUILD_PATH/"
    else
        echo "[WARNING] 未找到 APK 文件，请检查日志: $LOG_FILE"
    fi
    
    exit 0
else
    echo ""
    echo "=========================================="
    echo "   ❌ 构建失败!"
    echo "=========================================="
    echo ""
    echo "请查看日志文件了解详情:"
    echo "  $LOG_FILE"
    echo ""
    echo "常见问题:"
    echo "  1. 未激活 Unity 许可证"
    echo "  2. 未安装 Android Build Support 模块"
    echo "  3. Java SDK 路径配置错误"
    echo ""
    
    # 显示最后50行日志
    echo "--- 最后 50 行日志 ---"
    tail -50 "$LOG_FILE"
    
    exit 1
fi
