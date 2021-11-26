using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace ProgressDisplayer2
{
    public class Patch
    {
        private static int combo = 0, score = 0;
        private static bool isLevelStart = false;
        private static float accuracy {
            get
            {
                if (scrController.instance.mistakesManager == null)
                    return 0f;
                return scrController.instance.mistakesManager.percentAcc;
            }
        }

        private static string Repeat(string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }
        
        private static string DecimalFormat(float v)
        {
            return string.Format("{0:0." + Repeat(Main.setting.setZeroplaceholder? "0":"#", Main.setting.setDecimalpoints) + "}", v);
        }
        
        private static void Disable()
        {
            if (!Main.isEnabled) return;
            Main.textUI.TextObject.SetActive(false);
            isLevelStart = false;
        }

        private static void LevelStart(bool isOfficial)
        {
            if (!Main.isEnabled) return;
            combo = 0;
            isLevelStart = true;

            if(isOfficial)
                Main.Logger.Log(scrController.instance.levelName);
            
            TextUpdate();
            Main.textUI.TextObject.SetActive(true);
            Main.textUI.SetSize(Main.setting.fontSize);
        }

        private static void TextUpdate()
        {
            var i = 0;
            var texts = new string[4];
            if (Main.setting.useProgress)
            {
                texts[i] = string.Format(Main.setting.progressText,
                    DecimalFormat(scrController.instance.percentComplete * 100));
                i++;
            }
            if (Main.setting.useAccuracy)
            {
                texts[i] = string.Format(Main.setting.accuracyText, DecimalFormat(accuracy * 100));
                i++;
            }
            if (Main.setting.useCombo)
            {
                texts[i] = string.Format(Main.setting.comboText, combo);
                i++;
            }
            if (Main.setting.useScore)
                texts[i] = string.Format(Main.setting.scoreText, score);

            Main.textUI.SetText(string.Join("\n", texts));
        }

        [HarmonyPatch(typeof(CustomLevel), "Play")]
        private static class Play
        {
            private static void Postfix(CustomLevel __instance)
            {
                if (!__instance.controller.gameworld) return;
                if (__instance.controller.customLevel == null) return;
                
                LevelStart(false);
            }
        }
        
        [HarmonyPatch(typeof(scrPressToStart), "ShowText")]
        private static class ShowText
        {
            private static void Postfix(scrPressToStart __instance)
            {
                if (!__instance.controller.gameworld) return;
                if (__instance.controller.customLevel != null) return;
                
                LevelStart(true);
            }
        }
        
        [HarmonyPatch(typeof(scrCalibrationPlanet),"Start")]
        private static class Start
        {
            private static void Prefix()
            {
                Disable();
            }
        }
        
        [HarmonyPatch(typeof(scrUIController), "WipeToBlack")]
        private static class WipeToBlack
        {
            private static void Postfix()
            {
                Disable();
            }
        }


        [HarmonyPatch(typeof(scrMistakesManager), "AddHit")]
        private static class AddHit
        {
            private static void Postfix(HitMargin hit)
            {
                if (!isLevelStart) return;
                if (!Main.isEnabled) return;

                if (Main.setting.useScore)
                {
                    switch (hit)
                    {
                        case HitMargin.VeryEarly:
                        case HitMargin.VeryLate:
                            score += 91;
                            break;
                        case HitMargin.EarlyPerfect:
                        case HitMargin.LatePerfect:
                            score += 150;
                            break;
                        case HitMargin.Perfect:
                            score += 300;
                            break;
                    }
                }

                if (Main.setting.useCombo)
                {
                    if (hit == HitMargin.Perfect)
                        combo++;
                    else
                        combo = 0;
                }

                TextUpdate();
            }
        }
        
    }
}