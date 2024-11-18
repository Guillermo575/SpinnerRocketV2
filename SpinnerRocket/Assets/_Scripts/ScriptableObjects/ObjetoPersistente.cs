using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/**
 * @class
 * @brief Scriptable object con funciones de guardar y cargar cuando otro scriptable object hija es modificada dentro del juego
 */
public abstract class ObjetoPersistente : ScriptableObject
{
    /** Guarda el objeto persistente en un archivo en formato JSON 
      * @param NameFile: Nombre del archivo
      */
    public void Guardar(string NameFile = null)
    {
        var bf = new BinaryFormatter();
        var file = File.Create(ObtenerRuta(NameFile));
        var json = JsonUtility.ToJson(this);
        bf.Serialize(file, json);
        file.Close();
    }
    /** Carga el objeto persistente en un archivo en formato JSON 
      * @param NameFile: Nombre del archivo
      */
    public virtual void Cargar(string NameFile = null)
    {
        if (File.Exists(ObtenerRuta(NameFile)))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(ObtenerRuta(NameFile), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), this);
            file.Close();
        }
    }
    /** Guarda el objeto persistente en un archivo en formato JSON 
      * @param NameFile: Nombre del archivo
      */
    public string ObtenerRuta(string NameFile = null)
    {
        var nombreArchivoCompleto = string.IsNullOrEmpty(NameFile) ? name : NameFile;
        return string.Format("{0}/{1}.ebac", Application.persistentDataPath, nombreArchivoCompleto);
    }
}