﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App
{
    internal class Settings
    {
        private static INIFile iniFile;

        public static string Language { get; set; } = "ko-kr";
        public static bool ShowOverlay { get; set; } = true;
        public static int OverlayX { get; set; } = Global.OVERLAY_XY_UNSET;
        public static int OverlayY { get; set; } = Global.OVERLAY_XY_UNSET;
        public static bool StartupShowMainForm { get; set; } = true;
        public static bool FlashWindow { get; set; } = true;
        public static bool FateSound { get; set; } = false;
        public static bool CustomSound { get; set; } = false;
        public static string CustomSoundPath { get; set; } = "";
        public static bool CheatRoulette { get; set; } = false;
        public static bool Updated { get; set; } = true;
        public static HashSet<int> FATEs { get; set; } = new HashSet<int>();

        private static void Init()
        {
        }

        public static void Load()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Global.APPNAME, Global.SETTINGS_FILEPATH);

            iniFile = new INIFile(path);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                Init();
                Save();
            }
            else
            {
                StartupShowMainForm = iniFile.ReadValue("startup", "show") == "1";
                ShowOverlay = iniFile.ReadValue("overlay", "show") != "0";
                OverlayX = int.Parse(iniFile.ReadValue("overlay", "x") ?? "0");
                OverlayY = int.Parse(iniFile.ReadValue("overlay", "y") ?? "0");
                FlashWindow = iniFile.ReadValue("notification", "flashwindow") == "1";
                FateSound = iniFile.ReadValue("notification", "fatesound") == "1";
                CustomSound = iniFile.ReadValue("notification", "customsound") == "1";
                CustomSoundPath = iniFile.ReadValue("notification", "customsoundpath") ?? "";
                CheatRoulette = iniFile.ReadValue("misc", "cheatroulette") == "1";
                Language = iniFile.ReadValue("misc", "language") ?? "ko-kr";
                Updated = iniFile.ReadValue("internal", "updated") == "1";

                var fates = iniFile.ReadValue("fate", "fates");
                if (!string.IsNullOrEmpty(fates))
                {
                    FATEs = new HashSet<int>(from x in fates.Split(',') select int.Parse(x));
                }
            }
        }

        public static void Save()
        {
            iniFile.WriteValue("startup", "show", StartupShowMainForm ? "1" : "0");
            iniFile.WriteValue("overlay", "show", ShowOverlay ? "1" : "0");
            iniFile.WriteValue("overlay", "x", OverlayX.ToString());
            iniFile.WriteValue("overlay", "y", OverlayY.ToString());
            iniFile.WriteValue("notification", "flashwindow", FlashWindow ? "1" : "0");
            iniFile.WriteValue("notification", "fatesound", FateSound ? "1" : "0");
            iniFile.WriteValue("notification", "customsound", CustomSound ? "1" : "0");
            iniFile.WriteValue("notification", "customsoundpath", CustomSoundPath);
            iniFile.WriteValue("misc", "cheatroulette", CheatRoulette ? "1" : "0");
            iniFile.WriteValue("misc", "language", Language);
            iniFile.WriteValue("fate", "fates", string.Join(",", FATEs));
            iniFile.WriteValue("internal", "updated", Updated ? "1" : "0");
        }
    }
}
