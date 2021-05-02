using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SimpleWR2.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance;

        [DebuggerNonUserCode]
        [DefaultSettingValue("Black")]
        [UserScopedSetting]
        public Color backColor
        {
            get
            {
                return (Color)this["backColor"];
            }
            set
            {
                this["backColor"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("Crimson")]
        [UserScopedSetting]
        public Color baseColor
        {
            get
            {
                return (Color)this["baseColor"];
            }
            set
            {
                this["baseColor"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return Settings.defaultInstance;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("White")]
        [UserScopedSetting]
        public Color holdColor
        {
            get
            {
                return (Color)this["holdColor"];
            }
            set
            {
                this["holdColor"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("100, 100")]
        [UserScopedSetting]
        public Point locationMainWindow
        {
            get
            {
                return (Point)this["locationMainWindow"];
            }
            set
            {
                this["locationMainWindow"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("Yellow")]
        [UserScopedSetting]
        public Color peakColor
        {
            get
            {
                return (Color)this["peakColor"];
            }
            set
            {
                this["peakColor"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        [UserScopedSetting]
        public byte visualType
        {
            get
            {
                return (byte)this["visualType"];
            }
            set
            {
                this["visualType"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("50")]
        [UserScopedSetting]
        public int volumeLevel
        {
            get
            {
                return (int)this["volumeLevel"];
            }
            set
            {
                this["volumeLevel"] = value;
            }
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        [UserScopedSetting]
        public string channel
        {
            get
            {
                return ((string)(this["channel"]));
            }
            set
            {
                this["channel"] = value;
            }
        }

        static Settings()
        {
            Settings.defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
        }

        public Settings()
        {
        }

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }
    }
}