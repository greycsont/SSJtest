### 目前情况
当Jump触发时，sliding为false，boost为true

但是他妈的为什么

大概知道了，update太超模了运行多次没fixedupdate用dodge清理boost值，然后inputSystem检测到跳就直接爆了

### 大纲：
#### 三条时间线：
1.fixedUpdate() 根据视频所说大概运行在125fps左右
2.Update() 每帧运行一次
3.InputSystem.UpdateMode 为 ProcessEventsInDynamicUpdate, 及每帧运行一次
InputSystem在Update()之前处理，详情请见 https://docs.unity3d.com/Manual/execution-order.html

#### 假说：
fixedUpdate先执行，在这之后的时间内先松开滑铲，在这之后几帧把sliding状态修改为false，由于绝大部分情况下boost是在fixedUpdate里的dodge()修改所以boost一直为true，之后按下跳跃键，update继续检测到后走向Jump()分支，之后由于boost为true，sliding为false走向了关于冲刺跳的那条线，冲刺跳具体代码实现非常抽象，就字面意义上的冲刺+跳然后减体力
SSJ的判定因为boost为true但条件里要求boost为false所以没有触发

#### 解决方式
能有这个bug说明你的游戏帧率真的很高