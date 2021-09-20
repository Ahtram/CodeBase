using UnityEngine;
using System.Xml.Serialization;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
#endif

namespace Teamuni.Codebase {
    //The customized version data which auto-increment ability.
    //The format: http://stackoverflow.com/questions/65718/what-do-the-numbers-in-a-version-typically-represent-i-e-v1-9-0-1
    [XmlType("VER")]
    public class Version {

        [XmlElement("MAJ")]
        public int major = 0;

        [XmlElement("MIN")]
        public int minor = 0;

        [XmlElement("BF")]
        public int bugFix = 0;

        [XmlElement("BD")]
        public int build = 0;

        [XmlElement("BT")]
        public DateTime buildTime = DateTime.MinValue;

        //Loaded data.
        [XmlIgnore]
        [JsonIgnore]
        static private Version version = null;

        static public Version Get() {
            if (version == null) {
                //Try load from xml first.
                version = DataUtil.GetDataFromResource<Version>(SysPath.UniqueDataPath + SysPath.VersionFileName);

                //If no data exist, generate a new one.
                if (version == null) {
                    version = new Version();
                    Debug.Log("No Version xml exist. Create a new version.");
                }
            }

            return version;
        }

        static public void ClearCache() {
            version = null;
        }

        static public bool Save() {
            Get();
            if (version != null) {
                version.Serialize();
                return true;
            } else {
                Debug.LogWarning("OOps! Version not loaded yet! Save failed!");
                return false;
            }
        }

        static public string BundleVersion() {
            Get();
            if (version != null) {
                return (version.major + "." + version.minor + "." + version.bugFix + "." + version.build);
            } else {
                Debug.LogWarning("OOps! Version not loaded yet! Save failed!");
                return "";
            }
        }

        static public string BuildTime() {
            Get();
            if (version != null) {
                return version.buildTime.ToString();
            } else {
                Debug.LogWarning("OOps! Version not loaded yet! Save failed!");
                return "";
            }
        }

        static public string FullVersion() {
            return BundleVersion() + " - " + BuildTime();
        }

        public void Serialize() {
            DataUtil.SaveData(Application.dataPath + "/Resources/" + SysPath.UniqueDataPath + SysPath.VersionFileName, this);
        }

#if UNITY_EDITOR
        //This will update the build num and build time.
        [DidReloadScripts]
        public static async void UpdateBuild() {
            //Tricky! I have to do a bit delay for whatever after a rebuild you cannot read from Resources immediately.
            await Task.Delay(330);
            //Do auto increment by config.
            bool autoIncrement = Util.IntToBool(PlayerPrefs.GetInt("AutoIncrementBuild", 0));
            if (autoIncrement) {
                Get();
                version.build += 1;
                version.buildTime = DateTime.Now;
                version.Serialize();
                AssetDatabase.Refresh();
                //Also update bundle version.
                UnityEditor.PlayerSettings.bundleVersion = BundleVersion();
                ClearCache();
            }
        }
#endif
    }
}
