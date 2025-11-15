using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SSJtest;

[HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))]
public static class Revolver_Shoot_Patch
{
    static void Prefix()
    {
        Plugin.Logger.LogInfo("Input System Update Mode: " + InputSystem.settings.updateMode);
        Plugin.Logger.LogInfo("Current Boost value: " + NewMovement.instance.boost);
        Plugin.Logger.LogInfo("fixedDeltaTime: " + Time.fixedDeltaTime);
        Plugin.Logger.LogInfo("----");
    }
}

[HarmonyPatch(typeof(Nailgun), nameof(Nailgun.Shoot))]
public static class Nailgun_Shoot_Patch
{
    static void Prefix()
    {
        NewMovement.instance.boostLeft = 10f;
        Plugin.Logger.LogInfo("Toogled boostLeft to: " + NewMovement.instance.boostLeft);
    }
}

[HarmonyPatch(typeof(Railcannon), nameof(Railcannon.Shoot))]
public static class Railcannon_Shoot_Patch
{
    static void Prefix()
    {
        Settings.replaceUpdate = true;
        Plugin.Logger.LogInfo("Toggled replaceFixedUpdate to: " + Settings.replaceFixedUpdate);
    }
}

[HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Shoot))]
public static class RocketLauncher_Shoot_Patch
{
    static void Prefix()
    {
        NewMovement.instance.boost = !NewMovement.instance.boost;
        Plugin.Logger.LogInfo("Toggled Boost value to: " + NewMovement.instance.boost);
    }
}
