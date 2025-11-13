### 目前情况
当Jump触发时，sliding为false，boost为true

但是他妈的为什么

大概知道了，update太超模了运行多次没fixedupdate用dodge清理boost值，然后inputSystem检测到跳就直接爆了

### 项目本身
一些patch用于检测，但神秘扣体力跳还是要你自己搓

左轮的左键会输出当前InputSystem的UpdateMode

记得去BepInEx.cfg里把logging.console的Enabled设置为true，不然你拿什么看输出呢

### 大纲：
#### 三条时间线：
根据视频与Unity官方文档，当前的输入与帧循环情况如下：

1. **FixedUpdate()**
   - 大约运行在 **125 FPS** 左右
   - 时间间隔固定
   - (事实上这个东西才是我最不太确定的，不过哪怕具体运行帧率有误差只要不是那种一秒运行240次以上就无所谓)

2. **Update()**
   - 每帧执行一次
   - 用于处理游戏逻辑、动画、输入读取等

3. **InputSystem**
   - **UpdateMode**: `ProcessEventsInDynamicUpdate`
   - 每帧执行一次，在 **Update() 之前**处理输入事件

官方文档说明: [Event function execution order](https://docs.unity3d.com/Manual/execution-order.html)

#### 假说：
1. **FixedUpdate()**
   - 触发Dodge()，因为sliding为true所以不修改boost的值

2. **FixedUpdate间隔里中间的多帧**
   - InputSystem检测1 : 检测到松开了slide键
   - Update()循环1 : 由于松开了slide键执行了StopSlide()，将sliding设置为false
   - InputSystem检测2 : 检测到按下了跳跃键
   - Update()循环2 ：执行了Jump()，由于boost为true触发了冲刺音效和扣体力值



<img src="./docs/branch_cancelSlideInput.png"/>
<p style="text-align:center; font-size:0.9em;">Fig.1：在Update里的StopSlide分支</p>

<img src="./docs/function_StopSlide.png"/>
<p style="text-align:center; font-size:0.9em;">Fig.2：StopSlide函数</p>

<img src="./docs/branch_boostEqlTrue.png"/>
<p style="text-align:center; font-size:0.9em;">Fig.3: boost为true的分支</p>

#### 避免方式
能有这个bug说明你的游戏帧率真的很高

#### 解决方式 (WIP)
我怎么知道啊，反正FixedUpdate和Update里必须统一一下哪段放哪段

#### 感谢
10_days_till_xmas
Alma