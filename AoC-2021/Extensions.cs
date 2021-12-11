
namespace AoC_2021 {
using System;

    public static class Extensions {
        
        public static void ToEach<T>(this T[][] array, Action<(int y, int x)> block) {
            for (var j = 0; j < array.Length; j++) 
                for (var i = 0; i < array[j].Length; i++)
                    block((y: j, x: i));
        }
        public static void ToEach<T>(this T[][] array, Action<int, int> block) {
            for (var j = 0; j < array.Length; j++) 
                for (var i = 0; i < array[j].Length; i++)
                    block(j, i);
        }
    }
}