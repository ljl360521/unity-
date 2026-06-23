#include "gui.h"
#include "native_register_vk.h"


static bool s_AntiScreenCapture = true;
void 绘制控件() {
    ImGui::SetNextWindowPos(ImVec2(屏宽/2-750/2, 100), ImGuiCond_FirstUseEver);
    ImGui::SetNextWindowSize(ImVec2(750, 550), ImGuiCond_FirstUseEver);
    ImGui::Begin("Menu");
    ImGuiWindow* win = ImGui::GetCurrentWindow();
    g_MainWindowBounds[0] = win->Pos.x;
    g_MainWindowBounds[1] = win->Pos.y;
    g_MainWindowBounds[2] = win->Pos.x + win->Size.x;
    g_MainWindowBounds[3] = win->Pos.y + win->Size.y;

    if (ImGui::BeginTabBar("Tabs")) {
        if (ImGui::BeginTabItem("设置")) {
            if (ImGui::Checkbox("防录屏/截图", &s_AntiScreenCapture)) {
                setSurfaceSecurity(s_AntiScreenCapture, s_AntiScreenCapture);
            }
            ImGui::EndTabItem();
        }
        ImGui::EndTabBar();
    }

    ImGui::End();

}
