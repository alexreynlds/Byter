using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string filePath = "";

    private string fileName = "";

    private bool useEncryption = false;

    private readonly string encryptionCodeWode = "boink";

    public FileDataHandler(string filePath, string fileName, bool useEncryption)
    {
        this.filePath = filePath;
        this.fileName = fileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(this.filePath, this.fileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (
                    FileStream stream = new FileStream(fullPath, FileMode.Open)
                )
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (this.useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log (e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(this.filePath, this.fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            Debug.Log (fullPath);

            if (this.useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)
            )
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write (dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log (e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        char[] dataToEncrypt = data.ToCharArray();
        for (int i = 0; i < dataToEncrypt.Length; i++)
        {
            dataToEncrypt[i] =
                (
                char
                )(dataToEncrypt[i] ^
                this.encryptionCodeWode[i % this.encryptionCodeWode.Length]);
        }
        return new string(dataToEncrypt);
    }
}
