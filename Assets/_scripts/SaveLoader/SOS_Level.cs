using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using D_StructsAndEnums;

public static class SOS_Level 
{
    static string suffix = "lvl";
    public static bool prettyPrint = false;

	public static void Save(D_LevelData level)
    {
        if(D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Saving Started!\n ====================");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + string.Format("{0}.{1}", level.mID, suffix));

        LevelWrapper levelWrpd = new LevelWrapper(level);


        var json = JsonUtility.ToJson(levelWrpd, true);
        bf.Serialize(file, json);
        file.Close();
        if(D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Saving Done!\n ====================");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS: " + Application.persistentDataPath);
    }

    public static bool Load(string id, D_LevelData level)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Loading Started!\n ====================");
        string completePath = Application.persistentDataPath + string.Format("{0}.{1}", id, suffix);
        if (File.Exists(completePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(completePath, FileMode.Open);

            LevelWrapper levelWrpd = JsonUtility.FromJson<LevelWrapper>((string)bf.Deserialize(file));

            LevelWrapper.Unwrap(level, levelWrpd); 

            file.Close();
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Loading Done!\n ====================");
            return true;
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("SOS: No File existed for path " + completePath);
            return false;
        }
    }
}

[Serializable]
public class LevelWrapper
{
    public string levelID;

    public List<string> characterJsonList = new List<string>();
    public List<string> structureJsonList = new List<string>();
    public List<string> itemJsonList = new List<string>();
    public string gridJson;
    public string terrainJson;
    
    public LevelWrapper(D_LevelData level)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Wrapping Level "+level.mID+"!\n --------------------");
        levelID = level.mID;

        foreach(D_Character character in level.mCharacters)
        {
            CharacterWrapper characterWrpd = new CharacterWrapper(character);
            characterJsonList.Add( JsonUtility.ToJson(characterWrpd, SOS_Level.prettyPrint) );
        }
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS ("+characterJsonList.Count+" Characters): ");

        foreach(D_Structure structure in level.mStructures)
        {
            StructureWrapper structureWrpd = new StructureWrapper(structure);
            structureJsonList.Add( JsonUtility.ToJson(structureWrpd, SOS_Level.prettyPrint) );
        }
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS (" + structureJsonList.Count + " Structures)");

        foreach(D_Item item in level.mItems)
        {
            ItemWrapper itemWrpd = new ItemWrapper(item);
            itemJsonList.Add( JsonUtility.ToJson(itemWrpd, SOS_Level.prettyPrint) );
        }
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS (" + itemJsonList.Count + " Items)");

        GridWrapper gridWrpd = new GridWrapper(level.mGrid);
        gridJson = JsonUtility.ToJson(gridWrpd, SOS_Level.prettyPrint);
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS (Grid): " + gridJson);

        TerrainWrapper terrainWrpd = new TerrainWrapper(level.mTerrain);
        terrainJson = JsonUtility.ToJson(terrainWrpd, SOS_Level.prettyPrint);
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("SOS (Terrain): " + terrainJson);

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Level "+level.mID+" Wrapped!\n --------------------");
    }

    public static void Unwrap(D_LevelData level, LevelWrapper levelWrpd)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("SOS: Unwrapping Level "+levelWrpd.levelID+"!\n --------------------");
        level.mID = levelWrpd.levelID;

        //Unwrap Terrain
        level.mTerrain = TerrainWrapper.Unwrap(JsonUtility.FromJson<TerrainWrapper>( levelWrpd.terrainJson ));

        //Unwrap Grid
        level.mGrid = GridWrapper.Unwrap(JsonUtility.FromJson<GridWrapper>( levelWrpd.gridJson ));

        //Unwrap Items
        D_Item loadedItem;
        foreach(string item in levelWrpd.itemJsonList)
        {
            loadedItem = ItemWrapper.Unwrap(JsonUtility.FromJson<ItemWrapper>(item));
            level.mItems.Add(loadedItem);
        }

        //Unwrap Structures
        D_Structure loadedStructure;
        foreach(string structure in levelWrpd.structureJsonList)
        {
            loadedStructure = StructureWrapper.Unwrap(JsonUtility.FromJson<StructureWrapper>(structure));
            level.mStructures.Add(loadedStructure);
        }

