using Verse;
using HarmonyLib;
using System.Diagnostics.Eventing.Reader;
using System;
using System.Runtime;
using static UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure;
using UnityEngine;

namespace BigAndSmall
{
    [StaticConstructorOnStartup]
    public partial class ZombieApocalypse : Mod
    {

        public static ZASettings settings = null;
        public ZombieApocalypse(ModContentPack content) : base(content)
        {
            settings = GetSettings<ZASettings>();
            ApplyHarmonyPatches();
            
        }

        static void ApplyHarmonyPatches()
        {
            var harmony = new Harmony("RedMattis.BigSmallZombieApocalypse");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.DeadRisingChance), MethodType.Getter)]
        public static class DeadRisingChancePatch
        {
            public static void DeadRisingChance(ref float __result)
            {
                __result =  settings.deadRisingChance;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.ZombieApocalypseChance), MethodType.Getter)]
        public static class ZombieApocalypseChancePatch
        {
            [HarmonyPostfix]
            public static void ZombieApocalypseChance(ref float __result)
            {
                //Log.Warning($"DEBUG: ZombieApocalypseChancePatch: {settings.zombieApocalypseChance}");
                __result = settings.zombieApocalypseChance;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.ReturnChance), MethodType.Getter)]
        public static class ReturnChancePatch
        {
            [HarmonyPostfix]
            public static void ReturnChance(ref float __result)
            {
                __result = settings.returnChance;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.ReturnChanceApoc), MethodType.Getter)]
        public static class ReturnChanceApocPatch
        {
            [HarmonyPostfix]
            public static void ReturnChanceApoc(ref float __result)
            {
                __result = settings.returnChanceApoc;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.ReturnChanceColonist), MethodType.Getter)]
        public static class ReturnChanceColonistPatch
        {
            [HarmonyPostfix]
            public static void ReturnChanceColonist(ref float __result)
            {
                __result = settings.returnChanceColonist;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.BerserkChance), MethodType.Getter)]
        public static class BerserkChancePatch
        {
            [HarmonyPostfix]
            public static void BerserkChance(ref float __result)
            {
                __result = settings.berserkChance;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.BerserkChanceApoc), MethodType.Getter)]
        public static class BerserkChanceApocPatch
        {
            [HarmonyPostfix]
            public static void BerserkChanceApoc(ref float __result)
            {
                __result = settings.berserkChanceApoc;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.BerserkLoseFaction), MethodType.Getter)]
        public static class BerserkLoseFactionPatch
        {
            [HarmonyPostfix]
            public static void BerserkLoseFaction(ref float __result)
            {
                __result = settings.berserkLoseFaction;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.BerserkLoseFactionApoc), MethodType.Getter)]
        public static class BerserkLoseFactionApocPatch
        {
            [HarmonyPostfix]
            public static void BerserkLoseFactionApoc(ref float __result)
            {
                __result = settings.berserkLoseFactionApoc;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.PychoticWanderingChance), MethodType.Getter)]
        public static class PychoticWanderingChancePatch
        {
            [HarmonyPostfix]
            public static void PychoticWanderingChance(ref float __result)
            {
                __result = settings.pychoticWanderingChance;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.PermanentBerserk), MethodType.Getter)]
        public static class PermanentBerserkPatch
        {
            [HarmonyPostfix]
            public static void PermanentBerserk(ref float __result)
            {
                __result = settings.chanceBerserkIsPermanent;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.PermanentBerserkApoc), MethodType.Getter)]
        public static class PermanentBerserkApocPatch
        {
            [HarmonyPostfix]
            public static void PermanentBerserkApoc(ref float __result)
            {
                __result = settings.chanceBerserkIsPermanentApoc;
            }
        }

        [HarmonyPatch(typeof(VUReturning), nameof(VUReturning.ManhunterChance), MethodType.Getter)]
        public static class ManhunterChancePatch
        {
            [HarmonyPostfix]
            public static void ManhunterChance(ref float __result)
            {
                __result = 1 - settings.chanceManhunter;
            }
        }

        private static Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            //base.DoSettingsWindowContents(inRect);

            Listing_Standard listStd = new Listing_Standard();

            Rect rect = inRect.ContractedBy(10f);
            rect.height -= listStd.CurHeight;
            rect.y += listStd.CurHeight;
            Widgets.DrawBoxSolid(rect, Color.grey);
            Rect rect2 = rect.ContractedBy(1f);
            Widgets.DrawBoxSolid(rect2, new ColorInt(42, 43, 44).ToColor);
            Rect rect3 = rect2.ContractedBy(5f);
            //rect3.y += 15f;
            //rect3.height -= 15f;
            Rect rect4 = rect3;
            rect4.x = 0f;
            rect4.y = 0f;
            rect4.width -= 20f;
            rect4.height = 950f;
            Widgets.BeginScrollView(rect3, ref scrollPosition, rect4);
            listStd.Begin(rect4.AtZero());

            // Add a new button to set settings to their defaults.
            if (listStd.ButtonText("BSZA_DefaultSettings".Translate()))
            {
                SetDefault();
            }

            listStd.GapLine();
            listStd.Label("BSZA_Events".Translate());
            listStd.GapLine();

            // Settings
            listStd.Label("BSZA_DeadRisingChance".Translate() + ": " + settings.deadRisingChance.ToStringPercent());
            settings.deadRisingChance = listStd.Slider(settings.deadRisingChance, 0f, 1f);

            listStd.Label("BSZA_ZombieApocalypseChance".Translate() + ": " + settings.zombieApocalypseChance.ToStringPercent());
            settings.zombieApocalypseChance = listStd.Slider(settings.zombieApocalypseChance, 0f, 1f);

            listStd.GapLine();
            listStd.Label("BSZA_ReanimationChance".Translate());
            listStd.GapLine();

            listStd.Label("BSZA_ReturnChance".Translate() + ": " + settings.returnChance.ToStringPercent());
            settings.returnChance = listStd.Slider(settings.returnChance, 0f, 1f);

            listStd.Label("BSZA_ReturnChanceApoc".Translate() + ": " + settings.returnChanceApoc.ToStringPercent());
            settings.returnChanceApoc = listStd.Slider(settings.returnChanceApoc, 0f, 1f);

            listStd.Label("BSZA_ReturnChanceColonist".Translate() + ": " + settings.returnChanceColonist.ToStringPercent());
            settings.returnChanceColonist = listStd.Slider(settings.returnChanceColonist, 0f, 1f);

            listStd.GapLine();
            listStd.Label("BSZA_MentalStates".Translate());
            listStd.GapLine();

            listStd.Label("BSZA_PychoticWanderingChance".Translate() + ": " + settings.pychoticWanderingChance.ToStringPercent());
            settings.pychoticWanderingChance = listStd.Slider(settings.pychoticWanderingChance, 0f, 1f);

            listStd.Label("BSZA_BerserkChance".Translate() + ": " + settings.berserkChance.ToStringPercent());
            settings.berserkChance = listStd.Slider(settings.berserkChance, 0f, 1f);

            listStd.Label("BSZA_BerserkChanceApoc".Translate() + ": " + settings.berserkChanceApoc.ToStringPercent());
            settings.berserkChanceApoc = listStd.Slider(settings.berserkChanceApoc, 0f, 1f);

            listStd.Label("BSZA_BerserkPermanent".Translate() + ": " + settings.chanceBerserkIsPermanent.ToStringPercent());
            settings.chanceBerserkIsPermanent = listStd.Slider(settings.chanceBerserkIsPermanent, 0f, 1f);

            listStd.Label("BSZA_BerserkPermanentApoc".Translate() + ": " + settings.chanceBerserkIsPermanentApoc.ToStringPercent());
            settings.chanceBerserkIsPermanentApoc = listStd.Slider(settings.chanceBerserkIsPermanentApoc, 0f, 1f);

            listStd.Label("BSZA_BerserkLoseFaction".Translate() + ": " + settings.berserkLoseFaction.ToStringPercent());
            settings.berserkLoseFaction = listStd.Slider(settings.berserkLoseFaction, 0f, 1f);

            listStd.Label("BSZA_BerserkLoseFactionApoc".Translate() + ": " + settings.berserkLoseFactionApoc.ToStringPercent());
            settings.berserkLoseFactionApoc = listStd.Slider(settings.berserkLoseFactionApoc, 0f, 1f);

            listStd.Label("BSZA_Manhunter".Translate() + ": " + settings.chanceManhunter.ToStringPercent());
            settings.chanceManhunter = listStd.Slider(settings.chanceManhunter, 0f, 1f);

            listStd.End();
            Widgets.EndScrollView();

            base.DoSettingsWindowContents(inRect);
        }

        private void SetDefault()
        {
            settings.deadRisingChance = ZASettings.deadRisingChanceDefault;
            settings.zombieApocalypseChance = ZASettings.zombieApocalypseChanceDefault;
            settings.returnChance = ZASettings.returnChanceDefault;
            settings.returnChanceApoc = ZASettings.returnChanceApocDefault;
            settings.returnChanceColonist = ZASettings.returnChanceColonistDefault;
            settings.pychoticWanderingChance = ZASettings.pychoticWanderingChanceDefault;
            settings.berserkChance = ZASettings.berserkChanceDefault;
            settings.berserkChanceApoc = ZASettings.berserkChanceApocDefault;
            settings.berserkLoseFaction = ZASettings.berserkLoseFactionDefault;
            settings.berserkLoseFactionApoc = ZASettings.berserkLoseFactionApocDefault;
            settings.chanceBerserkIsPermanent = ZASettings.chanceBerserkIsPermanentDefault;
            settings.chanceBerserkIsPermanentApoc = ZASettings.chanceBerserkIsPermanentApocDefault;
            settings.chanceManhunter = ZASettings.chanceManhunterDefault;

        }

        public override string SettingsCategory()
        {
            return "BSZA_VengefulDead".Translate();
        }

    }

    public class ZASettings : ModSettings
    {
        // Settings
        public float deadRisingChance = deadRisingChanceDefault;
        public float zombieApocalypseChance = zombieApocalypseChanceDefault;
        public float returnChance = returnChanceDefault;
        public float returnChanceApoc = returnChanceApocDefault;
        public float returnChanceColonist = returnChanceColonistDefault;
        public float pychoticWanderingChance = pychoticWanderingChanceDefault;
        public float berserkChance = berserkChanceDefault;
        public float berserkChanceApoc = berserkChanceApocDefault;
        public float berserkLoseFaction = berserkLoseFactionDefault;
        public float berserkLoseFactionApoc = berserkLoseFactionApocDefault;
        public float chanceBerserkIsPermanent = chanceBerserkIsPermanentDefault;
        public float chanceBerserkIsPermanentApoc = chanceBerserkIsPermanentApocDefault;
        public float chanceManhunter = chanceManhunterDefault;

        // Default values
        public const float deadRisingChanceDefault = 0.5f;
        public const float zombieApocalypseChanceDefault = 0.20f;
        public const float returnChanceDefault = 0.2f;
        public const float returnChanceApocDefault = 0.5f;
        public const float returnChanceColonistDefault = 0.15f;

        public const float pychoticWanderingChanceDefault = 0.5f;
        public const float berserkChanceDefault = 0.33f;
        public const float berserkChanceApocDefault = 0.75f;
        public const float berserkLoseFactionDefault = 0.5f;
        public const float berserkLoseFactionApocDefault = 0.5f;
        public const float chanceBerserkIsPermanentDefault = 0.5f;
        public const float chanceBerserkIsPermanentApocDefault = 0.5f;
        public const float chanceManhunterDefault = 0.75f;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref deadRisingChance, "DeadRisingChance", deadRisingChanceDefault);
            Scribe_Values.Look(ref zombieApocalypseChance, "ZombieApocalypseChance", zombieApocalypseChanceDefault);
            Scribe_Values.Look(ref returnChance, "ReturnChance", returnChanceDefault);
            Scribe_Values.Look(ref returnChanceApoc, "ReturnChanceApoc", returnChanceApocDefault);
            Scribe_Values.Look(ref returnChanceColonist, "ReturnChanceColonist", returnChanceColonistDefault);
            Scribe_Values.Look(ref pychoticWanderingChance, "PychoticWanderingChance", pychoticWanderingChanceDefault);
            Scribe_Values.Look(ref berserkChance, "BerserkChance", berserkChanceDefault);
            Scribe_Values.Look(ref berserkChanceApoc, "BerserkChanceApoc", berserkChanceApocDefault);
            Scribe_Values.Look(ref berserkLoseFaction, "BerserkLoseFaction", berserkLoseFactionDefault);
            Scribe_Values.Look(ref berserkLoseFactionApoc, "BerserkLoseFactionApoc", berserkLoseFactionApocDefault);
            Scribe_Values.Look(ref chanceBerserkIsPermanent, "ChanceBerserkIsPermanent", chanceBerserkIsPermanentDefault);
            Scribe_Values.Look(ref chanceBerserkIsPermanentApoc, "ChanceBerserkIsPermanentApoc", chanceBerserkIsPermanentApocDefault);
            Scribe_Values.Look(ref chanceManhunter, "ChanceManhunter", chanceManhunterDefault);
        }
    }
}
