// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectL
{
    public class SearchPrefabsByKeyword : MonoBehaviour
    {
        private const string PREFIX_SEARCH_VALUE = ": ";
        private const string PREFAB_WILD_CARD = "*.prefab";

        [SerializeField]
        private string searchScripts;
        [SerializeField]
        private string searchPrefabs;
        [SerializeField]
        private string searchStringValue;
        [SerializeField]
        private List<string> resultPrefabs;

        private string[] prefabFiles;

        private string[] PrefabFiles
        {
            get
            {
                if (prefabFiles == null || prefabFiles.Length == 0)
                    LoadPrefabs();

                return prefabFiles;
            }
        }

        [EditorButton("SearchByScript")]
        public bool searchByScript;
        private void SearchByScript()
        {
            using (new PerfTimerRegion("SearchByScript"))
            {
                resultPrefabs.Clear();

                var scripts = GetFiles(searchScripts + ".cs.meta");

                foreach (var script in scripts)
                {
                    var guid = FindTextLine(script, "guid");

                    if (string.IsNullOrEmpty(guid))
                        continue;

                    SearchPrefabs(guid);
                }
            }
        }

        [EditorButton("SearchByPrefab")]
        public bool searchByPrefab;
        private void SearchByPrefab()
        {
            using(new PerfTimerRegion("SearchByPrefab"))
            {
                resultPrefabs.Clear();

                var prefabs = GetFiles(searchPrefabs + ".prefab.meta");

                foreach(var prefab in prefabs)
                {
                    var guid = FindTextLine(prefab, "guid");

                    if(string.IsNullOrEmpty(guid))
                        continue;

                    SearchPrefabs(guid);
                }
            }
        }


        [EditorButton("SearchByString")]
        public bool searchByString;
        private void SearchByString()
        {
            using (new PerfTimerRegion("SearchByString"))
            {
                resultPrefabs.Clear();

                var keyword = PREFIX_SEARCH_VALUE + searchStringValue;
                SearchPrefabs(keyword);
            }
        }

        private void LoadPrefabs()
        {
            prefabFiles = GetFiles(PREFAB_WILD_CARD);
        }

        private string[] GetFiles(string searchPattern)
        {
            return Directory.GetFiles(Application.dataPath, searchPattern, SearchOption.AllDirectories).Where(SearchFilter()).ToArray();
        }

        private Func<string, bool> SearchFilter()
        {
            return file => file.Contains(@"\Plugins\") == false &&
                       file.Contains(@"\TextMesh Pro\") == false;
        }

        private string FindTextLine(string path, string keyword)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(keyword))
                        return line;
                }
            }

            return string.Empty;
        }

        private void SearchPrefabs(string keyword)
        {
            foreach (var prefab in PrefabFiles)
            {
                var content = File.ReadAllText(prefab, Encoding.Default);

                if (IsContainsKeyword(content, keyword))
                    resultPrefabs.Add(prefab);
            }
        }

        private bool IsContainsKeyword(string content, string keyword)
        {
            return content.IndexOf(keyword) != -1 ? true : false;
        }
    }
}