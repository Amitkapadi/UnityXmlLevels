using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class DeserializedLevelsLoader
{
    // Levels deserialized
    private DeserializedLevels deserializedLevels;

    private const string prefabsFolder = "Prefabs/";

    struct ItemStruct
    {
        public GameObject prefab;
        public float x;
        public float y;
        public float rot;
        public float scale_x;
        public float scale_y;

        public ItemStruct(GameObject prefabGO, DeserializedLevels.Item deserializedItem)
        {
            prefab = prefabGO;
            x = toFloatZeroIfNull(deserializedItem.x);
            y = toFloatZeroIfNull(deserializedItem.y);
            rot = toFloatZeroIfNull(deserializedItem.rot);
            scale_x = toFloatOneIfNull(deserializedItem.scale_x);
            scale_y = toFloatOneIfNull(deserializedItem.scale_y);
        }
    }

    // Cache prefabs in prefabDict
    Dictionary<string, GameObject> prefabPool;

    // Cache all items with locations
    List<ItemStruct> sceneItemsList;

    Transform parentOfXmlItems;

    public const string xmlItemsGOName = "XmlItems";

    private void init()
    {
        prefabPool = new Dictionary<string, GameObject>();
        sceneItemsList = new List<ItemStruct>();

        // if the XmlItems gameobject folder remained in the Hierarcy, then delete it
        while (GameObject.Find(xmlItemsGOName) != null)
            MonoBehaviour.DestroyImmediate(GameObject.Find(xmlItemsGOName));

        parentOfXmlItems = new GameObject(xmlItemsGOName).transform;
    }

    public void generateItems()
    {
        init();

        createSceneItemsList();

        // Finally instantiate all items
        instantiateItems();
    }

    private DeserializedLevels.Level getCurLevel()
    {
        deserializedLevels = XmlIO.LoadXml<DeserializedLevels>("Levels");

        // if startlevel is in the XML i.e. <Developer StartLevel="3" /> then get level from there
        // otherwise start with level 1
        int startLevel = int.Parse(deserializedLevels.developer.startLevel);

        return deserializedLevels.levels[startLevel - 1]; ;
    }


    private void instantiateItems()
    {
        foreach (ItemStruct item in sceneItemsList)
        {

            // TODO load height coordinate from a directory
            GameObject newGameObject = MonoBehaviour.Instantiate(item.prefab) as GameObject;

            // set position
            setPos2D(newGameObject, new Vector2(item.x, item.y));

            // set rotation
            setRot2D(newGameObject, item.rot);

            // set scale
            newGameObject.transform.localScale = new Vector3(item.scale_x, item.scale_y, 1);

            // set parent
            newGameObject.transform.parent = parentOfXmlItems;
        }
    }

    private void createSceneItemsList()
    {
        // <Item prefab="Chair" x="1" y="10" rot="90" />
        foreach (DeserializedLevels.Item deserializedItem in getCurLevel().items)
        {
            // caching prefabString i.e. "phone"
            string prefabString = deserializedItem.prefab;

            // if the prefab in the item XmlNode has not been loaded then add it to the prefabsDict dictionary,
            if (!prefabPool.ContainsKey(prefabString))
            {
                // load prefab
                GameObject prefabObject = Resources.Load(prefabsFolder + prefabString, typeof(GameObject)) as GameObject;

                // if unsuccesful, error message and jump to next in the foreach loop
                if (prefabObject == null)
                {
                    Debug.LogError("Prefab \"" + prefabString + "\" does not exists.");
                    continue;
                }

                // otherwise add to dictionary
                prefabPool.Add(prefabString, prefabObject);
            }

            ItemStruct item = new ItemStruct(prefabPool[prefabString], deserializedItem);

            sceneItemsList.Add(item);
        }
    }


    // if no value then return zero or one, otherwise convert to float
    static float toFloatZeroIfNull(string value) { return value == null ? 0 : float.Parse(value); }
    static float toFloatOneIfNull(string value) { return value == null ? 1 : float.Parse(value); }


    void setPos2D(GameObject g, Vector2 pos)
    {
        g.transform.position = new Vector3(
            pos.x,
            pos.y,
            g.transform.position.z
        );
    }

    void setRot2D(GameObject g, float rot)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 0, rot);
        g.transform.localRotation = rotation;
    }

}
