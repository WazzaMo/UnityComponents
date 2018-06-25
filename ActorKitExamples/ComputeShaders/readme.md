# ComputeShader Tests

## Overview
This example exercises compute shaders to determine basic things about the computing environment. Code written in compute shaders execute differently to regular CPU environments, in terms of parallelism, loops and ordering.

## TestInvokationOrder
When dispatching to the GPU shader, the number of threads started equals the number of dispatches by X, Y, Z dimension multiplied by the group sizes defined in the shader.

All threads execute in parallel - Unity's documentation is a bit blurry on this.

Further, the order of execution isn't guaranteed according to some sources.

### Results GTX 1060 on PC

On an NVIDIA GTX 1060 (MaxQ), DirectX 12 drivers, Unity 2017.3, the execution order is linear. Thread group size 8 in X with dispatch of 2 in X, giving 16 threads.

That is thread group 0 executes first, followed by group 1.
Each group-level thread ID executes in order (0 to 8) per group.
The thread IDs at the dispatch level execute from 0 to 15.

### Results Android - Google Pixel
On a Google Pixel running Android 8.1 but Unity player build for v 7.1, the order of execution is more random.

The sequence of threads is:
4, 0, 5, 1, 6, 2, 7, 3, 12, 8, 13, 9, 14, 10, 15. 11

These threads were in thread group 0 (0 to 7), followed by group 1(8 to 15). This is true for successive runs. The thread IDs appear to be interleaved.

# References
[NVIDIA DirectCompute Programming Guide](http://developer.download.nvidia.com/compute/DevZone/docs/html/DirectCompute/doc/DirectCompute_Programming_Guide.pdf)

[Unity Technologies - ComputeShader.Dispatch](https://docs.unity3d.com/ScriptReference/ComputeShader.Dispatch.html)

