using UnityEngine;
using System;
using System.IO;

namespace com.MLR.Wesp
{
    public class PersistenceManager : SingletonBehaviour<PersistenceManager>
    {
        int levelReached = -1;

        public void SaveLevelReached(int level)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(Path.Combine(Application.persistentDataPath, "level.sav"), false, System.Text.Encoding.UTF8);
                writer.Write(level.ToString());
            }
            finally
            {
                if (writer != null)
                {
                    try
                    {
                        writer.Close();
                    }
                    catch
                    {
                    }
                }
            }

            this.levelReached = level;
        }

        public int LoadLevelReached()
        {
            if (this.levelReached == -1)
            {
                StreamReader reader = null;

                try
                {
                    reader = new StreamReader(Path.Combine(Application.persistentDataPath, "level.sav"), System.Text.Encoding.UTF8);
                    this.levelReached = Int32.Parse(reader.ReadToEnd());
                }
                catch
                {
                    this.levelReached = 1;
                }
                finally
                {
                    if (reader != null)
                    {
                        try
                        {
                            reader.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            
            return this.levelReached;
        }
    } 
}
