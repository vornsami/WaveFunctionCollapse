using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WaveFunctionCollapse.Models
{
    internal class MapGeneration
    {
        public static MapData GenerateMap(MapGenData data)
        {
            // setup
            int[] tiledata = data.tileData;

            for (int i = 0; i < tiledata.Length;i++)
            {
                Trace.WriteLine(Convert.ToString(tiledata[i], 2));
            }

            int sizeX = data.mapSizeX, sizeY = data.mapSizeY, n = data.tileTypeCount;
            
            // final map with the final map values
            int[,] finalMap = new int[sizeX, sizeY];
            
            // contains which tiles are possible in any given tile, binary 0 is possible 1 is impossible
            int[,] possibleMap = new int[sizeX, sizeY];

            // Array of all the tiles in random order
            var rand = new Random();
            int[] ordered = Enumerable.Range(0, sizeX * sizeY).OrderBy(x => rand.Next()).ToArray();
            (int x, int y)[] randomTileArray = Array.ConvertAll(ordered, num => ((int a, int b))(num % sizeX, (num - (num % sizeX)) / sizeX));

            int[] weights = data.weights;

            for (int i = 0; i < randomTileArray.Length; i++)
            {
                // Gets tile from queue
                (int x, int y) = randomTileArray[i];
                // If tile already has a value, skip (Should never happen)
                if (finalMap[x, y] > 0)
                    continue;

                // Get the value from the possible map
                int tileValue = GetWeightedRandomValidTile(possibleMap[x, y], n, weights);
                // Set the final tile value
                finalMap[x, y] = tileValue;

                int possibleMapTile = ~(1 << (tileValue - 1));
                possibleMapTile &= (1 << n) - 1;
                possibleMap[x, y] = possibleMapTile;

                UpdatePossibleMap(x, y, data, possibleMap, finalMap);
            }

            MapData mapdata = new(finalMap, data.tileColors);

            return mapdata;
        }
        private static void UpdatePossibleMap(int setX, int setY, MapGenData data, int[,] possibleMap, int[,] finalMap)
        {
            int[] tileData = data.tileData;

            Queue<(int x, int y)> possibleTileQueue = new();
            possibleTileQueue.Enqueue((setX, setY));

            while (possibleTileQueue.Count > 0)
            {
                (int x, int y) = possibleTileQueue.Dequeue();

                if (possibleMap[x, y] == (1 << data.tileTypeCount) - 1)  // Should never happen
                    Trace.WriteLine("???????");

                // Check which types of tiles can be adjacent to this tile
                int value = possibleMap[x, y];
                int possibleAdjacent = getPossibleTiles(value, tileData);
                if (possibleAdjacent == 0)
                    continue;

                // Update and add adjacent tiles to be examined
                UpdateAndQueuePossibleTiles(possibleAdjacent, x - 1, y, data, possibleTileQueue, possibleMap, finalMap);
                UpdateAndQueuePossibleTiles(possibleAdjacent, x + 1, y, data, possibleTileQueue, possibleMap, finalMap);
                UpdateAndQueuePossibleTiles(possibleAdjacent, x, y - 1, data, possibleTileQueue, possibleMap, finalMap);
                UpdateAndQueuePossibleTiles(possibleAdjacent, x, y + 1, data, possibleTileQueue, possibleMap, finalMap);

            }

        }

        private static int GetRandomValidTile(int tile, int n)
        {
            Random rand = new();
            int possibleAsOnes = ~tile & ((1 << n) - 1);
            int validCount = int.PopCount(possibleAsOnes);
            if (validCount == 0) // Failsafe in case for some reason there are no tiles that can be placed
                return 99;

            int ret = 0;
            int r = rand.Next(validCount);

            for (int i = 0; r >= 0; i++)
            {
                ret++;
                if ((possibleAsOnes & 1) == 1) 
                    r--;

                possibleAsOnes >>= 1;
                if (i > n + 1) 
                    throw new Exception("Too big number");
            }
            

            return ret;
        }

        private static int GetWeightedRandomValidTile(int tile, int n, int[] weights)
        {
            int possibleAsOnes = ~tile & ((1 << n) - 1);
            int validCount = int.PopCount(possibleAsOnes);
            if (validCount == 0) // Failsafe in case for some reason there are no tiles that can be placed
                return 99;

            int[] tileWeights = new int[validCount];
            int[] tileNums = new int[validCount];

            int t = 0;
            for (int i = 0; i < n; i++)
            {
                if ((possibleAsOnes & 1) == 1)
                {
                    tileWeights[t] = weights[i];
                    tileNums[t] = i;
                    t++;
                }
                possibleAsOnes >>= 1;

                if (i > n + 1 || t > validCount)
                    throw new Exception("Too big number");
            }

            int targetPos = 0;
            Random rand = new();

            int target = rand.Next(tileWeights.Sum());

            for (int i = 0; i <= validCount; i++)
            {
                target -= tileWeights[i];
                if (target <= 0)
                {
                    targetPos = i;
                    break;
                }
            }

            return tileNums[targetPos] + 1;
        }

        private static List<(int x, int y)> GetNextTiles(int x, int y, MapGenData data, int[,] finalMap)
        {
            // Figure out something better
            List<(int x, int y)> list =
            [
                CheckTile(x - 1, y, data, finalMap),
                CheckTile(x + 1, y, data, finalMap),
                CheckTile(x, y - 1, data, finalMap),
                CheckTile(x, y + 1, data, finalMap),
            ];
            
            return list.Select(a => a).Where(a => a.x != -1).ToList();
        }
        private static (int x, int y) CheckTile(int x, int y, MapGenData data, int[,] finalMap)
        {
            if (x < 0 || y < 0 || x >= data.mapSizeX || y >= data.mapSizeY || finalMap[x, y] != 0)
                return (-1, -1);

            finalMap[x, y] = -1;
            return (x, y);
        }
        private static void UpdateAndQueuePossibleTiles(int value, int x, int y, MapGenData data, Queue<(int x, int y)> possibleTileQueue, int[,] possibleMap, int[,] finalMap)
        {
            if (
                x < 0 || y < 0 || x >= data.mapSizeX || y >= data.mapSizeY // check tile is within the map
                || finalMap[x, y] > 0 // Check tile is not already set
                || int.PopCount(possibleMap[x, y]) == data.tileTypeCount - 1) // Check there is not only one option
                return;

            int atStart = possibleMap[x, y];

            possibleMap[x, y] |= value;
            
            if(atStart != possibleMap[x, y]) // Add only if a change was made
                possibleTileQueue.Enqueue((x, y));

        }
        private static int getPossibleTiles(int tile, int[] tileData)
        {
            int tileCopy = tile;
            int tileCheck = tile;
            // Goes through the tiletypes allowed and adds all the possible tiletypes to list
            for (int i = 0; i < tileData.Length; i++)
            {
                if ((tileCheck & 1) != 1) {
                    tileCopy &= tileData[i]; }

                tileCheck >>= 1;
            }
            return tileCopy;
        }
    }
}