        //Unwrap Chracters
        D_Character loadedCharacter;
        foreach(string character in levelWrpd.characterJsonList)
        {
            loadedCharacter = CharacterWrapper.Unwrap(JsonUtility.FromJson<CharacterWrapper>(character));
            level.mCharacters.Add(loadedCharacter);
        }
    }
}

[Serializable]
public class CharacterWrapper
{
    public string name;

    public CharacterWrapper(D_Character character)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Wrapping Character " + character.mName + "!\n . . . . . . . . . .");
        name = character.mName;
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("NOT IMPLEMENTED!");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Character " + character.mName + " Wrapped!\n . . . . . . . . . .");
    }

    public static D_Character Unwrap(CharacterWrapper characterWrpd)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Unwrapping Character " + characterWrpd.name + "!\n . . . . . . . . . .");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("NOT IMPLEMENTED!");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Character " + characterWrpd.name + " Unwrapped!\n . . . . . . . . . .");
        return null;
    }
}

[Serializable]
public class StructureWrapper
{
    public string name;

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public int x = -1;
    public int y = -1;

    public int integrity;
    // Only D_Tree is an D_IInventory
    // public List<D_Item> inventory;

    public StructureWrapper(D_Structure structure)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Wrapping Structure " + structure.name + "!\n . . . . . . . . . .");
        name = structure.name;
        
        
        // Transform
        this.position = structure.transform.position;
        this.rotation = structure.transform.eulerAngles;
        this.scale = structure.transform.localScale;

        // Node
        if(structure.GetNode() != null)
        {
            this.x = structure.GetNode().x;
            this.y = structure.GetNode().y; 
        }

        // integrity
        this.integrity = structure.GetIntegrity();
        // inventory - noneed here
        // inventory = structure.GetInventory();
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Structure " + structure.name + " Wrapped!\n . . . . . . . . . .");
    }

    public static D_Structure Unwrap(StructureWrapper structureWrpd)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Unwrapping Structure " + structureWrpd.name + "!\n . . . . . . . . . .");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("NOT IMPLEMENTED!");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Structure " + structureWrpd.name + " Unwrapped!\n . . . . . . . . . .");
        return null;
    }
}

[Serializable]
public class ItemWrapper
{
    public string name;

    public ItemWrapper(D_Item item)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Wrapping Item " + item.name + "!\n . . . . . . . . . .");
        this.name = item.name;

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("NOT IMPLEMENTED!");

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Item " + item.name + " Wrapped!\n . . . . . . . . . .");
    }

    public static D_Item Unwrap(ItemWrapper itemWrpd)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Unwrapping Item " + itemWrpd.name + "!\n . . . . . . . . . .");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Error)) Debug.LogError("NOT IMPLEMENTED!");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Item " + itemWrpd.name + " Unwrapped!\n . . . . . . . . . .");
        return null;
    }
}

[Serializable]
public class GridWrapper
{
    public int id;

    public GridWrapper(A_Grid grid)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Wrapping Grid " + grid.mID + "!\n . . . . . . . . . .");
        this.id = grid.mID;

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("NOT fully IMPLEMENTED!");
       
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Grid " + grid.mID + " Wrapped!\n . . . . . . . . . .");
    }

    public static A_Grid Unwrap(GridWrapper gridWrpd)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Unwrapping Grid " + gridWrpd.id + "!\n . . . . . . . . . .");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("NOT fully IMPLEMENTED!");
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Grid " + gridWrpd.id + " Unwrapped!\n . . . . . . . . . .");
        return null;
    }
}

[Serializable]
public class TerrainWrapper
{
    public string name;
    public float posX, posY, posZ;
    public float sizeX, sizeY, sizeZ;

    public int heightWidth, heightHeight, heightRes;
    public float[] heightMap;

    public List<string> splatJsons = new List<string>();

    public TerrainWrapper(Terrain terrain)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Wrapping Terrain " + terrain.name + "!\n . . . . . . . . . .");
        this.name = terrain.name;
        TerrainData data = terrain.terrainData;

        // position
        posX = terrain.transform.position.x;
        posY = terrain.transform.position.y;
        posZ = terrain.transform.position.z;

        // The total size in world units of the terrain
        sizeX = data.size.x;
        sizeY = data.size.y;
        sizeZ = data.size.z;

        // Save HeightMap
        heightRes = data.heightmapResolution;
        heightWidth = data.heightmapWidth;
        heightHeight = data.heightmapHeight;

