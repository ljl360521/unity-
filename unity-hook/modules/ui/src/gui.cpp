#include "gui.h"
#include "native_register_vk.h"

static bool s_AntiScreenCapture = true;
static bool s_ShowDemoWindow = true;
static bool s_ShowAnotherWindow = false;
static float s_FloatValue = 0.0f;
static int s_Counter = 0;
static ImVec4 s_ClearColor = ImVec4(0.45f, 0.55f, 0.60f, 1.00f);

void 绘制控件() {
    ImGuiIO& io = ImGui::GetIO();

    ImGui::Begin("你好，世界！");
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
            ImGui::Text("这是一些有用的文本。");
            ImGui::Checkbox("演示窗口", &s_ShowDemoWindow);
            ImGui::Checkbox("另一个窗口", &s_ShowAnotherWindow);

            ImGui::SliderFloat("浮点数", &s_FloatValue, 0.0f, 1.0f);
            ImGui::ColorEdit3("清除颜色", (float*)&s_ClearColor);

            if (ImGui::Button("按钮"))
                s_Counter++;
            ImGui::SameLine();
            ImGui::Text("计数 = %d", s_Counter);

            ImGui::Text("应用程序平均 %.3f 毫秒/帧 (%.1f FPS)", 1000.0f / io.Framerate, io.Framerate);

            ImGui::EndTabItem();
        }

        // ---- 设置标签页 ----
        if (ImGui::BeginTabItem("设置"))
        {
            if (ImGui::Checkbox("防录屏", &s_AntiScreenCapture)) {
                setSurfaceSecurity(s_AntiScreenCapture, s_AntiScreenCapture);
            }
            ImGui::EndTabItem();
        }

        ImGui::EndTabBar();
    }

    ImGui::End();

    // 演示窗口
    if (s_ShowDemoWindow)
        ImGui::ShowDemoWindow(&s_ShowDemoWindow);

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
