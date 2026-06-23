#include "gui.h"
#include "native_register_vk.h"

static bool s_AntiScreenCapture = true;
static bool s_ShowAnotherWindow = false;
static float s_UIScale = 1.0f;

void 绘制控件() {
    ImGui::Begin("工具箱");
    ImGuiWindow* win = ImGui::GetCurrentWindow();
    g_MainWindowBounds[0] = win->Pos.x;
    g_MainWindowBounds[1] = win->Pos.y;
    g_MainWindowBounds[2] = win->Pos.x + win->Size.x;
    g_MainWindowBounds[3] = win->Pos.y + win->Size.y;

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

            if (ImGui::SliderFloat("UI大小", &s_UIScale, 0.5f, 2.0f, "%.1f"))
            {
                ImGuiStyle& style = ImGui::GetStyle();
                style.ScaleAllSizes(s_UIScale);
                ImGui::GetIO().FontGlobalScale = s_UIScale;
            }

            ImGui::EndTabItem();
        }

        ImGui::EndTabBar();
    }

    ImGui::End();

    // 另一个窗口
    if (s_ShowAnotherWindow)
    {
        ImGui::Begin("另一个窗口", &s_ShowAnotherWindow);
        ImGui::Text("来自另一个窗口的问候！");
        if (ImGui::Button("关闭我"))
            s_ShowAnotherWindow = false;
        ImGui::End();
    }
}