        float[,] heightMap2D = terrain.terrainData.GetHeights(0, 0, heightWidth, heightHeight);
        heightMap = new float[heightWidth * heightHeight];
        for (int k = 0; k < heightHeight; k++)
        {
            for(int i = 0; i < heightWidth; i++)
            {
                heightMap[heightWidth * k + i] = heightMap2D[i,k];
            }
        }

        // Splat textures are the ground textures
        SplatPrototype[] splats = data.splatPrototypes;
        foreach(SplatPrototype splat in splats )
        {
            SplatWrapper splatWrpd = new SplatWrapper(splat);
            splatJsons.Add(JsonUtility.ToJson(splatWrpd));
        }
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Terrain wrapped " + splatJsons.Count + " SplatPrototypes!");


        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Terrain " + terrain.name + " Wrapped!\n . . . . . . . . . .");
    }

    public static Terrain Unwrap(TerrainWrapper terrainWrpd )
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Unwrapping Terrain " + terrainWrpd.name + "!\n . . . . . . . . . .");
        TerrainData data = new TerrainData();

        data.heightmapResolution = terrainWrpd.heightRes;  // <-- Resolution has to be set BEFORE the size, or it will resize everything

        // The total size in world units of the terrain
        data.size = new Vector3(terrainWrpd.sizeX, terrainWrpd.sizeY, terrainWrpd.sizeZ);
        Debug.Log("(>o.o)> " + data.size);
        
        // Load HeightMap
        float[,] heightMap2D = new float[terrainWrpd.heightWidth, terrainWrpd.heightHeight];
        for (int k = 0; k < terrainWrpd.heightWidth; k++)
        {
            for (int i = 0; i < terrainWrpd.heightHeight; i++)
            {
                heightMap2D[i, k] = terrainWrpd.heightMap[terrainWrpd.heightWidth * k + i];
            }
        }
        data.SetHeights(0, 0, heightMap2D);

        // Load Splat textures (ground textures)
        SplatPrototype[] splats = new SplatPrototype[terrainWrpd.splatJsons.Count];
        for(int n = 0; n < terrainWrpd.splatJsons.Count; n++)
        {
            splats[n] = SplatWrapper.Unwrap( JsonUtility.FromJson<SplatWrapper>(terrainWrpd.splatJsons[n]) );
        }
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Terrain unwrapped " + splats.Length + " SplatPrototypes!");
        data.splatPrototypes = splats;

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Warning)) Debug.LogWarning("ToDo: Alphamaps!");
        // ToDo: AlphaMaps
        // https://docs.unity3d.com/ScriptReference/TerrainData.SetAlphamaps.html 
        // data.SetAlphamaps

        Terrain terrain = Terrain.CreateTerrainGameObject(data).GetComponent<Terrain>();
        terrain.transform.position = new Vector3(terrainWrpd.posX, terrainWrpd.posY, terrainWrpd.posZ);

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_SOS_Message)) Debug.Log("Terrain " + terrainWrpd.name + " Unwrapped!\n . . . . . . . . . .");
        return terrain;
    }
}

[Serializable]
public class SplatWrapper
{
    public byte[] rawTextureData;
    public float tileSizeX, tileSizeY;
    public int texWidth;
    public int texHeight;

    public SplatWrapper(SplatPrototype splat)
    {
        rawTextureData = splat.texture.GetRawTextureData();
        Debug.Log("(Save) Raw Texture Data: " + rawTextureData.Length + "\nFormat: " + splat.texture.format);
        texWidth = splat.texture.width;
        texHeight = splat.texture.height;
        tileSizeX = splat.tileSize.x;
        tileSizeY = splat.tileSize.y;
    }

    public static SplatPrototype Unwrap(SplatWrapper splatWrpd)
    {
        SplatPrototype splat = new SplatPrototype();
        Texture2D tex = new Texture2D(splatWrpd.texWidth, splatWrpd.texHeight, TextureFormat.ETC_RGB4, true);
        Debug.Log("(Load) Raw Texture Data: " + splatWrpd.rawTextureData.Length);
        tex.LoadRawTextureData(splatWrpd.rawTextureData);
        tex.Apply();
        splat.texture = tex;
        splat.tileSize = new Vector2(splatWrpd.tileSizeX, splatWrpd.tileSizeY);

        return splat;
    }
}