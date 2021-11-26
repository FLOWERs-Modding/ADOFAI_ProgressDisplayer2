using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ProgressDisplayer2
{
    public class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool isEnabled = false;
        public static Setting setting;
        private static Language[] languages = {new English(), new Korean()};
        private static Language nowLanguage = languages[0];
        public static TextUI textUI;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnGUI = OnSettingGUI;
        }

        private static object GetValue<T>(string file, string key)
        {
            try
            {
                var data = file.Split(new[] {key+":"}, StringSplitOptions.None)[1]
                    .Split(new[] {"\n"}, StringSplitOptions.None)[0].Trim();
                if (typeof(T) == typeof(bool))
                {
                    return bool.Parse(data);
                }
                if (typeof(T) == typeof(int))
                {
                    return int.Parse(data);
                }
                return data;
            }
            catch
            {
                if (typeof(T) == typeof(bool))
                {
                    return false;
                }
                if (typeof(T) == typeof(int))
                {
                    return 0;
                }
                return false;
            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            isEnabled = value;
            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                textUI = new GameObject().AddComponent<TextUI>();
                UnityEngine.Object.DontDestroyOnLoad(textUI);
                textUI.TextObject.SetActive(false);
            }
            else
            {
                if (textUI != null)
                {
                    textUI.TextObject.SetActive(false);
                    UnityEngine.Object.DestroyImmediate(textUI);
                    textUI = null;
                }

                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }
        
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            setting.Save(modEntry);
        }

        private static void OnSettingGUI(UnityModManager.ModEntry modEntry)
        {
            nowLanguage = RDString.language == SystemLanguage.Korean ? languages[1] : languages[0];



            setting.useProgress = GUILayout.Toggle(setting.useProgress, nowLanguage.useProgress);
            if (setting.useProgress)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                setting.progressText = GUILayout.TextField(setting.progressText, GUILayout.Width(256));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            setting.useAccuracy = GUILayout.Toggle(setting.useAccuracy, nowLanguage.useAccuracy);
            if (setting.useAccuracy)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                setting.accuracyText = GUILayout.TextField(setting.accuracyText, GUILayout.Width(256));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                textUI.SetSize(setting.fontSize);
            }

            setting.useCombo = GUILayout.Toggle(setting.useCombo, nowLanguage.useCombo);
            if (setting.useCombo)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                setting.comboText = GUILayout.TextField(setting.comboText, GUILayout.Width(256));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            setting.useScore = GUILayout.Toggle(setting.useScore, nowLanguage.useScore);
            if (setting.useScore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                setting.scoreText = GUILayout.TextField(setting.scoreText, GUILayout.Width(256));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }


            if (setting.useProgress || setting.useAccuracy || setting.useCombo || setting.useScore)
            {
                GUILayout.Label(" ");

                setting.addShadow = GUILayout.Toggle(setting.addShadow, nowLanguage.useShadow);
                textUI.shadowText.enabled = setting.addShadow;
                setting.setBold = GUILayout.Toggle(setting.setBold, nowLanguage.useBold);
                textUI.text.fontStyle = setting.setBold ? FontStyle.Bold : FontStyle.Normal;
                setting.setZeroplaceholder =
                    GUILayout.Toggle(setting.setZeroplaceholder, nowLanguage.setZeroPlaceHolder);

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                var newV =
                    (int) MoreGUILayout.NamedSlider(
                        nowLanguage.setDecimalpoints,
                        setting.setDecimalpoints,
                        0,
                        6,
                        300f,
                        roundNearest: 1f,
                        valueFormat: "{0:0.##}");
                if (newV != setting.setDecimalpoints)
                {
                    setting.setDecimalpoints = newV;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                var newV4 =
                    (int) MoreGUILayout.NamedSlider(
                        nowLanguage.setFontsize,
                        setting.fontSize,
                        1,
                        100,
                        300f,
                        roundNearest: 1f,
                        valueFormat: "{0:0.##}");
                if (newV4 != setting.fontSize)
                {
                    setting.fontSize = newV4;
                    textUI.SetSize(setting.fontSize);
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                var newV2 =
                    (float) MoreGUILayout.NamedSlider(
                        nowLanguage.setX,
                        setting.x,
                        -0.02f,
                        1.05f,
                        300f,
                        roundNearest: 0.01f,
                        valueFormat: "{0:0.##}");
                if (newV2 != setting.setDecimalpoints)
                {
                    setting.x = newV2;
                    textUI.SetPosition(setting.x, setting.y);
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                var newV3 =
                    (float) MoreGUILayout.NamedSlider(
                        nowLanguage.setY,
                        setting.y,
                        -0.02f,
                        1.05f,
                        300f,
                        roundNearest: 0.01f,
                        valueFormat: "{0:0.##}");
                if (newV3 != setting.setDecimalpoints)
                {
                    setting.y = newV3;
                    textUI.SetPosition(setting.x, setting.y);
                }

                GUILayout.EndHorizontal();

                var aligns = new[] {nowLanguage.alignLeft, nowLanguage.alignCenter, nowLanguage.alignRight};
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(nowLanguage.setAlign);
                GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
                foreach (var text in aligns)
                {
                    if (setting.setAlign == Array.IndexOf(aligns, text)) guiStyle.fontStyle = FontStyle.Bold;
                    if (GUILayout.Button(text, guiStyle))
                    {
                        setting.setAlign = Array.IndexOf(aligns, text);
                        textUI.text.alignment = textUI.ToAlign(setting.setAlign);
                    }

                    guiStyle.fontStyle = FontStyle.Normal;
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }



            GUILayout.Label(" ");
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            if (GUILayout.Button(nowLanguage.loadOldSetting))
            {
                var path = modEntry.Path.Replace(@"Mods\ProgressDisplayer 2\", "") +
                           @"Options\ProgressDisplayerConfig.txt";

                if (!new DirectoryInfo(path).Exists)
                {
                    var file = File.ReadAllText(path);
                    setting.setBold = (bool) GetValue<bool>(file, "FontBold");
                    setting.addShadow = (bool) GetValue<bool>(file, "FontShadow");
                    setting.setZeroplaceholder = (bool) GetValue<bool>(file, "ValueAsStatic");
                    setting.fontSize = (int) GetValue<int>(file, "FontSize");
                    setting.setDecimalpoints = (int) GetValue<int>(file, "ValueRoundPoint");

                    setting.useProgress = (bool) GetValue<bool>(file, "DisplayProgress");
                    setting.useAccuracy = (bool) GetValue<bool>(file, "DisplayAccuracy");
                    setting.useCombo = (bool) GetValue<bool>(file, "DisplayPerfectsCombo");
                    setting.useScore = (bool) GetValue<bool>(file, "DisplayScore");

                    var textColor = (string) GetValue<string>(file, "FontColor");
                    var isWhite = textColor == "FFFFFFFF";
                    setting.progressText = (isWhite ? "" : "<color=#" + textColor + ">") +
                                           ((string) GetValue<string>(file, "String - ProgressSet")).Replace("@value","{0}") +
                                           (isWhite ? "" : "</color>");
                    setting.accuracyText = (isWhite ? "" : "<color=#" + textColor + ">") +
                                           ((string) GetValue<string>(file, "String - AccuracySet")).Replace("@value","{0}") +
                                           (isWhite ? "" : "</color>");
                    setting.comboText = (isWhite ? "" : "<color=#" + textColor + ">") +
                                        ((string) GetValue<string>(file, "String - PerfectsComboSet")).Replace("@value","{0}") +
                                        (isWhite ? "" : "</color>");
                    setting.scoreText = (isWhite ? "" : "<color=#" + textColor + ">") +
                                        ((string) GetValue<string>(file, "String - ScoreSet")).Replace("@value","{0}") +
                                        (isWhite ? "" : "</color>");
                    
                }
            }

            GUILayout.Space(100);
            GUILayout.EndHorizontal();

        }
    }
}