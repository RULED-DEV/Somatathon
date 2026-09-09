using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class save_data{ // saves data

    public static void SAVE (DATA data){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.Data";
        FileStream stream = new FileStream(path, FileMode.Create);

        hold_data data_stored = new hold_data(data);

        formatter.Serialize(stream, data_stored);
        stream.Close(); 
        Debug.Log("saved");
    } 

    public static hold_data LOAD () {
        string path = Application.persistentDataPath + "/Player.Data";
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            hold_data data_stored = formatter.Deserialize(stream) as hold_data;
            stream.Close(); 
            Debug.Log("loaded");
            return data_stored;
        }
        else{
            Debug.LogError("error.File not found :(");
            return null;
        }
        
    }


}
