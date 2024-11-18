using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Administrador que guarda y carga los objetos de la clase persistente
 */
public class AdministradorDePersistencia : MonoBehaviour
{
    /** Lista de los objetos de clase ObjetoPersistente que se guardaran */
    public List<ObjetoPersistente> ObjetosAGuardar;
    /** Habilita la carga de objetos */
    public void OnEnable()
    {
        CargarCambios();
    }
    /** Cuando se cierra el escenario se guardan los objetos */
    public void OnDisable()
    {
        GuardarCambios();
    }
    /** Metodo que carga los cambios */
    public void CargarCambios()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Cargar();
        }
    }
    /** Metodo que guarda los cambios */
    public void GuardarCambios()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Guardar();
        }
    }
}