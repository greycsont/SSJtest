[简体中文](README_CN.md)


### Introduction
Normally, the NewMovement.boost will set to false by the `Dodge()` in `FixedUpdate()` if not sliding, but the interval between two Fixedupdate is 8ms (it can be checked by Time.fixedDeltaTime).

The InputSystem's update mode is `ProcessEventsInDynamicUpdate`, which runs input updates right before every `Update()`.  
In `NewMovement.Update()`, releasing the slide calls `StopSliding()` and pressing jump calls `Jump()`.  

If the framerate is too high, it is possible for a release-slide and a jump to occur between two `FixedUpdate()` calls.  
In this case:  
- `StopSliding()` sets `sliding = false`  
- `boost` remains `true` due to the interval  
- `Jump()` goes through the dash-jump branches, consuming stamina and triggering SFX

#### Ideal Procedure

**Situation:** sliding (`sliding = true`, `boost = true`)

1. **FixedUpdate:** sliding, so no changes to `boost`.
2. **Frame 1:**  
   - InputSystem handles release slide  
   - `Update()` calls `StopSlide()`, sets `sliding = false`

3. **Frame 2:**  
   - InputSystem handles press jump  
   - `Update()` calls `Jump()`  
     - `sliding = false`, `boost = true`  
     - SSJ will not trigger because `boost = true`  
     - `Jump()` then sets `boost = false`

Reference:
- [Event function execution order](https://docs.unity3d.com/Manual/execution-order.html)
- [Input System Update Mode](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.16/api/UnityEngine.InputSystem.InputSettings.UpdateMode.html#:~:text=In%20this%20mode%2C%20Update%20%28%29%20must%20be%20called,in%20the%20frame%20explicitly%20at%20an%20exact%20location.)
- [KeyboardState](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.16/api/UnityEngine.InputSystem.LowLevel.KeyboardState.html)
- [SSJ Guide](https://www.youtube.com/watch?v=lwkfebp1_RE)