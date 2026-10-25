using System.Collections.Generic;
using UnityEngine;

namespace SplineMesh
{
	public struct SourceMesh
	{
		private Vector3 translation;

		private Quaternion rotation;

		private Vector3 scale;

		private List<MeshVertex> vertices;

		private int[] triangles;

		private float minX;

		private float length;

		internal Mesh Mesh { get; }

		internal List<MeshVertex> Vertices => null;

		internal int[] Triangles => null;

		internal float MinX => 0f;

		internal float Length => 0f;

		private SourceMesh(Mesh mesh)
		{
			translation = default;
			rotation = default;
			scale = default;
			Mesh = null;
			vertices = null;
			triangles = null;
			minX = 0f;
			length = 0f;
		}

		private SourceMesh(SourceMesh other)
		{
			translation = default;
			rotation = default;
			scale = default;
			Mesh = null;
			vertices = null;
			triangles = null;
			minX = 0f;
			length = 0f;
		}

		public static SourceMesh Build(Mesh mesh)
		{
			return default;
		}

		public SourceMesh Translate(Vector3 translation)
		{
			return default;
		}

		public SourceMesh Translate(float x, float y, float z)
		{
			return default;
		}

		public SourceMesh Rotate(Quaternion rotation)
		{
			return default;
		}

		public SourceMesh Scale(Vector3 scale)
		{
			return default;
		}

		public SourceMesh Scale(float x, float y, float z)
		{
			return default;
		}

		private void BuildData()
		{
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(SourceMesh sm1, SourceMesh sm2)
		{
			return false;
		}

		public static bool operator !=(SourceMesh sm1, SourceMesh sm2)
		{
			return false;
		}
	}
}
