﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles input in map editing mode
/// </summary>
public class MapEditor : MonoBehaviour
{
    //Stores values to adjust
    static int selectedTerrainType;
    static bool applyTerrainType;

    //Unit placement fields
    static bool placeUnit;
    static Allegiance unitAllegiance;

    //Country editing fields
    string countryName="t";
    CountryType type=CountryType.poor;
    static Allegiance countryAllegiance;    
    
    /// <summary>
    /// Applyes terrain and unit placement changes to specified cell
    /// </summary>
    /// <param name="cell"></param>
    public static void ApplyChanges(LogicalMapCell cell)
    {
        if (applyTerrainType)
        {
            cell.terrain = (TerrainType)selectedTerrainType;
            cell.ValidateHighlightWithTerrain();
        }
        if (placeUnit)
        {
            Unit unit = Instantiate(GameMaster.unitPrefab);
            unit.transform.SetParent(cell.transform, false);
            unit.cell = cell;
            unit.allegiance = unitAllegiance;
            cell.unit = unit;
            unit.Initialize(UnitType.Tank);
            unit.ValidatePosition();
            GameMaster.units.Add(unit);
        }
    }

    /// <summary>
    /// Takes terrain type from UI slider
    /// </summary>
    /// <param name="type"></param>
    public void SetTerrainType(float type)
    {
        selectedTerrainType = (int)type;
    }

    /// <summary>
    /// Takes weather terrain type should be applyed from UI checkbox
    /// </summary>
    /// <param name="value"></param>
    public void SetApplyTerrain(bool value)
    {
        applyTerrainType = value;
    }

    /// <summary>
    /// Takes weather unit must be placed in that cell
    /// </summary>
    public void SetPlaceUnit(bool value)
    {
        placeUnit = value;
    }
    
    /// <summary>
    /// Gets unit's allegiance
    /// </summary>
    /// <param name="value"></param>
    public void GetUnitAllegiance(int value)
    {
        unitAllegiance = (Allegiance)value;
    }

    /// <summary>
    /// Takes country name
    /// </summary>
    /// <param name="value"></param>
    public void GetCountryName(string value)
    {
        countryName = value;
    }

    /// <summary>
    /// Takes country allegiance
    /// </summary>
    /// <param name="value"></param>
    public void GetCountryAlligance(int value)
    {
        countryAllegiance = (Allegiance) value;
    }

    /// <summary>
    /// Takes country type
    /// </summary>
    /// <param name="value"></param>
    public void GetCountryType(int value)
    {
        type = CountryTypeExtentions.GetCountyType(value);
    }

    /// <summary>
    /// Creates a country with capital at selected cell
    /// </summary>
    public void CreateCountry()
    {
        if ((!countryName.Equals("")) && (MapInputs.GetSelectedCell() != null) &&
            (GameMaster.countries.Find(u => u.GetCountryName() == countryName)==null) &&
            (GameMaster.countries.Find(u => u.GetCountryCapital() == MapInputs.GetSelectedCell()) == null))
        {
            Country newCountry = Instantiate(GameMaster.countryPrefab);
            newCountry.CreateCountry(countryName, type, countryAllegiance, MapInputs.GetSelectedCell());
            GameMaster.countries.Add(newCountry);
        }
    }

    /// <summary>
    /// Changes country capital to selected cell
    /// </summary>
    public void ChangeCapital()
    {
        if ((!countryName.Equals("") && (MapInputs.GetSelectedCell() != null)) &&
            (GameMaster.countries.Find(u => u.GetCountryCapital() == MapInputs.GetSelectedCell()) == null))
        {
            Country selectedCountry = GameMaster.countries.Find(u => u.GetCountryName() == countryName);
            if (selectedCountry != null)
            {
                selectedCountry.ChangeCapital(MapInputs.GetSelectedCell());
            }                  
        }
    }

    /// <summary>
    /// Adds selected cell to country
    /// </summary>
    public void AddArea()
    {
        if ((!countryName.Equals("") && (MapInputs.GetSelectedCell() != null)) &&
            (GameMaster.countries.Find(u => u.GetCountryCapital() == MapInputs.GetSelectedCell()) == null))
        {
            Country selectedCountry = GameMaster.countries.Find(u => u.GetCountryName() == countryName);
            if (selectedCountry != null)
            {
                selectedCountry.AddAreaToCountry(MapInputs.GetSelectedCell());
            }
        }
    }

    /// <summary>
    /// Remove selected cell from country
    /// </summary>
    public void RemoveArea()
    {
        if ((MapInputs.GetSelectedCell() != null) &&
            (GameMaster.countries.Find(u => u.GetCountryCapital() == MapInputs.GetSelectedCell()) == null))
        {
            Country selectedCountry = MapInputs.GetSelectedCell().country;
            if (selectedCountry != null)
            {
                selectedCountry.RemoveAreaFromCountry(MapInputs.GetSelectedCell());
            }
        }
    }

    /// <summary>
    /// Deletes selected country
    /// </summary>
    public void DeleteCountry()
    {
        if ((MapInputs.GetSelectedCell() != null))
        {
            Country selectedCountry = MapInputs.GetSelectedCell().country;
            if (selectedCountry != null)
            {
                selectedCountry.DeleteCountry();                
            }
        }
    }

    /// <summary>
    /// Changes country name
    /// </summary>
    public void ChangeName()
    {
        if ((MapInputs.GetSelectedCell() != null))
        {
            Country selectedCountry = MapInputs.GetSelectedCell().country;
            if (selectedCountry != null)
            {
                selectedCountry.ChangeName(countryName);
            }
        }
    }
}
