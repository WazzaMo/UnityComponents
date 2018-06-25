# ComputeShader Tests

## Overview
This example exercises compute shaders to determine basic things about the computing environment. Code written in compute shaders execute differently to regular CPU environments, in terms of parallelism, loops and ordering.

## TestInvokationOrder
When dispatching to the GPU shader, the number of threads started equals the number of dispatches by X, Y, Z dimension multiplied by the group sizes defined in the shader.

All threads execute in parallel - Unity's documentation is a bit blurry on this.

Further, the order of execution isn't guaranteed according to some sources.

On an NVIDIA GTX 1060 (MaxQ), DirectX 12 drivers, Unity 2017.3, the execution order is linear. Thread group size 8 in X with dispatch of 2 in X, giving 16 threads.

That is thread group 0 executes first, followed by group 1.
Each group-level thread ID executes in order (0 to 8) per group.
The thread IDs at the dispatch level execute from 0 to 15.

# References
[NVIDIA DirectCompute Programming Guide](http://developer.download.nvidia.com/compute/DevZone/docs/html/DirectCompute/doc/DirectCompute_Programming_Guide.pdf)

[Unity Technologies - ComputeShader.Dispatch](https://docs.unity3d.com/ScriptReference/ComputeShader.Dispatch.html)

