#include "gui.h"
#include "native_register_vk.h"

static bool s_AntiScreenCapture = true;
static bool s_ShowAnotherWindow = false;
static float s_UIScale = 1.0f;
static float s_UIScaleLast = 1.0f;

// 额外窗口边界追踪（定义在 native_register_vk.cpp）
extern float g_SecondaryWindowBounds[4];
extern bool  g_SecondaryWindowVisible;

void 绘制控件() {
    ImGuiIO& io = ImGui::GetIO();

    ImGui::SetNextWindowPos(ImVec2(屏宽/2-750/2, 100), ImGuiCond_FirstUseEver);
    ImGui::SetNextWindowSize(ImVec2(750, 550), ImGuiCond_FirstUseEver);
    ImGui::Begin("工具箱");
    ImGuiWindow* win = ImGui::GetCurrentWindow();
    g_MainWindowBounds[0] = win->Pos.x;
    g_MainWindowBounds[1] = win->Pos.y;
    g_MainWindowBounds[2] = win->Pos.x + win->Size.x;
    g_MainWindowBounds[3] = win->Pos.y + win->Size.y;

    // 标签栏使用更大的间距，避免标签页连在一起
    ImGui::PushStyleVar(ImGuiStyleVar_ItemSpacing, ImVec2(20.0f, ImGui::GetStyle().ItemSpacing.y));
    if (ImGui::BeginTabBar("MainTabBar"))
    {
        // ---- 功能标签页 ----
        if (ImGui::BeginTabItem("功能"))
        {
            ImGui::Checkbox("另一个窗口", &s_ShowAnotherWindow);
            ImGui::EndTabItem();
        }

        // ---- 设置标签页 ----
        if (ImGui::BeginTabItem("设置"))
        {
            if (ImGui::Checkbox("防录屏", &s_AntiScreenCapture)) {
                setSurfaceSecurity(s_AntiScreenCapture, s_AntiScreenCapture);
            }

            ImGui::Separator();

            if (ImGui::SliderFloat("UI大小", &s_UIScale, 0.5f, 2.0f, "%.1f"))
            {
                float ratio = s_UIScale / s_UIScaleLast;
                ImGui::GetStyle().ScaleAllSizes(ratio);
                io.FontGlobalScale = s_UIScale;
                s_UIScaleLast = s_UIScale;
            }

            ImGui::EndTabItem();
        }

        ImGui::EndTabBar();
    }
    ImGui::PopStyleVar();

    ImGui::End();

    // 另一个窗口
    g_SecondaryWindowVisible = false;
    if (s_ShowAnotherWindow)
    {
        ImGui::Begin("另一个窗口", &s_ShowAnotherWindow);
        ImGui::Text("来自另一个窗口的问候！");
        if (ImGui::Button("关闭我"))
            s_ShowAnotherWindow = false;
        ImGuiWindow* win2 = ImGui::GetCurrentWindow();
        g_SecondaryWindowBounds[0] = win2->Pos.x;
        g_SecondaryWindowBounds[1] = win2->Pos.y;
        g_SecondaryWindowBounds[2] = win2->Pos.x + win2->Size.x;
        g_SecondaryWindowBounds[3] = win2->Pos.y + win2->Size.y;
        g_SecondaryWindowVisible = true;
        ImGui::End();
    }
}
