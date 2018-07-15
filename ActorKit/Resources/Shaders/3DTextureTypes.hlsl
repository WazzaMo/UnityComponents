/*
 * 3DTextureTypes Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


#ifndef __3DTextureTypes__
#define __3DTextureTypes__

struct SizeType
{
	uint Width;
	uint Height;
	uint Depth;
};

uint indexFrom(in SizeType size, uint3 pos)
{
	return size.Width * (pos.y + size.Height * pos.z) + pos.x;
}

#endif // __3DTextureTypes__