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

            ExceptionIfInvalidArray(values);
            stride = SizeInBytes<Tval>();
            count = values.Length;
            var computeBuffer = new ComputeBuffer(count, stride);
            computeBuffer.SetData(values);
            return computeBuffer;
        }

        public static void UpdateBufferForSimpleArray<Tval>(Tval[] values, ComputeBuffer buffer) {
            int stride, count;

            ExceptionIfInvalidArray(values);
            stride = SizeInBytes<Tval>();
            count = values.Length;
            if (buffer.count == count && buffer.stride == stride) {
                buffer.SetData(values);
            }
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

        public static void UpdateBufferForStruct<T>(T UpdatedValue, ComputeBuffer buffer) {
            if (buffer != null
                && buffer.count == 1
                && buffer.stride == SizeInBytes<T>()) {
                var list = new List<T>() { UpdatedValue };
                buffer.SetData(list);
            } else {
                Logging.Warning<ComputeBuffer>("Unable to update ComputeBuffer with updated value of type {0}", TypeUtil.NameOf<T>());
            }
        }

        public static bool CalculateCountAndStride<Tval>(List<Tval[]> values, out int count, out int stride) {
            if (values == null || values.Count() == 0) {
                count = 0;
                stride = 0;
                return false;
            } else {
                values.ForEach(valArray => ExceptionIfInvalidArray(valArray));
                int totalSize = TotalValuesInArrayList(values);
                stride = totalSize * SizeInBytes<Tval>() / values.Count();
                count = values.Count();
                return true;
            }
        }

        public static T[] GetBufferDataArray<T>(ComputeBuffer buffer) {
            T[] data;
            if (buffer == null) {
                data = new T[0];
            } else {
                data = new T[buffer.count];
                buffer.GetData(data);
            }
            return data;
        }

        public static int SizeInBytes<T>() {
            return Marshal.SizeOf(typeof(T));
        }

        private static void ExceptionIfInvalidArray<T>(T[] values) {
            if (values == null) {
                throw new ArgumentException(string.Format("Invalid Argument: NULL of type '{0}' Arrary given for ComputeBuffer", TypeUtil.NameOf<T>()));
            }
            if (values.Length == 0) {
                throw new ArgumentException(
                    string.Format("Invalid Argument: Type '{0}' Array of 0 length given for ComputeBuffer", TypeUtil.NameOf<T>())
                );
            }
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
