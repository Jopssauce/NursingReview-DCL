using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

// Place this script on a gameobject and run (make an editor button maybe idk too lazy)
public class CardImporter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MakeCards();
    }

    void MakeCards()
    {
        string folderName = "19_NurseStation";
        string spritesDirectoryPath = Application.dataPath + "/Sprites/Content CARDS/" + folderName;
        string assetDirectoryPath = Application.dataPath + "/Scriptable Objects/System Topics/Cards/" + folderName;
        if (Directory.Exists(spritesDirectoryPath))
        {
            DirectoryInfo dir = new DirectoryInfo(spritesDirectoryPath);
            DirectoryInfo createdDirectory = Directory.CreateDirectory(assetDirectoryPath);
            FileInfo[] files = dir.GetFiles();

            FileInfo frontFace = null;
            FileInfo backFace = null;
            string currentName = "";
            DataCard card = null;

            foreach (FileInfo file in files)
            {
                // Exclude meta files if iterating within the Assets folder
                if (file.Extension != ".meta")
                {
                    string t = file.Name.Substring(file.Name.Length - 6, 2);
                    string fileName = file.Name.Substring(0, file.Name.Length - 5);
                    int num = int.Parse(t);
                    //Debug.Log("Found file: " + fileName + num);

                    string spritePath = "Assets/Sprites/Content CARDS/" + folderName + "/" + file.Name;
                    //Debug.Log(spritePath);
                    // Perform operations on the file here

                    // Front
                    if (num % 2 != 0)
                    {
                        // Create Card
                        card = ScriptableObject.CreateInstance<DataCard>();

                        Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
                        card.FrontFace = sprite;

                        if (sprite.textureRect.width > sprite.textureRect.height)
                        {
                            card.IsFrontHorizontal = true;
                            Debug.LogWarning("Sprite is horizontal!");
                        }

                        currentName = System.IO.Path.GetFileNameWithoutExtension(Application.dataPath + "/Sprites/Content CARDS/" + file.Name);
                        Debug.Log(currentName);
                        AssetDatabase.CreateAsset(card, "Assets/Scriptable Objects/System Topics/Cards/" + folderName + "/" + currentName + ".asset");
                    }
                    // Back
                    else
                    {
                        // Rename Created Card when back face is found
                        Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
                        card.BackFace = sprite;
                        if (sprite.textureRect.width > sprite.textureRect.height)
                        {
                            card.IsBackHorizontal = true;
                            Debug.LogWarning("Sprite is horizontal!");
                        }

                        string lastName = currentName;
                        currentName += "-" + "0" + num;
                        Debug.Log(currentName);

                        DataCard newCard = ScriptableObject.CreateInstance<DataCard>();
                        newCard.FrontFace = card.FrontFace;
                        newCard.BackFace = card.BackFace;
                        newCard.IsFrontHorizontal = card.IsFrontHorizontal;
                        newCard.IsBackHorizontal = card.IsBackHorizontal;

                        AssetDatabase.CreateAsset(newCard, "Assets/Scriptable Objects/System Topics/Cards/" + folderName + "/" + currentName + ".asset");
                        AssetDatabase.DeleteAsset("Assets/Scriptable Objects/System Topics/Cards/" + folderName + "/" + lastName + ".asset");

                    }

                    
                }
            }

        }
        else
        {
            Debug.LogWarning("Directory not found: " + spritesDirectoryPath);
        }
    }
 
}
