using System;
using UnityEngine;

namespace Wordle
{
    public class Row : MonoBehaviour
    {
        public Tile[] Tiles { get; private set; }

        public GameObject TilePrefab;
        
        public string word
        {
            get
            {
                string word = "";
                for (int i = 0; i < Tiles.Length; i++)
                {
                    word += Tiles[i].Letter;
                }
                
                return word;
            }
        }
        private void Awake()
        {
            
            //Tiles = GetComponentsInChildren<Tile>();
        }

        private void Start()
        {
            Tiles = new Tile[Board.Instance.WordLength];
            for (int i = 0; i < Board.Instance.WordLength; i++)
            {
                GameObject tile = Instantiate(TilePrefab, transform);
                Tiles[i] = tile.GetComponent<Tile>();
                Tiles[i].SetTileState(Board.Instance.EmptyState);
            }
        }
    }
}
