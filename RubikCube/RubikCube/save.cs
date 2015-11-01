using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace RubikCube
{
    class Save : Microsoft.Xna.Framework.Game
    {
        StorageDevice device;
        private const string ContainerName = "MyGamesStorage";
        private const string Filename = "mysave.sav";
        SaveGame saveData;
        public bool IsFirstTime;

        [Serializable]
        public struct SaveGame
        {
            public bool IsFirstTime;
        }

        public Save()
        {
        }

        private void Replace()
        {
            saveData = new SaveGame();
            IsFirstTime = saveData.IsFirstTime;
        }

        public void InitializeSave()
        {
                device = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.SaveToDevice, null);
        }

        private void SaveToDevice(IAsyncResult result) {
            device = StorageDevice.EndShowSelector(result);
            if (device != null && device.IsConnected)
            {
                SaveGame SaveData = new SaveGame()
                {
                    IsFirstTime = false,
                };
            IAsyncResult r = device.BeginOpenContainer(ContainerName,null,null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(r);
            if(container.FileExists(Filename))
                container.DeleteFile(Filename);
            Stream stream = container.CreateFile(Filename);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
            serializer.Serialize(stream,SaveData);
            stream.Close();
            container.Dispose();
            result.AsyncWaitHandle.Close();
            }
        }

        public void InitiateLoad()
        {
                device = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.LoadFromDevice, null);
        }

        void LoadFromDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            IAsyncResult r = device.BeginOpenContainer(ContainerName, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(r);
            result.AsyncWaitHandle.Close();
            if (container.FileExists(Filename))
            {
                Stream stream = container.OpenFile(Filename, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                SaveGame SaveData = (SaveGame)serializer.Deserialize(stream);
                stream.Close();
                container.Dispose();
                //Update the game based on the save game file
                
            }
            Replace();
        }
    }
}
