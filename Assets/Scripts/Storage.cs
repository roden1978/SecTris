using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Storage: IStorage
{
   
   private readonly string _directory;
   private readonly BinaryFormatter _formatter;
   
   private const string SaveFolder = "/saves/";
   
   public Storage()
   {
      _directory = Application.persistentDataPath + SaveFolder;
      _formatter = new BinaryFormatter();
   }

   public object Load(object defaultData, string fileName)
   {
      CheckDirectory();
      var path = _directory + fileName;
      if (!File.Exists(path))
      {
         if(defaultData != null)
            Save(defaultData, fileName);
         return defaultData;
      }

      var file = File.Open(path, FileMode.Open);
      var data = _formatter.Deserialize(file);
      file.Close();
      return data;
   }

   public void Save(object saveData, string fileName)
   {
      var path = _directory + fileName;
      var file = File.Create(path);
      _formatter.Serialize(file, saveData);
      file.Close();
   }

   private void CheckDirectory()
   {
      if (!Directory.Exists(_directory))
         Directory.CreateDirectory(_directory);
   }
}
