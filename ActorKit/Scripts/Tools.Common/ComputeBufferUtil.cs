/*
 * ComputeBufferUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using UnityEngine;

namespace Tools.Common {

    public static class ComputeBufferUtil {

        public static ComputeBuffer CreateBufferForSimpleArray<Tval>(Tval[] values) {
            int stride, count;
            stride = SizeInBytes<Tval>();
            count = values.Length;
            var computeBuffer = new ComputeBuffer(count, stride);
            computeBuffer.SetData(values);
            return computeBuffer;
        }

        public static ComputeBuffer CreateBufferForListOfArrays<Tval>(List<Tval[]> values) {
            int stride, count;

            if (CalculateCountAndStride<Tval>(values, out count, out stride) ) {
                var computeBuffer = new ComputeBuffer(count, stride);
                var valArray = FlattenedArray<Tval>(values);
                computeBuffer.SetData(valArray);
                return computeBuffer;
            } else {
                return null;
            }
        }

        public static ComputeBuffer CreateBufferForStruct<T>(T value) {
            int count = 1, stride = SizeInBytes<T>();
            var buffer = new ComputeBuffer(count, stride);
            var list = new List<T>();
            list.Add(value);
            buffer.SetData(list);
            return buffer;
        }

        public static bool CalculateCountAndStride<Tval>(List<Tval[]> values, out int count, out int stride) {
            if (values == null || values.Count() == 0) {
                count = 0;
                stride = 0;
                return false;
            } else {
                int totalSize = TotalValuesInArrayList(values);
                stride = totalSize * SizeInBytes<Tval>() / values.Count();
                count = values.Count();
                return true;
            }
        }

        public static int SizeInBytes<T>() {
            return Marshal.SizeOf(typeof(T));
        }

        private static Tval[] FlattenedArray<Tval>(List<Tval[]> values) {
            int total = TotalValuesInArrayList(values);
            var flatArray = new Tval[total];
            int index = 0;
            values.ForEach(array => {
                for(int i = 0; i < array.Length; i++) {
                    flatArray[index] = array[i];
                    index++;
                }
            });
            return flatArray;
        }

        private static int TotalValuesInArrayList<Tval>(List<Tval[]> values) {
            return values.Aggregate((int)0, (size, array2) => size + array2.Length);
        }
    }

}
