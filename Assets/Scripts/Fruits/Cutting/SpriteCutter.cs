using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    private Mesh _rightSideMesh;
    private Mesh _leftSideMesh;

    private Vector2[] _currentVertices;
    private ushort[] _currentTriangles;
    private Vector2[] _currentUV;

    private const int TriangleVertexCount = 3;

    private float _UVkX, _UVkY, _UVbX, _UVbY;

    [SerializeField] private Material[] materials;
    private Material _currentMaterial;

    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private float Force;
    private Vector2 _leftSideDirection;
    private Vector2 _rightSideDirection;
    
    private ObjectPhysics _leftSideObjectPhysics;
    private ObjectPhysics _rightSideObjectPhysics;

    private MeshRenderer _leftSideMeshRenderer;
    private MeshRenderer _rightSideMeshRenderer;

    private void DivideVertices(Vector2 p1, Vector2 p2, bool[] isAtLeftSide, List<Vector3> leftSideVertices, List<Vector3> rightSideVertices, int[] newIndexes)
    {
        for (var i = 0; i < _currentVertices.Length; i++)
        {
            var p0 = new Vector2(_currentVertices[i].x, _currentVertices[i].y);
            
            if (p0.x * (p2.y - p1.y) + p1.x * (p0.y - p2.y) + p2.x * (p1.y - p0.y) > 0)
            {
                isAtLeftSide[i] = true;
                leftSideVertices.Add(_currentVertices[i]);
                newIndexes[i] = leftSideVertices.Count - 1;
            }
            else
            {
                rightSideVertices.Add(_currentVertices[i]);
                newIndexes[i] = rightSideVertices.Count - 1;
            }
        }
    }
    
    private void DivideUVs(List<Vector2> leftSideUV, List<Vector2> rightSideUV, bool[] isAtLeftSide)
    {
        for (var i = 0; i < _currentVertices.Length; i++)
        {
            if (isAtLeftSide[i])
            {
                leftSideUV.Add(_currentUV[i]);
            }
            else
            {
                rightSideUV.Add(_currentUV[i]);
            }
        }
    }

    private void PushTriangle(List<int> triangles, int index, int[] newIndexes)
    {
        var length = index + TriangleVertexCount;
        
        for (var j = index; j < length; j++)
        {
            triangles.Add(newIndexes[_currentTriangles[j]]);
        }
    }

    private void SetNewVertex(float A1, float B1, float C1, int vertex1, int vertex2, bool[,] existNewVertex, List<Vector2> leftSideUV, List<Vector2> rightSideUV, int[,] newVerticesIndexes, bool[] isAtLeftSide, List<Vector3> leftSideVertices, List<Vector3> rightSideVertices)
    {
        if (!existNewVertex[vertex1, vertex2])
        {
            existNewVertex[vertex1, vertex2] = true;
            existNewVertex[vertex2, vertex1] = true;
            var A2 = _currentVertices[vertex2].y - _currentVertices[vertex1].y;
            var B2 =  _currentVertices[vertex1].x - _currentVertices[vertex2].x;
            var C2 = - (A2 * _currentVertices[vertex1].x + B2 * _currentVertices[vertex1].y);
            var newVertexPosition = new Vector2((B2 * C1 - B1 * C2) / (A2 * B1 - A1 * B2), (A2 * C1 - A1 * C2) / (B2 * A1 - B1 * A2));

            if (isAtLeftSide[vertex1])
            {
                newVerticesIndexes[vertex1, vertex2] = leftSideVertices.Count;
                newVerticesIndexes[vertex2, vertex1] = rightSideVertices.Count;
            }
            else
            {
                newVerticesIndexes[vertex1, vertex2] = rightSideVertices.Count;
                newVerticesIndexes[vertex2, vertex1] = leftSideVertices.Count;
            }
            
            rightSideVertices.Add(newVertexPosition);
            leftSideVertices.Add(newVertexPosition);
            rightSideUV.Add(new Vector2(_UVkX * (newVertexPosition.x + _UVbX), _UVkY * (newVertexPosition.y + _UVbY)));
            leftSideUV.Add(new Vector2(_UVkX * (newVertexPosition.x + _UVbX), _UVkY * (newVertexPosition.y + _UVbY)));
        }
    }

    private void CutTriangle(float A, float B, float C, int index, bool[,] existNewVertex, List<Vector2> leftSideUV, List<Vector2> rightSideUV, bool[] isAtLeftSide, int[,] newVerticesIndexes, List<int> leftSideTriangles, List<int> rightSideTriangles, List<Vector3> leftSideVertices, List<Vector3> rightSideVertices, int[] newIndexes)
    {
        int[] triangle = new int[TriangleVertexCount];
        
        if (isAtLeftSide[_currentTriangles[index]] == isAtLeftSide[_currentTriangles[index + 1]])
        {
            triangle[0] = _currentTriangles[index];
            triangle[1] = _currentTriangles[index + 1];
            triangle[2] = _currentTriangles[index + 2];
        }
        else if (isAtLeftSide[_currentTriangles[index + 1]] == isAtLeftSide[_currentTriangles[index + 2]])
        {
            triangle[0] = _currentTriangles[index + 1];
            triangle[1] = _currentTriangles[index + 2];
            triangle[2] = _currentTriangles[index];
        }
        else
        {
            triangle[0] = _currentTriangles[index];
            triangle[1] = _currentTriangles[index + 2];
            triangle[2] = _currentTriangles[index + 1];
        }

        for (var j = 0; j < TriangleVertexCount - 1; j++)
        {
            SetNewVertex(A, B, C, triangle[j], triangle[TriangleVertexCount - 1], existNewVertex, leftSideUV, rightSideUV, newVerticesIndexes, isAtLeftSide, leftSideVertices, rightSideVertices);
        }

        if (isAtLeftSide[triangle[0]])
        {
            leftSideTriangles.Add(newIndexes[triangle[0]]);
            leftSideTriangles.Add(newIndexes[triangle[1]]);
            leftSideTriangles.Add(newVerticesIndexes[triangle[1], triangle[2]]);
            
            leftSideTriangles.Add(newIndexes[triangle[0]]);
            leftSideTriangles.Add(newVerticesIndexes[triangle[0], triangle[2]]);
            leftSideTriangles.Add(newVerticesIndexes[triangle[1], triangle[2]]);

            rightSideTriangles.Add(newVerticesIndexes[triangle[2], triangle[0]]);
            rightSideTriangles.Add(newVerticesIndexes[triangle[2], triangle[1]]);
            rightSideTriangles.Add(newIndexes[triangle[2]]);
        }
        else
        {
            rightSideTriangles.Add(newIndexes[triangle[0]]);
            rightSideTriangles.Add(newIndexes[triangle[1]]);
            rightSideTriangles.Add(newVerticesIndexes[triangle[1], triangle[2]]);
            
            rightSideTriangles.Add(newIndexes[triangle[0]]);
            rightSideTriangles.Add(newVerticesIndexes[triangle[0], triangle[2]]);
            rightSideTriangles.Add(newVerticesIndexes[triangle[1], triangle[2]]);

            leftSideTriangles.Add(newVerticesIndexes[triangle[2], triangle[0]]);
            leftSideTriangles.Add(newVerticesIndexes[triangle[2], triangle[1]]);
            leftSideTriangles.Add(newIndexes[triangle[2]]);
        }
    }

    private void DivideTriangles(float A, float B, float C, bool[] isAtLeftSide, List<Vector2> leftSideUV, List<Vector2> rightSideUV, List<int> leftSideTriangles, List<int> rightSideTriangles, int[] newIndexes, List<Vector3> leftSideVertices, List<Vector3> rightSideVertices)
    {
        var newVerticesIndexes = new int[_currentVertices.Length, _currentVertices.Length];
        var existNewVertex = new bool[_currentVertices.Length, _currentVertices.Length];
        
        var i = 0;
        while (i < _currentTriangles.Length)
        {
            if (isAtLeftSide[_currentTriangles[i]] == isAtLeftSide[_currentTriangles[i + 1]] && isAtLeftSide[_currentTriangles[i + 1]] == isAtLeftSide[_currentTriangles[i + 2]])
            {
                PushTriangle(isAtLeftSide[_currentTriangles[i]]? leftSideTriangles : rightSideTriangles, i, newIndexes);
            }
            else
            {
                CutTriangle(A, B, C, i, existNewVertex, leftSideUV, rightSideUV, isAtLeftSide, newVerticesIndexes, leftSideTriangles, rightSideTriangles, leftSideVertices, rightSideVertices, newIndexes);
            }
            i += TriangleVertexCount;
        }
    }

    private void SetDirection(float A, float B, Vector2 objectToCutDirection)
    {
        var e = Force / math.sqrt(A * A + B * B);
        
        var Aside = (A > 0)? 1: -1;
        var Bside = (B > 0)? 1: -1;
        var side = Aside * Bside;

        _leftSideDirection = new Vector2(-side * B * e, side * A * e);
        _rightSideDirection = new Vector2(side * B * e, -side * A * e);
        
        _rightSideDirection += objectToCutDirection;
        _leftSideDirection += objectToCutDirection;
    }
    
    private void SetSides(Vector2 p1, Vector2 p2, Vector2 objectToCutDirection)
    {
        var leftSideVertices = new List<Vector3>();
        var rightSideVertices = new List<Vector3>();
        var leftSideTriangles = new List<int>();
        var rightSideTriangles = new List<int>();
        var leftSideUV = new List<Vector2>();
        var rightSideUV = new List<Vector2>();

        var isAtLeftSide = new bool[_currentVertices.Length];
        var newIndexes = new int[_currentVertices.Length];

        _UVkX = (_currentUV[0].x - _currentUV[1].x) / (_currentVertices[0].x - _currentVertices[1].x);
        _UVkY = (_currentUV[0].y - _currentUV[1].y) / (_currentVertices[0].y - _currentVertices[1].y);
        _UVbX = _currentUV[0].x / _UVkX - _currentVertices[0].x;
        _UVbY = _currentUV[0].y / _UVkY - _currentVertices[0].y;

        DivideVertices(p1, p2, isAtLeftSide, leftSideVertices, rightSideVertices, newIndexes);
        
        DivideUVs(leftSideUV, rightSideUV, isAtLeftSide);

        var A = p2.y - p1.y;

        var B = p1.x - p2.x;
        
        var C = - (A * p1.x + B * p1.y);

        DivideTriangles(A, B, C, isAtLeftSide, leftSideUV, rightSideUV, leftSideTriangles, rightSideTriangles, newIndexes, leftSideVertices, rightSideVertices);

        _rightSideMesh = new Mesh();
        _leftSideMesh = new Mesh();

        _rightSideMesh.vertices = rightSideVertices.ToArray();
        _leftSideMesh.vertices = leftSideVertices.ToArray();

        _rightSideMesh.triangles = rightSideTriangles.ToArray();
        _leftSideMesh.triangles = leftSideTriangles.ToArray();

        _rightSideMesh.uv = rightSideUV.ToArray();
        _leftSideMesh.uv = leftSideUV.ToArray();

        SetDirection(A, B, objectToCutDirection);
    }

    public GameObject[] CutObject(GameObject objectToCut, Transform parent, Sprite currentSprite, int materialIndex, Vector2 p1, Vector2 p2, Vector2 objectToCutDirection, PlayerConfiguration playerConfiguration)
    {
        _currentVertices = currentSprite.vertices;
        _currentTriangles = currentSprite.triangles;
        _currentUV = currentSprite.uv;

        p1 = objectToCut.transform.InverseTransformPoint(p1);
        p2 = objectToCut.transform.InverseTransformPoint(p2);

        SetSides(p1, p2, objectToCutDirection);
        
        var _leftSideObject = Instantiate(slicePrefab, objectToCut.transform.position, objectToCut.transform.rotation, parent);
        var _rightSideObject = Instantiate(slicePrefab, objectToCut.transform.position, objectToCut.transform.rotation, parent);

        _rightSideObject.GetComponent<MeshFilter>().mesh = _rightSideMesh;
        _leftSideObject.GetComponent<MeshFilter>().mesh = _leftSideMesh;
        
        _currentMaterial = materials[materialIndex];

        _rightSideMeshRenderer = _rightSideObject.GetComponent<MeshRenderer>();
        _leftSideMeshRenderer = _leftSideObject.GetComponent<MeshRenderer>();
        _leftSideMeshRenderer.material = _rightSideMeshRenderer.material = _currentMaterial;
        _rightSideMeshRenderer.sortingOrder = 1;
        _leftSideMeshRenderer.sortingOrder = 1;
        _leftSideObject.transform.localScale = _rightSideObject.transform.localScale = objectToCut.transform.localScale;

        _leftSideObjectPhysics = _leftSideObject.GetComponent<ObjectPhysics>();
        _rightSideObjectPhysics = _rightSideObject.GetComponent<ObjectPhysics>();
        _leftSideObjectPhysics.direction = _leftSideDirection;
        _rightSideObjectPhysics.direction = _rightSideDirection;
        _leftSideObjectPhysics.playerConfiguration = _rightSideObjectPhysics.playerConfiguration = playerConfiguration;
        return new GameObject[] { _leftSideObject, _rightSideObject };
    }
}
