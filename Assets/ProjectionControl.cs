using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * A
 \\ K=R_{ far }Z_{ near }-R_{ near }Z_{ far }\\ \begin{pmatrix} a & 0 & 0 & 0 \\ 0 & b & 0 & 0 \\ 0 & 0 & c & d \\ 0 & 0 & e & K \end{pmatrix}\begin{pmatrix} P_{ x } \\ P_{ y } \\ P_{ z } \\ 1 \end{pmatrix}=\begin{pmatrix} aP_{ x } \\ bP_{ y } \\ cP_{ z }+d \\ eP_{ z }+K \end{pmatrix}\\ H_{ x }=\frac { aP_{ x } }{ eP_{ z }+K } \\ H_{ y }=\frac { bP_{ y } }{ eP_{ z }+K } \\ H_{ z }=\frac { cP_{ z }+d }{ eP_{ z }+K } \\ \begin{pmatrix} \bigstar R_{ near }\frac { w }{ h }  \\ \bigstar R_{ near } \\ -Z_{ near } \end{pmatrix}\rightarrow \begin{pmatrix} 1 \\ 1 \\ -1 \end{pmatrix}\\ \frac { aR_{ near }\frac { w }{ h }  }{ -eZ_{ near }+K } =1,\quad \frac { bR_{ near } }{ -eZ_{ near }+K } =1\\ aR_{ near }\frac { w }{ h } =-eZ_{ near }+K,\quad bR_{ near }=-eZ_{ near }+K\\ aR_{ near }\frac { w }{ h } -bR_{ near }=0\\ R_{ near }\left( a\frac { w }{ h } -b \right) =0\\ a\frac { w }{ h } =b\\ a=\frac { h }{ w } b
    
 * B
K=R_{ far }Z_{ near }-R_{ near }Z_{ far }\\ \begin{pmatrix} a & 0 & 0 & 0 \\ 0 & b & 0 & 0 \\ 0 & 0 & c & d \\ 0 & 0 & e & K \end{pmatrix}\begin{pmatrix} P_{ x } \\ P_{ y } \\ P_{ z } \\ 1 \end{pmatrix}=\begin{pmatrix} aP_{ x } \\ bP_{ y } \\ cP_{ z }+d \\ eP_{ z }+K \end{pmatrix}\\ H_{ x }=\frac { aP_{ x } }{ eP_{ z }+K } \\ H_{ y }=\frac { bP_{ y } }{ eP_{ z }+K } \\ H_{ z }=\frac { cP_{ z }+d }{ eP_{ z }+K } \\ \begin{pmatrix} \bigstar R_{ near }\frac { w }{ h }  \\ R_{ near } \\ -Z_{ near } \end{pmatrix}\rightarrow \begin{pmatrix} 1 \\ 1 \\ -1 \end{pmatrix},\begin{pmatrix} \bigstar R_{ far }\frac { w }{ h }  \\ R_{ far } \\ -Z_{ far } \end{pmatrix}\rightarrow \begin{pmatrix} 1 \\ 1 \\ 1 \end{pmatrix}\\ \frac { \frac { h }{ w } bR_{ near }\frac { w }{ h }  }{ -eZ_{ near }+K } =1,\frac { \frac { h }{ w } bR_{ far }\frac { w }{ h }  }{ -eZ_{ far }+K } =1\\ bR_{ near }=-eZ_{ near }+K,\quad bR_{ far }=-eZ_{ far }+K\\ bR_{ far }R_{ near }=-eR_{ far }Z_{ near }+R_{ far }K,\quad bR_{ far }R_{ near }=-eR_{ near }Z_{ far }+R_{ near }K\\ 0=-eR_{ far }Z_{ near }+R_{ far }K+\quad eR_{ near }Z_{ far }-R_{ near }K\\ 0=e\left( -R_{ far }Z_{ near }+R_{ near }Z_{ far } \right) +R_{ far }K-R_{ near }K\\ e\left( R_{ far }Z_{ near }-R_{ near }Z_{ far } \right) =R_{ far }K-R_{ near }K\\ e=\frac { R_{ far }K-R_{ near }K }{ R_{ far }Z_{ near }-R_{ near }Z_{ far } } \\ e=\frac { K\left( R_{ far }-R_{ near } \right)  }{ R_{ far }Z_{ near }-R_{ near }Z_{ far } } \\ e=R_{ far }-R_{ near }\\ \\ bR_{ far }=-eZ_{ far }+K\\ b=\frac { -eZ_{ far }+K }{ R_{ far } } \\ 
 
 * C
