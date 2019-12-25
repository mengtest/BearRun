# Introduce
该手游使用了网上的游戏素材，仅为学习使用无任何商业目的。该手游前端采用Unity引擎开发，主要编程语言为C#，基于MVC架构进行设计。Model层主要用于存储游戏内部数据以及与后端服务器进行交互，View层主要用于显示UI界面以及发送UI事件给Controller层，Controller层主要用于响应UI事件，与Model层进行数据交互并更新UI界面。

游戏中的实时排行榜和跑道资源均采用对象池技术进行管理，所有对象池均为一个单例模式类。通过AssetBundles进行资源的动态加载，使安装包体积大幅减少，借助xLua插件实现了补丁修复和开宝箱的热更新需求。

后端采用skynet框架，主要编程语言为Lua，通过sproto协议与客户端进行socket通信，数据库采用mysql，部署在阿里云Centos云服务器上。实现了登录/注册、玩家数据提交、个人信息显示，玩家得分排行榜和资源的动态加载等需求。经测试，安卓端和iOS端均可流畅运行。

# Show
![](https://github.com/neowyj/BearRun/blob/master/BearRun.jpeg)
