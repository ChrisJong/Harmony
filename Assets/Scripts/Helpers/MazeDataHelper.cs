namespace Helpers {

    using System.IO;
    using System.Collections.Generic;

    using UnityEngine;

    using Maze;
    using GameInfo;

    public static class MazeDataHelper {

        public static void SaveData() {
            int mazeMazeCount = MazeInfo.MazeMoveValue.Count;
            using(var stream = new StreamWriter(AssetPaths.PathPrefab + "Resources/" + "Data.csv", false)) {
                for(int i = 0; i < mazeMazeCount; i++) {
                    stream.WriteLine(i + "," + MazeInfo.MazeMoveValue[i].moveCount + "," + MazeInfo.MazeMoveValue[i].maxMoves);
                }
            }
        }

        public static void LoadData() {
            var data = Resources.Load("Data") as TextAsset;
            MazeInfo.MazeMoveValue = new Dictionary<int, MazeData>();

            using(var stream = new StringReader(data.text)) {
                string line;

                while(!string.IsNullOrEmpty(line = stream.ReadLine())) {
                    var lineParts = line.Split(',');
                    int index;
                    int status;
                    int moves;
                    
                    if(!int.TryParse(lineParts[0], out index))
                        Debug.LogError("Attempted conversion of index failed - " + index == null ? "null" : index.ToString());

                    if(!int.TryParse(lineParts[1], out status))
                        Debug.LogError("Attempted conversion of status failed - " + status == null ? "null" : status.ToString());

                    if(!int.TryParse(lineParts[2], out moves))
                        Debug.LogError("Attempted conversion of moves failed - " + moves == null ? "null" : moves.ToString());
                    else {
                        MazeInfo.MazeMoveValue.Add(index, new MazeData {
                            moveCount = status,
                            maxMoves = moves
                        });
                    }
                }
            }
        }

        public static bool GameDataExists() {
            if(!File.Exists(Application.persistentDataPath + "/GameData.csv")) {
                CreateGameData();
                SaveGameData();
                return false;
            }
            return true;
        }

        public static void CreateGameData() {
            try {
                MazeInfo.MazeMoveValue = new Dictionary<int, MazeData>();
                var data = Resources.Load("Data") as TextAsset;
                using(var stream = new StringReader(data.text)) {
                    string line;

                    while(!string.IsNullOrEmpty(line = stream.ReadLine())) {
                        var lineParts = line.Split(',');
                        int index;
                        int status;
                        int moves;

                        if(!int.TryParse(lineParts[0], out index))
                            Debug.LogError("Attempted conversion of index failed - " + index == null ? "null" : index.ToString());

                        if(!int.TryParse(lineParts[1], out status))
                            Debug.LogError("Attempted conversion of status failed - " + status == null ? "null" : status.ToString());

                        if(!int.TryParse(lineParts[2], out moves))
                            Debug.LogError("Attempted conversion of moves failed - " + moves == null ? "null" : moves.ToString());
                        else {
                            MazeInfo.MazeMoveValue.Add(index, new MazeData {
                                moveCount = status,
                                maxMoves = moves
                            });
                        }
                    }
                }
            } catch(FileNotFoundException) {
            }
        }

        public static void SaveGameData() {
            int mazeMazeCount = MazeInfo.MazeMoveValue.Count;
            using(var stream = new StreamWriter(Application.persistentDataPath + "/GameData.csv", false)) {
                for(int i = 0; i < mazeMazeCount; i++) {
                    stream.WriteLine(i + "," + MazeInfo.MazeMoveValue[i].moveCount + "," + MazeInfo.MazeMoveValue[i].maxMoves);
                }
            }
        }

        public static void LoadGameData() {
            if(GameDataExists()) {
                try {
                    MazeInfo.MazeMoveValue = new Dictionary<int, MazeData>();
                    using(var stream = new StreamReader(Application.persistentDataPath + "/GameData.csv")) {
                        string line;

                        while(!string.IsNullOrEmpty(line = stream.ReadLine())) {
                            var lineParts = line.Split(',');
                            int index;
                            int status;
                            int moves;

                            if(!int.TryParse(lineParts[0], out index))
                                Debug.LogError("Attempted conversion of index failed - " + index == null ? "null" : index.ToString());

                            if(!int.TryParse(lineParts[1], out status))
                                Debug.LogError("Attempted conversion of status failed - " + status == null ? "null" : status.ToString());

                            if(!int.TryParse(lineParts[2], out moves))
                                Debug.LogError("Attempted conversion of moves failed - " + moves == null ? "null" : moves.ToString());
                            else {
                                MazeInfo.MazeMoveValue.Add(index, new MazeData {
                                    moveCount = status,
                                    maxMoves = moves
                                });
                            }
                        }
                    }
                } catch(FileNotFoundException) {
                }
            }
        }

        public static void CheckGameData() {
            bool isGameDataOld = false;

            if(isGameDataOld) {
                DeleteSaveData();
                LoadGameData();
            }
        }

        public static void DeleteSaveData() {
            if(File.Exists(Application.persistentDataPath + "/GameData.csv")) {
#if UNITY_IPHONE
                File.Delete("/private" + Application.persistentDataPath + "/GameData.csv");
#else
                File.Delete(Application.persistentDataPath + "/GameData.csv");
#endif
            }
        }
    }
}