K=R_{ far }Z_{ near }-R_{ near }Z_{ far }\\ \begin{pmatrix} a & 0 & 0 & 0 \\ 0 & b & 0 & 0 \\ 0 & 0 & c & d \\ 0 & 0 & e & K \end{pmatrix}\begin{pmatrix} P_{ x } \\ P_{ y } \\ P_{ z } \\ 1 \end{pmatrix}=\begin{pmatrix} aP_{ x } \\ bP_{ y } \\ cP_{ z }+d \\ eP_{ z }+K \end{pmatrix}\\ H_{ z }=\frac { cP_{ z }+d }{ eP_{ z }+K } \\ -Z_{ near }\rightarrow -1,\quad -Z_{ far }\rightarrow 1\\ \\ \frac { -cZ_{ near }+d }{ -eZ_{ near }+K } =-1,\frac { -cZ_{ far }+d }{ -eZ_{ far }+K } =1\\ -cZ_{ near }+d=-\left( -eZ_{ near }+K \right) ,\quad -cZ_{ far }+d=\left( -eZ_{ far }+K \right) \\ c\left( -Z_{ near }+Z_{ far } \right) =-\left( -eZ_{ near }+K \right) -\left( -eZ_{ far }+K \right) \\ c\left( -Z_{ near }+Z_{ far } \right) =eZ_{ near }-K+eZ_{ far }-K\\ c\left( -Z_{ near }+Z_{ far } \right) =eZ_{ near }+eZ_{ far }-2K\\ c=\frac { eZ_{ near }+eZ_{ far }-2K }{ \left( Z_{ far }-Z_{ near } \right)  } \\ c=\frac { e\left( Z_{ near }+Z_{ far } \right) -2K }{ \left( Z_{ far }-Z_{ near } \right)  } \\ \\ -cZ_{ near }+d=-\left( -eZ_{ near }+K \right) \\ -cZ_{ near }+d=eZ_{ near }-K\\ d=eZ_{ near }-K+cZ_{ near }\\ \\ 
     */
[ExecuteInEditMode]
public class ProjectionControl : MonoBehaviour
{
    [Range(-1.0f, 1.0f)]
    public float _Perspective = 1.0f;

    // Update is called once per frame
    static float Mix(float a, float b, float s)
    {
        return a + (b - a) * s;
    }
    void Update()
    {
        Camera camera = GetComponent<Camera>();
        
        float zn = camera.nearClipPlane;
        float zf = camera.farClipPlane;
        
        float tanFovy = Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * 0.5f);

        float Rn_presp = zn * tanFovy;
        float Rf_presp = zf * tanFovy;

        float zOrtho = this.transform.position.magnitude;
        float R_ortho = zOrtho * tanFovy;
        float Rn = Mix(R_ortho, Rn_presp, _Perspective);
        float Rf = Mix(R_ortho, Rf_presp, _Perspective);

        // avoid singular
        const float eps_Rf = 0.1f;
        if (Rf < eps_Rf)
        {
            // Rn + a * z == eps
            // z == (eps - Rn) / a
            float t = (Rf - Rn) / (zf - zn);
            float z = (eps_Rf - Rn) / t;
            zf = z;
            Rf = Rn + t * z;
        }

        float width = (float)camera.pixelWidth;
        float height = (float)camera.pixelHeight;

        float K = Rf * zn - Rn * zf;
        float e = (Rf - Rn);
        float b = (-e * zf + K) / Rf;
        float a = height / width * b;
        float c = (e * (zn + zf) - 2.0f * K) / (zf - zn);
        float d = e * zn - K + c * zn;

        a = -a;
        b = -b;
        c = -c;
        d = -d;
        e = -e;
        K = -K;

        Matrix4x4 m = new Matrix4x4();
        m.m00 = a;
        m.m11 = b;
        m.m22 = c;
        m.m33 = K;

        m.m23 = d;
        m.m32 = e;

        camera.projectionMatrix = m;
    }
}
