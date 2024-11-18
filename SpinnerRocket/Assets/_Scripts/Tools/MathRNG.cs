using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/**
 * @file
 * @brief Clase que genera numeros pseudoaleatorios en base a una semilla inicial
 */
public class MathRNG
{
    #region Variables
    /** Semilla usada */
    public int Seed { get; set; }
    /** Valor minimo del rango */
    private decimal MinValue { get; set; }
    /** Valor maximo del rango */
    private decimal MaxValue { get; set; }
    /** Contador que inicia del 0 y aumenta con cada solicitud de numero aleatorio */
    private int Next { get; set; }
    #endregion

    #region Constructors
    /**
     * Inicializa la clase
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @param Seed: semilla utilizada
     */
    public MathRNG(decimal MinValue, decimal MaxValue, int Seed = 1)
    {
        this.MinValue = MinValue;
        this.MaxValue = MaxValue;
        this.Seed = Seed;
        Next = 1;
    }
    /**
     * Inicializa la clase
     * @param Seed: semilla utilizada
     */
    public MathRNG(int Seed = 1)
    {
        this.Seed = Seed;
        Next = 1;
    }
    #endregion

    #region NextValues
    /**
     * Solicita un numero aleatorio del 0 al 1
     * @return numero decimal del 0 al 1
     */
    public decimal GetRandom()
    {
        return GetCatalystSeed();
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero entero
     */
    public int GetRandom(int MinValue, int MaxValue)
    {
        return (int)Math.Round(NextValue((decimal)MinValue, (decimal)MaxValue));
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MaxValue: Valor maximo
     * @return numero entero
     */
    public int GetRandom(int MaxValue)
    {
        return (int)Math.Round(NextValue(0, (decimal)MaxValue));
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero flotante
     */
    public float GetRandom(float MinValue, float MaxValue)
    {
        return (float)NextValue((decimal)MinValue, (decimal)MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos instanciados
     * @return numero decimal
     */
    public decimal NextValue()
    {
        return NextValue(MinValue, MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero decimal
     */
    public decimal NextValue(decimal MinValue, decimal MaxValue)
    {
        decimal initialValue = GetCatalystSeed();
        decimal PseudoRandom = (initialValue * (MaxValue - MinValue)) + MinValue;
        return PseudoRandom;
    }
    /**
     * Formula que genera los numeros pseudoaleatorios
     * @return numero decimal entre 0 y 1
     */
    public decimal GetCatalystSeed()
    {
        decimal SeedModify = Seed;
        SeedModify = Next % 3 == 0 ? (decimal)(Seed * 0.0001) : SeedModify;
        SeedModify = Next % 3 == 1 ? (decimal)(Seed * 0.0000001) : SeedModify;
        SeedModify = Next % 3 == 2 ? (decimal)(Seed * 0.000000001) : SeedModify;
        decimal Catalist = Next % 2 == 0 ? (decimal)Mathf.Sin((float)(Next + SeedModify)) : (decimal)Mathf.Sin((float)(Next + SeedModify));
        decimal initialValue = (decimal)((Catalist + 1) / 2);
        Next++;
        Next = Next <= 0 ? 1 : Next;
        return initialValue;
    }
    #endregion

    #region NextValues (Other Numeric Values)
    /**
     * Solicita un numero aleatorio entre los rangos instanciados
     * @return numero flotante
     */
    public float NextValueFloat()
    {
        return (float)NextValue(MinValue, MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero flotante
     */
    public float NextValueFloat(float MinValue, float MaxValue)
    {
        return (float)NextValue((decimal)MinValue, (decimal)MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos instanciados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero double
     */
    public double NextValueDouble()
    {
        return (double)NextValue(MinValue, MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero flotante
     */
    public float NextValueDouble(double MinValue, double MaxValue)
    {
        return (float)NextValue((decimal)MinValue, (decimal)MaxValue);
    }
    /**
     * Solicita un numero aleatorio entre los rangos instanciados
     * @return numero entero
     */
    public Int32 NextValueInt()
    {
        return (Int32)Decimal.Round(NextValue(MinValue, MaxValue));
    }
    /**
     * Solicita un numero aleatorio entre los rangos solicitados
     * @param MinValue: Valor minimo
     * @param MaxValue: Valor maximo
     * @return numero entero
     */
    public Int32 NextValueInt(double MinValue, double MaxValue)
    {
        return (Int32)Decimal.Round(NextValue((decimal)MinValue, (decimal)MaxValue));
    }
    #endregion

    #region Test Area
    /** @hidden */
    public static void Testing()
    {
        int ciclos = 100;
        int SumatoriaProm = 0;
        int Maximo = 0;
        int Minimo = 0;
        for (int m = 0; m < ciclos; m++)
        {
            MathRNG obj = new MathRNG(1, 100, UnityEngine.Random.Range(1, 999999999));
            var lst = new List<String>();
            for (int l = 0; l < 300; l++)
            {
                lst.Add(((Int32)obj.NextValue()).ToString());
            }
            var lstgr = lst.Distinct();
            SumatoriaProm += lstgr.Count();
            Maximo = lstgr.Count() > Maximo ? lstgr.Count() : Maximo;
            Minimo = lstgr.Count() < Minimo || Minimo == 0 ? lstgr.Count() : Minimo;
            //Debug.Log("Seed " + obj.Seed + ": " + lstgr.Count());
            //Debug.Log(String.Join(", ", lst));
            //Debug.Log(String.Join(", ", lstgr));
        }
        SumatoriaProm = SumatoriaProm / ciclos;
        Debug.Log("Maximo: " + Maximo);
        Debug.Log("Promedio: " + SumatoriaProm);
        Debug.Log("Minimo: " + Minimo);
    }
    #endregion

    #region Tools
    /** @hidden */
    public Vector2 getRandomSpawnPoint(Vector2 minValues, Vector2 maxValues)
    {
        var RanX = NextValueInt(0, 3);
        Vector2 vecSpawn = new Vector2();
        switch (RanX)
        {
            case 0: vecSpawn = new Vector2(NextValueFloat(minValues.x, maxValues.x), maxValues.y); break;
            case 1: vecSpawn = new Vector2(NextValueFloat(minValues.x, maxValues.x), minValues.y); break;
            case 2: vecSpawn = new Vector2(maxValues.x, NextValueFloat(minValues.y, maxValues.y)); break;
            case 3: vecSpawn = new Vector2(minValues.x, NextValueFloat(minValues.y, maxValues.y)); break;
            default: vecSpawn = new Vector2(NextValueFloat(minValues.x, maxValues.x), maxValues.y); break;
        }
        return vecSpawn;
    }
    #endregion
}