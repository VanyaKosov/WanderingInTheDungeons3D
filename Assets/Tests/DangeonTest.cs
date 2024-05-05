using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Tests
{
    public class DangeonTest
    {
        private string[] inputMap =
        {
        "#########",
        "# #     #",
        "# # ### #",
        "# # #   #",
        "# # #####",
        "#       #",
        "# ##### #",
        "#   #E  #",
        "#########"
    };

        [Test]
        public void FindPath1()
        {
            var dungeon = new Dungeon(inputMap);
            var path = dungeon.FindPath(new Vector2Int(6, 5), new Vector2Int(6, 7));
            Assert.AreEqual(4, path.Count);

            Assert.AreEqual(new Vector2Int(7, 5), path.Pop());
            Assert.AreEqual(new Vector2Int(7, 6), path.Pop());
            Assert.AreEqual(new Vector2Int(7, 7), path.Pop());
            Assert.AreEqual(new Vector2Int(6, 7), path.Pop());
        }

    }
}