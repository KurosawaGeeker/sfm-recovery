# 源码快照与运行条件

## 这是什么

本仓库保留当前权威工程中 C# 文件的原字节，以及代码对应 `.meta` 和程序集定义。`docs/source-manifest.json` 给出全部导出路径、大小与 SHA-256。它用于代码阅读、复核和继续开发，不是“clone 后一键开玩”的完整 Unity 工程。

`Assembly-CSharp` 里既有恢复的方法，也有尚未恢复的声明/占位体；例如 `Corevale_SwapLightmap` 当前仍有空方法。不要以文件数、方法名或编译成功计算恢复率。部分原函数本来就是 no-op，空体是否正确也要核对原生代码。

## 代码入口

| 目录 | 用途 |
| --- | --- |
| `UnityProject/Assets/Scripts/Assembly-CSharp/ExposureUnnoticed2/` | 游戏系统、角色、NPC、场景、UI 与数据相关实现 |
| `UnityProject/Assets/Scripts/Assembly-CSharp/` 其他内容 | 依赖类型、序列化适配和恢复声明等；不是全部原创代码 |
| `UnityProject/Assets/Scripts/SfmMvp/` | 实际 Player 中的可选输入与连续流程观察器 |
| `UnityProject/Assets/MvpBridge/` | 恢复工程桥接和验证辅助 |
| `UnityProject/Assets/Editor/` | 阶段性 Editor 验证、构建和只读探针；部分只适用于原本地目录布局 |

## 运行还缺什么

- Unity **2022.3.62f2**（revision `7670c08855a9`），及有效使用许可。
- 由使用者合法取得的原游戏资源/工程依赖：场景、Prefab、网格、材质、贴图、动画控制器/片段、音频、字体、光照数据等。
- 对应组件/插件依赖。本地包含 UniGLTF/VRM10、Decal、TranslucentImage、Animation Rigging、AI Navigation、Postprocessing 等程序集；本次没有发布这些 DLL。
- 包清单已保留 InputSystem 1.14.2、TextMeshPro 3.0.6 等版本。本地 `packages-lock.json` 指向未发布的 embedded 包，因此不直接复制；应在自己的工程解析依赖，并处理 DLL 与包提供相同程序集的冲突。
- 原场景 GUID 与脚本引用、资源加载路径、Shader 与项目设置的匹配。

有些验证脚本包含原工作环境的绝对路径和隔离 profile 标识，应先阅读并配置自己的路径。不要直接运行会保存或构建的历史脚本去覆盖自己的游戏档。此公开快照没有通过“完全无资源”的 Unity 编译；缺失依赖和资源时出现错误符合该快照的边界。

## 可以立即验证的部分

```sh
python tools/verify-publication.py
```

检查逐文件 hash、代码-only 允许列表、禁止资源扩展名与明显凭据特征。它不证明玩法正确、不代替 Unity 导入/构建、不证明安全扫描没有遗漏。

用量汇总器 `tools/summarize-usage.py` 可对自己可访问的 Codex JSONL 会话复算逐响应账本，参数见 `--help`。本仓库不提供原始会话日志。

## 后续贡献

恢复行为要写清原生目标与定位、证据、实现和运行验收范围。修改 UI 可以改善可读性，但不要同时偷改任务条件、奖励数值或发现机制。游戏和依赖代码的权利范围见根目录许可说明；不要把游戏安装包或资源追加到这个公开仓库。
