using System;
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace BricksBreaker {
    [RequireComponent(typeof(Tilemap))]
    public class Bricks : SerializedMonoBehaviour {
        [SerializeField] float noiseScale;
        [SerializeField] RectInt generatedLayoutBounds;
        [OdinSerialize] SortedDictionary<float, Brick> brickThresholdDictionary;
        // [SerializeField] Brick brick100;
        // [SerializeField] Brick brick200;
        // [SerializeField] Brick brick500;
        // [SerializeField] float brick100Threshold = 0.3f;
        // [SerializeField] float brick200Threshold = 0.7f;
        // [SerializeField] float brick500Threshold = 0.9f;
        
        public event Action<Brick> BrickDestroyed;

        Tilemap _tilemap;

        void Awake() {
            _tilemap = GetComponent<Tilemap>();
        }

        public void Hit(Vector3 position) {
            var cell = _tilemap.WorldToCell(position);
            var tile = _tilemap.GetTile(cell);
            if (tile == null || !(tile is Brick brick)) return;
            _tilemap.SetTile(cell, null);
            BrickDestroyed?.Invoke(brick);
        }

        [Button]
        public void GenerateRandomLayout() {
            if (!_tilemap) _tilemap = GetComponent<Tilemap>();
            _tilemap.ResizeBounds();
            var bounds = _tilemap.cellBounds;
            var allTiles = _tilemap.GetTilesBlock(bounds);

            Vector2Int min = 100 * Vector2Int.one, max = -100 * Vector2Int.one;

            for (int x = 0; x < bounds.size.x; x++) {
                for (int y = 0; y < bounds.size.y; y++) {
                    var tile = allTiles[x + y * bounds.size.x];
                    if (tile != null && tile is Brick) {
                        _tilemap.SetTile(new Vector3Int(bounds.x + x, bounds.y + y, 0), null);
                    }
                }
            }
            
            float newNoise = Random.Range(0,10000f);

            float width = generatedLayoutBounds.width;
            float height = generatedLayoutBounds.height;
            for (int x = generatedLayoutBounds.x; x < generatedLayoutBounds.xMax; x++) {
                for (int y = generatedLayoutBounds.y; y < generatedLayoutBounds.yMax; y++) {
                    float noiseValue = Mathf.PerlinNoise(newNoise + noiseScale * x / width, newNoise + noiseScale * y / height);
                    foreach (var keyValuePair in brickThresholdDictionary.Reverse()) {
                        if (noiseValue > keyValuePair.Key) {
                            _tilemap.SetTile(new Vector3Int(x, y, 0), keyValuePair.Value);
                            break;
                        }
                    }
                }
            }
            
            _tilemap.ResizeBounds();

            for (int x = generatedLayoutBounds.x; x < generatedLayoutBounds.xMax; x++) {
                for (int y = generatedLayoutBounds.y; y < generatedLayoutBounds.yMax; y++) {
                    if (_tilemap.GetTile(new Vector3Int(x, y, 0)) == null && HasBrickBelow(x, y)) {
                        _tilemap.SetTile(new Vector3Int(x, y, 0), brickThresholdDictionary.First().Value);
                    }
                }
            }
        }

        bool HasBrickBelow(int x, int y) => HasBrickBelow(new Vector2Int(x, y));

        bool HasBrickBelow(Vector2Int position) {
            for (int y = _tilemap.cellBounds.yMin ; y < position.y; y++) {
                var tile = _tilemap.GetTile(new Vector3Int(position.x, y, 0));
                if (tile != null && tile is Brick) return true;
            }

            return false;
        }
    }
}