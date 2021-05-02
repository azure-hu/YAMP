using System.Configuration;
using System.Diagnostics;
using System.Drawing;

namespace SimpleWR2.Properties
{
    internal sealed partial class Settings : ApplicationSettingsBase
    {

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
        [DefaultSettingValue("1")]
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
        [DefaultSettingValue("0.5")]
        [UserScopedSetting]
        public float volumeLevel
        {
            get
            {
                return (float)this["volumeLevel"];
            }
            set
            {
                this["volumeLevel"] = value;
            }
        }
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public string channel
        {
            get
            {
                return (string)this["channel"];
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
    }
}