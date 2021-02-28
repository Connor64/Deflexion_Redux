using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Deflexion_Redux {
    public static class XmlManager {

        //public static void Save<T>(string path, T obj) {
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.Indent = true;

        //    using (XmlWriter writer = XmlWriter.Create(path, settings)) {
        //        IntermediateSerializer.Serialize(writer, obj, path);
        //    }
        //    Debug.Print(path);
        //}

        public static void Save<T>(string path, T obj) {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            using (StreamWriter writer = new StreamWriter(path)) {
                xmlSerializer.Serialize(writer, obj);
            }
            Debug.Print(path);
        }

        public static T Load<T>(string path) {
            using (TextReader reader = new StreamReader(path)) {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                return (T)xml.Deserialize(reader);
            }
        }

        public static void OpenFolder(string folderPath) {
            if (Directory.Exists(folderPath)) {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }
    }
}