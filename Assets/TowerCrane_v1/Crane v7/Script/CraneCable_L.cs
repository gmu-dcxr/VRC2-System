using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CraneCable_L : MonoBehaviour
{
    public float cableStart = 0.037f;
    public float cableEnd = 0.037f;
    public Material lineMaterial;
    public Transform pointLineWheelCableA1;
    public Transform pointLineWheelCableA2;
    public Transform pointLineWheelCableA3;
    public Transform pointLineWheelCableB1;
    public Transform pointLineWheelCableB2;
    public Transform pointLineWheelCableB3;
    private LineRenderer lineWheelCableA1;
    private LineRenderer lineWheelCableA3;
    private LineRenderer lineWheelCableB1;
    private LineRenderer lineWheelCableB3;
    public Transform pointLineWheelCableC1;
    public Transform pointLineWheelCableC2;
    public Transform pointLineWheelCableD1;
    public Transform pointLineWheelCableD2;
    private LineRenderer lineWheelCableC1;
    private LineRenderer lineWheelCableD1;
    public Transform pointLineWheelCableF1;
    public Transform pointLineWheelCableF2;
    public Transform pointLineWheelCableN1;
    public Transform pointLineWheelCableN2;
    public Transform pointLineWheelCableM1;
    public Transform pointLineWheelCableM2;
    public Transform pointLineWheelCableI1;
    public Transform pointLineWheelCableI2;
    public Transform pointLineWheelCableL1;
    public Transform pointLineWheelCableL2;
    private LineRenderer lineWheelCableF1;
    private LineRenderer lineWheelCableM1;
    private LineRenderer lineWheelCableN1;
    private LineRenderer lineWheelCableL1;
    private LineRenderer lineWheelCableI1;
    public Transform pointLineWheelCableK1;
    public Transform pointLineWheelCableK2;
    public Transform pointLineWheelCableK3;
    private LineRenderer lineWheelCableK1;
    private LineRenderer lineWheelCableK3;
    public Transform pointLineWheelCableF3;
    public Transform pointLineWheelCableN3;
    public Transform pointLineWheelCableM3;
    public Transform pointLineWheelCableL3;
    private LineRenderer lineWheelCableF3;
    private LineRenderer lineWheelCableN3;
    private LineRenderer lineWheelCableM3;
    private LineRenderer lineWheelCableL3;

    void Awake()
    {
        lineWheelCableA1 = pointLineWheelCableA1.GetComponent<LineRenderer>();
        lineWheelCableA3 = pointLineWheelCableA3.GetComponent<LineRenderer>();
        lineWheelCableB1 = pointLineWheelCableB1.GetComponent<LineRenderer>();
        lineWheelCableB3 = pointLineWheelCableB3.GetComponent<LineRenderer>();
        lineWheelCableC1 = pointLineWheelCableC1.GetComponent<LineRenderer>();
        lineWheelCableD1 = pointLineWheelCableD1.GetComponent<LineRenderer>();
        lineWheelCableF1 = pointLineWheelCableF1.GetComponent<LineRenderer>();
        lineWheelCableM1 = pointLineWheelCableN1.GetComponent<LineRenderer>();
        lineWheelCableN1 = pointLineWheelCableM1.GetComponent<LineRenderer>();
        lineWheelCableL1 = pointLineWheelCableL1.GetComponent<LineRenderer>();
        lineWheelCableI1 = pointLineWheelCableI1.GetComponent<LineRenderer>();
        lineWheelCableK1 = pointLineWheelCableK1.GetComponent<LineRenderer>();
        lineWheelCableK3 = pointLineWheelCableK3.GetComponent<LineRenderer>();
        //Dop
        lineWheelCableF3 = pointLineWheelCableF3.GetComponent<LineRenderer>();
        lineWheelCableN3 = pointLineWheelCableN3.GetComponent<LineRenderer>();
        lineWheelCableM3 = pointLineWheelCableM3.GetComponent<LineRenderer>();
        lineWheelCableL3 = pointLineWheelCableL3.GetComponent<LineRenderer>();
    }
    void LateUpdate()
    {
        LineRendererCrane();
    }
    public void LineRendererCrane()
    {
        Vector3[] L1_A = new Vector3[2];
        L1_A[0] = pointLineWheelCableA1.position;
        L1_A[1] = pointLineWheelCableA2.position;
        lineWheelCableA1.positionCount = L1_A.Length;
        lineWheelCableA1.SetPositions(L1_A);
        lineWheelCableA1.material = lineMaterial;
        lineWheelCableA1.startWidth = cableStart;
        lineWheelCableA1.endWidth = cableEnd;
        Vector3[] L3_A = new Vector3[2];
        L3_A[0] = pointLineWheelCableA3.position;
        L3_A[1] = pointLineWheelCableA2.position;
        lineWheelCableA3.positionCount = L3_A.Length;
        lineWheelCableA3.SetPositions(L3_A);
        lineWheelCableA3.material = lineMaterial;
        lineWheelCableA3.startWidth = cableStart;
        lineWheelCableA3.endWidth = cableEnd;
        Vector3[] C1 = new Vector3[2];
        C1[0] = pointLineWheelCableC1.position;
        C1[1] = pointLineWheelCableC2.position;
        lineWheelCableC1.positionCount = C1.Length;
        lineWheelCableC1.SetPositions(C1);
        lineWheelCableC1.material = lineMaterial;
        lineWheelCableC1.startWidth = cableStart;
        lineWheelCableC1.endWidth = cableEnd;
        Vector3[] L1 = new Vector3[2];
        L1[0] = pointLineWheelCableL1.position;
        L1[1] = pointLineWheelCableL2.position;
        lineWheelCableL1.positionCount = L1.Length;
        lineWheelCableL1.SetPositions(L1);
        lineWheelCableL1.material = lineMaterial;
        lineWheelCableL1.startWidth = cableStart;
        lineWheelCableL1.endWidth = cableEnd;
        Vector3[] N1 = new Vector3[2];
        N1[0] = pointLineWheelCableN1.position;
        N1[1] = pointLineWheelCableN2.position;
        lineWheelCableN1.positionCount = N1.Length;
        lineWheelCableN1.SetPositions(N1);
        lineWheelCableN1.material = lineMaterial;
        lineWheelCableN1.startWidth = cableStart;
        lineWheelCableN1.endWidth = cableEnd;
        Vector3[] M1 = new Vector3[2];
        M1[0] = pointLineWheelCableM1.position;
        M1[1] = pointLineWheelCableM2.position;
        lineWheelCableM1.positionCount = M1.Length;
        lineWheelCableM1.SetPositions(M1);
        lineWheelCableM1.material = lineMaterial;
        lineWheelCableM1.startWidth = cableStart;
        lineWheelCableM1.endWidth = cableEnd;
        Vector3[] F1 = new Vector3[2];
        F1[0] = pointLineWheelCableF1.position;
        F1[1] = pointLineWheelCableF2.position;
        lineWheelCableF1.positionCount = F1.Length;
        lineWheelCableF1.SetPositions(F1);
        lineWheelCableF1.material = lineMaterial;
        lineWheelCableF1.startWidth = cableStart;
        lineWheelCableF1.endWidth = cableEnd;
            Vector3[] I1 = new Vector3[2];
        I1[0] = pointLineWheelCableI1.position;
        I1[1] = pointLineWheelCableI2.position;
            lineWheelCableI1.positionCount = I1.Length;
            lineWheelCableI1.SetPositions(I1);
            lineWheelCableI1.material = lineMaterial;
            lineWheelCableI1.startWidth = cableStart;
            lineWheelCableI1.endWidth = cableEnd;
            Vector3[] D1 = new Vector3[2];
        D1[0] = pointLineWheelCableD1.position;
        D1[1] = pointLineWheelCableD2.position;
            lineWheelCableD1.positionCount = D1.Length;
            lineWheelCableD1.SetPositions(D1);
            lineWheelCableD1.material = lineMaterial;
            lineWheelCableD1.startWidth = cableStart;
            lineWheelCableD1.endWidth = cableEnd;
            Vector3[] L1_B = new Vector3[2];
        L1_B[0] = pointLineWheelCableB1.position;
        L1_B[1] = pointLineWheelCableB2.position;
            lineWheelCableB1.positionCount = L1_B.Length;
            lineWheelCableB1.SetPositions(L1_B);
            lineWheelCableB1.material = lineMaterial;
            lineWheelCableB1.startWidth = cableStart;
            lineWheelCableB1.endWidth = cableEnd;
            Vector3[] L3_B = new Vector3[2];
        L3_B[0] = pointLineWheelCableB3.position;
        L3_B[1] = pointLineWheelCableB2.position;
            lineWheelCableB3.positionCount = L3_B.Length;
            lineWheelCableB3.SetPositions(L3_B);
            lineWheelCableB3.material = lineMaterial;
            lineWheelCableB3.startWidth = cableStart;
            lineWheelCableB3.endWidth = cableEnd;
        Vector3[] K1 = new Vector3[2];
        K1[0] = pointLineWheelCableK1.position;
        K1[1] = pointLineWheelCableK2.position;
        lineWheelCableK1.positionCount = K1.Length;
        lineWheelCableK1.SetPositions(K1);
        lineWheelCableK1.material = lineMaterial;
        lineWheelCableK1.startWidth = cableStart;
        lineWheelCableK1.endWidth = cableEnd;
        Vector3[] K3 = new Vector3[2];
        K3[0] = pointLineWheelCableK3.position;
        K3[1] = pointLineWheelCableK2.position;
        lineWheelCableK3.positionCount = K3.Length;
        lineWheelCableK3.SetPositions(K3);
        lineWheelCableK3.material = lineMaterial;
        lineWheelCableK3.startWidth = cableStart;
        lineWheelCableK3.endWidth = cableEnd;
        //Dop
            Vector3[] F3D = new Vector3[2];
        F3D[0] = pointLineWheelCableF3.position;
        F3D[1] = pointLineWheelCableF2.position;
            lineWheelCableF3.positionCount = F3D.Length;
            lineWheelCableF3.SetPositions(F3D);
            lineWheelCableF3.material = lineMaterial;
            lineWheelCableF3.startWidth = cableStart;
            lineWheelCableF3.endWidth = cableEnd;
            Vector3[] M3D = new Vector3[2];
        M3D[0] = pointLineWheelCableM3.position;
        M3D[1] = pointLineWheelCableM2.position;
            lineWheelCableM3.positionCount = M3D.Length;
            lineWheelCableM3.SetPositions(M3D);
            lineWheelCableM3.material = lineMaterial;
            lineWheelCableM3.startWidth = cableStart;
            lineWheelCableM3.endWidth = cableEnd;
            Vector3[] N3D = new Vector3[2];
        N3D[0] = pointLineWheelCableN3.position;
        N3D[1] = pointLineWheelCableN2.position;
            lineWheelCableN3.positionCount = N3D.Length;
            lineWheelCableN3.SetPositions(N3D);
            lineWheelCableN3.material = lineMaterial;
            lineWheelCableN3.startWidth = cableStart;
            lineWheelCableN3.endWidth = cableEnd;
            Vector3[] L3D = new Vector3[2];
        L3D[0] = pointLineWheelCableL3.position;
        L3D[1] = pointLineWheelCableL2.position;
            lineWheelCableL3.positionCount = L3D.Length;
            lineWheelCableL3.SetPositions(L3D);
            lineWheelCableL3.material = lineMaterial;
            lineWheelCableL3.startWidth = cableStart;
            lineWheelCableL3.endWidth = cableEnd;
    }
    public void OnCable_A()
    {
        lineWheelCableK1.enabled = true;
        lineWheelCableK3.enabled = true;
        lineWheelCableF3.enabled = true;
        lineWheelCableN3.enabled = true;
        lineWheelCableM3.enabled = true;
        lineWheelCableL3.enabled = true;
        lineWheelCableB1.enabled = true;
        lineWheelCableB3.enabled = true;
    }
    public void OnCable_B()
    {
            lineWheelCableC1.enabled = false;
            lineWheelCableB1.enabled = false;
            lineWheelCableB3.enabled = false;
            lineWheelCableD1.enabled = false;
            lineWheelCableI1.enabled = false;
            lineWheelCableF1.enabled = false;
            lineWheelCableN1.enabled = false;
            lineWheelCableM1.enabled = false;
            lineWheelCableL1.enabled = false;
        lineWheelCableA1.enabled = false;
        lineWheelCableA3.enabled = false;
    }
    public void OnCable_C()
    {
        lineWheelCableK1.enabled = false;
        lineWheelCableK3.enabled = false;
        lineWheelCableF3.enabled = false;
        lineWheelCableN3.enabled = false;
        lineWheelCableM3.enabled = false;
        lineWheelCableL3.enabled = false;
        lineWheelCableB1.enabled = false;
        lineWheelCableB3.enabled = false;
    }
    public void OnCable_D()
    {
        lineWheelCableC1.enabled = true;
        lineWheelCableB1.enabled = true;
        lineWheelCableB3.enabled = true;
        lineWheelCableD1.enabled = true;
        lineWheelCableI1.enabled = true;
        lineWheelCableF1.enabled = true;
        lineWheelCableN1.enabled = true;
        lineWheelCableM1.enabled = true;
        lineWheelCableL1.enabled = true;
        lineWheelCableA1.enabled = true;
        lineWheelCableA3.enabled = true;
    }
}
