﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
	public ChunkData data;

	MeshFilter filter;
	MeshCollider coll;

	void Start()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
	}

	void Update()
	{
		if(data.update)
		{
			data.update = false;
			data.UpdateChunk();
		}
	}

	/**
	 * Sends the calculated mesh information in the
	 * chunk to the mesh and collision components.
	 **/
	public void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();
		
		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		
		coll.sharedMesh = null;
		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();
		
		coll.sharedMesh = mesh;
	}
}
