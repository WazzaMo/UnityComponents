# ActorKit Examples

## Contents
Scenes provided
- ComputeParallelSum
- ComputeTest-Order
- Texture3DLife-View
- Texture3DSwirl
- Texture3DSwirlCube

### Overview

The compute shader execution order and parallel sum tests
are the first two scenes listed and the results are described below.

The next three from the list above are about manipulating
3D textures in a compute shader, and using the texture in
a scene to see the results.

Unity handles 3D textures in a funny way. It has a Texture3D
class but this appears to be not very usable. Instead
it turned out that to create a Read/Write Texture 3D
a render texture is required and a third dimension must be configured. The class `ActorKit.Shader.Texture3DCompute`
is intended as a drop-in replacement for Texture3D
that works with Compute shaders and regular shaders.
The 'Texture3D*' scenes are about exercising this class
in different ways.

----
## Texture3DCompute Class Test/Example Scenes

Everything specific to the examples for compute shaders
are under the ComputeShaders directory.
The scenes use shaders in the `Resources` directory
and C# code in the `Script` directory.

Here's a mapping of the scene to the main C# component
class.

|        Scene       | Main Script Component   |
|--------------------|-------------------------|
| Texture3DLife-View | ThreeDTextureTest.cs    |
| Texture3DSwirl     | ThreeDTextureCompute.cs |
| Texture3DSwirlCube | ThreeDTextureRender.cs  |

The main discovery about performance of using 3D textures
is that reading or writing the whole texture is a very
expensive process and, if done every frame, will have a
severe impact. Even loading up a massive texture at start-up
can be significant start delay. 
- 32x32x32 up to 64x64x64 was fine on DELL 15" gaming 7757 Laptop
- 512x512x512 did not complete (over 134 million colors)

The best way to use 3D textures is to procedurally manipulate them in a compute shader and render them
in a Vertex / Pixel shader and keep all the data within 
the GPU.

----

## ComputeShader Tests

This example exercises compute shaders to determine basic things about the computing environment. Code written in compute shaders execute differently to regular CPU environments, in terms of parallelism, loops and ordering.

### TestInvokationOrder
When dispatching to the GPU shader, the number of threads started equals the number of dispatches by X, Y, Z dimension multiplied by the group sizes defined in the shader.

All threads execute in parallel - Unity's documentation is a bit blurry on this.

Further, the order of execution isn't guaranteed according to some sources.

#### Results GTX 1060 on PC

On an NVIDIA GTX 1060 (MaxQ), DirectX 12 drivers, Unity 2017.3, the execution order is linear. Thread group size 8 in X with dispatch of 2 in X, giving 16 threads.

That is thread group 0 executes first, followed by group 1.
Each group-level thread ID executes in order (0 to 8) per group.
The thread IDs at the dispatch level execute from 0 to 15.

#### Results Android - Google Pixel
On a Google Pixel running Android 8.1 but Unity player build for v 7.1, the order of execution is more random.

The sequence of threads is:
4, 0, 5, 1, 6, 2, 7, 3, 12, 8, 13, 9, 14, 10, 15. 11

These threads were in thread group 0 (0 to 7), followed by group 1(8 to 15). This is true for successive runs. The thread IDs appear to be interleaved.

### Parallel Sum Example
This test just demonstrates that the NVIDIA HLSL code shared by Nathan Hoobler at GDC 2011 works in Unity (reference below).


## References
[NVIDIA DirectCompute Programming Guide](http://developer.download.nvidia.com/compute/DevZone/docs/html/DirectCompute/doc/DirectCompute_Programming_Guide.pdf)

[Unity Technologies - ComputeShader.Dispatch](https://docs.unity3d.com/ScriptReference/ComputeShader.Dispatch.html)

[Nathan Hoobler, NVIDIA, High Performance Post Processing - GDC 2011](http://www.nvidia.com/content/PDF/GDC2011/Nathan_Hoobler.pdf)

