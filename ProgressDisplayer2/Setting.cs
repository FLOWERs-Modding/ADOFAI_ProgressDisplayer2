using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace ProgressDisplayer2
{
    public class Setting : UnityModManager.ModSettings
    {
        public bool useCombo = false;
        public bool useProgress = true;
        public bool useAccuracy = false;
        public bool useScore = false;
        public float x = 0.02f;
        public float y = 0.98f;
        public int fontSize = 25;
        public bool addShadow = true;
        public bool setBold = false;
        public bool setZeroplaceholder = true;
        public int setDecimalpoints = 2;
        public int setAlign = 0;
        public string comboText = "Combo: {0}";
        public string scoreText = "Score: {0}";
        public string progressText = "Progress: {0}%";
        public string accuracyText = "Accuracy: {0}%";
        
        public override void Save(UnityModManager.ModEntry modEntry) {
            var filepath = GetPath(modEntry);
            try {
                using (var writer = new StreamWriter(filepath)) {
                    var serializer = new XmlSerializer(GetType());
                    serializer.Serialize(writer, this);
                }
            } catch {
            }
        }
       
        public override string GetPath(UnityModManager.ModEntry modEntry) {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }
    }
}