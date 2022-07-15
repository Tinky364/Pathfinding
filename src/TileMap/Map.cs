﻿using System.Collections.Generic;
using Dijkstra;
using Godot;

namespace TileMap
{
    public class Map : Reference, IGraph<Tile>
    {
        private readonly Dictionary<string, Tile> _map;
        private readonly Dictionary<string, List<Edge<Tile>>> _edges;

        public Map(int xSize, int ySize)
        {
            _map = new Dictionary<string, Tile>();
            InitializeMap(xSize, ySize);
            _edges = new Dictionary<string, List<Edge<Tile>>>();
            InitializeEdges(xSize, ySize);
        }
        
        public Tile GetTile(int x, int y) => _map[Tile.ToName(x, y)];

        public Tile GetTile(string name) => _map[name];
        
        public IEnumerable<Edge<Tile>> GetEdges(PointRecord<Tile> pointRecord)
        {
            foreach (var pair in _edges)
            {
                if (pair.Key == pointRecord.Point.Name) return pair.Value;
            }
            return null;
        }
        
        private void InitializeMap(int xSize, int ySize)
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Tile tile = new Tile(x, y);
                    _map.Add(tile.Name, tile);
                }
            }
        }

        private void InitializeEdges(int xSize, int ySize)
        {
            foreach (var pair in _map)
            {
                List<string> neighborsNames = Tile.NeighborsNames(pair.Value, xSize, ySize);

                List<Edge<Tile>> edges = new List<Edge<Tile>>();
                foreach (string name in neighborsNames)
                {
                    Tile tile = GetTile(name);
                    edges.Add(new Edge<Tile>(pair.Value, tile, tile.Cost));
                }
                _edges.Add(pair.Key, edges);
            }
        }
    }
}