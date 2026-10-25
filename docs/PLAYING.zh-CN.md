# 试玩包运行说明

[Windows x64 / Mono · feedback-v3 发布页](https://github.com/KurosawaGeeker/sfm-recovery/releases/tag/home-street-feedback-v3)

1. 下载 `sfm-home-street-feedback-v3-windows-x64-mono.zip`，完整解压，预留约 3.20 GB 的解压空间。
2. 双击 `SFM-Recovery/Play.cmd`。不要只复制 EXE，也不要直接在压缩软件里运行。
3. 启动器使用窗口模式，并传入此构建所需的 `--sfm-manual-preview` 参数；无需安装 Unity 编辑器。

常用操作为 WASD 移动、Shift 跑步、Tab 菜单，其余以画面提示和游戏输入设置为准。

## 构建形式

本次直接打包现有 `home-street-feedback-v3`，未重新编译、替换或改写其中的游戏 DLL。使用 Unity 2022.3.62f2、Windows x64、Mono 后端，保留 `SFM_OriginalGameplay_Data/Managed/Assembly-CSharp.dll` 的托管 CIL 与可读类型名，未新增混淆、代码加密或 DRM。可以用常规 .NET 反编译工具研究该程序集。原构建没有附带 PDB，本发布不声称提供调试符号或 Development Build 功能。

ZIP 内含运行资源、启动脚本、构建摘要和逐文件 SHA-256 清单；不包含用户存档、凭据、原始录像、本机日志及开发机专用启动器。保存数据由游戏写入当前 Windows 用户的 LocalLow/SFMRecovery 产品目录，安装包不提供他人的进度。

## 验证边界

构建记录为零编译错误、54 个警告。打包时与 LayerFS 归档逐项核对了 168 个原生文件，并从 ZIP 读回验证各文件 SHA-256。没有为了发布复制另一套原生 Player。

这是研究预览，仍有 UI、动画、渲染与声音问题；最新 UI 自动回归及完整连续家—街区玩法尚未全部验收。本轮只验证打包和文件一致性，没有重新完成干净系统首次启动或全流程试玩，不能将压缩包校验通过理解为所有玩法通过。

## 校验

ZIP 大小：1,128,261,613 字节。

SHA-256：`235d3e769db70e73cd346e3a27829269c9cec4c77cc2c1b5e0316ceda7c8173a`

原作资源、第三方软件及引擎组件的权利仍属于各自权利人；项目根目录 MIT 许可不授予这些内容的权利，见 [THIRD_PARTY_NOTICES.md](../THIRD_PARTY_NOTICES.md)。
