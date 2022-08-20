# Unity 框架的 UI 框架

# 游戏流程管理
1. 框架GameLogic 入口
2. TODO : Mock 数据， 方便UI 界面自动化测试

# 功能
1. UI Manager 统筹管理UI 问题，  管理所有界面的生命周期：生成，展示，销毁，查找， UI 界面切换， 选择性预加载UI;
2. 为所有UI 提供继承基类， 对UI界面国过度动画，预加载提供支持。 TODO ： UI 组件自动绑定。
3. TODO: 网络异步接口支持, 接到网络回复数据才打开
4. TODO : 事件机制， 不同界面之间的事件收发，界面与网络层的交互
5. 动画系统的接入
6. 资源加载系统的接入：
7. 配置系统的接入
8. TODO 内置新手引导系统
9. TODO 红点系统支持
10. 扩展组件支持
     * UIList
     * 3 渲 2
     * 多语言文本
     * 聊天系统
     * HUD
     * UI特效 粒子系统 https://github.com/mob-sakai/ParticleEffectForUGUI ,https://stackoverflow.com/questions/58830542/unity-particle-effects-on-canvas
     * UISpine 支持

11.TODO 渲染顺序支持
     * Page 下 不同UI Object 支持