using UnityEditor;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace BricksBreaker {
    public class Brick : Tile {
        public int score;

#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("Assets/Create/RoadTile")]
        public static void CreateRoadTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Brick Tile", 
                "New Brick Tile", "Asset", "Save Brick Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(CreateInstance<Brick>(), path);
        }
#endif
    }
}