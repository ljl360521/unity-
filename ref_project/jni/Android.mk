LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

LOCAL_MODULE := ImGuiExample

# C++11 只给 C++ 文件用
LOCAL_CPPFLAGS := -std=c++11

# 宏定义
LOCAL_CFLAGS += -DIMGUI_IMPL_OPENGL_ES3

# 源文件
LOCAL_SRC_FILES := \
    main.cpp \
    imgui/imgui.cpp \
    imgui/imgui_demo.cpp \
    imgui/imgui_draw.cpp \
    imgui/imgui_tables.cpp \
    imgui/imgui_widgets.cpp \
    imgui/backends/imgui_impl_android.cpp \
    imgui/backends/imgui_impl_opengl3.cpp \
    $(NDK_ROOT)/sources/android/native_app_glue/android_native_app_glue.c

# 头文件路径
LOCAL_C_INCLUDES := \
    $(LOCAL_PATH)/imgui \
    $(LOCAL_PATH)/imgui/backends \
    $(NDK_ROOT)/sources/android/native_app_glue

# 链接库
LOCAL_LDLIBS := \
    -landroid \
    -lEGL \
    -lGLESv3 \
    -llog

# 导出 ANativeActivity_onCreate
LOCAL_LDFLAGS += -u ANativeActivity_onCreate

include $(BUILD_SHARED_LIBRARY)
